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
        TraoDoiDoEntities db = new TraoDoiDoEntities();
        List<NguoiDung> dsNguoiDung = new List<NguoiDung>();
        List<NguoiDung> listNguoiDung = new List<NguoiDung>();

        public TabQuanLyNguoiDungUC()
        {
            InitializeComponent();
        }
        public TabQuanLyNguoiDungUC(NguoiDung nguoiDung)
        {
            InitializeComponent();
        }
      
        private void HienThi_QuanLyNguoiDung()
        {
            try
            {
                lsvQuanLyNguoiDung.Items.Clear();
                dsNguoiDung = (from p in db.NguoiDung select p).ToList();
                foreach (var nguoiDung in dsNguoiDung)
                {

                    listNguoiDung.Add(nguoiDung);
                    lsvQuanLyNguoiDung.Items.Add(new { UserId = nguoiDung.IdNguoiDung, FullName = nguoiDung.HoTenNguoiDung, Identification = nguoiDung.CMNDNguoiDung, Gender = nguoiDung.GioiTinhNguoiDung, PhoneNumber = nguoiDung.SdtNguoiDung, DateOfBirth = nguoiDung.NgaySinhNguoiDung, Address = nguoiDung.DiaChiNguoiDung, Email = nguoiDung.EmailNguoiDung });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Tab Quản lý người dùng


        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            ListViewItem item = HoTroTimPhanTu.FindAncestor<ListViewItem>(btn);

            if (item != null)
            {
                dynamic dataItem = item.DataContext;
                int index = lsvQuanLyNguoiDung.Items.IndexOf(dataItem);
                ThongTinNguoiDang f = new ThongTinNguoiDang(listNguoiDung[index].IdNguoiDung);
                f.Show();
            }
        }
        private void btnXemDoanhThu_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ListViewItem item = HoTroTimPhanTu.FindAncestor<ListViewItem>(btn);
            if (item != null)
            {
                dynamic dataItem = item.DataContext;
                int id = Convert.ToInt32(dataItem.UserId);
                if (dataItem != null)
                {

                    NguoiDung ngDung = (from p in db.NguoiDung where p.IdNguoiDung == id select p).SingleOrDefault();
                    QuanLyDoanhThuNguoiDung f = new QuanLyDoanhThuNguoiDung(ngDung);
                    f.Show();

                }
            }
        }
        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ListViewItem item = HoTroTimPhanTu.FindAncestor<ListViewItem>(btn);
            if (item != null)
            {
                dynamic dataItem = item.DataContext;
                int idNguoiDung = dataItem.IdNguoiDung;
                if (dataItem != null)
                {
                    if (MessageBox.Show("Bạn có chắc muốn xóa tài khoản này?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            
                            NguoiDung ngDung = (from nguoi in db.NguoiDung where nguoi.IdNguoiDung == idNguoiDung  select nguoi).SingleOrDefault();
                            db.NguoiDung.Remove(ngDung);
                            db.SaveChanges();
                            TaiKhoan taikhoan = (from tk in db.TaiKhoan where tk.IdNguoiDung == idNguoiDung select tk).SingleOrDefault();
                            db.TaiKhoan.Remove(taikhoan);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi xảy ra khi xóa người dùng: " + ex.Message);
                        }
                    }
                }
            }

        }
        private void HienThiNguoiDangUyTien(string soSaoDau, string soSaoCuoi)
        {
            int saoDau = Convert.ToInt32(soSaoDau);
            int saoCuoi = Convert.ToInt32(soSaoCuoi);
            var dsDanhGiaSoSao = from danhGia in db.DanhGiaNguoiDang
                                                    group danhGia by danhGia.IdNguoiDang into g
                                                    let avgSoSao = g.Average(dg => dg.SoSao)
                                                    where avgSoSao >= saoDau && avgSoSao <= saoCuoi
                                                    select new
                                                    {
                                                        IdNguoiDang = g.Key,
                                                        SoSao = avgSoSao
                                                    };
            foreach (var danhGia in dsDanhGiaSoSao)
            {
                int idNguoiDang = danhGia.IdNguoiDang;
                var nguoiDung = (from d in db.DanhGiaNguoiDang
                                       join ng in db.NguoiDung
                                       on d.IdNguoiDang equals ng.IdNguoiDung
                                       where d.IdNguoiDang == idNguoiDang
                                       select new
                                       {
                                           IdNguoiDung = ng.IdNguoiDung,
                                           HoTenNguoiDung = ng.HoTenNguoiDung,
                                           GioiTinhNguoiDung = ng.GioiTinhNguoiDung,
                                           NgaySinhNguoiDung = ng.NgaySinhNguoiDung,
                                           CMNDNguoiDung = ng.CMNDNguoiDung,
                                           EmailNguoiDung = ng.EmailNguoiDung,
                                           SdtNguoiDung = ng.SdtNguoiDung,
                                           DiaChiNguoiDung = ng.DiaChiNguoiDung,
                                       }).Distinct().SingleOrDefault();
                lsvQuanLyNguoiDung.Items.Add(new { UserId = nguoiDung.IdNguoiDung, FullName = nguoiDung.HoTenNguoiDung, Identification = nguoiDung.CMNDNguoiDung, Gender = nguoiDung.GioiTinhNguoiDung, PhoneNumber = nguoiDung.SdtNguoiDung, DateOfBirth = nguoiDung.NgaySinhNguoiDung, Address = nguoiDung.DiaChiNguoiDung, SoSao = danhGia.SoSao, Email = nguoiDung.EmailNguoiDung });
            }
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
            try
            {
                lsvQuanLyNguoiDung.Items.Clear();
                if (string.IsNullOrEmpty(txbTimKiemNguoiDung.Text))
                    HienThi_QuanLyNguoiDung();
                else
                {
                    string idNguoiDung = txbTimKiemNguoiDung.Text.Trim();
                    var nguoiDung = (from nd in db.NguoiDung
                                           join tk in db.TaiKhoan
                                           on nd.IdNguoiDung equals tk.IdNguoiDung
                                           where nd.HoTenNguoiDung.Contains(idNguoiDung)
                                           select new
                                           {
                                               NguoiDung = nd
                                           }).FirstOrDefault();
                    if (nguoiDung != null)
                        lsvQuanLyNguoiDung.Items.Add(new { UserId = nguoiDung.NguoiDung.IdNguoiDung, FullName = nguoiDung.NguoiDung.HoTenNguoiDung, Identification = nguoiDung.NguoiDung.CMNDNguoiDung, Gender = nguoiDung.NguoiDung.GioiTinhNguoiDung, PhoneNumber = nguoiDung.NguoiDung.SdtNguoiDung, DateOfBirth = nguoiDung.NguoiDung.NgaySinhNguoiDung, Address = nguoiDung.NguoiDung.DiaChiNguoiDung, Email = nguoiDung.NguoiDung.EmailNguoiDung });
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hãy nhập đúng Id người dùng: "+ex);
            }
        }

        private void FQuanLyNguoiDung_Loaded(object sender, RoutedEventArgs e)
        {
            HienThi_QuanLyNguoiDung();
        }
    }
}
