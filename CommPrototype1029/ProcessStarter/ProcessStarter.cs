/////////////////////////////////////////////////////////////////////
// ProcessStarter.cs - Demonstrate running a child program         //
//                                                                 //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2015 //
/////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Project4Starter
{
  public class ProcessStarter
  {
    public bool startProcess(string process,string arguments= "one two three",bool showconsole=false)
    {
      process = Path.GetFullPath(process);
      Console.Write("\n  fileSpec - \"{0}\"", process);
      ProcessStartInfo psi = new ProcessStartInfo
      {
        FileName = process,
        Arguments = arguments,
        // set UseShellExecute to true to see child console, false hides console
        UseShellExecute = showconsole
      };
      try
      {
        Process p = Process.Start(psi);
        return true;
      }
      catch(Exception ex)
      {
        Console.Write("\n  {0}", ex.Message);
        return false;
      }
    }
    static void Main(string[] args)
    {
      Console.Write("\n  current directory is: \"{0}\"", Directory.GetCurrentDirectory());
      ProcessStarter ps = new ProcessStarter();
      ps.startProcess("../../../StartedProcess/bin/debug/StartedProcess.exe");

      Console.Write("\n  press key to exit: ");
      Console.ReadKey();
    }
  }
}
