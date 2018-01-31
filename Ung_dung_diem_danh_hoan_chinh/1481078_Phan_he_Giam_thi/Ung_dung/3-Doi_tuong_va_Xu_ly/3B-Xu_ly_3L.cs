using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Net;

//************************* View-Layer/Presentation-Layer  VL/PL **********************************
public partial class XL_THE_HIEN
{
    public static string Dia_chi_Media = $"{XL_LUU_TRU.Dia_chi_Dich_vu}/Media";
    public static CultureInfo Dinh_dang_VN = CultureInfo.GetCultureInfo("vi-VN");

    public static string Tao_Chuoi_HTML_Danh_sach_Hoc_sinh(XmlElement Danh_sach)
    {
        var Chuoi_HTML_Danh_sach = "<div class='row'>";
        foreach (XmlElement Hoc_sinh in Danh_sach.GetElementsByTagName("Hoc_sinh"))
        {
            var Ho_ten = Hoc_sinh.GetAttribute("Ho_ten");
            var Ma_so = Hoc_sinh.GetAttribute("Ma_so");
            var CMND = Hoc_sinh.GetAttribute("CMND");
            var Dia_chi = Hoc_sinh.GetAttribute("Dia_chi");
            var Ngay_sinh = DateTime.Parse(Hoc_sinh.GetAttribute("Ngay_sinh"));
            var Lop = Hoc_sinh.GetAttribute("Lop");
            var So_ngay_vang = Hoc_sinh.GetAttribute("So_ngay_vang");
            var Chuoi_Hinh = $"<img src='{Dia_chi_Media}/{Ma_so}.png' " +
                             "style='width:60px;height:60px;' />";
            var Chuoi_Thong_tin = $"<div class='btn' style='text-align:left'> " +
                          $"{Ho_ten}" +
                          $"<br />CMND: {CMND }" +
                          $"<br />Địa chỉ: {Dia_chi}" +
                          $"<br />Ngày sinh: {Ngay_sinh.ToString("d",Dinh_dang_VN)}" +
                          $"<br />{Lop}" +
                          $"<br />Số ngày vắng: {So_ngay_vang}" +
                          $"</div>";
            var Chuoi_Diem_danh = $"<form method='post'>" +
                                 $"<input name='Th_Ma_so_Hoc_sinh' type='hidden' value='{Ma_so}'/>" +
                                 $"Lý do: <input name='Th_Ly_do_Vang' autocomplete='off'" +
                                 $"style='border:none; border-bottom:solid 1px blue; width:30%;'/>" +
                                 $"<button class='btn btn-danger' type='submit'>Điểm danh Vắng</button>" +
                                 $"</form>";
            var Hoc_sinh_Vang = XL_NGHIEP_VU.Kiem_tra_Hoc_sinh_Vang(Hoc_sinh, DateTime.Today);
            var Dinh_dang_Trang_thai = "";
            if(Hoc_sinh_Vang)
            {
                Chuoi_Diem_danh = "";
                Dinh_dang_Trang_thai = "opacity:0.7;";
            }
            var Chuoi_Chi_tiet = "";
            var Danh_sach_Vang = (XmlElement)Hoc_sinh.GetElementsByTagName("Danh_sach_Vang")[0];
            foreach (XmlElement Vang in Danh_sach_Vang.GetElementsByTagName("Vang"))
            {
                var Ngay = Vang.GetAttribute("Ngay");
                var Ly_do = Vang.GetAttribute("Ly_do");
                Chuoi_Chi_tiet += $"<div class='dropdown-item'>{Ngay}, Lý do: {Ly_do}</div>";
            }
            var Chuoi_Chi_tiet_Ngay_vang = $"<div class='dropdown'>" +
                                           $"<button class='btn btn-secondary dropdown-toggle' type='button' id='dropdownMenuButton' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>" +
                                           $"Chi tiết ngày vắng</button>" +
                                           $"<div class='dropdown-menu' aria-labelledby='dropdownMenuButton'>" +
                                           $"{Chuoi_Chi_tiet}" +
                                           $"</div>" +
                                           $"</div>";

            var Chuoi_HTML = $"<div class='col-12' style='margin:10px;>" +
                             $"<div style='{Dinh_dang_Trang_thai}'>" +
                             $"{Chuoi_Hinh}" + $"{Chuoi_Thong_tin}</div>" + 
                             $"{Chuoi_Chi_tiet_Ngay_vang}" + $"<br/>{Chuoi_Diem_danh}" +
                             "</div>";
            Chuoi_HTML_Danh_sach += Chuoi_HTML;
        }
        Chuoi_HTML_Danh_sach += "</div>";
        return Chuoi_HTML_Danh_sach;
    }
        
}

//************************* Business-Layer BL **********************************
public partial class XL_NGHIEP_VU
{
    
    public static bool Kiem_tra_Hoc_sinh_Vang(XmlElement Hoc_sinh, DateTime Ngay)
    {
        var Kq = false;
        var Danh_sach_Vang = (XmlElement)Hoc_sinh.GetElementsByTagName("Danh_sach_Vang")[0];
        foreach(XmlElement Vang in Danh_sach_Vang.GetElementsByTagName("Vang"))
        {
            var Ngay_vang = DateTime.Parse(Vang.GetAttribute("Ngay"));
            if(Ngay_vang.Day==Ngay.Day && Ngay_vang.Month==Ngay.Month&&Ngay_vang.Year==Ngay.Year)
            {
                Kq = true;
                break;
            }
        }
        return Kq;
    }
    //---------------------

    public static XmlElement Tim_Hoc_sinh(string Ma_so, XmlElement Du_lieu)
    {
        var Danh_sach_Hoc_sinh = (XmlElement)Du_lieu.GetElementsByTagName("Danh_sach_Hoc_sinh")[0];
        var Kq = (XmlElement)null;
        foreach (XmlElement Hoc_sinh in Danh_sach_Hoc_sinh.GetElementsByTagName("Hoc_sinh"))
        {
            if (Ma_so == Hoc_sinh.GetAttribute("Ma_so"))
                Kq = Hoc_sinh;
        }
        return Kq;
    }
    public static XmlElement Dang_nhap_Giam_thi(string Ten_Dang_nhap, string Mat_khau,
                            XmlElement Giam_thi)
    {
        var Kq = (XmlElement)null;
        var Ten_Dang_nhap_Giam_thi = Giam_thi.GetAttribute("Ten_Dang_nhap");
        var Mat_khau_Giam_thi = Giam_thi.GetAttribute("Mat_khau");
        if(Ten_Dang_nhap_Giam_thi==Ten_Dang_nhap && Mat_khau_Giam_thi==Mat_khau)
        {
            Kq = Giam_thi;
        }
        return Kq;
    }
    public static XmlElement Tra_cuu_Hoc_sinh(string Chuoi_Tra_cuu, XmlElement Danh_sach_Hoc_sinh)
    {
        Chuoi_Tra_cuu = Chuoi_Tra_cuu.ToUpper();
        var Chuoi_Danh_sach_Kq = "<Danh_sach_Hoc_sinh />";
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_Danh_sach_Kq);
        var Danh_sach_Kq = Tai_lieu.DocumentElement;
        foreach (XmlElement Hoc_sinh in Danh_sach_Hoc_sinh.GetElementsByTagName("Hoc_sinh"))
        {
            var Ten = Hoc_sinh.GetAttribute("Ho_ten");
            if (Ten.ToUpper().Contains(Chuoi_Tra_cuu))
            {
                var Hoc_sinh_Kq = Tai_lieu.ImportNode(Hoc_sinh, true);
                Danh_sach_Kq.AppendChild(Hoc_sinh_Kq);
            }
        }

        return Danh_sach_Kq;
    }

}

//************************* Data-Layer DL **********************************
public partial class XL_LUU_TRU
{
    public static string Dia_chi_Dich_vu = "http://localhost:50800";
    static string Dia_chi_Dich_vu_Du_lieu = $"{Dia_chi_Dich_vu}/1-Dich_vu_Giao_tiep/DV_Chinh.cshtml";

    public static XmlElement Doc_Du_lieu()
    {
        var Chuoi_XML = "<Du_lieu />";
        var Xu_ly = new WebClient();
        Xu_ly.Encoding = System.Text.Encoding.UTF8;
        var Tham_so = "Ma_so_Xu_ly=KHOI_DONG_DU_LIEU_GIAM_THI";
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

    public static string Ghi_Diem_danh(XmlElement Hoc_sinh, XmlElement Vang)
    {
        var Kq = "OK";

        try
        {
            var Xu_ly = new WebClient();
            Xu_ly.Encoding = System.Text.Encoding.UTF8;
            var Tham_so = $"Ma_so_Xu_ly=GHI_DIEM_DANH&Ma_so_Hoc_sinh={Hoc_sinh.GetAttribute("Ma_so")}";
            var Dia_chi_Xu_ly = $"{Dia_chi_Dich_vu_Du_lieu}?{Tham_so}";
            var Chuoi_XML = Vang.OuterXml;
            Kq = Xu_ly.UploadString(Dia_chi_Xu_ly, Chuoi_XML).Trim();
        }
        catch (Exception Loi)
        {
            Kq = Loi.Message;
        }
        if (Kq == "OK")
        {
            var So_ngay_vang = long.Parse(Hoc_sinh.GetAttribute("So_ngay_vang"));
            So_ngay_vang += 1;
            Hoc_sinh.SetAttribute("So_ngay_vang", So_ngay_vang.ToString());
            var Danh_sach_Vang = Hoc_sinh.GetElementsByTagName("Danh_sach_Vang")[0];
            Danh_sach_Vang.AppendChild(Vang);
        }
        return Kq;

    }

    
}