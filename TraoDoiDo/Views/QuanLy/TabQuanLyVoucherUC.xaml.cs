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
    /// Interaction logic for TabQuanLyVoucherUC.xaml
    /// </summary>
    public partial class TabQuanLyVoucherUC : UserControl
    {
      
        public TabQuanLyVoucherUC()
        {
            InitializeComponent();
        }

        private void LoadDanhSachVoucer()
        {
            
        }
        private void lsvQLVoucher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
        private void btnDangVoucher_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void btnSuaVoucher_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnXoaVoucher_Click(object sender, RoutedEventArgs e) // truy vấn id trên lsv sẽ hiệu quả hơn thay vì lấy id từ textblock , từ đó ta có thể đặt thuộc tính isReadOnly thành True
        {
           
        }

        private void FQuanLyVoucher_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDanhSachVoucer();   
        }
    }
}
