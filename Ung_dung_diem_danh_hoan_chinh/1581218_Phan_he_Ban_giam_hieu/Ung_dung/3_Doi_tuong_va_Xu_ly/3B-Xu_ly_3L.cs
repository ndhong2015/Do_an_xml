using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Net;
//************************* View/Presentation -Layers VL/PL **********************************
public partial class XL_THE_HIEN
{
    public static string Dia_chi_Media = $"{XL_LUU_TRU.Dia_chi_Dich_vu}/Media";
    public static CultureInfo Dinh_dang_VN = CultureInfo.GetCultureInfo("vi-VN");


    public static string Tao_Chuoi_HTML_Danh_sach_Hoc_sinh(XmlElement  Danh_sach_Hoc_sinh)
    {
        var Chuoi_HTML_Danh_sach = "<div class='row'>";
        foreach(XmlElement Hoc_sinh in Danh_sach_Hoc_sinh.GetElementsByTagName("Hoc_sinh"))
        {
            var Ten = Hoc_sinh.GetAttribute("Ho_ten");
            var Ma_so = Hoc_sinh.GetAttribute("Ma_so");
            var Lop = Hoc_sinh.GetAttribute("Lop");
            var So_ngay_vang = Hoc_sinh.GetAttribute("So_ngay_vang");
            var Chuoi_Hinh = $"<img src='{Dia_chi_Media}/{ Ma_so}.png' " +
                             "style='width:90px;height:90px;' />";
            var Chuoi_Thong_tin = $"<div class='btn' style='text-align:left'> " +
                          $"{ Ten}" +
                          $"<br />{Lop.ToString()}" +
                            $"<br />Số ngày vắng: {  So_ngay_vang.ToString() }" +
                          $"</div>";            
            var Chuoi_HTML = $"<div class='col-md-2' style='margin-bottom:10px;text-align:center;' >" +
                               $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" +
                             "</div>";
            Chuoi_HTML_Danh_sach += Chuoi_HTML;
        }
        
        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }    
}
//************************* Business-Layers BL **********************************
public partial class XL_NGHIEP_VU
{
    public static XmlElement Tim_Hoc_sinh(
          string Ma_so, XmlElement Du_lieu)
    {
        var Danh_sach_Hoc_sinh = (XmlElement)Du_lieu.GetElementsByTagName("Danh_sach_Hoc_sinh")[0];
        var Kq = (XmlElement)null;
        foreach (XmlElement Hoc_sinh in Danh_sach_Hoc_sinh.GetElementsByTagName("Hoc_sinh"))
        {
            if (Ma_so == Hoc_sinh.GetAttribute("Ma_so"))
                Kq = Hoc_sinh;

        }
        return Kq;
    }
    public static XmlElement Dang_nhap_Ban_giam_hieu(string Ten_Dang_nhap, string Mat_khau, 
                            XmlElement Danh_sach_Ban_giam_hieu)
    {
        var Chuoi_Dieu_kien = $"@Ten_Dang_nhap='{Ten_Dang_nhap}' and @Mat_khau='{Mat_khau}' ";
        var Chuoi_XPath = $"Giam_hieu[{Chuoi_Dieu_kien}]";
        var Ban_giam_hieu = (XmlElement)Danh_sach_Ban_giam_hieu.SelectSingleNode(Chuoi_XPath);
        return Ban_giam_hieu;
    }
    public static  XmlElement  Tra_cuu_Hoc_sinh(
          string Chuoi_Tra_cuu, XmlElement   Danh_sach_Hoc_sinh)
    {
        Chuoi_Tra_cuu = Chuoi_Tra_cuu.ToUpper();
        var Chuoi_Danh_sach_Kq = "<Danh_sach_Hoc_sinh />";
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_Danh_sach_Kq);
        var Danh_sach_Kq = Tai_lieu.DocumentElement;
        foreach(XmlElement Hoc_sinh in Danh_sach_Hoc_sinh.GetElementsByTagName("Hoc_sinh"))
        {
            var Ten = Hoc_sinh.GetAttribute("Ho_ten");
            
            if (Ten.ToUpper().Contains(Chuoi_Tra_cuu ) )
            {
                var Hoc_sinh_Kq = Tai_lieu.ImportNode(Hoc_sinh,true );
                Danh_sach_Kq.AppendChild(Hoc_sinh_Kq);
            }
        }        
                    
        return Danh_sach_Kq;
    }
}

//************************* Data-Layers DL **********************************
public partial class XL_LUU_TRU
{
    public static string Dia_chi_Dich_vu = "http://localhost:50800";
    static string Dia_chi_Dich_vu_Du_lieu = $"{Dia_chi_Dich_vu}/1-Dich_vu_Giao_tiep/DV_Chinh.cshtml";

    public static XmlElement Doc_Du_lieu()
    {
        var Chuoi_XML = "<Du_lieu />";
        var Xu_ly = new WebClient();
        Xu_ly.Encoding = System.Text.Encoding.UTF8;
        var Tham_so = "Ma_so_Xu_ly=KHOI_DONG_DU_LIEU_BAN_GIAM_HIEU";
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

        return Du_lieu;

    }
}