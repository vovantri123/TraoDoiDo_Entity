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
namespace TraoDoiDo
{
    /// <summary>
    /// Interaction logic for Voucher.xaml
    /// </summary>
    public partial class VoucherUC : UserControl
    {
        private int idNguoiMua;
        
        TraoDoiDoEntities db = new TraoDoiDoEntities();

        public VoucherUC()
        {
            InitializeComponent();
        }

        public VoucherUC(int idNguoiMua)
        {
            InitializeComponent();
            this.idNguoiMua = idNguoiMua;
        }

        #region TỪ UC con truyền ngược lên UC cha
        // Tạo lớp hoặc cặp tham số mới để đại diện cho cả hai giá trị
        public class TextBlockChangedEventArgs : EventArgs
        {
            public string GiaTriMoi { get; set; }
            public string IdMoi { get; set; }
        }

        // Sự kiện được phát ra khi TextBlock thay đổi
        public event EventHandler<TextBlockChangedEventArgs> TextBlockChanged;

        private void btnDungVoucher_Click(object sender, RoutedEventArgs e)
        {
            // Lấy giá trị từ TextBox và gửi đến form cha
            TextBlockChanged?.Invoke(this, new TextBlockChangedEventArgs
            {
                GiaTriMoi = txtbGiaTri.Text,
                IdMoi = txtbIdVoucher.Text
            });
        }
        #endregion
         
        private void btnNhanVoucher_Click(object sender, RoutedEventArgs e)
        {
            // Thêm voucher vào bảng NguoiDungVoucher, nếu tồn tại trước đó rồi thì xóa cái cũ đi và thêm 
            NguoiDungVoucher ndvcMoi = new NguoiDungVoucher() { IdVoucher = Convert.ToInt32(txtbIdVoucher.Text), IdNguoiDung = idNguoiMua};
            NguoiDungVoucher ndvc = db.NguoiDungVoucher.Find(Convert.ToInt32(txtbIdVoucher.Text), idNguoiMua);
            if(ndvc != null) 
                db.NguoiDungVoucher.Remove(ndvc);
            db.NguoiDungVoucher.Add(ndvcMoi);
            db.SaveChanges();
            MessageBox.Show("Bạn đã nhận được voucher");
        } 
    }
}
