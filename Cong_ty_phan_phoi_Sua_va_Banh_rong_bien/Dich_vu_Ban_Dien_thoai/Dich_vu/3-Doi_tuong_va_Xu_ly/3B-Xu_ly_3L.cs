using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.IO;
using System.Xml;
using System.Globalization;

 
//************************* Business-Layers BL **********************************
public partial class XL_NGHIEP_VU
{   // PET : Minh họa Kỹ thuật đơn giản nhất - Không xét tính hiệu quả 
    // ==> Có thể cải tiến 
    public static XmlElement Tim_Dien_thoai(
          string Ma_so, XmlElement Du_lieu)
    {
        var Danh_sach_Dien_thoai =(XmlElement) Du_lieu.GetElementsByTagName("Danh_sach_Dien_thoai")[0];
        var Kq= (XmlElement)null;
        foreach (XmlElement Dien_thoai in Danh_sach_Dien_thoai.GetElementsByTagName("Dien_thoai"))
        {
            if (Ma_so == Dien_thoai.GetAttribute("Ma_so"))
                Kq = Dien_thoai;
             
        }
        return Kq;
    }
    public static XmlElement Tao_Doi_tuong_Con(string Chuoi_XML, XmlElement Cha)
    {
        var Doi_tuong_Kq = (XmlElement)null;
        try
        {        
            var Tai_lieu = new XmlDocument();
            Tai_lieu.LoadXml(Chuoi_XML);
            var Doi_tuong = Tai_lieu.DocumentElement;
            Doi_tuong_Kq = (XmlElement) Cha.OwnerDocument.ImportNode(Doi_tuong, true);
        }
        catch(Exception Loi)
        {

        }
        return Doi_tuong_Kq;
    }
    public static XmlElement Tao_Dien_thoai_moi(string Chuoi_XML, XmlElement Dien_thoai)
    {
        var Doi_tuong_Kq = (XmlElement)null;
        try
        {
            var Tai_lieu = new XmlDocument();
            Tai_lieu.LoadXml(Chuoi_XML);
            var Doi_tuong = Tai_lieu.DocumentElement;
            Dien_thoai.SetAttribute("Don_gia_Ban", Doi_tuong.GetAttribute("Don_gia_Ban").ToString());
            Doi_tuong_Kq = Dien_thoai;
        }
        catch (Exception Loi)
        {
            
        }
        return Doi_tuong_Kq;
    }

    // Tạo Dữ liệu 
    public static XmlElement Tao_Du_lieu_cua_Khach_Tham_quan(XmlElement Du_lieu_Ung_dung)
    {

        var Chuoi_XML = Du_lieu_Ung_dung.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu_He_khach= Tai_lieu.DocumentElement;
        var Cua_hang_He_khach = (XmlElement)Du_lieu_He_khach.GetElementsByTagName("Cua_hang")[0];
        Cua_hang_He_khach.InnerXml = "";
        foreach (XmlElement Dien_thoai_He_khach in Du_lieu_He_khach.SelectNodes("//Dien_thoai"))
        {
            var So_luong_Ton = Tinh_So_luong_Ton_Dien_thoai(Dien_thoai_He_khach);
            Dien_thoai_He_khach.SetAttribute("So_luong_Ton", So_luong_Ton.ToString());
            Dien_thoai_He_khach.InnerXml = "";
        }
        return Du_lieu_He_khach;
    }
    
    public static XmlElement Tao_Du_lieu_cua_Nhan_vien_Nhap_hang(XmlElement Du_lieu_Ung_dung)
    {
        var Chuoi_XML = Du_lieu_Ung_dung.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu_He_khach = Tai_lieu.DocumentElement;
        var Cua_hang_He_khach = (XmlElement)Du_lieu_He_khach.GetElementsByTagName("Cua_hang")[0];
        foreach (XmlElement Dien_thoai_He_khach in Du_lieu_He_khach.SelectNodes("//Dien_thoai"))
        {
            var So_luong_Ton = Tinh_So_luong_Ton_Dien_thoai(Dien_thoai_He_khach);
            Dien_thoai_He_khach.SetAttribute("So_luong_Ton", So_luong_Ton.ToString());
            Dien_thoai_He_khach.InnerXml = "";

        }
        return Du_lieu_He_khach;
    }
    public static XmlElement Tao_Du_lieu_cua_Nhan_vien_Ban_hang(XmlElement Du_lieu_Ung_dung)
    {
        var Chuoi_XML = Du_lieu_Ung_dung.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu_He_khach = Tai_lieu.DocumentElement;
        var Cua_hang_He_khach = (XmlElement)Du_lieu_He_khach.GetElementsByTagName("Cua_hang")[0];
        foreach (XmlElement Dien_thoai_He_khach in Du_lieu_He_khach.SelectNodes("//Dien_thoai"))
        {
            var So_luong_Ton = Tinh_So_luong_Ton_Dien_thoai(Dien_thoai_He_khach);
            Dien_thoai_He_khach.SetAttribute("So_luong_Ton", So_luong_Ton.ToString());
            var Doanh_thu = Tinh_Doanh_thu_Dien_thoai(Dien_thoai_He_khach,DateTime.Today);
            Dien_thoai_He_khach.SetAttribute("Doanh_thu", Doanh_thu.ToString());
            Dien_thoai_He_khach.InnerXml = "";
        }
        return Du_lieu_He_khach;
    }
    public static XmlElement Tao_Du_lieu_cua_Quan_ly_Ban_hang(XmlElement Du_lieu_Ung_dung)
    {
        var Chuoi_XML = Du_lieu_Ung_dung.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu_He_khach = Tai_lieu.DocumentElement;
        var Cua_hang_He_khach = (XmlElement)Du_lieu_He_khach.GetElementsByTagName("Cua_hang")[0];
        foreach (XmlElement Nhom_Dien_thoai_He_khach in Du_lieu_He_khach.SelectNodes("//Nhom_Dien_thoai"))
        {
            var So_luong_Ton_Nhom_Dien_thoai = Tinh_So_luong_Ton_Nhom_Dien_thoai(Nhom_Dien_thoai_He_khach, (XmlElement)Du_lieu_He_khach.GetElementsByTagName("Danh_sach_Dien_thoai")[0]);
            Nhom_Dien_thoai_He_khach.SetAttribute("So_luong_ton_Nhom_Dien_thoai", So_luong_Ton_Nhom_Dien_thoai.ToString());
            var Doanh_thu_ngay_Nhom_Dien_thoai = Tinh_Doanh_thu_ngay_Nhom_Dien_thoai(Nhom_Dien_thoai_He_khach, (XmlElement)Du_lieu_He_khach.GetElementsByTagName("Danh_sach_Dien_thoai")[0]);
            Nhom_Dien_thoai_He_khach.SetAttribute("Doanh_thu_ngay_Nhom_Dien_thoai", Doanh_thu_ngay_Nhom_Dien_thoai.ToString());
            Nhom_Dien_thoai_He_khach.InnerXml = "";
        }
        foreach (XmlElement Dien_thoai_He_khach in Du_lieu_He_khach.SelectNodes("//Dien_thoai"))
        {
            var So_luong_Ton = Tinh_So_luong_Ton_Dien_thoai(Dien_thoai_He_khach);
            Dien_thoai_He_khach.SetAttribute("So_luong_Ton", So_luong_Ton.ToString());
            var Doanh_thu = Tinh_Doanh_thu_Dien_thoai(Dien_thoai_He_khach, DateTime.Today);
            Dien_thoai_He_khach.SetAttribute("Doanh_thu", Doanh_thu.ToString());
            Dien_thoai_He_khach.InnerXml = "";
        }
        return Du_lieu_He_khach;
    }
    public static XmlElement Tao_Du_lieu_cua_Quan_ly_Cua_hang(XmlElement Du_lieu_Ung_dung)
    {
        var Chuoi_XML = Du_lieu_Ung_dung.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu_He_khach = Tai_lieu.DocumentElement;
        var Cua_hang_He_khach = (XmlElement)Du_lieu_He_khach.GetElementsByTagName("Cua_hang")[0];
        var Tong_Doanh_thu_ngay = Tinh_Tong_Doanh_thu_Dien_thoai((XmlElement)Du_lieu_He_khach.GetElementsByTagName("Danh_sach_Dien_thoai")[0]);
        var Tong_so_luong_ton = Tinh_Tong_So_luong_Ton_Dien_thoai((XmlElement)Du_lieu_He_khach.GetElementsByTagName("Danh_sach_Dien_thoai")[0]);
        foreach (XmlElement Nhom_Dien_thoai_He_khach in Du_lieu_He_khach.SelectNodes("//Nhom_Dien_thoai"))
        {
            var So_luong_Ton_Nhom_Dien_thoai = Tinh_So_luong_Ton_Nhom_Dien_thoai(Nhom_Dien_thoai_He_khach, (XmlElement)Du_lieu_He_khach.GetElementsByTagName("Danh_sach_Dien_thoai")[0]);
            Nhom_Dien_thoai_He_khach.SetAttribute("So_luong_ton_Nhom_Dien_thoai", So_luong_Ton_Nhom_Dien_thoai.ToString());
            var Doanh_thu_ngay_Nhom_Dien_thoai = Tinh_Doanh_thu_ngay_Nhom_Dien_thoai(Nhom_Dien_thoai_He_khach, (XmlElement)Du_lieu_He_khach.GetElementsByTagName("Danh_sach_Dien_thoai")[0]);
            Nhom_Dien_thoai_He_khach.SetAttribute("Doanh_thu_ngay_Nhom_Dien_thoai", Doanh_thu_ngay_Nhom_Dien_thoai.ToString());
            Nhom_Dien_thoai_He_khach.InnerXml = "";
        }
        foreach (XmlElement Dien_thoai_He_khach in Du_lieu_He_khach.SelectNodes("//Dien_thoai"))
        {
            var So_luong_Ton = Tinh_So_luong_Ton_Dien_thoai(Dien_thoai_He_khach);
            Dien_thoai_He_khach.SetAttribute("So_luong_Ton", So_luong_Ton.ToString());
            var Doanh_thu = Tinh_Doanh_thu_Dien_thoai(Dien_thoai_He_khach, DateTime.Today);
            Dien_thoai_He_khach.SetAttribute("Doanh_thu", Doanh_thu.ToString());
            Dien_thoai_He_khach.InnerXml = "";
        }
        Cua_hang_He_khach.SetAttribute("Tong_Doanh_thu_ngay", Tong_Doanh_thu_ngay.ToString());
        Cua_hang_He_khach.SetAttribute("Tong_so_luong_ton", Tong_so_luong_ton.ToString());
        return Du_lieu_He_khach;
    }
    //==== Tính toán 
    public static long Tinh_So_luong_Ton_Dien_thoai(XmlElement Dien_thoai)
    {
        var Tong_Nhap_hang = 0L;
        foreach (XmlElement Nhap_hang in Dien_thoai.GetElementsByTagName("Nhap_hang"))
            Tong_Nhap_hang += long.Parse(Nhap_hang.GetAttribute("So_luong"));
        var Tong_Ban_hang = 0L;
        foreach (XmlElement Ban_hang in Dien_thoai.GetElementsByTagName("Ban_hang"))
            Tong_Ban_hang += long.Parse(Ban_hang.GetAttribute("So_luong"));

        var So_luong_Ton = Tong_Nhap_hang - Tong_Ban_hang;
        return So_luong_Ton;
    }
    public static long Tinh_Tong_So_luong_Ton_Dien_thoai(XmlElement Danh_sach_Dien_thoai)
    {
        var Tong_ton = 0L;
        foreach (XmlElement Dien_thoai in Danh_sach_Dien_thoai.GetElementsByTagName("Dien_thoai"))
            Tong_ton += Tinh_So_luong_Ton_Dien_thoai(Dien_thoai);
        return Tong_ton;
    }
    public static long Tinh_So_luong_Ton_Nhom_Dien_thoai(XmlElement Nhom_dien_thoai, XmlElement Danh_sach_dien_thoai)
    {
        var So_ton = 0L;
        var Ten_nhom = Nhom_dien_thoai.GetAttribute("Ma_so");
        foreach (XmlElement Dien_thoai in Danh_sach_dien_thoai.GetElementsByTagName("Dien_thoai"))
        {
            var Nhom_cua_Dien_thoai = (XmlElement)Dien_thoai.GetElementsByTagName("Nhom_Dien_thoai")[0];
            var Ten_Nhom_cua_Dien_thoai = Nhom_cua_Dien_thoai.GetAttribute("Ma_so");
            if (Ten_nhom == Ten_Nhom_cua_Dien_thoai)
            {
                So_ton += Tinh_So_luong_Ton_Dien_thoai(Dien_thoai);
            }
        }
        return So_ton;
    }
    public static long Tinh_Doanh_thu_ngay_Nhom_Dien_thoai(XmlElement Nhom_dien_thoai, XmlElement Danh_sach_dien_thoai)
    {
        var Doanh_thu_ngay_nhom_Dien_thoai = 0L;
        var Ten_nhom = Nhom_dien_thoai.GetAttribute("Ma_so");
        foreach (XmlElement Dien_thoai in Danh_sach_dien_thoai.GetElementsByTagName("Dien_thoai"))
        {
            var Nhom_cua_Dien_thoai = (XmlElement)Dien_thoai.GetElementsByTagName("Nhom_Dien_thoai")[0];
            var Ten_Nhom_cua_Dien_thoai = Nhom_cua_Dien_thoai.GetAttribute("Ma_so");
            if (Ten_nhom == Ten_Nhom_cua_Dien_thoai)
            {
                Doanh_thu_ngay_nhom_Dien_thoai += Tinh_Doanh_thu_Dien_thoai(Dien_thoai, DateTime.Today);
            }
        }
        return Doanh_thu_ngay_nhom_Dien_thoai;
    }
    public static long Tinh_Doanh_thu_Dien_thoai(XmlElement Dien_thoai, DateTime Ngay)
    {
        var Doanh_thu = 0L;
        foreach (XmlElement Ban_hang in Dien_thoai.GetElementsByTagName("Ban_hang"))
        {
            var Ngay_Ban = DateTime.Parse(Ban_hang.GetAttribute("Ngay"));
            if (Ngay.Day==Ngay_Ban.Day && Ngay.Month == Ngay_Ban.Month && Ngay.Year == Ngay_Ban.Year)
            Doanh_thu += long.Parse(Ban_hang.GetAttribute("Tien"));
        }
        return Doanh_thu;
    }
    public static long Tinh_Tong_Doanh_thu_Dien_thoai(XmlElement Danh_sach_Dien_thoai)
    {
        var Doanh_thu = 0L;
        foreach (XmlElement Dien_thoai in Danh_sach_Dien_thoai.GetElementsByTagName("Dien_thoai"))
        {
            Doanh_thu += Tinh_Doanh_thu_Dien_thoai(Dien_thoai, DateTime.Today);
        }
        return Doanh_thu;
    }
}

//************************* Data-Layers DL **********************************
public partial class XL_LUU_TRU
{
    static DirectoryInfo Thu_muc_Project = new DirectoryInfo(HostingEnvironment.ApplicationPhysicalPath);
    static DirectoryInfo Thu_muc_Du_lieu = Thu_muc_Project.GetDirectories("2-Du_lieu_Luu_tru")[0];
    static DirectoryInfo Thu_muc_Cua_hang = Thu_muc_Du_lieu.GetDirectories("Cua_hang")[0];
    static DirectoryInfo Thu_muc_Dien_thoai = Thu_muc_Du_lieu.GetDirectories("Dien_thoai")[0];
    static XmlElement Du_lieu;
    public static XmlElement Doc_Du_lieu()
    {   if (Du_lieu == null)
        {
            var Chuoi_XML = "<Du_lieu />";
            var Tai_lieu = new XmlDocument();
            Tai_lieu.LoadXml(Chuoi_XML);
            Du_lieu = Tai_lieu.DocumentElement;
            var Cua_hang = Doc_Danh_sach_Cua_hang().FirstChild;
            Du_lieu.AppendChild(Tai_lieu.ImportNode(Cua_hang, true));
            var Danh_sach_Dien_thoai = Doc_Danh_sach_Dien_thoai();
            Du_lieu.AppendChild(Tai_lieu.ImportNode(Danh_sach_Dien_thoai, true));
        }
        
        return Du_lieu;
    }
    static XmlElement Doc_Danh_sach_Dien_thoai()
    {
        var Chuoi_XML_Danh_sach = "<Danh_sach_Dien_thoai />";
        var Tai_lieu_Danh_sach = new XmlDocument();
        Tai_lieu_Danh_sach.LoadXml(Chuoi_XML_Danh_sach);
        var Danh_sach = Tai_lieu_Danh_sach.DocumentElement;
        Thu_muc_Dien_thoai.GetFiles("*.xml").ToList().ForEach(Tap_tin =>
        {
            var Duong_dan = Tap_tin.FullName;
            var Tai_lieu = new XmlDocument();
            Tai_lieu.Load(Duong_dan);
            var Dien_thoai = Tai_lieu.DocumentElement;
            var Dien_thoai_cua_Danh_sach = Tai_lieu_Danh_sach.ImportNode(Dien_thoai, true);
            Danh_sach.AppendChild(Dien_thoai_cua_Danh_sach);
        });
        return Danh_sach;
    }
    static XmlElement Doc_Danh_sach_Cua_hang()
    {
        var Chuoi_XML_Danh_sach = "<Danh_sach_Cua_hang />";
        var Tai_lieu_Danh_sach = new XmlDocument();
        Tai_lieu_Danh_sach.LoadXml(Chuoi_XML_Danh_sach);
        var Danh_sach = Tai_lieu_Danh_sach.DocumentElement;
        Thu_muc_Cua_hang.GetFiles("*.xml").ToList().ForEach(Tap_tin =>
        {
            var Duong_dan = Tap_tin.FullName;
            var Tai_lieu = new XmlDocument();
            Tai_lieu.Load(Duong_dan);
            var Cua_hang = Tai_lieu.DocumentElement;
            var Cua_hang_cua_Danh_sach = Tai_lieu_Danh_sach.ImportNode(Cua_hang, true);
            Danh_sach.AppendChild(Cua_hang_cua_Danh_sach);
        });
        return Danh_sach;
    }

    // Ghi 
    public static string Ghi_Nhap_hang_Moi(XmlElement Dien_thoai, XmlElement Nhap_hang)
    {
        var Kq = "";
        
        try
        {
           
            var Danh_sach_Nhap_hang = Dien_thoai.GetElementsByTagName("Danh_sach_Nhap_hang")[0];
            Danh_sach_Nhap_hang.AppendChild(Nhap_hang);
            var Duong_dan = Thu_muc_Dien_thoai.FullName + $"\\{Dien_thoai.GetAttribute("Ma_so")}.xml";
            var Chuoi_XML = Dien_thoai.OuterXml;
            File.WriteAllText(Duong_dan, Chuoi_XML);
            Kq = "OK";
        }
        catch (Exception Loi)
        {
            Kq = Loi.Message;
        }
        if (Kq != "OK" && Dien_thoai !=null && Nhap_hang !=null)
        {
            var Danh_sach_Nhap_hang = Dien_thoai.GetElementsByTagName("Danh_sach_Nhap_hang")[0];
            Danh_sach_Nhap_hang.RemoveChild(Nhap_hang);
        }
        return Kq;

    }
    public static string Ghi_Ban_hang_Moi(XmlElement Dien_thoai, XmlElement Ban_hang)
    {
        var Kq = "";

        try
        {

            var Danh_sach_Ban_hang = Dien_thoai.GetElementsByTagName("Danh_sach_Ban_hang")[0];
            Danh_sach_Ban_hang.AppendChild(Ban_hang);
            var Duong_dan = Thu_muc_Dien_thoai.FullName + $"\\{Dien_thoai.GetAttribute("Ma_so")}.xml";
            var Chuoi_XML = Dien_thoai.OuterXml;
            File.WriteAllText(Duong_dan, Chuoi_XML);
            Kq = "OK";
        }
        catch (Exception Loi)
        {
            Kq = Loi.Message;
        }
        if (Kq != "OK" && Dien_thoai != null && Ban_hang != null)
        {
            var Danh_sach_Ban_hang = Dien_thoai.GetElementsByTagName("Danh_sach_Ban_hang")[0];
            Danh_sach_Ban_hang.RemoveChild(Ban_hang);
        }
        return Kq;
    }
    public static string Cap_nhat_gia_ban(XmlElement Dien_thoai)
    {
        var Kq = "";

        try
        {            
            var Duong_dan = Thu_muc_Dien_thoai.FullName + $"\\{Dien_thoai.GetAttribute("Ma_so")}.xml";
            var Chuoi_XML = Dien_thoai.OuterXml;
            File.WriteAllText(Duong_dan, Chuoi_XML);
            Kq = "OK";
        }
        catch (Exception Loi)
        {
            Kq = Loi.Message;
        }
        return Kq;
    }


}