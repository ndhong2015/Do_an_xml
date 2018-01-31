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


    public static string Tao_Chuoi_HTML_Hoc_sinh(XmlElement Hoc_sinh)
    {
        var Chuoi_HTML_Thong_tin_Hoc_sinh = "<div class='row'>";
        var Ten = Hoc_sinh.GetAttribute("Ho_ten");
        var Ma_so = Hoc_sinh.GetAttribute("Ma_so");
        var CMND = Hoc_sinh.GetAttribute("CMND");
        var Dia_chi = Hoc_sinh.GetAttribute("Dia_chi");
        var Ngay_sinh = Hoc_sinh.GetAttribute("Ngay_sinh");
        var Lop_HS = ((XmlElement)Hoc_sinh.GetElementsByTagName("Lop")[0]).GetAttribute("Ten");
        var So_ngay_vang = Hoc_sinh.GetAttribute("So_ngay_vang");
        var Chuoi_Chi_tiet_ngay_vang = "";
        if (((XmlElement)Hoc_sinh.GetElementsByTagName("Danh_sach_Vang")[0]).InnerXml.ToString().Trim() != "")
        {
            Chuoi_Chi_tiet_ngay_vang += $"<div class='btn' style='text-align:left'><b> Danh sách ngày vắng:</b></div>";
            foreach (XmlElement Vang in Hoc_sinh.GetElementsByTagName("Vang"))
            {
                if (Vang != null)
                {
                    var Ngay_vang = Vang.GetAttribute("Ngay");
                    var Ly_do = Vang.GetAttribute("Ly_do");
                    Chuoi_Chi_tiet_ngay_vang += $"<div class='btn' style='text-align:left'> " +
                                                $"Ngày vắng: {Ngay_vang}" +
                                                $"<br />" +
                                                $"Lý do: {Ly_do}" +
                                                $"</div>";
                }
            }
        }
        var Chuoi_Hinh = $"<img src='{Dia_chi_Media}/{ Ma_so}.png' " +
                             "style='width:90px;height:120px;' />";

        var Chuoi_Thong_tin = $"<div class='btn' style='text-align:left'> " +
                          $"{ Ten}" +
                          $"<br />Mã số {  Ma_so }" +
                          $"<br />CMND {  CMND }" +
                          $"<br />Ngày sinh {  Ngay_sinh }" +
                          $"<br />Địa chỉ { Dia_chi }" +
                          $"<br />Số ngày vắng { So_ngay_vang.ToString() }" +
                          $"</div>";
        var Chuoi_HTML = $"<div class='col-md-2' style='margin-bottom:10px;text-align:center;' >" +
                               $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" + $"{Chuoi_Chi_tiet_ngay_vang}"+
                             "</div>";        
        Chuoi_HTML_Thong_tin_Hoc_sinh += Chuoi_HTML;
        return Chuoi_HTML_Thong_tin_Hoc_sinh;
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
    public static XmlElement Dang_nhap_Hoc_sinh(string Ten_Dang_nhap, string Mat_khau,
                            XmlElement Danh_sach_Hoc_sinh)
    {
        var Chuoi_Dieu_kien = $"@Ten_Dang_nhap='{Ten_Dang_nhap}' and @Mat_khau='{Mat_khau}' ";
        var Chuoi_XPath = $"Hoc_sinh[{Chuoi_Dieu_kien}]";
        var Hoc_sinh = (XmlElement)Danh_sach_Hoc_sinh.SelectSingleNode(Chuoi_XPath);
        return Hoc_sinh;
    }
    public static long Tinh_so_ngay_vang(XmlElement Hoc_sinh)
    {
        var So_ngay_vang = 0;
        foreach (XmlElement Vang in (XmlElement)Hoc_sinh.GetElementsByTagName("Danh_sach_Vang")[0])
        {
            So_ngay_vang++;
        }
        return So_ngay_vang;
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
        var Tham_so = "Ma_so_Xu_ly=KHOI_DONG_DU_LIEU_HOC_SINH";
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




