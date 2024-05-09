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
using System.Windows.Shapes;

namespace TraoDoiDo
{
    /// <summary>
    /// Interaction logic for Thoat.xaml
    /// </summary>
    public partial class Thoat : Window
    {
        public Thoat()
        {
            InitializeComponent();
        }

        private void btnCo_Click(object sender, RoutedEventArgs e)
        { 
            foreach (Window window in Application.Current.Windows) 
                if(window!= Application.Current.MainWindow)
                    window.Close(); 
             
            Application.Current.MainWindow.Show();
        }

        private void btnKhong_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnCo_Click(sender, e);
            }
        }
    }
}
