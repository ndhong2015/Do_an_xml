using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Globalization;
using System.Net;

public class XL_UNG_DUNG
{
    //==================== Khởi động ==============
    static XL_UNG_DUNG Ung_dung = null;
    XmlElement Du_lieu_Ung_dung;
    XmlElement Cua_hang;
    List<XmlElement> Danh_sach_Nhom_San_pham = new List<XmlElement>();
    List<XmlElement> Danh_sach_San_pham = new List<XmlElement>();
    List<XmlElement> Danh_sach_Nguoi_dung = new List<XmlElement>();
    List<XmlElement> Danh_sanh_Phieu_dat = new List<XmlElement>();


    public static XL_UNG_DUNG Khoi_dong_Ung_dung()
    {
        if (Ung_dung == null)
        {
            Ung_dung = new XL_UNG_DUNG();
            Ung_dung.Khoi_dong_Du_lieu_Ung_dung();
        }
        return Ung_dung;
    }
    void Khoi_dong_Du_lieu_Ung_dung()
    {
        Du_lieu_Ung_dung = XL_LUU_TRU.Doc_Du_lieu();
        Cua_hang = (XmlElement)Du_lieu_Ung_dung.GetElementsByTagName("Cua_hang")[0];
        var DS_Nhom_San_pham = (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nhom_San_pham")[0];
        Danh_sach_Nhom_San_pham = XL_NGHIEP_VU.Tao_Danh_sach(DS_Nhom_San_pham, "Nhom_San_pham");
        var DS_San_pham = (XmlElement)Du_lieu_Ung_dung.GetElementsByTagName("Danh_sach_San_pham")[0];
        Danh_sach_San_pham = XL_NGHIEP_VU.Tao_Danh_sach(DS_San_pham, "San_pham");

        var DS_Phieu_dat = (XmlElement)Du_lieu_Ung_dung.GetElementsByTagName("Danh_sach_Phieu_dat")[0];
        Danh_sanh_Phieu_dat = XL_NGHIEP_VU.Tao_Danh_sach(DS_Phieu_dat, "PHIEU_DAT");

        var DS_Nguoi_dung = (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nguoi_dung")[0];
        Danh_sach_Nguoi_dung = XL_NGHIEP_VU.Tao_Danh_sach(DS_Nguoi_dung, "Nguoi_dung");
    }
    //============= Xử lý Chức năng ========
    public string Dang_nhap(string Ten_Dang_nhap, string Mat_khau)
    {

        var Nguoi_dung = Danh_sach_Nguoi_dung.FirstOrDefault(
            x => x.GetAttribute("Ten_Dang_nhap") == Ten_Dang_nhap && x.GetAttribute("Mat_khau") == Mat_khau);
        var Chuoi_HTML = "";
        if (Nguoi_dung != null)
        {
            var Dia_chi_MH_Dang_nhap = Nguoi_dung.SelectSingleNode("Nhom_Nguoi_dung/@Dia_chi_MH_Dang_nhap").Value;
            var Tham_so = $"Th_Ma_so_Chuc_nang=DANG_NHAP&Th_Ten_Dang_nhap={Ten_Dang_nhap}&Th_Mat_khau={Mat_khau}";
            var Dia_chi_Xu_ly = $"{Dia_chi_MH_Dang_nhap}?{Tham_so}";
            HttpContext.Current.Response.Redirect(Dia_chi_Xu_ly);
        }

        else Chuoi_HTML = "Không hợp lệ";
        return Chuoi_HTML;
    }
    public string Khoi_dong_Nguoi_dung()
    {
        var Khach_Tham_quan = new XL_KHACH_THAM_QUAN();
        Khach_Tham_quan.Danh_sach_San_pham = Danh_sach_San_pham;
        Khach_Tham_quan.Danh_sach_Nhom_San_pham = Danh_sach_Nhom_San_pham;
        Khach_Tham_quan.Danh_sach_San_pham_Xem = Danh_sach_San_pham;
        Khach_Tham_quan.Danh_sach_San_pham_Chon = new List<XmlElement>();
        Khach_Tham_quan.Danh_sach_Phieu_dat = Danh_sanh_Phieu_dat;
        HttpContext.Current.Session["Khach_Tham_quan"] = Khach_Tham_quan;

        var Chuoi_HTML = Tao_Chuoi_HTML_Ket_qua();
        return Chuoi_HTML;
    }
    public string Chon_Nhom_San_pham(string Ma_so_Nhom_San_pham)
    {

        var Danh_sach_San_pham_Xem = XL_NGHIEP_VU.Tra_cuu_San_pham(Ma_so_Nhom_San_pham, Danh_sach_San_pham);
        var Danh_sach_Nhom_San_pham_Xem = Danh_sach_Nhom_San_pham;
        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nhom_San_pham_Xem(Danh_sach_Nhom_San_pham_Xem)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_San_pham_Xem(Danh_sach_San_pham_Xem)}" +
             $"</div>";
        return Chuoi_HTML;
    }
    public string Chon_San_pham(string Ma_so_San_pham)
    {
        var Khach_Tham_quan = (XL_KHACH_THAM_QUAN)HttpContext.Current.Session["Khach_Tham_quan"];
        // Xử lý 
        var San_pham = Khach_Tham_quan.Danh_sach_San_pham.FirstOrDefault(x => x.GetAttribute("Ma_so") == Ma_so_San_pham);

        if (!Khach_Tham_quan.Danh_sach_San_pham_Chon.Contains(San_pham))
        {
            San_pham.SetAttribute("So_luong", "1");
            Khach_Tham_quan.Danh_sach_San_pham_Chon.Add(San_pham);
        }
        else
        {
            var So_luong_Ton = int.Parse(San_pham.GetAttribute("So_luong_Ton"));
            var So_luong = int.Parse(San_pham.GetAttribute("So_luong"));
            if (So_luong < So_luong_Ton)
            {
                So_luong += 1;
                San_pham.SetAttribute("So_luong", So_luong.ToString());
            }

        }
        // Tạo chuỗi HTML kết quả xem 
        var Chuoi_HTML = Tao_Chuoi_HTML_Ket_qua();
        return Chuoi_HTML;
    }
    public string Giam_So_luong_San_pham(string Ma_so_San_pham)
    {
        var Khach_Tham_quan = (XL_KHACH_THAM_QUAN)HttpContext.Current.Session["Khach_Tham_quan"];
        // Xử lý 
        var San_pham = Khach_Tham_quan.Danh_sach_San_pham.FirstOrDefault(x => x.GetAttribute("Ma_so") == Ma_so_San_pham);
        var So_luong = int.Parse(San_pham.GetAttribute("So_luong"));
        So_luong -= 1;
        San_pham.SetAttribute("So_luong", So_luong.ToString());
        if (So_luong == 0)
            Khach_Tham_quan.Danh_sach_San_pham_Chon.Remove(San_pham);
        // Tạo chuỗi HTML kết quả xem 
        var Chuoi_HTML = Tao_Chuoi_HTML_Ket_qua();
        return Chuoi_HTML;
    }
    public string Dat_hang()
    {
        var Khach_Tham_quan = (XL_KHACH_THAM_QUAN)HttpContext.Current.Session["Khach_Tham_quan"];
        // Xử lý 

        // Tạo chuỗi HTML kết quả xem 
        var Chuoi_HTML = "<iframe src='MH_Dat_hang.cshtml' style='width: 100%; height: 500px'  ></iframe>";
        return Chuoi_HTML;
    }
    public string Tra_cuu(string Chuoi_Tra_cuu)
    {
        var Danh_sach_San_pham_Xem = XL_NGHIEP_VU.Tra_cuu_San_pham(Chuoi_Tra_cuu, Danh_sach_San_pham);
        var Danh_sach_Nhom_San_pham_Xem = Danh_sach_Nhom_San_pham;
        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nhom_San_pham_Xem(Danh_sach_Nhom_San_pham_Xem)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_San_pham_Xem(Danh_sach_San_pham_Xem)}" +
             $"</div>";
        return Chuoi_HTML;
    }
    public string Tao_Chuoi_HTML_Dat_hang()
    {
        var Khach_Tham_quan = (XL_KHACH_THAM_QUAN)HttpContext.Current.Session["Khach_Tham_quan"];

        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Thong_bao(Khach_Tham_quan.Thong_bao)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Dat_hang(Khach_Tham_quan.Danh_sach_San_pham_Chon)}" +
             $"</div>";
        return Chuoi_HTML;

    }
    public string Tao_Chuoi_HTML_Ket_qua()
    {
        var Khach_Tham_quan = (XL_KHACH_THAM_QUAN)HttpContext.Current.Session["Khach_Tham_quan"];

        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Thong_bao(Khach_Tham_quan.Thong_bao)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_San_pham_Chon(Khach_Tham_quan.Danh_sach_San_pham_Chon)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nhom_San_pham_Xem(Khach_Tham_quan.Danh_sach_Nhom_San_pham)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_San_pham_Xem(Khach_Tham_quan.Danh_sach_San_pham_Xem)}" +
             $"</div>";
        return Chuoi_HTML;

    }
    public string Ten_Cua_hang()
    {
        return Cua_hang.GetAttribute("Ten");
    }
    //2222222 Chức năng Ghi 222222222222222
    public string Ghi_Phieu_Dat_moi(List<XmlElement> Danh_sach, string Ho_ten, string Dien_thoai, string Dia_chi)
    {
        var Khach_Tham_quan = (XL_KHACH_THAM_QUAN)HttpContext.Current.Session["Khach_Tham_quan"];
        var Danh_sach_Ma_so_Phieu_dat = new List<string>();
        Danh_sanh_Phieu_dat.ForEach(Phieu_dat =>
        Danh_sach_Ma_so_Phieu_dat.Add(Phieu_dat.GetAttribute("Ma_so")));
        var Ma_so_Phieu_dat_moi = "";
        for (var i = 0; i <= Danh_sach_Ma_so_Phieu_dat.Count(); i++)
        {
            var Chuoi_Ma_so_dat_moi = "P_" + i;
            if (!Danh_sach_Ma_so_Phieu_dat.Contains(Chuoi_Ma_so_dat_moi))
            {
                Ma_so_Phieu_dat_moi = Chuoi_Ma_so_dat_moi;
                break;
            }
        }
        var Chuoi_XML = "<PHIEU_DAT/>";
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Phieu_dat_moi = Tai_lieu.DocumentElement;
        Phieu_dat_moi.SetAttribute("Ma_so", Ma_so_Phieu_dat_moi.ToString());
        Phieu_dat_moi.SetAttribute("Ngay", DateTime.Now.ToString());
        Phieu_dat_moi.SetAttribute("Trang_thai", "CHO_PHAN_CONG");
        Chuoi_XML = "<Khach_hang/>";
        Tai_lieu.LoadXml(Chuoi_XML);
        var Khach_hang = Tai_lieu.DocumentElement;
        Khach_hang.SetAttribute("Ho_ten", Ho_ten);
        Khach_hang.SetAttribute("Dien_thoai", Dien_thoai);
        Khach_hang.SetAttribute("Dia_chi", Dia_chi);
        Phieu_dat_moi.AppendChild(Khach_hang);

        Chuoi_XML = "<Danh_sach_San_pham/>";
        Tai_lieu.LoadXml(Chuoi_XML);
        var Danh_sach_San_pham = Tai_lieu.DocumentElement;
        Phieu_dat_moi.AppendChild(Danh_sach_San_pham);
        Danh_sach.ForEach(San_pham =>
        {
            Chuoi_XML = "<San_pham/>";
            Tai_lieu.LoadXml(Chuoi_XML);
            var San_pham_chon = Tai_lieu.DocumentElement;
            San_pham_chon.SetAttribute("Ma_so", San_pham.GetAttribute("Ma_so"));
            San_pham_chon.SetAttribute("Ten", San_pham.GetAttribute("Ten"));
            San_pham_chon.SetAttribute("Don_gia", San_pham.GetAttribute("Don_gia_Ban"));
            San_pham_chon.SetAttribute("So_luong", San_pham.GetAttribute("So_luong"));
            var Tien = (long.Parse(San_pham.GetAttribute("Don_gia_Ban"))) * (int.Parse(San_pham.GetAttribute("So_luong")));
            San_pham_chon.SetAttribute("Tien", Tien.ToString());
            Danh_sach_San_pham.AppendChild(San_pham_chon);
        }
        );
        Chuoi_XML = "<Nhan_vien_Giao_hang/>";
        Tai_lieu.LoadXml(Chuoi_XML);
        var Nhan_vien_Giao_hang = Tai_lieu.DocumentElement;
        Phieu_dat_moi.AppendChild(Nhan_vien_Giao_hang);
        Nhan_vien_Giao_hang.SetAttribute("Ma_so", "");

        var Hop_le = Phieu_dat_moi != null;
        if (Hop_le)
        {
            var Kq_Ghi = XL_LUU_TRU.Ghi_Phieu_Dat_moi(Phieu_dat_moi);
            if (Kq_Ghi == "OK")
                Khach_Tham_quan.Thong_bao = "Đã ghi nhận phiếu đặt hàng";
            else
                Khach_Tham_quan.Thong_bao = "Lỗi Hệ thống - Xin Thực hiện lại  ";

        }
        else
            Khach_Tham_quan.Thong_bao = "Lỗi Hệ thống - Xin Thực hiện lại ";
        var Chuoi_HTML = Tao_Chuoi_HTML_Ket_qua();
        return Chuoi_HTML;

    }
}
