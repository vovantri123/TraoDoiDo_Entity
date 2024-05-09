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
using TraoDoiDo.ViewModels;

namespace TraoDoiDo.Views.Windows
{
    /// <summary>
    /// Interaction logic for TabGioHangUC.xaml
    /// </summary>
    public partial class TabGioHangUC : UserControl
    { 
        private int soLuongSP = 0;
        private SanPhamUC[] DanhSachSanPham = new SanPhamUC[100];
        List<TrangThaiDonHang> dsSanPhamDeThanhToan = new List<TrangThaiDonHang>();

        NguoiDung ngMua;

        TraoDoiDoEntities db = new TraoDoiDoEntities();

        public TabGioHangUC()
        {
            InitializeComponent();
        }

        public TabGioHangUC(NguoiDung ngMua)
        {
            InitializeComponent();
            this.ngMua = ngMua;
            Loaded += GioHang_Load;
        }

        private void GioHang_Load(object sender, RoutedEventArgs e)
        {
            LsvGioHang_Load(sender, e);
            LoadVoucherCuaToi(sender, e);
        }

        private void LsvGioHang_Load(object sender, RoutedEventArgs e)
        { 
            var dsGioHang = (from gh in db.GioHang
                             join nd in db.NguoiDung on gh.IdNguoiMua equals nd.IdNguoiDung
                             join sp in db.SanPham on gh.IdSanPham equals sp.IdSanPham
                             where nd.IdNguoiDung == ngMua.IdNguoiDung
                             select new
                             {
                                 GioHang = gh,
                                 NguoiDung = nd,
                                 SanPham = sp
                             }).ToList();

            lsvGioHang.Items.Clear();
            foreach (var dong in dsGioHang)
            {
                string tenFileAnh = dong.SanPham.LinkAnhBia;
                string linkAnhBia = XuLyAnh.layDuongDanDayDuToiFileAnhSanPham(tenFileAnh);

                int soLuongTong = Convert.ToInt32(dong.SanPham.SoLuong);
                int soLuongDaBan = Convert.ToInt32(dong.SanPham.SoLuongDaBan);
                string trangThai = "";

                if (soLuongDaBan >= soLuongTong)
                    trangThai = "Hết hàng";
                else
                    trangThai = "Còn hàng";
                lsvGioHang.Items.Add(new { IdSP = dong.SanPham.IdSanPham, TenSP = dong.SanPham.Ten, LinkAnhBia = linkAnhBia, Gia = Convert.ToDecimal(dong.SanPham.GiaBan).ToString("#,##0"), PhiShip = Convert.ToDecimal(dong.SanPham.PhiShip).ToString("#,##0"), SoLuongMua = dong.GioHang.SoLuongMua, TrangThaiConHayHet = trangThai });
            }
        }

        private void LoadVoucherCuaToi(object sender, RoutedEventArgs e)
        {
            spnlVoucherCuaToi.Children.Clear();
            var dsVoucher = (from vc in db.Voucher
                             join ndvc in db.NguoiDungVoucher on vc.IdVoucher equals ndvc.IdVoucher
                             where ndvc.IdNguoiDung == ngMua.IdNguoiDung
                             select vc).ToList();

            foreach (var dong in dsVoucher)
            {
                VoucherUC vcUC = new VoucherUC(ngMua.IdNguoiDung);

                vcUC.txtbTenVoucher.Text = dong.TenVoucher;
                vcUC.txtbGiaTri.Text = Convert.ToDecimal(dong.GiaTri).ToString("#,##0");
                vcUC.txtbSoLuotSuDungConLai.Text = (Convert.ToInt32(dong.SoLuotSuDungToiDa) - Convert.ToInt32(dong.SoLuotDaSuDung)).ToString();
                vcUC.txtbNGayKetThuc.Text = dong.NgayKetThuc;
                vcUC.txtbIdVoucher.Text = dong.IdVoucher.ToString();

                vcUC.TextBlockChanged += vcUC_TextBlockChanged;
                vcUC.btnNhanVoucher.Visibility = Visibility.Collapsed;

                spnlVoucherCuaToi.Children.Add(vcUC);
            }
        }

        private void vcUC_TextBlockChanged(object sender, VoucherUC.TextBlockChangedEventArgs e) //Cho phep voucherUC có thể thay đổi textBlock của cha và gọi hàm của cha
        {
            // Cập nhật TextBlock trên form cha
            txtbgiaTriVoucher.Text = e.GiaTriMoi;
            txtbIdVoucher.Text = e.IdMoi;

            // Gọi hàm của form cha
            TinhTien();
        }
         
        private void btnXoaKhoiGioHang_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ListViewItem dongChuaButton = HoTroTimPhanTu.FindAncestor<ListViewItem>(btn);
            dynamic duLieuCuaDongChuaButton = dongChuaButton.DataContext;

            if (duLieuCuaDongChuaButton != null && MessageBox.Show("Bạn có chắc chắn muốn xóa mục đã chọn?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                try
                { 
                    GioHang gioHang = db.GioHang.Find(duLieuCuaDongChuaButton.IdSP, ngMua.IdNguoiDung);
                    db.GioHang.Remove(gioHang);
                    db.SaveChanges();
                    GioHang_Load(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xảy ra khi xóa sản phẩm: " + ex.Message);
                }
        }
         
        private void btnThanhToan_Click(object sender, RoutedEventArgs e)
        {
            DiaChi f = new DiaChi(ngMua, dsSanPhamDeThanhToan);
            f.tongThanhToan = txtbTongThanhToan.Text;
            f.idVoucher = Convert.ToInt32(txtbIdVoucher.Text);
            f.Closed += (s, ev) =>
            {
                GioHang_Load(sender, e);
            };
            f.ShowDialog();
        }

        private void ChonTatCaCacDong_Checked(object sender, RoutedEventArgs e)
        { 
            // Kiểm tra xem checkBox tổng đã được chọn hay không
            if (sender is CheckBox checkBoxTong && checkBoxTong.IsChecked.HasValue)
            {
                // Lặp qua mỗi dong trong ListView để đặt trạng thái thành đã chọn
                foreach (var dong in lsvGioHang.Items)
                { 
                    ListViewItem duLieuCuaDong = lsvGioHang.ItemContainerGenerator.ContainerFromItem(dong) as ListViewItem;
                    if (duLieuCuaDong != null)
                    { 
                        CheckBox checkBoxCuaDong = HoTroTimPhanTu.FindVisualChild<CheckBox>(duLieuCuaDong);
                        if (checkBoxCuaDong != null)
                        {
                            // Đặt trạng thái của CheckBox theo trạng thái của CheckBox chọn tất cả
                            checkBoxCuaDong.IsChecked = checkBoxTong.IsChecked;
                        }
                    }
                }
            }
            TinhTien();
        }

        private void TinhTienCuaNhungDongDuocChon_Checked(object sender, RoutedEventArgs e)
        {
            TinhTien();
        }

        private void TinhTien()
        {
            double tongTienHang = 0;
            double tongTienShip = 0;
            double tongThanhToan = 0;
            foreach (var dongDuocChon in lsvGioHang.SelectedItems)
            {
                dynamic dong = dongDuocChon;

                string idSP = dong.IdSP.ToString();
                string tenSP = dong.TenSP;

                string giaBan = dong.Gia;
                string phiShip = dong.PhiShip;
                string soLuongMua = dong.SoLuongMua;


                string ngayThanhToan = DateTime.Now.ToString("dd/MM/yyyy");
                string trangThai = "Chờ xác nhận";


                tongTienHang += Convert.ToDouble(giaBan.Replace(",", "")) * Convert.ToInt32(soLuongMua.Replace(",", ""));
                tongTienShip += Convert.ToDouble(phiShip.Replace(",", ""));

                tongThanhToan = tongTienHang + tongTienShip - Convert.ToDouble(txtbgiaTriVoucher.Text.Replace(",", ""));


               dsSanPhamDeThanhToan.Add(new TrangThaiDonHang() {IdNguoiMua = ngMua.IdNguoiDung, IdSanPham = Convert.ToInt32(idSP), SoLuongMua = soLuongMua, TongThanhToan = tongThanhToan.ToString(), Ngay = ngayThanhToan,TrangThai = trangThai});
            }

            txtbTongTienHang.Text = Convert.ToDecimal(tongTienHang).ToString("#,##0");
            txtbTongTienShip.Text = Convert.ToDecimal(tongTienShip).ToString("#,##0");
            txtbTongThanhToan.Text = Convert.ToDecimal(tongThanhToan).ToString("#,##0");
        } 
    }
}
