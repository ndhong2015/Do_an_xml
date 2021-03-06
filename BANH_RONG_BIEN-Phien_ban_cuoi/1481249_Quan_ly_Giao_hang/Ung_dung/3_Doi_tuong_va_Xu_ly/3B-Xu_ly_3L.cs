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
        var DS_Nguoi_dung = (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nguoi_dung")[0];
        Danh_sach_Nguoi_dung = XL_NGHIEP_VU.Tao_Danh_sach(DS_Nguoi_dung, "Nguoi_dung");
        var DS_Phieu_dat = (XmlElement)Du_lieu_Ung_dung.GetElementsByTagName("Danh_sach_Phieu_dat")[0];
        Danh_sach_Phieu_dat = XL_NGHIEP_VU.Tao_Danh_sach(DS_Phieu_dat, "PHIEU_DAT");
    }
    //============= Xử lý Chức năng ========
   
    public XL_NGUOI_DUNG_DANG_NHAP Dang_nhap(string Ten_Dang_nhap, string Mat_khau)
    {
        var Nguoi_dung_Dang_nhap = (XL_NGUOI_DUNG_DANG_NHAP)null;
        var Nguoi_dung = Danh_sach_Nguoi_dung.FirstOrDefault(
            x => x.GetAttribute("Ten_Dang_nhap") == Ten_Dang_nhap && x.GetAttribute("Mat_khau") == Mat_khau);
        if (Nguoi_dung != null)
        {
            var Danh_sach_Phieu_dat_Chua_Phan_cong = Danh_sach_Phieu_dat.FindAll(
                             x => x.GetAttribute("Trang_thai") == "CHO_PHAN_CONG");
            var Danh_sach_Phieu_dat_Da_Phan_cong = Danh_sach_Phieu_dat.FindAll(
                             x => x.GetAttribute("Trang_thai") == "CHO_GIAO_HANG");

            // Thống tin Online 
            Nguoi_dung_Dang_nhap = new XL_NGUOI_DUNG_DANG_NHAP();
            Nguoi_dung_Dang_nhap.Ho_ten = Nguoi_dung.GetAttribute("Ho_ten");
            Nguoi_dung_Dang_nhap.Ma_so = Nguoi_dung.GetAttribute("Ma_so");
            Nguoi_dung_Dang_nhap.Cua_hang = Cua_hang;
            Nguoi_dung_Dang_nhap.Danh_sach_Phieu_dat = Danh_sach_Phieu_dat;
            Nguoi_dung_Dang_nhap.Danh_sach_Phieu_dat_Xem = Danh_sach_Phieu_dat;
            Nguoi_dung_Dang_nhap.Danh_sach_Phieu_dat_Chua_Phan_cong = Danh_sach_Phieu_dat_Chua_Phan_cong;
            Nguoi_dung_Dang_nhap.Danh_sach_Phieu_dat_Da_Phan_cong = Danh_sach_Phieu_dat_Da_Phan_cong;
            HttpContext.Current.Session["Nguoi_dung_Dang_nhap"] = Nguoi_dung_Dang_nhap;
        }
        return Nguoi_dung_Dang_nhap;
    }
    public string Khoi_dong()
    {
        var Chuoi_HTML = Tao_Chuoi_HTML_Ket_qua();
        return Chuoi_HTML;
    }
    public string Tra_cuu(string Chuoi_Tra_cuu)
    {
        var Nguoi_dung_Dang_nhap = (XL_NGUOI_DUNG_DANG_NHAP)HttpContext.Current.Session["Nguoi_dung_Dang_nhap"];
        Nguoi_dung_Dang_nhap.Danh_sach_Phieu_dat_Xem = XL_NGHIEP_VU.Tra_cuu_Phieu_dat(
                                    Chuoi_Tra_cuu, Nguoi_dung_Dang_nhap.Danh_sach_Phieu_dat);
        var Chuoi_HTML = Tao_Chuoi_HTML_Ket_qua();
        return Chuoi_HTML;
    }
    // GHi
    public string Ghi_Phan_cong(string Ma_so_Phieu_dat, string Ma_NV_Giao_hang)
    {
        var Nguoi_dung_Dang_nhap = (XL_NGUOI_DUNG_DANG_NHAP)HttpContext.Current.Session["Nguoi_dung_Dang_nhap"];
        var Phieu_dat = Danh_sach_Phieu_dat.FirstOrDefault(x => x.GetAttribute("Ma_so") == Ma_so_Phieu_dat);
        var Hop_le = Phieu_dat != null;
        if (Hop_le)
        {
            var Nhan_vien_Giao_hang = (XmlElement)Phieu_dat.GetElementsByTagName("Nhan_vien_Giao_hang")[0];
            Nhan_vien_Giao_hang.SetAttribute("Ma_so", Ma_NV_Giao_hang);
            var Kq_Ghi = XL_LUU_TRU.Ghi_Phan_cong(Phieu_dat, Nhan_vien_Giao_hang);
            if (Kq_Ghi == "OK")
            {
                Nguoi_dung_Dang_nhap.Thong_bao = $"Phân công hoàn tất Phiếu {Phieu_dat.GetAttribute("Ma_so")}";

                Nguoi_dung_Dang_nhap.Danh_sach_Phieu_dat_Chua_Phan_cong.Remove(Phieu_dat);
                Nguoi_dung_Dang_nhap.Danh_sach_Phieu_dat_Da_Phan_cong.Add(Phieu_dat);
            }
            else
                Nguoi_dung_Dang_nhap.Thong_bao = "Lỗi Hệ thống - Xin Thực hiện lại  ";
        }
        else
            Nguoi_dung_Dang_nhap.Thong_bao = "Lỗi Hệ thống - Xin Thực hiện lại ";

        var Chuoi_HTML = Tao_Chuoi_HTML_Chua_Phan_cong();
        return Chuoi_HTML;

    }
    public string Tao_Chuoi_HTML_Ket_qua()
    {
        var Nguoi_dung_Dang_nhap = (XL_NGUOI_DUNG_DANG_NHAP)HttpContext.Current.Session["Nguoi_dung_Dang_nhap"];
        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Thong_bao(Nguoi_dung_Dang_nhap.Thong_bao)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Phieu_dat_Xem(Nguoi_dung_Dang_nhap.Danh_sach_Phieu_dat_Xem)}" +
                
             $"</div>";
        return Chuoi_HTML;
    }
    public string Tao_Chuoi_HTML_Da_Phan_cong()
    {
        var Nguoi_dung_Dang_nhap = (XL_NGUOI_DUNG_DANG_NHAP)HttpContext.Current.Session["Nguoi_dung_Dang_nhap"];
        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Thong_bao(Nguoi_dung_Dang_nhap.Thong_bao)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Phieu_dat_Da_Phan_cong(Nguoi_dung_Dang_nhap.Danh_sach_Phieu_dat_Da_Phan_cong)}" +
             $"</div>";
        return Chuoi_HTML;
    }
    public string Tao_Chuoi_HTML_Chua_Phan_cong()
    {
        var Nguoi_dung_Dang_nhap = (XL_NGUOI_DUNG_DANG_NHAP)HttpContext.Current.Session["Nguoi_dung_Dang_nhap"];
        var Chuoi_HTML = $"<div>" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Thong_bao(Nguoi_dung_Dang_nhap.Thong_bao)}" +
                 $"{XL_THE_HIEN.Tao_Chuoi_HTML_Danh_sach_Phieu_dat_Chua_Phan_cong(Nguoi_dung_Dang_nhap.Danh_sach_Phieu_dat_Chua_Phan_cong)}" +
             $"</div>";
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
    public static string Tao_Chuoi_HTML_Danh_sach_Phieu_dat_Xem(List<XmlElement> Danh_sach_Phieu_dat)
    {
        var Chuoi_HTML_Danh_sach = "<div class='row'>";

        Danh_sach_Phieu_dat.ForEach(Phieu_dat =>
        {
            var Ma_Phieu = Phieu_dat.GetAttribute("Ma_so");
            var Ngay = Phieu_dat.GetAttribute("Ngay");
            var Trang_thai = Phieu_dat.GetAttribute("Trang_thai");
            var Dia_chi = Phieu_dat.SelectSingleNode("Khach_hang/@Dia_chi").Value;
            var NV_Giao_hang = "";
            var Chuoi_Thong_tin_sp = "";
            var Tong_tien = 0L;
            foreach (XmlElement San_pham in Phieu_dat.GetElementsByTagName("San_pham"))
            {
                var Ma_San_pham = San_pham.GetAttribute("Ma_so");
                var Ten_San_pham = San_pham.GetAttribute("Ten");
                var Don_gia = San_pham.GetAttribute("Don_gia");
                var So_luong = San_pham.GetAttribute("So_luong");
                var Thanh_tien = San_pham.GetAttribute("Tien");
                Tong_tien += long.Parse(Thanh_tien);
                Chuoi_Thong_tin_sp = Chuoi_Thong_tin_sp +
                          $"<br />Mã Sản phẩm giao: {  Ma_San_pham}" +
                          $"<br />Tên Sản phẩm: {  Ten_San_pham}" +
                          $"<br />Đơn giá bán: {  Don_gia}" +
                          $"<br />Số lượng: {  So_luong}" +
                          $"<br /><i>Thành tiền: { Thanh_tien}</i>";
            }
            var Chuoi_Thong_tin = $"<div class='btn' style='text-align:left; border:1px' > " +
                          $"<br /><b>Mã phiếu: {  Ma_Phieu}</b>" +
                          $"<br />Ngày: {  Ngay}" +
                          $"<br />Trạng thái: {  Trang_thai}" +
                          $"<br />Địa chỉ Khách hàng: {  Dia_chi}";
            var Chuoi_Phan_cong = $"<form method='post'>" +
                        $"<b>Tổng tiền: { Tong_tien}</b> </br>" +
                        $"<input name='Th_Ma_so_Chuc_nang' type='hidden' value='PHAN_CONG' />  " +
                        $"<input name='Th_Ma_so_Phieu_dat' type='hidden' value='{Ma_Phieu}' />  " +
                        $"<input type='text' name='Th_Ma_so_Nhan_vien' value='' />  " +
                        $"<button type='submit' class ='btn btn-primary'>Phân công</button>" +
                        $"</form>" +
                        $"</div>";
            if (Trang_thai == "CHO_PHAN_CONG")
                Chuoi_HTML_Danh_sach += Chuoi_Thong_tin + Chuoi_Thong_tin_sp + Chuoi_Phan_cong;
            else
            {
                NV_Giao_hang = Phieu_dat.SelectSingleNode("Nhan_vien_Giao_hang/@Ma_so").Value;
                Chuoi_HTML_Danh_sach += Chuoi_Thong_tin + Chuoi_Thong_tin_sp + $"<br />Nhân viên Giao hàng: {NV_Giao_hang}" +"</div>";
            }
        });
        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }
    public static string Tao_Chuoi_HTML_Danh_sach_Phieu_dat_Chua_Phan_cong(List<XmlElement> Danh_sach_Phieu_dat)
    {
        var Chuoi_HTML_Danh_sach = "<div class='row'>";

        Danh_sach_Phieu_dat.ForEach(Phieu_dat =>
        {
            var Ma_Phieu = Phieu_dat.GetAttribute("Ma_so");
            var Ngay = Phieu_dat.GetAttribute("Ngay");
            var Trang_thai = Phieu_dat.GetAttribute("Trang_thai");
            var Dia_chi = Phieu_dat.SelectSingleNode("Khach_hang/@Dia_chi").Value;
            var Chuoi_Thong_tin_sp = "";
            var Tong_tien = 0L;
            foreach (XmlElement San_pham in Phieu_dat.GetElementsByTagName("San_pham"))
            {
                var Ma_San_pham = San_pham.GetAttribute("Ma_so");
                var Ten_San_pham = San_pham.GetAttribute("Ten");
                var Don_gia = San_pham.GetAttribute("Don_gia");
                var So_luong = San_pham.GetAttribute("So_luong");
                var Thanh_tien = San_pham.GetAttribute("Tien");
                Tong_tien += long.Parse(Thanh_tien);
                Chuoi_Thong_tin_sp = Chuoi_Thong_tin_sp +
                          $"<br />Mã Sản phẩm giao: {  Ma_San_pham}" +
                          $"<br />Tên Sản phẩm: {  Ten_San_pham}" +
                          $"<br />Đơn giá bán: {  Don_gia}" +
                          $"<br />Số lượng: {  So_luong}" +
                          $"<br /><i>Thành tiền: { Thanh_tien}</i>";
            }
            var Chuoi_Thong_tin = $"<div class='btn' style='text-align:left; border:1px' > " +
                          $"<br /><b>Mã phiếu: {  Ma_Phieu}</b>" +
                          $"<br />Ngày: {  Ngay}" +
                          $"<br />Trạng thái: {  Trang_thai}" +
                          $"<br />Địa chỉ Khách hàng: {  Dia_chi}";
            var Chuoi_Phan_cong = $"<form method='post'>" +
                        $"<b>Tổng tiền: { Tong_tien}</b> </br>" +
                        $"<input name='Th_Ma_so_Chuc_nang' type='hidden' value='GHI_PHAN_CONG' />  " +
                        $"<input name='Th_Ma_so_Phieu_dat' type='hidden' value='{Ma_Phieu}' />  " +
                        $"<select name='Th_Ma_so_Nhan_vien'> " +
                             $"<option value='NV_7'> Nhân viên 7 </option>" +
                             $"<option value='NV_8'> Nhân viên 8 </option>" +
                        $"</select>" +
                        $"<button type='submit' class ='btn btn-primary'>Đồng ý</button>" +
                        $"</form>" +
                        $"</div>";
            
            Chuoi_HTML_Danh_sach += Chuoi_Thong_tin + Chuoi_Thong_tin_sp + Chuoi_Phan_cong;
        });
        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }
    public static string Tao_Chuoi_HTML_Danh_sach_Phieu_dat_Da_Phan_cong(List<XmlElement> Danh_sach_Phieu_dat)
    {
        var Chuoi_HTML_Danh_sach = "<div class='row'>";

        Danh_sach_Phieu_dat.ForEach(Phieu_dat =>
        {
            var Ma_Phieu = Phieu_dat.GetAttribute("Ma_so");
            var Ngay = Phieu_dat.GetAttribute("Ngay");
            var Trang_thai = Phieu_dat.GetAttribute("Trang_thai");
            var Dia_chi = Phieu_dat.SelectSingleNode("Khach_hang/@Dia_chi").Value;
            var Ma_NV_Giao_hang = Phieu_dat.SelectSingleNode("Nhan_vien_Giao_hang/@Ma_so").Value;
            var Chuoi_Thong_tin_sp = "";
            var Tong_tien = 0L;
            foreach (XmlElement San_pham in Phieu_dat.GetElementsByTagName("San_pham"))
            {
                var Ma_San_pham = San_pham.GetAttribute("Ma_so");
                var Ten_San_pham = San_pham.GetAttribute("Ten");
                var Don_gia = San_pham.GetAttribute("Don_gia");
                var So_luong = San_pham.GetAttribute("So_luong");
                var Thanh_tien = San_pham.GetAttribute("Tien");
                Tong_tien += long.Parse(Thanh_tien);
                Chuoi_Thong_tin_sp = Chuoi_Thong_tin_sp +
                          $"<br />Mã Sản phẩm giao: {  Ma_San_pham}" +
                          $"<br />Tên Sản phẩm: {  Ten_San_pham}" +
                          $"<br />Đơn giá bán: {  Don_gia}" +
                          $"<br />Số lượng: {  So_luong}" +
                          $"<br /><i>Thành tiền: { Thanh_tien}</i>";
                          
            }
            var Chuoi_Thong_tin = $"<div class='btn' style='text-align:left; border:1px' > " +
                          $"<br /><b>Mã phiếu: {  Ma_Phieu}</b>" +
                          $"<br />Ngày: {  Ngay}" +
                          $"<br />Trạng thái: {  Trang_thai}" +
                          $"<br />Địa chỉ Khách hàng: {  Dia_chi}";
            var Chuoi_Tong_tien = $"</br><b>Tổng tiền: { Tong_tien}</b>" +
                          $"</div >";

            Chuoi_HTML_Danh_sach += Chuoi_Thong_tin + Chuoi_Thong_tin_sp + Chuoi_Tong_tien ;
            
        });
        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }
}
   
//************************* Business-Layers BL **********************************
public partial class XL_NGHIEP_VU
{
    public static List<XmlElement> Tra_cuu_Phieu_dat(
         string Chuoi_Tra_cuu, List<XmlElement> Danh_sach)
    {
        Chuoi_Tra_cuu = Chuoi_Tra_cuu.ToUpper();
        var Danh_sach_Kq = new List<XmlElement>();
        Danh_sach_Kq = Danh_sach.FindAll(x => x.GetAttribute("Ma_so") == Chuoi_Tra_cuu
                                         || x.GetAttribute("Ngay").Contains(Chuoi_Tra_cuu));
        return Danh_sach_Kq;
    }
    //Tạo danh sách
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
public partial class XL_LUU_TRU
{
    public static string Dia_chi_Dich_vu = "http://localhost:50800";
    static string Dia_chi_Dich_vu_Du_lieu = $"{Dia_chi_Dich_vu}/1-Dich_vu_Giao_tiep/DV_Chinh.cshtml";

    public static XmlElement Doc_Du_lieu()
    {
        var Chuoi_XML = "<Du_lieu />";
        var Xu_ly = new WebClient();
        Xu_ly.Encoding = System.Text.Encoding.UTF8;
        var Tham_so = "Ma_so_Xu_ly=KHOI_DONG_DU_LIEU_QUAN_LY_GIAO_HANG";
        var Dia_chi_Xu_ly = $"{Dia_chi_Dich_vu_Du_lieu}?{Tham_so}";
        try
        {
            var Chuoi_Kq = Xu_ly.DownloadString(Dia_chi_Xu_ly);
            if (Chuoi_Kq.Trim() != "")
                Chuoi_XML = Chuoi_Kq;
        }
        catch (Exception Loi)
        {
            Chuoi_XML = $"<Du_lieu Kq='{Loi.Message}'  />";
        }
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu = Tai_lieu.DocumentElement;
        Du_lieu.SetAttribute("Kq", "OK");

        return Du_lieu;
    }
    // Ghi 
   
    public static string Ghi_Phan_cong(XmlElement Phieu_dat, XmlElement Nhan_vien_Giao_hang)
    {
        var Kq = "OK";

        try
        {
            var Xu_ly = new WebClient();
            Xu_ly.Encoding = System.Text.Encoding.UTF8;
            var Tham_so = $"Ma_so_Xu_ly=GHI_PHAN_CONG&Ma_so_Phieu_dat={Phieu_dat.GetAttribute("Ma_so")}" +
                          $"&Ma_so_Nhan_vien_Giao_hang={Nhan_vien_Giao_hang.GetAttribute("Ma_so")}";
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
        return Kq;

    }
}
