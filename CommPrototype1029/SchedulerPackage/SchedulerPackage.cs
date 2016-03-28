/////////////////////////////////////////////////////////////////////
// SchedulerPackage.cs - This pacakage that times save call in the //
//                  database repeteadly after some interval of time//
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
 *  This pacakage that times save call in the
 *      database repeteadly after some interval of time
 *
 *  Public Interface
 *===================
 *
 *(1) schedular { get; set; }                -->This property is used to implement timer that does save call repeteadky
 *
 */
/*
 * Maintenance:
 * ------------
 * Required Files: SchedulerPackage.cs
 *  DBElement.cs, DBEngine.cs,DBExtension.cs, PersistEngine and UtilityExtensions.cs
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
using System.Xml;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Project4Starter
{
    public class SchedulerPackage<Key,Data, Value,T> where Data : IEnumerable<T>
    {
        //public int time { get; set; }
        //public int number_writes { get; set; }
        public Timer schedular { get; set; } = new Timer();
        private int numberOfWrites = 0;
        private DBEngine<Key, Value> db;
        public SchedulerPackage(DBEngine<Key, Value> db)
        {
            schedular.Interval = 1000;
            schedular.AutoReset = true;
            this.db = db;
            schedular.Enabled = true;
            //implementation of timer that calls save on database
            schedular.Elapsed += (object source, ElapsedEventArgs e) =>
            { 
                numberOfWrites++;                
                ("Saving the Key/Value Database to file\"" + db.filename + "\"").writeToConsole();
                if (typeof(DBElement<Key, T>).ToString() == typeof(Value).ToString())
                { db.dbStoreToStringType1<Key, T, Value>(); }
                else { db.dbStoreToStringType2<Key, Data, Value, T>(); }
                if(numberOfWrites==3)
                    schedular.Enabled = false;
            };
        }
        
    }
#if (TEST_SCHEDULERPACKAGE)
    class TestDemoPackage
  {
    //<<---    This is just test stub for scheduler package    -->>
    static void Main(string[] args)
    {
            "Testing Scheduler Package".writeToConsole();
            DBEngine<int, DBElement<int, string>> db1 = new DBEngine<int, DBElement<int, string>>("NativeDatabase1.xml");
            SchedulerPackage<int, List<string>, DBElement<int, string>, string> sch1 = new SchedulerPackage<int, List<string>, DBElement<int, string>, string>(db1);
    }
  }
#endif
}
