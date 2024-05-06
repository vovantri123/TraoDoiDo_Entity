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
using TraoDoiDo.ViewModels;

namespace TraoDoiDo
{
    /// <summary>
    /// Interaction logic for MucXepHangNguoiMuaNhieuNhatUC.xaml
    /// </summary>
    public partial class MucXepHangNguoiMuaNhieuNhatUC : UserControl
    { 
        public MucXepHangNguoiMuaNhieuNhatUC()
        {
            InitializeComponent();
        }
        public MucXepHangNguoiMuaNhieuNhatUC(string tenNguoiMua,string diaChi, string anh, string soLuotMua)
        {
            InitializeComponent();
            txtbTenNguoiMuaNhieuNhat.Text = tenNguoiMua;
            txtbDiaChiNguoiMuaNhieuNhat.Text = diaChi;

            string linkAnh = XuLyAnh.layDuongDanDayDuToiFileAnhDaiDien(anh);
            BitmapImage image = new BitmapImage(new Uri(linkAnh));
            imgAnhNguoiMuaNhieuNhat.Source = image;

            txtbSoLuotMua.Text = soLuotMua;
        }
    }
}
