using LiveCharts.Wpf.Charts.Base;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TraoDoiDo.ViewModels;

namespace TraoDoiDo
{
    /// <summary>
    /// Interaction logic for DangKy.xaml
    /// </summary>
    public partial class DangKy : Window
    {
        TraoDoiDoEntities db = new TraoDoiDoEntities();

        public DangKy()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void btnDangKy_Click(object sender, RoutedEventArgs e)
        {
            var idMax = db.NguoiDung.Max(p => p.IdNguoiDung);
            int id = Convert.ToInt32(idMax) + 1;
             
            NguoiDung nguoi = new NguoiDung() { IdNguoiDung = id, HoTenNguoiDung = txtHoTen.Text, GioiTinhNguoiDung = cbGioiTinh.Text, NgaySinhNguoiDung = dtpNgaySinh.Text, CMNDNguoiDung = txtCMND.Text, EmailNguoiDung = txtEmail.Text, SdtNguoiDung = txtSdt.Text, DiaChiNguoiDung = txtDiaChi.Text, AnhNguoiDung = txtbTenFileAnh.Text, TienNguoiDung = "0" };
            TaiKhoan taiKhoan = new TaiKhoan() { TenDangNhap = txtTenDangNhap.Text, MatKhau = txtMatKhau.Password.ToString(), IdNguoiDung = id };
            try
            {
                db.NguoiDung.Add(nguoi);
                db.TaiKhoan.Add(taiKhoan);
                db.SaveChanges();
                XuLyAnh.LuuAnhVaoThuMuc(txtbDuongDanAnh.Text, "HinhDaiDien");
                MessageBox.Show("Đăng kí thành công");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đăng ký thất bại: " + ex.Message);
            }

        }

        private void btnChonAnh_Click(object sender, RoutedEventArgs e) //Chọn để hiển thị chứ chưa có lưu dô folder và csdl
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFileName = openFileDialog.FileName;
                imageDaiDien.Source = new BitmapImage(new Uri(selectedFileName));
                txtbDuongDanAnh.Text = selectedFileName;
                txtbTenFileAnh.Text = System.IO.Path.GetFileName(selectedFileName); 
            }
        }
    }
}
