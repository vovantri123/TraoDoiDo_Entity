using LiveCharts.Wpf;
using LiveCharts;
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
using System.Windows.Shapes;
using LiveCharts.Wpf.Charts.Base;


namespace TraoDoiDo.Views.QuanLy
{
    /// <summary>
    /// Interaction logic for QuanLyDoanhThuNguoiDung.xaml
    /// </summary>
    public partial class QuanLyDoanhThuNguoiDung : Window
    {
        TraoDoiDoEntities db = new TraoDoiDoEntities();
        NguoiDung nguoiDung = new NguoiDung();
        List<string> dsNhan = new List<string>(12)
                {
                    "1","2","3","4","5","6","7","8","9","10","11","12",
                };
        int countLoad = 0;
        public QuanLyDoanhThuNguoiDung()
        {
            InitializeComponent();
        }
        public QuanLyDoanhThuNguoiDung(NguoiDung nguoiDung)
        {
            InitializeComponent();
            this.nguoiDung = nguoiDung;

        }
        #region Hàm load đồ thị
        private void LoadDoThi(string idNguoi, string nam)
        {
            List<int> dsCotDoanhThu = Enumerable.Repeat(0,12).ToList();
            List<int> dsCotSoLuongDaBan = Enumerable.Repeat(0, 12).ToList();
            List<int> dsCotSoLuongKH = Enumerable.Repeat(0, 12).ToList();
            int id = Convert.ToInt32(idNguoi);
            // Load danh sách doanh thu sản phẩm
            var dsDoanhThuSanPham =  (from t in db.TrangThaiDonHang
                                      join sp in db.SanPham
                                      on t.IdSanPham equals sp.IdSanPham
                                      where sp.IdNguoiDang == id &&
                                      t.TrangThai == "Đã nhận" &&
                                      t.Ngay.Substring(6, 4) == nam
                                      select new
                                      {
                                      IdNguoiDang = sp.IdNguoiDang,
                                      IdSanPham = sp.IdSanPham,
                                      SoLuongDaBan = sp.SoLuongDaBan,
                                      SoLuong = sp.SoLuong,
                                      Ngay = t.Ngay,
                                      TongThanhToan = t.TongThanhToan
                                       }).ToList();
            foreach (var dong in dsDoanhThuSanPham)
            {
                DateTime t = DateTime.ParseExact(dong.Ngay, "d/M/yyyy", null);
                int thang = t.Month;
                dsCotDoanhThu[thang - 1] += Convert.ToInt32(dong.TongThanhToan);
                dsCotSoLuongDaBan[thang - 1] += Convert.ToInt32(dong.SoLuongDaBan);
            }
            for (int i = 1; i <= 12; i++)
            {
                string thang = i.ToString();
                //đếm số lượng khách hàng mua sản phẩm từ một người đăng (người bán) cụ thể trong một tháng và năm cụ thể khi đơn hàng có trạng thái là "Đã nhận"
                var soLuongKH = (from tt in db.TrangThaiDonHang
                                 join sp in db.SanPham
                                 on tt.IdSanPham equals sp.IdSanPham
                                 where tt.TrangThai == "Đã nhận" &&
                                        tt.Ngay.Substring(6, 4).Contains(nam) &&
                                        tt.Ngay.Substring(3, 2).Contains(thang) &&
                                        sp.IdNguoiDang == id
                                 group tt by sp.IdNguoiDang into ttGroup
                                 select new
                                 {
                                     TongSoKhachHang = ttGroup.Select(t => t.IdNguoiMua).Distinct().Count()
                                 }).ToList();
                foreach (var s in soLuongKH)
                {
                    int sl = Convert.ToInt32(s.TongSoKhachHang);
                    dsCotSoLuongKH[i - 1] += sl;
                }
               
            }
            
            HamVeDoThiCot(dsCotDoanhThu, chart_TongDoanhThuTheoSanPham, "Doanh thu", "Tháng", "Doanh thu");
            HamVeDoThiTron(dsCotDoanhThu, chart_TiLePhanTramDoanhThuTheoSanPham);
            HamVeDoThiCot(dsCotSoLuongDaBan, chart_SoLuongSanPhamDaBan, "Số lượng đã bán", "Tháng", "Số lượng");
            HamVeDoThiTron(dsCotSoLuongDaBan, chart_TiLePhanTramSoLuongSanPham);
            HamVeDoThiCot(dsCotSoLuongKH, chart_SoLuongKhachHang, "Số lượng khách hàng", "Tháng", "Số lượng");
            HamVeDoThiTron(dsCotSoLuongKH, chart_TiLePhanTramSoLuongKhachHang);
            
            txtbDoanhThuCaoNhat.Text = Convert.ToDecimal(dsCotDoanhThu.Sum()).ToString("#,##0");
            txtbTongSoLuongSanPhamDaBan.Text = Convert.ToDecimal(dsCotSoLuongDaBan.Sum()).ToString("#,##0");
            txtbTongSoLuongKhachHang.Text = Convert.ToDecimal(dsCotSoLuongKH.Sum()).ToString("#,##0");

        }
        #endregion
        private void cboNam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ComboBox comboBox = sender as ComboBox;

            string selectedItemContent = comboBox.SelectedItem.ToString().Trim();
            LoadDoThi(nguoiDung.IdNguoiDung.ToString(), selectedItemContent);
        }

        private void FQuanLyDoanhThu_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDoThi(nguoiDung.IdNguoiDung.ToString(), "2024");
            countLoad = 1;
        }

        private void cboNam_Loaded(object sender, RoutedEventArgs e)
        {
            int namHienTai = DateTime.Now.Year;
            int namTruoc = namHienTai - 1;
            int namTruocTruoc = namTruoc - 1;
            List<string> nam = new List<string>()
            {
                $"{namHienTai}",$"{namTruoc}",$"{namTruocTruoc}",
            };
            foreach (var x in nam)
            {
                cboNam.Items.Add(x);
            }
        }
        private void HamVeDoThiTron(List<int> dsCot, Chart doThi)
        {
            if (countLoad > 0)
            {
                doThi.Series.Clear();
                
            }
            SeriesCollection TiLePhanTramDoanhThuTheoSanPham_SC = new SeriesCollection();

            for (int i = 0; i < dsNhan.Count; i++)
            {
                TiLePhanTramDoanhThuTheoSanPham_SC.Add(new PieSeries
                {
                    Title = dsNhan[i],  
                    Values = new ChartValues<int> { dsCot[i] } 

                });
            }

            doThi.Series = TiLePhanTramDoanhThuTheoSanPham_SC;

        }
        private void HamVeDoThiCot(List<int> dsCot, Chart doThi, string tieuDe, string X, string Y)
        {
            if (countLoad > 0)
            {
                doThi.Series.Clear();
                doThi.AxisX.Clear();
                doThi.AxisY.Clear();
            }
            SeriesCollection DoanhThuTheoSanPham_SC = new SeriesCollection
                    {
                         new ColumnSeries
                         {
                             Title = $"{tieuDe}",
                             Values = new ChartValues<int> (dsCot)
                         }
                    };


            doThi.Series = DoanhThuTheoSanPham_SC;

            var xAxis = new LiveCharts.Wpf.Axis
            {
                Title = $"{X}",
                FontSize = 9.5,
                Labels = dsNhan.ToArray(),
                Separator = LiveCharts.Wpf.DefaultAxes.CleanSeparator
            };
            doThi.AxisX.Add(xAxis);

            var yAxis = new LiveCharts.Wpf.Axis
            {
                Title = $"{Y}",
                FontSize = 9.5,
                LabelFormatter = value => value.ToString("#,##0")  
            };
            doThi.AxisY.Add(yAxis);

        }
    }
}
