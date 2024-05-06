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
    /// Interaction logic for DangNhap.xaml
    /// </summary>
    public partial class DangNhap : Window
    {
        NguoiDung nguoi;
        TraoDoiDoEntities db = new TraoDoiDoEntities();

        public DangNhap()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, RoutedEventArgs e)
        {
            TaiKhoan taiKhoan = new TaiKhoan() {TenDangNhap = txtTenDangNhap.Text, MatKhau = txtMatKhau.Password.ToString()}; // Không truyền thì tự hiểu là null ha sao á

            int idNguoiDung = (from nd in db.NguoiDungs
                               join tk in db.TaiKhoans on nd.IdNguoiDung equals tk.IdNguoiDung
                               where tk.TenDangNhap == taiKhoan.TenDangNhap && tk.MatKhau == taiKhoan.MatKhau
                               select nd.IdNguoiDung).DefaultIfEmpty(-1).FirstOrDefault();
             
            nguoi = db.NguoiDungs.Find(idNguoiDung);
              
            if (nguoi == null)
            {
                MessageBox.Show("Tài khoản sai! Vui lòng đăng nhập lại");
                return;
            }
            else
            {
                this.Hide();
                MainWindow f = new MainWindow(nguoi);
                f.ShowDialog();
            }
        }

        private void btnDangKy_Click(object sender, RoutedEventArgs e)
        {
            DangKy dangKy = new DangKy();
            dangKy.ShowDialog();
        }

        private void txtbQuenMatKhau_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            QuenMatKhau quenMK = new QuenMatKhau();
            quenMK.ShowDialog();
        }
    }
}
