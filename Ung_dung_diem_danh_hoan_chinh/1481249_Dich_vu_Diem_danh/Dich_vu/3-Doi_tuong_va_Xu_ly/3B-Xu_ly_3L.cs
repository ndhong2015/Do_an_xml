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
    public static XmlElement Tim_Hoc_sinh(
          string Ma_so, XmlElement Du_lieu)
    {
        var Danh_sach_Hoc_sinh =(XmlElement) Du_lieu.GetElementsByTagName("Danh_sach_Hoc_sinh")[0];
        var Kq= (XmlElement)null;
        foreach (XmlElement Hoc_sinh in Danh_sach_Hoc_sinh.GetElementsByTagName("Hoc_sinh"))
        {
            if (Ma_so == Hoc_sinh.GetAttribute("Ma_so"))
                Kq = Hoc_sinh;             
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
            Doi_tuong_Kq = (XmlElement)Cha.OwnerDocument.ImportNode(Doi_tuong, true);
        }
        catch (Exception Loi)
        {

        }
        return Doi_tuong_Kq;
    }
    public static XmlElement Tao_Du_lieu_cua_Hoc_sinh(XmlElement Du_lieu_Ung_dung)
    {
        var Chuoi_XML = Du_lieu_Ung_dung.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu_He_khach = Tai_lieu.DocumentElement;
        var Quan_ly = (XmlElement)Du_lieu_He_khach.GetElementsByTagName("Quan_ly")[0];
        Quan_ly.InnerXml = "";
        foreach (XmlElement Hoc_sinh_He_khach in Du_lieu_He_khach.SelectNodes("//Hoc_sinh"))
        {
            var So_ngay_vang = Tinh_so_ngay_vang(Hoc_sinh_He_khach);      
            Hoc_sinh_He_khach.SetAttribute("So_ngay_vang", So_ngay_vang.ToString());
        }
        return Du_lieu_He_khach;
    }

    public static XmlElement Tao_Du_lieu_cua_Ban_giam_hieu(XmlElement Du_lieu_Ung_dung)
    {
        var Chuoi_XML = Du_lieu_Ung_dung.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu_He_khach = Tai_lieu.DocumentElement;
        foreach (XmlElement Hoc_sinh_He_khach in Du_lieu_He_khach.SelectNodes("//Hoc_sinh"))
        {
            var Ho_ten = Hoc_sinh_He_khach.GetAttribute("Ho_ten");
            var Ma_so = Hoc_sinh_He_khach.GetAttribute("Ma_so");
            var Lop = ((XmlElement)Hoc_sinh_He_khach.GetElementsByTagName("Lop")[0]).GetAttribute("Ten");
            var So_ngay_vang = Tinh_so_ngay_vang(Hoc_sinh_He_khach);
            Hoc_sinh_He_khach.RemoveAll();
            Hoc_sinh_He_khach.SetAttribute("Ho_ten", Ho_ten.ToString());
            Hoc_sinh_He_khach.SetAttribute("Ma_so", Ma_so.ToString());
            Hoc_sinh_He_khach.SetAttribute("Lop", Lop.ToString());
            Hoc_sinh_He_khach.SetAttribute("So_ngay_vang", So_ngay_vang.ToString());
        }
        return Du_lieu_He_khach;        
    }
    public static XmlElement Tao_Du_lieu_cua_Giam_thi(XmlElement Du_lieu_Ung_dung)
    {
        var Chuoi_XML = Du_lieu_Ung_dung.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu_He_khach = Tai_lieu.DocumentElement;
        foreach (XmlElement Hoc_sinh_He_khach in Du_lieu_He_khach.SelectNodes("//Hoc_sinh"))
        {
            var Ho_ten = Hoc_sinh_He_khach.GetAttribute("Ho_ten");
            var Ma_so = Hoc_sinh_He_khach.GetAttribute("Ma_so");
            var CMND = Hoc_sinh_He_khach.GetAttribute("CMND");
            var Dia_chi = Hoc_sinh_He_khach.GetAttribute("Dia_chi");
            var Ngay_sinh = DateTime.Parse(Hoc_sinh_He_khach.GetAttribute("Ngay_sinh"));
            var Lop = ((XmlElement)Hoc_sinh_He_khach.GetElementsByTagName("Lop")[0]).GetAttribute("Ten");
            var Danh_sach_Vang = (XmlElement)Hoc_sinh_He_khach.GetElementsByTagName("Danh_sach_Vang")[0];
            var So_ngay_vang = Tinh_so_ngay_vang(Hoc_sinh_He_khach);
            Hoc_sinh_He_khach.RemoveAll();
            Hoc_sinh_He_khach.SetAttribute("Ho_ten", Ho_ten.ToString());
            Hoc_sinh_He_khach.SetAttribute("Ma_so", Ma_so.ToString());
            Hoc_sinh_He_khach.SetAttribute("CMND", CMND.ToString());
            Hoc_sinh_He_khach.SetAttribute("Dia_chi", Dia_chi.ToString());
            Hoc_sinh_He_khach.SetAttribute("Ngay_sinh", $"{Ngay_sinh.Day}/{Ngay_sinh.Month}/{Ngay_sinh.Year}");
            Hoc_sinh_He_khach.SetAttribute("Lop", Lop.ToString());
            Hoc_sinh_He_khach.SetAttribute("So_ngay_vang", So_ngay_vang.ToString());
            Hoc_sinh_He_khach.AppendChild(Danh_sach_Vang);
        }
        return Du_lieu_He_khach;
    }
    // Tinh toan
    public static long Tinh_so_ngay_vang (XmlElement Hoc_sinh)
    {
        var So_ngay_vang = 0;
        foreach (XmlElement Vang in (XmlElement)Hoc_sinh.GetElementsByTagName("Danh_sach_Vang")[0])
        {
            So_ngay_vang ++;
        }
        return So_ngay_vang;
    }
}

//************************* Data-Layers DL **********************************
public partial class XL_LUU_TRU
{
    static DirectoryInfo Thu_muc_Project = new DirectoryInfo(HostingEnvironment.ApplicationPhysicalPath);
    static DirectoryInfo Thu_muc_Du_lieu = Thu_muc_Project.GetDirectories("2-Du_lieu_Luu_tru")[0];
    static DirectoryInfo Thu_muc_Quan_ly = Thu_muc_Du_lieu.GetDirectories("Quan_ly")[0];
    static DirectoryInfo Thu_muc_Hoc_sinh = Thu_muc_Du_lieu.GetDirectories("Hoc_sinh")[0];
    static XmlElement Du_lieu;
    public static XmlElement Doc_Du_lieu()
    {   if (Du_lieu == null)
        {
            var Chuoi_XML = "<Du_lieu />";
            var Tai_lieu = new XmlDocument();
            Tai_lieu.LoadXml(Chuoi_XML);
            Du_lieu = Tai_lieu.DocumentElement;
            var Quan_ly = Doc_Danh_sach_Quan_ly().FirstChild;
            Du_lieu.AppendChild(Tai_lieu.ImportNode(Quan_ly, true));
            var Danh_sach_Hoc_sinh = Doc_Danh_sach_Hoc_sinh();
            Du_lieu.AppendChild(Tai_lieu.ImportNode(Danh_sach_Hoc_sinh, true));
        }
        
        return Du_lieu;
    }
    static XmlElement Doc_Danh_sach_Hoc_sinh()
    {
        var Chuoi_XML_Danh_sach = "<Danh_sach_Hoc_sinh />";
        var Tai_lieu_Danh_sach = new XmlDocument();
        Tai_lieu_Danh_sach.LoadXml(Chuoi_XML_Danh_sach);
        var Danh_sach = Tai_lieu_Danh_sach.DocumentElement;
        Thu_muc_Hoc_sinh.GetFiles("*.xml").ToList().ForEach(Tap_tin =>
        {
            var Duong_dan = Tap_tin.FullName;
            var Tai_lieu = new XmlDocument();
            Tai_lieu.Load(Duong_dan);
            var Hoc_sinh = Tai_lieu.DocumentElement;
            var Hoc_sinh_cua_Danh_sach = Tai_lieu_Danh_sach.ImportNode(Hoc_sinh, true);
            Danh_sach.AppendChild(Hoc_sinh_cua_Danh_sach);
        });
        return Danh_sach;
    }
    static XmlElement Doc_Danh_sach_Quan_ly()
    {
        var Chuoi_XML_Danh_sach = "<Danh_sach_Quan_ly />";
        var Tai_lieu_Danh_sach = new XmlDocument();
        Tai_lieu_Danh_sach.LoadXml(Chuoi_XML_Danh_sach);
        var Danh_sach = Tai_lieu_Danh_sach.DocumentElement;
        Thu_muc_Quan_ly.GetFiles("*.xml").ToList().ForEach(Tap_tin =>
        {
            var Duong_dan = Tap_tin.FullName;
            var Tai_lieu = new XmlDocument();
            Tai_lieu.Load(Duong_dan);
            var Quan_ly = Tai_lieu.DocumentElement;
            var Quan_ly_cua_Danh_sach = Tai_lieu_Danh_sach.ImportNode(Quan_ly, true);
            Danh_sach.AppendChild(Quan_ly_cua_Danh_sach);
        });
        return Danh_sach;
    }
    // Ghi 
    public static string Ghi_Diem_danh(XmlElement Hoc_sinh, XmlElement Vang)
    {
        var Kq = "";

        try
        {

            var Danh_sach_Vang = Hoc_sinh.GetElementsByTagName("Danh_sach_Vang")[0];
            Danh_sach_Vang.AppendChild(Vang);
            var Duong_dan = Thu_muc_Hoc_sinh.FullName + $"\\{Hoc_sinh.GetAttribute("Ma_so")}.xml";
            var Chuoi_XML = Hoc_sinh.OuterXml;
            File.WriteAllText(Duong_dan, Chuoi_XML);
            Kq = "OK";
        }
        catch (Exception Loi)
        {
            Kq = Loi.Message;
        }
        if (Kq != "OK" && Hoc_sinh != null && Vang != null)
        {
            var Danh_sach_Vang = Hoc_sinh.GetElementsByTagName("Danh_sach_Vang")[0];
            Danh_sach_Vang.RemoveChild(Vang);
        }
        return Kq;

    }
}