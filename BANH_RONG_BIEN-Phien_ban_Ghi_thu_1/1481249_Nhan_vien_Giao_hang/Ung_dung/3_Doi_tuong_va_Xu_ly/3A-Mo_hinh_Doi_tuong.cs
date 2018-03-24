using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
#region "********Nhân viên Giao hàng ***********"
// Du_lieu:
//   Danh_sach_San_pham: 
//      * San_pham :Ma_so,Ten,Don_gia_Ban,Don_gia_Nhap
//          Nhom_San_pham:Ma_so,Ten
//   Cua_hang: Ma_so,Ten
//     Danh_sach_Nguoi_dung:
//       * Nguoi_dung :Ma_so,Ho_ten,Ten_Dang_nhap,Mat_khau
//          Nhom_Nguoi_dung:Ma_so,Ten
//   Phieu_dat: Ma_so, Ngay
//     * Khach_hang: Ho_ten, Dien_thoai, Dia_chi
//       Danh_sach_San_pham:
//         * San_pham: Ma_so, Ten, Don_gia, So_luong, Tien
#endregion

public class XL_NGUOI_DUNG_DANG_NHAP
{
    public string Ho_ten, Ma_so = "";
    public List<XmlElement> Danh_sach_Phieu_dat = new List<XmlElement>();
    
    public string Thong_bao = "";
    public List<XmlElement> Danh_sach_Phieu_dat_Xem = new List<XmlElement>();
    public XmlElement Cua_hang = null;
}
