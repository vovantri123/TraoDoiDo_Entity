using LiveCharts.Wpf;
using LiveCharts;
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

namespace TraoDoiDo.Views.DangDo
{
    /// <summary>
    /// Interaction logic for TabThongKe.xaml
    /// </summary>
    public partial class TabThongKeUC : UserControl
    {
        

        public TabThongKeUC()
        {
            InitializeComponent();
        }

       
        private void ThongKe_Load()
        {
            LoadTongDoanhThu();
            LoadTongSoLuongSanPhamDaBan();
            LoadTongKhachHang();

            LoadDoanhThuTheoSanPham();
            LoadTiLePhanTramDoanhThuTheoSanPham();
            LoadSoLuongSanPhamDaBan();

            LoadDanhGiaNguoiMua();
        }

        private void LoadDanhGiaNguoiMua()
        {
            
        }

        private void LoadSoLuongSanPhamDaBan()
        {
            
        }

        private void LoadTiLePhanTramDoanhThuTheoSanPham()
        {
            
        }

        private void LoadDoanhThuTheoSanPham()
        {
            
        }

        private void LoadTongDoanhThu()
        {
            
        }
        private void LoadTongSoLuongSanPhamDaBan()
        {
             
        }
        private void LoadTongKhachHang()
        {
           
        }
        private void btnThongTinChiTietDanhGia_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
