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

        public DangDo_Dang()
        {
            InitializeComponent();
        }
 
        private void themThongTinVaoCSDL()
        {
             

        }
        private void themAnhVaMoTaVaoCSDL()
        {
             
        }

        private void btnDang_Click(object sender, RoutedEventArgs e)
        {
            

        }

        private void btnThemAnh_Click(object sender, RoutedEventArgs e)
        {
            DanhSachAnhVaMoTa[soLuongAnh] = new ThemAnhKhiDangUC();
            ucThongTin.wpnlChuaAnh.Children.Add(DanhSachAnhVaMoTa[soLuongAnh]);
            soLuongAnh++;
        }
    }
}
