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
    public static XmlElement Tra_cuu_San_pham(
          string Chuoi_Tra_cuu, XmlElement Danh_sach_San_pham)
    {
        Chuoi_Tra_cuu = Chuoi_Tra_cuu.ToUpper();
        var Chuoi_Danh_sach_Kq = "<Danh_sach_San_pham />";
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_Danh_sach_Kq);
        var Danh_sach_Kq = Tai_lieu.DocumentElement;
        foreach (XmlElement San_pham in Danh_sach_San_pham.GetElementsByTagName("San_pham"))
        {
            var Ten = San_pham.GetAttribute("Ten");
            var Ma_so_Nhom_San_pham = San_pham.SelectSingleNode("Nhom_San_pham/@Ma_so").Value.ToUpper();
            if (Ten.ToUpper().Contains(Chuoi_Tra_cuu) || Ma_so_Nhom_San_pham == Chuoi_Tra_cuu)
            {
                var San_pham_Kq = Tai_lieu.ImportNode(San_pham, true);
                Danh_sach_Kq.AppendChild(San_pham_Kq);
            }
        }


        return Danh_sach_Kq;
    }

}
