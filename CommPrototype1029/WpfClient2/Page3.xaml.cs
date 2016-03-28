/*/////////////////////////////////////////////////////////////
// Page3.xaml.cs - UI Used as WPF READ and WRITE client's   //
//                 Query Tab for Project4                   //
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
 * This package has UI Used as WPF READ and WRITE client's Query Tab for WPF Project4
 *
 *  Public Interface
 *===================
 *
 *  Page3()     Constructor that initalizes Page3
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
    // Page3 class
    // - provides the performance UI for read and writer client's Query Tab
    //
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();
            status_5.Content = "Select the category of query, enter search text and hit Search!";
            commonInfo.regReq(reqList5);
            commonInfo.respReq(respList5);
            if (commonInfo.reqmsgs != null && commonInfo.respmsgs != null)
            {
                commonInfo.initReq(reqList5);
                commonInfo.initResp(respList5);
            }
        }

        //----< helper function for help button>---------------------
        private void Button_Click(object sender, RoutedEventArgs e)//Search_Click_q
        {
            MessageBox.Show("Can I help you? Well, not yet.");
        }

        //----< helper function for search button>---------------------
        private void Search_Click_q(object sender, RoutedEventArgs e)
        {
            if (commonInfo.isConnect == true)
            {
                if (textBlock_search != null)
                {                    
                    string name = comboBox.SelectionBoxItem.ToString();                                        
                    commonInfo.querySend(name, textBlock_search.Text);
                    status_5.Content = "Query Requested!";    
                    commonInfo.updateReq("Query requested");
                }
                else
                    status_5.Content = "Enter all the fields consistently";
            }
            else
            {
                status_5.Content = "Make connection with server and then perform operation";
            }
        }

        //----< Redirect to Page1>---------------------
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page1());
        }

        //----< Redirect to Page2>---------------------
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page2());
        }

        //----< Redirect to Page3>---------------------
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page3());
        }

        //----< Redirect to Page4>---------------------
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page4());
        }

        //----< Redirect to Page5>---------------------
        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page5());
        }
    }
}
