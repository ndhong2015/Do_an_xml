using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Globalization;
using System.Net;
using System.Xml;

public partial class XL_UNG_DUNG
{
    static XL_UNG_DUNG Ung_dung = null;

    XmlElement Du_lieu_Ung_dung;
    XmlElement Cua_hang;

    List<XmlElement> Danh_sach_Nguoi_dung = new List<XmlElement>();

    public static XL_UNG_DUNG Khoi_dong_Ung_dung()
    {   if (Ung_dung == null)
        {
            Ung_dung = new XL_UNG_DUNG();
            Ung_dung.Khoi_dong_Du_lieu_Ung_dung();
        }
        else
        {
           
        }
        return  Ung_dung;
    }
    void Khoi_dong_Du_lieu_Ung_dung()
    {
        var Du_lieu_tu_Dich_vu = XL_DU_LIEU.Doc_Du_lieu();
        Du_lieu_Ung_dung = Du_lieu_tu_Dich_vu;
        Cua_hang = (XmlElement)Du_lieu_Ung_dung.GetElementsByTagName("Cua_hang")[0];
        var DS_Nguoi_dung = (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nguoi_dung")[0];
        Danh_sach_Nguoi_dung = Tao_Danh_sach(DS_Nguoi_dung, "Nguoi_dung");

    }  
    public XL_LAP_TRINH_VIEN Khoi_dong_Lap_trinh_vien()
    {
        var Lap_trinh_vien= (XL_LAP_TRINH_VIEN)HttpContext.Current.Session["Lap_trinh_vien"];
        if (Lap_trinh_vien == null)
        {
            Lap_trinh_vien = new XL_LAP_TRINH_VIEN();
            Lap_trinh_vien.Danh_sach_Nguoi_dung = Danh_sach_Nguoi_dung;
           
            HttpContext.Current.Session["Lap_trinh_vien"] = Lap_trinh_vien;
        }
        
        return Lap_trinh_vien;
    }
    // Xử lý Chức năng của Lập trình viên :
    public string Khoi_dong_MH_Chinh()
    {
        var Lap_trinh_vien = (XL_LAP_TRINH_VIEN)HttpContext.Current.Session["Lap_trinh_vien"];
        
        Lap_trinh_vien.Danh_sach_Nguoi_dung_Xem = Lap_trinh_vien.Danh_sach_Nguoi_dung;
        Lap_trinh_vien.Danh_sach_Nguoi_dung_Chon = new List<XmlElement>();

        var Chuoi_HTML =  Tao_Chuoi_HTML_Xem() ;
        return Chuoi_HTML;
    }
    public string Chon_Nguoi_dung(string Ma_so_Nguoi_dung)
    {
        var Lap_trinh_vien = (XL_LAP_TRINH_VIEN)HttpContext.Current.Session["Lap_trinh_vien"];
        // Xử lý 
        var Nguoi_dung = Lap_trinh_vien.Danh_sach_Nguoi_dung.FirstOrDefault(x => x.GetAttribute("Ma_so") == Ma_so_Nguoi_dung);
        var Chuoi_HTML = "";
        if (Ma_so_Nguoi_dung == "KHACH_THAM_QUAN")
        {
            var Dia_chi_MH_Dang_nhap = Nguoi_dung.SelectSingleNode("Nhom_Nguoi_dung/@Dia_chi_MH_Dang_nhap").Value;
            var Dia_chi_Xu_ly = $"{Dia_chi_MH_Dang_nhap}";
            Chuoi_HTML = $"<iframe " +
                        $"src='{Dia_chi_Xu_ly}' " +
                         $"style='width:100%;height:1000vh;border:none'" +
                $"></iframe>";
        }
        else
        {
            var Dia_chi_MH_Dang_nhap = Nguoi_dung.SelectSingleNode("Nhom_Nguoi_dung/@Dia_chi_MH_Dang_nhap").Value;
            var Tham_so = $"Th_Ma_so_Chuc_nang=DANG_NHAP&Th_Ten_Dang_nhap={Nguoi_dung.GetAttribute("Ten_Dang_nhap")}" +
                $"&Th_Mat_khau={Nguoi_dung.GetAttribute("Mat_khau")}";
            var Dia_chi_Xu_ly = $"{Dia_chi_MH_Dang_nhap}?{Tham_so}";
            Chuoi_HTML = $"<iframe " +
                        $"src='{Dia_chi_Xu_ly}' " +
                         $"style='width:100%;height:1000vh;border:none'" +
                $"></iframe>";
        }
        ;

              
        
        return Chuoi_HTML;
    }
    public string Tra_cuu(string Chuoi_Tra_cuu)
    {
        var Danh_sach_Nguoi_dung_Xem = Tra_cuu_Nguoi_dung(Chuoi_Tra_cuu, Danh_sach_Nguoi_dung);
        var Chuoi_HTML = $"<div>" +
                 $"{Tao_Chuoi_HTML_Danh_sach_Nguoi_dung_Xem(Danh_sach_Nguoi_dung_Xem)}" +
             $"</div>";
        return Chuoi_HTML;
    }
    
    public string Tao_Chuoi_HTML_Xem()
    {
        var Lap_trinh_vien = (XL_LAP_TRINH_VIEN)HttpContext.Current.Session["Lap_trinh_vien"];
        var Chuoi_HTML = $"<div>" +
                 $"{ Tao_Chuoi_HTML_Danh_sach_Nguoi_dung_Xem(Lap_trinh_vien.Danh_sach_Nguoi_dung)}" +
             $"</div>";
        return Chuoi_HTML;
    }
}

//************************* View-Layers/Presentation Layers VL/PL **********************************
public partial class XL_UNG_DUNG
{    
    public  string Dia_chi_Media = $"{XL_DU_LIEU.Dia_chi_Dich_vu}/Media";
    public  CultureInfo Dinh_dang_VN = CultureInfo.GetCultureInfo("vi-VN");

    

    public string Tao_Chuoi_HTML_Danh_sach_Nguoi_dung_Xem(List<XmlElement> Danh_sach_Nguoi_dung)
    {
        var Lap_trinh_vien = (XL_LAP_TRINH_VIEN)HttpContext.Current.Session["Lap_trinh_vien"];
        var Danh_sach = Lap_trinh_vien.Danh_sach_Nguoi_dung_Xem;
        var Chuoi_HTML_Danh_sach = "<div class='row'>";
        Danh_sach.ForEach(Nguoi_dung =>
        {

            var Chuoi_Hinh = $"<img src='{Dia_chi_Media}/{Nguoi_dung.GetAttribute("Ma_so")}.png' " +
                             "style='width:90px;height:90px;' />";
            var Chuoi_Chuc_nang_Chon = $"<form method='post'>" +
                      $"<input name='Th_Ma_so_Chuc_nang' type='hidden' value='{Lap_trinh_vien.Chuc_nang_Chon_Nguoi_dung.Ma_so}' />" +
                       $"<input name='Th_Ma_so_Nguoi_dung' type='hidden' value='{Nguoi_dung.GetAttribute("Ma_so")}' />" +
                       $"<button type='submit' class='btn btn-danger' >Chọn</button>" +
                  "</form>";
          
            var Chuoi_Thong_tin = $"<div class='btn' style='text-align:left'> " +
                          $"{Nguoi_dung.GetAttribute("Ho_ten")}" +
                          $"<br />{ Nguoi_dung.SelectSingleNode("Nhom_Nguoi_dung/@Ten").Value}" +
                           $" { Chuoi_Chuc_nang_Chon}" +
                          $"</div>";


            var Chuoi_HTML = $"<div class='col-md-5' style='margin-bottom:10px' >" +
                               $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" +

                             "</div>";
            Chuoi_HTML_Danh_sach += Chuoi_HTML;
        });
        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }
  



     
}
//************************* Business-Layers BL **********************************
public partial class XL_UNG_DUNG
{        
    public static List<XmlElement> Tra_cuu_Nguoi_dung(string Chuoi_Tra_cuu, List<XmlElement> Danh_sach_Nguoi_dung)
    {
        Chuoi_Tra_cuu = Chuoi_Tra_cuu.ToUpper();
        var Danh_sach_Kq = new List<XmlElement>();
        Danh_sach_Kq = Danh_sach_Nguoi_dung.FindAll(x => x.GetAttribute("Ho_ten").ToUpper().Contains(Chuoi_Tra_cuu)
           || x.SelectSingleNode("Nhom_Nguoi_dung/@Ma_so").Value == Chuoi_Tra_cuu);
        return Danh_sach_Kq;        
    }
    public static List<XmlElement> Tao_Danh_sach(XmlElement Danh_sach_Nguon, string Loai_Doi_tuong)
    {
        var Danh_sach = new List<XmlElement>();
        foreach (XmlElement Doi_tuong in Danh_sach_Nguon.GetElementsByTagName(Loai_Doi_tuong))
        {
            Danh_sach.Add(Doi_tuong);
        }
        return Danh_sach;
    }
}
//************************* Data-Layers DL **********************************
public partial class XL_DU_LIEU
{    
    // Cục bộ
    public static string Dia_chi_Dich_vu = "http://localhost:50800";

    static string Dia_chi_Dich_vu_Du_lieu = $"{Dia_chi_Dich_vu}/1-Dich_vu_Giao_tiep/DV_Chinh.cshtml";
    
    public static XmlElement Doc_Du_lieu()
    {
        var Chuoi_XML = "<Du_lieu />";
        var Xu_ly = new WebClient();
        Xu_ly.Encoding = System.Text.Encoding.UTF8;
        var Tham_so = "Ma_so_Xu_ly=KHOI_DONG_DU_LIEU_KHACH_THAM_QUAN";
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