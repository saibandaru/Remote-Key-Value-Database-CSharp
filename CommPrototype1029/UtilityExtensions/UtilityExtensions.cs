/////////////////////////////////////////////////////////////////////
// UtilityExtensions.cs - define methods to simplify project code  //
// Ver 1.0                                                         //
// Application: Demonstration for CSE681-SMA, Project#2            //
// Language:    C#, ver 6.0, Visual Studio 2015                    //
// Platform:    ASUS SonicMaster, Core-i3, Windows 10              //
// Author:      Sai Krishna, Syracuse University                   //
//              (813) 940-8083, sbandaru@syr.edu                   //
/////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package implements utility extensions that are not specific
 * to a single package.
 *
 *  Public Interface
 *===================
 *
 *(1) void title(this string aString, char underline = '-')               -->This function is used to write titles into the console
 *
 *(2) void demosntrate_display(this string aString, char underline = '-') -->This function is used to write titles for demonstration requirements into the console
 *
 *(3) void writeToConsole(this string aString)                            -->This function is used to write some content into the console
 */
/*
 * Maintenance:
 * ------------
 * Required Files: UtilityExtensions.cs
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * ver 1.0 : 30 Sep 15
 * - first release
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Project4Starter
{
  public static class UtilityExtensions
  {
        //<<---    To print title with an underline    -->>
        public static void title(this string aString, char underline = '-')
        {
            Console.Write("\n  {0}", aString);
            Console.Write("\n {0}", new string(underline, aString.Length + 2));
        }

        //<<---    To print demonstration title with a default underline -    -->>
        public static void demosntrate_display(this string aString, char underline = '-')
        {
            Console.Write("\n  {0}", aString);
            Console.Write("\n {0}", new string(underline, aString.Length + 2));
        }

        //<<---    To print an ordinary line instead of WriteLine()   -->>
        public static void writeToConsole(this string aString)
        {
            Console.Write("\n  {0}", aString);            
        }

        //<<---    To print save title with an underline    -->>
        public static void save_title(this string aString, char underline = '-')
        {
            Console.Write("\n {0}", new string(underline, aString.Length + 2));
            Console.Write("\n  {0}", aString);
            Console.Write("\n {0}", new string(underline, aString.Length + 2));
        }
    }

  public class TestUtilityExtensions
  {
        //<<---    This is just Test stub    -->>
        static void Main(string[] args)
        {
            "Testing UtilityExtensions.title".title();
            "Testing UtilityExtensions.demosntrate_display".demosntrate_display();
            "Testing UtilityExtensions.writeToConsole".writeToConsole();
            Write("\n\n");
        }
  }
}
