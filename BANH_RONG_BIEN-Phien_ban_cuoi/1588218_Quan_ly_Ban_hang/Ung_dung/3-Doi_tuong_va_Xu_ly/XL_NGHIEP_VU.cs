using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Net;

public class XL_NGHIEP_VU
{
    public static List<XmlElement> Tra_cuu_San_pham(string Chuoi_Tra_cuu, List<XmlElement> Danh_sach_San_pham)
    {
        Chuoi_Tra_cuu = Chuoi_Tra_cuu.ToUpper();
        var Danh_sach_Kq = new List<XmlElement>();
        Danh_sach_Kq = Danh_sach_San_pham.FindAll(x => x.GetAttribute("Ten").ToUpper().Contains(Chuoi_Tra_cuu)
                                                || x.GetAttribute("Ma_so").ToUpper() == (Chuoi_Tra_cuu)
                                                || x.SelectSingleNode("Nhom_San_pham/@Ma_so").Value == Chuoi_Tra_cuu);
        return Danh_sach_Kq;
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
    public static List<XmlElement> Tao_Danh_sach_San_pham_cua_Nhom_San_pham(XmlElement Nhom_San_pham, XmlElement Danh_sach_Tat_ca_San_pham)
    {
        var Danh_sach = new List<XmlElement>();
        var DS_Tat_ca_San_pham = Tao_Danh_sach(Danh_sach_Tat_ca_San_pham, "San_pham");
        Danh_sach = DS_Tat_ca_San_pham.FindAll(
               San_pham => San_pham.SelectSingleNode("Nhom_San_pham/@Ma_so").Value == Nhom_San_pham.GetAttribute("Ma_so"));
        return Danh_sach;
    }
    public static List<XmlElement> Tao_Danh_sach_San_pham_cua_Nhan_vien_Ban_hang(XmlElement Nhan_vien, List<XmlElement> Danh_sach_Tat_ca_San_pham)
    {
        var Danh_sach = new List<XmlElement>();
        var DS_Nhom_San_pham = (XmlElement)Nhan_vien.GetElementsByTagName("Danh_sach_Nhom_San_pham")[0];
        var Danh_sach_Nhom_San_pham = XL_NGHIEP_VU.Tao_Danh_sach(DS_Nhom_San_pham, "Nhom_San_pham");
        Danh_sach_Tat_ca_San_pham.ForEach(San_pham =>
        {
            var Ma_so_Nhom_San_pham = San_pham.SelectSingleNode("Nhom_San_pham/@Ma_so").Value;
            if (Danh_sach_Nhom_San_pham.Any(Nhom_San_pham => Nhom_San_pham.GetAttribute("Ma_so") == Ma_so_Nhom_San_pham))
                Danh_sach.Add(San_pham);
        });
        return Danh_sach;
    }

}
