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
        NguoiDung nguoiDung;

        TraoDoiDoEntities db = new TraoDoiDoEntities();

        public TabQuanLyVoucherUC()
        {
            InitializeComponent();
        }

        public TabQuanLyVoucherUC(NguoiDung nguoiDung)
        {
            InitializeComponent();
            this.nguoiDung = nguoiDung;
            Loaded += FQuanLyVoucher_Loaded;
        }
        private void FQuanLyVoucher_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDanhSachVoucer();
        }

        private void LoadDanhSachVoucer()
        {
            List<Voucher> dsVoucher = (from vc in db.Voucher
                                       select vc).ToList();
            lsvQLVoucher.ItemsSource = dsVoucher;
        }
        private void lsvQLVoucher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Kiểm tra xem có dòng nào được chọn không
            if (lsvQLVoucher.SelectedItem != null)
            {
                // Lấy dữ liệu của dòng được chọn
                Voucher dongDuocChon = lsvQLVoucher.SelectedItem as Voucher;

                if (dongDuocChon != null)
                {
                    txtbIdVoucher.Text = dongDuocChon.IdVoucher.ToString();
                    txtbTenVoucher.Text = dongDuocChon.TenVoucher;
                    txtbGiaTri.Text = dongDuocChon.GiaTri;
                    dtpNgayBatDau.SelectedDate = DateTime.Parse(dongDuocChon.NgayBatDau);
                    dtpNgayKetThuc.SelectedDate = DateTime.Parse(dongDuocChon.NgayKetThuc);
                    ucTangGiamSoLuotSuDungToiDa.txtbSoLuong.Text = dongDuocChon.SoLuotSuDungToiDa;
                    ucTangGiamSoLuotDaSuDung.txtbSoLuong.Text = dongDuocChon.SoLuotDaSuDung;
                }
            }
        }
        private void btnDangVoucher_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng ?", "Xác nhận đăng", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Voucher voucher = new Voucher()
                {
                    IdVoucher = 1,
                    TenVoucher = txtbTenVoucher.Text,
                    GiaTri = txtbGiaTri.Text,
                    SoLuotSuDungToiDa = ucTangGiamSoLuotSuDungToiDa.txtbSoLuong.Text,
                    SoLuotDaSuDung = ucTangGiamSoLuotDaSuDung.txtbSoLuong.Text,
                    NgayBatDau = dtpNgayBatDau.Text,
                    NgayKetThuc = dtpNgayKetThuc.Text
                };

                //if (voucher.kiemTraCacTextBox())
                { 
                    db.Voucher.Add(voucher);
                    db.SaveChanges();

                    FQuanLyVoucher_Loaded(sender, e);
                }

            }
        }
        private void btnSuaVoucher_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn sửa mục đã chọn?", "Xác nhận sửa", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Voucher voucher = db.Voucher.Find(Convert.ToInt32(txtbIdVoucher.Text));
                Voucher voucherMoi = new Voucher()
                {
                    TenVoucher = txtbTenVoucher.Text,
                    GiaTri = txtbGiaTri.Text,
                    SoLuotSuDungToiDa = ucTangGiamSoLuotSuDungToiDa.txtbSoLuong.Text,
                    SoLuotDaSuDung = ucTangGiamSoLuotDaSuDung.txtbSoLuong.Text,
                    NgayBatDau = dtpNgayBatDau.Text,
                    NgayKetThuc = dtpNgayKetThuc.Text
                };

                //if (voucherMoi.kiemTraCacTextBox())
                {
                    voucher.TenVoucher = voucherMoi.TenVoucher;
                    voucher.GiaTri = voucherMoi.GiaTri;
                    voucher.SoLuotSuDungToiDa = voucherMoi.SoLuotSuDungToiDa;
                    voucher.SoLuotDaSuDung = voucherMoi.SoLuotDaSuDung;
                    voucher.NgayBatDau = voucherMoi.NgayBatDau;
                    voucher.NgayKetThuc = voucherMoi.NgayKetThuc;
                      
                    db.SaveChanges(); 

                    FQuanLyVoucher_Loaded(sender, e);
                }
            }
        }

        private void btnXoaVoucher_Click(object sender, RoutedEventArgs e) // truy vấn id trên lsv sẽ hiệu quả hơn thay vì lấy id từ textblock , từ đó ta có thể đặt thuộc tính isReadOnly thành True
        {
            Button btn = sender as Button;
            ListViewItem dongChuaButton = HoTroTimPhanTu.FindAncestor<ListViewItem>(btn);
            dynamic duLieuCuaDongChuaButton = dongChuaButton.DataContext;

            if (duLieuCuaDongChuaButton != null)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa mục đã chọn?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int idVoucher = duLieuCuaDongChuaButton.IdVoucher;

                    List<NguoiDungVoucher> NguoiDungVoucherMuonXoa = (from ndvc in db.NguoiDungVoucher
                                                                      where ndvc.IdVoucher == idVoucher
                                                                      select ndvc).ToList();
                    db.NguoiDungVoucher.RemoveRange(NguoiDungVoucherMuonXoa);

                    Voucher voucherMuonXoa = db.Voucher.Find(idVoucher);
                    db.Voucher.Remove(voucherMuonXoa);

                    db.SaveChanges();

                    FQuanLyVoucher_Loaded(sender, e);
                }
            }
        }
    }
}
