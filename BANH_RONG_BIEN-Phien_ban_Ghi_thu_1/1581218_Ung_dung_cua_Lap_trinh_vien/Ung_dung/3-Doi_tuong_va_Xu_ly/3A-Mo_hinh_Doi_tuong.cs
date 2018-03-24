using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

public class XL_CHUC_NANG
{
    public string Ten, Ma_so;
}


//*************************** Đối tượng Dữ liệu   *********
public partial class XL_DU_LIEU
{
    public List<XmlElement> Danh_sach_Nguoi_dung = new List<XmlElement>();
}
//=========== Đối tượng Con người ===============
public class XL_NGUOI_DUNG
{
    public string Ho_ten, Ma_so = "", Ten_Dang_nhap, Mat_khau;
    public List<XmlElement> Nhom_Nguoi_dung = new List<XmlElement>();
}
public class XL_LAP_TRINH_VIEN
{
    public string Ho_ten, Ma_so = "", Ten_Dang_nhap, Mat_khau;
    public List<XmlElement> Danh_sach_Nguoi_dung = new List<XmlElement>();
    public List<XmlElement> Danh_sach_Nguoi_dung_Xem = new List<XmlElement>();
    public List<XmlElement> Danh_sach_Nguoi_dung_Chon = new List<XmlElement>();

    // Chức năng 
    public XL_CHUC_NANG Chuc_nang_Khoi_dong_MH_Chinh = new XL_CHUC_NANG()
    { Ten = "Khởi động", Ma_so = "KHOI_DONG_MH_CHINH" };
    public XL_CHUC_NANG Chuc_nang_Chon_Nguoi_dung = new XL_CHUC_NANG()
    { Ten = "Chọn người dùng", Ma_so = "CHON_NGUOI_DUNG" };
    public XL_CHUC_NANG Chuc_nang_Tra_cuu_Nguoi_dung = new XL_CHUC_NANG()
    { Ten = "Tra cứu người dùng", Ma_so = "TRA_CUU_NGUOI_DUNG" };
 

}
 
//public class XL_NHOM_NGUOI_DUNG
//{
//    public string Ten, Ma_so = "";
//}


 
