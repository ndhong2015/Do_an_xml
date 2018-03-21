
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

#region "********Nhân viên Bán hàng ***********"
// Du_lieu:
//   Danh_sach_San_pham: (Chỉ có các sản phẩm mà nhân viên đó bán)
//      * San_pham :Ma_so,Ten,Don_gia_Ban,//So_luong_Ton,Doanh_thu//
//          Nhom_San_pham:Ma_so,Ten
//   Cua_hang: Ma_so,Ten
//     Danh_sach_Nhom_San_pham: (Chỉ nhóm Sản phẩm mà nhân viên đó bán)
//       * Nhom_San_pham:Ma_so,Ten,//So_luong_Ton,Doanh_thu//
//     Danh_sach_Nguoi_dung: (Chỉ có các Nhân viên Bán hàng)
//       * Nguoi_dung :Ma_so,Ho_ten,Ten_Dang_nhap,Mat_khau
//          Nhom_Nguoi_dung:Ma_so,Ten

#endregion

public class XL_NGUOI_DUNG_DANG_NHAP
{
    public string Ho_ten, Ma_so = "";
    public XmlElement Cua_hang = null;
    public List<XmlElement> Danh_sach_San_pham = new List<XmlElement>();
    public List<XmlElement> Danh_sach_Nhom_San_pham = new List<XmlElement>();

    public string Thong_bao = "";
    public List<XmlElement> Danh_sach_San_pham_Xem = new List<XmlElement>();

}