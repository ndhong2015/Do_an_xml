using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Net;

public class XL_UNG_DUNG
{
    //==================== Khởi động ==============
    static XL_UNG_DUNG Ung_dung = null;
    public bool Khoi_dong_Co_loi = false;
    XmlElement Du_lieu_Ung_dung;
    XmlElement Cua_hang;
    List<XmlElement> Danh_sach_Nhom_San_pham = new List<XmlElement>();
    List<XmlElement> Danh_sach_San_pham = new List<XmlElement>();
    List<XmlElement> Danh_sach_Phieu_dat = new List<XmlElement>();
    List<XmlElement> Danh_sach_Nguoi_dung = new List<XmlElement>();

    public static XL_UNG_DUNG Khoi_dong_Ung_dung()
    {
        if (Ung_dung == null)
        {
            Ung_dung = new XL_UNG_DUNG();
            Ung_dung.Du_lieu_Ung_dung = XL_LUU_TRU.Doc_Du_lieu();
            if (Ung_dung.Du_lieu_Ung_dung.GetAttribute("Kq") == "OK")
                Ung_dung.Khoi_dong_Du_lieu_Ung_dung();
            else
                Ung_dung.Khoi_dong_Co_loi = true;
        }

        return Ung_dung;
    }

    void Khoi_dong_Du_lieu_Ung_dung()
    {
        Cua_hang = (XmlElement)Du_lieu_Ung_dung.GetElementsByTagName("Cua_hang")[0];
        var DS_Nhom_San_pham = (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nhom_San_pham")[0];
        Danh_sach_Nhom_San_pham = XL_NGHIEP_VU.Tao_Danh_sach(DS_Nhom_San_pham, "Nhom_San_pham");
        var DS_Nguoi_dung = (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nguoi_dung")[0];
        Danh_sach_Nguoi_dung = XL_NGHIEP_VU.Tao_Danh_sach(DS_Nguoi_dung, "Nguoi_dung");
        var DS_San_pham = (XmlElement)Du_lieu_Ung_dung.GetElementsByTagName("Danh_sach_San_pham")[0];
        Danh_sach_San_pham = XL_NGHIEP_VU.Tao_Danh_sach(DS_San_pham, "San_pham");
        var DS_Phieu_dat = (XmlElement)Du_lieu_Ung_dung.GetElementsByTagName("Danh_sach_Phieu_dat")[0];
        Danh_sach_Phieu_dat = XL_NGHIEP_VU.Tao_Danh_sach(DS_Phieu_dat, "PHIEU_DAT");
    }
    //============= Xử lý Chức năng ========
    public XL_NGUOI_DUNG_DANG_NHAP Dang_nhap(string Ten_Dang_nhap, string Mat_khau)
    {
        var Nguoi_dung_Dang_nhap = (XL_NGUOI_DUNG_DANG_NHAP)null;
        var Danh_sach_Quan_ly_Ban_hang = Danh_sach_Nguoi_dung.FindAll(x => x.SelectSingleNode("Nhom_Nguoi_dung/@Ma_so").Value == "QUAN_LY_BAN_HANG");
        var Danh_sach_Nhan_vien_Ban_hang = Danh_sach_Nguoi_dung.FindAll(x => x.SelectSingleNode("Nhom_Nguoi_dung/@Ma_so").Value == "BAN_HANG");
        var Nguoi_dung = Danh_sach_Quan_ly_Ban_hang.FirstOrDefault(
            x => x.GetAttribute("Ten_Dang_nhap") == Ten_Dang_nhap && x.GetAttribute("Mat_khau") == Mat_khau);
        if (Nguoi_dung != null)
        {

            // Thống tin Online 
            Nguoi_dung_Dang_nhap = new XL_NGUOI_DUNG_DANG_NHAP();
            Nguoi_dung_Dang_nhap.Ho_ten = Nguoi_dung.GetAttribute("Ho_ten");
            Nguoi_dung_Dang_nhap.Ma_so = Nguoi_dung.GetAttribute("Ma_so");
            Nguoi_dung_Dang_nhap.Cua_hang = Cua_hang;
            Nguoi_dung_Dang_nhap.Danh_sach_San_pham = Danh_sach_San_pham;
            Nguoi_dung_Dang_nhap.Danh_sach_Nhom_San_pham = Danh_sach_Nhom_San_pham;
            Nguoi_dung_Dang_nhap.Danh_sach_Phieu_dat = Danh_sach_Phieu_dat;
            Nguoi_dung_Dang_nhap.Danh_sach_Nhan_vien_Ban_hang = Danh_sach_Nhan_vien_Ban_hang;
            Nguoi_dung_Dang_nhap.Danh_sach_San_pham_Xem = Nguoi_dung_Dang_nhap.Danh_sach_San_pham;
            //Bổ xung thông tin cho Nhân viên Bán hàng
            Danh_sach_Nhan_vien_Ban_hang.ForEach(Nhan_vien =>
            {
                var DS_Nhom_San_pham_Nhan_vien = (XmlElement)Nhan_vien.GetElementsByTagName("Danh_sach_Nhom_San_pham")[0];
                var Danh_sach_Nhom_San_pham_Nhan_vien = XL_NGHIEP_VU.Tao_Danh_sach(DS_Nhom_San_pham_Nhan_vien, "Nhom_San_pham");
                var Doanh_thu = Danh_sach_Nhom_San_pham_Nhan_vien.Sum(x => long.Parse(x.GetAttribute("Doanh_thu")));
                Nhan_vien.SetAttribute("Doanh_thu", Doanh_thu.ToString());
            });

            HttpContext.Current.Session["Nguoi_dung_Dang_nhap"] = Nguoi_dung_Dang_nhap;
        }

        return Nguoi_dung_Dang_nhap;
    }

    // Chức năng Xem
    public string Khoi_dong()
    {
        var Chuoi_HTML = Tao_Chuoi_HTML_Ket_qua();
        return Chuoi_HTML;
    }
    public string Chon_Nhom_San_pham(string Ma_so_San_pham)
    {
        var Nguoi_dung_Dang_nhap = (XL_NGUOI_DUNG_DANG_NHAP)HttpContext.Current.Session["Nguoi_dung_Dang_nhap"];
        Nguoi_dung_Dang_nhap.Danh_sach_San_pham_Xem = XL_NGHIEP_VU.Tra_cuu_San_pham(
           Ma_so_San_pham, Nguoi_dung_Dang_nhap.Danh_sach_San_pham);
        var Chuoi_HTML = Tao_Chuoi_HTML_Ket_qua();
        return Chuoi_HTML;
    }
    public string Tra_cuu(string Chuoi_Tra_cuu)
    {
        var Nguoi_dung_Dang_nhap = (XL_NGUOI_DUNG_DANG_NHAP)HttpContext.Current.Session["Nguoi_dung_Dang_nhap"];
        Nguoi_dung_Dang_nhap.Danh_sach_San_pham_Xem = XL_NGHIEP_VU.Tra_cuu_San_pham(
            Chuoi_Tra_cuu, Nguoi_dung_Dang_nhap.Danh_sach_San_pham);

        var Chuoi_HTML = Tao_Chuoi_HTML_Ket_qua();
        return Chuoi_HTML;
    }
    public string Xem_Phieu_dat()
    {
        var Nguoi_dung_Dang_nhap = (XL_NGUOI_DUNG_DANG_NHAP)HttpContext.Current.Session["Nguoi_dung_Dang_nhap"];
        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Phieu_dat(Nguoi_dung_Dang_nhap.Danh_sach_Phieu_dat)}" +
             $"</div>";
        return Chuoi_HTML;
    }
    public string Tao_Chuoi_HTML_Ket_qua()
    {
        var Nguoi_dung_Dang_nhap = (XL_NGUOI_DUNG_DANG_NHAP)HttpContext.Current.Session["Nguoi_dung_Dang_nhap"];

        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Nguoi_dung(Nguoi_dung_Dang_nhap)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nhan_vien_Ban_hang(Nguoi_dung_Dang_nhap.Danh_sach_Nhan_vien_Ban_hang, Nguoi_dung_Dang_nhap.Danh_sach_San_pham_Xem, Nguoi_dung_Dang_nhap.Danh_sach_Nhom_San_pham)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Thong_bao(Nguoi_dung_Dang_nhap.Thong_bao)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_San_pham_Xem(Nguoi_dung_Dang_nhap.Danh_sach_San_pham_Xem)}" +
             $"</div>";
        return Chuoi_HTML;

    }

    // Chức năng Ghi
    public string Cap_nhat_Don_gia_Ban(string Ma_so_San_pham, long Don_gia_Ban)
    {
        var Nguoi_dung_Dang_nhap = (XL_NGUOI_DUNG_DANG_NHAP)HttpContext.Current.Session["Nguoi_dung_Dang_nhap"];
        var San_pham = Danh_sach_San_pham.FirstOrDefault(x => x.GetAttribute("Ma_so") == Ma_so_San_pham);

        var Hop_le = San_pham != null;
        if (Hop_le)
        {
            Nguoi_dung_Dang_nhap.Danh_sach_San_pham_Xem = new List<XmlElement>();
            Nguoi_dung_Dang_nhap.Danh_sach_San_pham_Xem.Add(San_pham);

            var Kq_Ghi = XL_LUU_TRU.Cap_nhat_Don_gia_Ban(San_pham, Don_gia_Ban);
            if (Kq_Ghi == "OK")
                Nguoi_dung_Dang_nhap.Thong_bao = $"Đơn giá Bán mới là: {Don_gia_Ban.ToString("c0", XL_THE_HIEN.Dinh_dang_VN)}";
            else
                Nguoi_dung_Dang_nhap.Thong_bao = $"Lỗi Hệ thống - Xin Thực hiện lại";
        }
        else
            Nguoi_dung_Dang_nhap.Thong_bao = $"Lỗi Hệ thống - Xin Thực hiện lại";

        var Chuoi_HTML = Tao_Chuoi_HTML_Ket_qua();
        return Chuoi_HTML;

    }

}
