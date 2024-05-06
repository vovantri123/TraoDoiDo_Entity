using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes; 
using TraoDoiDo.ViewModels; 
using System.Xml.Linq;
using TraoDoiDo.Views.DangDo;
using TraoDoiDo.Views.MuaDo;
using TraoDoiDo.Views.Windows;
namespace TraoDoiDo
{
    /// <summary>
    /// Interaction logic for MuaDo.xaml
    /// </summary>
    public partial class MuaDoUC : UserControl
    {  
        public MuaDoUC()
        {
            InitializeComponent();  
        }
        public MuaDoUC(NguoiDung nguoi)
        {
            InitializeComponent();
            ccTabMuaSam.Content = new TabMuaSamUC(nguoi);
            ccTabGioHang.Content = new TabGioHangUC(nguoi);
            ccTabTrangThaiDonHang.Content = new TabTrangThaiDonHangUC(nguoi);
        }
    }
}
