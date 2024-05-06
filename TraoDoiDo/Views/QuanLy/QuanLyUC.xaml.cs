using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using System.Data.SqlClient;
using TraoDoiDo.ViewModels;
using TraoDoiDo.Views.QuanLy;

namespace TraoDoiDo
{
    /// <summary>
    /// Interaction logic for QuanLy.xaml
    /// </summary>
    public partial class QuanLyUC : UserControl
    {
        public QuanLyUC()
        {
            InitializeComponent();
        }

        public QuanLyUC(NguoiDung nguoiDung)
        {
            InitializeComponent();
            //ccTabQuanLySanPham.Content = new TabQuanLySanPhamUC(nguoiDung);
            //ccTabQuanLyNguoiDung.Content = new TabQuanLyNguoiDungUC(nguoiDung);
            ccTabQuanLyVoucher.Content = new TabQuanLyVoucherUC(nguoiDung);
        }
       

        
    }
}
