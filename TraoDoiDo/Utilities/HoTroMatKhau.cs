﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using MaterialDesignThemes.Wpf;
using System.Dynamic;
namespace TraoDoiDo.ViewModels
{
    public static class HoTroMatKhau
    {
        public static readonly DependencyProperty Password = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(HoTroMatKhau), new PropertyMetadata(string.Empty,OnBoundPasswordChanged));

        public static readonly DependencyProperty BindPassword = DependencyProperty.RegisterAttached("BindPassword", typeof(bool), typeof(HoTroMatKhau), new PropertyMetadata(false,OnBindPasswordChanged));

        private static readonly DependencyProperty UpdatingPassword = DependencyProperty.RegisterAttached("UpdatingPassword",typeof(bool), typeof(HoTroMatKhau),new PropertyMetadata(false));

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox;
            if (d == null || !GetBindPassword(d)) 
                return;
            passwordBox.PasswordChanged -= HandlePasswordChanged;
            string newPassword = (string)e.NewValue;
            if (!GetUpdatingPassword(passwordBox))
                passwordBox.Password = newPassword;
            passwordBox.PasswordChanged += HandlePasswordChanged;
        }

        private static void OnBindPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox;
            if (passwordBox == null) return;
            bool wasBound = (bool)e.OldValue;
            bool needToBind = (bool)e.NewValue;
            if (wasBound)
                passwordBox.PasswordChanged -= HandlePasswordChanged;
            if (needToBind)
                passwordBox.PasswordChanged += HandlePasswordChanged;
        }
        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            SetUpdatingPassword(passwordBox, true);
            SetBoundPassword(passwordBox, passwordBox.Password);
            SetUpdatingPassword(passwordBox, false);
        }
        public static void SetBindPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(BindPassword, value);
        }
        public static bool GetBindPassword(DependencyObject dp)
        {
            return (bool)dp.GetValue(BindPassword);
        }
        public static string GetBoundPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(Password);
        }
        public static void SetBoundPassword(DependencyObject dp, string value)
        {
            dp.SetValue(Password, value);
        }
        private static bool GetUpdatingPassword(DependencyObject dp)
        {
            return (bool)dp.GetValue(UpdatingPassword);
        }
        private static void SetUpdatingPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(UpdatingPassword, value);
        }
    }
}
