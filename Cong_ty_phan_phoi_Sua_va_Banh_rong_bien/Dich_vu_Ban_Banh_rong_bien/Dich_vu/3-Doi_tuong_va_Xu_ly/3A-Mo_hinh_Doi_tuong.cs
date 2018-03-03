using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

#region "********Dữ liệu Ứng dụng ***********"
// Du_lieu:
//   Cong_ty:Ma_so,Ten
//     Danh_sach_Nhom_San_pham:
//       * Nhom_San_pham:Ma_so,Ten
//     Danh_sach_Nhan_vien:
//       * Nhan_vien :Ma_so,Ho_ten,Ten_Dang_nhap,Mat_khau
//          Nhom_Nhan_vien:Ma_so,Ten
//     Danh_sach_Quan_ly:
//       * Quan_ly :Ma_so,Ho_ten,Ten_Dang_nhap,Mat_khau
//          Nhom_Quan_ly:Ma_so,Ten
//   Danh_sach_San_pham: 
//      * San_pham :Ma_so,Ten,Don_gia_Ban,Don_gia_Nhap
//          Nhom_San_pham:Ma_so,Ten
//          Danh_sach_Ban_hang:
//            * Ban_hang : Ngay,So_luong,Don_gia,Tien, Giao_hang
//          Danh_sach_Nhap_hang:
//            * Nhap_hang : Ngay,So_luong,Don_gia,Tien
#endregion

#region "********Dữ liệu của Phân hệ Khách Tham quan ***********"
//############### Lưu ý :Có bổ sung
//  Số lượng Tồn của Sản phẩm 
//###############
// Du_lieu:
//   Cong_ty:Ma_so,Ten
//   Danh_sach_San_pham: 
//      * San_pham :Ma_so,Ten,Don_gia_Ban,Don_gia_Nhap, So_luong_Ton
#endregion

#region "********Dữ liệu của Phân hệ Nhân viên Bán hàng ***********"
//############### Lưu ý :Có bổ sung
//  Số lượng Tồn,Doanh thu của Sản phẩm 
//###############
// Du_lieu:
//   Cong_ty:Ma_so,Ten
//     Danh_sach_Nhom_San_pham:
//       * Nhom_San_pham:Ma_so,Ten
//     Danh_sach_Nhan_vien:
//       * Nhan_vien :Ma_so,Ho_ten,Ten_Dang_nhap,Mat_khau,Doanh_thu
//          Nhom_Nhan_vien:Ma_so,Ten
//     Danh_sach_Quan_ly:
//       * Quan_ly :Ma_so,Ho_ten,Ten_Dang_nhap,Mat_khau
//          Nhom_Quan_ly:Ma_so,Ten
//   Danh_sach_San_pham: 
//      * San_pham :Ma_so,Ten,Don_gia_Ban,Don_gia_Nhap, So_luong_Ton,Doanh_thu
#endregion

#region "********Dữ liệu của Phân hệ Nhân viên Giao hàng ***********"
//############### Lưu ý :Có bổ sung
// Du_lieu:
//   Cong_ty:Ma_so,Ten
//     Danh_sach_Nhom_San_pham:
//       * Nhom_San_pham:Ma_so,Ten
//     Danh_sach_Nhan_vien:
//       * Nhan_vien :Ma_so,Ho_ten,Ten_Dang_nhap,Mat_khau,Doanh_thu
//          Nhom_Nhan_vien:Ma_so,Ten
//     Danh_sach_Quan_ly:
//       * Quan_ly :Ma_so,Ho_ten,Ten_Dang_nhap,Mat_khau
//          Nhom_Quan_ly:Ma_so,Ten
//   Danh_sach_San_pham: 
//      * San_pham :Ma_so,Ten,Don_gia_Ban,Don_gia_Nhap, So_luong_Ton,Doanh_thu
#endregion

#region "********Dữ liệu của Phân hệ Quản lý Bán hàng ***********"
//############### Lưu ý :Có bổ sung
//  Số lượng Tồn,Doanh thu của Sản phẩm 
//###############
// Du_lieu:
//   Cong_ty:Ma_so,Ten
//     Danh_sach_Nhom_San_pham:
//       * Nhom_San_pham:Ma_so,Ten
//     Danh_sach_Nhan_vien:
//       * Nhan_vien :Ma_so,Ho_ten,Ten_Dang_nhap,Mat_khau,Doanh_thu
//          Nhom_Nhan_vien:Ma_so,Ten
//     Danh_sach_Quan_ly:
//       * Quan_ly :Ma_so,Ho_ten,Ten_Dang_nhap,Mat_khau
//          Nhom_Quan_ly:Ma_so,Ten
//   Danh_sach_San_pham: 
//      * San_pham :Ma_so,Ten,Don_gia_Ban,Don_gia_Nhap, So_luong_Ton,Doanh_thu
#endregion

#region "********Dữ liệu của Phân hệ Quản lý Kho ***********"
//############### Lưu ý :Có bổ sung
//  Số lượng Tồn của Điện thoại 
//###############
// Du_lieu:
//   Cua_hang:Ma_so,Ten
//     Danh_sach_Nhom_Dien_thoai:
//       * Nhom_Dien_thoai:Ma_so,Ten
//     Danh_sach_Nhan_vien:
//       * Nhan_vien :Ma_so,Ho_ten,Ten_Dang_nhap,Mat_khau
//          Nhom_Nhan_vien:Ma_so,Ten
//     Danh_sach_Quan_ly:
//       * Quan_ly :Ma_so,Ho_ten,Ten_Dang_nhap,Mat_khau
//          Nhom_Quan_ly:Ma_so,Ten
//   Danh_sach_Dien_thoai: 
//      * Dien_thoai :Ma_so,Ten,Don_gia_Ban,Don_gia_Nhap, So_luong_Ton
#endregion

#region "********Dữ liệu của Phân hệ Quản lý Công ty ***********"
//############### Lưu ý :Có bổ sung
//  Số lượng Tồn,Doanh thu của Sản phẩm 
//###############
// Du_lieu:
//   Cong_ty:Ma_so,Ten
//     Danh_sach_Nhom_San_pham:
//       * Nhom_San_pham:Ma_so,Ten
//     Danh_sach_Nhan_vien:
//       * Nhan_vien :Ma_so,Ho_ten,Ten_Dang_nhap,Mat_khau,Doanh_thu
//          Nhom_Nhan_vien:Ma_so,Ten
//     Danh_sach_Quan_ly:
//       * Quan_ly :Ma_so,Ho_ten,Ten_Dang_nhap,Mat_khau
//          Nhom_Quan_ly:Ma_so,Ten
//   Danh_sach_San_pham: 
//      * San_pham :Ma_so,Ten,Don_gia_Ban,Don_gia_Nhap, So_luong_Ton,Doanh_thu
#endregion



