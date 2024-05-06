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

namespace TraoDoiDo.Views.DangDo
{
    /// <summary>
    /// Interaction logic for TabQuanLyDonHangUC.xaml
    /// </summary>
    public partial class TabQuanLyDonHangUC : UserControl
    {
      

       

        public TabQuanLyDonHangUC()
        {
            InitializeComponent();
        }
         
        private void QuanLyDonHang_Load(object sender, RoutedEventArgs e)
        {
            LoadLsvTrongTabQuanLyDonHang("lsvChoDongGoi", "Chờ đóng gói");
            LoadLsvTrongTabQuanLyDonHang("lsvDangGiao", "Đang giao");
            LoadLsvTrongTabQuanLyDonHang("lsvDaGiao", "Đã giao");
            LoadLsvTrongTabQuanLyDonHang("lsvDonHangBiHoanTra", "Bị hoàn trả");
        }
        private void LoadLsvTrongTabQuanLyDonHang(string tenLsv, string trangthai)
        {
             
        }


        private void btnDiaChiGuiHang_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void btnGuiHang_Click(object sender, RoutedEventArgs e)
        {
            
        }

    }
}
