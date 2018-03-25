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


    public static string Tao_Chuoi_HTML_Danh_sach_San_pham_Xem(XmlElement Danh_sach_San_pham)
    {
        var Chuoi_HTML_Danh_sach = "<div class='row'>";
        foreach (XmlElement San_pham in Danh_sach_San_pham.GetElementsByTagName("San_pham"))
        {
            var Ten = San_pham.GetAttribute("Ten");
            var Ma_so = San_pham.GetAttribute("Ma_so");
            var Don_gia_Nhap = long.Parse(San_pham.GetAttribute("Don_gia_Nhap"));
            var So_luong_Ton = int.Parse(San_pham.GetAttribute("So_luong_Ton"));
            var Dinh_dang_Trang_thai = "";
            if (So_luong_Ton == 0)
            {
                Dinh_dang_Trang_thai = ";opacity:0.7"; ;
            }

            var Chuoi_Hinh = $"<img src='{Dia_chi_Media}/{ Ma_so}.jpg' " +
                             "style='width:160px;height:180px;' />";
            var Chuoi_Thong_tin = $"<div class='btn' style='text-align:left'> " +
                          $"{ Ten}" +
                          $"<br />Đơn giá Nhập {  Don_gia_Nhap.ToString("n0", Dinh_dang_VN) }" +
                          $"<br />Số lượng Tồn {  So_luong_Ton.ToString("n0", Dinh_dang_VN) }" +
                          $"</div>";
            var Chuoi_HTML = $"<div class='col-md-5' style='margin-bottom:10px;{Dinh_dang_Trang_thai}' >" +
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
                                         $"<button type='submit' class ='btn btn-primary'><b>{Ten}</b> (Tồn {So_luong_Ton})</button>" +
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
    public static string Tao_Chuoi_HTML_Danh_sach_Nhom_Nhan_vien_Nhap_hang(XmlElement Danh_sach_Nguoi_dung)
    {
        var Chuoi_HTML_Danh_sach = "<div class='btn btn-primary' style='margin:10px'>";

        foreach (XmlElement Nhan_vien in Danh_sach_Nguoi_dung.GetElementsByTagName("Nguoi_dung"))
        {
            var Ten = "";
            var Ma_so = "";
            var Nhom_Nguoi_dung = Nhan_vien.SelectSingleNode("Nhom_Nguoi_dung/@Ma_so").Value;
            if (Nhom_Nguoi_dung == "NHAP_HANG")
            {
                Ten = Nhan_vien.GetAttribute("Ho_ten");
                Ma_so = Nhan_vien.GetAttribute("Ma_so");
                var Chuoi_Hinh = $"<img src='{Dia_chi_Media}/{ Ma_so}.png' ";
                var Chuoi_Thong_tin = $"<div class='' style=''> " +
                              $"{Ten}" +
                              $"</div>";
                var Chuoi_HTML = $"<div class='btn ' style=' ' >" +
                                   $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" +
                                 "</div>";
                Chuoi_HTML_Danh_sach += Chuoi_HTML;
            }


        }

        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }
}
