/*/////////////////////////////////////////////////////////////
// Page2.xaml.cs - UI Used as WPF READ and WRITE client's    //
//                 Insert Tabfor Project4                   //
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
 * This package has UI Used as WPF READ and WRITE client's Insert Tab for WPF Project4
 *
 *  Public Interface
 *===================
 *
 *  Page2()     Constructor that initalizes Page2
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
    // Page2 class
    // - provides the performance UI for read and writer client's Insert Tab
    //
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
            status_2.Content = "Enter values and then press Insert";
            commonInfo.regReq(reqList2);
            commonInfo.respReq(respList2);
            if (commonInfo.reqmsgs != null && commonInfo.respmsgs != null)
            {
                commonInfo.initReq(reqList2);
                commonInfo.initResp(respList2);
            }
        }

        //----< helper function for help button>---------------------
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Can I help you? Well, not yet.");
        }

        //----< helper function for insert button>---------------------
        private void insert_clicked(object sender, RoutedEventArgs e)
        {
            if(commonInfo.isConnect==true)
            {
                if (textBlock_name != null && textBlock_desc != null && textBlock_child != null && textBlock_payload != null) {
                     MessageMaker mm = new MessageMaker();
                    List<string> children = mm.makeList(textBlock_child.Text);
                    List<string> payload = mm.makeList(textBlock_payload.Text);
                    Message msg = new Message();
                    msg.toUrl = commonInfo.remoteUrl;
                    msg.fromUrl = commonInfo.localUrl;
                    msg.content =mm.constructInsertContext(textBlock_name.Text, textBlock_desc.Text, children, payload);
                    commonInfo.sendMessage(msg);
                    status_2.Content = "Insert Requested!";
                    commonInfo.updateReq("Insert requested");
                }
                else
                    status_2.Content = "Enter all the fields consistently";
            }
            else
            {
                status_2.Content = "Make connection with server and then perform operation";
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

        //----< Redirect to Page4 >---------------------
        private void MenuItem_Click_11(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page2());
        }

        //----< Redirect to Page21 >---------------------
        private void MenuItem_Click_12(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page21());
        }

        //----< Redirect to Page22 >---------------------
        private void MenuItem_Click_13(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page22());
        }

        //----< Redirect to Page5 >---------------------
        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page5());
        }
    }
}
