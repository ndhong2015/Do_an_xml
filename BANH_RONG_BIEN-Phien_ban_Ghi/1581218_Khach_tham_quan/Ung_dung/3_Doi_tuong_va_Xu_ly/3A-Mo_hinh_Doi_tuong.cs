﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;



#region "********Khách Tham quan ***********"
// Du_lieu:
//    Danh_sach_San_pham: 
//      * San_pham :Ma_so,Ten,Don_gia_Ban,Don_gia_Nhap,So_luong_Ton,Doanh_thu
//          Nhom_San_pham:Ma_so,Ten
//   Cua_hang:Ma_so,Ten, San_pham, Dia_chi, Email
//     Danh_sach_Nhom_San_pham:
//       * Nhom_San_pham:Ma_so,Ten,So_luong_Ton,Doanh_thu
#endregion

public class XL_KHACH_THAM_QUAN
{
    public List<XmlElement> Danh_sach_San_pham = new List<XmlElement>();
    public List<XmlElement> Danh_sach_Nhom_San_pham = new List<XmlElement>();

    public string Thong_bao = "";
    public List<XmlElement> Danh_sach_San_pham_Xem = new List<XmlElement>();
    public List<XmlElement> Danh_sach_San_pham_Chon = new List<XmlElement>();

}

