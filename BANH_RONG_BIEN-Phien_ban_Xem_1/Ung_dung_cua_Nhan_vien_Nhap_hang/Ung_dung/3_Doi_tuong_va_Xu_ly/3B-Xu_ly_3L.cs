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
//==================== Khở động ==============
    static XL_UNG_DUNG Ung_dung = null;
    XmlElement Du_lieu_Ung_dung;
    XmlElement Cua_hang;
    XmlElement Danh_sach_Nhom_Dien_thoai;
    XmlElement Danh_sach_Dien_thoai;
    XmlElement Danh_sach_Nguoi_dung;

    public static XL_UNG_DUNG Khoi_dong_Ung_dung()
    {
        if (Ung_dung==null)
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
        Danh_sach_Nhom_Dien_thoai = (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nhom_Dien_thoai")[0];
        Danh_sach_Dien_thoai = (XmlElement)Du_lieu_Ung_dung.GetElementsByTagName("Danh_sach_Dien_thoai")[0];
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
        var Danh_sach_Dien_thoai_Xem = Danh_sach_Dien_thoai;
        var Danh_sach_Nhom_Dien_thoai_Xem= Danh_sach_Nhom_Dien_thoai;
        var Chuoi_HTML = $"<div>" +
                $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nhom_Dien_thoai_Xem(Danh_sach_Nhom_Dien_thoai_Xem)}" +
                $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Dien_thoai_Xem(Danh_sach_Dien_thoai_Xem)}" +
            $"</div>"; 
        return Chuoi_HTML;
    }
    public string Chon_Nhom_Dien_thoai(string Ma_so_Nhom_Dien_thoai)
    {

        var Danh_sach_Dien_thoai_Xem = XL_NGHIEP_VU.Tra_cuu_Dien_thoai(Ma_so_Nhom_Dien_thoai, Danh_sach_Dien_thoai);
        var Danh_sach_Nhom_Dien_thoai_Xem = Danh_sach_Nhom_Dien_thoai;
        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nhom_Dien_thoai_Xem(Danh_sach_Nhom_Dien_thoai_Xem)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Dien_thoai_Xem(Danh_sach_Dien_thoai_Xem)}" +
             $"</div>";
        return Chuoi_HTML;
    }
    public string Tra_cuu(string Chuoi_Tra_cuu)
    {
        var Danh_sach_Dien_thoai_Xem =XL_NGHIEP_VU.Tra_cuu_Dien_thoai(Chuoi_Tra_cuu,Danh_sach_Dien_thoai);
        var Danh_sach_Nhom_Dien_thoai_Xem = Danh_sach_Nhom_Dien_thoai;
        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nhom_Dien_thoai_Xem(Danh_sach_Nhom_Dien_thoai_Xem)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Dien_thoai_Xem(Danh_sach_Dien_thoai_Xem)}" +
             $"</div>";
        return Chuoi_HTML;
    }
}
//************************* View/Presentation -Layers VL/PL **********************************
public partial class XL_THE_HIEN
{
    public static string Dia_chi_Media = $"{XL_LUU_TRU.Dia_chi_Dich_vu}/Media";
    public static CultureInfo Dinh_dang_VN = CultureInfo.GetCultureInfo("vi-VN");


    public static string Tao_Chuoi_HTML_Danh_sach_Dien_thoai_Xem(XmlElement  Danh_sach_Dien_thoai)
    {
        var Chuoi_HTML_Danh_sach = "<div class='row'>";
        foreach(XmlElement Dien_thoai in Danh_sach_Dien_thoai.GetElementsByTagName("Dien_thoai"))
        {
            var Ten = Dien_thoai.GetAttribute("Ten");
            var Ma_so = Dien_thoai.GetAttribute("Ma_so");
            var Don_gia_Nhap = long.Parse (Dien_thoai.GetAttribute("Don_gia_Nhap"));
            var So_luong_Ton = int.Parse(Dien_thoai.GetAttribute("So_luong_Ton"));
            var Dinh_dang_Trang_thai = ""; 
            if (So_luong_Ton == 0)
            {
                Dinh_dang_Trang_thai = ";opacity:0.7"; ;
            }
                
            var Chuoi_Hinh = $"<img src='{Dia_chi_Media}/{ Ma_so}.png' " +
                             "style='width:90px;height:90px;' />";
            var Chuoi_Thong_tin = $"<div class='btn' style='text-align:left'> " +
                          $"{ Ten}" +
                          $"<br />Đơn giá Nhập {  Don_gia_Nhap.ToString("n0", Dinh_dang_VN) }" +
                          $"<br />Số lượng Tồn {  So_luong_Ton.ToString("n0", Dinh_dang_VN) }" +
                          $"</div>";
            var Chuoi_HTML = $"<div class='col-md-4' style='margin-bottom:10px;{Dinh_dang_Trang_thai}' >" +
                               $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" +
                             "</div>";
            Chuoi_HTML_Danh_sach += Chuoi_HTML;
        }
        
        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }
    public static string Tao_Chuoi_HTML_Danh_sach_Nhom_Dien_thoai_Xem(XmlElement Danh_sach_Nhom_Dien_thoai)
    {
        var Chuoi_HTML_Danh_sach = "<div class='btn btn-primary' style='margin:10px'>";
        foreach (XmlElement Nhom_Dien_thoai in Danh_sach_Nhom_Dien_thoai.GetElementsByTagName("Nhom_Dien_thoai"))
        {
            var Ten = Nhom_Dien_thoai.GetAttribute("Ten");
            var Ma_so = Nhom_Dien_thoai.GetAttribute("Ma_so");
            
            var So_luong_Ton = int.Parse(Nhom_Dien_thoai.GetAttribute("So_luong_Ton"));
            var Chuoi_Chuc_nang_Chon = $"<form method='post'>" +
                                        "<input name='Th_Ma_so_Chuc_nang' type='hidden' value='CHON_NHOM_DIEN_THOAI' />" +
                                         $"<input name='Th_Ma_so_Nhom_Dien_thoai' type='hidden' value='{Ma_so}' />" +
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
        }

        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }
}
//************************* Business-Layers BL **********************************
public partial class XL_NGHIEP_VU
{
    public static  XmlElement  Tra_cuu_Dien_thoai(
          string Chuoi_Tra_cuu, XmlElement   Danh_sach_Dien_thoai)
    {
        Chuoi_Tra_cuu = Chuoi_Tra_cuu.ToUpper();
        var Chuoi_Danh_sach_Kq = "<Danh_sach_Dien_thoai />";
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_Danh_sach_Kq);
        var Danh_sach_Kq = Tai_lieu.DocumentElement;
        foreach (XmlElement Dien_thoai in Danh_sach_Dien_thoai.GetElementsByTagName("Dien_thoai"))
        {
            var Ten = Dien_thoai.GetAttribute("Ten");
            var Ma_so_Nhom_Dien_thoai = Dien_thoai.SelectSingleNode("Nhom_Dien_thoai/@Ten").Value.ToUpper();
            if (Ten.ToUpper().Contains(Chuoi_Tra_cuu) || Ma_so_Nhom_Dien_thoai == Chuoi_Tra_cuu)
            {
                var Dien_thoai_Kq = Tai_lieu.ImportNode(Dien_thoai, true);
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

    public static  XmlElement  Doc_Du_lieu()
    {
        var Chuoi_XML = "<Du_lieu />";
        var Xu_ly = new WebClient();
        Xu_ly.Encoding = System.Text.Encoding.UTF8;
        var Tham_so = "Ma_so_Xu_ly=KHOI_DONG_DU_LIEU_NHAN_VIEN_NHAP_HANG";
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