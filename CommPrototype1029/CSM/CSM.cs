/*/////////////////////////////////////////////////////////////
// Server.cs - Used as the Server that hosts noSQL database  //
//                   in Project 4                            //
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
 * This package Used as the server that hosts noSQL Database in this Project 4 
 *
 *  Public Interface
 *===================
 *
 *void ProcessCommandLine(string[] args)		quick way to grab ports and addresses from commandline
 *
 *void startDatabase()				used to load database augment/load database from different file
 *
 */
/*
 * Maintenance History:
 * --------------------
 * ver 2.3 : 29 Oct 2015
 * - added handling of special messages: 
 *   "connection start message", "done", "closeServer"
 * ver 2.2 : 25 Oct 2015
 * - minor changes to display
 * ver 2.1 : 24 Oct 2015
 * - added Sender so Server can echo back messages it receives
 * - added verbose mode to support debugging and learning
 * - to see more detail about what is going on in Sender and Receiver
 *   set Utilities.verbose = true
 * ver 2.0 : 20 Oct 2015
 * - Defined Receiver and used that to replace almost all of the
 *   original Server's functionality.
 * ver 1.0 : 18 Oct 2015
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


namespace Project4Starter
{
  using Util = Utilities;
   //////////////////////////////////////////////////////////////////
  //Server that loads noSQL Database and performs all the requests  
  //
  class Server
  {
        string address { get; set; } = "localhost";
        string port { get; set; } = "6000";
        string filename { get; set; } = "Database.xml";
        private DBEngine<string, DBElement<string, List<string>>> noSQL;
        public ulong elapsed { get; set; } = 0;
        public ulong elapsed_perMsg { get; set; } = 0;
        public List<String> registered;

        //----< instantiate noSQL Database and load the database from the file >-----
        public Server()
        {
            noSQL = new DBEngine<string, DBElement<string, List<string>>>(filename,false);
            noSQL.getFromFile<string, List<string>, string>(filename);
            registered = new List<string>();
        }

        //----< quick way to grab ports and addresses from commandline >-----
        public void ProcessCommandLine(string[] args)
        {
            if (args.Length > 0)
            {
                port = args[0];
            }
            if (args.Length > 1)
            {
                address = args[1];
            }
            if (args.Length > 1)
            {
                filename = args[2];
            }
        }

        //----< used to load database augment/load database from different file >-----
        public void startDatabase()
        {
            noSQL.getFromFile<string, List<string>, string>(noSQL.filename);
        }

        public static void print_console(Message msg)
        {
            MessageMaker mm = new MessageMaker();            
            Console.WriteLine("Time :" + DateTime.Now + " Requester:" + mm.parseRequestMessageForString(msg, "REQ") + "Command:" + mm.parseRequestMessageForString(msg, "COMM"));
        }

        //----< Main function from where the execution starts >-----
        static void Main(string[] args)
        {
            Util.verbose = false; Server srvr = new Server();srvr.ProcessCommandLine(args);
            Console.Title = "CSM";Console.Write(String.Format("\n  Starting CommService server listening on port {0}", srvr.port));
            Console.Write("\n ====================================================\n");    
            Sender sndr = new Sender(Util.makeUrl(srvr.address, srvr.port));   Receiver rcvr = new Receiver(srvr.port, srvr.address);      
            Action serviceAction = () => { Message msg = null; ulong count_no = 0,previous=0;          
                while (true) {
                    msg = rcvr.getMessage();
                    
                    //Console.WriteLine("Server content:\n {0}", msg.content); Console.WriteLine("Server from address:\n {0}", msg.fromUrl); Console.WriteLine("Server from address:\n {0}", msg.toUrl);
                    if (msg.content == "connection start message") {continue; }if (msg.content == "done") { Console.Write("\n  client has finished\n"); continue;}if (msg.content == "closeServer"){Console.Write("received closeServer");break;}                      
                    try {
                        ProcessMessage ms = new ProcessMessage(srvr.noSQL);                        
                        HiResTimer timer_perf = new HiResTimer();
                        timer_perf.Start();   timer_perf.Stop(); ms.OperateCSM(msg, srvr.elapsed_perMsg, srvr.registered, sndr); 
                        previous = timer_perf.ElapsedMicroseconds; srvr.elapsed += timer_perf.ElapsedMicroseconds;
                        count_no = count_no + 1; ; srvr.elapsed_perMsg = srvr.elapsed / count_no;
                        //Console.WriteLine("Thruput :{0}", srvr.elapsed_perMsg.ToString());

                    }
                    catch (Exception e) {
                        Console.WriteLine("Exception :{0}", e.ToString());
                    }  //Console.WriteLine("Server two:\n {0}", msg.content); //Console.WriteLine("Server send content:\n {0}", msg.content);
                    string temp = msg.toUrl;
                    msg.toUrl = msg.fromUrl;msg.fromUrl = temp; msg.content = "log";
                    //sndr.sendMessage(msg);
                }
            };
            if (rcvr.StartService()) { rcvr.doService(serviceAction); }
            Util.waitForUser(); 
        }        
    }
}
