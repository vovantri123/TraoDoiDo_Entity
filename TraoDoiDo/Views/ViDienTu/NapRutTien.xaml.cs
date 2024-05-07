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
    /// Interaction logic for NapTien.xaml
    /// </summary>
    public partial class NapRutTien : Window
    {
        public double soTienNap = 0;
        public double soTienRut = 0;
        public string nguonTienDen = "";
        public string nguonTienTu = "";
        public string thoiGianGiaoDich = "";

        NguoiDung ngDung = new NguoiDung();

        TraoDoiDoEntities db = new TraoDoiDoEntities();

        public NapRutTien()
        {
            InitializeComponent();
        }
        public NapRutTien(NguoiDung ngDung)
        {
            InitializeComponent();
            this.DataContext = this;
            this.ngDung = ngDung;
        }

        private void FNapRutTien_Loaded(object sender, RoutedEventArgs e)
        {
            if (txtbTieuDe.Text == "Nạp tiền")
            {
                btnRutTien.Visibility = Visibility.Hidden;
                btnNapTien.Visibility = Visibility.Visible;
            }
            else
            {
                btnRutTien.Visibility = Visibility.Visible;
                btnNapTien.Visibility = Visibility.Hidden;
            }
        }

        private void btnNapTien_Click(object sender, RoutedEventArgs e)
        {
            soTienNap = tinhTien();
            nguonTienTu = chonNguonTien();
            nguonTienDen = "Ví điện tử";
            thoiGianGiaoDich = DateTime.Now.ToString();
            double soTienNguoiDung = Convert.ToDouble((from nd in db.NguoiDung
                                                       where nd.IdNguoiDung == ngDung.IdNguoiDung
                                                       select nd.TienNguoiDung).FirstOrDefault());

            double soTienSauNap = soTienNguoiDung + soTienNap;

            GiaoDich giaoDich = new GiaoDich() { IdNguoiDung = ngDung.IdNguoiDung, LoaiGiaoDich = txtbTieuDe.Text,SoTien = soTienNap.ToString(), TuNguonGiaoDich = nguonTienTu, DenNguonGiaoDich = nguonTienDen, NgayGiaoDich = thoiGianGiaoDich};
            db.GiaoDich.Add(giaoDich);

            NguoiDung nguoiDung = db.NguoiDung.Find(ngDung.IdNguoiDung);
            nguoiDung.TienNguoiDung = soTienSauNap.ToString(); 

            db.SaveChanges();

            MainWindow fNguoiDung = new MainWindow();
            fNguoiDung.LoadWindow();
            MessageBox.Show("Nạp tiền thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnRutTien_Click(object sender, RoutedEventArgs e)
        {
            soTienRut = tinhTien();
            nguonTienTu = "Ví điện tử";
            nguonTienDen = chonNguonTien();
            thoiGianGiaoDich = DateTime.Now.ToString();
            double soTienNguoiDung = Convert.ToDouble((from nd in db.NguoiDung
                                                       where nd.IdNguoiDung == ngDung.IdNguoiDung
                                                       select nd.TienNguoiDung).FirstOrDefault());
            double soTienSauRut = soTienNguoiDung - soTienRut;

            GiaoDich giaoDich = new GiaoDich() { IdNguoiDung = ngDung.IdNguoiDung, LoaiGiaoDich = txtbTieuDe.Text, SoTien = soTienRut.ToString(), TuNguonGiaoDich = nguonTienTu, DenNguonGiaoDich = nguonTienDen, NgayGiaoDich = thoiGianGiaoDich };
            db.GiaoDich.Add(giaoDich);

            NguoiDung nguoiDung = db.NguoiDung.Find(ngDung.IdNguoiDung);
            nguoiDung.TienNguoiDung = soTienSauRut.ToString();

            db.SaveChanges();

            MainWindow fNguoiDung = new MainWindow();
            fNguoiDung.LoadWindow();
            MessageBox.Show("Rút tiền thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rbtn = (RadioButton)sender;
            txtGiaTien.Text = rbtn.Content.ToString();
        }

        public static string XoaDauCham(string tien)
        { 
            return tien.Replace(",", "");
        }

        public double tinhTien()
        {
            double soTien = 0;
            if (string.IsNullOrEmpty(txtGiaTien.Text))
            {
                if (rbtnMotTram.IsChecked == true)
                {
                    soTien += 100000;
                }
                if (rbtnHaiTram.IsChecked == true)
                {
                    soTien += 200000;
                }
                if (rbtnNamTram.IsChecked == true)
                {
                    soTien += 500000;
                }
                if (rbtnMotTrieu.IsChecked == true)
                {
                    soTien += 1000000;
                }
                if (rbtnHaiTrieu.IsChecked == true)
                {
                    soTien += 2000000;
                }
                if (rbtnNamTrieu.IsChecked == true)
                {
                    soTien += 5000000;
                }
            }
            else
            {
                soTien += Convert.ToDouble(XoaDauCham(txtGiaTien.Text));
            }
            return soTien;

        }
        public string chonNguonTien()
        {
            string nguonTien = "";
            if (rbtnBIDV.IsChecked == true)
            {
                nguonTien = "BIDV";
            }
            if (rbtnSacombank.IsChecked == true)
            {
                nguonTien = "Sacombank";
            }
            if (rbtnACB.IsChecked == true)
            {
                nguonTien = "ACB";
            }
            if (rbtnTPBank.IsChecked == true)
            {
                nguonTien = "TPBank";
            }
            if (rbtnTechcombank.IsChecked == true)
            {
                nguonTien = "Techcombank";
            }
            if (rbtnViettin.IsChecked == true)
            {
                nguonTien = "Viettin Bank";
            }
            if (rbtnVietcombank.IsChecked == true)
            {
                nguonTien = "Vietcombank";
            }
            if (rbtnVIB.IsChecked == true)
            {
                nguonTien = "VIB Bank";
            }
            if (rbtnVPBank.IsChecked == true)
            {
                nguonTien = "VPBank";
            }
            
            if (rbtnAgribank.IsChecked == true)
            {
                nguonTien = "Agribank";
            }
            if (rbtnBaoViet.IsChecked == true)
            {
                nguonTien = "BAOVIET Bank";
            }
            if (rbtnVietA.IsChecked == true)
            {
                nguonTien = "VietA";
            }
            return nguonTien;
        }
    }
}
