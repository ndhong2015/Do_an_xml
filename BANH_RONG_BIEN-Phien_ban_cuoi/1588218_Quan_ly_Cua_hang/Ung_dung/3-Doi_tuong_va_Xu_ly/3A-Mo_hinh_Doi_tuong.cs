
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

#region "******** Quản lý Bán hàng ***********"
// Du_lieu:
//   Danh_sach_San_pham: 
//      * San_pham :Ma_so,Ten,Don_gia_Ban,Don_gia_Nhap, //So_luong_Ton,Doanh_thu//
//          Nhom_San_pham:Ma_so,Ten
//          Danh_sach_Ban_hang:
//            * Ban_hang : Ngay,So_luong,Don_gia,Tien
//          Danh_sach_Nhap_hang:
//            * Nhap_hang : Ngay,So_luong,Don_gia,Tien
//   Cua_hang: Ma_so,Ten
//     Danh_sach_Nhom_San_pham:
//       * Nhom_San_pham:Ma_so,Ten,//So_luong_Ton,Doanh_thu//
//     Danh_sach_Nguoi_dung:
//       * Nguoi_dung :Ma_so,Ho_ten,Ten_Dang_nhap,Mat_khau
//          Nhom_Nguoi_dung:Ma_so,Ten
//          Danh_sach_Nhom_San_pham:
//              * Nhom_San_pham:Ma_so,Ten
#endregion

public class XL_NGUOI_DUNG_DANG_NHAP
{
    public string Ho_ten, Ma_so = "";
    public XmlElement Cua_hang = null;
    public List<XmlElement> Danh_sach_San_pham = new List<XmlElement>();
    public List<XmlElement> Danh_sach_Nhom_San_pham = new List<XmlElement>();
    public List<XmlElement> Danh_sach_Phieu_dat = new List<XmlElement>();
    public List<XmlElement> Danh_sach_Nhan_vien_Ban_hang = new List<XmlElement>();
    public List<XmlElement> Danh_sach_Nhan_vien_Nhap_hang = new List<XmlElement>();
    public List<XmlElement> Danh_sach_Nhan_vien_Giao_hang = new List<XmlElement>();
    public List<XmlElement> Danh_sach_Quan_ly_Ban_hang = new List<XmlElement>();
    public List<XmlElement> Danh_sach_Quan_ly_Nhap_hang = new List<XmlElement>();
    public List<XmlElement> Danh_sach_Quan_ly_Cua_hang = new List<XmlElement>();
    public List<XmlElement> Danh_sach_San_pham_Xem = new List<XmlElement>();

    public List<XmlElement> Danh_sach_Nguoi_dung_Xem = new List<XmlElement>();
    public List<XmlElement> Danh_sach_Nguoi_dung_Chon = new List<XmlElement>();
    public List<XmlElement> Danh_sach_Nguoi_dung = new List<XmlElement>();

    public string Thong_bao = "";
    

    // Chức năng 
    public XL_CHUC_NANG Chuc_nang_Khoi_dong_MH_Chinh = new XL_CHUC_NANG()
    { Ten = "Khởi động", Ma_so = "KHOI_DONG_MH_CHINH" };
    public XL_CHUC_NANG Chuc_nang_Chon_Nguoi_dung = new XL_CHUC_NANG()
    { Ten = "Chọn người dùng", Ma_so = "CHON_NGUOI_DUNG" };
    public XL_CHUC_NANG Chuc_nang_Tra_cuu_Nguoi_dung = new XL_CHUC_NANG()
    { Ten = "Tra cứu người dùng", Ma_so = "TRA_CUU_NGUOI_DUNG" };
}
public class XL_CHUC_NANG
{
    public string Ten, Ma_so;
}