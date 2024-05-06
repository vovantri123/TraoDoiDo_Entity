using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging; 
using TraoDoiDo.Utilities;
using TraoDoiDo.ViewModels;
using TraoDoiDo.Views.DangDo;

namespace TraoDoiDo
{ 
    public partial class DangDoUC : UserControl
    {  
        public DangDoUC()
        {
            InitializeComponent();
        }
        public DangDoUC(NguoiDung nguoi)
        {
            InitializeComponent();
            ccTabQuanLySanPham.Content = new TabSanPhamDaDangUC(nguoi);
            //ccTabQuanLyDonHang.Content = new TabQuanLyDonHangUC(nguoi);
            //ccTabThongKe.Content = new TabThongKeUC(nguoi);
        }
    }
}

