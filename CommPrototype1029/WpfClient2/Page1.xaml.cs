/*/////////////////////////////////////////////////////////////
// Page1.xaml.cs - UI Used as WPF READ and WRITE client's    //
//                 Connect Tabfor Project4                   //
//                                                           //
// Ver 1.0                                                   //
// Application: Demonstration for CSE681-SMA, Project#4      //
// Language:    C#, ver 6.0, Visual Studio 2015              //
// Platform:    ASUS SonicMaster, Core-i3, Windows 10        //
// Author:      Sai Krishna, Syracuse University             //
//              (813) 940-8083, sbandaru@syr.edu             //
///////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package has UI Used as WPF READ and WRITE client's Connect Tab for WPF Project4
 *
 *  Public Interface
 *===================
 *
 *  Page1()     Constructor that initalizes Page1
 *
 */
/*
 * Maintenance History:
 * --------------------
 * ver 1.0 : 15 Nov 2015
 * - first release
 *
 * Build Process:  devenv CommPrototype.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 */
using System;
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

namespace Project4Starter
{
    ///////////////////////////////////////////////////////////////////////
    // Page1 class
    // - provides the performance UI for read and writer client's Connect Tab
    //
    public partial class Page1 : Page
    {
        //----< Constructor that initalizes Page1 >---------------------
        public Page1()
        {
            InitializeComponent();
            commonInfo.regReq(reqList1);
            commonInfo.respReq(respList1);
            if (commonInfo.reqmsgs != null && commonInfo.respmsgs != null)
            {
                commonInfo.initReq(reqList1);
                commonInfo.initResp(respList1);
            }
            if (commonInfo.isConnect) { connect_button.Content = "Connected"; connect_button.IsEnabled = false; remoteAddr.IsEnabled = false;
                remotePort.IsEnabled = false;
            }
            else connect_button.Content = "Connect";

        }

        //----< Invoke Help button >---------------------
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Can I help you? Well, not yet.");
        }

        //----< Invoke Connect button >---------------------
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (commonInfo.isConnect == false)
            {
                if (remoteAddr != null && remotePort != null)
                {
                    MessageMaker mm = new MessageMaker(); //connection start message
                    commonInfo.raddr = remoteAddr.Text;
                    commonInfo.rport = remotePort.Text;
                    commonInfo.remoteUrl = Utilities.makeUrl(remoteAddr.Text, remotePort.Text);
                    Message msg = mm.makeMessage(commonInfo.localUrl, commonInfo.remoteUrl, "connection start message");
                    commonInfo.updateReq("Connecting to Server " + commonInfo.raddr + ":" + commonInfo.rport);
                    if (commonInfo.connect(msg, status_1))
                    {
                        connect_button.Content = "Connected";
                        connect_button.IsEnabled = false;
                        remoteAddr.IsEnabled = false;
                        remotePort.IsEnabled = false;
                    }
                }
                else
                {
                    status_1.Content = "Please enter server address and port number before connecting";
                }
            }
            else
            {
                commonInfo.isConnect = false;
                commonInfo.updateReq("Disconnecting from Server " + commonInfo.raddr + ":" + commonInfo.rport);
                commonInfo.updateResp("Disconnected from Server " + commonInfo.raddr + ":" + commonInfo.rport);                
                status_1.Content = "Disconnected";
                connect_button.Content = "Disconnect";
            }
        }

        //----< Redirect to Page1 >---------------------
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page1());
        }

        //----< Redirect to Page2 >---------------------
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page2());
        }

        //----< Redirect to Page3 >---------------------
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page3());
        }

        //----< Redirect to Page4 >---------------------
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page4());
        }

        //----< Redirect to Page5 >---------------------
        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page5());
        }
    }
}
