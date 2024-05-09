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

namespace TraoDoiDo.Views.MuaDo
{
    /// <summary>
    /// Interaction logic for TabTrangThaiDonHangUC.xaml
    /// </summary>
    public partial class TabTrangThaiDonHangUC : UserControl
    {
        private int soLuongSP = 0;
        private SanPhamUC[] DanhSachSanPham = new SanPhamUC[150];
        List<TrangThaiDonHang> dsSanPhamDeThanhToan = new List<TrangThaiDonHang>();

        NguoiDung ngMua;

        TraoDoiDoEntities db = new TraoDoiDoEntities();

        public TabTrangThaiDonHangUC()
        {
            InitializeComponent();
        }

        public TabTrangThaiDonHangUC(NguoiDung ngMua)
        {
            InitializeComponent();
            this.ngMua = ngMua;
            Loaded += TrangThaiDonHang_Load;
        }

        private void TrangThaiDonHang_Load(object sender, RoutedEventArgs e)
        {
            LoadLsvTrongTabTrangThaiDonHang("lsvChoNguoiBanXacNhan", "Chờ xác nhận");
            LoadLsvTrongTabTrangThaiDonHang("lsvChoGiaoHang", "Chờ giao hàng");
            LoadLsvTrongTabTrangThaiDonHang("lsvDaNhan", "Đã nhận");
        }
        private void LoadLsvTrongTabTrangThaiDonHang(string tenLsv, string trangthai)
        {
            var dsTrangThaiDon = (from ttdh in db.TrangThaiDonHang
                                  join nd in db.NguoiDung on ttdh.IdNguoiMua equals nd.IdNguoiDung
                                  join sp in db.SanPham on ttdh.IdSanPham equals sp.IdSanPham
                                  where nd.IdNguoiDung == ngMua.IdNguoiDung && ttdh.TrangThai == trangthai
                                  select new
                                  {
                                      TrangThaiDonHang = ttdh,
                                      NguoiMua = nd,
                                      SanPham = sp
                                  }
                                  ).ToList();

            if (tenLsv == "lsvChoNguoiBanXacNhan")
                lsvChoNguoiBanXacNhan.Items.Clear();
            else if (tenLsv == "lsvChoGiaoHang")
                lsvChoGiaoHang.Items.Clear();
            else if (tenLsv == "lsvDaNhan")
                lsvDaNhan.Items.Clear();

            foreach (var dong in dsTrangThaiDon)
            {
                string tenFileAnh = dong.SanPham.LinkAnhBia;
                string linkAnhBia = XuLyAnh.layDuongDanDayDuToiFileAnhSanPham(tenFileAnh);

                if (tenLsv == "lsvChoNguoiBanXacNhan")
                    lsvChoNguoiBanXacNhan.Items.Add(new { IdSP = dong.SanPham.IdSanPham, TenSP = dong.SanPham.Ten, LinkAnhBia = linkAnhBia, Gia = dong.SanPham.GiaBan, PhiShip = dong.SanPham.PhiShip, SoLuongMua = dong.TrangThaiDonHang.SoLuongMua, TongThanhToan = dong.TrangThaiDonHang.TongThanhToan, Ngay = dong.TrangThaiDonHang.Ngay });
                else if (tenLsv == "lsvChoGiaoHang")
                    lsvChoGiaoHang.Items.Add(new { IdSP = dong.SanPham.IdSanPham, TenSP = dong.SanPham.Ten, LinkAnhBia = linkAnhBia, Gia = dong.SanPham.GiaBan, PhiShip = dong.SanPham.PhiShip, SoLuongMua = dong.TrangThaiDonHang.SoLuongMua, TongThanhToan = dong.TrangThaiDonHang.TongThanhToan, Ngay = dong.TrangThaiDonHang.Ngay });
                else if (tenLsv == "lsvDaNhan")
                    lsvDaNhan.Items.Add(new { IdSP = dong.SanPham.IdSanPham, TenSP = dong.SanPham.Ten, LinkAnhBia = linkAnhBia, Gia = dong.SanPham.GiaBan, PhiShip = dong.SanPham.PhiShip, SoLuongMua = dong.TrangThaiDonHang.SoLuongMua, TongThanhToan = dong.TrangThaiDonHang.TongThanhToan, Ngay = dong.TrangThaiDonHang.Ngay });
            }
        }

        private void btnHuyDatHang_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ListViewItem dongChuaButton = HoTroTimPhanTu.FindAncestor<ListViewItem>(btn);
            dynamic duLieuCuaDongChuaButton = dongChuaButton.DataContext;

            if (duLieuCuaDongChuaButton != null)
            {
                if (MessageBox.Show("Bạn có chắc là muốn hủy đặt hàng 0_o\nĐơn hàng mà bạn hủy sẽ được hoàn tiền", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                { 
                    TrangThaiDonHang trangThai = db.TrangThaiDonHang.Find(ngMua.IdNguoiDung, duLieuCuaDongChuaButton.IdSP);
                    db.TrangThaiDonHang.Remove(trangThai);

                    int idsp = Convert.ToInt32(duLieuCuaDongChuaButton.IdSP);
                    var donHang = (from qldh in db.QuanLyDonHang 
                                    where qldh.IdNguoiMua == ngMua.IdNguoiDung && qldh.IdSanPham == idsp
                                    select qldh).FirstOrDefault();
                    db.QuanLyDonHang.Remove(donHang);

                    db.SaveChanges();
                         
                    TrangThaiDonHang_Load(sender, e); 
                }
            }
        }
         
        private void btnDaNhanHang_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ListViewItem dongChuaButton = HoTroTimPhanTu.FindAncestor<ListViewItem>(btn);
            dynamic duLieuCuaDongChuaButton = dongChuaButton.DataContext;

            if (duLieuCuaDongChuaButton != null)
            {
                if (MessageBox.Show("Bạn có chắc là đã nhận được hàng 0_o", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                { 
                    TrangThaiDonHang trangThai = db.TrangThaiDonHang.Find(ngMua.IdNguoiDung, duLieuCuaDongChuaButton.IdSP);
                    trangThai.TrangThai = "Đã nhận";

                    int idsp = Convert.ToInt32(duLieuCuaDongChuaButton.IdSP);
                    var donHang = (from qldh in db.QuanLyDonHang
                                    where qldh.IdNguoiMua == ngMua.IdNguoiDung && qldh.IdSanPham == idsp
                                    select qldh).FirstOrDefault();
                    donHang.TrangThai = "Đã giao";

                    db.SaveChanges();

                    TrangThaiDonHang_Load(sender, e); 
                }
            }
        }

        private void btnDanhGia_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ListViewItem dongChuaButton = HoTroTimPhanTu.FindAncestor<ListViewItem>(btn);
            dynamic duLieuCuaDongChuaButton = dongChuaButton.DataContext;

            if (duLieuCuaDongChuaButton != null)
            { 
                int idsp = Convert.ToInt32(duLieuCuaDongChuaButton.IdSP);
                var nguoiDang = (from sp in db.SanPham
                                    join  nd in db.NguoiDung on sp.IdNguoiDang equals nd.IdNguoiDung
                                    where sp.IdSanPham == idsp
                                    select nd).FirstOrDefault();

                DanhGia f = new DanhGia(ngMua.IdNguoiDung, nguoiDang.IdNguoiDung);
                f.txtbTenNguoiDang.Text = nguoiDang.HoTenNguoiDung;
                f.ShowDialog(); 
            }
        }
         
        private void btnTraHang_Click(object sender, RoutedEventArgs e)
        {
            ucLyDoTraHang.idNguoiMua = ngMua.IdNguoiDung;
            Button btn = sender as Button;
            ListViewItem dongChuaButton = HoTroTimPhanTu.FindAncestor<ListViewItem>(btn);
            dynamic duLieuCuaDongChuaButton = dongChuaButton.DataContext;

            if (duLieuCuaDongChuaButton != null)
            {
                ucLyDoTraHang.idSP = duLieuCuaDongChuaButton.IdSP;
                ucLyDoTraHang.DrawerClosed += (btnSender, args) =>
                {
                    LoadLsvTrongTabTrangThaiDonHang("lsvChoGiaoHang", "Chờ giao hàng");
                };
            }
        }
    }
}
