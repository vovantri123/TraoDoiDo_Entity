using System.Windows;
using System; 
using TraoDoiDo.ViewModels;
using System.Windows.Navigation;
namespace TraoDoiDo
{
    /// <summary>
    /// Interaction logic for DangDo_Dang.xaml
    /// </summary>
    public partial class DangDo_Dang : Window
    {
        int soLuongAnh = 0;
        ThemAnhKhiDangUC[] DanhSachAnhVaMoTa = new ThemAnhKhiDangUC[100];

        NguoiDung ngDang;
        SanPham sanPhamMoi;



        TraoDoiDoEntities db = new TraoDoiDoEntities();

        public DangDo_Dang()
        {
            InitializeComponent();
        }

        public DangDo_Dang(NguoiDung ngDang)
        {
            InitializeComponent();
            Loaded += btnThemAnh_Click;
            this.ngDang = ngDang;
            ucThongTin.btnSua.Visibility = Visibility.Collapsed;
            ucThongTin.btnDang.Click += btnDang_Click;
            ucThongTin.btnThemAnh.Click += btnThemAnh_Click;
        }

        private void themThongTinVaoCSDL()
        {
            string tenFileAnh = "";
            for (int i = 0; i < soLuongAnh; i++)
                if (DanhSachAnhVaMoTa[i].txtbTenFileAnh.Text != null && !string.IsNullOrEmpty(DanhSachAnhVaMoTa[i].txtbTenFileAnh.Text.Trim()) && DanhSachAnhVaMoTa[i].txtbTenFileAnh.Text != "no_image.jpg")
                {
                    tenFileAnh = DanhSachAnhVaMoTa[i].txtbTenFileAnh.Text; //Lấy cái ảnh đầu tiên làm ảnh bìa
                    break;
                }
            string ngayHienTai = DateTime.Today.ToShortDateString();
            string ngayMua = ucThongTin.dtpNgayMua.Text;

            sanPhamMoi = new SanPham()
            {
                IdSanPham = Convert.ToInt32(ucThongTin.txtbIdSanPham.Text),
                IdNguoiDang = ngDang.IdNguoiDung,
                Ten = ucThongTin.txtbTen.Text,
                LinkAnhBia = tenFileAnh,
                Loai = ucThongTin.txtbLoai.Text,
                SoLuong = ucThongTin.ucTangGiamSoLuongTong.txtbSoLuong.Text,
                SoLuongDaBan = ucThongTin.ucTangGiamSoLuongDaBan.txtbSoLuong.Text,
                GiaGoc = ucThongTin.txtbGiaGoc.Text,
                GiaBan = ucThongTin.txtbGiaBan.Text,
                PhiShip = ucThongTin.txtbPhiShip.Text,
                NoiBan = ucThongTin.cboNoiBan.Text,
                XuatXu = ucThongTin.cboXuatXu.Text,
                NgayMua = ucThongTin.dtpNgayMua.SelectedDate.Value.ToString("dd/MM/yyyy"),
                MoTaChung = ucThongTin.txtbMoTaChung.Text,
                PhanTramMoi = ucThongTin.progressSlidere_PhanTramMoi.Value.ToString(),
                SoLuotXem = "0",
                NgayDang = DateTime.Today.ToShortDateString()
            };
        }

        private void themAnhVaMoTaVaoCSDL()
        {
            int idsp = Convert.ToInt32(ucThongTin.txtbIdSanPham.Text);
            for (int i = 0; i < soLuongAnh; i++)
            {
                if (DanhSachAnhVaMoTa[i].txtbTenFileAnh.Text != null && !string.IsNullOrEmpty(DanhSachAnhVaMoTa[i].txtbTenFileAnh.Text.Trim()) && DanhSachAnhVaMoTa[i].txtbTenFileAnh.Text != "no_image.jpg")
                {
                    int idAnhMinhHoa = (i + 1);
                    string tenFileAnh = DanhSachAnhVaMoTa[i].txtbTenFileAnh.Text;
                    string moTa = DanhSachAnhVaMoTa[i].txtbMoTa.Text;
                    //MoTaAnhSanPham mta = new MoTaAnhSanPham(id, idAnhMinhHoa, tenFileAnh, moTa);
                    MoTaAnhSanPham mta = new MoTaAnhSanPham() {IdSanPham = idsp, IdAnhMinhHoa = idAnhMinhHoa, LinkAnhMinhHoa = tenFileAnh, MoTa = moTa}; 
                    db.MoTaAnhSanPhams.Add(mta);
                    db.SaveChanges();
                    XuLyAnh.LuuAnhVaoThuMuc(DanhSachAnhVaMoTa[i].txtbDuongDanAnh.Text, "HinhSanPham");
                }
                else
                    continue;
            }
        }

        private void btnDang_Click(object sender, RoutedEventArgs e)
        { 
            themThongTinVaoCSDL();
            // bool check = sp.kiemTraCacTextBox();
            //if (check)
            {
                db.SanPhams.Add(sanPhamMoi);
                db.SaveChanges();
                themAnhVaMoTaVaoCSDL();
                MessageBox.Show("Đăng đồ thành công");
            }
        }

        private void btnThemAnh_Click(object sender, RoutedEventArgs e)
        {
            DanhSachAnhVaMoTa[soLuongAnh] = new ThemAnhKhiDangUC();
            ucThongTin.wpnlChuaAnh.Children.Add(DanhSachAnhVaMoTa[soLuongAnh]);
            soLuongAnh++;
        }
    }
}


