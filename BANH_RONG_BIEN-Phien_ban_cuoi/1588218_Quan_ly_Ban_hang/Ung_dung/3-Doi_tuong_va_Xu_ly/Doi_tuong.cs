
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;


public class XL_NGUOI_DUNG_DANG_NHAP
{
    public string Ho_ten, Ma_so = "";
    public XmlElement Cua_hang = null;
    public List<XmlElement> Danh_sach_San_pham = new List<XmlElement>();
    public List<XmlElement> Danh_sach_Nhom_San_pham = new List<XmlElement>();
    public List<XmlElement> Danh_sach_Phieu_dat = new List<XmlElement>();
    public List<XmlElement> Danh_sach_Nhan_vien_Ban_hang = new List<XmlElement>();

    public string Thong_bao = "";
    public List<XmlElement> Danh_sach_San_pham_Xem = new List<XmlElement>();
}