using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes; 

namespace TraoDoiDo
{
    /// <summary>
    /// Interaction logic for UCSanPham.xaml
    /// </summary>
    public partial class SanPhamUC : UserControl
    {
        public int idNguoiDang;
        public int idNguoiMua; 
        public int yeuThich = 0;

        SanPham sp;
        NguoiDung nguoiDang;


        TraoDoiDoEntities db = new TraoDoiDoEntities();

        public SanPhamUC()
        {
            InitializeComponent();
        }

        public SanPhamUC(int yeuThich, int idNguoiMua)
        {
            InitializeComponent();

            this.yeuThich = yeuThich;
            this.idNguoiMua = idNguoiMua;
            sp = db.SanPhams.Find(Convert.ToInt32(txtbIdSanPham.Text));

            nguoiDang = (from sp in db.SanPhams
                         join nd in db.NguoiDungs on sp.IdNguoiDang equals nd.IdNguoiDung
                         where sp.IdSanPham == sp.IdSanPham
                         select nd
                         ).FirstOrDefault() ;
             

            if (yeuThich == 0)
            {
                btnThemVaoYeuThich.Visibility = Visibility.Visible;
                btnBoYeuThich.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnThemVaoYeuThich.Visibility = Visibility.Collapsed;
                btnBoYeuThich.Visibility = Visibility.Visible;
            }
            //nguoiDang = sanPhamDao.timKiemNguoiDangTheoIdSP(txtbIdSanPham.Text);
        }


        private void tangSoLuotXemThem1()
        {
            int idSanPham = Convert.ToInt32(txtbIdSanPham.Text);
            var soLuotXem = (from sp in db.SanPhams 
                             where sp.IdSanPham == idSanPham
                             select sp.SoLuotXem).FirstOrDefault();

            SanPham sanPham = db.SanPhams.Find(idSanPham);
            sanPham.SoLuotXem = (Convert.ToInt32(soLuotXem) + 1).ToString();

            db.SaveChanges();
        }

        private void btnThongTinChiTietSanPham_Click(object sender, MouseButtonEventArgs e)
        {
            tangSoLuotXemThem1();
             
            sp = new SanPham() {IdSanPham=Convert.ToInt32(txtbIdSanPham.Text), IdNguoiDang=idNguoiDang, Ten = txtbTen.Text, LinkAnhBia = sp.LinkAnhBia, Loai = txtbLoai.Text,  SoLuong = sp.SoLuong, SoLuongDaBan = sp.SoLuongDaBan, GiaGoc = txtbGiaGoc.Text, GiaBan = txtbGiaBan.Text,PhiShip=sp.PhiShip, TrangThai = sp.TrangThai, NoiBan = txtbNoiBan.Text, XuatXu = sp.XuatXu, NgayMua = sp.NgayMua, MoTaChung = sp.MoTaChung, PhanTramMoi = sp.PhanTramMoi, SoLuotXem = txtbSoLuotXem.Text, NgayDang = sp.NgayDang};
            ThongTinChiTietSanPham f = new ThongTinChiTietSanPham(sp);

            f.idNguoiDang = nguoiDang.IdNguoiDung;
            f.idNguoiMua = idNguoiMua;
            f.idSanPham = Convert.ToInt32(txtbIdSanPham.Text);
           
            f.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            f.ShowDialog();

        }

        private void btnThemVaoYeuThich_Click(object sender, RoutedEventArgs e)
        {
            btnThemVaoYeuThich.Visibility = Visibility.Collapsed;
            btnBoYeuThich.Visibility = Visibility.Visible; 

            DanhMucYeuThich danhMuc = new DanhMucYeuThich() { IdNguoiMua = Convert.ToInt32(idNguoiMua), IdSanPham = Convert.ToInt32(txtbIdSanPham.Text) };
            db.DanhMucYeuThiches.Add(danhMuc);
            db.SaveChanges(); 
        }

        private void btnBoYeuThich_Click(object sender, RoutedEventArgs e)
        {
            btnBoYeuThich.Visibility = Visibility.Collapsed;
            btnThemVaoYeuThich.Visibility = Visibility.Visible;

            DanhMucYeuThich danhMuc = db.DanhMucYeuThiches.Find(Convert.ToInt32(idNguoiMua), Convert.ToInt32(txtbIdSanPham.Text));
            db.DanhMucYeuThiches.Remove(danhMuc); 
            db.SaveChanges();
        }
    }
}
