/*//////////////////////////////////////////////////////////////
// Utilities.cs - Define different Utilities for Project4     //
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
 * This package defines different Utilities the operations that are need to 
 *  Project4
 *
 *  Public Interface
 *===================
 *
 *void title(this string aString, char underline = '-')		helper to display title
 *
 *string processCommandLineForLocal(string[] args, string localUrl)	helper makes it easy to grab endpoints
 *
 *string processCommandLineForRemote(string[] args, string remoteUrl)	helper makes it easy to grab endpoints
 *
 *string processCommandLineForFile(string[] args, string filename)	helper makes it easy to grab filename
 *
 *bool processCommandLinebool(string[] args, bool flag)			helper makes it easy to grab bool from commandline
 *
 *bool processCommandLineCount(string[] args,out int count)		helper makes it easy to grab count from command line
 *
 *bool processCommandgetReaders(string[] args, out int count)		helper makes it easy to grab no of readers
 *
 *bool processCommandgetWriters(string[] args, out int count)		helper makes it easy to grab no of write
 *
 *string makeUrl(string address, string port)				helper functions to construct url strings
 *
 *string urlPort(string url)						helper functions to construct url port
 *
 *string urlAddress(string url)						helper functions to construct get url address
 *
 *void swapUrls(ref Message msg)						helper functions to swap  urls
 *
 *void waitForUser()							helper functions to wait for user to end procesing
 *
 *showMessage(Message msg)						helper functions to show message
 *
 */
/*
 * Maintenance History:
 * --------------------
 * ver 1.1 : 24 Oct 2015
 * - added url parsing functions and other helpers
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
using System.Threading;

namespace Project4Starter
{
    ///////////////////////////////////////////////////////////////////////
    // Utilities class has all the utiliteis needed by Project4
    //

  static public class Utilities
  {
        public static bool verbose { get; set; } = false;

        //----< helper to display title >---------------------
        public static void title(this string aString, char underline = '-')
    {
      Console.Write("\n  {0}", aString);
      Console.Write("\n {0}", new string(underline, aString.Length + 2));
    }

        //----< helper makes it easy to grab endpoints >---------------------
        static public string processCommandLineForLocal(string[] args, string localUrl)
    {
      for (int i = 0; i < args.Length; ++i)
      {
        if ((args.Length > i + 1) && (args[i] == "/l" || args[i] == "/L"))
        {
          localUrl = args[i + 1];
        }
      }
      return localUrl;
    }

        //----< helper makes it easy to grab endpoints >---------------------
        static public string processCommandLineForRemote(string[] args, string remoteUrl)
    {
      for (int i = 0; i < args.Length; ++i)
      {
        if ((args.Length > i + 1) && (args[i] == "/r" || args[i] == "/R"))
        {
          remoteUrl = args[i + 1];
        }
      }
      return remoteUrl;
    }

        //----< helper makes it easy to grab filename >---------------------
        static public string processCommandLineForFile(string[] args, string filename)
        {
            for (int i = 0; i < args.Length; ++i)
            {
                if ((args.Length > i + 1) && (args[i] == "/f" || args[i] == "/F"))
                {
                    filename = args[i + 1];
                }
            }
            return filename;
        }

        //----< helper makes it easy to grab bool from commandline >---------------------
        static public bool processCommandLinebool(string[] args, bool flag)
        {
            string boolean="false";
            for (int i = 0; i < args.Length; ++i)
            {
                if ((args.Length > i + 1) && (args[i] == "/p" || args[i] == "/P"))
                {
                    boolean = args[i + 1];
                }
            }
            if (boolean == "true") flag= true;
            else flag = false;
            return flag;
        }

        //----< helper makes it easy to grab count from command line >---------------------
        static public bool processCommandLineCount(string[] args,out int count)
        {
            count = 100;//default value
            for (int i = 0; i < args.Length; ++i)
            {
                if ((args.Length > i + 1) && (args[i] == "/c" || args[i] == "/C"))
                {
                   return int.TryParse(args[i + 1],out count);
                }
            }
            return false;
        }

        //----< helper makes it easy to grab no of readers >---------------------
        static public bool processCommandgetReaders(string[] args, out int count)
        {
            count = 1;//default value
            for (int i = 0; i < args.Length; ++i)
            {
                if ((args.Length > i + 1) && (args[i] == "/rc" || args[i] == "/RC"))
                {
                    return int.TryParse(args[i + 1], out count);
                }
            }
            return false;
        }

        //----< helper makes it easy to grab no of write >---------------------
        static public bool processCommandgetWriters(string[] args, out int count)
        {
            count = 1;//default value
            for (int i = 0; i < args.Length; ++i)
            {
                if ((args.Length > i + 1) && (args[i] == "/wc" || args[i] == "/WC"))
                {
                    return int.TryParse(args[i + 1], out count);
                }
            }
            return false;
        }

        //----< helper functions to construct url strings >------------------
        public static string makeUrl(string address, string port)
    {
      return "http://" + address + ":" + port + "/CommService";
    }

        //----< helper functions to construct url port >------------------
        public static string urlPort(string url)
    {
      int posColon = url.LastIndexOf(':');
      int posSlash = url.LastIndexOf('/');
      string port = url.Substring(posColon + 1, posSlash - posColon - 1);
      return port;
    }

        //----< helper functions to construct get url address >------------------
        public static string urlAddress(string url)
    {
      int posFirstColon = url.IndexOf(':');
      int posLastColon = url.LastIndexOf(':');
      string port = url.Substring(posFirstColon + 3, posLastColon - posFirstColon - 3);
      return port;
    }

        //----< helper functions to swap  urls >------------------
        public static void swapUrls(ref Message msg)
    {
      string temp = msg.fromUrl;
      msg.fromUrl = msg.toUrl;
      msg.toUrl = temp;
    }

        //----< helper functions to wait for user to end procesing >------------------
        public static void waitForUser()
    {
      Thread.Sleep(200);
      Console.Write("\n  press any key to quit: ");
      Console.ReadKey();
    }

        //----< helper functions to show message >------------------
        public static void showMessage(Message msg)
    {
      Console.Write("\n  msg.fromUrl: {0}", msg.fromUrl);
      Console.Write("\n  msg.toUrl:   {0}", msg.toUrl);
      Console.Write("\n  msg.content: {0}", msg.content);
    }

        //----< Test stub >------------------
#if (TEST_UTIL)
     static void Main(string[] args)
     {
      "testing utilities".title('=');
      Console.WriteLine();

      "testing makeUrl".title();
      string localUrl = Utilities.makeUrl("localhost", "7070");
      string remoteUrl = Utilities.makeUrl("localhost", "7071");
      Console.Write("\n  localUrl  = {0}", localUrl);
      Console.Write("\n  remoteUrl = {0}", remoteUrl);
      Console.WriteLine();

      "testing url parsing".title();
      string port = urlPort(localUrl);
      string addr = urlAddress(localUrl);
      Console.Write("\n  local port = {0}", port);
      Console.Write("\n  local addr = {0}", addr);
      Console.WriteLine();

      "testing processCommandLine".title();
      localUrl = Utilities.processCommandLineForLocal(args, localUrl);
      remoteUrl = Utilities.processCommandLineForRemote(args, remoteUrl);
      Console.Write("\n  localUrl  = {0}", localUrl);
      Console.Write("\n  remoteUrl = {0}", remoteUrl);
      Console.WriteLine();

      "testing swapUrls(ref Message msg)".title();
      Message msg = new Message();
      msg.toUrl = "http://localhost:8080/CommService";
      msg.fromUrl = "http://localhost:8081/CommService";
      msg.content = "swapee";
      Utilities.showMessage(msg);
      Console.WriteLine();

      Utilities.swapUrls(ref msg);
      Utilities.showMessage(msg);
      Console.Write("\n\n");
    }
#endif
  }
}
