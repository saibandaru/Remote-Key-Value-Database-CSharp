﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for Page7.xaml
    /// </summary>
    public partial class Page21 : Page
    {
        public Page21()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Can I help you? Well, not yet.");
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page1());
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page2());
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page3());
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page4());
        }
        private void MenuItem_Click_11(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page2());
        }
        private void MenuItem_Click_12(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page21());
        }
        private void MenuItem_Click_13(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page22());
        }
    }
}
