using System;
using System.Collections.Generic;
using System.Linq;
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

namespace TraoDoiDo.Views.QuanLy
{
    /// <summary>
    /// Interaction logic for TabQuanLySanPhamUC.xaml
    /// </summary>
    public partial class TabQuanLySanPhamUC : UserControl
    {
        TraoDoiDoEntities db = new TraoDoiDoEntities();
        List<SanPham> dsSanPham = new List<SanPham>();
        public TabQuanLySanPhamUC()
        {
            InitializeComponent();
            
        }
        public TabQuanLySanPhamUC(NguoiDung nguoiDung)
        {
            InitializeComponent();
            
        }
        
        private void FQuanLySanPham_Loaded(object sender, RoutedEventArgs e)
        {
            HienThi_QuanLySanPham();
        }
        private void HienThi_QuanLySanPham()
        {
            try
            {
                lsvQuanLySanPham.Items.Clear();
                // Load danh sách sản phẩm
                dsSanPham = (from sp in db.SanPham select sp).ToList();
                foreach (var sanPham in dsSanPham)
                {
                    string tenAnh = XuLyAnh.layDuongDanDayDuToiFileAnhSanPham(sanPham.LinkAnhBia);
                    lsvQuanLySanPham.Items.Add(new { Id = sanPham.IdSanPham.ToString(), Ten = sanPham.Ten, LinkAnh = tenAnh, Loai = sanPham.Loai, SoLuong = sanPham.SoLuong, SoLuongDaBan = sanPham.SoLuongDaBan, GiaGoc = sanPham.GiaGoc, GiaBan = sanPham.GiaBan, PhiShip = sanPham.PhiShip, NgayDang = sanPham.NgayDang });

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static int TinhKhoangCachSoNgay(DateTime ngay)
        {
            TimeSpan kc = DateTime.Now.Date - ngay.Date;
            return Math.Abs(kc.Days);
        }
        private void HienThiNgayMuaLau(bool kt)
        {
            lsvQuanLySanPham.Items.Clear();
            // Load danh sách sản phẩm
            List<SanPham> dsSanPham = (from sp in db.SanPham select sp).ToList();
            foreach (var sp in dsSanPham)
            {
                int soNgay = TinhKhoangCachSoNgay(DateTime.ParseExact(sp.NgayDang, "d/M/yyyy", null));
                if (kt)
                {
                    if (soNgay < 100)
                    {
                        string linkAnh = sp.LinkAnhBia;
                        string tenAnh = XuLyAnh.layDuongDanDayDuToiFileAnhSanPham(linkAnh);
                        lsvQuanLySanPham.Items.Add(new { Id = sp.IdSanPham, Ten = sp.Ten, LinkAnh = tenAnh, Loai = sp.Loai, SoLuong = sp.SoLuong, SoLuongDaBan = sp.SoLuongDaBan, GiaGoc = sp.GiaGoc, GiaBan = sp.GiaBan, PhiShip = sp.PhiShip, NgayDang = sp.NgayDang });
                    }
                }
                else
                {
                    if (soNgay >= 100)
                    {
                        string linkAnh = sp.LinkAnhBia;
                        string tenAnh = XuLyAnh.layDuongDanDayDuToiFileAnhSanPham(linkAnh);
                        lsvQuanLySanPham.Items.Add(new { Id = sp.IdSanPham, Ten = sp.Ten, LinkAnh = tenAnh, Loai = sp.Loai, SoLuong = sp.SoLuong, SoLuongDaBan = sp.SoLuongDaBan, GiaGoc = sp.GiaGoc, GiaBan = sp.GiaBan, PhiShip = sp.PhiShip, NgayDang = sp.NgayDang });
                    }
                }

            }
        }
        private void cbLocLoai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            try
            {
                lsvQuanLySanPham.Items.Clear();
                string selectedItemContent = (comboBox.SelectedItem as ComboBoxItem).Content.ToString();
                if (string.Equals(selectedItemContent, "Tất cả"))
                    HienThi_QuanLySanPham();
                else
                {
                    // Load danh sách sản phẩm theo loại 
                    dsSanPham = (from sp in db.SanPham where sp.Loai == selectedItemContent select sp).ToList();
                    foreach (var sanPham in dsSanPham)
                    {
                        string tenAnh = XuLyAnh.layDuongDanDayDuToiFileAnhSanPham(sanPham.LinkAnhBia);
                        lsvQuanLySanPham.Items.Add(new { Id = sanPham.IdSanPham.ToString(), Ten = sanPham.Ten.ToString(), LinkAnh = tenAnh, Loai = sanPham.Loai.ToString(), SoLuong = sanPham.SoLuong.ToString(), SoLuongDaBan = sanPham.SoLuongDaBan.ToString(), GiaGoc = sanPham.GiaGoc.ToString(), GiaBan = sanPham.GiaBan.ToString(), PhiShip = sanPham.PhiShip.ToString(), NgayDang = sanPham.NgayDang });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txbTimKiemSanPham_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                lsvQuanLySanPham.Items.Clear();
                if (string.IsNullOrEmpty(txbTimKiemSanPham.Text))
                    HienThi_QuanLySanPham();
                else
                {
                    string tenSp = txbTimKiemSanPham.Text.Trim();
                    // Load danh sách sản phẩm theo tên
                    List<SanPham> sanPham = (from sp in db.SanPham where sp.Ten.Contains(tenSp) select sp).ToList();
                    foreach(var sp in sanPham)
                    {
                        if (sp != null)
                        {
                            string tenAnh = XuLyAnh.layDuongDanDayDuToiFileAnhSanPham(sp.LinkAnhBia);
                            lsvQuanLySanPham.Items.Add(new { Id = sp.IdSanPham.ToString(), Ten = sp.Ten.ToString(), LinkAnh = tenAnh, Loai = sp.Loai.ToString(), SoLuong = sp.SoLuong.ToString(), SoLuongDaBan = sp.SoLuongDaBan.ToString(), GiaGoc = sp.GiaGoc.ToString(), GiaBan = sp.GiaBan.ToString(), PhiShip = sp.PhiShip.ToString() });
                        }
                    }
                     
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void cboSapXep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            try
            {
                lsvQuanLySanPham.Items.Clear();
                string selectedItemContent = (comboBox.SelectedItem as ComboBoxItem).Content.ToString().Trim();
                int index = comboBox.SelectedIndex;
                if (string.Equals(selectedItemContent, "Tất cả"))
                    HienThi_QuanLySanPham();
                else if (index == 0)
                {
                    HienThiNgayMuaLau(true);
                }
                else
                {
                    HienThiNgayMuaLau(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnSuaSP_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void btnXoaSP_Click(object sender, RoutedEventArgs e)
        {
          

        }

        

        private void lsvQuanLySanPham_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
