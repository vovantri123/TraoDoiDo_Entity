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
    /// Interaction logic for DiaChiNhanHang.xaml
    /// </summary>
    public partial class DiaChi : Window
    {
        public string tongThanhToan;
        public int idVoucher = -1;
        List<TrangThaiDonHang> dsSanPhamDeThanhToan = new List<TrangThaiDonHang>();
        NguoiDung ngDung = new NguoiDung(); // người tương đối (mua hoặc bán)

        TraoDoiDoEntities db = new TraoDoiDoEntities();

        public DiaChi()
        {
            InitializeComponent();
        }

        public DiaChi(NguoiDung kh, List<TrangThaiDonHang> dsSanPhamDeThanhToan)
        {
            InitializeComponent();
            ngDung = kh;
            this.dsSanPhamDeThanhToan = dsSanPhamDeThanhToan;
            Loaded += FDiaChi_Loaded;
        }


        private void btnXacNhanThanhToan_Click(object sender, RoutedEventArgs e)
        {
            capNhatThongTinCaNhan();
            foreach (var dong in dsSanPhamDeThanhToan)
            {
                var nguoiDang = (from sp in db.SanPhams
                                 join nd in db.NguoiDungs on sp.IdNguoiDang equals nd.IdNguoiDung
                                 where sp.IdSanPham == dong.IdSanPham
                                 select nd).FirstOrDefault();


                var quanLyDonHang = (from qldh in db.QuanLyDonHangs
                                     where qldh.IdNguoiDang == nguoiDang.IdNguoiDung && qldh.IdNguoiMua == ngDung.IdNguoiDung && qldh.IdSanPham == dong.IdSanPham && qldh.TrangThai == "Chờ đóng gói"
                                     select qldh).FirstOrDefault();
                if (quanLyDonHang != null)
                    db.QuanLyDonHangs.Remove(quanLyDonHang); //Xóa trước khi thêm, do ràng buộc unique //quanLyDonHang nay bên phía Người Bán
                quanLyDonHang = new QuanLyDonHang() { IdNguoiDang = nguoiDang.IdNguoiDung, IdNguoiMua = ngDung.IdNguoiDung, IdSanPham = dong.IdSanPham, TrangThai = "Chờ đóng gói" };
                db.QuanLyDonHangs.Add(quanLyDonHang);


                var trangThaiDonHang = db.TrangThaiDonHangs.Find(dong.IdNguoiMua, dong.IdSanPham);
                if (trangThaiDonHang != null)
                    db.TrangThaiDonHangs.Remove(trangThaiDonHang); //Trạng thái don hàng bên phía Người Mua 
                trangThaiDonHang = new TrangThaiDonHang() { IdNguoiMua = dong.IdNguoiMua, IdSanPham = dong.IdSanPham, SoLuongMua = dong.SoLuongMua, TongThanhToan = dong.TongThanhToan, Ngay = dong.Ngay, TrangThai = dong.TrangThai };
                db.TrangThaiDonHangs.Add(trangThaiDonHang);

                var giohang = db.GioHangs.Find(dong.IdNguoiMua, dong.IdSanPham);
                db.GioHangs.Remove(giohang);
            }

            if (idVoucher != -1)
            {
                int idvc = Convert.ToInt32(idVoucher);
                NguoiDungVoucher nguoiDungVoucher = db.NguoiDungVouchers.Find(ngDung.IdNguoiDung, idvc);
                db.NguoiDungVouchers.Remove(nguoiDungVoucher);

                var soLuotDaSuDung = (from vc in db.Vouchers
                                      where vc.IdVoucher == idvc
                                      select vc.SoLuotDaSuDung).FirstOrDefault();
                NguoiDungVoucher nguoidungVoucher = db.NguoiDungVouchers.Find(ngDung.IdNguoiDung, nguoiDungVoucher.IdVoucher);
                db.NguoiDungVouchers.Remove(nguoidungVoucher);

                Voucher voucher = db.Vouchers.Find(nguoidungVoucher.IdVoucher);
                voucher.SoLuotDaSuDung = (Convert.ToInt32(soLuotDaSuDung) + 1).ToString();

            }

            double tienTT = Convert.ToDouble(ngDung.TienNguoiDung) - Convert.ToDouble(tongThanhToan);
            if (Convert.ToDouble(tongThanhToan) == 0)
                MessageBox.Show("Xin hãy chọn món đồ thanh toán", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            if (tienTT < 0)
                MessageBox.Show("Số tiền trong tài khoản của bạn không đủ vui lòng nạp tiền thêm!!!!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            else
            {
                NguoiDung nguoi = db.NguoiDungs.Find(ngDung.IdNguoiDung);
                nguoi.TienNguoiDung = tienTT.ToString();
                MessageBox.Show("Thanh toán thành công");
            }
            db.SaveChanges(); 
        }
        private void capNhatThongTinCaNhan() // ???????????, không biết sửa sao luôn, tại chưa coi mấy cái model check này
        {

        }

        private void FDiaChi_Loaded(object sender, RoutedEventArgs e)
        {
            txtHoTen.Text = ngDung.HoTenNguoiDung.ToString();
            txtSoDienThoai.Text = ngDung.SdtNguoiDung.ToString();
            txtEmail.Text = ngDung.EmailNguoiDung.ToString();
            txtDiaChi.Text = ngDung.DiaChiNguoiDung.ToString();
        }
    }
}
