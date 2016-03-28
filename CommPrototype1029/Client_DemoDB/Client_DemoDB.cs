/*//////////////////////////////////////////////////////////////
// Client_DemoDB.cs - Define Client for Demonstrating Req 2  //
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
 * This package defines the operations that are need to 
 *  demonstrate Requirement 2
 *
 *  Public Interface
 *===================
 *
 *(1) void processCommandLine(string[] args)                        retrieve urls from the CommandLine if there are any
 *
 *(2) void demoRequiements(Sender sndr,Client_DemoDB clnt)		    Function to demonstrate requirement 2
 *
 *(3) void demo_RequirenmentWrite(Sender sndr,Client_DemoDB clnt)	Function to demonstrate requirement 2 write operations 
 *
 *(4) void demo_RequirenmentRead(Sender sndr, Client_DemoDB clnt)	Function to demonstrate requirement 2 read operations
 *
 *(5) void demo_RequirenmentPersist(Sender sndr, Client_DemoDB clnt)	Function to demonstrate requirement 2 persist operations
 *
 */
/* Maintenance History:
* --------------------
* ver 2.1 : 29 Oct 2015
* - fixed bug in processCommandLine(...)
* - added rcvr.shutdown() and sndr.shutDown() 
* ver 2.0 : 20 Oct 2015
* - replaced almost all functionality with a Sender instance
* - added Receiver to retrieve Server echo messages.
* - added verbose mode to support debugging and learning
* - to see more detail about what is going on in Sender and Receiver
*   set Utilities.verbose = true
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
using System.Xml;
using System.Xml.Linq;
using System.Threading;

namespace Project4Starter
{
  using Util = Utilities;

  ///////////////////////////////////////////////////////////////////////
  // Client class sends and receives messages in this version
  // - commandline format: /L http://localhost:8085/CommService 
  //                       /R http://localhost:8080/CommService
  //   Either one or both may be ommitted

  class Client_DemoDB
  {
        string localUrl { get; set; } = "http://localhost:8081/CommService";
        string remoteUrl { get; set; } = "http://localhost:8080/CommService";
        public Client_DemoDB()
        {
            localUrl  = "http://localhost:8081/CommService";
            remoteUrl = "http://localhost:8080/CommService";
        }

        //----< retrieve urls from the CommandLine if there are any >--------
        public void processCommandLine(string[] args)
        {
            if (args.Length == 0)
                return;
            localUrl = Util.processCommandLineForLocal(args, localUrl);
            remoteUrl = Util.processCommandLineForRemote(args, remoteUrl);      
        }

        //----< Function to demonstrate requirement 2  >--------        
        public void demoRequiements(Sender sndr,Client_DemoDB clnt)
        {
            demo_RequirenmentWrite(sndr, clnt);
            demo_RequirenmentRead(sndr, clnt);
            demo_RequirenmentPersist(sndr, clnt);
        }

        //----< Function to demonstrate requirement 2 write operations  >--------        
        public void demo_RequirenmentWrite(Sender sndr,Client_DemoDB clnt)
        {
            Console.WriteLine("\nDemonstrating :Support addition, deletion and edit of Key/Value pair.");
            Console.WriteLine("Demonstrating :Insert");            
            MessageMaker mm = new MessageMaker();
            Message msg = new Message();msg.fromUrl = clnt.localUrl;msg.toUrl = clnt.remoteUrl;
            msg.content= mm.constructInsertContext("Element", "ElementDesp", new List<string> { "C1,C2" }, new List<string> { "Value1", "Value2" });
            Console.WriteLine("Sending Insert Message :\n"+msg.content);
            sndr.sendMessage(msg);
            Thread.Sleep(5000);
            Console.WriteLine("\n Demonstrating :Delete");
            msg.content = mm.constructDeleteContext("1000");
            Console.WriteLine("Sending Delete Message :\n" + msg.content);
            sndr.sendMessage(msg);
            Thread.Sleep(5000);
            Console.WriteLine("\n Demonstrating :Edit");
            msg.content = mm.constructEditContext("1141577281", "NAME", "Edited Element");
            Console.WriteLine("Sending Edit Message :\n" + msg.content);
            sndr.sendMessage(msg);
            Thread.Sleep(5000);
        }

        //----< Function to demonstrate requirement 2 read operations  >--------        
        public void demo_RequirenmentRead(Sender sndr, Client_DemoDB clnt)
        {
            Console.WriteLine("\nDemonstrating :Support Querying of Key/Value Database");
            Console.WriteLine("Demonstrating :Query Value object");
            MessageMaker mm = new MessageMaker();
            Message msg = new Message(); msg.fromUrl = clnt.localUrl; msg.toUrl = clnt.remoteUrl;
            msg.content = mm.constructQueryContext("KEY_S", "1141577281");
            Console.WriteLine("Sending Query Value Message :\n" + msg.content);
            sndr.sendMessage(msg);
            Thread.Sleep(5000);
            Console.WriteLine("\n Demonstrating :Query Children of a Key");
            msg.content = mm.constructQueryContext("CHILD_S", "1141577281");
            Console.WriteLine("Sending Query Children Message :\n" + msg.content);
            sndr.sendMessage(msg);
            Thread.Sleep(5000);
            Console.WriteLine("\n Demonstrating :Query Metadata");
            msg.content = mm.constructQueryContext("METADATA_S", "Element");
            Console.WriteLine("Sending Query Metadata Message :\n" + msg.content);
            sndr.sendMessage(msg);
            Thread.Sleep(5000);
            Console.WriteLine("\n Demonstrating :Query Key Pattern Match");
            msg.content = mm.constructQueryContext("PATTERN_S", "1141");
            Console.WriteLine("Sending Query Key Pattern Match Message :\n" + msg.content);
            sndr.sendMessage(msg);
            Thread.Sleep(5000);
            DateTime startdate = DateTime.Now;
            DateTime enddate = DateTime.Now;
            Console.WriteLine("\n Demonstrating :Query Keys that fall in some time interval");
            msg.content = mm.constructQueryContext( "TIME_S", startdate.ToString(), enddate.ToString());
            Console.WriteLine("Sending Query Key Pattern Match Message :\n" + msg.content);
            sndr.sendMessage(msg);
            Thread.Sleep(5000);
        }

        //----< Function to demonstrate requirement 2 persist operations  >--------        
        public void demo_RequirenmentPersist(Sender sndr, Client_DemoDB clnt)
        {
            Console.WriteLine("\nDemonstrating :Support Persist and Augment of Key/Value Database");
            Console.WriteLine("Demonstrating :Persist");
            MessageMaker mm = new MessageMaker();
            Message msg = new Message(); msg.fromUrl = clnt.localUrl; msg.toUrl = clnt.remoteUrl;
            msg.content = mm.constructSaveRestoreContext("SAVE", "NewDatabase.xml");            
            Console.WriteLine("Sending Persist Message :\n" + msg.content);
            sndr.sendMessage(msg);
            Thread.Sleep(5000);
            Console.WriteLine("\n Demonstrating :Augment");
            msg.content = mm.constructSaveRestoreContext("PERSIST", "NewDatabase.xml");
            Console.WriteLine("Sending Augment Message :\n" + msg.content);
            sndr.sendMessage(msg);
            Thread.Sleep(5000);
        }

        //----< MAin function the execution starts from  >--------        
        static void Main(string[] args)
        {
            Console.Write("\n  starting CommService client");
            Console.Write("\n =============================\n");                        
            Client_DemoDB clnt = new Client_DemoDB(); clnt.processCommandLine(args);            
            string localPort = Util.urlPort(clnt.localUrl);
            string localAddr = Util.urlAddress(clnt.localUrl);
            Console.Write("\n Listening to {0}:{1} ", localAddr, localPort);
            Receiver rcvr = new Receiver(localPort, localAddr);
            Sender sndr = new Sender(clnt.localUrl);  // Sender needs localUrl for start message
            Action serviceAction = () =>  {  Message msg = null;
                while (true)  {                     
                    msg = rcvr.getMessage();                    
                    if (msg.content == "connection start message") { continue; } if (msg.content == "done") { Console.Write("\n  client has finished\n"); continue; } if (msg.content == "closeServer") { Console.Write("received closeServer"); break; }
                    Console.WriteLine("\nResponce Message:\n {0}", msg.content);                                       
                }
            };
            if (rcvr.StartService())  {  rcvr.doService(serviceAction); }
            clnt.demoRequiements(sndr, clnt);
            Util.waitForUser();
        rcvr.shutDown();
        sndr.shutdown();
    }
  }
}
