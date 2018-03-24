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
    List<XmlElement> Danh_sach_Nhom_San_pham = new List<XmlElement>();
    List<XmlElement> Danh_sach_San_pham = new List<XmlElement>();
    List<XmlElement> Danh_sach_Nguoi_dung = new List<XmlElement>();
    List<XmlElement> Danh_sanh_Phieu_dat = new List<XmlElement>();


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
        var DS_Nhom_San_pham = (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nhom_San_pham")[0];
        Danh_sach_Nhom_San_pham = XL_NGHIEP_VU.Tao_Danh_sach(DS_Nhom_San_pham, "Nhom_San_pham");
        var DS_San_pham = (XmlElement)Du_lieu_Ung_dung.GetElementsByTagName("Danh_sach_San_pham")[0];
        Danh_sach_San_pham = XL_NGHIEP_VU.Tao_Danh_sach(DS_San_pham, "San_pham");

        var DS_Phieu_dat = (XmlElement)Du_lieu_Ung_dung.GetElementsByTagName("Danh_sach_Phieu_dat")[0];
        Danh_sanh_Phieu_dat = XL_NGHIEP_VU.Tao_Danh_sach(DS_Phieu_dat, "PHIEU_DAT");

        var DS_Nguoi_dung = (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nguoi_dung")[0];
        Danh_sach_Nguoi_dung = XL_NGHIEP_VU.Tao_Danh_sach(DS_Nguoi_dung, "Nguoi_dung");
    }
    //============= Xử lý Chức năng ========
    public string Dang_nhap(string Ten_Dang_nhap, string Mat_khau)
    {

        var Nguoi_dung = Danh_sach_Nguoi_dung.FirstOrDefault(
            x => x.GetAttribute("Ten_Dang_nhap") == Ten_Dang_nhap && x.GetAttribute("Mat_khau") == Mat_khau);
        var Chuoi_HTML = "";
        if (Nguoi_dung != null)
        {
            var Dia_chi_MH_Dang_nhap = Nguoi_dung.SelectSingleNode("Nhom_Nguoi_dung/@Dia_chi_MH_Dang_nhap").Value;
            var Tham_so = $"Th_Ma_so_Chuc_nang=DANG_NHAP&Th_Ten_Dang_nhap={Ten_Dang_nhap}&Th_Mat_khau={Mat_khau}";
            var Dia_chi_Xu_ly = $"{Dia_chi_MH_Dang_nhap}?{Tham_so}";
            HttpContext.Current.Response.Redirect(Dia_chi_Xu_ly);
        }

        else Chuoi_HTML = "Không hợp lệ";
        return Chuoi_HTML;
    }
    public string Khoi_dong_Nguoi_dung()
    {
        var Khach_Tham_quan = new XL_KHACH_THAM_QUAN();
        Khach_Tham_quan.Danh_sach_San_pham = Danh_sach_San_pham;
        Khach_Tham_quan.Danh_sach_Nhom_San_pham = Danh_sach_Nhom_San_pham;
        Khach_Tham_quan.Danh_sach_San_pham_Xem = Danh_sach_San_pham;
        Khach_Tham_quan.Danh_sach_San_pham_Chon = new List<XmlElement>();
        HttpContext.Current.Session["Khach_Tham_quan"] = Khach_Tham_quan;

        var Chuoi_HTML = Tao_Chuoi_HTML_Ket_qua();
        return Chuoi_HTML;
    }
    public string Chon_Nhom_San_pham(string Ma_so_Nhom_San_pham)
    {
         
        var Danh_sach_San_pham_Xem = XL_NGHIEP_VU.Tra_cuu_San_pham(Ma_so_Nhom_San_pham, Danh_sach_San_pham); 
        var Danh_sach_Nhom_San_pham_Xem = Danh_sach_Nhom_San_pham;
        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nhom_San_pham_Xem(Danh_sach_Nhom_San_pham_Xem)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_San_pham_Xem(Danh_sach_San_pham_Xem)}" +
             $"</div>";
        return Chuoi_HTML;
    }
    public string Chon_San_pham(string Ma_so_San_pham)
    {
        var Khach_Tham_quan = (XL_KHACH_THAM_QUAN)HttpContext.Current.Session["Khach_Tham_quan"];
        // Xử lý 
        var San_pham = Khach_Tham_quan.Danh_sach_San_pham.FirstOrDefault(x => x.GetAttribute("Ma_so") == Ma_so_San_pham);

        if (!Khach_Tham_quan.Danh_sach_San_pham_Chon.Contains(San_pham))
        {
            San_pham.SetAttribute("So_luong", "1");
            Khach_Tham_quan.Danh_sach_San_pham_Chon.Add(San_pham);
        }
        else
        {
            var So_luong_Ton = int.Parse(San_pham.GetAttribute("So_luong_Ton"));
            var So_luong = int.Parse(San_pham.GetAttribute("So_luong"));
            if (So_luong < So_luong_Ton)
            {
                So_luong += 1;
                San_pham.SetAttribute("So_luong", So_luong.ToString());
            }

        }
        // Tạo chuỗi HTML kết quả xem 
        var Chuoi_HTML = Tao_Chuoi_HTML_Ket_qua();
        return Chuoi_HTML;
    }
    public string Giam_So_luong_San_pham(string Ma_so_San_pham)
    {
        var Khach_Tham_quan = (XL_KHACH_THAM_QUAN)HttpContext.Current.Session["Khach_Tham_quan"];
        // Xử lý 
        var San_pham = Khach_Tham_quan.Danh_sach_San_pham.FirstOrDefault(x => x.GetAttribute("Ma_so") == Ma_so_San_pham);
        var So_luong = int.Parse(San_pham.GetAttribute("So_luong"));
        So_luong -= 1;
        San_pham.SetAttribute("So_luong", So_luong.ToString());
        if (So_luong == 0)
            Khach_Tham_quan.Danh_sach_San_pham_Chon.Remove(San_pham);
        // Tạo chuỗi HTML kết quả xem 
        var Chuoi_HTML = Tao_Chuoi_HTML_Ket_qua();
        return Chuoi_HTML;
    }
    public string Dat_hang()
    {
        var Khach_Tham_quan = (XL_KHACH_THAM_QUAN)HttpContext.Current.Session["Khach_Tham_quan"];
        // Xử lý 

        // Tạo chuỗi HTML kết quả xem 
        var Chuoi_HTML = "<iframe src='MH_Dat_hang.cshtml' style='width: 100%; height: 500px'  ></iframe>";
        return Chuoi_HTML;
    }
    public string Tra_cuu(string Chuoi_Tra_cuu)
    {
        var Danh_sach_San_pham_Xem =XL_NGHIEP_VU.Tra_cuu_San_pham(Chuoi_Tra_cuu,Danh_sach_San_pham);
        var Danh_sach_Nhom_San_pham_Xem = Danh_sach_Nhom_San_pham;
        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nhom_San_pham_Xem(Danh_sach_Nhom_San_pham_Xem)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_San_pham_Xem(Danh_sach_San_pham_Xem)}" +
             $"</div>";
        return Chuoi_HTML;
    }
    public string Tao_Chuoi_HTML_Dat_hang()
    {
        var Khach_Tham_quan = (XL_KHACH_THAM_QUAN)HttpContext.Current.Session["Khach_Tham_quan"];

        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Thong_bao(Khach_Tham_quan.Thong_bao)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Dat_hang(Khach_Tham_quan.Danh_sach_San_pham_Chon)}" +                 
             $"</div>";
        return Chuoi_HTML;

    }
    public string Tao_Chuoi_HTML_Ket_qua()
    {
        var Khach_Tham_quan = (XL_KHACH_THAM_QUAN)HttpContext.Current.Session["Khach_Tham_quan"];

        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Thong_bao(Khach_Tham_quan.Thong_bao)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_San_pham_Chon(Khach_Tham_quan.Danh_sach_San_pham_Chon)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Nhom_San_pham_Xem(Khach_Tham_quan.Danh_sach_Nhom_San_pham)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_San_pham_Xem(Khach_Tham_quan.Danh_sach_San_pham_Xem)}" +
             $"</div>";
        return Chuoi_HTML;

    }
    public string Ten_Cua_hang()
    {
        return Cua_hang.GetAttribute("Ten");
    }
    //2222222 Chức năng Ghi 222222222222222
    public string Ghi_Phieu_Dat_moi(List<XmlElement> Danh_sach, string Ho_ten, string Dien_thoai, string Dia_chi)
    {
        var Khach_Tham_quan = (XL_KHACH_THAM_QUAN)HttpContext.Current.Session["Khach_Tham_quan"];
        var Danh_sach_Ma_so_Phieu_dat = new List<string>();
        Danh_sanh_Phieu_dat.ForEach(Phieu_dat =>
        Danh_sach_Ma_so_Phieu_dat.Add(Phieu_dat.GetAttribute("Ma_so")));
        var Ma_so_Phieu_dat_moi = "";
        for (var i = 0; i <= Danh_sach_Ma_so_Phieu_dat.Count(); i++ )
        {            
            var Chuoi_Ma_so_dat_moi = "P_" + i;
            if (!Danh_sach_Ma_so_Phieu_dat.Contains(Chuoi_Ma_so_dat_moi))
            {
                Ma_so_Phieu_dat_moi = Chuoi_Ma_so_dat_moi;
                break;
            }            
        }
        var Phieu_dat_moi = Danh_sanh_Phieu_dat[0];
        Phieu_dat_moi.SetAttribute("Ma_so", Ma_so_Phieu_dat_moi.ToString());
        Phieu_dat_moi.SetAttribute("Ngay", DateTime.Now.ToString());
        Phieu_dat_moi.SetAttribute("Trang_thai", "CHO_PHAN_CONG");
        Phieu_dat_moi.SelectSingleNode("Khach_hang/@Ho_ten").Value = Ho_ten;
        Phieu_dat_moi.SelectSingleNode("Khach_hang/@Dien_thoai").Value = Dien_thoai;
        Phieu_dat_moi.SelectSingleNode("Khach_hang/@Dia_chi").Value = Dia_chi;
        Phieu_dat_moi.GetElementsByTagName("Danh_sach_San_pham")[0].InnerXml = "";

        Danh_sach.ForEach(San_pham =>
        Phieu_dat_moi.GetElementsByTagName("Danh_sach_San_pham")[0].AppendChild(San_pham)
        );
        Phieu_dat_moi.SelectSingleNode("Nhan_vien_Giao_hang/@Ma_so").Value = "";
        var Hop_le = Phieu_dat_moi != null;
        if (Hop_le)
        {           
            var Kq_Ghi = XL_LUU_TRU.Ghi_Phieu_Dat_moi(Phieu_dat_moi);
            if (Kq_Ghi == "OK")
                Khach_Tham_quan.Thong_bao = "Đã ghi nhận phiếu đặt hàng";
            else
                Khach_Tham_quan.Thong_bao = "Lỗi Hệ thống - Xin Thực hiện lại  ";

        }
        else
            Khach_Tham_quan.Thong_bao = "Lỗi Hệ thống - Xin Thực hiện lại ";
        var Chuoi_HTML = Tao_Chuoi_HTML_Ket_qua();
        return Chuoi_HTML;

    }    
}
//************************* View/Presentation -Layers VL/PL **********************************
public partial class XL_THE_HIEN
{
    public static string Dia_chi_Media = $"{XL_LUU_TRU.Dia_chi_Dich_vu}/Media";
    public static CultureInfo Dinh_dang_VN = CultureInfo.GetCultureInfo("vi-VN");

    public static string Tao_Chuoi_HTML_Thong_bao(string Thong_bao)
    {
        var Chuoi_HTML = $"<div class='alert alert-info'>" +
                          $"{Thong_bao} " +
                          $"</div>";
        return Chuoi_HTML;
    }
    public static string Tao_Chuoi_HTML_Danh_sach_San_pham_Xem(List<XmlElement> Danh_sach)
    {
        var Chuoi_HTML_Danh_sach = "<div class='row'>";
        Danh_sach.ForEach(San_pham =>
        {
            var Ten = San_pham.GetAttribute("Ten");
            var Ma_so = San_pham.GetAttribute("Ma_so");
            var Don_gia_Ban = long.Parse(San_pham.GetAttribute("Don_gia_Ban"));
            var So_luong_Ton = int.Parse(San_pham.GetAttribute("So_luong_Ton"));

            var Dinh_dang_Trang_thai = ""; var Chuoi_Trang_thai = "";
            var Chuoi_Chuc_nang_Chon = $"<form method='post'>" +
                     "<input name='Th_Ma_so_Chuc_nang' type='hidden' value='CHON_SAN_PHAM' />" +
                      $"<input name='Th_Ma_so_San_pham' type='hidden' value='{Ma_so}' />" +
                      $"<button type='submit' class='btn btn-danger' >Chọn</button>" +
                 "</form>";
            if (So_luong_Ton == 0)
            {
                Dinh_dang_Trang_thai = ";opacity:0.7"; ;
                Chuoi_Trang_thai = "Tạm thời hết hàng";
                Chuoi_Chuc_nang_Chon = "";
            }

            var Chuoi_Hinh = $"<img src='{Dia_chi_Media}/{ Ma_so}.jpg' " +
                             "style='width:90px;height:90px;' />";

            var Chuoi_Thong_tin = $"<div class='btn' style='text-align:left'> " +
                          $"{ Ten}" +
                          $"<br />Đơn giá Bán {  Don_gia_Ban.ToString("n0", Dinh_dang_VN) }" +
                          $"<br />{ Chuoi_Trang_thai }" +
                          $"</div>";
            var Chuoi_HTML = $"<div class='col-md-4' style='margin-bottom:10px;{Dinh_dang_Trang_thai}' >" +
                               $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" +
                                 $"{Chuoi_Chuc_nang_Chon}" +
                             "</div>";
            Chuoi_HTML_Danh_sach += Chuoi_HTML;
        });

        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }

    public static string Tao_Chuoi_HTML_Danh_sach_San_pham_Chon(List<XmlElement> Danh_sach)
    {
        var Chuoi_HTML_Danh_sach = "<div class='row'>";
        var Chuoi_Chuc_nang_Dat_hang = $"<div style='margin-left:10px' ><form method='post'>" +
                      "<input name='Th_Ma_so_Chuc_nang' type='hidden' value='DAT_HANG' />" +
                       $"<button type='submit' class='btn btn-danger' >Đặt hàng</button>" +
                  "</form></div>";
        if (Danh_sach.Count > 0)
            Chuoi_HTML_Danh_sach += Chuoi_Chuc_nang_Dat_hang;

        Danh_sach.ForEach(San_pham =>
        {
            var Ten = San_pham.GetAttribute("Ten");
            var Ma_so = San_pham.GetAttribute("Ma_so");
            var Don_gia_Ban = long.Parse(San_pham.GetAttribute("Don_gia_Ban"));
            var So_luong_Ton = int.Parse(San_pham.GetAttribute("So_luong_Ton"));
            var So_luong = int.Parse(San_pham.GetAttribute("So_luong"));
            var Dinh_dang_Trang_thai = ""; var Chuoi_Trang_thai = "";
            var Chuoi_Chuc_nang_Giam_So_luong = $"<form method='post'>" +
                        "<input name='Th_Ma_so_Chuc_nang' type='hidden' value='GIAM_SO_LUONG_SAN_PHAM' />" +
                         $"<input name='Th_Ma_so_San_pham' type='hidden' value='{Ma_so}' />" +
                         $"<button type='submit' class='btn btn-danger' >-</button>" +
                    "</form>";


            var Chuoi_Hinh = $"<img src='{Dia_chi_Media}/{ Ma_so}.jpg' " +
                             "style='width:90px;height:90px;' />";

            var Chuoi_Thong_tin = $"<div class='btn' style='text-align:left'> " +
                          $"{ Ten}" +
                          $"<br />Đơn giá Bán {  Don_gia_Ban.ToString("n0", Dinh_dang_VN) }" +
                          $"<br />Số lượng Đặt {  So_luong.ToString("n0", Dinh_dang_VN) }" +
                          $"<br />{ Chuoi_Trang_thai }" +
                          $"</div>";
            var Chuoi_HTML = $"<div class='col-md-4' style='margin-bottom:10px;{Dinh_dang_Trang_thai}' >" +
                               $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" +
                                 $"{Chuoi_Chuc_nang_Giam_So_luong}" +
                             "</div>";
            Chuoi_HTML_Danh_sach += Chuoi_HTML;
        });

        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }

    public static string Tao_Chuoi_HTML_Gio_hang(List<XmlElement> Danh_sach)
    {
        var Chuoi_HTML_Danh_sach = "<div class='row' style='background-color:pink'><h5 style='color:red'>Giỏ hàng<h5/>";
        var Tong_tien = 0.0;
        Danh_sach.ForEach(San_pham =>
        {
            var Ten = San_pham.GetAttribute("Ten");
            var Ma_so = San_pham.GetAttribute("Ma_so");
            var Don_gia_Ban = long.Parse(San_pham.GetAttribute("Don_gia_Ban"));
            var So_luong = int.Parse(San_pham.GetAttribute("So_luong"));
            Tong_tien += Don_gia_Ban * So_luong;
            var Chuoi_Hinh = $"<img src='{Dia_chi_Media}/{ Ma_so}.jpg' " +
                             "style='width:90px;height:90px;' />";

            var Chuoi_Thong_tin = $"<div class='btn' style='text-align:left'> " +
                          $"{ Ten}" +
                          $"<br />Đơn giá Bán {  Don_gia_Ban.ToString("n0", Dinh_dang_VN) }" +
                          $"<br />Số lượng Đặt {  So_luong.ToString("n0", Dinh_dang_VN) }" +
                          $"</div>";
            var Chuoi_HTML = $"<div class='col-md-4' style='margin-bottom:10px;>" +
                               $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" +
                             "</div>";
            Chuoi_HTML_Danh_sach += Chuoi_HTML;
        });
        
        Chuoi_HTML_Danh_sach +=$"<div><h4 style='color:red'>Tổng tiền: {Tong_tien}<h4> <div></div>";
        return Chuoi_HTML_Danh_sach;
    }

    public static string Tao_Chuoi_HTML_Danh_sach_Nhom_San_pham_Xem(List<XmlElement> Danh_sach)
    {
        var Chuoi_HTML_Danh_sach = "<div class='' style='margin:10px'>";
        Danh_sach.ForEach(Nhom_San_pham =>
        {
            var Ten = Nhom_San_pham.GetAttribute("Ten");
            var Ma_so = Nhom_San_pham.GetAttribute("Ma_so");
            var Chuoi_Hinh = $"";
            var Chuoi_Chuc_nang_Chon = "<form method='post'>" +
                                         "<input name='Th_Ma_so_Chuc_nang' type='hidden' value='CHON_NHOM_SAN_PHAM' />" +
                                          $"<input name='Th_Ma_so_Nhom_San_pham' type='hidden' value='{Ma_so}' />" +
                                          $"<button type='submit' class='btn btn-primary'>{Ten}</button>" +
                                        "</form>";
            var Chuoi_Thong_tin = $"<div class='' style=''> " +
                          $"{Chuoi_Chuc_nang_Chon} " +
                          $"</div>";
            var Chuoi_HTML = $"<div class='btn ' style=' ' >" +
                               $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" +
                             "</div>";
            Chuoi_HTML_Danh_sach += Chuoi_HTML;
        });

        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }
    public static string Tao_Chuoi_HTML_Dat_hang(List<XmlElement> Danh_sach)
    {
        var Chuoi_Thong_tin = $"<div class='btn' style='text-align:left'> " +
                          $"<h1>PHIẾU ĐẶT HÀNG</h1>" +                          
                          $"</div>";
        var Chuoi_Chuc_nang_Dat_hang = $"<form method='post'>   " +
                                 $"<input name='Th_Ma_so_Chuc_nang' type='hidden' value='GHI_PHIEU_DAT_MOI' />  " +
                                
                                    $"Họ tên: <br><input name='Th_Ho_ten' required='required' autocomplete='off' " +
                                     $"style='border:none;border-bottom:solid 1px blue'" +
                                    $"type='text' value='' /> <br> " +

                                    $"Số điện thoại: <br><input name='Th_Dien_thoai' required='required' autocomplete='off' " +
                                     $"style='border:none;border-bottom:solid 1px blue'" +
                                    $"type='text' value='' /> <br> " +                                   

                                    $"Địa chỉ: <br><input name='Th_Dia_chi' required='required' autocomplete='off' " +
                                     $"style='border:none;border-bottom:solid 1px blue'" +
                                    $"type='text' value='' /> <br> " +

                                    $"<button type='submit' class='btn btn-primary'>Đồng ý</button>" +
                             $"</form><br>";
        var Chuoi_Gio_hang = Tao_Chuoi_HTML_Gio_hang(Danh_sach);
        var Chuoi_HTML = $"<div class='col-md-4' style='margin-bottom:10px;' >" +
                           $"{Chuoi_Thong_tin}" +
                           $"{Chuoi_Chuc_nang_Dat_hang}" +
                           $"{Chuoi_Gio_hang}" +
                         "</div>";
        return Chuoi_HTML;
    }   
}
//************************* Business-Layers BL **********************************
public partial class XL_NGHIEP_VU
{
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
    
    public static List<XmlElement> Tra_cuu_San_pham(
         string Chuoi_Tra_cuu, List<XmlElement> Danh_sach)
    {
        Chuoi_Tra_cuu = Chuoi_Tra_cuu.ToUpper();
        var Danh_sach_Kq = new List<XmlElement>();
        Danh_sach_Kq = Danh_sach.FindAll(x => x.GetAttribute("Ten").ToUpper().Contains(Chuoi_Tra_cuu)
           || x.SelectSingleNode("Nhom_San_pham/@Ma_so").Value == Chuoi_Tra_cuu);
        return Danh_sach_Kq;
    }
    public static XmlElement Tim_Nhom_San_pham(string Ma_so, XmlElement Danh_sach_Nhom_San_pham)
    {
        var Kq = (XmlElement)null;
        foreach (XmlElement Nhom_San_pham in Danh_sach_Nhom_San_pham.GetElementsByTagName("San_pham"))
        {   
            if (Ma_so == Nhom_San_pham.GetAttribute("Ma_so"))
                Kq = Nhom_San_pham;

        }


        return Kq;
    }
}

//************************* Data-Layers DL **********************************
public partial class XL_LUU_TRU
{
    // Cục bộ
    public static string Dia_chi_Dich_vu = "http://localhost:50800";
 
    static string Dia_chi_Dich_vu_Du_lieu = $"{Dia_chi_Dich_vu}/1-Dich_vu_Giao_tiep/DV_Chinh.cshtml";

    public static  XmlElement  Doc_Du_lieu()
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
    // Ghi 
    public static string Ghi_Phieu_Dat_moi(XmlElement Phieu_dat)
    {
        var Kq = "OK";

        try
        {
            var Xu_ly = new WebClient();
            Xu_ly.Encoding = System.Text.Encoding.UTF8;
            var Tham_so = $"Ma_so_Xu_ly=GHI_DAT_HANG_MOI&Ma_so_Phieu_dat={Phieu_dat.GetAttribute("Ma_so")}";
            var Dia_chi_Xu_ly = $"{Dia_chi_Dich_vu_Du_lieu}?{Tham_so}";
            var Chuoi_XML_Phieu_dat = Phieu_dat.OuterXml;
            var Chuoi_XML_Kq = Xu_ly.UploadString(Dia_chi_Xu_ly, Chuoi_XML_Phieu_dat).Trim();
            var Tai_lieu = new XmlDocument();
            Tai_lieu.LoadXml(Chuoi_XML_Kq);
            Kq = Tai_lieu.DocumentElement.GetAttribute("Kq");
        }
        catch (Exception Loi)
        {
            Kq = Loi.Message;
        }
        //if (Kq=="OK")
        //{
            

        //    var So_luong_Ton = int.Parse(San_pham.GetAttribute("So_luong_Ton"));
        //    var So_luong = int.Parse(Nhap_hang.GetAttribute("So_luong"));
        //    So_luong_Ton += So_luong;
        //    San_pham.SetAttribute("So_luong_Ton", So_luong_Ton.ToString());
        //    var Nhom_San_pham_hien_hanh = Nguoi_dung_Dang_nhap.Danh_sach_Nhom_San_pham.FirstOrDefault(x => San_pham.SelectSingleNode("Nhom_San_pham/@Ma_so").Value == x.GetAttribute("Ma_so"));
        //    var So_luong_Ton_Nhom_San_pham = int.Parse(Nhom_San_pham_hien_hanh.GetAttribute("So_luong_Ton")) + So_luong;
        //    Nhom_San_pham_hien_hanh.SetAttribute("So_luong_Ton", So_luong_Ton_Nhom_San_pham.ToString());
        //}
        return Kq;

    }


}