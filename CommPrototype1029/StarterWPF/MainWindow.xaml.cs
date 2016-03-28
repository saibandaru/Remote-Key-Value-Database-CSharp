/*/////////////////////////////////////////////////////////////
// MainWindow.xaml.cs - UI Used to display only the          //
//                 performance results in Project 4          //
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
 * This package has UI Used to display the performance results of different read/write clients 
 *
 *  Public Interface
 *===================
 *
 *void processCommandLine(string[] args)		parses commandline parameters for UI
 *
 *void postthruput(string pref)			indirectly used by child receive thread to post results
 *
 *void postlat()					used by main thread
 *
 */
/*
 * Maintenance History:
 * --------------------
 * ver 2.0 : 29 Oct 2015
 * - changed Xaml to achieve more fluid design
 *   by embedding controls in grid columns as well as rows
 * - added derived sender, overridding notification methods
 *   to put notifications in status textbox
 * - added use of MessageMaker in send_click
 * ver 1.0 : 25 Oct 2015
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
using System.Threading;
using Project4Starter;

namespace WpfApplication1
{
    ///////////////////////////////////////////////////////////////////////
    // MainWindow class
    // - provides the performance UI for read and writer clients
    //
  public partial class MainWindow : Window
  {
    static bool firstConnect = true;
    static Receiver rcvr = null;
    static wpfSender sndr = null;
    string localAddress = "localhost";
    string localPort = "8050";
    string remoteAddress = "localhost";
    string remotePort = "8080";
    string prefofile { get; set; } = "preformance.txt";
    string redirection { get; set; } = "../../../../TestFolder/";
    int reader, writer;

        /////////////////////////////////////////////////////////////////////
        // nested class wpfSender used to override Sender message handling
        // - routes messages to status textbox
        public class wpfSender : Sender
        {     
            System.Windows.Threading.Dispatcher dispatcher_ = null;

            public wpfSender(System.Windows.Threading.Dispatcher dispatcher)
            {
                dispatcher_ = dispatcher;  // use to send results action to main UI thread       
            }
        }

        //----< initializes the components in main window >---------------------
        public MainWindow()
        {
            InitializeComponent();                  
            Title = "Starter Client";
            string[] args = Environment.GetCommandLineArgs();
            processCommandLine(args);
            noofreaders.Text = reader.ToString();
            noofwriters.Text = writer.ToString();
            serveraddr.Text = remoteAddress + ":" + remotePort;            
            start_Click();
        }

        //----< parses commandline parameters for UI >---------------------
        public void processCommandLine(string[] args)
        {
            int reader1, writer1;
            if (args.Length == 0)
                return;
            Utilities.processCommandgetReaders(args, out reader1);
            Utilities.processCommandgetWriters(args, out writer1);            
            reader = reader1;
            writer = writer1;
        }

        //----< indirectly used by child receive thread to post results >----
        public void postthruput(string pref)
        {
            thruput.Text = pref;
        }

        //----< used by main thread >----------------------------------------
        public void postlat()
        {
            MessageMaker ms = new MessageMaker();
            string lat=ms.getLat(redirection + prefofile);
            latency.Text = lat;
        }
        
        //----< get Receiver and Sender running >----------------------------
        void setupChannel()
        {
            rcvr = new Receiver(localPort, localAddress);
            Action serviceAction = () =>  {
                try {
                    Message rmsg = null;              
                     while (true) {
                        rmsg = rcvr.getMessage();
                        if (rmsg.content == "connection start message") continue;
                        MessageMaker mm = new MessageMaker();
                        string content = mm.parseRequestMessageForString(rmsg, "RESULT");// rmsg.content;   //postRcvMsg(rmsg.content);
                        Action act = () => { postthruput(content); };
                        Action act2 = () => { dummy.Text="\""+content+ "\""; };//textBox1
                        Action act3 = () => { this.postlat(); };
                        Dispatcher.Invoke(act);
                        Dispatcher.Invoke(act3); 
                    }
                }
                catch(Exception ex) {
                    Action act = () => { dummy.Text = ex.ToString(); };
                    Dispatcher.Invoke(act);
                }
            };
            if (rcvr.StartService()) { rcvr.doService(serviceAction); }
            sndr = new wpfSender(this.Dispatcher);
        }

        //----< set up channel after entering ports and addresses >----------
        private void start_Click()
        {
            if (firstConnect)
            {
                firstConnect = false;
                if (rcvr != null)
                rcvr.shutDown();
                setupChannel();
            }      
        }

        //----< send a demonstraton message >--------------------------------
        private void send_Click(object sender, RoutedEventArgs e)
        {
            try  {
                MessageMaker maker = new MessageMaker();
                Message msg = maker.constructprefrequest(Utilities.makeUrl(localAddress, localPort), Utilities.makeUrl(remoteAddress, remotePort) );
                sndr.localUrl = msg.fromUrl;
                sndr.remoteUrl = msg.toUrl;               
                sndr.sendMessage(msg);          
            }
            catch(Exception ex)  {
               string exception = ex.ToString();
            }
        }
    }
}
