/*/////////////////////////////////////////////////////////////
// Window1.xaml.cs - UI Used as WPF READ and WRITE client    //
//                 for Project4                              //
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
 * This package has UI Used as WPF READ and WRITE client for WPF Project4
 *
 *  Public Interface
 *===================
 *
 *void initReq(ListBox list)		function to initalize the list while switiching between tabs
 *
 *void initResp(ListBox list)		function to initalize the list while switiching between tabs
 *
 *void regReq(ListBox list)		function to register the list for populating reqs and responces
 *
 *void respReq(ListBox list)		function to register the list for populating reqs and responces
 *
 *void updateReq(string request)		Update the list each time you switch between tabs
 *
 *void updateResp(string respon)		Update the list each time you switch between tabs
 *
 *void setup(string lport, string laddr)	start listen port for the WPF Client
 *
 *bool connect(Message msg, Label status)	Connect to remote server
 *
 *void sendMessage(Message msg)		send requested messages to server
 *
 *void querySend(string type, string text)send requested query messages to server
 *
 *int getQtype(string type)		get query type based on the combobox
 *
 *void saveRestore(string type, string text) send requested save/restore messages to server
 *
 *int saveRestoreType(string type)	get save/restore type
 *
 *void sendPreformance()			send requested performance messages to server
 *
 *void updatePred(string thru)		update performance boxes
 *
 *void updateedit(string name, string description, string children, string paylaod)	update the editable fields
 *
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
    // Window1 class
    // - acts as WPF READ and WRITE client for Project 4
    //
    public partial class Window1 : NavigationWindow
    {        
        public Window1()
        {
            InitializeComponent();            
            this.ShowsNavigationUI = false;
            commonInfo.setDisp(this.Dispatcher);
                //.dispatcher_ = this.Dispatcher;
            commonInfo.setup(commonInfo.lport, commonInfo.laddr);
        }
    }

    //----< Specialized Sender for WPF >---------------------
    public class wpfSender : Sender
    {
        //TextBox lStat_ = null;  // reference to UIs local status textbox
        System.Windows.Threading.Dispatcher dispatcher_ = null;

        //----< Constructor for WPF that take dispatcher >---------------------
        public wpfSender( System.Windows.Threading.Dispatcher dispatcher)
        {
            dispatcher_ = dispatcher;  // use to send results action to main UI thread  
            this.MaxConnectAttempts = 3;
        }

        //----< Overriding virtual functions of Sender to display responce messages in UI >---------------------
        public override void sendMsgNotify(string msg)
        {
            Action act = () => { commonInfo.updateResp(msg); };
            dispatcher_.Invoke(act);

        }

        //----< Overriding virtual functions of Sender to display responce messages in UI >---------------------
        public override void sendExceptionNotify(Exception ex, string msg = "")
        {
            Action act = () => { commonInfo.updateResp(ex.ToString()); };
            dispatcher_.Invoke(act);
        }

        //----< Overriding virtual functions of Sender to display responce messages in UI >---------------------
        public override void sendAttemptNotify(int attemptNumber)
        {
            Action act = null;
            act = () => { commonInfo.updateResp(String.Format("attempt to send #{0}", attemptNumber)); };
            dispatcher_.Invoke(act);
        }
    }

    //----< class CommonInfo for all the Pages of UI>---------------------
    public class commonInfo
    {
        //Common properties for different Pages to access
        public static bool isConnect { set; get; } = false;
        public static string lport { set; get; } = "8070";
        public static string laddr { set; get; } = "localhost";
        public static string rport { set; get; }
        public static string raddr { set; get; }
        public static string remoteUrl { set; get; }
        public static string localUrl { set; get; }
        public static List<ListBox> requestList { set; get; }
        public static List<ListBox> respList { set; get; }
        public static List<string> reqmsgs { set; get; }
        public static List<string> respmsgs { set; get; }
        public static string redirection { set; get; }= "../../../../TestFolder/";
        public static string prefofile { get; set; } = "preformance.txt";
        private static wpfSender sndr;
        private static Receiver rcvr;
        public static TextBlock latency { get; set; }
        public static TextBlock Thru { get; set; }
        public static TextBox name { get; set; }
        public static TextBox description { get; set; }
        public static TextBox children { get; set; }
        public static TextBox payload { get; set; }        
        static System.Windows.Threading.Dispatcher dispatcher_ = null;

        public static void setDisp(System.Windows.Threading.Dispatcher dispatcher1)
        {
            dispatcher_ = dispatcher1;
        }
        //----< Constructor for commonInfo class that takes dispatcher as input parameter >---------------------
        public commonInfo(System.Windows.Threading.Dispatcher dispatcher)
        {
            commonInfo.dispatcher_ = dispatcher;
        }

        //----< function to initalize the list while switiching between tabs >---------------------
        public static void initReq(ListBox list)
        {
            foreach(var req in reqmsgs)
            {
                foreach (var reql in requestList)
                {
                    TextBlock item = new TextBlock();
                    item.Text = req;
                    reql.Items.Insert(0, item);
                }
            }
        }

        //----< function to initalize the list while switiching between tabs >---------------------
        public static void initResp(ListBox list)
        {
            foreach (var resp in respmsgs)
            {
                foreach (var respl in respList)
                {
                    TextBlock item = new TextBlock();
                    item.Text = resp;
                    respl.Items.Insert(0, item);
                }
            }
        }

        //----< function to register the list for populating reqs and responces >---------------------
        public static void regReq(ListBox list){
            if(requestList!=null){
                requestList.Add(list);
            }
            else{
                requestList = new List<ListBox>();
                requestList.Add(list);
            }
        }

        //----< function to register the list for populating reqs and responces >---------------------
        public static void respReq(ListBox list){
            if (respList != null){
                respList.Add(list);
            }
            else{
                respList = new List<ListBox>();
                respList.Add(list);
            }
        }

        //----< Update the list each time you switch between tabs >---------------------
        public static void updateReq(string request)
        {
            if (reqmsgs == null)
                reqmsgs = new List<string>();
            reqmsgs.Add(request);
            foreach (var req in requestList)
            {
                TextBlock item = new TextBlock();
                item.Text = request;               
                req.Items.Insert(0, item);
            }
        }

        //----< Update the list each time you switch between tabs >---------------------
        public static void updateResp(string respon)
        {
            if (respmsgs == null)
                respmsgs = new List<string>();
            respmsgs.Add(respon);
            foreach (var req in respList)
            {
                TextBlock item = new TextBlock();
                item.Text = respon;               
                req.Items.Insert(0, item);
            }
        }

        //----< start listen port for the WPF Client >---------------------
        public static void setup(string lport, string laddr)
        {
            if (rcvr != null)
                rcvr.shutDown();
            setupChannel(lport, laddr);

        }

        //----< Connect to remote server >---------------------
        public static bool connect(Message msg, Label status)
        {
            if (!isConnect)
            {
                try
                {
                    sndr.localUrl = msg.fromUrl;
                    sndr.remoteUrl = msg.fromUrl;
                    if (!sndr.Connect(msg.toUrl)) { sndr.sendMsgNotify("Failed to connect to "+ msg.toUrl); isConnect = false;return false; }
                    remoteUrl = msg.toUrl;
                    isConnect = true;
                    Action act = () => { status.Content = "Connected to server"; };//update resp msgs
                    commonInfo.dispatcher_.Invoke(act);
                    return true;
                }
                catch (Exception e)
                {
                    Action act = () => { status.Content = "Cannot connect to Server, please check server address"; };//update resp msgs
                    commonInfo.dispatcher_.Invoke(act);string exception = e.ToString();
                    return false;
                }
            }
            else
            {
                Action act = () => { status.Content = "Disconnected"; };//update resp msgs
                commonInfo.dispatcher_.Invoke(act);
                return false;
            }
        }

        //----< Starting receiver channel for responce messages >---------------------
        static void setupChannel(string lport, string laddr)
        {
            rcvr = new Receiver(lport, laddr);
            Action serviceAction = () => {
                try {
                    Message rmsg = null;string type,name,description,children,paylaod, thru;
                    while (true) {
                        rmsg = rcvr.getMessage();                        
                        if (rmsg.content == "connection start message") { continue; }  if (rmsg.content == "done") { Console.Write("\n  client has finished\n"); continue; }//        if (rmsg.content == "closeServer") { Console.Write("received closeServer"); break; }
                        Action act = () => { MessageMaker mm = new MessageMaker();
                            string rply;// rply = rmsg.content; commonInfo.updateResp(rply);
                            rply =mm.getMessageReply(rmsg, out type,out thru);
                            if (type != "KEY_S_E") commonInfo.updateResp(rply);
                            else {mm.getEditSearch(rmsg, out name, out description, out children, out paylaod);
                                 commonInfo.updateedit(name, description, children, paylaod); }
                            if (type == "P_TIME")
                                commonInfo.updatePred(thru);
                        };//update resp msgs
                        commonInfo.dispatcher_.Invoke(act);
                    }
                }
                catch (Exception ex)   {
                    Action act = () => {
                        string rply = "Failed to Get Message";
                        commonInfo.updateResp(rply);
                    };
                    commonInfo.dispatcher_.Invoke(act);
                    string exception = ex.ToString();
                }
            };
            if (rcvr.StartService())   {  rcvr.doService(serviceAction);  }
            localUrl = Utilities.makeUrl(laddr, lport); sndr = new wpfSender(commonInfo.dispatcher_);
        }

        //----< send requested messages to server >---------------------
        public static void sendMessage(Message msg)
        {
            try
            {
                sndr.sendMessage(msg);
                isConnect = true;
            }
            catch (Exception e)
            {
                if (isConnect)
                    Console.WriteLine("To title: connection lost");
                else
                    Console.WriteLine("To title: Failed to connect to remote server try again");
                string exception = e.ToString();
            }            
        }

        //----< send requested query messages to server >---------------------   
        public static void querySend(string type, string text)
        {
            int choice = getQtype(type);string type_send= "KEY_S";
            switch(choice)
            {
                case 0:                    type_send = "KEY_S";                     break;
                case 1:                    type_send = "CHILD_S";                   break;
                case 2:                    type_send = "PATTERN_S";                 break;
                case 3:                    type_send = "METADATA_S";                break;
                case 4:                    type_send = "TIME_S";                    break;
            }
            MessageMaker mm = new MessageMaker();
            Message msg = new Message();msg.content = mm.constructQueryContext(type_send, text);msg.toUrl = commonInfo.remoteUrl;msg.fromUrl = commonInfo.localUrl;
            commonInfo.sendMessage(msg);
        }

        //----< get query type based on the combobox >---------------------   
        public static int getQtype(string type)//0-get value,1-get children,2-pattern,3-metadata,4-time
        {
            if (type == "Search for value object using Key")
                return 0;
            else if (type == "Search for children of particular Key")
                return 1;
            else if (type == "Search for keys that start with some particular text")
                return 2;
            else if (type == "Search for value object using search text in name or description")
                return 3;
            else if (type == "Search for keys that fall in specified time interval")
                return 4;
            else
                return 0;

        }

        //----< send requested save/restore messages to server >---------------------  
        public static void saveRestore(string type, string text)
        {
            int choice = saveRestoreType(type); string type_send = "SAVE";
            switch (choice)
            {
                case 0: type_send = "SAVE"; break;
                case 1: type_send = "PERSIST"; break;
            }
            MessageMaker mm = new MessageMaker();
            Message msg = new Message(); msg.content = mm.constructSaveRestoreContext(type_send, text); msg.toUrl = commonInfo.remoteUrl; msg.fromUrl = commonInfo.localUrl;
            commonInfo.sendMessage(msg);
        }

        //----< get save/restore type >---------------------  
        public static int saveRestoreType(string type)//0-get value,1-get children,2-pattern,3-metadata,4-time
        {
            if (type == "Persist to XML file")
                return 0;
            else if (type == "Augment from XML file")
                return 1;           
            else
                return 0;

        }

        //----< send requested performance messages to server >--------------------- 
        public static void sendPreformance()
        {            
            MessageMaker mm = new MessageMaker();
            Message msg = new Message(); msg = mm.constructprefrequest(commonInfo.remoteUrl, commonInfo.localUrl); msg.toUrl = commonInfo.remoteUrl; msg.fromUrl = commonInfo.localUrl;
            commonInfo.sendMessage(msg);
        }

        //----< update performance boxes >--------------------- 
        private static void updatePred(string thru)
        {
            Action act = () => {
                MessageMaker ms = new MessageMaker();
                string lat = ms.getLat(redirection + prefofile);
                latency.Text = lat;
                commonInfo.Thru.Text = thru; };
            commonInfo.dispatcher_.Invoke(act);
        }

        //----< update the editable fields >--------------------- 
        private static void updateedit(string name, string description, string children, string paylaod)
        {
            Action act = () => {
                commonInfo.name.Text = name;
                commonInfo.description.Text = description;
                commonInfo.children.Text = children;
                commonInfo.payload.Text = paylaod;
            };
            commonInfo.dispatcher_.Invoke(act);
        }

    }

}
