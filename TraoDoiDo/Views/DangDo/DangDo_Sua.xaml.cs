using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Interaction logic for DangDo_Sua.xaml
    /// </summary>
    public partial class DangDo_Sua : Window
    {
        public int soLuongAnh = 0;
        public ThemAnhKhiDangUC[] DanhSachAnhVaMoTa = new ThemAnhKhiDangUC[150];

        SanPham sanPham; 

        TraoDoiDoEntities db = new TraoDoiDoEntities();


        public DangDo_Sua() 
        {
            InitializeComponent(); 
        }

        public DangDo_Sua(SanPham sanPham)
        {
            InitializeComponent();
            this.sanPham = sanPham;
            ucThongTin.btnSua.Click += btnSua_Click;
            ucThongTin.btnThemAnh.Click += btnThemAnh_Click;
        } 

        private void FDangDoSua_Loaded(object sender, RoutedEventArgs e)
        {
            ucThongTin.txtbIdSanPham.Text = sanPham.IdSanPham.ToString();
            ucThongTin.txtbTen.Text = sanPham.Ten;
            ucThongTin.txtbLoai.Text = sanPham.Loai;

            string dateString = sanPham.NgayMua;
            ucThongTin.dtpNgayMua.SelectedDate = DateTime.Parse(dateString);

            ucThongTin.txtbGiaBan.Text = sanPham.GiaBan;
            ucThongTin.txtbGiaGoc.Text = sanPham.GiaGoc;
            ucThongTin.cboXuatXu.Text = sanPham.XuatXu;
            ucThongTin.txtbMoTaChung.Text = sanPham.MoTaChung;
            ucThongTin.progressSlidere_PhanTramMoi.Value = Convert.ToInt32(sanPham.PhanTramMoi);
            ucThongTin.txtbPhiShip.Text = sanPham.PhiShip;
            ucThongTin.cboNoiBan.Text = sanPham.NoiBan;
            ucThongTin.ucTangGiamSoLuongTong.txtbSoLuong.Text = sanPham.SoLuong;
            ucThongTin.ucTangGiamSoLuongDaBan.txtbSoLuong.Text = sanPham.SoLuongDaBan;
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            suaAnhVaMoTaTrongCSDL(); //Phải để cái này ở trên cái dưới
            suaThongTinSanPhamTrongCSDL();
        }

        private void suaAnhVaMoTaTrongCSDL()
        {
            // Xóa dữ liệu cũ khỏi bảng MoTaAnhSanPham 
            var moTaAnhSPCanXoa = (from mtasp in db.MoTaAnhSanPham
                                   where mtasp.IdSanPham == sanPham.IdSanPham
                                   select mtasp).ToList();
             
            db.MoTaAnhSanPham.RemoveRange(moTaAnhSPCanXoa); 

            //Cập  nhật dữ liệu mới
            for (int i = 0; i < soLuongAnh; i++)
            { 
                if (DanhSachAnhVaMoTa[i].txtbTenFileAnh.Text != null && !string.IsNullOrEmpty(DanhSachAnhVaMoTa[i].txtbTenFileAnh.Text.Trim()) && DanhSachAnhVaMoTa[i].txtbTenFileAnh.Text != "no_image.jpg")
                {
                    //MoTaAnhSanPham moTaAnhSP = new MoTaAnhSanPham(ucThongTin.txtbIdSanPham.Text, (i + 1).ToString(), null, DanhSachAnhVaMoTa[i].txtbTenFileAnh.Text, DanhSachAnhVaMoTa[i].txtbMoTa.Text);
                    var moTaAnhSP = new MoTaAnhSanPham() { IdSanPham = Convert.ToInt32(ucThongTin.txtbIdSanPham.Text), IdAnhMinhHoa = i + 1, LinkAnhMinhHoa = DanhSachAnhVaMoTa[i].txtbTenFileAnh.Text, MoTa = DanhSachAnhVaMoTa[i].txtbMoTa.Text };
                    db.MoTaAnhSanPham.Add(moTaAnhSP);

                    XuLyAnh.LuuAnhVaoThuMuc(DanhSachAnhVaMoTa[i].txtbDuongDanAnh.Text, "HinhSanPham");
                }
                else
                    continue;
            }
            db.SaveChanges();
            MessageBox.Show("Thành công");
        }
        
        private void suaThongTinSanPhamTrongCSDL()
        {
            // Dữ liệu cần chèn
            string tenFileAnh = "no_image.jpg";
            for (int i = 0; i < soLuongAnh; i++)
                if (DanhSachAnhVaMoTa[i].txtbTenFileAnh.Text != null && !string.IsNullOrEmpty(DanhSachAnhVaMoTa[i].txtbTenFileAnh.Text.Trim()) && DanhSachAnhVaMoTa[i].txtbTenFileAnh.Text != "no_image.jpg")
                {
                    tenFileAnh = DanhSachAnhVaMoTa[i].txtbTenFileAnh.Text;
                    break;
                } 

            //SanPham sanPhamMoi = new SanPham(ucThongTin.txtbIdSanPham.Text, sanPham.IdNguoiDang, ucThongTin.txtbTen.Text, tenFileAnh, ucThongTin.txtbLoai.Text, ucThongTin.ucTangGiamSoLuongTong.txtbSoLuong.Text, ucThongTin.ucTangGiamSoLuongDaBan.txtbSoLuong.Text, ucThongTin.txtbGiaGoc.Text,
            //    ucThongTin.txtbGiaBan.Text, ucThongTin.txtbPhiShip.Text, "Đã duyệt", ucThongTin.cboNoiBan.Text, ucThongTin.cboXuatXu.Text, ucThongTin.dtpNgayMua.SelectedDate.Value.ToString("dd/MM/yyyy"), ucThongTin.txtbMoTaChung.Text, ucThongTin.progressSlidere_PhanTramMoi.Value.ToString(), luotXem, null, null);

            SanPham sanPhamMoi = db.SanPham.Find(sanPham.IdSanPham);
            sanPhamMoi.IdSanPham = Convert.ToInt32(ucThongTin.txtbIdSanPham.Text);
            sanPhamMoi.Ten = ucThongTin.txtbTen.Text;
            sanPhamMoi.LinkAnhBia = tenFileAnh;
            sanPhamMoi.Loai = ucThongTin.txtbLoai.Text;
            sanPhamMoi.SoLuong = ucThongTin.ucTangGiamSoLuongTong.txtbSoLuong.Text;
            sanPhamMoi.SoLuongDaBan = ucThongTin.ucTangGiamSoLuongDaBan.txtbSoLuong.Text;
            sanPhamMoi.GiaGoc = ucThongTin.txtbGiaGoc.Text;
            sanPhamMoi.GiaBan = ucThongTin.txtbGiaBan.Text;
            sanPhamMoi.PhiShip = ucThongTin.txtbPhiShip.Text; 
            sanPhamMoi.NoiBan = ucThongTin.cboNoiBan.Text;
            sanPhamMoi.XuatXu = ucThongTin.cboXuatXu.Text;
            sanPhamMoi.NgayMua = ucThongTin.dtpNgayMua.SelectedDate.Value.ToString("dd/MM/yyyy");
            sanPhamMoi.MoTaChung = ucThongTin.txtbMoTaChung.Text;
            sanPhamMoi.PhanTramMoi = ucThongTin.progressSlidere_PhanTramMoi.Value.ToString();
            db.SaveChanges();
        }

        public void btnThemAnh_Click(object sender, RoutedEventArgs e)
        { 
            DanhSachAnhVaMoTa[soLuongAnh] = new ThemAnhKhiDangUC();
            ucThongTin.wpnlChuaAnh.Children.Add(DanhSachAnhVaMoTa[soLuongAnh]);
            soLuongAnh++;
        }
    }
}

