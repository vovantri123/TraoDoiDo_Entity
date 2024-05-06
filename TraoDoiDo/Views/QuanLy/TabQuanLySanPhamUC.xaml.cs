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
       
        public TabQuanLySanPhamUC()
        {
            InitializeComponent();
            
        }
        
        private void FQuanLySanPham_Loaded(object sender, RoutedEventArgs e)
        {
            HienThi_QuanLySanPham();
        }
        private void HienThi_QuanLySanPham()
        {
            
        }
        private static int TinhKhoangCachSoNgay(DateTime ngay)
        {
            TimeSpan kc = DateTime.Now.Date - ngay.Date;
            return Math.Abs(kc.Days);
        }
        private void HienThiNgayMuaLau(bool kt)
        {
           
        }
        private void cbLocLoai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void txbTimKiemSanPham_TextChanged(object sender, TextChangedEventArgs e)
        {
            
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
