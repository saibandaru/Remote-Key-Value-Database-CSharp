/////////////////////////////////////////////////////////////////////
// QueryPackage.cs - This pacakage is used to implement queries    //
//                  on Key/Value Database                          //
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
 * This pacakage is used to implement queries
 *          on Key/Value Database
 *
 *  Public Interface
 *===================
 *
 *(1) queryValueWithKey(Key key) 		        Query for Value object of a particular key
 * 
 *(2) queryChildrenWithKey( Key key)		    Query for Children of a particular key
 *
 *(3) querySearchMatchWithKey(Key key_part)	    Query for all keys that match pattern in database keys
 *
 *(4) querySearchMetadata(string searchString)  Query for all keys that match a text in name or description
 *
 *(5) querySearchInTime(DateTime startTime) 	Query for all keys that fall in particular time interval
 *
 *(6) querySearchInTime(DateTime startTime, DateTime endTime)	Query for all keys that fall in particular time interval 
 *
 *(7) defineQuery(Key test)			            Query definition for searching a key in database
 *
 *(8) defineQuery_metadataSearch(string test)	Query definition for searching a metadata section of a key in database
 *
 *(9) defineQuery_inTime(DateTime startTime,DateTime endTime)	Query definition for keys that fall in some time interval of database 
 *
 *(10) processQuery(Func<Key, bool> queryPredicate)	process query using queryPredicate that all query definitions can use
 */
/*
 * Maintenance:
 * ------------
 * Required Files: QueryPackage.cs
 *  DBElement.cs, DBEngine.cs,DBExtension.cs, PersistEngine, Display.cs, DBFactory.cs and UtilityExtensions.cs
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
    public class QueryPackage<Key, Data>
    {
        private DBEngine<Key, DBElement<Key, Data>> db;
        private DBFactory<Key,  Data> factory;                  //creates an immutabale database
        
        //<<---    QueryPackage constructor    -->>
        public QueryPackage(DBEngine<Key, DBElement<Key, Data>> db)
        {
            this.db = db;           
        }

        //<<---    Query for Value object of a particular key    -->>
        public DBElement<Key, Data> queryValueWithKey(Key key)
        {
            DBElement<Key, Data> value;
            db.getValue(key, out value);
            return value;
        }

        //<<---    Query for Children of a particular key    -->>
        public string queryChildrenWithKey( Key key)
        {
            DBElement<Key, Data> value;
            db.getValue(key, out value);
            return value.showChildren();
        }

        //<<---    Query for all keys that match pattern in database keys    -->>
        public List<Key> querySearchMatchWithKey(Key key_part)
        {
            factory = new DBFactory<Key, Data>(db);
            Func<Key, bool> query = this.defineQuery(key_part);
            bool result = this.processQuery(query);
            return factory.Keys();
        }

        //<<---    Query for all keys that match a text in name or description     -->>
        public List<Key> querySearchMetadata(string searchString)
        {
            factory = new DBFactory<Key, Data>(db);
            Func<Key, bool> query = this.defineQuery_metadataSearch(searchString);
            bool result = this.processQuery(query);
            return factory.Keys();
        }

        //<<---    Query for all keys that fall in particular time interval     -->>
        public List<Key> querySearchInTime(DateTime startTime)
        {
            DateTime endTime = DateTime.Now;            
            factory = new DBFactory<Key, Data>(db);
            Func<Key, bool> query = this.defineQuery_inTime(startTime, endTime);
            bool result = this.processQuery(query);
            return factory.Keys();
        }

        //<<---    Query for all keys that fall in particular time interval     -->>
        public List<Key> querySearchInTime(DateTime startTime, DateTime endTime)
        {            
            factory = new DBFactory<Key, Data>(db);
            Func<Key, bool> query = this.defineQuery_inTime(startTime, endTime);
            bool result = this.processQuery(query);
            return factory.Keys();
        }

        //<<---    Query definition for searching a key in database     -->>
        Func<Key, bool> defineQuery(Key test)
        {
            Func<Key, bool> queryPredicate = (Key key) =>
            {
                     if (key.ToString().StartsWith(test.ToString()))  // string test will be captured by lambda
                    return true;
                return false;
            };
            return queryPredicate;
        }

        //<<---    Query definition for searching a metadata section of a key in database     -->>
        Func<Key, bool> defineQuery_metadataSearch(string test)
        {
            Func<Key, bool> queryPredicate = (Key key) =>
            {
                DBElement<Key, Data> elem;
                db.getValue(key, out elem);                
                if (elem.name.ToString().Contains(test.ToString())|| elem.descr.ToString().Contains(test.ToString())) // string test will be captured by lambda
                    return true;
                return false;
            };
            return queryPredicate;
        }

        //<<---    Query definition for keys that fall in some time interval of database     -->>
        Func<Key, bool> defineQuery_inTime(DateTime startTime,DateTime endTime)
        {
            Func<Key, bool> queryPredicate = (Key key) =>
            {
                DBElement<Key, Data> elem;
                db.getValue(key, out elem);
                if (elem.IsBetween(startTime, endTime))
                    return true;
                return false;
            };
            return queryPredicate;
        }

        //----< Process query using queryPredicate that all query definitions can use >-----------------------
        bool processQuery(Func<Key, bool> queryPredicate)
        {
            bool count = false;
            foreach (Key key in db.Keys())
            {
                        if (queryPredicate(key))
                        {
                            factory.addKey(key);
                            count = true;
                        }                            
            }
            return count;
        }
    }
#if (TEST_QUERYPACKAGE)
    class TestDemoPackage
  {
    
    //----< This is just a test stub for Query engine >-----------------------
    static void Main(string[] args)
    {
            DBEngine<string, DBElement<string, List<string>>> db2 = new DBEngine<string, DBElement<string, List<string>>>("EnumerableDatabase.xml");
            db2.showEnumerableDB();
            "".writeToConsole();
            IEnumerable<string> keys = db2.Keys();
            string first = keys.First();
            QueryPackage<string, List<string>> query = new QueryPackage<string, List<string>>(db2);
            string key = "1";
            ("Querying for \"" + key + "\" Key Value Object").writeToConsole();
            DBElement<string, List<string>> value;
            value = query.queryValueWithKey(key);
            ("Result of Query:").writeToConsole();
            if (value != null)
                value.showEnumerableElement();
        }
  }
#endif
}
