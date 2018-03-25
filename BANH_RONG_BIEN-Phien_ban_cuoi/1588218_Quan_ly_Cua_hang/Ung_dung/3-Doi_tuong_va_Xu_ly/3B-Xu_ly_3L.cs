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
    List<XmlElement> Danh_sach_Phieu_dat = new List<XmlElement>();
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
        var DS_Phieu_dat = (XmlElement)Du_lieu_Ung_dung.GetElementsByTagName("Danh_sach_Phieu_dat")[0];
        Danh_sach_Phieu_dat = XL_NGHIEP_VU.Tao_Danh_sach(DS_Phieu_dat, "PHIEU_DAT");

        
    }
    //============= Xử lý Chức năng ========
    public XL_NGUOI_DUNG_DANG_NHAP Dang_nhap(string Ten_Dang_nhap, string Mat_khau)
    {
        var Nguoi_dung_Dang_nhap = (XL_NGUOI_DUNG_DANG_NHAP)null;
        var Danh_sach_Quan_ly_Cua_hang = Danh_sach_Nguoi_dung.FindAll(x => x.SelectSingleNode("Nhom_Nguoi_dung/@Ma_so").Value == "QUAN_LY_CUA_HANG");
        var Danh_sach_Quan_ly_Ban_hang = Danh_sach_Nguoi_dung.FindAll(x => x.SelectSingleNode("Nhom_Nguoi_dung/@Ma_so").Value == "QUAN_LY_BAN_HANG");
        var Danh_sach_Quan_ly_Nhap_hang = Danh_sach_Nguoi_dung.FindAll(x => x.SelectSingleNode("Nhom_Nguoi_dung/@Ma_so").Value == "QUAN_LY_NHAP_HANG");
        var Danh_sach_Quan_ly_Giao_hang = Danh_sach_Nguoi_dung.FindAll(x => x.SelectSingleNode("Nhom_Nguoi_dung/@Ma_so").Value == "QUAN_LY_GIAO_HANG");
        var Danh_sach_Nhan_vien_Ban_hang = Danh_sach_Nguoi_dung.FindAll(x => x.SelectSingleNode("Nhom_Nguoi_dung/@Ma_so").Value == "BAN_HANG");
        var Danh_sach_Nhan_vien_Giao_hang = Danh_sach_Nguoi_dung.FindAll(x => x.SelectSingleNode("Nhom_Nguoi_dung/@Ma_so").Value == "GIAO_HANG");

        var Nguoi_dung = Danh_sach_Quan_ly_Cua_hang.FirstOrDefault(
            x => x.GetAttribute("Ten_Dang_nhap") == Ten_Dang_nhap && x.GetAttribute("Mat_khau") == Mat_khau);
        if (Nguoi_dung != null)
        {
            
            // Thống tin Online 
            Nguoi_dung_Dang_nhap = new XL_NGUOI_DUNG_DANG_NHAP();
            Nguoi_dung_Dang_nhap.Ho_ten = Nguoi_dung.GetAttribute("Ho_ten");
            Nguoi_dung_Dang_nhap.Ma_so = Nguoi_dung.GetAttribute("Ma_so");
            Nguoi_dung_Dang_nhap.Cua_hang = Cua_hang;
            Nguoi_dung_Dang_nhap.Danh_sach_San_pham = Danh_sach_San_pham;
            Nguoi_dung_Dang_nhap.Danh_sach_Nhom_San_pham = Danh_sach_Nhom_San_pham;
            Nguoi_dung_Dang_nhap.Danh_sach_Phieu_dat = Danh_sach_Phieu_dat;
            Nguoi_dung_Dang_nhap.Danh_sach_Nhan_vien_Ban_hang = Danh_sach_Nhan_vien_Ban_hang;
            Nguoi_dung_Dang_nhap.Danh_sach_Nhan_vien_Giao_hang = Danh_sach_Nhan_vien_Giao_hang;
            Nguoi_dung_Dang_nhap.Danh_sach_Nhan_vien_Nhap_hang = Danh_sach_Quan_ly_Nhap_hang;
            Nguoi_dung_Dang_nhap.Danh_sach_Quan_ly_Ban_hang = Danh_sach_Quan_ly_Ban_hang;
            Nguoi_dung_Dang_nhap.Danh_sach_Quan_ly_Cua_hang = Danh_sach_Quan_ly_Cua_hang;
            Nguoi_dung_Dang_nhap.Danh_sach_Quan_ly_Nhap_hang = Danh_sach_Quan_ly_Nhap_hang;
            Nguoi_dung_Dang_nhap.Danh_sach_San_pham_Xem = Nguoi_dung_Dang_nhap.Danh_sach_San_pham;
            Nguoi_dung_Dang_nhap.Danh_sach_Nguoi_dung = Danh_sach_Nguoi_dung;
            Nguoi_dung_Dang_nhap.Danh_sach_Nguoi_dung_Chon = new List<XmlElement>();
            //Bổ xung thông tin cho Nhân viên Bán hàng
            Danh_sach_Nhan_vien_Ban_hang.ForEach(Nhan_vien =>
            {
                var DS_Nhom_San_pham_Nhan_vien = (XmlElement)Nhan_vien.GetElementsByTagName("Danh_sach_Nhom_San_pham")[0];
                var Danh_sach_Nhom_San_pham_Nhan_vien = XL_NGHIEP_VU.Tao_Danh_sach(DS_Nhom_San_pham_Nhan_vien, "Nhom_San_pham");
                var Doanh_thu = Danh_sach_Nhom_San_pham_Nhan_vien.Sum(x => long.Parse(x.GetAttribute("Doanh_thu")));
                Nhan_vien.SetAttribute("Doanh_thu", Doanh_thu.ToString());
            });            

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
    public string Tra_cuu_Nguoi_dung(string Chuoi_Tra_cuu)
    {
        var Danh_sach_Nguoi_dung_Xem = XL_NGHIEP_VU.Tra_cuu_Nguoi_dung_Dang_nhap(Chuoi_Tra_cuu, Danh_sach_Nguoi_dung);
        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nguoi_dung_Xem(Danh_sach_Nguoi_dung_Xem)}" +
             $"</div>";
        return Chuoi_HTML;
    }
    public string Xem_Phieu_dat()
    {
        var Nguoi_dung_Dang_nhap = (XL_NGUOI_DUNG_DANG_NHAP)HttpContext.Current.Session["Nguoi_dung_Dang_nhap"];
        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Phieu_dat(Nguoi_dung_Dang_nhap.Danh_sach_Phieu_dat)}" +
             $"</div>";
        return Chuoi_HTML;
    }
    public string Tao_Chuoi_HTML_Ket_qua()
    {
        var Nguoi_dung_Dang_nhap = (XL_NGUOI_DUNG_DANG_NHAP)HttpContext.Current.Session["Nguoi_dung_Dang_nhap"];

        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Nguoi_dung(Nguoi_dung_Dang_nhap)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Bao_cao_thong_ke(Nguoi_dung_Dang_nhap)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nguoi_dung_Xem(Nguoi_dung_Dang_nhap.Danh_sach_Nguoi_dung)}"+
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nhan_vien_Ban_hang(Nguoi_dung_Dang_nhap.Danh_sach_Nhan_vien_Ban_hang, Nguoi_dung_Dang_nhap.Danh_sach_San_pham_Xem, Nguoi_dung_Dang_nhap.Danh_sach_Nhom_San_pham)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_San_pham_Xem(Nguoi_dung_Dang_nhap.Danh_sach_San_pham_Xem)}" +
             $"</div>";
        return Chuoi_HTML;

    }    // Chức năng Chon
    public string Chon_Nguoi_dung(string Ma_so_Nguoi_dung)
    {
        var Nguoi_dung_Dang_nhap = (XL_NGUOI_DUNG_DANG_NHAP)HttpContext.Current.Session["Nguoi_dung_Dang_nhap"];
        // Xử lý 
        var Nguoi_dung = Nguoi_dung_Dang_nhap.Danh_sach_Nguoi_dung.FirstOrDefault(x => x.GetAttribute("Ma_so") == Ma_so_Nguoi_dung);
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
}
//************************* View/Presentation -Layers VL/PL **********************************
public partial class XL_THE_HIEN
{
    public static string Dia_chi_Media = $"{XL_LUU_TRU.Dia_chi_Dich_vu}/Media";
    public static CultureInfo Dinh_dang_VN = CultureInfo.GetCultureInfo("vi-VN");
    public static string Tao_Chuoi_HTML_Danh_sach_Nguoi_dung_Xem(List<XmlElement> Danh_sach_Nguoi_dung)
    {
        var Nguoi_dung_Dang_nhap = (XL_NGUOI_DUNG_DANG_NHAP)HttpContext.Current.Session["Nguoi_dung_Dang_nhap"];
        var Chuoi_HTML_Danh_sach = "<h3 style='color:red'>   Danh sách nhân sự:</h3><div class='row'></br>";
        Danh_sach_Nguoi_dung.ForEach(Nguoi_dung =>
        {

            var Chuoi_Hinh = $"<img src='{Dia_chi_Media}/{Nguoi_dung.GetAttribute("Ma_so")}.png' " +
                             "style='width:90px;height:90px;' />";        

            var Chuoi_Thong_tin = $"<div class='btn' style='text-align:left'> " +
                          $"{Nguoi_dung.GetAttribute("Ho_ten")}" +
                          $"<br />{ Nguoi_dung.SelectSingleNode("Nhom_Nguoi_dung/@Ten").Value}" +
                          $"</div>";


            var Chuoi_HTML = $"<div class='col-md-3' style='margin-bottom:10px' >" +
                               $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" +

                             "</div>";
            Chuoi_HTML_Danh_sach += Chuoi_HTML;
        });
        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }   

    public static string Tao_Chuoi_HTML_Danh_sach_San_pham_Xem(List<XmlElement> Danh_sach_San_pham)
    {
        var Chuoi_HTML_Danh_sach = "<h3 style='color:red'>   Danh sách sản phẩm:</h3><div class='row'><div>";
        Danh_sach_San_pham.ForEach(San_pham =>
        {
            var Ten = San_pham.GetAttribute("Ten");
            var Ma_so = San_pham.GetAttribute("Ma_so");
            var Don_gia_Nhap = long.Parse(San_pham.GetAttribute("Don_gia_Nhap"));
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
                          $"<br />Đơn giá Nhập {  Don_gia_Nhap.ToString("n0", Dinh_dang_VN) }" +
                          $"<br />Đơn giá Bán {  Don_gia_Ban.ToString("n0", Dinh_dang_VN) }" +
                          $"<br />Số lượng Tồn {  So_luong_Ton.ToString("n0", Dinh_dang_VN) }" +
                          $"<br />Doanh thu {  Doanh_thu.ToString("n0", Dinh_dang_VN) }" +
                          $"</div>";
            
            var Chuoi_HTML = $"<div class='float-left' style='width:550px;height:150px;margin-bottom:10px;margin-left:10px;{Dinh_dang_Trang_thai}' >" +
                               $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" +
                               
                             "</div>";
            Chuoi_HTML_Danh_sach += Chuoi_HTML;
        });
        Chuoi_HTML_Danh_sach += "</div></div>";
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
    public static string Tao_Chuoi_HTML_Danh_sach_Phieu_dat(List<XmlElement> Danh_sach_Phieu_dat)
    {
        var Chuoi_HTML_Danh_sach = "<div class=''>";
        Danh_sach_Phieu_dat.ForEach(Phieu_dat =>
        {
            var Tong_tien = 0L;
            var Danh_sach_San_pham_cua_Phieu_dat = (XmlElement)Phieu_dat.GetElementsByTagName("Danh_sach_San_pham")[0];
            var Chuoi_Thong_tin_San_pham = $"<div class=''>";
            foreach (XmlElement San_pham in Danh_sach_San_pham_cua_Phieu_dat)
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
        });

        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }
    public static string Tao_Chuoi_HTML_Danh_sach_Nhan_vien_Ban_hang
    (List<XmlElement> Danh_sach_Nhan_vien, List<XmlElement> Danh_sach_San_pham, List<XmlElement> Danh_sach_Nhom_San_pham)
    {
        var Chuoi_HTML_Danh_sach = "<h3 style = 'color:red' > Danh sách nhân viên bán hàng và thống kê nhóm hàng:</h3>";
        Danh_sach_Nhan_vien.ForEach(Nhan_vien =>
        {
            var Danh_sach_San_pham_cua_Nhan_vien = XL_NGHIEP_VU.Tao_Danh_sach_San_pham_cua_Nhan_vien_Ban_hang(Nhan_vien, Danh_sach_San_pham);
            var Doanh_thu = long.Parse(Nhan_vien.GetAttribute("Doanh_thu"));
            var Chuoi_Hinh = $"<img src='{Dia_chi_Media}/{Nhan_vien.GetAttribute("Ma_so")}.png' " +
                                 "style='width:50px;height:50px;' />";
            var Chuoi_Thong_tin = $"<div class='btn text-left' > " +
                          $"{Nhan_vien.GetAttribute("Ho_ten")}" +
                           $"<br /><i><b>Doanh thu: {Doanh_thu.ToString("n0", Dinh_dang_VN)}</b></i>" +
                          $"</div>";

            var DS_Nhom_San_pham = (XmlElement)Nhan_vien.GetElementsByTagName("Danh_sach_Nhom_San_pham")[0];
            var Danh_sach_Nhom_San_pham_cua_Nhan_vien = XL_NGHIEP_VU.Tao_Danh_sach(DS_Nhom_San_pham, "Nhom_San_pham");
            var Chuoi_Nhom_San_pham = Tao_Chuoi_HTML_Danh_sach_Nhom_San_pham_Xem(Danh_sach_Nhom_San_pham_cua_Nhan_vien);
            var Chuoi_HTML = $"<div class='col-md-5'>" +
                               $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" + $"{Chuoi_Nhom_San_pham}" +
                             "</div>";
            Chuoi_HTML_Danh_sach += Chuoi_HTML;
        });

        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }
    public static string Tao_Chuoi_HTML_Nguoi_dung(XL_NGUOI_DUNG_DANG_NHAP Nguoi_dung)
    {
        var Chuoi_Hinh = $"<img src='{Dia_chi_Media}/{Nguoi_dung.Ma_so}.png' " +
                                 "style='width:50px;height:50px;' />";
        var Chuoi_Thong_tin = $"<div class='btn text-left' > " +
                      $"{Nguoi_dung.Ho_ten}" +
                      $"</div>";
        var Chuoi_HTML = $"<div class='alert'>" +
                         $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" +
                         $"</div>";
        return Chuoi_HTML;
    }
    public static string Tao_Chuoi_HTML_Bao_cao_thong_ke(XL_NGUOI_DUNG_DANG_NHAP Nguoi_dung)
    {
        var Chuoi_Thong_tin = $"<h3 style = 'color:red' >Danh sách nhân viên bán hàng và thống kê nhóm hàng:</h3><div style='color:red; font-size:15px' class='btn text-left' > " +
                      $"<h2 style = 'color:blue'>Tổng Số lượng tồn: {long.Parse(Nguoi_dung.Cua_hang.GetAttribute("Tong_So_luong_ton")).ToString("n0", Dinh_dang_VN)}</br>" +
                      $"Tổng Doanh thu ngày: {long.Parse(Nguoi_dung.Cua_hang.GetAttribute("Tong_Doanh_thu_Ngay_hien_hanh")).ToString("c0", Dinh_dang_VN)}</br>"+
                      $"Tổng Doanh thu tháng: {long.Parse(Nguoi_dung.Cua_hang.GetAttribute("Tong_Doanh_thu_Thang_hien_hanh")).ToString("c0", Dinh_dang_VN)}</br>" +
                      $"Tổng Doanh thu năm: {long.Parse(Nguoi_dung.Cua_hang.GetAttribute("Tong_Doanh_thu_Nam_hien_hanh")).ToString("c0", Dinh_dang_VN)}</h2></br>" +

                      $"</div>";
        var Chuoi_HTML = $"<div>" +
                         $"{Chuoi_Thong_tin}" +
                         $"</div>";
        return Chuoi_HTML;
    }


}
//************************* Business-Layers BL **********************************
public partial class XL_NGHIEP_VU
{
    public static List<XmlElement> Tra_cuu_Nguoi_dung_Dang_nhap(string Chuoi_Tra_cuu, List<XmlElement> Danh_sach_Nguoi_dung)
    {
        Chuoi_Tra_cuu = Chuoi_Tra_cuu.ToUpper();
        var Danh_sach_Kq = new List<XmlElement>();
        Danh_sach_Kq = Danh_sach_Nguoi_dung.FindAll(x => x.GetAttribute("Ho_ten").ToUpper().Contains(Chuoi_Tra_cuu)
           || x.SelectSingleNode("Nhom_Nguoi_dung/@Ma_so").Value == Chuoi_Tra_cuu);
        return Danh_sach_Kq;
    }
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
    public static List<XmlElement> Tao_Danh_sach_San_pham_cua_Nhan_vien_Ban_hang(XmlElement Nhan_vien, List<XmlElement> Danh_sach_Tat_ca_San_pham)
    {
        var Danh_sach = new List<XmlElement>();
        var DS_Nhom_San_pham = (XmlElement)Nhan_vien.GetElementsByTagName("Danh_sach_Nhom_San_pham")[0];
        var Danh_sach_Nhom_San_pham = XL_NGHIEP_VU.Tao_Danh_sach(DS_Nhom_San_pham, "Nhom_San_pham");
        Danh_sach_Tat_ca_San_pham.ForEach(San_pham =>
        {
            var Ma_so_Nhom_San_pham = San_pham.SelectSingleNode("Nhom_San_pham/@Ma_so").Value;
            if (Danh_sach_Nhom_San_pham.Any(Nhom_San_pham => Nhom_San_pham.GetAttribute("Ma_so") == Ma_so_Nhom_San_pham))
                 Danh_sach.Add(San_pham);
        });
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
        Du_lieu.SetAttribute("Kq", "OK");
        return Du_lieu;
    }   

}