using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Net;

public class XL_THE_HIEN
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
        var Chuoi_HTML_Gio_hang = "<div style='background-color:pink; width:500px; border: 2px solid blue;'>";
        var Chuoi_HTML_Danh_sach = "<h2 style='color:red; text-align:center' >GIỎ HÀNG</h2>";
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
            var Chuoi_HTML = $"<div>" +
                               $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" +
                             "</div>";
            Chuoi_HTML_Danh_sach += Chuoi_HTML;
        });
        Chuoi_HTML_Danh_sach += $"<div><h4 style='color:red; text-align:center'>Tổng tiền: {Tong_tien}<h4>";
        Chuoi_HTML_Gio_hang += Chuoi_HTML_Danh_sach + "</div></div>";
        return Chuoi_HTML_Gio_hang;
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
                                    $"type='text' value='' /> </br> " +

                                    $"Số điện thoại: <br><input name='Th_Dien_thoai' required='required' autocomplete='off' " +
                                     $"style='border:none;border-bottom:solid 1px blue'" +
                                    $"type='text' value='' /> </br> " +

                                    $"Địa chỉ: <br><input name='Th_Dia_chi' required='required' autocomplete='off' " +
                                     $"style='border:none;border-bottom:solid 1px blue'" +
                                    $"type='text' value='' /> </br></br> " +

                                    $"<button type='submit' class='btn btn-primary'>Đồng ý</button>" +
                             $"</form></br>";
        var Chuoi_Gio_hang = Tao_Chuoi_HTML_Gio_hang(Danh_sach);
        var Chuoi_HTML = $"<div class='col-md-4' style='margin-bottom:10px;' >" +
                           $"{Chuoi_Thong_tin}" +
                           $"{Chuoi_Chuc_nang_Dat_hang}" +
                           $"{Chuoi_Gio_hang}" +
                         "</div>";
        return Chuoi_HTML;
    }
}
