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
using System.Windows.Shapes; 
using TraoDoiDo.ViewModels;

namespace TraoDoiDo
{
    /// <summary>
    /// Interaction logic for QuenMatKhau.xaml
    /// </summary>
    public partial class QuenMatKhau : Window
    { 
        TraoDoiDoEntities db = new TraoDoiDoEntities();
        KiemTraDinhDang kiemTra = new KiemTraDinhDang();

        public QuenMatKhau()
        {
            InitializeComponent();
        }

        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            string matKhau = "";
            //string thongTinTaiKhoan = txtThongTinTaiKhoan.Text;
            if (kiemTra.kiemTraEmail(txtThongTinTaiKhoan.Text)) 
                matKhau = (from tk in db.TaiKhoans 
                           join ng in db.NguoiDungs on tk.IdNguoiDung equals ng.IdNguoiDung
                           where ng.EmailNguoiDung == txtThongTinTaiKhoan.Text
                           select tk.MatKhau).FirstOrDefault();
            if (kiemTra.kiemTraSoDienThoai(txtThongTinTaiKhoan.Text))
                matKhau = (from tk in db.TaiKhoans
                           join ng in db.NguoiDungs on tk.IdNguoiDung equals ng.IdNguoiDung
                           where ng.SdtNguoiDung == txtThongTinTaiKhoan.Text
                           select tk.MatKhau).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(matKhau))
                MessageBox.Show($"Mật khẩu của khách hàng là: {matKhau}");
            else
                MessageBox.Show($"Không tìm thấy email hoặc số điện thoại");
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
