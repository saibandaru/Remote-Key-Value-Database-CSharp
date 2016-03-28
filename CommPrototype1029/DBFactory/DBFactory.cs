///////////////////////////////////////////////////////////////
// DBFactory.cs - This package helps to craete immutable     //
//                    database which is the result of a query//
// Ver 1.1                                                   //
// Application: Demonstration for CSE687-SMA, Project#2      //
// Language:    C#, ver 6.0, Visual Studio 2015              //
// Platform:    Dell XPS2700, Core-i7, Windows 10            //
// Author:      Sai Krishna, Syracuse University             //
//              (832) 940-8083, sbandaru@syr.edu             //
///////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package helps to craete immutable database which is the result of a query
 *
 *  Public Interface
 *===================
 *
 *(1) getValue(Key key, out DBElement<Key, Data> elem)	Populates the value object of the given key
 *
 *(2) Keys()		Returns the set of keys that are populates as the result of a query
 *
 *(3) addKey(Key key)	Adds a key to the list of keys which satisfies the query consition
 *
 */
/*
 * Maintenance:
 * ------------
 * Required Files: DBFactory.cs
 *                  DBElement.cs, DBEngine.cs and UtilityExtensions.cs
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

namespace Project4Starter
{
    public class DBFactory<Key,Data>
    {
        private DBEngine<Key, DBElement<Key, Data>> db;
        private List<Key> result_keys=new List<Key>();

        //----< DBFactory constructor inatilizes DBEngine instance >------------
        public DBFactory(DBEngine<Key, DBElement<Key, Data>> db)
        {
            this.db = db;
        }

        //----< Populates the value object of the given key >------------
        public bool getValue(Key key, out DBElement<Key, Data> elem)
        {
           return db.getValue(key, out elem);
        }

        //----< Returns the set of keys that are populates as the result of a query >------------
        public List<Key> Keys()
        {
            return result_keys;
        }

        //----< Adds a key to the list of keys which satisfies the query consition >------------
        public void addKey(Key key)
        {
            result_keys.Add(key);
        }
    }
#if (TEST_DBFACTORY)
    class TestDemoPackage
  {
        //----< This acts as a test stub for DBFactory class >------------
        static void Main(string[] args)
        {
            "This DBFactory cannot exist independently as it is instantated by Query engine to construct immutable database".writeToConsole();
            "So the test stub for this is in QuerkPackage package".writeToConsole();
        }
  }
#endif
}
