///////////////////////////////////////////////////////////////
// DBEleEdit.cs - This DBEleEdit is used edit value of       //
//                  that is DBElemeni in DBEngine            //
// Ver 1.0                                                   //
// Application: Demonstration for CSE681-SMA, Project#2      //
// Language:    C#, ver 6.0, Visual Studio 2015              //
// Platform:    ASUS SonicMaster, Core-i3, Windows 10        //
// Author:      Sai Krishna, Syracuse University             //
//              (813) 940-8083, sbandaru@syr.edu             //
///////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package helps us to edit Value object of Key-Value Database DBEngine<key, Value>, this is an class that has bunch of extension methods
 *                  DBEngine<key, Value> where Value is DBElement<Key, Data>.
 *
 *Public Interface
 *================
 *
 *(1)writes_call(DBEngine<Key, Value> db)  -->  this is static class used to store database into XML file
 *
 *(2)addKeyValuePair(this DBEngine<Key, DBElement<Key, Data>> db, DBElement<Key, Data> elem, Key key)   --> This is used to add new Key-Value instanve into Database
 *
 *(3)edit_Value(this DBEngine<Key, DBElement<Key, Data>> db, DBElement<Key, Data> elem, Key key)    -->This is used to replace Value instance of a Key-Value pair
 *
 *(4)edit_Meta_NameDesc(this DBEngine<Key, DBElement<Key, Data>> db, string name_desc, Key key, bool nameOrDesc)    -->This is used to edit name and description of metadata section of Value object 
 *
 *(5)addOrDel_relation(this DBEngine<Key, DBElement<Key, Data>> db, Key addkey, Key key, bool addOrDel) -->This function is used to add or delete child relation  for key of a database
 *
 */
/*
 * Maintenance:
 * ------------
 * Required Files: DBEleEdit.cs, UtilityExtensions.cs
 *                      DBElement.cs, DBEngine.cs, PersistEngine.CS
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
    public static class DBEleEdit
    {
        //<<---    To call persist on DBEngine and store database to XML file for IEnumerable type payload    -->>
        public static void writes_call<Key, Data,Value,T>(DBEngine<Key, Value> db) where Data : IEnumerable<T>
        {
            if (db.write_persist_flag)
            {
                ("For enum").save_title();
                ("Save call on Collection Database into \"" + db.filename + "\" as it reached maximum number of writes:Req 6").save_title();
                db.dbStoreToStringType2<Key, Data, Value, T>();
            }
        }
        //<<---    To call persist on DBEngine and store database to XML file for Native type payload    -->>
        public static void writes_call<Key, Data, Value>(DBEngine<Key, Value> db) 
        {
            if (db.write_persist_flag)
            {
                ("For normal").save_title();
                ("Save call on Native Database into \"" + db.filename + "\" as it reached maximum number of writes:Req 6").save_title();
                db.dbStoreToStringType1<Key, Data, Value>();
            }
        }
        //<<---    To add Key-Value pair to Database for IEnumerable type payload    -->>
        public static bool addKeyValuePair<Key, Data, T>(this DBEngine<Key, DBElement<Key, Data>> db, DBElement<Key, Data> elem, Key key) where Data : IEnumerable<T>
        {
            bool insert_result = db.insert(key, elem);
            if (insert_result)
            {
                db.numbetOfWrites++;
                if (db.numbetOfWrites >= db.number_writes_predef)
                { writes_call<Key, List<T>, DBElement<Key, Data>, T>(db); db.numbetOfWrites = 0; }
            }
            return (insert_result);
        }
        //<<---    To add Key-Value pair to Database for Native type payload    -->>
        public static bool addKeyValuePair<Key, Data>(this DBEngine<Key, DBElement<Key, Data>> db, DBElement<Key, Data> elem, Key key)
        {
            bool insert_result = db.insert(key, elem);
            if (insert_result)
            {
                db.numbetOfWrites++;
                if (db.numbetOfWrites >= db.number_writes_predef)
                { writes_call<Key, Data, DBElement<Key, Data>>(db); db.numbetOfWrites = 0; }
            }
            return (insert_result);
        }
        //<<---    To delete Key-Value pair to Database for Native type payload    -->>
        public static bool deleteKeyValuePair<Key, Data, T>(this DBEngine<Key, DBElement<Key, Data>> db, Key key) where Data : IEnumerable<T>
        {
            bool delete_result = db.delete(key);
            if (delete_result)
            {
                db.numbetOfWrites++;
                if (db.numbetOfWrites >= db.number_writes_predef)
                { writes_call<Key, List<T>, DBElement<Key, Data>, T>(db); db.numbetOfWrites = 0; }
            }
            return (delete_result);
        }
        //<<---    To add Key-Value pair to Database for Native type payload    -->>
        public static bool deleteKeyValuePair<Key, Data>(this DBEngine<Key, DBElement<Key, Data>> db, Key key)
        {
            bool delete_result = db.delete(key);
            //Console.WriteLine("Number {0}",db.Keys_count());

            //Console.WriteLine("Delete ::{0},{1}", key,delete_result);
            if (delete_result)
            {
                db.numbetOfWrites++;
                if (db.numbetOfWrites >= db.number_writes_predef)
                { writes_call<Key, Data, DBElement<Key, Data>>(db); db.numbetOfWrites = 0; }
            }
            return (delete_result);
        }
        //<<---    To replace Value object of Key-Value pair to Database of IEnumerable type payload    -->>
        public static bool edit_Value<Key, Data, T>(this DBEngine<Key, DBElement<Key, Data>> db, DBElement<Key, Data> elem,Key key) where Data : IEnumerable<T>
        {
            bool edit_result = db.edit(key, elem);
            if (edit_result)
            {
                db.numbetOfWrites++;
                if (db.numbetOfWrites >= db.number_writes_predef)
                { writes_call<Key, List<T>, DBElement<Key, Data>, T>(db); db.numbetOfWrites = 0; }
            }
            return (edit_result);
        }
        //<<---    To replace Value object of Key-Value pair to Database of Native type payload    -->>
        public static bool edit_Value<Key, Data>(this DBEngine<Key, DBElement<Key, Data>> db, DBElement<Key, Data> elem, Key key)
        {
            bool edit_result = db.edit(key, elem);
            if (edit_result)
            {
                db.numbetOfWrites++;
                if (db.numbetOfWrites >= db.number_writes_predef)
                { writes_call<Key, Data, DBElement<Key, Data>>(db); db.numbetOfWrites = 0; }
            }
            return (edit_result);
        }
        //<<---    To edit Name or Description of Metadata section of Value instance in Key-Value Database when payloaf is of IEnumerable type    -->>
        public static bool edit_Meta_NameDesc<Key, Data,T>(this DBEngine<Key, DBElement<Key, Data>> db, string name_desc, Key key, bool nameOrDesc) where Data : IEnumerable<T>
        {
            DBElement<Key, Data> elem;
            if (db.getValue(key, out elem))
            {
                if (nameOrDesc)
                    elem.name = name_desc;
                else
                    elem.descr = name_desc;
                db.numbetOfWrites++;
                if (db.numbetOfWrites >= db.number_writes_predef)
                { writes_call<Key, List<T>, DBElement<Key, Data>, T>(db); db.numbetOfWrites = 0; }
                return true;
            }
            return false;
        }
        //<<---    To edit Name or Description of Metadata section of Value instance in Key-Value Database when payloaf is of Native type    -->>
        public static bool edit_Meta_NameDesc<Key, Data>(this DBEngine<Key, DBElement<Key, Data>> db, string name_desc, Key key, bool nameOrDesc) 
        {
            DBElement<Key, Data> elem;
            if (db.getValue(key, out elem))
            {
                if (nameOrDesc)
                    elem.name = name_desc;
                else
                    elem.descr = name_desc;
                db.numbetOfWrites++;
                if (db.numbetOfWrites >= db.number_writes_predef)
                { writes_call<Key,Data, DBElement<Key, Data>>(db); db.numbetOfWrites = 0; }
                return true;
            }
            return false;
        }
        //<<---    To add or delete child relationship for an instance of Database for IEnumerable type payload    -->>
        public static bool addOrDel_relation<Key, Data,T>(this DBEngine<Key, DBElement<Key, Data>> db, Key addkey, Key key, bool addOrDel) where Data : IEnumerable<T>
        {
            DBElement<Key, Data> elem;
            if (db.getValue(key, out elem))
            {
                if (addOrDel)
                    elem.children.Add(addkey);
                else
                    elem.children.Remove(addkey);
                db.numbetOfWrites++;
                if (db.numbetOfWrites >= db.number_writes_predef)
                { writes_call<Key, List<T>, DBElement<Key, Data>, T>(db); db.numbetOfWrites = 0; }
                return true;
            }
            return false;
        }
        //<<---    To add or delete child relationship for an instance of Database for Native type payload    -->>
        public static bool addOrDel_relation<Key, Data>(this DBEngine<Key, DBElement<Key, Data>> db, Key addkey, Key key, bool addOrDel)
        {
            DBElement<Key, Data> elem;
            if (db.getValue(key, out elem))
            {
                if (addOrDel)
                    elem.children.Add(addkey);
                else
                    elem.children.Remove(addkey);
                db.numbetOfWrites++;
                if (db.numbetOfWrites >= db.number_writes_predef)
                { writes_call<Key,Data, DBElement<Key, Data>>(db); db.numbetOfWrites = 0; }
                return true;
            }
            return false;
        }
    }
#if (TEST_DBELEMENTEDIT)
    class TestDemoPackage
  {
    //<<---    Test stub for DBEleEdit Package    -->>
    static void Main(string[] args)
    {
            DBEngine<string, DBElement<string, List<string>>> db2 = new DBEngine<string, DBElement<string, List<string>>>("EnumerableDatabase.xml");
            IEnumerable<string> keys_d = db2.Keys();
            string first_enumerable_key = keys_d.First(); string addAsChild = "Added_Child";
            "Collection payload Key/Value Database :".writeToConsole();
            db2.showEnumerableDB();
            db2.addOrDel_relation<string, List<string>, string>(addAsChild, first_enumerable_key, true);
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
  }
#endif
}
