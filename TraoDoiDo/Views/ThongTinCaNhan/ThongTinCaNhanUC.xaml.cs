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
using Microsoft.Win32;
using TraoDoiDo.ViewModels;
using System.IO;
namespace TraoDoiDo
{
    /// <summary>
    /// Interaction logic for ThongTinCaNhanUC.xaml
    /// </summary>
    public partial class ThongTinCaNhanUC : UserControl
    {
        NguoiDung nguoi;
        TaiKhoan taiKhoan;

        TraoDoiDoEntities db = new TraoDoiDoEntities();

        public ThongTinCaNhanUC()
        {
            InitializeComponent(); 
        }

        public ThongTinCaNhanUC(NguoiDung nguoi)
        {
            InitializeComponent();
            this.DataContext = this;
            Loaded += UCThongTinCaNhan_Loaded;
            this.nguoi = db.NguoiDungs.Find(nguoi.IdNguoiDung);
            this.taiKhoan = db.TaiKhoans.Find(nguoi.IdNguoiDung);
        }

        private void UCThongTinCaNhan_Loaded(object sender, RoutedEventArgs e)
        {
            LoadThongTin(nguoi);
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            TaiKhoan taiKhoanMoi = new TaiKhoan() { TenDangNhap = txtTenDangNhap.Text, MatKhau = txtMatKhau.Password, IdNguoiDung = nguoi.IdNguoiDung };

            string tenAnh = XuLyAnh.layDuongDanDayDuToiFileAnhDaiDien(imageHinhDaiDien.Source.ToString());
            string tenFileAnh = Path.GetFileName(tenAnh);
            string tien = (from nd in db.NguoiDungs
                           where nd.IdNguoiDung == nguoi.IdNguoiDung
                           select nd.TienNguoiDung).FirstOrDefault();

            NguoiDung nguoiMoi = new NguoiDung() { IdNguoiDung = nguoi.IdNguoiDung, HoTenNguoiDung = txtHoTen.Text, GioiTinhNguoiDung = cbGioiTinh.Text, NgaySinhNguoiDung = dtpNgaySinh.Text, CMNDNguoiDung = txtCmnd.Text, EmailNguoiDung = txtEmail.Text, SdtNguoiDung = txtSdt.Text, DiaChiNguoiDung = txtDiaChi.Text, AnhNguoiDung = tenFileAnh, TienNguoiDung = tien};

            //if (nguoi.kiemTraCacTextBox())
            {
                db.Entry(nguoi).CurrentValues.SetValues(nguoiMoi);
                db.SaveChanges();
                MessageBox.Show("Cập nhật thành công");
            }
        }

        private void btnDoiMatKhau_Click(object sender, RoutedEventArgs e)
        {
            TaiKhoan taiKhoanMoi = new TaiKhoan() { TenDangNhap = txtTenDangNhap.Text, MatKhau = txtMatKhau.Password, IdNguoiDung = nguoi.IdNguoiDung}; 
            //if (taiKhoanMoi.kiemTraCacTextBox())
            { 
                taiKhoan.TenDangNhap = taiKhoanMoi.TenDangNhap;
                taiKhoan.MatKhau = taiKhoanMoi.MatKhau;
                db.SaveChanges();
                MessageBox.Show("Đổi mật khẩu thành công");
            }
        }

        private void btnChonAnh_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Image files(*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files(*.*)|*.*";
            if (file.ShowDialog() == true)
            {
                string imagePath = file.FileName;
                BitmapImage hinh = new BitmapImage(new Uri(imagePath));
                imageHinhDaiDien.Source = hinh;
            }
        }

        private void LoadThongTin(NguoiDung nguoi)
        {
            txtHoTen.Text = nguoi.HoTenNguoiDung;
            txtCmnd.Text = nguoi.CMNDNguoiDung;
            txtDiaChi.Text = nguoi.DiaChiNguoiDung;
            txtId.Text = nguoi.IdNguoiDung.ToString();
            txtSdt.Text = nguoi.SdtNguoiDung;
            txtTenDangNhap.Text = nguoi.TaiKhoan.TenDangNhap;
            txtMatKhau.Password = nguoi.TaiKhoan.MatKhau;
            txtEmail.Text = nguoi.EmailNguoiDung;
            cbGioiTinh.Text = nguoi.GioiTinhNguoiDung;

            string selectedDate = nguoi.NgaySinhNguoiDung;
            dtpNgaySinh.Text = selectedDate;

            imageHinhDaiDien.Source = new BitmapImage(new Uri(XuLyAnh.layDuongDanDayDuToiFileAnhDaiDien(nguoi.AnhNguoiDung)));
        }
    }
}
