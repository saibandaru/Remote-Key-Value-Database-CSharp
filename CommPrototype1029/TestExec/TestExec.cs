/////////////////////////////////////////////////////////////////////
// TestExec.cs - This pacakage is the logical entry point of the   //
//                  application used to demonstrate requirements   //
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
 * This package is the logical entry point of the
 *                          application used to demonstrate requirements
 *
 *  Public Interface
 *===================
 * No Public interfaces defined here
 */
/*
 * Maintenance:
 * ------------
 * Required Files: TestExec.cs
 *  DBElement.cs, DBEngine.cs,Display.cs, DemonstrationPackage.cs and UtilityExtensions.cs
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

namespace Project2Starter
{
  class TestExec
  {
    //<<---    This is the logical entry point for the Application    -->>
    static void Main(string[] args)
    {     
            "Demonstrating Project#2 Requirements".title('='); Write("\n");
            DemosnstrationPackage demo_obj = new DemosnstrationPackage();
            DBEngine<int, DBElement<int, string>> db1 = new DBEngine<int, DBElement<int, string>>("NativeDatabase1.xml");
            DBEngine < string,DBElement<string, List<string>>> db2 = new DBEngine<string, DBElement<string, List<string>>>("EnumerableDatabase.xml");
            demo_obj.demo_Requirenment2(db1, db2);           
            demo_obj.demo_Requirenment3(db1, db2);            
            demo_obj.demo_Requirenment4(db1, db2);
            DBEngine<int, DBElement<int, string>> db3 = new DBEngine<int, DBElement<int, string>>("NativeDatabase2.xml");
            string aug_filename1 = "db1.xml", aug_filename2 = "db2.xml";
            demo_obj.demo_Requirenment5(db1,db2, aug_filename1, aug_filename2);
            demo_obj.demo_Requirenment7(db1, db2);
            demo_obj.demo_Requirenment8();
            demo_obj.demo_Requirenment9("ProjectPackage.xml");
            demo_obj.demo_Requirenment12("Bonus.xml");
            demo_obj.demo_Requirenment6(db1, db2);            
            Console.ReadKey();            
    }
  }
}
