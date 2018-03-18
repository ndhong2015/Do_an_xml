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
    XmlElement Danh_sach_Quan_ly_Ban_hang=null;
    XmlElement Danh_sach_Nhan_vien_Ban_hang=null;
    XmlElement Danh_sach_Phieu_dat;

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
        Danh_sach_Phieu_dat = (XmlElement)Du_lieu_Ung_dung.GetElementsByTagName("Danh_sach_Phieu_dat")[0];
    }
    //============= Xử lý Chức năng ========
    public XmlElement Dang_nhap(string Ten_Dang_nhap, string Mat_khau)
    {
        var DS_Quan_ly_Ban_hang = Danh_sach_Nguoi_dung.SelectNodes("Nguoi_dung[Nhom_Nguoi_dung/@Ma_so='QUAN_LY_BAN_HANG']");
        Danh_sach_Quan_ly_Ban_hang = XL_NGHIEP_VU.Tao_Danh_sach_XmlElement(DS_Quan_ly_Ban_hang, "Danh_sach_Nguoi_dung");
        var DS_Nhan_vien_Ban_hang = Danh_sach_Nguoi_dung.SelectNodes("Nguoi_dung[Nhom_Nguoi_dung/@Ma_so='BAN_HANG']");
        Danh_sach_Nhan_vien_Ban_hang = XL_NGHIEP_VU.Tao_Danh_sach_XmlElement(DS_Nhan_vien_Ban_hang, "Danh_sach_Nguoi_dung");
        var Chuoi_Dieu_kien = $"@Ten_Dang_nhap='{Ten_Dang_nhap}' and @Mat_khau='{Mat_khau}' ";
        var Chuoi_XPath = $"Nguoi_dung[{Chuoi_Dieu_kien}]";
        var Nguoi_dung = (XmlElement)Danh_sach_Quan_ly_Ban_hang.SelectSingleNode(Chuoi_XPath);
        if (Nguoi_dung != null)
        {
            HttpContext.Current.Session["Nguoi_dung_Dang_nhap"] = Nguoi_dung;
            
        }
        return Nguoi_dung;
    }
    public string Khoi_dong()
    {
        var Danh_sach_San_pham_Xem = Danh_sach_San_pham;
        var Danh_sach_Nhom_San_pham_Xem = Danh_sach_Nhom_San_pham;
        var Chuoi_HTML = $"<div>" +
                $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nhan_vien_Ban_hang(Danh_sach_Nhan_vien_Ban_hang,Danh_sach_San_pham_Xem,Danh_sach_Nhom_San_pham_Xem)}" +
                $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_San_pham_Xem(Danh_sach_San_pham_Xem)}" +
            $"</div>";
        return Chuoi_HTML;
    }
    public string Chon_Nhom_San_pham(string Ma_so_Nhom_San_pham)
    {

        var Danh_sach_San_pham_Xem = XL_NGHIEP_VU.Tra_cuu_San_pham(Ma_so_Nhom_San_pham, Danh_sach_San_pham);
        var Danh_sach_Nhom_San_pham_Xem = Danh_sach_Nhom_San_pham;
        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nhan_vien_Ban_hang(Danh_sach_Nhan_vien_Ban_hang, Danh_sach_San_pham_Xem, Danh_sach_Nhom_San_pham_Xem)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_San_pham_Xem(Danh_sach_San_pham_Xem)}" +
             $"</div>";
        return Chuoi_HTML;
    }
    public string Tra_cuu(string Chuoi_Tra_cuu)
    {
        var Danh_sach_San_pham_Xem = XL_NGHIEP_VU.Tra_cuu_San_pham(Chuoi_Tra_cuu, Danh_sach_San_pham);
        var Danh_sach_Nhom_San_pham_Xem = Danh_sach_Nhom_San_pham;
        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nhan_vien_Ban_hang(Danh_sach_Nhan_vien_Ban_hang, Danh_sach_San_pham_Xem, Danh_sach_Nhom_San_pham_Xem)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_San_pham_Xem(Danh_sach_San_pham_Xem)}" +
             $"</div>";
        return Chuoi_HTML;
    }
    public string Xem_Phieu_dat()
    {
        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Phieu_dat(Danh_sach_Phieu_dat)}" +
             $"</div>";
        return Chuoi_HTML;
    }
}
//************************* View/Presentation -Layers VL/PL **********************************
public partial class XL_THE_HIEN
{
    public static string Dia_chi_Media = $"{XL_LUU_TRU.Dia_chi_Dich_vu}/Media";
    public static CultureInfo Dinh_dang_VN = CultureInfo.GetCultureInfo("vi-VN");


    public static string Tao_Chuoi_HTML_Danh_sach_San_pham_Xem(XmlElement Danh_sach_San_pham)
    {
        var Chuoi_HTML_Danh_sach = "<div class='row'>";
        foreach (XmlElement San_pham in Danh_sach_San_pham.GetElementsByTagName("San_pham"))
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
            var Chuoi_Thong_tin = $"<div class='text-left float-left' style='width:65%;height:100%;margin-left:10px;'> " +
                          $"{ Ten}" +
                          $"<br />Đơn giá Bán {  Don_gia_Ban.ToString("n0", Dinh_dang_VN) }" +
                          $"<br />Số lượng Tồn {  So_luong_Ton.ToString("n0", Dinh_dang_VN) }" +
                          $"<br />Doanh thu {  Doanh_thu.ToString("n0", Dinh_dang_VN) }" +
                          $"</div>";
            var Chuoi_HTML = $"<div class='float-left' style='width:320px;height:150px;margin-bottom:10px;margin-left:10px;{Dinh_dang_Trang_thai}' >" +
                               $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" +
                             "</div>";
            Chuoi_HTML_Danh_sach += Chuoi_HTML;
        }

        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }
    public static string Tao_Chuoi_HTML_Danh_sach_Nhom_San_pham_Xem(XmlElement Danh_sach_Nhom_San_pham)
    {
        var Chuoi_HTML_Danh_sach = "<div class='btn btn-primary' style='margin:10px'>";
        foreach (XmlElement Nhom_San_pham in Danh_sach_Nhom_San_pham.GetElementsByTagName("Nhom_San_pham"))
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
        }

        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }
    public static string Tao_Chuoi_HTML_Danh_sach_Phieu_dat(XmlElement Danh_sach_Phieu_dat)
    {
        var Chuoi_HTML_Danh_sach = "<div class=''>";
        foreach (XmlElement Phieu_dat in Danh_sach_Phieu_dat.GetElementsByTagName("PHIEU_DAT"))
        {
            var Tong_tien = 0L;
            var Danh_sach_San_pham = Phieu_dat.GetElementsByTagName("Danh_sach_San_pham")[0];
            var Chuoi_Thong_tin_San_pham = $"<div class=''>";
            foreach (XmlElement San_pham in Danh_sach_San_pham)
            {
                var Ma_so_San_pham = San_pham.GetAttribute("Ma_so");
                var Ten = San_pham.GetAttribute("Ten");
                var Don_gia = long.Parse(San_pham.GetAttribute("Don_gia"));
                var So_luong = int.Parse(San_pham.GetAttribute("So_luong"));
                var Tien = long.Parse(San_pham.GetAttribute("Tien"));
                Tong_tien += Tien;
                var Chuoi_Hinh = $"<img src='{Dia_chi_Media}/{ Ma_so_San_pham}.jpg' " +
                             "class='float-left' style='width:25%;height:80%;' />";
                var Chuoi_Thong_tin = $"<div class='text-left float-left' style='width:65%;height:100%;margin-left:10px;'> " +
                          $"{ Ten}" +
                          $"<br />Đơn giá Bán: {Don_gia.ToString("n0", Dinh_dang_VN)}" +
                          $"<br />Số lượng: {So_luong.ToString("n0", Dinh_dang_VN)}" +
                          $"<br />Tiền: {Tien.ToString("n0", Dinh_dang_VN)}" +
                          $"</div>";
                var Chuoi_San_pham = $"<div class='float-left ' style='width:320px;height:150px;margin-bottom:10px;margin-left:10px;' >" +
                               $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" +
                             "</div>";
                Chuoi_Thong_tin_San_pham += Chuoi_San_pham;
            }
            Chuoi_Thong_tin_San_pham += $"</div>";

            var Ma_so_Phieu_dat = Phieu_dat.GetAttribute("Ma_so");
            var Ngay_dat = Phieu_dat.GetAttribute("Ngay");
            var Trang_thai = Phieu_dat.GetAttribute("Trang_thai");
            var Ho_ten = Phieu_dat.SelectSingleNode("Khach_hang/@Ho_ten").Value;
            var Dien_thoai = Phieu_dat.SelectSingleNode("Khach_hang/@Dien_thoai").Value;
            var Dia_chi = Phieu_dat.SelectSingleNode("Khach_hang/@Dia_chi").Value;

            var Chuoi_Thong_tin_Khach_hang = $"<div class='row' style='clear: both;'>" +
                                             $"<div class='btn text-left' >" +
                                             $"Mã Phiếu đặt: {Ma_so_Phieu_dat}" +
                                             $"<br/>Họ tên khách hàng: {Ho_ten}" +
                                             $"<br/>Điện thoai: {Dien_thoai}" +
                                             $"<br/>Địa chỉ: {Dia_chi}" +
                                             $"<br/>Ngày đặt: {Ngay_dat}" +
                                             $"<br/>Tổng tiền: {Tong_tien.ToString("n0", Dinh_dang_VN)}" +
                                             $"<br/>Danh sách Sản phẩm: <br/>" +
                                             $"</div>" +
                                             $"</div>";

            var Chuoi_HTML = $"<div class='' style='margin-bottom:10px;'>" +
                             $"{Chuoi_Thong_tin_Khach_hang}" + $"{Chuoi_Thong_tin_San_pham}" +
                             $"</div>";
            Chuoi_HTML_Danh_sach += Chuoi_HTML;
        }

        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }
    public static string Tao_Chuoi_HTML_Danh_sach_Nhan_vien_Ban_hang(XmlElement Danh_sach_Nhan_vien, XmlElement Danh_sach_San_pham, XmlElement Danh_sach_Nhom_San_pham)
    {
        var Chuoi_HTML_Danh_sach = "<div class='row'>";
        foreach(XmlElement Nhan_vien in Danh_sach_Nhan_vien)
        {
            var Danh_sach_San_pham_cua_Nhan_vien = XL_NGHIEP_VU.Tao_Danh_sach_San_pham_cua_Nhan_vien(Nhan_vien, Danh_sach_San_pham);
            var Doanh_thu = 0L;
            var Chuoi_Hinh = $"<img src='{Dia_chi_Media}/{Nhan_vien.GetAttribute("Ma_so")}.png' " +
                                 "style='width:50px;height:50px;' />";
            var Chuoi_Thong_tin = $"<div class='btn text-left' > " +
                          $"{Nhan_vien.GetAttribute("Ho_ten")}" +
                           $"<br /><i><b>Doanh thu: {Doanh_thu.ToString("n0", Dinh_dang_VN)}</b></i>" +
                          $"</div>";

            var Ma_so_Nhom_San_pham = Nhan_vien.SelectSingleNode("Danh_sach_Nhom_San_pham/Nhom_San_pham/@Ma_so").Value;
            var Chuoi_XPath = $"Nhom_San_pham[@Ma_so='{Ma_so_Nhom_San_pham}']";
            var Danh_sach_Nhom_San_pham_cua_Nhan_vien = XL_NGHIEP_VU.Tao_Danh_sach_XmlElement(Danh_sach_Nhom_San_pham.SelectNodes(Chuoi_XPath), "Danh_sach_Nhom_San_pham");
            var Chuoi_Nhom_San_pham = Tao_Chuoi_HTML_Danh_sach_Nhom_San_pham_Xem(Danh_sach_Nhom_San_pham_cua_Nhan_vien);
            var Chuoi_HTML = $"<div class='col-md-5'>" +
                               $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" + $"{Chuoi_Nhom_San_pham}" +
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
    public static XmlElement Tra_cuu_San_pham(
          string Chuoi_Tra_cuu, XmlElement Danh_sach_San_pham)
    {
        Chuoi_Tra_cuu = Chuoi_Tra_cuu.ToUpper();
        var Chuoi_Danh_sach_Kq = "<Danh_sach_San_pham />";
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_Danh_sach_Kq);
        var Danh_sach_Kq = Tai_lieu.DocumentElement;
        foreach (XmlElement San_pham in Danh_sach_San_pham.GetElementsByTagName("San_pham"))
        {
            var Ten = San_pham.GetAttribute("Ten");
            var Ma_so_Nhom_San_pham = San_pham.SelectSingleNode("Nhom_San_pham/@Ma_so").Value.ToUpper();
            if (Ten.ToUpper().Contains(Chuoi_Tra_cuu) || Ma_so_Nhom_San_pham == Chuoi_Tra_cuu)
            {
                var San_pham_Kq = Tai_lieu.ImportNode(San_pham, true);
                Danh_sach_Kq.AppendChild(San_pham_Kq);
            }
        }
        return Danh_sach_Kq;
    }
    // Tạo Danh sách ======
    public static XmlElement Tao_Danh_sach_XmlElement(XmlNodeList Danh_sach_Doi_tuong, string Ten_the_Doi_tuong)
    {
        var Chuoi_XML_Danh_sach = $"<{Ten_the_Doi_tuong} />";
        var Tai_lieu_Danh_sach = new XmlDocument();
        Tai_lieu_Danh_sach.LoadXml(Chuoi_XML_Danh_sach);
        var Danh_sach = Tai_lieu_Danh_sach.DocumentElement;
        foreach(XmlElement Doi_tuong in Danh_sach_Doi_tuong)
        {
            Danh_sach.AppendChild(Tai_lieu_Danh_sach.ImportNode(Doi_tuong,true));
        }
        return Danh_sach;
    }
    public static XmlElement Tao_Danh_sach_San_pham_cua_Nhan_vien(XmlElement Nhan_vien, XmlElement Danh_sach_San_pham)
    {
        var Ma_so_Nhom_San_pham = Nhan_vien.SelectSingleNode("Danh_sach_Nhom_San_pham/Nhom_San_pham/@Ma_so").Value;
        var Chuoi_Dieu_kien = $"Nhom_San_pham/@Ma_so='{Ma_so_Nhom_San_pham}'";
        var Chuoi_XPath = $"San_pham[{Chuoi_Dieu_kien}]";
        var Danh_sach = XL_NGHIEP_VU.Tao_Danh_sach_XmlElement(Danh_sach_San_pham.SelectNodes(Chuoi_XPath), "Danh_sach_San_pham");
        
        return Danh_sach;
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
        var Tham_so = "Ma_so_Xu_ly=KHOI_DONG_DU_LIEU_QUAN_LY_BAN_HANG";
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