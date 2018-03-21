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
    }

    //============= Xử lý Chức năng ========
    public XL_NGUOI_DUNG_DANG_NHAP Dang_nhap(string Ten_Dang_nhap, string Mat_khau)
    {
        var Nguoi_dung_Dang_nhap = (XL_NGUOI_DUNG_DANG_NHAP)null;
        var Nguoi_dung = Danh_sach_Nguoi_dung.FirstOrDefault(
            x => x.GetAttribute("Ten_Dang_nhap") == Ten_Dang_nhap && x.GetAttribute("Mat_khau") == Mat_khau);
        if (Nguoi_dung != null)
        {
            var DS_Nhom_San_pham = (XmlElement)Nguoi_dung.GetElementsByTagName("Danh_sach_Nhom_San_pham")[0];
            var Danh_sach_Nhom_San_pham_cua_Nguoi_dung = XL_NGHIEP_VU.Tao_Danh_sach(DS_Nhom_San_pham, "Nhom_San_pham");
            var Danh_sach_San_pham_cua_Nguoi_dung = Danh_sach_San_pham.FindAll(
                x => Danh_sach_Nhom_San_pham_cua_Nguoi_dung.Any(Nhom => Nhom.GetAttribute("Ma_so") == x.SelectSingleNode("Nhom_San_pham/@Ma_so").Value));

            // Thống tin Online 
            Nguoi_dung_Dang_nhap = new XL_NGUOI_DUNG_DANG_NHAP();
            Nguoi_dung_Dang_nhap.Ho_ten = Nguoi_dung.GetAttribute("Ho_ten");
            Nguoi_dung_Dang_nhap.Ma_so = Nguoi_dung.GetAttribute("Ma_so");
            Nguoi_dung_Dang_nhap.Cua_hang = Cua_hang;
            Nguoi_dung_Dang_nhap.Danh_sach_San_pham = Danh_sach_San_pham_cua_Nguoi_dung;
            Nguoi_dung_Dang_nhap.Danh_sach_Nhom_San_pham = Danh_sach_Nhom_San_pham_cua_Nguoi_dung;
            Nguoi_dung_Dang_nhap.Danh_sach_San_pham_Xem = Nguoi_dung_Dang_nhap.Danh_sach_San_pham;
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
    public string Tao_Chuoi_HTML_Ket_qua()
    {
        var Nguoi_dung_Dang_nhap = (XL_NGUOI_DUNG_DANG_NHAP)HttpContext.Current.Session["Nguoi_dung_Dang_nhap"];

        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Thong_bao(Nguoi_dung_Dang_nhap.Thong_bao)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nhom_San_pham_Xem(Nguoi_dung_Dang_nhap.Danh_sach_Nhom_San_pham)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_San_pham_Xem(Nguoi_dung_Dang_nhap.Danh_sach_San_pham_Xem)}" +
             $"</div>";
        return Chuoi_HTML;

    }

    // Chức năng Ghi
    public string Ghi_Ban_hang_Moi(string Ma_so_San_pham, int So_luong)
    {
        var Nguoi_dung_Dang_nhap = (XL_NGUOI_DUNG_DANG_NHAP)HttpContext.Current.Session["Nguoi_dung_Dang_nhap"];
        var San_pham = Danh_sach_San_pham.FirstOrDefault(x => x.GetAttribute("Ma_so") == Ma_so_San_pham);


        var Hop_le = San_pham != null;
        if (Hop_le)
        {
            Nguoi_dung_Dang_nhap.Danh_sach_San_pham_Xem = new List<XmlElement>();
            Nguoi_dung_Dang_nhap.Danh_sach_San_pham_Xem.Add(San_pham);

            var Ban_hang = San_pham.OwnerDocument.CreateElement("Ban_hang");
            var Don_gia_Ban = long.Parse(San_pham.GetAttribute("Don_gia_Ban"));
            var Tien = So_luong * Don_gia_Ban;
            Ban_hang.SetAttribute("Ngay", DateTime.Now.ToString());
            Ban_hang.SetAttribute("So_luong", So_luong.ToString());
            Ban_hang.SetAttribute("Don_gia", Don_gia_Ban.ToString());
            Ban_hang.SetAttribute("Tien", Tien.ToString());
            var Kq_Ghi = XL_LUU_TRU.Ghi_Ban_hang_Moi(San_pham, Ban_hang);
            if (Kq_Ghi == "OK")
            {
                Nguoi_dung_Dang_nhap.Thong_bao = "Tiền phải thu là " + Tien.ToString("c0",XL_THE_HIEN.Dinh_dang_VN);
                var Nhom_San_pham = Nguoi_dung_Dang_nhap.Danh_sach_Nhom_San_pham.FirstOrDefault(x => San_pham.SelectSingleNode("Nhom_San_pham/@Ma_so").Value == x.GetAttribute("Ma_so"));
                var So_luong_Ton_Nhom_San_pham = int.Parse(Nhom_San_pham.GetAttribute("So_luong_Ton")) - So_luong;
                Nhom_San_pham.SetAttribute("So_luong_Ton", So_luong_Ton_Nhom_San_pham.ToString());
            }
            else
                Nguoi_dung_Dang_nhap.Thong_bao = "Lỗi Hệ thống - Xin Thực hiện lại  ";

        }
        else
            Nguoi_dung_Dang_nhap.Thong_bao = "Lỗi Hệ thống - Xin Thực hiện lại ";

        var Chuoi_HTML = Tao_Chuoi_HTML_Ket_qua();
        return Chuoi_HTML;

    }


}
//************************* View/Presentation -Layers VL/PL **********************************
public partial class XL_THE_HIEN
{
    public static string Dia_chi_Media = $"{XL_LUU_TRU.Dia_chi_Dich_vu}/Media";
    public static CultureInfo Dinh_dang_VN = CultureInfo.GetCultureInfo("vi-VN");

    public static string Tao_Chuoi_HTML_Thong_bao(string Thong_bao)
    {
        var Chuoi_HTML = $"<div class='alert alert-info'>" +
                          $"{Thong_bao} " +
                          $"</div>";
        return Chuoi_HTML;
    }
    public static string Tao_Chuoi_HTML_Danh_sach_San_pham_Xem(List<XmlElement> Danh_sach_San_pham)
    {
        var Chuoi_HTML_Danh_sach = "<div class='row'>";
        Danh_sach_San_pham.ForEach(San_pham =>
        {
            var Ten = San_pham.GetAttribute("Ten");
            var Ma_so = San_pham.GetAttribute("Ma_so");
            var Don_gia_Ban = long.Parse(San_pham.GetAttribute("Don_gia_Ban"));
            var So_luong_Ton = int.Parse(San_pham.GetAttribute("So_luong_Ton"));
            var Doanh_thu = long.Parse(San_pham.GetAttribute("Doanh_thu"));
            var Dinh_dang_Trang_thai = "";
            if (So_luong_Ton == 0)
            {
                Dinh_dang_Trang_thai = ";opacity:0.7"; ;
            }
            var Chuoi_Hinh = $"<img src='{Dia_chi_Media}/{ Ma_so}.jpg' " +
                             "class='float-left' style='width:25%;height:80%;' />";
            var Chuoi_Chuc_nang_Ban_hang = $"<form method='post'>   " +
                                     $"<input name='Th_Ma_so_Chuc_nang' type='hidden' value='GHI_BAN_HANG_MOI' />  " +
                                    $"<input name='Th_Ma_so_San_pham' type='hidden' value='{Ma_so}' />  " +
                                    $"<input name='Th_So_luong' required='required' autocomplete='off' " +
                                         $"style='border:none;border-bottom:solid 1px blue'" +
                                        $"type='number' min='1'  max='{So_luong_Ton}' value='1' />  " +
                                 $"</form>";
            var Chuoi_Thong_tin = $"<div class='text-left float-left' style='width:65%;height:100%;margin-left:10px;'> " +
                          $"{ Ten}" +
                          $"<br />Đơn giá Bán {Don_gia_Ban.ToString("n0", Dinh_dang_VN)}" +
                          $"<br />Số lượng Tồn {So_luong_Ton.ToString("n0", Dinh_dang_VN)}" +
                          $"<br />Doanh thu {Doanh_thu.ToString("n0", Dinh_dang_VN)}" +
                          $"<br/>{Chuoi_Chuc_nang_Ban_hang}" +
                          $"</div>";
            

            var Chuoi_HTML = $"<div class='float-left' style='width:550px;height:150px;margin-bottom:10px;margin-left:10px;{Dinh_dang_Trang_thai}' >" +
                               $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" +
                               
                             "</div>";
            Chuoi_HTML_Danh_sach += Chuoi_HTML;
        });

        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }
    public static string Tao_Chuoi_HTML_Danh_sach_Nhom_San_pham_Xem(List<XmlElement> Danh_sach_Nhom_San_pham)
    {
        var Chuoi_HTML_Danh_sach = "<div class='btn btn-primary' style='margin:10px'>";
        Danh_sach_Nhom_San_pham.ForEach(Nhom_San_pham =>
        {
            var Ten = Nhom_San_pham.GetAttribute("Ten");
            var Ma_so = Nhom_San_pham.GetAttribute("Ma_so");
            var So_luong_Ton = int.Parse(Nhom_San_pham.GetAttribute("So_luong_Ton"));
            var Chuoi_Chuc_nang_Chon = $"<form method='post'>" +
                                        "<input name='Th_Ma_so_Chuc_nang' type='hidden' value='CHON_NHOM_SAN_PHAM' />" +
                                         $"<input name='Th_Ma_so_Nhom_San_pham' type='hidden' value='{Ma_so}' />" +
                                         $"<button type='submit' class ='btn btn-primary'>{Ten} ({So_luong_Ton})</button>" +
                                       "</form>";
            var Chuoi_Hinh = $"";
            var Chuoi_Thong_tin = $"<div class='' style=''> " +
                          $"{Chuoi_Chuc_nang_Chon}" +
                          $"</div>";
            var Chuoi_HTML = $"<div class='btn ' style=' ' >" +
                               $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" +
                             "</div>";
            Chuoi_HTML_Danh_sach += Chuoi_HTML;
        });

        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }
    
}
//************************* Business-Layers BL **********************************
public partial class XL_NGHIEP_VU
{
    public static List<XmlElement> Tra_cuu_San_pham(string Chuoi_Tra_cuu, List<XmlElement> Danh_sach_San_pham)
    {
        Chuoi_Tra_cuu = Chuoi_Tra_cuu.ToUpper();
        var Danh_sach_Kq = new List<XmlElement>();
        Danh_sach_Kq = Danh_sach_San_pham.FindAll(x => x.GetAttribute("Ten").ToUpper().Contains(Chuoi_Tra_cuu)
                                                || x.GetAttribute("Ma_so").ToUpper() == (Chuoi_Tra_cuu)
                                                || x.SelectSingleNode("Nhom_San_pham/@Ma_so").Value == Chuoi_Tra_cuu);
        return Danh_sach_Kq;
    }
    // Tạo Danh sách ======
    public static List<XmlElement> Tao_Danh_sach(XmlElement Danh_sach_Nguon, string Loai_Doi_tuong)
    {
        var Danh_sach = new List<XmlElement>();
        foreach (XmlElement Doi_tuong in Danh_sach_Nguon.GetElementsByTagName(Loai_Doi_tuong))
        {
            Danh_sach.Add(Doi_tuong);
        }
        return Danh_sach;
    }
    public static List<XmlElement> Tao_Danh_sach_San_pham_cua_Nhom_San_pham(XmlElement Nhom_San_pham, XmlElement Danh_sach_Tat_ca_San_pham)
    {
        var Danh_sach = new List<XmlElement>();
        var DS_Tat_ca_San_pham = Tao_Danh_sach(Danh_sach_Tat_ca_San_pham, "San_pham");
        Danh_sach = DS_Tat_ca_San_pham.FindAll(
               San_pham => San_pham.SelectSingleNode("Nhom_San_pham/@Ma_so").Value == Nhom_San_pham.GetAttribute("Ma_so"));
        return Danh_sach;
    }
    public static List<XmlElement> Tao_Danh_sach_San_pham_cua_Nguoi_dung(XmlElement Nguoi_dung, XmlElement Danh_sach_Tat_ca_San_pham)
    {
        var Danh_sach = new List<XmlElement>();
        var DS_Tat_ca_San_pham = Tao_Danh_sach(Danh_sach_Tat_ca_San_pham, "San_pham");
        Danh_sach = DS_Tat_ca_San_pham.FindAll(
               San_pham => San_pham.SelectSingleNode("Nhom_San_pham/@Ma_so").Value == Nguoi_dung.GetAttribute("Ma_so"));
        return Danh_sach;
    }

}

//************************* Data-Layers DL **********************************
public partial class XL_LUU_TRU
{

    public static string Dia_chi_Dich_vu = "http://localhost:50800";
    static string Dia_chi_Dich_vu_Du_lieu = $"{Dia_chi_Dich_vu}/1-Dich_vu_Giao_tiep/DV_Chinh.cshtml";
    // Đọc
    public static XmlElement Doc_Du_lieu()
    {
        var Chuoi_XML = "<Du_lieu />";
        var Xu_ly = new WebClient();
        Xu_ly.Encoding = System.Text.Encoding.UTF8;
        var Tham_so = "Ma_so_Xu_ly=KHOI_DONG_DU_LIEU_NHAN_VIEN_BAN_HANG";
        var Dia_chi_Xu_ly = $"{Dia_chi_Dich_vu_Du_lieu}?{Tham_so}";
        try
        {
            var Chuoi_Kq = Xu_ly.DownloadString(Dia_chi_Xu_ly);
            if (Chuoi_Kq.Trim() != "")
                Chuoi_XML = Chuoi_Kq;


        }
        catch (Exception Loi)
        {
            Chuoi_XML = $"<Du_lieu Loi='{Loi.Message}'  />";
        }

        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu = Tai_lieu.DocumentElement;
        Du_lieu.SetAttribute("Kq", "OK");
        return Du_lieu;
    }

    // Ghi 
    public static string Ghi_Ban_hang_Moi(XmlElement San_pham, XmlElement Ban_hang)
    {
        var Kq = "OK";

        try
        {
            var Xu_ly = new WebClient();
            Xu_ly.Encoding = System.Text.Encoding.UTF8;
            var Tham_so = $"Ma_so_Xu_ly=GHI_BAN_HANG_MOI&Ma_so_San_pham={San_pham.GetAttribute("Ma_so")}";
            var Dia_chi_Xu_ly = $"{Dia_chi_Dich_vu_Du_lieu}?{Tham_so}";
            var Chuoi_XML_Ban_hang = Ban_hang.OuterXml;
            var Chuoi_XML_Kq = Xu_ly.UploadString(Dia_chi_Xu_ly, Chuoi_XML_Ban_hang).Trim();
            var Tai_lieu = new XmlDocument();
            Tai_lieu.LoadXml(Chuoi_XML_Kq);
            Kq = Tai_lieu.DocumentElement.GetAttribute("Kq");
        }
        catch (Exception Loi)
        {
            Kq = Loi.Message;
        }
        if (Kq == "OK")
        {
            var So_luong_Ton = int.Parse(San_pham.GetAttribute("So_luong_Ton"));
            var So_luong = int.Parse(Ban_hang.GetAttribute("So_luong"));
            So_luong_Ton -= So_luong;
            San_pham.SetAttribute("So_luong_Ton", So_luong_Ton.ToString());

        }
        return Kq;

    }

}