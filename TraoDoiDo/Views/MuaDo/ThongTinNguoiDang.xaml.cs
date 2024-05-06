using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ThongTinNguoiDang.xaml
    /// </summary>
    /// 
     
    public partial class ThongTinNguoiDang : Window
    { 
        TraoDoiDoEntities db = new TraoDoiDoEntities();
       
        public ThongTinNguoiDang() 
        {
            InitializeComponent(); 
        }

        public ThongTinNguoiDang(int idNguoiDang)
        {
            InitializeComponent(); 
            LoadThongTinNguoiDang(idNguoiDang);
            LoadDSDanhGia(idNguoiDang);
        }


        private void LoadThongTinNguoiDang(int idNguoiDang)
        { 
            var nguoiDang = (from dgnd in db.DanhGiaNguoiDangs
                             join nd in db.NguoiDungs on dgnd.IdNguoiDang equals nd.IdNguoiDung
                             where dgnd.IdNguoiDang == idNguoiDang
                             select nd).FirstOrDefault();   


            itemsControlDSDanhGia.Items.Clear();
            txtHoTen.Text = nguoiDang.HoTenNguoiDung;
            txtSoDienThoai.Text = nguoiDang.SdtNguoiDung;
            txtEmail.Text = nguoiDang.EmailNguoiDung;
            txtDiaChi.Text = nguoiDang.DiaChiNguoiDung;
            imgNguoiDang.Source = new BitmapImage(new Uri(XuLyAnh.layDuongDanDayDuToiFileAnhDaiDien(nguoiDang.AnhNguoiDung)));
        }

        private void LoadDSDanhGia(int idNguoiDang)
        {
            var dsDanhGia = (from dgnd in db.DanhGiaNguoiDangs
                             join nd in db.NguoiDungs on dgnd.IdNguoiMua equals nd.IdNguoiDung
                             where dgnd.IdNguoiDang == idNguoiDang
                             select new
                             { 
                                 TenNguoiMua = nd.HoTenNguoiDung,
                                 Anh = nd.AnhNguoiDung,
                                 NhanXet = dgnd.NhanXet,
                                 SoSao = dgnd.SoSao
                             } );
            itemsControlDSDanhGia.Items.Clear();
            foreach (var dong in dsDanhGia)
            {
                itemsControlDSDanhGia.Items.Add(new { Ten = dong.TenNguoiMua, SoSao = dong.SoSao, NhanXet = dong.NhanXet, LinkAnhDaiDienNguoiDanhGia = XuLyAnh.layDuongDanDayDuToiFileAnhDaiDien(dong.Anh) });
            }
        }
    }
}
