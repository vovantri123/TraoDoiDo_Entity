using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using TraoDoiDo;

namespace TraoDoiDo.Views.DangDo
{
    /// <summary>
    /// Interaction logic for TabQuanLySanPham.xaml
    /// </summary>
    public partial class TabSanPhamDaDangUC : UserControl
    {
        List<SanPham> dsSanPham;
        List<MoTaAnhSanPham> moTaAnhSanPham;

        SanPham sanPham;
        NguoiDung nguoiDung; //Người đăng 

        TraoDoiDoEntities db = new TraoDoiDoEntities();

        public TabSanPhamDaDangUC()
        {
            InitializeComponent();
        }  

        public TabSanPhamDaDangUC(NguoiDung nguoi)
        {
            InitializeComponent();
            Loaded += QuanLySanPham_Load;
            nguoiDung = nguoi;
        }

        private void QuanLySanPham_Load(object sender, RoutedEventArgs e) 
        {
            HienThi_QuanLySanPham();
        }

        private void HienThi_QuanLySanPham()
        {
            lsvQuanLySanPham.Items.Clear();
            // Load danh sách sản phẩm đã đăng
            dsSanPham = (from sp in db.SanPham
                         where sp.IdNguoiDang == nguoiDung.IdNguoiDung
                         select sp).ToList();

            foreach (var sanPham in dsSanPham)
            {
                string duongDanDayDu = XuLyAnh.layDuongDanDayDuToiFileAnhSanPham(sanPham.LinkAnhBia);
                lsvQuanLySanPham.Items.Add(new { Id = sanPham.IdSanPham, Ten = sanPham.Ten, LinkAnh = duongDanDayDu, Loai = sanPham.Loai, SoLuong = sanPham.SoLuong, SoLuongDaBan = sanPham.SoLuongDaBan, GiaGoc = sanPham.GiaGoc, GiaBan = sanPham.GiaBan, PhiShip = sanPham.PhiShip, NgayDang = sanPham.NgayDang });
            }
        }

        private void btnThemSanPhamMoi_Click(object sender, RoutedEventArgs e)
        {
            DangDo_Dang f = new DangDo_Dang(nguoiDung);

            f.ucThongTin.txtbIdSanPham.Text = (timIdMaxTrongBangSanPham() + 1).ToString();

            f.Closed += (s, ev) =>
            {
                HienThi_QuanLySanPham();
            };
            f.ShowDialog();
        }

        private int timIdMaxTrongBangSanPham()
        { 
            return db.SanPham.Max(sp => sp.IdSanPham);
        }

        private void btnSuaDo_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button; 
            ListViewItem dongChuaButton = HoTroTimPhanTu.FindAncestor<ListViewItem>(btn); 
            dynamic duLieuCuaDongChuaButton = dongChuaButton.DataContext; 

            if (duLieuCuaDongChuaButton != null)
            {
                //Load thông tin sản phẩm lên để sửa
                SanPham sanPham = db.SanPham.Find(duLieuCuaDongChuaButton.Id);
                DangDo_Sua f = new DangDo_Sua(sanPham);
                int idsp = Convert.ToInt32(duLieuCuaDongChuaButton.Id);
                moTaAnhSanPham = (from mtasp in db.MoTaAnhSanPham
                                  join sp in db.SanPham on mtasp.IdSanPham equals sp.IdSanPham
                                  where sp.IdSanPham == idsp
                                  select mtasp).ToList();

                foreach (var dong in moTaAnhSanPham)
                {
                    f.DanhSachAnhVaMoTa[f.soLuongAnh] = new ThemAnhKhiDangUC();

                    f.DanhSachAnhVaMoTa[f.soLuongAnh].txtbTenFileAnh.Text = dong.LinkAnhMinhHoa;

                    string duongDanAnh = XuLyAnh.layDuongDanDayDuToiFileAnhSanPham(dong.LinkAnhMinhHoa);
                    f.DanhSachAnhVaMoTa[f.soLuongAnh].txtbDuongDanAnh.Text = duongDanAnh;

                    f.DanhSachAnhVaMoTa[f.soLuongAnh].imgAnhSP.Source = new BitmapImage(new Uri(duongDanAnh));

                    f.DanhSachAnhVaMoTa[f.soLuongAnh].txtbMoTa.Text = dong.MoTa;

                    f.ucThongTin.wpnlChuaAnh.Children.Add(f.DanhSachAnhVaMoTa[f.soLuongAnh]);
                    f.soLuongAnh++;
                }
                f.Closed += (s, ev) =>
                {
                    HienThi_QuanLySanPham();
                };
                f.ShowDialog(); 
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ListViewItem dongChuaButton = HoTroTimPhanTu.FindAncestor<ListViewItem>(btn); 
            dynamic duLieuCuaDongChuaButton = dongChuaButton.DataContext; 

            if (duLieuCuaDongChuaButton != null)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa mục đã chọn?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        int idSanPhamMuonXoa = Convert.ToInt32(duLieuCuaDongChuaButton.Id);

                        //Xóa danh sách ảnh của sản phẩm
                        var dsMoTaAnhSanPhamMuonXoa = (from mtasp in db.MoTaAnhSanPham
                                                       where mtasp.IdSanPham == idSanPhamMuonXoa
                                                       select mtasp).ToList();
                        db.MoTaAnhSanPham.RemoveRange(dsMoTaAnhSanPhamMuonXoa);

                        //Xóa sản phẩm
                        var sanPhamMuonXoa = db.SanPham.Find(idSanPhamMuonXoa);
                        db.SanPham.Remove(sanPhamMuonXoa);

                        db.SaveChanges();

                        MessageBox.Show("Xóa thành công");
                        lsvQuanLySanPham.Items.Remove(duLieuCuaDongChuaButton);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xảy ra khi xóa sản phẩm: " + ex.Message);
                    }
                }
            }
        }

        

        private void txbTiemKiem_TextChanged(object sender, TextChangedEventArgs e)
        {
            lsvQuanLySanPham.Items.Clear();
            if (txbTimKiem.Text == null)
                HienThi_QuanLySanPham();
            
            foreach (var sanPham in dsSanPham)
                if(sanPham.Ten.ToLower().Contains(txbTimKiem.Text.Trim().ToLower()))
                {
                    string duongDanDayDu = XuLyAnh.layDuongDanDayDuToiFileAnhSanPham(sanPham.LinkAnhBia);
                    lsvQuanLySanPham.Items.Add(new { Id = sanPham.IdSanPham, Ten = sanPham.Ten, LinkAnh = duongDanDayDu, Loai = sanPham.Loai, SoLuong = sanPham.SoLuong, SoLuongDaBan = sanPham.SoLuongDaBan, GiaGoc = sanPham.GiaGoc, GiaBan = sanPham.GiaBan, PhiShip = sanPham.PhiShip });
                }    
        }

        private void cbLocLoai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            lsvQuanLySanPham.Items.Clear();
            string mucDaChon = (comboBox.SelectedItem as ComboBoxItem).Content.ToString();
            if (string.Equals(mucDaChon, "Tất cả"))
                HienThi_QuanLySanPham();
            else 
                foreach (var sanPham in dsSanPham)
                    if (sanPham.Loai.ToLower().Contains(mucDaChon.Trim().ToLower()))
                    {
                        string tenAnh = XuLyAnh.layDuongDanDayDuToiFileAnhSanPham(sanPham.LinkAnhBia);
                        lsvQuanLySanPham.Items.Add(new { Id = sanPham.IdSanPham, Ten = sanPham.Ten, LinkAnh = tenAnh, Loai = sanPham.Loai, SoLuong = sanPham.SoLuong, SoLuongDaBan = sanPham.SoLuongDaBan, GiaGoc = sanPham.GiaGoc, GiaBan = sanPham.GiaBan, PhiShip = sanPham.PhiShip });
                    }     
        }
    }
} 