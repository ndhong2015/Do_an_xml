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
    public static string Tao_Chuoi_HTML_Danh_sach_San_pham_Xem(List<XmlElement> Danh_sach_San_pham)
    {
        var Chuoi_HTML_Danh_sach = "<div class='row'>";
        Danh_sach_San_pham.ForEach(San_pham =>
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
            var Chuoi_Chuc_nang_Cap_nhat_Don_gia_Ban = $"<form method='post'>   " +
                                     $"<input name='Th_Ma_so_Chuc_nang' type='hidden' value='CAP_NHAT_DON_GIA_BAN' />  " +
                                    $"<input name='Th_Ma_so_San_pham' type='hidden' value='{Ma_so}' />  " +
                                    $"<input name='Th_Don_gia_Ban' required='required' autocomplete='off' " +
                                         $"style='border:none;border-bottom:solid 1px blue'" +
                                        $"type='number' min='1'  max='10000000' value='{Don_gia_Ban}' />  " +
                                 $"</form>";
            var Chuoi_Thong_tin = $"<div class='text-left float-left' style='width:65%;height:100%;margin-left:10px;'> " +
                          $"{ Ten}" +
                          $"<br />Đơn giá Bán {  Don_gia_Ban.ToString("n0", Dinh_dang_VN) }" +
                          $"<br />Số lượng Tồn {  So_luong_Ton.ToString("n0", Dinh_dang_VN) }" +
                          $"<br />Doanh thu {  Doanh_thu.ToString("n0", Dinh_dang_VN) }" +
                          $"<br/>{Chuoi_Chuc_nang_Cap_nhat_Don_gia_Ban}" +
                          $"</div>";

            var Chuoi_HTML = $"<div class='float-left' style='width:550px;height:150px;margin-bottom:10px;margin-left:10px;{Dinh_dang_Trang_thai}' >" +
                               $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" +

                             "</div>";
            Chuoi_HTML_Danh_sach += Chuoi_HTML;
        });
        Chuoi_HTML_Danh_sach += "</div>";
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
        var Chuoi_HTML_Danh_sach = "<div class='row'>";
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
        //var Doanh_thu = Nguoi_dung.Doanh_thu;
        var Chuoi_Hinh = $"<img src='{Dia_chi_Media}/{Nguoi_dung.Ma_so}.png' " +
                                 "style='width:50px;height:50px;' />";
        var Chuoi_Thong_tin = $"<div class='btn text-left' > " +
                      $"{Nguoi_dung.Ho_ten}" +
                      //             $"<br /><i><b>Doanh thu: {Doanh_thu.ToString("n0", Dinh_dang_VN)}</b></i>" +
                      $"</div>";
        var Chuoi_HTML = $"<div class='alert'>" +
                         $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}" +
                         $"</div>";
        return Chuoi_HTML;
    }


}
