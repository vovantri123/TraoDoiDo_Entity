using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Channels;
using TraoDoiDo.ViewModels;
using System.Reflection;



namespace TraoDoiDo
{
    /// <summary>
    /// Interaction logic for ThongTinChiTietSanPham.xaml
    /// </summary>
    public partial class ThongTinChiTietSanPham : Window
    {
        public int idNguoiMua;
        public int idNguoiDang;
        public int idSanPham;
        private SanPhamUC[] DanhSachSanPhamUC;
        public string linkAnhBia = "";
        private List<string> danhSachAnh = new List<string>();
        private int indexAnhHienTai = -1;

        SanPham sp;

        TraoDoiDoEntities db = new TraoDoiDoEntities();

        public ThongTinChiTietSanPham()
        { 
            InitializeComponent();
        }
        public ThongTinChiTietSanPham(SanPham sp)
        {
            this.sp = sp; 

            InitializeComponent();

            Loaded += LoadThongTinSanPham;
            Loaded += LoadAnhVaMoTa;
            Loaded += btnAnhSau_Click;
            Loaded += LoadThongTinNguoiDang;
        }

        private void LoadThongTinSanPham(object sender, RoutedEventArgs e)
        {
            string soLuong = sp.SoLuong;
            string soLuongDaBan = sp.SoLuongDaBan;
            txtbSoLuongConLai.Text = (Convert.ToInt32(soLuong) - Convert.ToInt32(soLuongDaBan)).ToString();
            txtbTen.Text = sp.Ten;
            txtbLoai.Text = sp.Loai;
            txtbGiaGoc.Text = Convert.ToDecimal(sp.GiaGoc).ToString("#,##0");
            txtbGiaBan.Text = Convert.ToDecimal(sp.GiaBan).ToString("#,##0");
            txtbPhiShip.Text = Convert.ToDecimal(sp.PhiShip).ToString("#,##0");
            txtbNoiBan.Text = sp.NoiBan;
            txtbXuatXu.Text = sp.XuatXu;
            txtbNgayMua.Text = sp.NgayMua;
            txtbPhanTramConMoi.Text = sp.PhanTramMoi;
            txtbMoTaChung.Text = sp.MoTaChung;

            LoadSanPhamlenWpnlHienThiSPCungLoai(sender, e); 
        }

        private void LoadAnhVaMoTa(object sender, RoutedEventArgs e)
        {
            var dsMoTaAnh = (from spham in db.SanPham
                             join mtasp in db.MoTaAnhSanPham on spham.IdSanPham equals mtasp.IdSanPham
                             where spham.IdSanPham == sp.IdSanPham
                             select new
                             {
                                 LinkAnhBia = spham.LinkAnhBia,
                                 LinkAnhMinhHoa = mtasp.LinkAnhMinhHoa,
                                 MoTa = mtasp.MoTa
                             }).ToList();

            foreach (var dong in dsMoTaAnh)
            {
                linkAnhBia = dong.LinkAnhBia; //Cập nhật biến toàn cục linkAnhBia đã khai báo ở đầu
                string moTa = dong.MoTa;
                string linkAnhMinhHoa = XuLyAnh.layDuongDanDayDuToiFileAnhSanPham(dong.LinkAnhMinhHoa);
                danhSachAnh.Add(linkAnhMinhHoa);
                if (moTa.Trim() == "")
                    continue;
                lsvThongTinChiTietSP.Items.Add(new { MoTa = "- " + moTa });
            }
        }

        private void LoadSanPhamlenWpnlHienThiSPCungLoai(object sender, RoutedEventArgs e)
        { 
            var dsSanPhamCungLoai = (from spham in db.SanPham
                                     where spham.Loai == sp.Loai && spham.IdSanPham != sp.IdSanPham && spham.IdNguoiDang != idNguoiMua
                                     select spham).ToList();

            DanhSachSanPhamUC = new SanPhamUC[dsSanPhamCungLoai.Count + 1];
            wpnlHienThiSPCungLoai.Children.Clear();
            int i = 0;
            foreach (var dong in dsSanPhamCungLoai)
            { 
                DanhSachSanPhamUC[i] = new SanPhamUC(0, idNguoiMua);

                DanhSachSanPhamUC[i].txtbIdSanPham.Text = dong.IdSanPham.ToString();
                DanhSachSanPhamUC[i].txtbTen.Text = dong.Ten;

                string tenFileAnh = dong.LinkAnhBia;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                string duongDanhAnh = XuLyAnh.layDuongDanDayDuToiFileAnhSanPham(tenFileAnh);
                bitmap.UriSource = new Uri(duongDanhAnh);
                bitmap.EndInit();
                DanhSachSanPhamUC[i].imgSP.Source = bitmap;

                DanhSachSanPhamUC[i].txtbGiaGoc.Text = Convert.ToDecimal(dong.GiaGoc).ToString("#,##0");
                DanhSachSanPhamUC[i].txtbGiaBan.Text = Convert.ToDecimal(dong.GiaBan).ToString("#,##0");
                DanhSachSanPhamUC[i].txtbNoiBan.Text = dong.NoiBan;
                DanhSachSanPhamUC[i].txtbLoai.Text = dong.Loai;

                DanhSachSanPhamUC[i].Margin = new Thickness(8);
                DanhSachSanPhamUC[i].btnBoYeuThich.Visibility = Visibility.Collapsed;
                DanhSachSanPhamUC[i].btnThemVaoYeuThich.Visibility = Visibility.Collapsed;
                wpnlHienThiSPCungLoai.Children.Add(DanhSachSanPhamUC[i]);
                i++;

            }
        }
         
        private void LoadThongTinNguoiDang(object sender, RoutedEventArgs e)
        { 
            var linkAnh = (from nd in db.NguoiDung
                           where nd.IdNguoiDung == idNguoiDang
                           select nd.AnhNguoiDung).FirstOrDefault();

            imgAnhNguoiDang.Source = new BitmapImage(new Uri(XuLyAnh.layDuongDanDayDuToiFileAnhDaiDien(linkAnh)));

            var nguoiDang = (from nd in db.NguoiDung
                             join dgnd in db.DanhGiaNguoiDang on nd.IdNguoiDung equals dgnd.IdNguoiDang
                             group dgnd by new { nd.IdNguoiDung, nd.HoTenNguoiDung } into g
                             where g.Key.IdNguoiDung == idNguoiDang
                             select new
                             {
                                 TenNguoiDang = g.Key.HoTenNguoiDung,
                                 SoLuotDanhGia = g.Count()
                             }).FirstOrDefault();

            txtbTenNguoiDang.Text = nguoiDang.TenNguoiDang;
            txtbSoLuotDanhGia.Text = nguoiDang.SoLuotDanhGia.ToString();
        }
         

        private void ThongTinNguoiDang_Click(object sender, RoutedEventArgs e)
        {
            ThongTinNguoiDang f = new ThongTinNguoiDang(idNguoiDang);
            f.ShowDialog();
        }


        private void HienThiAnh()
        {
            string duongDanAnh = danhSachAnh[indexAnhHienTai];
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(duongDanAnh);
            bitmapImage.EndInit();

            imgAnhSP.Source = bitmapImage;
        }


        private void btnAnhTruoc_Click(object sender, RoutedEventArgs e)
        {
            if (danhSachAnh.Count > 0)
            {
                indexAnhHienTai--;
                if (indexAnhHienTai < 0)
                    indexAnhHienTai = danhSachAnh.Count - 1;

                HienThiAnh();
            }
        }

        private void btnAnhSau_Click(object sender, RoutedEventArgs e)
        {
            if (danhSachAnh.Count > 0)
            {
                indexAnhHienTai++;

                if (indexAnhHienTai >= danhSachAnh.Count)
                    indexAnhHienTai = 0;

                HienThiAnh();
            }
        }


        private void btnThemVaoGioHang_Click(object sender, RoutedEventArgs e)
        {
            GioHang gioHang = db.GioHang.Find(idNguoiMua, idSanPham);
            if (gioHang != null)
                db.GioHang.Remove(gioHang);

            gioHang = new GioHang() { IdNguoiMua =idNguoiMua, IdSanPham=idSanPham, SoLuongMua = txtbSoLuongHienTai.Text };
            db.GioHang.Add(gioHang);

            db.SaveChanges();
            
            this.Close();
        }
        
        private void btnTang_Click(object sender, RoutedEventArgs e)
        {
            int n = Convert.ToInt32(txtbSoLuongHienTai.Text);
            if (n + 1 <= Convert.ToInt32(txtbSoLuongConLai.Text))
            {
                n += 1;
            }

            txtbSoLuongHienTai.Text = n.ToString(); 
        }

        private void btnGiam_Click(object sender, RoutedEventArgs e)
        {
            int n = Convert.ToInt32(txtbSoLuongHienTai.Text);
            if (n - 1 > 0)
            {
                n -= 1;
            }
            txtbSoLuongHienTai.Text = n.ToString();
        }
    }
}
