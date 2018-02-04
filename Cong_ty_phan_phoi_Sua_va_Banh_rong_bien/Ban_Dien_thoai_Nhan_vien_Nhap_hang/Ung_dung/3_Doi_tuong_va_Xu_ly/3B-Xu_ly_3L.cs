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
            var Don_gia_Nhap = long.Parse (Dien_thoai.GetAttribute("Don_gia_Nhap"));
            var So_luong_Ton = int.Parse(Dien_thoai.GetAttribute("So_luong_Ton")); 
           
                
            var Chuoi_Hinh = $"<img src='{Dia_chi_Media}/{ Ma_so}.png' " +
                             "style='width:90px;height:90px;' />";
            var Chuoi_Thong_tin = $"<div class='btn' style='text-align:left'> " +
                          $"{ Ten}" +
                          $"<br />Đơn giá Nhập {  Don_gia_Nhap.ToString("n0", Dinh_dang_VN) }" +
                           $"<br />Số lượng Tồn {  So_luong_Ton.ToString("n0", Dinh_dang_VN) }" +
                          $"</div>";
            var Chuoi_So_luong = $"<form method='post'>   " +
                                     $"<input name='Th_Ma_so_Dien_thoai' type='hidden' value='{Ma_so}' />  " +
                                     $"<input name='Th_So_luong' required='required' autocomplete='off' " +
                                          $"style='border:none;border-bottom:solid 1px blue'" +
                                         $"type='number' min='1'  value='10' />  " +
                                  $"</form>";
            var Chuoi_HTML = $"<div class='col-md-2' style='margin-bottom:10px' >" +
                               $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" +
                               $"{Chuoi_So_luong}" +
                             "</div>";
            Chuoi_HTML_Danh_sach += Chuoi_HTML;
        }
        
        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }

}
//************************* Business-Layers BL **********************************
public partial class XL_NGHIEP_VU
{   public static XmlElement Dang_nhap_Nhan_vien_Nhap_hang(string Ten_Dang_nhap, string Mat_khau, 
                            XmlElement Danh_sach_Nhan_vien)
    {
        var Chuoi_Dieu_kien = $"@Ten_Dang_nhap='{Ten_Dang_nhap}' and @Mat_khau='{Mat_khau}' " +
                                      $" and Nhom_Nhan_vien/@Ma_so='NHAP_HANG'";
        var Chuoi_XPath = $"Nhan_vien[{Chuoi_Dieu_kien}]";
        var Nhan_vien = (XmlElement)Danh_sach_Nhan_vien.SelectSingleNode(Chuoi_XPath);
        return Nhan_vien;
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

    public static XmlElement Tao_Nhap_hang_Moi( XmlElement Dien_thoai, int So_luong)
    {
        var Nhap_hang = Dien_thoai.OwnerDocument.CreateElement("Nhap_hang");
        var Don_gia_Nhap = long.Parse(Dien_thoai.GetAttribute("Don_gia_Nhap"));
        var Tien = So_luong * Don_gia_Nhap;
        Nhap_hang.SetAttribute("Ngay", DateTime.Now.ToString());
        Nhap_hang.SetAttribute("So_luong", So_luong.ToString());
        Nhap_hang.SetAttribute("Don_gia", Don_gia_Nhap.ToString());
        Nhap_hang.SetAttribute("Tien", Tien.ToString());
        return Nhap_hang;
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

    public static string Ghi_Nhap_hang_Moi(XmlElement Dien_thoai, XmlElement Nhap_hang)
    {
        var Kq = "OK";

        try
        {
            var Xu_ly = new WebClient();
            Xu_ly.Encoding = System.Text.Encoding.UTF8;
            var Tham_so = $"Ma_so_Xu_ly=GHI_NHAP_HANG_MOI&Ma_so_Dien_thoai={Dien_thoai.GetAttribute("Ma_so")}";
            var Dia_chi_Xu_ly = $"{Dia_chi_Dich_vu_Du_lieu}?{Tham_so}";
            var Chuoi_XML = Nhap_hang.OuterXml;
            Kq = Xu_ly.UploadString(Dia_chi_Xu_ly, Chuoi_XML).Trim();
        }
        catch (Exception Loi)
        {
            Kq = Loi.Message;
        }
        if (Kq == "OK")
        {
            var So_luong_Ton = int.Parse(Dien_thoai.GetAttribute("So_luong_Ton"));
            var So_luong = int.Parse(Nhap_hang.GetAttribute("So_luong"));
            So_luong_Ton += So_luong;
            Dien_thoai.SetAttribute("So_luong_Ton", So_luong_Ton.ToString());
            
        }
        return Kq;

    }

}