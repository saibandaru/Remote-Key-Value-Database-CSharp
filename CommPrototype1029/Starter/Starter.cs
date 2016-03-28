/*/////////////////////////////////////////////////////////////
// Starter.cs - Used as the TestExecutive that starts read   //
//         and write clients in Project 4                    //
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
 * This package Used as the TestExecutive that starts Read and Write Clients for Project 4 
 *
 *  Public Interface
 *===================
 *
 *void processCommandLine(string[] args)	process commandline and get number of read and write clients
 *
 */
/*
* Maintenance History:
* --------------------
* ver 1.0 : 17 Nov 2015
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
using System.Threading;

namespace Project4Starter
{
    ///////////////////////////////////////////////////////////////////////////////////
    //Starter acts as TestExec that loads read and write clients given in command line
    //
    class Starter
    {
        int reader { set; get; }
        int writer { set; get; }
        string prefofile { get; set; } = "preformance.txt";

        //----< process commandline and get number of read and write clients >-----
        public void processCommandLine(string[] args)
        {
            int reader,writer;
            if (args.Length == 0)
                return;
            Utilities.processCommandgetReaders(args,out reader);
            Utilities.processCommandgetWriters(args,out writer);
            this.reader = reader;
            this.writer = writer;
        }

        //----< The execution starts here>-----
        static void Main(string[] args)
        {
            Starter st = new Starter();
            st.processCommandLine(args);
            string redirection = "../../../../TestFolder/";
            ProcessStarter ps = new ProcessStarter();
            MessageMaker mm = new MessageMaker();
            mm.saveClear(redirection + st.prefofile);                       
            //bool flag1 = true, flag2 = true;
            for (int i=0;i< st.writer;i++)//writers
            {
                string cmdlinewriter1 = "/R http://localhost:8080/CommService /L http://localhost:809" + i.ToString() + "/CommService /F write.xml /C 100 /P false";// + st.writer.ToString();                
                Thread.Sleep(500);
                ps.startProcess("../../../Client/bin/debug/Client.exe", cmdlinewriter1, false);//flag1 = false;
            }
            for (int i = 0; i < st.reader; i++)//writers
            {
                string cmdlinereader1 = "/R http://localhost:8080/CommService /L http://localhost:900"+i+"/CommService /F read.xml /C 100 /P false";// + st.reader.ToString();
                Thread.Sleep(500);
                ps.startProcess("../../../Client2/bin/debug/Client2.exe", cmdlinereader1, false);// flag2 = false;
            }
            string cmdline = "/RC " + st.reader + " /WC " + st.writer;
            ps.startProcess("../../../StarterWPF/bin/debug/WpfApplication1.exe", cmdline, false);//start UI for Performance
        }
    }
}
