using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;


public class XL_KHACH_THAM_QUAN
{
    public string Ma_so, Ho_ten, Dien_thoai, Mail, Dia_chi;
    public List<XmlElement> Danh_sach_San_pham = new List<XmlElement>();
    public List<XmlElement> Danh_sach_Nhom_San_pham = new List<XmlElement>();

    public string Thong_bao = "";
    public List<XmlElement> Danh_sach_San_pham_Xem = new List<XmlElement>();
    public List<XmlElement> Danh_sach_San_pham_Chon = new List<XmlElement>();
    public List<XmlElement> Danh_sach_Phieu_dat = new List<XmlElement>();
}
