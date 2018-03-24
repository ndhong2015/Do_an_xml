using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.IO;
using System.Xml;
using System.Globalization;

 public partial class XL_DICH_VU
{
    static XL_DICH_VU Dich_vu = null;

    XmlElement Du_lieu_Dich_vu = null;
    //========= Khởi động ======
    public static XL_DICH_VU Khoi_dong_Dich_vu()
    {   if ( Dich_vu == null)
        {
            Dich_vu = new XL_DICH_VU();
            Dich_vu.Khoi_dong_Du_lieu_cua_Dich_vu();
        }
            
        return Dich_vu;
    }
    void Khoi_dong_Du_lieu_cua_Dich_vu()
    {
        var Du_lieu_Luu_tru = XL_LUU_TRU.Doc_Du_lieu();
        var Chuoi_XML = Du_lieu_Luu_tru.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
         Du_lieu_Dich_vu = Tai_lieu.DocumentElement;
        var Danh_sach_San_pham = (XmlElement)Du_lieu_Dich_vu.GetElementsByTagName("Danh_sach_San_pham")[0];
        var Cua_hang = (XmlElement)Du_lieu_Dich_vu.GetElementsByTagName("Cua_hang")[0];
        var Danh_sach_Nhom_San_pham = (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nhom_San_pham")[0];
        var Danh_sach_Nguoi_dung = (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nguoi_dung")[0];
        var Danh_sach_Phieu_dat = (XmlElement)Du_lieu_Dich_vu.GetElementsByTagName("Danh_sach_Phieu_dat")[0];

        // ===================== Bổ sung thông tin   =============================== 
        foreach (XmlElement San_pham in Danh_sach_San_pham.GetElementsByTagName("San_pham"))
        {
            var So_luong_Ton = XL_NGHIEP_VU.Tinh_So_luong_Ton_San_pham(San_pham);
            San_pham.SetAttribute("So_luong_Ton", So_luong_Ton.ToString());
            var Doanh_thu = XL_NGHIEP_VU.Tinh_Doanh_thu_San_pham(San_pham, DateTime.Today);
            San_pham.SetAttribute("Doanh_thu", Doanh_thu.ToString());
        }
        foreach (XmlElement Nhom_San_pham in Danh_sach_Nhom_San_pham.GetElementsByTagName("Nhom_San_pham"))
        {
            var Danh_sach_San_pham_cua_Nhom_San_pham = XL_NGHIEP_VU.Tao_Danh_sach_San_pham_cua_Nhom_San_pham(Nhom_San_pham, Danh_sach_San_pham);
            var So_luong_Ton = XL_NGHIEP_VU.Tinh_So_luong_Ton_Danh_sach_San_pham(Danh_sach_San_pham_cua_Nhom_San_pham);
            Nhom_San_pham.SetAttribute("So_luong_Ton", So_luong_Ton.ToString());
            var Doanh_thu = XL_NGHIEP_VU.Tinh_Doanh_thu_Danh_sach_San_pham(Danh_sach_San_pham_cua_Nhom_San_pham, DateTime.Today);
            Nhom_San_pham.SetAttribute("Doanh_thu", Doanh_thu.ToString());
        }
        foreach (XmlElement Nguoi_dung in Danh_sach_Nguoi_dung.GetElementsByTagName("Nguoi_dung"))
        {
            foreach (XmlElement Nhom_San_pham in Nguoi_dung.SelectNodes("Danh_sach_Nhom_San_pham/Nhom_San_pham"))
            {
                var Danh_sach_San_pham_cua_Nhom_San_pham = XL_NGHIEP_VU.Tao_Danh_sach_San_pham_cua_Nhom_San_pham(Nhom_San_pham, Danh_sach_San_pham);
                var So_luong_Ton = XL_NGHIEP_VU.Tinh_So_luong_Ton_Danh_sach_San_pham(Danh_sach_San_pham_cua_Nhom_San_pham);
                Nhom_San_pham.SetAttribute("So_luong_Ton", So_luong_Ton.ToString());
                var Doanh_thu = XL_NGHIEP_VU.Tinh_Doanh_thu_Danh_sach_San_pham(Danh_sach_San_pham_cua_Nhom_San_pham, DateTime.Today);
                Nhom_San_pham.SetAttribute("Doanh_thu", Doanh_thu.ToString());
            }
        }
    }
    //====== Tạo Dữ liệu cho các Hệ khách ======

    //public XmlElement Tao_Du_lieu_cua_Ung_dung_Quan_ly_Nguoi_dung()
    //{
    //    var Chuoi_XML = Du_lieu_Dich_vu.OuterXml;
    //    var Tai_lieu = new XmlDocument();
    //    Tai_lieu.LoadXml(Chuoi_XML);
    //    var Du_lieu = Tai_lieu.DocumentElement;
    //    var Cua_hang = (XmlElement)Du_lieu.GetElementsByTagName("Cua_hang")[0];
    //    //var Danh_sach_Nguoi_dung = (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nguoi_dung")[0];
    //    //var DS_Nguoi_dung = XL_NGHIEP_VU.Tao_Danh_sach(Danh_sach_Nguoi_dung, "Nguoi_dung");
    //    //foreach (XmlElement Nguoi_dung in DS_Nguoi_dung)
    //    //    if (Nguoi_dung.SelectSingleNode("Nhom_Nguoi_dung/@Ma_so").Value != "NHAP_HANG")
    //    //        Danh_sach_Nguoi_dung.RemoveChild(Nguoi_dung);// Xóa Các Người dùng không thuộc Nhóm tương ứng  
    //    foreach (XmlElement Dien_thoai in Du_lieu.GetElementsByTagName("Dien_thoai"))
    //    {
    //        var Danh_sach_Ban_hang = (XmlElement)Dien_thoai.GetElementsByTagName("Danh_sach_Ban_hang")[0];
    //        Dien_thoai.RemoveChild(Danh_sach_Ban_hang);
    //        var Danh_sach_Nhap_hang = (XmlElement)Dien_thoai.GetElementsByTagName("Danh_sach_Nhap_hang")[0];
    //        Dien_thoai.RemoveChild(Danh_sach_Nhap_hang);
    //    }// Xóa Tất các  Nhập hàng, Bán hàng 


    //    return Du_lieu;
    //}
    public XmlElement Tao_Du_lieu_cua_Ung_dung_Khach_Tham_quan()
    {

        var Chuoi_XML = Du_lieu_Dich_vu.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu = Tai_lieu.DocumentElement;
        var Cua_hang = (XmlElement)Du_lieu.GetElementsByTagName("Cua_hang")[0];
        //var Danh_sach_Nguoi_dung= (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nguoi_dung")[0];
        //Cua_hang.RemoveChild(Danh_sach_Nguoi_dung);
        //var Danh_sach_Phieu_dat = (XmlElement)Du_lieu.GetElementsByTagName("Danh_sach_Phieu_dat")[0];

        foreach (XmlElement San_pham in Du_lieu.GetElementsByTagName("San_pham"))
        {
            if (San_pham.GetElementsByTagName("Danh_sach_Nhap_hang").Count > 0)
            {
                var Danh_sach_Ban_hang = (XmlElement)San_pham.GetElementsByTagName("Danh_sach_Ban_hang")[0];
                San_pham.RemoveChild(Danh_sach_Ban_hang);
                var Danh_sach_Nhap_hang = (XmlElement)San_pham.GetElementsByTagName("Danh_sach_Nhap_hang")[0];
                San_pham.RemoveChild(Danh_sach_Nhap_hang);
            }

        }// Xóa Tất các  Nhập hàng, Bán hàng 



        return Du_lieu;
    }
    public  XmlElement Tao_Du_lieu_cua_Ung_dung_Nhan_vien_Nhap_hang()
    {
        var Chuoi_XML = Du_lieu_Dich_vu.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu = Tai_lieu.DocumentElement;
        var Cua_hang = (XmlElement)Du_lieu.GetElementsByTagName("Cua_hang")[0];
        var Danh_sach_Nguoi_dung = (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nguoi_dung")[0];
        var DS_Nguoi_dung = XL_NGHIEP_VU.Tao_Danh_sach(Danh_sach_Nguoi_dung, "Nguoi_dung");
        foreach (XmlElement Nguoi_dung in DS_Nguoi_dung)
            if (Nguoi_dung.SelectSingleNode("Nhom_Nguoi_dung/@Ma_so").Value !="NHAP_HANG")
                Danh_sach_Nguoi_dung.RemoveChild(Nguoi_dung);// Xóa Các Người dùng không thuộc Nhóm tương ứng  
        foreach (XmlElement San_pham in Du_lieu.GetElementsByTagName("San_pham"))
        {
            if (San_pham.GetElementsByTagName("Danh_sach_Nhap_hang").Count > 0)
            {
                var Danh_sach_Ban_hang = (XmlElement)San_pham.GetElementsByTagName("Danh_sach_Ban_hang")[0];
                San_pham.RemoveChild(Danh_sach_Ban_hang);
                var Danh_sach_Nhap_hang = (XmlElement)San_pham.GetElementsByTagName("Danh_sach_Nhap_hang")[0];
                San_pham.RemoveChild(Danh_sach_Nhap_hang);
            }

        }// Xóa Tất các  Nhập hàng, Bán hàng 


        return Du_lieu;
    }
    public XmlElement Tao_Du_lieu_cua_Ung_dung_Nhan_vien_Ban_hang()
    {
        var Chuoi_XML = Du_lieu_Dich_vu.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu = Tai_lieu.DocumentElement;
        var Cua_hang = (XmlElement)Du_lieu.GetElementsByTagName("Cua_hang")[0];
        var Danh_sach_Nguoi_dung = (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nguoi_dung")[0];
        var DS_Nguoi_dung = XL_NGHIEP_VU.Tao_Danh_sach(Danh_sach_Nguoi_dung, "Nguoi_dung");
        foreach (XmlElement Nguoi_dung in DS_Nguoi_dung)
            if (Nguoi_dung.SelectSingleNode("Nhom_Nguoi_dung/@Ma_so").Value != "BAN_HANG")
                Danh_sach_Nguoi_dung.RemoveChild(Nguoi_dung);// Xóa Các Người dùng không thuộc Nhóm tương ứng  
        foreach (XmlElement San_pham in Du_lieu.GetElementsByTagName("San_pham"))
        {
            if (San_pham.GetElementsByTagName("Danh_sach_Nhap_hang").Count > 0)
            {
                var Danh_sach_Ban_hang = (XmlElement)San_pham.GetElementsByTagName("Danh_sach_Ban_hang")[0];
                San_pham.RemoveChild(Danh_sach_Ban_hang);
                var Danh_sach_Nhap_hang = (XmlElement)San_pham.GetElementsByTagName("Danh_sach_Nhap_hang")[0];
                San_pham.RemoveChild(Danh_sach_Nhap_hang);
            }
                
        }// Xóa Tất các  Nhập hàng, Bán hàng 

        return Du_lieu;
    }
    public XmlElement Tao_Du_lieu_cua_Ung_dung_Nhan_vien_Giao_hang()
    {
         var Chuoi_XML = Du_lieu_Dich_vu.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu = Tai_lieu.DocumentElement;

        var Cua_hang = (XmlElement)Du_lieu.GetElementsByTagName("Cua_hang")[0];
        var Danh_sach_Nguoi_dung = (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nguoi_dung")[0];
        var DS_Nguoi_dung = XL_NGHIEP_VU.Tao_Danh_sach(Danh_sach_Nguoi_dung, "Nguoi_dung");
        foreach (XmlElement Nguoi_dung in DS_Nguoi_dung)
            if (Nguoi_dung.SelectSingleNode("Nhom_Nguoi_dung/@Ma_so").Value != "GIAO_HANG")
                Danh_sach_Nguoi_dung.RemoveChild(Nguoi_dung);
        foreach (XmlElement San_pham in Du_lieu.GetElementsByTagName("San_pham"))
        {
            if (San_pham.GetElementsByTagName("Danh_sach_Nhap_hang").Count > 0)
            {
                var Danh_sach_Ban_hang = (XmlElement)San_pham.GetElementsByTagName("Danh_sach_Ban_hang")[0];
                San_pham.RemoveChild(Danh_sach_Ban_hang);
                var Danh_sach_Nhap_hang = (XmlElement)San_pham.GetElementsByTagName("Danh_sach_Nhap_hang")[0];
                San_pham.RemoveChild(Danh_sach_Nhap_hang);
            }
        }// Xóa Tất các  Nhập hàng, Bán hàng 
        var Danh_sach_Phieu_dat = (XmlElement)Du_lieu.GetElementsByTagName("Danh_sach_Phieu_dat")[0];
        var DS_Phieu_dat = XL_NGHIEP_VU.Tao_Danh_sach(Danh_sach_Phieu_dat, "PHIEU_DAT");
        foreach (XmlElement Phieu_dat in DS_Phieu_dat)
            if (Phieu_dat.SelectSingleNode("@Trang_thai").Value != "CHO_GIAO_HANG")
                Danh_sach_Phieu_dat.RemoveChild(Phieu_dat);
        return Du_lieu;
    }
    public XmlElement Tao_Du_lieu_cua_Ung_dung_Quan_ly_Nhap_hang()
    {
        var Chuoi_XML = Du_lieu_Dich_vu.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu = Tai_lieu.DocumentElement;

        return Du_lieu;
    }
    public XmlElement Tao_Du_lieu_cua_Ung_dung_Quan_ly_Ban_hang()
    {
        var Chuoi_XML = Du_lieu_Dich_vu.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu = Tai_lieu.DocumentElement;
        var Cua_hang = (XmlElement)Du_lieu.GetElementsByTagName("Cua_hang")[0];
        var Danh_sach_Nguoi_dung = (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nguoi_dung")[0];
        var DS_Nguoi_dung = XL_NGHIEP_VU.Tao_Danh_sach(Danh_sach_Nguoi_dung, "Nguoi_dung");
        foreach (XmlElement Nguoi_dung in DS_Nguoi_dung)
            if (Nguoi_dung.SelectSingleNode("Nhom_Nguoi_dung/@Ma_so").Value != "QUAN_LY_BAN_HANG" &&
                Nguoi_dung.SelectSingleNode("Nhom_Nguoi_dung/@Ma_so").Value != "BAN_HANG")
                Danh_sach_Nguoi_dung.RemoveChild(Nguoi_dung);// Xóa Các Người dùng không thuộc Nhóm tương ứng  

        var Danh_sach_Phieu_dat = (XmlElement)Du_lieu.GetElementsByTagName("Danh_sach_Phieu_dat")[0];
        var DS_Phieu_dat = XL_NGHIEP_VU.Tao_Danh_sach(Danh_sach_Phieu_dat, "PHIEU_DAT");
        foreach (XmlElement Phieu_dat in DS_Phieu_dat)
            if (Phieu_dat.SelectSingleNode("@Trang_thai").Value == "DA_GIAO_HANG")
                Danh_sach_Phieu_dat.RemoveChild(Phieu_dat);// Xóa Các Phiếu đặt đã giao hàng  
        foreach (XmlElement San_pham in Du_lieu.GetElementsByTagName("San_pham"))
        {
            if (San_pham.GetElementsByTagName("Danh_sach_Nhap_hang").Count > 0)
            {
                var Danh_sach_Ban_hang = (XmlElement)San_pham.GetElementsByTagName("Danh_sach_Ban_hang")[0];
                San_pham.RemoveChild(Danh_sach_Ban_hang);
                var Danh_sach_Nhap_hang = (XmlElement)San_pham.GetElementsByTagName("Danh_sach_Nhap_hang")[0];
                San_pham.RemoveChild(Danh_sach_Nhap_hang);
            }
        }// Xóa Tất các  Nhập hàng, Bán hàng 

        return Du_lieu;
    }
    public XmlElement Tao_Du_lieu_cua_Ung_dung_Quan_ly_Giao_hang()
    {
         var Chuoi_XML = Du_lieu_Dich_vu.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu = Tai_lieu.DocumentElement;

        var Cua_hang = (XmlElement)Du_lieu.GetElementsByTagName("Cua_hang")[0];
        var Danh_sach_Nguoi_dung = (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nguoi_dung")[0];
        var DS_Nguoi_dung = XL_NGHIEP_VU.Tao_Danh_sach(Danh_sach_Nguoi_dung, "Nguoi_dung");
        foreach (XmlElement Nguoi_dung in DS_Nguoi_dung)
            if (Nguoi_dung.SelectSingleNode("Nhom_Nguoi_dung/@Ma_so").Value != "QUAN_LY_GIAO_HANG" &&
                Nguoi_dung.SelectSingleNode("Nhom_Nguoi_dung/@Ma_so").Value != "GIAO_HANG")
                Danh_sach_Nguoi_dung.RemoveChild(Nguoi_dung);// Xóa Các Người dùng không thuộc Nhóm tương ứng  
        foreach (XmlElement San_pham in Du_lieu.GetElementsByTagName("San_pham"))
        {
            if (San_pham.GetElementsByTagName("Danh_sach_Nhap_hang").Count > 0)
            {
                var Danh_sach_Ban_hang = (XmlElement)San_pham.GetElementsByTagName("Danh_sach_Ban_hang")[0];
                San_pham.RemoveChild(Danh_sach_Ban_hang);
                var Danh_sach_Nhap_hang = (XmlElement)San_pham.GetElementsByTagName("Danh_sach_Nhap_hang")[0];
                San_pham.RemoveChild(Danh_sach_Nhap_hang);
            }
        }// Xóa Tất các  Nhập hàng, Bán hàng 
        var Danh_sach_Phieu_dat = (XmlElement)Du_lieu.GetElementsByTagName("Danh_sach_Phieu_dat")[0];
        var DS_Phieu_dat = XL_NGHIEP_VU.Tao_Danh_sach(Danh_sach_Phieu_dat, "PHIEU_DAT");
        foreach (XmlElement Phieu_dat in DS_Phieu_dat)
            if (Phieu_dat.SelectSingleNode("@Trang_thai").Value == "DA_GIAO_HANG")
                Danh_sach_Phieu_dat.RemoveChild(Phieu_dat);
        
        return Du_lieu;
    }
    public XmlElement Tao_Du_lieu_cua_Ung_dung_Quan_ly_Cua_hang()
    {
        var Chuoi_XML = Du_lieu_Dich_vu.OuterXml;
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu = Tai_lieu.DocumentElement;

        //Bổ sung theo yêu cầu của phân hệ  


        return Du_lieu;
    }
    public string Ghi_Nhap_hang_Moi(string Ma_so_San_pham, string Chuoi_Xml_Nhap_hang)
    {
        var San_pham = XL_NGHIEP_VU.Tim_San_pham(Ma_so_San_pham, Du_lieu_Dich_vu);
        var Nhap_hang = XL_NGHIEP_VU.Tao_Doi_tuong_Con(Chuoi_Xml_Nhap_hang, San_pham);
        var Chuoi_Kq_Ghi = XL_LUU_TRU.Ghi_Nhap_hang_Moi(San_pham, Nhap_hang);
        if (Chuoi_Kq_Ghi == "OK")
        {
            var So_luong_Ton = XL_NGHIEP_VU.Tinh_So_luong_Ton_San_pham(San_pham);
            San_pham.SetAttribute("So_luong_Ton", So_luong_Ton.ToString());
        }
        var Chuoi_Xml_Kq = $"<DU_LIEU Kq='{Chuoi_Kq_Ghi}' />";
        return Chuoi_Xml_Kq;
    }
    public string Ghi_Phan_cong(string Ma_so_Phieu_dat, string Chuoi_Xml_NV_Giao_hang)
    {
        var Phieu_dat = XL_NGHIEP_VU.Tim_Phieu_dat(Ma_so_Phieu_dat, Du_lieu_Dich_vu);
        var Nhan_vien_Giao_hang = XL_NGHIEP_VU.Tao_Doi_tuong_Con(Chuoi_Xml_NV_Giao_hang, Phieu_dat);
        var Chuoi_Kq_Ghi = XL_LUU_TRU.Ghi_Phan_cong(Phieu_dat, Nhan_vien_Giao_hang);
        if (Chuoi_Kq_Ghi == "OK")
        {
            Phieu_dat.SetAttribute("Trang_thai", "CHO_GIAO_HANG");
        }
        var Chuoi_Xml_Kq = $"<DU_LIEU Kq='{Chuoi_Kq_Ghi}' />";
        return Chuoi_Xml_Kq;
    }
    public string Ghi_Phieu_Dat_moi(string Ma_so_Phieu_dat, string Chuoi_Xml_Phieu_dat)
    {        
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_Xml_Phieu_dat);
        var Phieu_dat = Tai_lieu.DocumentElement;
        var Chuoi_Kq_Ghi = XL_LUU_TRU.Ghi_Phieu_Dat_moi(Phieu_dat);     
            var Chuoi_Xml_Kq = $"<DU_LIEU Kq='{Chuoi_Kq_Ghi}' />";
        return Chuoi_Xml_Kq;
    }
    public string Ghi_Giao_hang(string Ma_so_Phieu_dat)
    {
        var Phieu_dat = XL_NGHIEP_VU.Tim_Phieu_dat(Ma_so_Phieu_dat, Du_lieu_Dich_vu);
        
        var Chuoi_Kq_Ghi = XL_LUU_TRU.Ghi_Giao_hang(Phieu_dat);
        if (Chuoi_Kq_Ghi == "OK")
        {
            Phieu_dat.SetAttribute("Trang_thai", "DA_GIAO_HANG");
        }
        var Chuoi_Xml_Kq = $"<DU_LIEU Kq='{Chuoi_Kq_Ghi}' />";
        return Chuoi_Xml_Kq;
    }
    public string Ghi_Ban_hang_Moi(string Ma_so_San_pham, string Chuoi_Xml_Ban_hang)
    {
        var San_pham = XL_NGHIEP_VU.Tim_San_pham(Ma_so_San_pham, Du_lieu_Dich_vu);
        var Ban_hang = XL_NGHIEP_VU.Tao_Doi_tuong_Con(Chuoi_Xml_Ban_hang, San_pham);
        var Chuoi_Kq_Ghi = XL_LUU_TRU.Ghi_Ban_hang_Moi(San_pham, Ban_hang);
        if (Chuoi_Kq_Ghi == "OK")
        {
            var So_luong_Ton = XL_NGHIEP_VU.Tinh_So_luong_Ton_San_pham(San_pham);
            San_pham.SetAttribute("So_luong_Ton", So_luong_Ton.ToString());
            var Doanh_thu = XL_NGHIEP_VU.Tinh_Doanh_thu_San_pham(San_pham, DateTime.Today);
            San_pham.SetAttribute("Doanh_thu", Doanh_thu.ToString());

            //Cập nhật Doanh thu Nhóm Sản phẩm
            var Cua_hang = (XmlElement)Du_lieu_Dich_vu.GetElementsByTagName("Cua_hang")[0];
            var Danh_sach_Nguoi_dung = (XmlElement)Cua_hang.GetElementsByTagName("Danh_sach_Nguoi_dung")[0];
            var DS_Nhom_San_pham_cua_Nguoi_dung = (XmlElement)Danh_sach_Nguoi_dung.GetElementsByTagName("Danh_sach_Nhom_San_pham")[0];
            var Danh_sach_Nhom_San_pham_cua_Nguoi_dung = XL_NGHIEP_VU.Tao_Danh_sach(DS_Nhom_San_pham_cua_Nguoi_dung, "Nhom_San_pham");
            var Ma_so_Nhom_San_pham = San_pham.SelectSingleNode("Nhom_San_pham/@Ma_so").Value;
            var Nhom_San_pham_Nguoi_dung = Danh_sach_Nhom_San_pham_cua_Nguoi_dung.FirstOrDefault(x => x.GetAttribute("Ma_so") == Ma_so_Nhom_San_pham);
            var Tien = long.Parse(Ban_hang.GetAttribute("Tien"));
            var Doanh_thu_Nhom_San_pham = long.Parse(Nhom_San_pham_Nguoi_dung.GetAttribute("Doanh_thu"));
            Doanh_thu_Nhom_San_pham += Tien;
            Nhom_San_pham_Nguoi_dung.SetAttribute("Doanh_thu", Doanh_thu_Nhom_San_pham.ToString());
        }
        var Chuoi_Xml_Kq = $"<DU_LIEU Kq='{Chuoi_Kq_Ghi}' />";
        return Chuoi_Xml_Kq;
    }

    public string Cap_nhat_Don_gia_Ban(string Ma_so_San_pham, string Chuoi_Don_gia_Ban)
    {
        var San_pham = XL_NGHIEP_VU.Tim_San_pham(Ma_so_San_pham, Du_lieu_Dich_vu);
        var Don_gia_Ban = long.Parse(Chuoi_Don_gia_Ban);
        var Chuoi_Kq_Ghi = XL_LUU_TRU.Cap_nhat_Don_gia_Ban(San_pham, Don_gia_Ban);
        var Chuoi_Xml_Kq = $"<DU_LIEU Kq='{Chuoi_Kq_Ghi}' />";
        return Chuoi_Xml_Kq;
    }

}
//************************* Business-Layers BL **********************************
public partial class XL_NGHIEP_VU
{
    public static XmlElement Tim_San_pham(
          string Ma_so, XmlElement Du_lieu)
    {

        var Danh_sach_San_pham = (XmlElement)Du_lieu.GetElementsByTagName("Danh_sach_San_pham")[0];
        var Kq = (XmlElement)null;
        foreach (XmlElement San_pham in Danh_sach_San_pham.GetElementsByTagName("San_pham"))
        {
            if (Ma_so == San_pham.GetAttribute("Ma_so"))
                Kq = San_pham;

        }
        return Kq;
    }
    public static XmlElement Tim_Phieu_dat(
         string Ma_so, XmlElement Du_lieu)
    {

        var Danh_sach_Phieu_dat = (XmlElement)Du_lieu.GetElementsByTagName("Danh_sach_Phieu_dat")[0];
        var Kq = (XmlElement)null;
        foreach (XmlElement Phieu_dat in Danh_sach_Phieu_dat.GetElementsByTagName("PHIEU_DAT"))
        {
            if (Ma_so == Phieu_dat.GetAttribute("Ma_so"))
                Kq = Phieu_dat;
        }
        return Kq;
    }
 
    public static XmlElement Tao_Doi_tuong_Con(string Chuoi_XML, XmlElement Cha)
    {
        var Doi_tuong_Kq = (XmlElement)null;
        try
        {

            var Tai_lieu = new XmlDocument();
            Tai_lieu.LoadXml(Chuoi_XML);
            var Doi_tuong = Tai_lieu.DocumentElement;
            Doi_tuong_Kq = (XmlElement)Cha.OwnerDocument.ImportNode(Doi_tuong, true);
        }
        catch (Exception Loi)
        {

        }
        return Doi_tuong_Kq;
    }
    //==== Tính toán ======
    public static long Tinh_So_luong_Ton_San_pham(XmlElement San_pham)
    {
        var Tong_Nhap_hang = 0L;
        foreach (XmlElement Nhap_hang in San_pham.GetElementsByTagName("Nhap_hang"))
            Tong_Nhap_hang += long.Parse(Nhap_hang.GetAttribute("So_luong"));
        var Tong_Ban_hang = 0L;
        foreach (XmlElement Ban_hang in San_pham.GetElementsByTagName("Ban_hang"))
            Tong_Ban_hang += long.Parse(Ban_hang.GetAttribute("So_luong"));

        var So_luong_Ton = Tong_Nhap_hang - Tong_Ban_hang;
        return So_luong_Ton;
    }
    public static long Tinh_So_luong_Ton_Danh_sach_San_pham(List<XmlElement> Danh_sach_San_pham)
    {
        var So_luong_Ton = Danh_sach_San_pham.Sum(San_pham=> Tinh_So_luong_Ton_San_pham(San_pham));     
        return So_luong_Ton;
    }
    public static long Tinh_Doanh_thu_San_pham(XmlElement San_pham, DateTime Ngay)
    {
        var Doanh_thu = 0L;
        foreach (XmlElement Ban_hang in San_pham.GetElementsByTagName("Ban_hang"))
        {
            var Ngay_Ban = DateTime.Parse(Ban_hang.GetAttribute("Ngay"));
            if (Ngay.Day==Ngay_Ban.Day && Ngay.Month == Ngay_Ban.Month && Ngay.Year == Ngay_Ban.Year)
            Doanh_thu += long.Parse(Ban_hang.GetAttribute("Tien"));
        }
        return Doanh_thu;
    }
    public static long Tinh_Doanh_thu_Danh_sach_San_pham(List<XmlElement> Danh_sach_San_pham, DateTime Ngay)
    {
        var Doanh_thu = Danh_sach_San_pham.Sum(San_pham => Tinh_Doanh_thu_San_pham(San_pham,Ngay));
         
        return Doanh_thu;
    }
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
    public static List<XmlElement> Tao_Danh_sach_San_pham_cua_Nhom_San_pham(
            XmlElement Nhom_San_pham, XmlElement Danh_sach_Tat_ca_San_pham)
    {
        var Danh_sach = new List<XmlElement>();
        var DS_Tat_ca_San_pham = Tao_Danh_sach(Danh_sach_Tat_ca_San_pham, "San_pham");
        Danh_sach = DS_Tat_ca_San_pham.FindAll(
               San_pham => San_pham.SelectSingleNode("Nhom_San_pham/@Ma_so").Value == Nhom_San_pham.GetAttribute("Ma_so"));
        return Danh_sach;
    }
}

//************************* Data-Layers DL **********************************
public partial class XL_LUU_TRU
{
    static DirectoryInfo Thu_muc_Project = new DirectoryInfo(HostingEnvironment.ApplicationPhysicalPath);
    static DirectoryInfo Thu_muc_Du_lieu = Thu_muc_Project.GetDirectories("2-Du_lieu_Luu_tru")[0];
    static DirectoryInfo Thu_muc_Cua_hang = Thu_muc_Du_lieu.GetDirectories("Cua_hang")[0];
    static DirectoryInfo Thu_muc_San_pham = Thu_muc_Du_lieu.GetDirectories("San_pham")[0];
    static DirectoryInfo Thu_muc_Phieu_dat = Thu_muc_Du_lieu.GetDirectories("Phieu_dat")[0];  //Bổ sung cho Phân hệ Nhân viên Giao hàng
    //=== Đọc =======
    public static XmlElement Doc_Du_lieu()
    {
        var Chuoi_XML = "<Du_lieu />";
        var Tai_lieu = new XmlDocument();
        Tai_lieu.LoadXml(Chuoi_XML);
        var Du_lieu = Tai_lieu.DocumentElement;
        var Cua_hang = Doc_Cua_hang();
        Du_lieu.AppendChild(Tai_lieu.ImportNode(Cua_hang, true));
        var Danh_sach_San_pham = Doc_Danh_sach_San_pham();
        Du_lieu.AppendChild(Tai_lieu.ImportNode(Danh_sach_San_pham, true));
        var Danh_sach_Phieu_dat = Doc_Danh_sach_Phieu_dat();         //Bổ sung cho Phân hệ Nhân viên Giao hàng
        Du_lieu.AppendChild(Tai_lieu.ImportNode(Danh_sach_Phieu_dat, true));   //Bổ sung cho Phân hệ Nhân viên Giao hàng

        return Du_lieu;
    }
    static XmlElement Doc_Danh_sach_San_pham()
    {
        var Chuoi_XML_Danh_sach = "<Danh_sach_San_pham />";
        var Tai_lieu_Danh_sach = new XmlDocument();
        Tai_lieu_Danh_sach.LoadXml(Chuoi_XML_Danh_sach);
        var Danh_sach = Tai_lieu_Danh_sach.DocumentElement;
        Thu_muc_San_pham.GetFiles("*.xml").ToList().ForEach(Tap_tin =>
        {
            var Duong_dan = Tap_tin.FullName;
            var Tai_lieu = new XmlDocument();
            Tai_lieu.Load(Duong_dan);
            var San_pham = Tai_lieu.DocumentElement;
            var San_pham_cua_Danh_sach = Tai_lieu_Danh_sach.ImportNode(San_pham, true);
            Danh_sach.AppendChild(San_pham_cua_Danh_sach);
           
        });
        return Danh_sach;
    }
    static XmlElement Doc_Cua_hang()
    {
        var Duong_dan = Thu_muc_Cua_hang.FullName + "\\Cua_hang.xml";
        var Tai_lieu = new XmlDocument();
        Tai_lieu.Load(Duong_dan);
        var Cua_hang = Tai_lieu.DocumentElement;
        return Cua_hang;
    }
 
   static XmlElement Doc_Danh_sach_Phieu_dat()
    {
        var Chuoi_XML_Danh_sach = "<Danh_sach_Phieu_dat/>";
        var Tai_lieu_Danh_sach = new XmlDocument();
        Tai_lieu_Danh_sach.LoadXml(Chuoi_XML_Danh_sach);
        var Danh_sach = Tai_lieu_Danh_sach.DocumentElement;
        Thu_muc_Phieu_dat.GetFiles("*.xml").ToList().ForEach(Tap_tin =>
        {
            var Duong_dan = Tap_tin.FullName;
            var Tai_lieu = new XmlDocument();
            Tai_lieu.Load(Duong_dan);
            var Phieu_dat = Tai_lieu.DocumentElement;
            var Phieu_dat_cua_Danh_sach = Tai_lieu_Danh_sach.ImportNode(Phieu_dat, true);
            Danh_sach.AppendChild(Phieu_dat_cua_Danh_sach);
        });
        return Danh_sach;
    }

    //===Ghi ========
    public static string Ghi_Nhap_hang_Moi(XmlElement San_pham, XmlElement Nhap_hang)
    {
        var Kq = "";

        try
        {

            var Danh_sach_Nhap_hang = San_pham.GetElementsByTagName("Danh_sach_Nhap_hang")[0];
            Danh_sach_Nhap_hang.AppendChild(Nhap_hang);
            var Duong_dan = Thu_muc_San_pham.FullName + $"\\{San_pham.GetAttribute("Ma_so")}.xml";
            var Chuoi_XML = San_pham.OuterXml;
            File.WriteAllText(Duong_dan, Chuoi_XML);
            Kq = "OK";
        }
        catch (Exception Loi)
        {
            Kq = Loi.Message;
        }
        if (Kq != "OK" && San_pham != null && Nhap_hang != null)
        {
            var Danh_sach_Nhap_hang = San_pham.GetElementsByTagName("Danh_sach_Nhap_hang")[0];
            Danh_sach_Nhap_hang.RemoveChild(Nhap_hang);
        }
        return Kq;

    }
    
    public static string Ghi_Ban_hang_Moi(XmlElement San_pham, XmlElement Ban_hang)
    {
        var Kq = "";

        try
        {

            var Danh_sach_Ban_hang = San_pham.GetElementsByTagName("Danh_sach_Ban_hang")[0];
            Danh_sach_Ban_hang.AppendChild(Ban_hang);
            var Duong_dan = Thu_muc_San_pham.FullName + $"\\{San_pham.GetAttribute("Ma_so")}.xml";
            var Chuoi_XML = San_pham.OuterXml;
            File.WriteAllText(Duong_dan, Chuoi_XML);
            Kq = "OK";
        }
        catch (Exception Loi)
        {
            Kq = Loi.Message;
        }
        if (Kq != "OK" && San_pham != null && Ban_hang != null)
        {
            var Danh_sach_Ban_hang = San_pham.GetElementsByTagName("Danh_sach_Ban_hang")[0];
            Danh_sach_Ban_hang.RemoveChild(Ban_hang);
        }
        return Kq;

    }
    public static string Ghi_Phieu_Dat_moi(XmlElement Phieu_dat)
    {
        var Kq = "";
        var Duong_dan = $"{Thu_muc_Phieu_dat.FullName}\\{Phieu_dat.GetAttribute("Ma_so")}.xml";        
        try
        {
            var Chuoi_XML = Phieu_dat.OuterXml;
            File.WriteAllText(Duong_dan, Chuoi_XML);
            Kq = "OK";
        }
        catch (Exception Loi)
        {
            Kq = Loi.Message;
        }
        if (Kq != "OK")
        {
            var DS_Phieu_dat = Phieu_dat.GetElementsByTagName("PHIEU_DAT")[0];//Can xem lai
            DS_Phieu_dat.RemoveChild(Phieu_dat);
        }
        return Kq;

    }
    public static string Ghi_Phan_cong(XmlElement Phieu_dat, XmlElement Nhan_vien_Giao_hang)
    {
        var Kq = "";

        try
        {
            var DS_Phieu_dat = Phieu_dat.GetElementsByTagName("PHIEU_DAT")[0];//Can xem lai
            DS_Phieu_dat.AppendChild(Nhan_vien_Giao_hang);
            var Duong_dan = Thu_muc_Phieu_dat.FullName + $"\\{Phieu_dat.GetAttribute("Ma_so")}.xml";
            var Chuoi_XML = Phieu_dat.OuterXml;
            File.WriteAllText(Duong_dan, Chuoi_XML);
            Kq = "OK";
        }
        catch (Exception Loi)
        {
            Kq = Loi.Message;
        }
        if (Kq != "OK" && Phieu_dat != null && Nhan_vien_Giao_hang != null)
        {
            var DS_Phieu_dat = Phieu_dat.GetElementsByTagName("PHIEU_DAT")[0];//Can xem lai
            DS_Phieu_dat.RemoveChild(Nhan_vien_Giao_hang);
        }
        return Kq;

    }
    public static string Ghi_Giao_hang(XmlElement Phieu_dat)
    {
        var Kq = "";

        try
        {
            var Duong_dan = Thu_muc_Phieu_dat.FullName + $"\\{Phieu_dat.GetAttribute("Ma_so")}.xml";
            var Chuoi_XML = Phieu_dat.OuterXml;
            File.WriteAllText(Duong_dan, Chuoi_XML);
            Kq = "OK";
        }
        catch (Exception Loi)
        {
            Kq = Loi.Message;
        }
        return Kq;

    }
    public static string Cap_nhat_Don_gia_Ban(XmlElement San_pham, long Don_gia_Ban)
    {
        var Kq = "";

        if (San_pham != null)
        {
            var Don_gia_Ban_Goc = 0L;
            var Don_gia_Ban_Goc_Hop_le = long.TryParse(San_pham.GetAttribute("Don_gia_Ban"), out Don_gia_Ban_Goc);
            try
            {
                San_pham.SetAttribute("Don_gia_Ban", Don_gia_Ban.ToString());
                var Duong_dan = Thu_muc_San_pham.FullName + $"\\{San_pham.GetAttribute("Ma_so")}.xml";
                var Chuoi_XML = San_pham.OuterXml;
                File.WriteAllText(Duong_dan, Chuoi_XML);
                Kq = "OK";
            }
            catch (Exception Loi)
            {
                Kq = Loi.Message;
            }
            if (Kq != "OK" && Don_gia_Ban_Goc_Hop_le)
            {
                San_pham.SetAttribute("Don_gia_Ban", Don_gia_Ban_Goc.ToString());
            }
        }

        return Kq;

    }

}
