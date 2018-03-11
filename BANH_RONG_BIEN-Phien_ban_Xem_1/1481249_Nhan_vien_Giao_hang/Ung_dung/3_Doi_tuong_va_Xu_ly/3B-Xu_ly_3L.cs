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
    XmlElement Danh_sach_San_pham;
    XmlElement Danh_sach_Nguoi_dung;
    XmlElement Danh_sach_Phieu_dat;

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
        Danh_sach_San_pham = (XmlElement)Du_lieu_Ung_dung.GetElementsByTagName("Danh_sach_San_pham")[0];
        Danh_sach_Nguoi_dung = (XmlElement)Du_lieu_Ung_dung.GetElementsByTagName("Danh_sach_Nguoi_dung")[0];
        Danh_sach_Phieu_dat = (XmlElement)Du_lieu_Ung_dung.GetElementsByTagName("Danh_sach_Phieu_dat")[0];
        
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
        var Danh_sach_Phieu_dat_Xem = Danh_sach_Phieu_dat;
        var Chuoi_HTML = $"<div>" +
                $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Phieu_dat_Xem(Danh_sach_Phieu_dat_Xem)}" +
            $"</div>"; 
        return Chuoi_HTML;
    }
   
    public string Tra_cuu(string Chuoi_Tra_cuu)
    {
        var Danh_sach_Phieu_dat_Xem = XL_NGHIEP_VU.Tra_cuu_Phieu_dat(Chuoi_Tra_cuu,Danh_sach_Phieu_dat);
        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Phieu_dat_Xem(Danh_sach_Phieu_dat_Xem)}" +
             $"</div>";
        return Chuoi_HTML;
    }
}
//************************* View/Presentation -Layers VL/PL **********************************
public partial class XL_THE_HIEN
{
    public static string Dia_chi_Media = $"{XL_LUU_TRU.Dia_chi_Dich_vu}/Media";
    public static CultureInfo Dinh_dang_VN = CultureInfo.GetCultureInfo("vi-VN");


    public static string Tao_Chuoi_HTML_Danh_sach_Phieu_dat_Xem(XmlElement Danh_sach_Phieu_dat)
    {
        var Chuoi_HTML_Danh_sach = "<div class='row'>";
       
        foreach (XmlElement Phieu_dat in Danh_sach_Phieu_dat.GetElementsByTagName("PHIEU_DAT"))
        {
            var Ma_Phieu = Phieu_dat.GetAttribute("Ma_so");
            var Ngay = Phieu_dat.GetAttribute("Ngay");
            var Ten_Khach_hang = Phieu_dat.SelectSingleNode("Khach_hang/@Ho_ten").Value;
            var So_Dien_thoai = Phieu_dat.SelectSingleNode("Khach_hang/@Dien_thoai").Value;
            var Dia_chi = Phieu_dat.SelectSingleNode("Khach_hang/@Dia_chi").Value;
          
            var Chuoi_Thong_tin_sp = "";
            foreach (XmlElement San_pham in Phieu_dat.GetElementsByTagName("San_pham"))
            {
                var Ma_San_pham = San_pham.GetAttribute("Ma_so");
                var Ten_San_pham = San_pham.GetAttribute("Ten");
                var Don_gia = San_pham.GetAttribute("Don_gia");
                var So_luong = San_pham.GetAttribute("So_luong");
                var Thanh_tien = San_pham.GetAttribute("Tien");
                Chuoi_Thong_tin_sp = Chuoi_Thong_tin_sp +
                          $"<br />Mã Sản phẩm giao: {  Ma_San_pham}" +
                          $"<br />Tên Sản phẩm: {  Ten_San_pham}" +
                          $"<br />Đơn giá bán: {  Don_gia}" +
                          $"<br />Số lượng: {  So_luong}" +
                          $"<br />Thành tiền: {  Thanh_tien}" +
                          $"</div>";
            }
            var Chuoi_Thong_tin = $"<div class='btn' style='text-align:left'> " +
                          $"<br />Mã phiếu: {  Ma_Phieu}" +
                          $"<br />Ngày: {  Ngay}" +
                          $"<br />Tên khách hàng: {  Ten_Khach_hang}" +
                          $"<br />Số Điện thoại Khách hàng: {  So_Dien_thoai}" +
                          $"<br />Địa chỉ Khách hàng: {  Dia_chi}";
                          

            Chuoi_HTML_Danh_sach += Chuoi_Thong_tin + Chuoi_Thong_tin_sp;
        }

        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }
}
   
//************************* Business-Layers BL **********************************
public partial class XL_NGHIEP_VU
{
    public static XmlElement Tra_cuu_Phieu_dat(string Chuoi_Tra_cuu, XmlElement   Danh_sach_Phieu_dat)
    {
        Chuoi_Tra_cuu = Chuoi_Tra_cuu.ToUpper();
        var Chuoi_Danh_sach_Kq = "<Danh_sach_Phieu_dat />";
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_Danh_sach_Kq);
        var Danh_sach_Kq = Tai_lieu.DocumentElement;
        foreach (XmlElement Phieu_dat in Danh_sach_Phieu_dat.GetElementsByTagName("PHIEU_DAT"))
        {
            var Ngay = Phieu_dat.GetAttribute("Ngay");
            var Ma_phieu = Phieu_dat.GetAttribute("Ma_so");
            if (Ngay.Contains(Chuoi_Tra_cuu) || Ma_phieu == Chuoi_Tra_cuu)
            {
                var Phieu_dat_Kq = Tai_lieu.ImportNode(Phieu_dat, true);
                Danh_sach_Kq.AppendChild(Phieu_dat_Kq);
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
        var Tham_so = "Ma_so_Xu_ly=KHOI_DONG_DU_LIEU_NHAN_VIEN_GIAO_HANG";
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