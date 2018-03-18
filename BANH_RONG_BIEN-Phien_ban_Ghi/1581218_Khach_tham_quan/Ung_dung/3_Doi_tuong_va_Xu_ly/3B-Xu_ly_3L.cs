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
        
    }
    //============= Xử lý Chức năng ========
    
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
        var Chuoi_HTML = "<iframe src='MH_Dat_hang.cshtml'  ></iframe>";
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
    public string Tao_Chuoi_HTML_Ket_qua()
    {
        var Khach_Tham_quan = (XL_KHACH_THAM_QUAN)HttpContext.Current.Session["Khach_Tham_quan"];

        var Chuoi_HTML = $"<div>" +
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
}
//************************* View/Presentation -Layers VL/PL **********************************
public partial class XL_THE_HIEN
{
    public static string Dia_chi_Media = $"{XL_LUU_TRU.Dia_chi_Dich_vu}/Media";
    public static CultureInfo Dinh_dang_VN = CultureInfo.GetCultureInfo("vi-VN");


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
    
     

}