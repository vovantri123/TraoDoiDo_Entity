using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

namespace TraoDoiDo.Views.MuaDo
{
    /// <summary>
    /// Interaction logic for TabMuaSamUC.xaml
    /// </summary>
    public partial class TabMuaSamUC : UserControl
    {
        private int soLuongSP = 0;
        private SanPhamUC[] DanhSachSanPham = new SanPhamUC[100];
        List<TrangThaiDonHang> dsSanPhamDeThanhToan = new List<TrangThaiDonHang>();
        NguoiDung ngMua;

        TraoDoiDoEntities db = new TraoDoiDoEntities();

        public TabMuaSamUC()
        {
            InitializeComponent();
        }
        public TabMuaSamUC(NguoiDung ngMua)
        {
            InitializeComponent();
            this.ngMua = ngMua;
            Loaded += MuaSam_Load;
        }

        private void MuaSam_Load(object sender, RoutedEventArgs e)
        {
            LoadDanhSachSanPham();  
            SapXepTheoGanDay();
        }

        private void LoadDanhSachSanPham() //Load dữ liệu lên cái mảng DangSachSanPham
        {
            soLuongSP = 0;
            try
            {
                var dsSanPham = from sp in db.SanPhams
                                join nd in db.NguoiDungs on sp.IdNguoiDang equals nd.IdNguoiDung
                                join dmyt in db.DanhMucYeuThiches
                                    on new { IdNguoiMua = ngMua.IdNguoiDung, sp.IdSanPham } equals new { dmyt.IdNguoiMua, dmyt.IdSanPham } into g
                                from dmyt in g.DefaultIfEmpty()
                                where sp.IdNguoiDang != ngMua.IdNguoiDung
                                select new
                                {
                                    SanPham = sp, 
                                    DanhMucYeuThich = dmyt
                                };
                 
                foreach (var dong in dsSanPham)
                {
                    int yeuThich = 0;

                    if (dong.DanhMucYeuThich != null && !string.IsNullOrEmpty(dong.DanhMucYeuThich.IdNguoiMua.ToString())) // Id người mua này của bảng yêu thích ,Neu nguoi mua co trong bang yeu thich (tức đang yêu thich một sản phẩm nào đó)
                    {
                        yeuThich = 1;
                    }

                    DanhSachSanPham[soLuongSP] = new SanPhamUC(yeuThich, ngMua.IdNguoiDung); // Khởi tạo mỗi phần tử của mảng (KHÔNG CÓ LÀ LỖI)

                    DanhSachSanPham[soLuongSP].txtbIdSanPham.Text = dong.SanPham.IdSanPham.ToString();
                    DanhSachSanPham[soLuongSP].txtbTen.Text = dong.SanPham.Ten;

                    string tenFileAnh = dong.SanPham.LinkAnhBia;
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    string duongDanhAnh = XuLyAnh.layDuongDanDayDuToiFileAnhSanPham(tenFileAnh);
                    bitmap.UriSource = new Uri(duongDanhAnh);
                    bitmap.EndInit();
                    DanhSachSanPham[soLuongSP].imgSP.Source = bitmap;

                    DanhSachSanPham[soLuongSP].txtbGiaGoc.Text = Convert.ToDecimal(dong.SanPham.GiaGoc).ToString("#,##0");
                    DanhSachSanPham[soLuongSP].txtbGiaBan.Text = Convert.ToDecimal(dong.SanPham.GiaBan).ToString("#,##0");
                    DanhSachSanPham[soLuongSP].txtbNoiBan.Text = dong.SanPham.NoiBan;
                    DanhSachSanPham[soLuongSP].txtbSoLuotXem.Text = dong.SanPham.SoLuotXem;
                    DanhSachSanPham[soLuongSP].idNguoiDang = dong.SanPham.IdNguoiDang ?? 0; //Nó báo lỗi k thể convert từ int? sang int, do kết quả có thể bị null và k thể gán cho int được
                    DanhSachSanPham[soLuongSP].txtbLoai.Text = dong.SanPham.Loai;
                    DanhSachSanPham[soLuongSP].Margin = new Thickness(8);
                     
                    soLuongSP++;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
          
        private void cboSapXep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedItem != null)
            {
                string mucDuocChon = (comboBox.SelectedItem as ComboBoxItem).Content.ToString(); 
                
                if (mucDuocChon == "Giá tăng dần")
                    SapXepTheoGiaTangDan();
                else if (mucDuocChon == "Giá giảm dần")
                    SapXepTheoGiaGiamDan();
                else if (mucDuocChon == "Lượt xem tăng dần")
                    SapXepTangDanTheoSoLuotXem();
                else if (mucDuocChon == "Lượt xem giảm dần")
                    SapXepGiamDanTheoSoLuotXem();
                else if (mucDuocChon == "Yêu thích của tôi")
                    SapXeoTheoYeuThich();
                else //"Tất cả"
                    SapXepTheoGanDay();
            }
        }

        private void LoadToanBoDanhSachSanPhamLenWpnlHienThi()
        {
            for (int i = 0; i < soLuongSP; i++)
                wpnlHienThi.Children.Add(DanhSachSanPham[i]);
        }

        private void HoanDoi(ref SanPhamUC sp1, ref SanPhamUC sp2)
        {
            SanPhamUC spTam = sp1;
            sp1 = sp2;
            sp2 = spTam;
        }

        private void SapXepTheoGiaTangDan()
        {
            wpnlHienThi.Children.Clear();
            for (int i = 0; i < soLuongSP - 1; i++)
                for (int j = i + 1; j < soLuongSP; j++)
                    if (Convert.ToInt32(DanhSachSanPham[i].txtbGiaBan.Text.Replace(",", "")) > Convert.ToInt32(DanhSachSanPham[j].txtbGiaBan.Text.Replace(",", "")))
                        HoanDoi(ref DanhSachSanPham[i], ref DanhSachSanPham[j]);

            LoadToanBoDanhSachSanPhamLenWpnlHienThi();
        }

        private void SapXepTheoGiaGiamDan()
        {
            wpnlHienThi.Children.Clear();
            for (int i = 0; i < soLuongSP - 1; i++)
                for (int j = i + 1; j < soLuongSP; j++)
                    if (Convert.ToInt32(DanhSachSanPham[i].txtbGiaBan.Text.Replace(",", "")) < Convert.ToInt32(DanhSachSanPham[j].txtbGiaBan.Text.Replace(",", "")))
                        HoanDoi(ref DanhSachSanPham[i], ref DanhSachSanPham[j]);

            LoadToanBoDanhSachSanPhamLenWpnlHienThi();
        }

        private void SapXepGiamDanTheoSoLuotXem()
        {
            wpnlHienThi.Children.Clear();
            for (int i = 0; i < soLuongSP - 1; i++)
                for (int j = i + 1; j < soLuongSP; j++)
                    if (Convert.ToInt32(DanhSachSanPham[i].txtbSoLuotXem.Text) < Convert.ToInt32(DanhSachSanPham[j].txtbSoLuotXem.Text))
                        HoanDoi(ref DanhSachSanPham[i], ref DanhSachSanPham[j]);

            LoadToanBoDanhSachSanPhamLenWpnlHienThi();
        }

        private void SapXepTangDanTheoSoLuotXem()
        {
            wpnlHienThi.Children.Clear();
            for (int i = 0; i < soLuongSP - 1; i++)
                for (int j = i + 1; j < soLuongSP; j++)
                    if (Convert.ToInt32(DanhSachSanPham[i].txtbSoLuotXem.Text) > Convert.ToInt32(DanhSachSanPham[j].txtbSoLuotXem.Text))
                        HoanDoi(ref DanhSachSanPham[i], ref DanhSachSanPham[j]);

            LoadToanBoDanhSachSanPhamLenWpnlHienThi();
        }

        private void SapXeoTheoYeuThich()
        {
            wpnlHienThi.Children.Clear();
            for (int i = 0; i < soLuongSP; i++)
                if (DanhSachSanPham[i].yeuThich == 1)
                    wpnlHienThi.Children.Add(DanhSachSanPham[i]);
            for (int i = 0; i < soLuongSP; i++)
                if (DanhSachSanPham[i].yeuThich == 0)
                    wpnlHienThi.Children.Add(DanhSachSanPham[i]);
        }

        private void SapXepTheoGanDay()
        { 
            wpnlHienThi.Children.Clear();
            var tuKhoaSanPhamDangQuanTam = (from ng in db.NguoiDungs
                                            where ng.IdNguoiDung == ngMua.IdNguoiDung
                                            select ng.TuKhoaSanPhamDangQuanTam).FirstOrDefault();
            if (tuKhoaSanPhamDangQuanTam == null)
                tuKhoaSanPhamDangQuanTam = "";

            for (int i = 0; i < soLuongSP; i++)
                if (DanhSachSanPham[i].txtbTen.Text.Trim().ToLower().Contains(tuKhoaSanPhamDangQuanTam))
                    wpnlHienThi.Children.Add(DanhSachSanPham[i]);

            for (int i = 0; i < soLuongSP; i++)
                if (!DanhSachSanPham[i].txtbTen.Text.Trim().ToLower().Contains(tuKhoaSanPhamDangQuanTam))
                    wpnlHienThi.Children.Add(DanhSachSanPham[i]); 
        }

        private void btnXacNhanTimKiem_Click(object sender, RoutedEventArgs e)
        {
            wpnlHienThi.Children.Clear();
            if (txbTimKiem.Text.Trim() != "")
            {
                wpnlHienThi.Children.Clear();
                for (int i = 0; i < soLuongSP; i++)
                {
                    string tenSP = DanhSachSanPham[i].txtbTen.Text.Trim().ToLower();
                    string timKiem = txbTimKiem.Text.Trim().ToLower();
                    if (tenSP.Contains(timKiem))
                        wpnlHienThi.Children.Add(DanhSachSanPham[i]);

                    NguoiDung nd = db.NguoiDungs.Find(ngMua.IdNguoiDung);
                    nd.TuKhoaSanPhamDangQuanTam = timKiem;
                    db.SaveChanges(); 
                }
            }
        }

        private void txbTimKiem_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txbTimKiem.Text.Trim() == "")
            {
                wpnlHienThi.Children.Clear();
                SapXepTheoGanDay();
            }   
        }
         
        private void cboLocTheoLoai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedItem != null)
            {
                string selectedItemContent = (comboBox.SelectedItem as ComboBoxItem).Content.ToString();
                if (selectedItemContent == "Tất cả")
                    SapXepTheoGanDay();
                else
                {
                    wpnlHienThi.Children.Clear();
                    for (int i = 0; i < soLuongSP; i++)
                        if (DanhSachSanPham[i].txtbLoai.Text.Contains(selectedItemContent))
                            wpnlHienThi.Children.Add(DanhSachSanPham[i]);
                }
            }
        } 
    }
}
