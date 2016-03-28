/*//////////////////////////////////////////////////////////////
// Client2.cs - Define Read Client for Project4              //
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
 * This package defines read client the operations that are need to 
 *  Project4
 *
 *  Public Interface
 *===================
 *
 *(1) void processCommandLine(string[] args)                        retrieve urls from the CommandLine if there are any
 *
 *(2) public int getCountFile(IEnumerable<XElement> commands)	    get no of messages in input XML file
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

    class ReadClient
    {
        string localUrl { get; set; } = "http://localhost:8081/CommService";
        string remoteUrl { get; set; } = "http://localhost:8080/CommService";
        string filename { get; set; } = "read.xml";
        string prefofile { get; set; } = "preformance.txt";
        int count { get; set; } = 100;        
        int countinFile { get; set; } = 0;
        public bool printToConsole { get; set; } = false;
        public ReadClient(string filename = "read.xml", int count = 100)
        {
            this.filename = filename;
            this.count = count;
        }

        //----< retrieve urls from the CommandLine if there are any >--------
        public void processCommandLine(string[] args)
        {
            int getcount;
            if (args.Length == 0)
                return;
            localUrl = Util.processCommandLineForLocal(args, localUrl);
            remoteUrl = Util.processCommandLineForRemote(args, remoteUrl);
            filename = Util.processCommandLineForFile(args, filename);
            printToConsole = Util.processCommandLinebool(args, printToConsole);
            if (Util.processCommandLineCount(args, out getcount))
                this.count = getcount;
        }

        //----< get no of messages in input XML file >--------
        public int getCountFile(IEnumerable<XElement> commands)
        {
            int count = 0;
            foreach (var op in commands) count++;
            return count;
        }

        //----< get no of loop messages in input XML file >--------
        public void parseNumber(XElement elem)
        {
            try { if (elem != null)
                {
                    int value;
                    Int32.TryParse(elem.Value, out value);
                    this.count = value;
                }
            }
            catch (Exception e)
            { Console.WriteLine("No of messages given wrong in XML taking command line or degault messages"); string exception = e.ToString(); }
        }

        //----< Main function the processing starts from here >--------
        static void Main(string[] args)
        {
            string redirection = "../../../../TestFolder/";
            HiResTimer timer = new HiResTimer(); ReadClient clnt = new ReadClient(); clnt.processCommandLine(args);
            string localPort = Util.urlPort(clnt.localUrl);  string localAddr = Util.urlAddress(clnt.localUrl);
            Console.Write("\n  Starting Read Client with local address {0}:{1}", localAddr, localPort);
            Console.Write("\n ==========================================================================\n");
            Receiver rcvr = new Receiver(localPort, localAddr);  Sender sndr = new Sender(clnt.localUrl);  // Sender needs localUrl for start message
            XDocument readXml = new XDocument();  readXml = XDocument.Load(redirection+clnt.filename); 
            XElement root = readXml.Element("OPERATIONS"); IEnumerable<XElement> operations = root.Elements("REQUEST"); XElement number = root.Element("NUMBER");
            clnt.parseNumber(number);
            //try { if (number != null) { int value; Int32.TryParse(number.Value, out value); clnt.count = value; } } catch (Exception e) { Console.WriteLine("No of messages given wrong in XML taking command line or degault messages");string exception = e.ToString(); }
            clnt.countinFile = clnt.getCountFile(operations);
            Action serviceAction = () => {
                ulong count = 0; ulong totalcount = Convert.ToUInt64(clnt.count * clnt.countinFile);
                Message msg = null;
                while (true)   {
                    msg = rcvr.getMessage();                                              
                    if (msg.content == "connection start message") { continue; } if (msg.content == "done") { Console.Write("\n  client has finished\n"); continue; }// if (msg.content == "closeServer") { Console.Write("received closeServer"); break; }
                    count++; if (clnt.printToConsole) { Console.WriteLine("Recieved Responce Message {0}", msg.content); }
                    if (count == (totalcount)) {
                        timer.Stop();
                        Console.WriteLine("Total time:{0}", timer.ElapsedMicroseconds);
                        Console.WriteLine("Latency per msg :{0}", (timer.ElapsedMicroseconds) / totalcount);
                        MessageMaker mm = new MessageMaker();
                        mm.savePref(redirection + clnt.prefofile, ((timer.ElapsedMicroseconds) / totalcount).ToString());
                    }
                }
            };
            if (rcvr.StartService())  {  rcvr.doService(serviceAction);  }
            timer.Start();
            for (int i = 0; i < clnt.count; i++)
            {
                foreach (var operation in operations)
                {
                    Message msg = new Message(); msg.fromUrl = clnt.localUrl; msg.toUrl = clnt.remoteUrl; msg.content = operation.ToString();
                    if (clnt.printToConsole) { Console.WriteLine("Sending Message {0}", msg.content); }
                    sndr.sendMessage(msg);
                }
            }
            Util.waitForUser();  rcvr.shutDown(); sndr.shutdown();
        }
    }
}
