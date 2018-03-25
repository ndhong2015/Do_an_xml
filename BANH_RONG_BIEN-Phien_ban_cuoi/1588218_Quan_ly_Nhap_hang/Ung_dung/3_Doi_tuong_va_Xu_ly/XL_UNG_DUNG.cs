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
    XmlElement Du_lieu_Ung_dung;
    XmlElement Cua_hang;
    XmlElement Danh_sach_Nhom_San_pham;
    XmlElement Danh_sach_San_pham;
    XmlElement Danh_sach_Nguoi_dung;

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
        Danh_sach_Nhom_San_pham = (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nhom_San_pham")[0];
        Danh_sach_San_pham = (XmlElement)Du_lieu_Ung_dung.GetElementsByTagName("Danh_sach_San_pham")[0];
        Danh_sach_Nguoi_dung = (XmlElement)Du_lieu_Ung_dung.GetElementsByTagName("Danh_sach_Nguoi_dung")[0];
    }
    //============= Xử lý Chức năng ========
    public XmlElement Dang_nhap(string Ten_Dang_nhap, string Mat_khau)
    {
        var Chuoi_Dieu_kien = $"@Ten_Dang_nhap='{Ten_Dang_nhap}' and @Mat_khau='{Mat_khau}' ";
        var Chuoi_XPath = $"Nguoi_dung[{Chuoi_Dieu_kien}]";
        var Nguoi_dung = (XmlElement)Danh_sach_Nguoi_dung.SelectSingleNode(Chuoi_XPath);
        if (Nguoi_dung != null)
            HttpContext.Current.Session["Nguoi_dung_Dang_nhap"] = Nguoi_dung;
        return Nguoi_dung;
    }
    public string Khoi_dong()
    {
        var Danh_sach_San_pham_Xem = Danh_sach_San_pham;
        var Danh_sach_Nhom_San_pham_Xem = Danh_sach_Nhom_San_pham;
        var Danh_sach_Nhan_vien_Nhap_hang = Danh_sach_Nguoi_dung;
        var Chuoi_HTML = $"<div>" +
                $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nhom_San_pham_Xem(Danh_sach_Nhom_San_pham_Xem)}" +
                $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nhom_Nhan_vien_Nhap_hang(Danh_sach_Nhan_vien_Nhap_hang)}" +
                $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_San_pham_Xem(Danh_sach_San_pham_Xem)}" +
            $"</div>";
        return Chuoi_HTML;
    }
    public string Chon_Nhom_San_pham(string Ma_so_Nhom_San_pham)
    {

        var Danh_sach_San_pham_Xem = XL_NGHIEP_VU.Tra_cuu_San_pham(Ma_so_Nhom_San_pham, Danh_sach_San_pham);
        var Danh_sach_Nhom_San_pham_Xem = Danh_sach_Nhom_San_pham;
        var Danh_sach_Nhan_vien_Nhap_hang = Danh_sach_Nguoi_dung;
        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nhom_San_pham_Xem(Danh_sach_Nhom_San_pham_Xem)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nhom_Nhan_vien_Nhap_hang(Danh_sach_Nhan_vien_Nhap_hang)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_San_pham_Xem(Danh_sach_San_pham_Xem)}" +
             $"</div>";
        return Chuoi_HTML;
    }
    public string Tra_cuu(string Chuoi_Tra_cuu)
    {
        var Danh_sach_San_pham_Xem = XL_NGHIEP_VU.Tra_cuu_San_pham(Chuoi_Tra_cuu, Danh_sach_San_pham);
        var Danh_sach_Nhom_San_pham_Xem = Danh_sach_Nhom_San_pham;
        var Danh_sach_Nhan_vien_Nhap_hang = Danh_sach_Nguoi_dung;
        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nhom_San_pham_Xem(Danh_sach_Nhom_San_pham_Xem)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nhom_Nhan_vien_Nhap_hang(Danh_sach_Nhan_vien_Nhap_hang)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_San_pham_Xem(Danh_sach_San_pham_Xem)}" +
             $"</div>";
        return Chuoi_HTML;
    }
    public string Ten_Cua_hang()
    {
        return Cua_hang.GetAttribute("Ten");
    }
}
