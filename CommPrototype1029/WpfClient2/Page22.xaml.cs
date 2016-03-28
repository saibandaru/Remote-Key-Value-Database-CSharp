/*/////////////////////////////////////////////////////////////
// Page22.xaml.cs - UI Used as WPF READ and WRITE client's   //
//                 Edit Tabfor Project4                      //
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
 * This package has UI Used as WPF READ and WRITE client's Edit Tab for WPF Project4
 *
 *  Public Interface
 *===================
 *
 *  Page22()     Constructor that initalizes Page22
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
    // Page22 class
    // - provides the performance UI for read and writer client's Edit Tab
    //
    public partial class Page22 : Page
    {
        public Page22()
        {
            InitializeComponent();
            status_4.Content = "Enter Key and then press Process";
            commonInfo.regReq(reqList4);
            commonInfo.respReq(respList4);
            commonInfo.name = textBlock_name;
            commonInfo.description = textBlock_desc;
            commonInfo.children = textBlock_child;
            commonInfo.payload= textBlock_payload;
            if (commonInfo.reqmsgs != null && commonInfo.respmsgs != null)
            {
                commonInfo.initReq(reqList4);
                commonInfo.initResp(respList4);
            }            
        }

        //----< helper function for help button>---------------------
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Can I help you? Well, not yet.");
        }

        //----< helper function for edit button>---------------------
        private void edit_clicked(object sender, RoutedEventArgs e)
        {
            if (commonInfo.isConnect == true)
            {
                if (textBlock_key!=null&&textBlock_name != null&& textBlock_desc != null && textBlock_child != null && textBlock_payload != null )
                {
                    MessageMaker mm = new MessageMaker();
                    Message msg = new Message();
                    msg.toUrl = commonInfo.remoteUrl;
                    msg.fromUrl = commonInfo.localUrl;
                    List<string> children = mm.makeList(textBlock_child.Text);
                    List<string> payload = mm.makeList(textBlock_payload.Text);
                    msg.content = mm.constructInsertasEdit(textBlock_key.Text,textBlock_name.Text, textBlock_desc.Text, children, payload);
                    commonInfo.sendMessage(msg);
                    status_4.Content = "Editing value of Key!";
                    commonInfo.updateReq("Editing value of Key!");
                }
                else
                    status_4.Content = "Enter Key field consistently";
            }
            else
            {
                status_4.Content = "Make connection with server and then perform operation";
            }
        }

        //----< helper function for edit search button>---------------------
        private void search_clicked_e(object sender, RoutedEventArgs e)
        {
            if (commonInfo.isConnect == true)
            {
                if (textBlock_key != null)
                {
                    MessageMaker mm = new MessageMaker();
                    Message msg = new Message();
                    msg.toUrl = commonInfo.remoteUrl;
                    msg.fromUrl = commonInfo.localUrl;
                    msg.content = mm.constructQueryContext(string.Format("KEY_S_E"), textBlock_key.Text);//KEY_S
                    commonInfo.sendMessage(msg);
                    //commonInfo.editUpdate(textBlock_name, textBlock_desc, textBlock_child, textBlock_payload);
                    status_4.Content = "Searching for the Key!";                    
                }
                else
                    status_4.Content = "Enter Key field consistently";
            }
            else
            {
                status_4.Content = "Make connection with server and then perform operation";
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

        //----< Redirect to Page2 >---------------------
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
