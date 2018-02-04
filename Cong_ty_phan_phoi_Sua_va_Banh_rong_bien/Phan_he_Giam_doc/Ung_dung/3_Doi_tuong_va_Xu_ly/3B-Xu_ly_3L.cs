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


    public static string Tao_Chuoi_HTML_Danh_sach_Dien_thoai(XmlElement  Danh_sach_Dien_thoai)
    {
        var Chuoi_HTML_Danh_sach = "<div class='row'>";
        foreach(XmlElement Dien_thoai in Danh_sach_Dien_thoai.GetElementsByTagName("Dien_thoai"))
        {
            var Ten = Dien_thoai.GetAttribute("Ten");
            var Ma_so = Dien_thoai.GetAttribute("Ma_so");
            var Don_gia_Ban = long.Parse (Dien_thoai.GetAttribute("Don_gia_Ban"));
            var So_luong_Ton = int.Parse(Dien_thoai.GetAttribute("So_luong_Ton"));
            var Doanh_thu = long.Parse(Dien_thoai.GetAttribute("Doanh_thu"));

            var Chuoi_Hinh = $"<img src='{Dia_chi_Media}/{ Ma_so}.png' " +
                             "style='width:90px;height:90px;' />";
            var Chuoi_Thong_tin = $"<div class='btn' style='text-align:left'> " +
                          $"{ Ten}" +
                          $"<br />Đơn giá Bán {  Don_gia_Ban.ToString("n0", Dinh_dang_VN) }" +
                           $"<br />Số lượng Tồn {  So_luong_Ton.ToString("n0", Dinh_dang_VN) }" +
                            $"<br />Doanh thu {  Doanh_thu.ToString("n0", Dinh_dang_VN) }" +
                          $"</div>";            
            var Chuoi_HTML = $"<div class='col-md-2' style='margin-bottom:10px;text-align:center;' >" +
                               $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" +
                             "</div>";
            Chuoi_HTML_Danh_sach += Chuoi_HTML;
        }
        
        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }

    public static string Tao_Chuoi_HTML_Danh_sach_Nhom_Dien_thoai(XmlElement Danh_sach_Nhom_Dien_thoai)
    {
        var Chuoi_HTML_Danh_sach = "<div class='row'>";
        foreach (XmlElement Nhom_Dien_thoai in Danh_sach_Nhom_Dien_thoai.GetElementsByTagName("Nhom_Dien_thoai"))
        {
            var Ten = Nhom_Dien_thoai.GetAttribute("Ten");
            var Ma_so = Nhom_Dien_thoai.GetAttribute("Ma_so");
            var Doanh_thu = long.Parse(Nhom_Dien_thoai.GetAttribute("Doanh_thu_ngay_Nhom_Dien_thoai"));
            var So_luong_Ton = long.Parse(Nhom_Dien_thoai.GetAttribute("So_luong_ton_Nhom_Dien_thoai"));

            var Chuoi_Thong_tin = $"<div class='btn' style='text-align:left'> " +
                          $"<h1 style = 'color:red'>{ Ten}</h1>" +
                          $"<br /><b>Doanh thu ngày: {  Doanh_thu.ToString("c0", Dinh_dang_VN) }</b>" +
                           $"<br /><b>Số lượng tồn: {  So_luong_Ton.ToString("n0", Dinh_dang_VN) }</b>" +
                          $"</div>";
            
            var Chuoi_HTML = $"<div class='col-md-3' style='margin-bottom:12px' >" +
                               $"{Chuoi_Thong_tin}" +                               
                             "</div>";
            Chuoi_HTML_Danh_sach += Chuoi_HTML;
        }

        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }
    public static string Tao_Chuoi_HTML_Tong_Doanh_thu_So_luong_ton(XmlElement Cua_hang)
    {
        var Chuoi_HTML = "";
        var Doanh_thu = long.Parse(Cua_hang.GetAttribute("Tong_Doanh_thu_ngay"));
        var Tong_ton = long.Parse(Cua_hang.GetAttribute("Tong_so_luong_ton"));
        var Chuoi_Thong_tin = $"<div class='btn' style='text-align:left'> " +
                          $"<h1 style = 'color:red'>Tổng doanh thu ngày và tổng số lượng tồn</h1>" +
                          $"<br /><b>Tổng doanh thu ngày: {  Doanh_thu.ToString("c0", Dinh_dang_VN) }</b>" +
                           $"<br /><b>Tổng số lượng tồn: {  Tong_ton.ToString("n0", Dinh_dang_VN) }</b>" +
                          $"</div>";
        return Chuoi_HTML = Chuoi_Thong_tin;
    }
}
//************************* Business-Layers BL **********************************
public partial class XL_NGHIEP_VU
{
    public static XmlElement Tim_Dien_thoai(
          string Ma_so, XmlElement Du_lieu)
    {
        var Danh_sach_Dien_thoai = (XmlElement)Du_lieu.GetElementsByTagName("Danh_sach_Dien_thoai")[0];
        var Kq = (XmlElement)null;
        foreach (XmlElement Dien_thoai in Danh_sach_Dien_thoai.GetElementsByTagName("Dien_thoai"))
        {
            if (Ma_so == Dien_thoai.GetAttribute("Ma_so"))
                Kq = Dien_thoai;

        }
        return Kq;
    }
    public static XmlElement Dang_nhap_Quan_ly_Cua_hang(string Ten_Dang_nhap, string Mat_khau, 
                            XmlElement Danh_sach_Nhan_vien)
    {
        var Chuoi_Dieu_kien = $"@Ten_Dang_nhap='{Ten_Dang_nhap}' and @Mat_khau='{Mat_khau}' " +
                                      $" and Nhom_Quan_ly/@Ma_so='QUAN_LY_CUA_HANG'";
        var Chuoi_XPath = $"Quan_ly[{Chuoi_Dieu_kien}]";
        var Quan_ly = (XmlElement)Danh_sach_Nhan_vien.SelectSingleNode(Chuoi_XPath);
        return Quan_ly;
    }
    public static  XmlElement  Tra_cuu_Dien_thoai(
          string Chuoi_Tra_cuu, XmlElement   Danh_sach_Dien_thoai)
    {
        Chuoi_Tra_cuu = Chuoi_Tra_cuu.ToUpper();
        var Chuoi_Danh_sach_Kq = "<Danh_sach_Dien_thoai />";
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_Danh_sach_Kq);
        var Danh_sach_Kq = Tai_lieu.DocumentElement;
        foreach(XmlElement Dien_thoai in Danh_sach_Dien_thoai.GetElementsByTagName("Dien_thoai"))
        {
            var Ten = Dien_thoai.GetAttribute("Ten");
            
            if (Ten.ToUpper().Contains(Chuoi_Tra_cuu ) )
            {
                var Dien_thoai_Kq = Tai_lieu.ImportNode(Dien_thoai,true );
                Danh_sach_Kq.AppendChild(Dien_thoai_Kq);
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
        var Tham_so = "Ma_so_Xu_ly=KHOI_DONG_DU_LIEU_QUAN_LY_CUA_HANG";
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