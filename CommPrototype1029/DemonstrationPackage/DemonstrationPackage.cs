///////////////////////////////////////////////////////////////
// DemosnstrationPackage.cs - To  package helps to           //
//                      demonstrate requirements of Projact#2//
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
 * To  package helps to demonstrate requirements of Projact#2
 *
 *  Public Interface
 *===================
 *
 *(1)demo_Requirenment2(db1,db2)	To Demonstrate Requirement 2:Generic Database
 *
 *(2)demo_Requirenment3(db1,db2)	To Demonstrate Requirement 3:Adding/Deleting Key/Value instance
 *
 *(3)demo_Requirenment4(db1,db2)	To Demonstrate Requirement 4:Edit Database
 *
 *(4)demo_Requirenment5(db1,db2)	To Demonstrate Requirement 5:Persist and restore or agument database from(or) into XML file
 *
 *(5)demo_Requirenment6(db1,db2)	To Demonstrate Requirement 6:Persist Database through Scheduler
 *
 *(6)demo_Requirenment7(db1,db2)	To Demonstrate Requirement 7:Quering Database
 *
 *(7)demo_Requirenment8()	        To Demonstrate Requirement 8:Build immutable database through quering
 *
 *(8)demo_Requirenment9(filename)	To Demonstrate Requirement 9:Load an XML file with pacakage name with references
 *
 */
/*
 * Maintenance:
 * ------------
 * Required Files: DemosnstrationPackage.cs
 *              DBElement.cs, DBEngine.cs,DBExtension.cs, DBEleEdit.cs, PersistEngine, QueryPackage.cs,
 *              Display.cs and UtilityExtensions.cs
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
using System.Xml;
using System.Xml.Linq;
using static System.Console;
 

namespace Project2Starter
{
    public class DemosnstrationPackage
    {

        //----< To Demonstrate Requirement 2 >------------
        public void demo_Requirenment2(DBEngine<int, DBElement<int, string>> db1, DBEngine<string, DBElement<string, List<string>>> db2)
        {
            "Demonstrating Requirement 2: Implementing generic Key/Value Database".demosntrate_display();
            "Key/Value Database with Native paylaod:\n".writeToConsole();
            DBElement<int, string> elem1 = new DBElement<int, string>("Element_1 Native", "Element_1 Native Description");
            elem1.children = new List<int> { 2, 3, 4 };
            elem1.payload = "Element_1 Native Data";            
            db1.addKeyValuePair<int,string>(elem1,elem1.keygen_int());
            DBElement<int, string> elem2 = new DBElement<int, string>("Element_2 Native", "Element_2 Native Description");
            elem2.children = new List<int> { 3, 4, 5 };
            elem2.payload = "Element_2 Native Data";
            db1.addKeyValuePair<int, string>(elem2,elem2.keygen_int());
            db1.showDB();WriteLine();

            "Key/Value Database with Enumerable paylaod:\n".writeToConsole();
            DBElement<string, List<string>> newelem1 = new DBElement<string, List<string>>();
            newelem1.name = "Element_1 List";
            newelem1.descr = "Element_1 List Description";
            newelem1.payload = new List<string> { "Element_1 Data1", "Element_1 Data2", "Element_1 Data3" };
            newelem1.children = new List<string> { "List Child_1", "List Child_2", "List Child_3" };            
            db2.addKeyValuePair<string, List<string>, string>(newelem1, newelem1.keygen_string());
            DBElement<string, List<string>> newelem2 = new DBElement<string, List<string>>();
            newelem2.name = "Element_2 List";
            newelem2.descr = "Element_2 List Description";
            newelem2.payload = new List<string> { "Element_2 Data1", "Element_2 Data2" };
            newelem2.children = new List<string> { "List Child_1", "List Child_2", "List Child_3" };            
            db2.addKeyValuePair<string, List<string>, string>(newelem2, newelem2.keygen_string());
            db2.showEnumerableDB();WriteLine();
        }

        //----< To Demonstrate Requirement 3 >------------
        public void demo_Requirenment3(DBEngine<int, DBElement<int, string>> db1, DBEngine<string, DBElement<string, List<string>>> db2)
        {
            "Demonstrating Requirement 3:Support addition and deletion of Key/Value pair.".demosntrate_display();                        
            DBElement<int, string> elem1 = new DBElement<int, string>("Element_3 Native", "Element_3 Native Description");
            int catch_key_native = elem1.keygen_int(); ("Adding the below Value with key \""+ catch_key_native + "\" to Native database\n").writeToConsole();
            elem1.children = new List<int> { 4, 5, 6 };
            elem1.payload = "Element_3 Native Data"; elem1.showElement();
            db1.addKeyValuePair<int, string>(elem1,catch_key_native); 
            "\n Native Database after adding the above Value instance".writeToConsole();
            db1.showDB();
            
            DBElement<string, List<string>> newelem3 = new DBElement<string, List<string>>("Element_3 List", "Element_3 List Description");
            "".writeToConsole();
            string catch_key_list = newelem3.keygen_string(); ("Adding the below Value with key \"" + catch_key_list + "\" to Enumerable database\n").writeToConsole();
            newelem3.payload = new List<string> { "Element_3 Data1", "Element_3 Data2" };
            newelem3.children = new List<string> { "List Child_1", "List Child_2", "List Child_3" }; newelem3.showEnumerableElement();
            db2.addKeyValuePair<string, List<string>, string>(newelem3, catch_key_list);
            "\n Enumerable Database after adding the above Value instance".writeToConsole(); db2.showEnumerableDB(); WriteLine();
           
            db1.deleteKeyValuePair<int, string>(catch_key_native);
            ("Database after deleting key \"" + catch_key_native + "\" from Native Database").ToString().writeToConsole();
            db1.showDB();
            "".writeToConsole();
            db2.deleteKeyValuePair<string,List<string>, string>(catch_key_list);
            ("Database after deleting key \"" + catch_key_list + "\" from Enumerable Database").ToString().writeToConsole();
            db2.showEnumerableDB();
            "".writeToConsole();
        }

        //----< To Demonstrate Requirement 4 >------------
        public void demo_Requirenment4(DBEngine<int, DBElement<int, string>> db1, DBEngine<string, DBElement<string, List<string>>> db2)
        {
            "Demonstrating Requirement 4: Addition and/or deletion of relationships, editing text metadata, and replacing an existing value's instance".demosntrate_display();
            this.demo_Requirenment41(db1);
            this.demo_Requirenment42(db2);

        }

        //----< To Demonstrate Requirement 4 part 1 >------------
        private void demo_Requirenment41(DBEngine<int, DBElement<int, string>> db1)
        {
            IEnumerable<int> keys_d = db1.Keys();
            int first_native_key = keys_d.First(); int addAsChild = 33;            
            "Native Key/Value Database :".writeToConsole();
            db1.showDB();
            db1.addOrDel_relation<int, string>(addAsChild, first_native_key, true);
            ("\n Database after adding \"" + addAsChild + "\" as child relationship to instance of key \"" + first_native_key + "\"").writeToConsole();
            db1.showDB();
            db1.addOrDel_relation<int, string>(addAsChild, first_native_key, false);
            ("\n Database after removing \"" + addAsChild + "\" as child relationship from instance of key \"" + first_native_key + "\"").writeToConsole();
            db1.showDB();
            string edit_name = "Element_1 Native Edit", edit_desc = "Element_1 Native Edit Description";
            ("\n Database after editing name and description with \"" + edit_name + "\",\"" + edit_desc + "\" respectively of key \"" + first_native_key + "\" ").writeToConsole();
            db1.edit_Meta_NameDesc<int, string>(edit_name, first_native_key, true);
            db1.edit_Meta_NameDesc<int, string>(edit_desc, first_native_key, false);
            db1.showDB();
            ("\n Database after replacing value instance of key \"" + first_native_key + "\" ").writeToConsole();
            DBElement<int, string> replace = new DBElement<int, string>("Element_1 Native Replace", "Element_1 Native Replace Description");
            replace.children = new List<int> { 12, 13, 14 };
            replace.payload = "Element_1 Native Replace Data";
            db1.edit_Value<int, string>(replace, first_native_key);
            db1.showDB();
        }

        //----< To Demonstrate Requirement 4 part 2 >------------
        private void demo_Requirenment42(DBEngine<string, DBElement<string, List<string>>> db2)
        {
            IEnumerable<string> keys_d = db2.Keys();
            string first_enumerable_key = keys_d.First(); string addAsChild = "Added_Child";
            "".writeToConsole();
            "Collection payload Key/Value Database :".writeToConsole();
            db2.showEnumerableDB();
            db2.addOrDel_relation<string,List<string>, string>(addAsChild, first_enumerable_key, true);
            ("\n Database after adding \"" + addAsChild + "\" as child relationship to instance of key \"" + first_enumerable_key + "\"").writeToConsole();
            db2.showEnumerableDB();
            db2.addOrDel_relation<string, List<string>, string>(addAsChild, first_enumerable_key, false);
            ("\n Database after removing \"" + addAsChild + "\" as child relationship from instance of key \"" + first_enumerable_key + "\"").writeToConsole();
            db2.showEnumerableDB();
            string edit_name = "Element_1 List Edit", edit_desc = "Element_1 List Description Edit";
            ("\n Database after editing name and description with \"" + edit_name + "\",\"" + edit_desc + "\" respectively of key \"" + first_enumerable_key + "\" ").writeToConsole();
            db2.edit_Meta_NameDesc<string, List<string>, string>(edit_name, first_enumerable_key, true);
            db2.edit_Meta_NameDesc<string, List<string>, string>(edit_desc, first_enumerable_key, false);
            db2.showEnumerableDB();
            ("\n Database after replacing value instance of key \"" + first_enumerable_key + "\" ").writeToConsole();
            DBElement<string, List<string>> replace = new DBElement<string, List<string>>();
            replace.name = "Element_1 List Replace";
            replace.descr = "Element_1 List Description Replace";
            replace.payload = new List<string> { "Element_1 Data1 Replace", "Element_1 Data2 Replace", "Element_1 Data3 Replace" };
            replace.children = new List<string> { "List Child_1 Replace", "List Child_2 Replace", "List Child_3 Replace" };
            db2.edit_Value<string, List<string>, string>(replace, first_enumerable_key);
            db2.showEnumerableDB();
        }

        //----< To Demonstrate Requirement 5 >------------
        public void demo_Requirenment5(DBEngine<int, DBElement<int, string>> db1, DBEngine<string, DBElement<string, List<string>>> db2,string aug_filename,string aug_filename2)
        {
            "".writeToConsole();
            "Demonstrating Requirement 5:Save the database into XML file and augment XML file to database.".demosntrate_display();
            ("Persisting Native Database into XML file with name \""+db1.filename+"\"").writeToConsole();
            db1.dbStoreToStringType1<int,string,DBElement<int,string>>(); "".writeToConsole();
            ("Augmenting Native Database by XML file with name \"" + aug_filename + "\" that containing the following data").writeToConsole();
            string path_encap = "../../../../TestFolder/";
            try { XDocument augXml = new XDocument(); augXml = XDocument.Load(path_encap + aug_filename);
                augXml.ToString().writeToConsole();
            }
            catch(Exception e) { ("File " + aug_filename + " Loading error please check for file in path before loading\n Error:" + e.ToString()).writeToConsole(); }
            db1.getFromFile<int, string>(aug_filename);
            if (db1 != null)
            {
                "".writeToConsole();
                "Native Database after augmenting".writeToConsole();
                db1.showDB();
            }
            "".writeToConsole();
            ("Persisting Native Database into XML file with name \"" + db2.filename + "\"").writeToConsole();
            db2.dbStoreToStringType2<string, List<string>, DBElement<string,List< string>>,string>(); "".writeToConsole();
            ("Augmenting Collection payload Database by XML file with name \"" + aug_filename2 + "\" that containing the following data").writeToConsole();
            XDocument augXml2 = new XDocument();
            try { augXml2 = XDocument.Load(path_encap + aug_filename2);
                augXml2.ToString().writeToConsole(); }
            catch (Exception e) { ("File " + aug_filename2 + " Loading error please check for file in path before loading \n Error:"+e.ToString()).writeToConsole(); }
            db2.getFromFile<string,List<string>, string>(aug_filename2);
            if (db2 != null)
            {
                "".writeToConsole();
                "Collection Database after augmenting".writeToConsole();
                db2.showEnumerableDB();
            }
            "".writeToConsole();
            ("Restoring database from just saved database into file \"" + db2.filename + "\" that is before agumenting").writeToConsole();
            DBEngine<string, DBElement<string, List<string>>> db3 = new DBEngine<string, DBElement<string, List<string>>>("EnumerableDatabase2.xml");
            db3.getFromFile<string, List<string>, string>(db2.filename); "".writeToConsole();
            "Restored Database:".writeToConsole();
            db3.showEnumerableDB();
        }

        //----< To Demonstrate Requirement 8 >------------
        public void demo_Requirenment8()
        {           
            "".writeToConsole();
            "Demonstrating Requirement 8:Creating Immutable database".demosntrate_display();
            "Immutable database has been created by captureing the resultant keys from query into List<Key> in DBFactory which doesnt have any edit operations on the database".writeToConsole();
            "Check public interface of DBFactory for reference in 16th line  of DBFactory.cs".writeToConsole();
        }

        //----< To Demonstrate Requirement 9 >------------
        public void demo_Requirenment9(string filename)
        {
            "".writeToConsole();
            "Demonstrating Requirement 9:Load XML file, that describe your project's package structure and dependency relationships".demosntrate_display();
            try {
                XDocument package_structure = new XDocument();
                string path_encap = "../../../../TestFolder/";
                package_structure = XDocument.Load(path_encap + filename);
                package_structure.ToString().writeToConsole();
            }
            catch (Exception e) { ("File "+ filename + " Loading error please check for file in path before loading\n Error:" + e.ToString()).writeToConsole(); }
        }

        //----< To Demonstrate Requirement 6 >------------
        public void demo_Requirenment6(DBEngine<int, DBElement<int, string>> db1, DBEngine<string, DBElement<string, List<string>>> db2)
        {
            "".writeToConsole();
            "Demonstrating Requirement 6:Persisting Database through scheduler package after every 1 sec for three times".demosntrate_display();
            "Here for the demonstration purpose we are stoping the scheduler after three saves\n".writeToConsole();
            SchedulerPackage<int, List<string>, DBElement<int, string>, string> sch1 = new SchedulerPackage<int, List<string>, DBElement<int, string>, string>(db1);
            SchedulerPackage<string, List<string>,DBElement<string, List<string>>,string> sch2 = new SchedulerPackage<string, List<string>, DBElement<string, List<string>>, string>(db2);
        }

        //----< To Demonstrate Requirement 7 >------------
        public void demo_Requirenment7(DBEngine<int, DBElement<int, string>> db1, DBEngine<string, DBElement<string, List<string>>> db2)
        {
            "".writeToConsole();
            ("Demonstrating Requirement 7: Querying").title('-');
            this.queryForNativeDatabase(db1);
            this.queryForCollectionDatabase(db2);
        }

        //----< To Demonstrate Requirement 12 >------------
        public void demo_Requirenment12(string filename)
        {
            DBEngine<string, DBElement<string, List<string>>> db2 = new DBEngine<string, DBElement<string, List<string>>>("Bonus_out.xml");
            "".writeToConsole();
            ("Demonstrating Requirement 12: Implement categories by using a Dictionary<key,value>").title('-');
            "In this example Mobile, T.V and Laptop are the categories and Samsung, Apple etc are their manufacturers".writeToConsole();
            if (db2 != null)
            {
                try {
                    db2.getFromFile<string, List<string>, string>(filename);
                    db2.showEnumerableDB();
                    string type = "Mobile";
                    string manuf = "Samsung";
                    QueryPackage<string, List<string>> query_pack = new QueryPackage<string, List<string>>(db2);
                    DBElement<string, List<string>> value;
                    value = query_pack.queryValueWithKey(type);
                    List<string> manufs = value.payload;
                    ("Quering for \"" + type + "\" category manufacturers").writeToConsole();
                    manufs.getStringFromKeyList().writeToConsole();
                    "".writeToConsole();
                    ("Quering for all the categries that \"" + manuf + "\" produces").writeToConsole();
                    (query_pack.queryChildrenWithKey(manuf)).writeToConsole();
                }
                catch (Exception e) { ("File " + filename + " Loading error please check for file in path before loading\n Error:" + e.ToString()).writeToConsole(); }
            }
        }

        //----< To Demonstrate Requirement 7 part 1 for Native payload database query >------------
        private void queryForNativeDatabase(DBEngine<int, DBElement<int, string>> db1)
        {
            "Performing Query operations on Native Payload Database displayed below:".writeToConsole();
            db1.showDB();
            "".writeToConsole();
            IEnumerable<int> keys = db1.Keys();
            int first = keys.First();
            QueryPackage<int, string> query = new QueryPackage<int, string>(db1);
            this.queryValueWithKeyT1(query, first);
            this.queryChildrenWithKeyT1(query, first);
            this.querySearchMatchWithKeyT1(query, 1);
            this.querySearchMetadataT1(query, "Element_1");
            DateTime startdate = new DateTime(2012, DateTime.Today.Month, DateTime.Today.Day, 10, 5, 45);
            DateTime enddate = DateTime.Now;
            this.querySearchInTimeT1(query, startdate, enddate);
        }

        //----< To Demonstrate Requirement 7 part 2 for Collection payload database query >------------
        private void queryForCollectionDatabase(DBEngine<string, DBElement<string, List<string>>> db2)
        {
            "".writeToConsole();
            "Performing Query operations on Collection Payload Database displayed below:".writeToConsole();
            db2.showEnumerableDB();
            "".writeToConsole();
            IEnumerable<string> keys = db2.Keys();
            string first = keys.First();
            QueryPackage<string, List<string>> query = new QueryPackage<string, List<string>>(db2);
            this.queryValueWithKeyT2(query, first);
            this.queryChildrenWithKeyT2(query, first);
            this.querySearchMatchWithKeyT2(query, "1");
            this.querySearchMetadataT2(query, "Element_1");
            DateTime startdate = new DateTime(2012, DateTime.Today.Month, DateTime.Today.Day, 10, 5, 45);            
            this.querySearchInTimeT2(query, startdate, startdate);
        }

        //----< To Demonstrate Quering for value object of a database with key in collection database >------------
        private void queryValueWithKeyT2(QueryPackage<string,List<string>> query, string key)
        {
            ("Querying for \"" + key + "\" Key Value Object").writeToConsole();
            DBElement<string, List<string>> value;
            value = query.queryValueWithKey(key);
            ("Result of Query:").writeToConsole();
            if (value != null)
                value.showEnumerableElement();
            else
                "Key is not found in the Database".writeToConsole();
            WriteLine();
        }

        //----< To Demonstrate Quering for value object of a database with key in Native database >------------
        private void queryValueWithKeyT1(QueryPackage<int, string> query, int key)
        {
            ("Querying for \"" + key + "\" Key Value Object").writeToConsole();            
            DBElement<int, string> value;
            value = query.queryValueWithKey(key);
            ("Result of Query:").writeToConsole();
            if (value != null)
                value.showElement();
            else
                "Key is not found in the Database".writeToConsole();
            WriteLine();
        }

        //----< To Demonstrate Quering for children object of a database with key in collection database >------------
        private void queryChildrenWithKeyT2(QueryPackage<string, List<string>> query, string key)
        {
            ("Querying for \"" + key + "\" Key Children").writeToConsole();
            ("Result of the query:").writeToConsole();
            (query.queryChildrenWithKey(key)).writeToConsole();
        }

        //----< To Demonstrate Quering for children object of a database with key in native database >------------
        private void queryChildrenWithKeyT1(QueryPackage<int, string> query, int key)
        {
            ("Querying for \"" + key + "\" Key Children").writeToConsole();
            ("Result of the query:").writeToConsole();
            (query.queryChildrenWithKey(key)).writeToConsole();
        }

        //----< To Demonstrate Quering for collection of keys of a database that match with pattern in collection database >------------
        private void querySearchMatchWithKeyT2(QueryPackage<string, List<string>> query, string search)
        {
            "".writeToConsole();
            ("Querying for Keys that starts with \""+ search + "\" text").writeToConsole();
            List<string> key_list = query.querySearchMatchWithKey(search);
            ("Result of Query:").writeToConsole();
            key_list.getStringFromKeyList().writeToConsole();
        }

        //----< To Demonstrate Quering for collection of keys of a database that match with pattern in native database >------------
        private void querySearchMatchWithKeyT1(QueryPackage<int, string> query, int search)
        {
            "".writeToConsole();
            ("Querying for Keys that starts with \"" + search + "\" text").writeToConsole();
            List<int> key_list = query.querySearchMatchWithKey(search);
            ("Result of Query:").writeToConsole();
            key_list.getStringFromKeyList().writeToConsole();
        }

        //----< To Demonstrate Quering for collection of keys of a database that match with text of metadata(name or description) in collection database >------------
        private void querySearchMetadataT2(QueryPackage<string, List<string>> query, string search)
        {
            "".writeToConsole();
            ("Querying for Keys that contain \""+ search + "\" text in Metadata").writeToConsole();
            List<string> key_list = query.querySearchMetadata(search);
            key_list.getStringFromKeyList().writeToConsole();
        }

        //----< To Demonstrate Quering for collection of keys of a database that match with text of metadata(name or description) in native database >------------
        private void querySearchMetadataT1(QueryPackage<int, string> query, string search)
        {
            "".writeToConsole();
            ("Querying for Keys that contain \"" + search + "\" text in Metadata").writeToConsole();
            List<int> key_list = query.querySearchMetadata(search);
            key_list.getStringFromKeyList().writeToConsole();
        }

        //----< To Demonstrate Quering for collection of keys of a database that fall in time interval of collection database >------------
        private void querySearchInTimeT2(QueryPackage<string, List<string>> query, DateTime startTime, DateTime endTime)
        {
            "".writeToConsole();
            ("Querying for Keys that falls between time interval \"" + startTime + "\" and not passing end time so it takes now as end time").writeToConsole();
            List<string> key_list = query.querySearchInTime(startTime);
            key_list.getStringFromKeyList().writeToConsole();
        }

        //----< To Demonstrate Quering for collection of keys of a database that fall in time interval of native database >------------
        private void querySearchInTimeT1(QueryPackage<int, string> query, DateTime startTime, DateTime endTime)
        {
            "".writeToConsole();
            ("Querying for Keys that falls between time interval \"" + startTime + "\" and \"" + endTime + "\"").writeToConsole();
            List<int> key_list = query.querySearchInTime(startTime);
            key_list.getStringFromKeyList().writeToConsole();
        }
    }
#if (TEST_DEMOPACKAGE)
    class TestDemoPackage
  {
    //----< This is just a test stub >------------
    static void Main(string[] args)
    {
            "Demonstrating Project#2 Requirements".title('='); Write("\n");
            DemosnstrationPackage demo_obj = new DemosnstrationPackage();
            DBEngine<int, DBElement<int, string>> db1 = new DBEngine<int, DBElement<int, string>>("NativeDatabase1.xml");
            DBEngine<string, DBElement<string, List<string>>> db2 = new DBEngine<string, DBElement<string, List<string>>>("EnumerableDatabase.xml");
            demo_obj.demo_Requirenment2(db1, db2);
            demo_obj.demo_Requirenment3(db1, db2);
            demo_obj.demo_Requirenment4(db1, db2);
            DBEngine<int, DBElement<int, string>> db3 = new DBEngine<int, DBElement<int, string>>("NativeDatabase2.xml");
            demo_obj.demo_Requirenment5(db1, db2,"db1.xml","db2.xml");
            demo_obj.demo_Requirenment7(db1, db2);
            demo_obj.demo_Requirenment8();
            demo_obj.demo_Requirenment9("db2.xml");
            demo_obj.demo_Requirenment6(db1, db2);
            //"Press Enter to Quit".writeToConsole();
            Console.ReadKey();
        }
  }
#endif
}