﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes; 
using TraoDoiDo.Utilities;
using TraoDoiDo.ViewModels;
using TraoDoiDo;

namespace TraoDoiDo.Views.DangDo
{
    /// <summary>
    /// Interaction logic for TabQuanLyDonHangUC.xaml
    /// </summary>
    public partial class TabQuanLyDonHangUC : UserControl
    {
        List<SanPham> dsSanPham;

        SanPham sanPham;
        NguoiDung nguoiDang;
        MoTaAnhSanPham moTaHangHoa;

        TraoDoiDoEntities db = new TraoDoiDoEntities();


        public TabQuanLyDonHangUC()
        {
            InitializeComponent();
        }

        public TabQuanLyDonHangUC(NguoiDung nguoiDang)
        {
            InitializeComponent();
            Loaded += QuanLyDonHang_Load;
            this.nguoiDang = nguoiDang;
        }

        private void QuanLyDonHang_Load(object sender, RoutedEventArgs e)
        {
            LoadLsvTrongTabQuanLyDonHang("lsvChoDongGoi", "Chờ đóng gói");
            LoadLsvTrongTabQuanLyDonHang("lsvDangGiao", "Đang giao");
            LoadLsvTrongTabQuanLyDonHang("lsvDaGiao", "Đã giao");
            LoadLsvTrongTabQuanLyDonHang("lsvDonHangBiHoanTra", "Bị hoàn trả");
        }
        private void LoadLsvTrongTabQuanLyDonHang(string tenLsv, string trangthai)
        {
            //Load danh sách đơn hàng
            var dsDonHang = (from qldh in db.QuanLyDonHang
                             join nd in db.NguoiDung on qldh.IdNguoiMua equals nd.IdNguoiDung
                             join sp in db.SanPham on qldh.IdSanPham equals sp.IdSanPham
                             join ttdh in db.TrangThaiDonHang on new { IdNguoiMua = (int)qldh.IdNguoiMua, IdSanPham = (int)qldh.IdSanPham } equals new { ttdh.IdNguoiMua, ttdh.IdSanPham }
                             where qldh.IdNguoiDang == nguoiDang.IdNguoiDung && qldh.TrangThai == trangthai
                             select new
                             {
                                 QuanLyDonHang = qldh,
                                 SanPham = sp,
                                 TrangThaiDonHang = ttdh
                             }).ToList();

            if (tenLsv == "lsvChoDongGoi")
                lsvChoDongGoi.Items.Clear();
            else if (tenLsv == "lsvDangGiao")
                lsvDangGiao.Items.Clear();
            else if (tenLsv == "lsvDaGiao")
                lsvDaGiao.Items.Clear();
            else if (tenLsv == "lsvDonHangBiHoanTra")
                lsvDonHangBiHoanTra.Items.Clear();

            foreach (var dong in dsDonHang)
            {
                string tenFileAnh = dong.SanPham.LinkAnhBia;
                string linkAnhBia = XuLyAnh.layDuongDanDayDuToiFileAnhSanPham(tenFileAnh);

                if (tenLsv == "lsvChoDongGoi")
                    lsvChoDongGoi.Items.Add(new { IdSP = dong.SanPham.IdSanPham, IdNguoiMua = dong.QuanLyDonHang.IdNguoiMua, TenSP = dong.SanPham.Ten, LinkAnhBia = linkAnhBia, SoLuongMua = dong.TrangThaiDonHang.SoLuongMua, Gia = dong.SanPham.GiaBan, PhiShip = dong.SanPham.PhiShip, TongTien = dong.TrangThaiDonHang.TongThanhToan});
                else if (tenLsv == "lsvDangGiao")
                    lsvDangGiao.Items.Add(new { IdSP = dong.SanPham.IdSanPham, IdNguoiMua = dong.QuanLyDonHang.IdNguoiMua, TenSP = dong.SanPham.Ten, LinkAnhBia = linkAnhBia, SoLuongMua = dong.TrangThaiDonHang.SoLuongMua, Gia = dong.SanPham.GiaBan, PhiShip = dong.SanPham.PhiShip, TongTien = dong.TrangThaiDonHang.TongThanhToan });
                else if (tenLsv == "lsvDaGiao")
                    lsvDaGiao.Items.Add(new { IdSP = dong.SanPham.IdSanPham, IdNguoiMua = dong.QuanLyDonHang.IdNguoiMua, TenSP = dong.SanPham.Ten, LinkAnhBia = linkAnhBia, SoLuongMua = dong.TrangThaiDonHang.SoLuongMua, Gia = dong.SanPham.GiaBan, PhiShip = dong.SanPham.PhiShip, TongTien = dong.TrangThaiDonHang.TongThanhToan });
                else if (tenLsv == "lsvDonHangBiHoanTra")
                    lsvDonHangBiHoanTra.Items.Add(new { IdSP = dong.SanPham.IdSanPham, IdNguoiMua = dong.QuanLyDonHang.IdNguoiMua, TenSP = dong.SanPham.Ten, LinkAnhBia = linkAnhBia, SoLuongMua = dong.TrangThaiDonHang.SoLuongMua, Gia = dong.SanPham.GiaBan, PhiShip = dong.SanPham.PhiShip, TongTien = dong.TrangThaiDonHang.TongThanhToan, LyDoTraHang = dong.QuanLyDonHang.LyDoTraHang });

            }
        }


        private void btnDiaChiGuiHang_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ListViewItem dongChuaButton = HoTroTimPhanTu.FindAncestor<ListViewItem>(btn);
            dynamic duLieuCuaDongChuaButton = dongChuaButton.DataContext;

            if (duLieuCuaDongChuaButton != null)
            {
                int idNguoiMua = Convert.ToInt32(duLieuCuaDongChuaButton.IdNguoiMua);
                int idSP = Convert.ToInt32(duLieuCuaDongChuaButton.IdSP);
                // Load dữ liệu người mua 
                NguoiDung nguoi = (from ttdh in db.TrangThaiDonHang
                                   join nd in db.NguoiDung on ttdh.IdNguoiMua equals nd.IdNguoiDung
                                   where ttdh.IdNguoiMua == idNguoiMua  && ttdh.IdSanPham == idSP
                                   select nd
                                   ).FirstOrDefault();

                DiaChi f = new DiaChi(nguoi);
                f.txtbTieuDe.Text = "Địa chỉ khách hàng";
                f.btnXacNhanThanhToan.Visibility = Visibility.Collapsed;
                f.ShowDialog();
            }
        }
        private void btnGuiHang_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ListViewItem dongChuaButton = HoTroTimPhanTu.FindAncestor<ListViewItem>(btn);
            dynamic duLieuCuaDongChuaButton = dongChuaButton.DataContext;

            if (duLieuCuaDongChuaButton != null)
            {
                try
                {
                    int idNguoiMua = Convert.ToInt32(duLieuCuaDongChuaButton.IdNguoiMua);
                    int idSP = Convert.ToInt32(duLieuCuaDongChuaButton.IdSP);
                    //Cập nhật trạng thái đơn hàng ở hai bảng là QuanLyDonHang và TrangThaiDonHang
                    QuanLyDonHang quanLy = (from qldh in db.QuanLyDonHang
                                            where qldh.IdNguoiMua == idNguoiMua && qldh.IdSanPham == idSP
                                            select qldh).FirstOrDefault();
                    quanLy.TrangThai = "Đang giao";

                    TrangThaiDonHang trangThaiDon = (from ttdh in db.TrangThaiDonHang
                                                     where ttdh.IdNguoiMua == idNguoiMua && ttdh.IdSanPham == idSP
                                                     select ttdh).FirstOrDefault();
                    trangThaiDon.TrangThai = "Chờ giao hàng"; 

                    db.SaveChanges(); 

                    QuanLyDonHang_Load(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi:" + ex.Message);
                }
            }
        }

    }
}
