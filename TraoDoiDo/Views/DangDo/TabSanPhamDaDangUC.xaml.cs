﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes; 
using TraoDoiDo.Utilities;
using TraoDoiDo.ViewModels;

namespace TraoDoiDo.Views.DangDo
{
    /// <summary>
    /// Interaction logic for TabQuanLySanPham.xaml
    /// </summary>
    public partial class TabSanPhamDaDangUC : UserControl
    {
        List<SanPham> dsSanPham;
        List<MoTaAnhSanPham> moTaAnhSanPham;

        SanPham sanPham;
        NguoiDung nguoiDung; //Người đăng 

        TraoDoiDoEntities db = new TraoDoiDoEntities();

        public TabSanPhamDaDangUC()
        {
            InitializeComponent();
        }  

        public TabSanPhamDaDangUC(NguoiDung nguoi)
        {
            InitializeComponent();
            Loaded += QuanLySanPham_Load;
            nguoiDung = nguoi;
        }

        private void QuanLySanPham_Load(object sender, RoutedEventArgs e) //Form load của Tab1
        {
            HienThi_QuanLySanPham();
        }

        private void HienThi_QuanLySanPham()
        {
            lsvQuanLySanPham.Items.Clear();
            dsSanPham = (from sp in db.SanPhams
                         where sp.IdNguoiDang == nguoiDung.IdNguoiDung
                         select sp).ToList();

            foreach (var sanPham in dsSanPham)
            {
                string duongDanDayDu = XuLyAnh.layDuongDanDayDuToiFileAnhSanPham(sanPham.LinkAnhBia);
                lsvQuanLySanPham.Items.Add(new { Id = sanPham.IdSanPham, Ten = sanPham.Ten, LinkAnh = duongDanDayDu, Loai = sanPham.Loai, SoLuong = sanPham.SoLuong, SoLuongDaBan = sanPham.SoLuongDaBan, GiaGoc = sanPham.GiaGoc, GiaBan = sanPham.GiaBan, PhiShip = sanPham.PhiShip, NgayDang = sanPham.NgayDang });
            }
        }

        private void btnThemSanPhamMoi_Click(object sender, RoutedEventArgs e)
        {
            DangDo_Dang f = new DangDo_Dang(nguoiDung);

            f.ucThongTin.txtbIdSanPham.Text = (timIdMaxTrongBangSanPham() + 1).ToString();

            // Load lại lsvQuanLySanPham sau khi (thêm sản phẩm và đóng cái DangDo_Dang)
            f.Closed += (s, ev) =>
            {
                HienThi_QuanLySanPham();
            };
            f.ShowDialog();
        }

        private int timIdMaxTrongBangSanPham()
        { 
            return db.SanPhams.Max(sp => sp.IdSanPham);
        }

        private void btnSuaDo_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button; // Lấy button được click 
            ListViewItem dongChuaButton = HoTroTimPhanTu.FindAncestor<ListViewItem>(btn); // Lấy dòng chứa button 
            dynamic duLieuCuaDongChuaButton = dongChuaButton.DataContext; // Lấy dữ liệu của dong

            if (duLieuCuaDongChuaButton != null)
            {
                //Load thông tin sản phẩm lên 
                SanPham sanPham = db.SanPhams.Find(duLieuCuaDongChuaButton.Id);
                DangDo_Sua f = new DangDo_Sua(sanPham);
                int idsp = Convert.ToInt32(duLieuCuaDongChuaButton.Id);
                moTaAnhSanPham = (from mtasp in db.MoTaAnhSanPhams
                                  join sp in db.SanPhams on mtasp.IdSanPham equals sp.IdSanPham
                                  where sp.IdSanPham == idsp
                                  select mtasp).ToList();

                //Load ảnh và mô tả lên
                foreach (var dong in moTaAnhSanPham)
                {
                    f.DanhSachAnhVaMoTa[f.soLuongAnh] = new ThemAnhKhiDangUC();

                    f.DanhSachAnhVaMoTa[f.soLuongAnh].txtbTenFileAnh.Text = dong.LinkAnhMinhHoa; // Rãnh sửa thuộc tính LinkAnh thành TenFileAnh

                    string duongDanAnh = XuLyAnh.layDuongDanDayDuToiFileAnhSanPham(dong.LinkAnhMinhHoa);
                    f.DanhSachAnhVaMoTa[f.soLuongAnh].txtbDuongDanAnh.Text = duongDanAnh;

                    f.DanhSachAnhVaMoTa[f.soLuongAnh].imgAnhSP.Source = new BitmapImage(new Uri(duongDanAnh));

                    f.DanhSachAnhVaMoTa[f.soLuongAnh].txtbMoTa.Text = dong.MoTa;

                    f.ucThongTin.wpnlChuaAnh.Children.Add(f.DanhSachAnhVaMoTa[f.soLuongAnh]);
                    f.soLuongAnh++;
                }
                f.Closed += (s, ev) =>
                {
                    HienThi_QuanLySanPham();
                };
                f.ShowDialog(); 
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button; // Lấy button được click 
            ListViewItem dongChuaButton = HoTroTimPhanTu.FindAncestor<ListViewItem>(btn); // Lấy dòng chứa button 
            dynamic duLieuCuaDongChuaButton = dongChuaButton.DataContext; // Lấy dữ liệu của dong

            if (duLieuCuaDongChuaButton != null)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa mục đã chọn?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        int idSanPhamMuonXoa = Convert.ToInt32(duLieuCuaDongChuaButton.Id);

                        var dsMoTaAnhSanPhamMuonXoa = (from mtasp in db.MoTaAnhSanPhams
                                                       where mtasp.IdSanPham == idSanPhamMuonXoa
                                                       select mtasp).ToList();
                        db.MoTaAnhSanPhams.RemoveRange(dsMoTaAnhSanPhamMuonXoa);

                        var sanPhamMuonXoa = db.SanPhams.Find(idSanPhamMuonXoa);
                        db.SanPhams.Remove(sanPhamMuonXoa);

                        db.SaveChanges();

                        MessageBox.Show("Xóa thành công");
                        lsvQuanLySanPham.Items.Remove(duLieuCuaDongChuaButton);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xảy ra khi xóa sản phẩm: " + ex.Message);
                    }
                }
            }
        }

        

        private void txbTiemKiem_TextChanged(object sender, TextChangedEventArgs e)
        {
            lsvQuanLySanPham.Items.Clear();
            if (txbTimKiem.Text == null)
                HienThi_QuanLySanPham();
            
            foreach (var sanPham in dsSanPham)
                if(sanPham.Ten.ToLower().Contains(txbTimKiem.Text.Trim().ToLower()))
                {
                    string duongDanDayDu = XuLyAnh.layDuongDanDayDuToiFileAnhSanPham(sanPham.LinkAnhBia);
                    lsvQuanLySanPham.Items.Add(new { Id = sanPham.IdSanPham, Ten = sanPham.Ten, LinkAnh = duongDanDayDu, Loai = sanPham.Loai, SoLuong = sanPham.SoLuong, SoLuongDaBan = sanPham.SoLuongDaBan, GiaGoc = sanPham.GiaGoc, GiaBan = sanPham.GiaBan, PhiShip = sanPham.PhiShip });
                }    
        }

        private void cbLocLoai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            lsvQuanLySanPham.Items.Clear();
            string mucDaChon = (comboBox.SelectedItem as ComboBoxItem).Content.ToString();
            if (string.Equals(mucDaChon, "Tất cả"))
                HienThi_QuanLySanPham();
            else 
                foreach (var sanPham in dsSanPham)
                    if (sanPham.Loai.ToLower().Contains(mucDaChon.Trim().ToLower()))
                    {
                        string tenAnh = XuLyAnh.layDuongDanDayDuToiFileAnhSanPham(sanPham.LinkAnhBia);
                        lsvQuanLySanPham.Items.Add(new { Id = sanPham.IdSanPham, Ten = sanPham.Ten, LinkAnh = tenAnh, Loai = sanPham.Loai, SoLuong = sanPham.SoLuong, SoLuongDaBan = sanPham.SoLuongDaBan, GiaGoc = sanPham.GiaGoc, GiaBan = sanPham.GiaBan, PhiShip = sanPham.PhiShip });
                    }     
        }
    }
} 