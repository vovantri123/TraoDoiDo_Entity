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
using TraoDoiDo.Utilities;
using TraoDoiDo;
namespace TraoDoiDo
{
    /// <summary>
    /// Interaction logic for LyDoTraHangUC.xaml
    /// </summary>
    public partial class LyDoTraHangUC : UserControl
    {
        public int idNguoiMua;
        public int idSP; 
        public event EventHandler DrawerClosed;
       
        TraoDoiDoEntities db = new TraoDoiDoEntities();

        public LyDoTraHangUC()
        {
            InitializeComponent();
        }

        private void btnXacNhanTraHang_Click(object sender, RoutedEventArgs e)
        {
            var trangThai = db.TrangThaiDonHang.Find(idNguoiMua, idSP);
            trangThai.TrangThai = "Đã trả hàng";

            var quanLyDonHang = (from qldh in db.QuanLyDonHang
                                 where qldh.IdNguoiMua == idNguoiMua && qldh.IdSanPham == idSP
                                 select qldh).FirstOrDefault();
            quanLyDonHang.TrangThai = "Bị hoàn trả";
            quanLyDonHang.LyDoTraHang = timLyDoDuocChon();

            db.SaveChanges();
             
            btnXacNhanTraHang.IsEnabled = false; 
            // Tìm DrawerHost gần nhất
            DependencyObject cha = VisualTreeHelper.GetParent(this);
            while (!(cha is DrawerHost))
            {
                cha = VisualTreeHelper.GetParent(cha);
            }

            // Ẩn DrawerHost 
            DrawerHost drawerHost = cha as DrawerHost;
            if (drawerHost != null)
            {
                drawerHost.IsBottomDrawerOpen = false;
                OnDrawerClosed(EventArgs.Empty);
            } 
        }

        protected virtual void OnDrawerClosed(EventArgs e)
        {
            DrawerClosed?.Invoke(this, e);
        }

        private string timLyDoDuocChon()
        {
            // Duyệt qua từng Border trong Grid
            foreach (var border in GridLyDo.Children)
            {
                if (border is Border)
                {
                    var radioButton = HoTroTimPhanTu.FindVisualChild<RadioButton>(border as Border);
                    if (radioButton != null && radioButton.IsChecked == true)
                    { 
                        return radioButton.Content.ToString(); 
                    }
                }
            }
            return "";
        } 
    }
}
