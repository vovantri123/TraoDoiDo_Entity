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
using TraoDoiDo;
using TraoDoiDo.ViewModels;
namespace TraoDoiDo
{
    /// <summary>
    /// Interaction logic for DanhGia.xaml
    /// </summary>
    public partial class DanhGia : Window
    {

        public int idNguoiDang;
        public int idNguoiMua; 

        TraoDoiDoEntities db = new TraoDoiDoEntities();

        public DanhGia()
        {
            InitializeComponent();
        }
        public DanhGia(int idNguoiMua, int idNguoiDang)
        {
            InitializeComponent();
            this.idNguoiMua = idNguoiMua;
            this.idNguoiDang = idNguoiDang;

            string tenFileAnh = (from nd in db.NguoiDung
                                 where nd.IdNguoiDung == idNguoiDang
                                 select nd.AnhNguoiDung).FirstOrDefault();
            imgAnhNguoiDang.Source = new BitmapImage(new Uri(XuLyAnh.layDuongDanDayDuToiFileAnhDaiDien(tenFileAnh)));
        }


        private void btnGuiDanhGia_Click(object sender, RoutedEventArgs e)
        {
            var danhGia = db.DanhGiaNguoiDang.Find(idNguoiDang, idNguoiMua);
            if(danhGia != null)
                db.DanhGiaNguoiDang.Remove(danhGia);

            danhGia = new DanhGiaNguoiDang() { IdNguoiDang = idNguoiDang, IdNguoiMua = idNguoiMua, SoSao = Convert.ToInt32(ratingBarSoSao.Value), NhanXet = txtbDanhGia.Text };
            db.DanhGiaNguoiDang.Add(danhGia);

            db.SaveChanges();

            MessageBox.Show("Cảm ơn bạn đã gửi đánh giá\nChúc bạn một ngày thật vui vẻ", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Information);
        }
    }
}
