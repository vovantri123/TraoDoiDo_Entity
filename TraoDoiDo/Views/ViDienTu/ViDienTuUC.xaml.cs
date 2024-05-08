using MaterialDesignThemes.Wpf;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TraoDoiDo.ViewModels;

namespace TraoDoiDo
{
    /// <summary>
    /// Interaction logic for ViDienTuUC.xaml
    /// </summary>
    public partial class ViDienTuUC : UserControl
    { 
        NguoiDung nguoiDung;

        TraoDoiDoEntities db = new TraoDoiDoEntities();

        public ViDienTuUC()
        {
            InitializeComponent();
        }

        public ViDienTuUC(NguoiDung nguoiDung)
        {
            InitializeComponent();
            this.nguoiDung = nguoiDung;
            imageHinhDaiDien.Source = new BitmapImage(new Uri(XuLyAnh.layDuongDanDayDuToiFileAnhDaiDien(nguoiDung.AnhNguoiDung)));
            Loaded += UcViDienTU_Loaded;
        }

        private void btnNapTien_Click(object sender, RoutedEventArgs e)
        {
            NapRutTien f = new NapRutTien(nguoiDung);
            f.ShowDialog();
        }

        private void btnRutTien_Click(object sender, RoutedEventArgs e)
        {
            NapRutTien f = new NapRutTien(nguoiDung);
            f.txtbTieuDe.Text = "Rút tiền";
            f.txtbTu.Text = "Rút tiền từ";
            f.txtbDen.Text = "Đến ngân hàng";
            f.ShowDialog();
        }

        private void UcViDienTU_Loaded(object sender, RoutedEventArgs e)
        {
            HienThi_GiaoDich();
            string t = (from nd in db.NguoiDung
                        where nd.IdNguoiDung == nguoiDung.IdNguoiDung
                        select nd.TienNguoiDung).FirstOrDefault();
            double tien = Convert.ToDouble(t);
            txtbSoDu.Text = DinhDangTien(tien);
        }

        private void HienThi_GiaoDich()
        {
            var dsGiaoDich = (from gd in db.GiaoDich
                              where gd.IdNguoiDung == nguoiDung.IdNguoiDung
                              select gd).ToList();
            foreach (var dong in dsGiaoDich)
                lsvLichSuGiaoDich.Items.Add(new { Id = dong.IdGiaoDich, Type = dong.LoaiGiaoDich, Money = dong.SoTien, Initial = dong.TuNguonGiaoDich, End = dong.DenNguonGiaoDich, Date = dong.NgayGiaoDich });
        }

        private static string DinhDangTien(double t)
        {
            return t.ToString("#,##0");
        }
        

    }
}
