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
