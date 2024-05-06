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

namespace TraoDoiDo.Views.QuanLy
{
    /// <summary>
    /// Interaction logic for TabQuanLyNguoiDungUC.xaml
    /// </summary>
    public partial class TabQuanLyNguoiDungUC : UserControl
    {
       
        public TabQuanLyNguoiDungUC()
        {
            InitializeComponent();
        }
      
        private void HienThi_QuanLyNguoiDung()
        {
        }

        //Tab Quản lý người dùng


        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
         

        }


        private void btnDuyet_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Nếu chưa duyệt thì chuyển sang đã duyệt\nNếu đã duyệt rồi, thì khi ấn nút này nó báo là sp đã được duyệt(hoặc vô hiệu hóa cái nút được thì càng tốt)");
        }


        private void cbSoSao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        private void HienThiNguoiDangUyTien(string soSaoDau, string soSaoCuoi)
        {
           
        }


        private void cbNgayMua_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbSoSao_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            try
            {
                lsvQuanLyNguoiDung.Items.Clear();
                string selectedItemContent = (comboBox.SelectedItem as ComboBoxItem).Content.ToString().Trim();
                if (string.Equals(selectedItemContent, "Tất cả"))
                    HienThi_QuanLyNguoiDung();
                else if (string.Equals(selectedItemContent, "Số sao từ 0 đến 2"))
                    HienThiNguoiDangUyTien("0", "2");
                else
                {
                    HienThiNguoiDangUyTien("3", "5");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void txbTimKiemNguoiDung_TextChanged(object sender, TextChangedEventArgs e)
        {
          
        }

        private void FQuanLyNguoiDung_Loaded(object sender, RoutedEventArgs e)
        {
            HienThi_QuanLyNguoiDung();
        }
    }
}
