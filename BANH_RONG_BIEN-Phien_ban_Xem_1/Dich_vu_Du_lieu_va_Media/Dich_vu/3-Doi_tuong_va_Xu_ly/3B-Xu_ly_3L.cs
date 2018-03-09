using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.IO;
using System.Xml;
using System.Globalization;

 public partial class XL_DICH_VU
{
    static XL_DICH_VU Dich_vu = null;

    XmlElement Du_lieu_Dich_vu = null;
    //========= Khởi động ======
    public static XL_DICH_VU Khoi_dong_Dich_vu()
    {   if ( Dich_vu == null)
        {
            Dich_vu = new XL_DICH_VU();
            Dich_vu.Khoi_dong_Du_lieu_cua_Dich_vu();
        }
            
        return Dich_vu;
    }
    void Khoi_dong_Du_lieu_cua_Dich_vu()
    {
        var Du_lieu_Luu_tru = XL_LUU_TRU.Doc_Du_lieu();
        var Chuoi_XML = Du_lieu_Luu_tru.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
         Du_lieu_Dich_vu = Tai_lieu.DocumentElement;
        var Danh_sach_San_pham = (XmlElement)Du_lieu_Dich_vu.GetElementsByTagName("Danh_sach_San_pham")[0];
        var Cua_hang = (XmlElement)Du_lieu_Dich_vu.GetElementsByTagName("Cua_hang")[0];
        var Danh_sach_Nhom_San_pham = (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nhom_San_pham")[0];
        // ===================== Bổ sung thông tin   =============================== 
        foreach (XmlElement San_pham in Danh_sach_San_pham.GetElementsByTagName("San_pham"))
        {
            var So_luong_Ton = XL_NGHIEP_VU.Tinh_So_luong_Ton_San_pham(San_pham);
            San_pham.SetAttribute("So_luong_Ton", So_luong_Ton.ToString());
            var Doanh_thu = XL_NGHIEP_VU.Tinh_Doanh_thu_San_pham(San_pham, DateTime.Today);
            San_pham.SetAttribute("Doanh_thu", Doanh_thu.ToString());
        }
        foreach (XmlElement Nhom_San_pham in Danh_sach_Nhom_San_pham.GetElementsByTagName("Nhom_San_pham"))
        {
            var Danh_sach_San_pham_cua_Nhom_San_pham = XL_NGHIEP_VU.Tao_Danh_sach_San_pham_cua_Nhom_San_pham(Nhom_San_pham, Danh_sach_San_pham);
            var So_luong_Ton = XL_NGHIEP_VU.Tinh_So_luong_Ton_Danh_sach_San_pham(Danh_sach_San_pham_cua_Nhom_San_pham);
            Nhom_San_pham.SetAttribute("So_luong_Ton", So_luong_Ton.ToString());
            var Doanh_thu = XL_NGHIEP_VU.Tinh_Doanh_thu_Danh_sach_San_pham(Danh_sach_San_pham_cua_Nhom_San_pham, DateTime.Today);
            Nhom_San_pham.SetAttribute("Doanh_thu", Doanh_thu.ToString());
        }
    }
    //====== Tạo Dữ liệu cho các Hệ khách ======
    public XmlElement Tao_Du_lieu_cua_Ung_dung_Khach_Tham_quan()
    {

        var Chuoi_XML = Du_lieu_Dich_vu.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu = Tai_lieu.DocumentElement;
        var Cua_hang = (XmlElement)Du_lieu.GetElementsByTagName("Cua_hang")[0];
        var Danh_sach_Nguoi_dung= (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nguoi_dung")[0];
        Cua_hang.RemoveChild(Danh_sach_Nguoi_dung);

        foreach (XmlElement San_pham in Du_lieu.GetElementsByTagName("San_pham"))
        {
            var Danh_sach_Ban_hang = (XmlElement)San_pham.GetElementsByTagName("Danh_sach_Ban_hang")[0];
            San_pham.RemoveChild(Danh_sach_Ban_hang);
            var Danh_sach_Nhap_hang = (XmlElement)San_pham.GetElementsByTagName("Danh_sach_Nhap_hang")[0];
            San_pham.RemoveChild(Danh_sach_Nhap_hang);
        }// Xóa Tất các  Nhập hàng, Bán hàng 



        return Du_lieu;
    }
    public  XmlElement Tao_Du_lieu_cua_Ung_dung_Nhan_vien_Nhap_hang()
    {
        var Chuoi_XML = Du_lieu_Dich_vu.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu = Tai_lieu.DocumentElement;
        var Cua_hang = (XmlElement)Du_lieu.GetElementsByTagName("Cua_hang")[0];
        var Danh_sach_Nguoi_dung = (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nguoi_dung")[0];
        var DS_Nguoi_dung = XL_NGHIEP_VU.Tao_Danh_sach(Danh_sach_Nguoi_dung, "Nguoi_dung");
        foreach (XmlElement Nguoi_dung in DS_Nguoi_dung)
            if (Nguoi_dung.SelectSingleNode("Nhom_Nguoi_dung/@Ma_so").Value !="NHAP_HANG")
                Danh_sach_Nguoi_dung.RemoveChild(Nguoi_dung);// Xóa Các Người dùng không thuộc Nhóm tương ứng  
        foreach (XmlElement San_pham in Du_lieu.GetElementsByTagName("San_pham"))
        {
            var Danh_sach_Ban_hang = (XmlElement)San_pham.GetElementsByTagName("Danh_sach_Ban_hang")[0];
            San_pham.RemoveChild(Danh_sach_Ban_hang);
            var Danh_sach_Nhap_hang = (XmlElement)San_pham.GetElementsByTagName("Danh_sach_Nhap_hang")[0];
            San_pham.RemoveChild(Danh_sach_Nhap_hang);
        }// Xóa Tất các  Nhập hàng, Bán hàng 


        return Du_lieu;
    }
    public XmlElement Tao_Du_lieu_cua_Ung_dung_Nhan_vien_Ban_hang()
    {
        var Chuoi_XML = Du_lieu_Dich_vu.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu = Tai_lieu.DocumentElement;

        //Bổ sung theo yêu cầu của phân hệ  
        

        return Du_lieu;
    }
    public XmlElement Tao_Du_lieu_cua_Ung_dung_Nhan_vien_Giao_hang()
    {
        var Chuoi_XML = Du_lieu_Dich_vu.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu = Tai_lieu.DocumentElement;

        //Bổ sung theo yêu cầu của phân hệ  


        return Du_lieu;
    }
    public XmlElement Tao_Du_lieu_cua_Ung_dung_Quan_ly_Nhap_hang()
    {
        var Chuoi_XML = Du_lieu_Dich_vu.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu = Tai_lieu.DocumentElement;

        //Bổ sung theo yêu cầu của phân hệ  


        return Du_lieu;
    }
    public XmlElement Tao_Du_lieu_cua_Ung_dung_Quan_ly_Ban_hang()
    {
        var Chuoi_XML = Du_lieu_Dich_vu.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu = Tai_lieu.DocumentElement;

        //Bổ sung theo yêu cầu của phân hệ  


        return Du_lieu;
    }
    public XmlElement Tao_Du_lieu_cua_Ung_dung_Quan_ly_Giao_hang()
    {
        var Chuoi_XML = Du_lieu_Dich_vu.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu = Tai_lieu.DocumentElement;

        //Bổ sung theo yêu cầu của phân hệ  


        return Du_lieu;
    }
    public XmlElement Tao_Du_lieu_cua_Ung_dung_Quan_ly_Cua_hang()
    {
        var Chuoi_XML = Du_lieu_Dich_vu.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu = Tai_lieu.DocumentElement;

        //Bổ sung theo yêu cầu của phân hệ  


        return Du_lieu;
    }


}
//************************* Business-Layers BL **********************************
public partial class XL_NGHIEP_VU
{    
    
    //==== Tính toán ======
    public static long Tinh_So_luong_Ton_San_pham(XmlElement San_pham)
    {
        var Tong_Nhap_hang = 0L;
        foreach (XmlElement Nhap_hang in San_pham.GetElementsByTagName("Nhap_hang"))
            Tong_Nhap_hang += long.Parse(Nhap_hang.GetAttribute("So_luong"));
        var Tong_Ban_hang = 0L;
        foreach (XmlElement Ban_hang in San_pham.GetElementsByTagName("Ban_hang"))
            Tong_Ban_hang += long.Parse(Ban_hang.GetAttribute("So_luong"));

        var So_luong_Ton = Tong_Nhap_hang - Tong_Ban_hang;
        return So_luong_Ton;
    }
    public static long Tinh_So_luong_Ton_Danh_sach_San_pham(List<XmlElement> Danh_sach_San_pham)
    {
        var So_luong_Ton = Danh_sach_San_pham.Sum(San_pham=> Tinh_So_luong_Ton_San_pham(San_pham));     
        return So_luong_Ton;
    }
    public static long Tinh_Doanh_thu_San_pham(XmlElement San_pham, DateTime Ngay)
    {
        var Doanh_thu = 0L;
        foreach (XmlElement Ban_hang in San_pham.GetElementsByTagName("Ban_hang"))
        {
            var Ngay_Ban = DateTime.Parse(Ban_hang.GetAttribute("Ngay"));
            if (Ngay.Day==Ngay_Ban.Day && Ngay.Month == Ngay_Ban.Month && Ngay.Year == Ngay_Ban.Year)
            Doanh_thu += long.Parse(Ban_hang.GetAttribute("Tien"));
        }
        return Doanh_thu;
    }
    public static long Tinh_Doanh_thu_Danh_sach_San_pham(List<XmlElement> Danh_sach_San_pham, DateTime Ngay)
    {
        var Doanh_thu = Danh_sach_San_pham.Sum(San_pham => Tinh_Doanh_thu_San_pham(San_pham,Ngay));
         
        return Doanh_thu;
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
    public static List<XmlElement> Tao_Danh_sach_San_pham_cua_Nhom_San_pham(
            XmlElement Nhom_San_pham, XmlElement Danh_sach_Tat_ca_San_pham)
    {
        var Danh_sach = new List<XmlElement>();
        var DS_Tat_ca_San_pham = Tao_Danh_sach(Danh_sach_Tat_ca_San_pham, "San_pham");
        Danh_sach = DS_Tat_ca_San_pham.FindAll(
               San_pham => San_pham.SelectSingleNode("Nhom_San_pham/@Ma_so").Value == Nhom_San_pham.GetAttribute("Ma_so"));
        return Danh_sach;
    }
}

//************************* Data-Layers DL **********************************
public partial class XL_LUU_TRU
{
    static DirectoryInfo Thu_muc_Project = new DirectoryInfo(HostingEnvironment.ApplicationPhysicalPath);
    static DirectoryInfo Thu_muc_Du_lieu = Thu_muc_Project.GetDirectories("2-Du_lieu_Luu_tru")[0];
    static DirectoryInfo Thu_muc_Cua_hang = Thu_muc_Du_lieu.GetDirectories("Cua_hang")[0];
    static DirectoryInfo Thu_muc_San_pham = Thu_muc_Du_lieu.GetDirectories("San_pham")[0];
    //=== Đọc =======
    public static XmlElement Doc_Du_lieu()
    {
        var Chuoi_XML = "<Du_lieu />";
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu = Tai_lieu.DocumentElement;
        var Cua_hang = Doc_Cua_hang();
        Du_lieu.AppendChild(Tai_lieu.ImportNode(Cua_hang, true));
        var Danh_sach_San_pham = Doc_Danh_sach_San_pham();
        Du_lieu.AppendChild(Tai_lieu.ImportNode(Danh_sach_San_pham, true));

        return Du_lieu;
    }
    static XmlElement Doc_Danh_sach_San_pham()
    {
        var Chuoi_XML_Danh_sach = "<Danh_sach_San_pham />";
        var Tai_lieu_Danh_sach = new XmlDocument();
        Tai_lieu_Danh_sach.LoadXml(Chuoi_XML_Danh_sach);
        var Danh_sach = Tai_lieu_Danh_sach.DocumentElement;
        Thu_muc_San_pham.GetFiles("*.xml").ToList().ForEach(Tap_tin =>
        {
            var Duong_dan = Tap_tin.FullName;
            var Tai_lieu = new XmlDocument();
            Tai_lieu.Load(Duong_dan);
            var San_pham = Tai_lieu.DocumentElement;
            var San_pham_cua_Danh_sach = Tai_lieu_Danh_sach.ImportNode(San_pham, true);
            Danh_sach.AppendChild(San_pham_cua_Danh_sach);
           
        });
        return Danh_sach;
    }
    static XmlElement Doc_Cua_hang()
    {
        var Duong_dan = Thu_muc_Cua_hang.FullName + "\\Cua_hang.xml";
        var Tai_lieu = new XmlDocument();
        Tai_lieu.Load(Duong_dan);
        var Cua_hang = Tai_lieu.DocumentElement;
        return Cua_hang;
    }
     

    //===Ghi ========
    


}