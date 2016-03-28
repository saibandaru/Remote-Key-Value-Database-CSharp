/////////////////////////////////////////////////////////////////////
// PersistEngine.cs - This pacakage helps to save database into    //
//                  an XML file and retrive or agument from XML    //
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
 * This pacakage helps to save database into
 *                          an XML file or retrive and agument from XML
 *
 *  Public Interface
 *===================
 *
 *(1) dbStoreToStringType1<Key, Data, Value>(this DBEngine<Key, Value> db)	function's task is to store native payload database into XML file
 *
 *(2) dbStoreToStringType2<Key,Data,Value, T>(this DBEngine<Key,Value> db)	function's task is to store Collection payload database into XML file
 *
 *(3) getFromFile<Key, Data,  T>(this DBEngine<Key, DBElement<Key,Data>> db,string filename)	function's restore/ augment Collection payload database from XML file
 *
 *(4) getFromFile<Key, Data>(this DBEngine<Key, DBElement<Key, Data>> db, string filename)	function's restore/ augment native payload database from XML file
 */
/*
 * Maintenance:
 * ------------
 * Required Files: PersistEngine.cs
 *  DBElement.cs, DBEngine.cs,DBExtension.cs,  Display.cs and UtilityExtensions.cs
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
using static System.Console;

namespace Project4Starter
{
    public static  class PersistEngine
    {
        private static string path_encap = "../../../../TestFolder/";
        //<<---    This function's task is to store native payload database into XML file    -->>
        public static void dbStoreToStringType1<Key, Data, Value>(this DBEngine<Key, Value> db) 
        {
            XDocument xml = new XDocument();
            xml.Declaration = new XDeclaration("1.0", "utf-8", "yes");
            XElement root = new XElement("NoSQLDatabase");
            XElement key_type = new XElement("KEY_TYPE", typeof(Key).ToString().typeInfo());
            XElement payload_type = new XElement("PAYLOAD_TYPE", typeof(Data).ToString().typeInfo());
            root.Add(key_type);
            root.Add(payload_type);
            xml.Add(root);
            foreach (Key key in db.Keys())
            {
                XElement key_value = new XElement("KEY");
                key_value.SetAttributeValue("key", key.ToString());
                Value value;
                db.getValue(key, out value);
                XElement payload_element;
                DBElement<Key, Data> elem = value as DBElement<Key, Data>;
                elem.showEnumerableValueType1<Key, Data>(out payload_element);
                key_value.Add(payload_element);
                key_value.SetAttributeValue("name", elem.name);
                key_value.SetAttributeValue("description", elem.descr);
                key_value.SetAttributeValue("timestamp", elem.timeStamp);
                if (elem.children.Count() > 0)
                {
                    foreach (var key_c in elem.children)
                    {
                        XElement child_element = new XElement("child", key_c.ToString());
                        key_value.Add(child_element);
                    }
                }
                root.Add(key_value);
            }
            string filename_store = path_encap + db.filename;
            try {
                xml.Save(filename_store); }
            catch (Exception e) { ("Directory not found to write file\n Error:" + e.ToString()).writeToConsole(); }
        }

        //<<---    This function's task is to store Collection payload database into XML file    -->>
        public static void dbStoreToStringType2<Key,Data,Value, T>(this DBEngine<Key,Value> db)
         where Data : IEnumerable<T>
        {
            XDocument xml = new XDocument();
            xml.Declaration = new XDeclaration("1.0", "utf-8", "yes");
            XElement root = new XElement("NoSQLDatabase");
            XElement key_type = new XElement("KEY_TYPE", typeof(Key).ToString().typeInfo());
            XElement payload_type = new XElement("PAYLOAD_TYPE", typeof(Data).ToString().typeInfo());
            root.Add(key_type);
            root.Add(payload_type);
            xml.Add(root);
            foreach (Key key in db.Keys())
            {
                XElement key_value = new XElement("KEY");
                key_value.SetAttributeValue("key",key.ToString());
                Value value; 
                db.getValue(key, out value);
                XElement payload_element;
                DBElement<Key, Data> elem = value as DBElement<Key, Data>;
                elem.showEnumerableValueType2<Key, Data, T>(out payload_element);
                key_value.Add(payload_element);
                key_value.SetAttributeValue("name", elem.name);
                key_value.SetAttributeValue("description", elem.descr);
                key_value.SetAttributeValue("timestamp", elem.timeStamp);
                if (elem.children.Count() > 0)
                {
                     foreach (var key_c in elem.children)
                    {                        
                        XElement child_element = new XElement("child", key_c.ToString());
                        key_value.Add(child_element);
                    }
                }          
                root.Add(key_value);
            }
            string filename_store = path_encap + db.filename;
            try
            {
                xml.Save(filename_store);
            }
            catch (Exception e) { ("Directory not found to write file\n Error:" + e.ToString()).writeToConsole(); }
        }

        public static void dbStoreToStringType22<Key, Data, Value, T>(this DBEngine<Key, Value> db,string filename)
         where Data : IEnumerable<T>
        {
            XDocument xml = new XDocument();
            xml.Declaration = new XDeclaration("1.0", "utf-8", "yes");
            XElement root = new XElement("NoSQLDatabase");
            XElement key_type = new XElement("KEY_TYPE", typeof(Key).ToString().typeInfo());
            XElement payload_type = new XElement("PAYLOAD_TYPE", typeof(Data).ToString().typeInfo());
            root.Add(key_type);
            root.Add(payload_type);
            xml.Add(root);
            foreach (Key key in db.Keys())
            {
                XElement key_value = new XElement("KEY");
                key_value.SetAttributeValue("key", key.ToString());
                Value value;
                db.getValue(key, out value);
                XElement payload_element;
                DBElement<Key, Data> elem = value as DBElement<Key, Data>;
                elem.showEnumerableValueType2<Key, Data, T>(out payload_element);
                key_value.Add(payload_element);
                key_value.SetAttributeValue("name", elem.name);
                key_value.SetAttributeValue("description", elem.descr);
                key_value.SetAttributeValue("timestamp", elem.timeStamp);
                if (elem.children.Count() > 0)
                {
                    foreach (var key_c in elem.children)
                    {
                        XElement child_element = new XElement("child", key_c.ToString());
                        key_value.Add(child_element);
                    }
                }
                root.Add(key_value);
            }
            string filename_store = path_encap + filename;
            try
            {
                xml.Save(filename_store);
            }
            catch (Exception e) { ("Directory not found to write file\n Error:" + e.ToString()).writeToConsole(); }
        }

        //<<---    This function's restore/ augment Collection payload database from XML file    -->>
        public static DBEngine<Key, DBElement<Key, Data>> getFromFile<Key, Data,  T>(this DBEngine<Key, DBElement<Key,Data>> db,string filename)
         where Data : List<T>
        {
            try {
                XDocument xml = new XDocument();
                xml = XDocument.Load(path_encap + filename);
                XElement keyValues1 = xml.Element("NoSQLDatabase");
                XElement key = keyValues1.Element("KEY_TYPE");
                XElement payloat_t = keyValues1.Element("PAYLOAD_TYPE");
                IEnumerable<XElement> keyValues = keyValues1.Elements("KEY");
                if (checkForCompatablety(typeof(Key).ToString().typeInfo(), typeof(Data).ToString().typeInfo(), key.Value.ToString(), payloat_t.Value.ToString()))
                {
                    foreach (var keyValue in keyValues)
                    {
                        if (keyValue.Name.ToString().Equals("KEY"))
                        {
                            DBElement<Key, Data> newele = new DBElement<Key, Data>();
                            Key value_data = (Key)Convert.ChangeType(keyValue.Attribute("key").Value, typeof(Key));
                            XElement Value_ele = keyValue.Element("Payload");
                            newele.populateDBElement<Key, Data, T>(keyValue);
                            IEnumerable<XElement> children = keyValue.Elements("child");
                            if (children.Count() > 0)
                            {
                                foreach (var ch_c in children)
                                {
                                    Key child = (Key)Convert.ChangeType(ch_c.Value, typeof(Key));
                                    newele.children.Add(child);
                                }
                            }
                            db.insert(value_data, newele);
                        }
                    }
                }
                else
                    "Given input XML file is not compatable to load into the this database types".writeToConsole();
            }
            catch (Exception e) { ("File " + filename + " Loading error please check for file in path before loading\n Error:" + e.ToString()).writeToConsole(); }
            return db;       
        }

        public static DBEngine<Key, DBElement<Key, Data>> getFromFile1<Key, Data, T>(this DBEngine<Key, DBElement<Key, Data>> db, string filename,out bool processed)
        where Data : List<T>
        {
            try
            {
                XDocument xml = new XDocument();
                xml = XDocument.Load(path_encap + filename);
                XElement keyValues1 = xml.Element("NoSQLDatabase");
                XElement key = keyValues1.Element("KEY_TYPE");
                XElement payloat_t = keyValues1.Element("PAYLOAD_TYPE");
                IEnumerable<XElement> keyValues = keyValues1.Elements("KEY");
                if (checkForCompatablety(typeof(Key).ToString().typeInfo(), typeof(Data).ToString().typeInfo(), key.Value.ToString(), payloat_t.Value.ToString()))
                {
                    foreach (var keyValue in keyValues)
                    {
                        if (keyValue.Name.ToString().Equals("KEY"))
                        {
                            DBElement<Key, Data> newele = new DBElement<Key, Data>();
                            Key value_data = (Key)Convert.ChangeType(keyValue.Attribute("key").Value, typeof(Key));
                            XElement Value_ele = keyValue.Element("Payload");
                            newele.populateDBElement<Key, Data, T>(keyValue);
                            IEnumerable<XElement> children = keyValue.Elements("child");
                            if (children.Count() > 0)
                            {
                                foreach (var ch_c in children)
                                {
                                    Key child = (Key)Convert.ChangeType(ch_c.Value, typeof(Key));
                                    newele.children.Add(child);
                                }
                            }
                            db.insert(value_data, newele);
                        }
                    }
                    processed = true;
                }
                else
                { "Given input XML file is not compatable to load into the this database types".writeToConsole(); processed = false; }
                }
            catch (Exception e) { string exception = e.ToString(); ("File " + filename + " Loading error please check for file in path before loading\n " ).writeToConsole(); processed = false; }
            return db;
        }

        //<<---    This function's restore/ augment native payload database from XML file    -->>
        public static DBEngine<Key, DBElement<Key, Data>> getFromFile<Key, Data>(this DBEngine<Key, DBElement<Key, Data>> db, string filename)
        {
            try {
                XDocument xml = new XDocument();
                xml = XDocument.Load(path_encap + filename);
                XElement keyValues1 = xml.Element("NoSQLDatabase");
                XElement key = keyValues1.Element("KEY_TYPE");
                XElement payloat_t = keyValues1.Element("PAYLOAD_TYPE");
                IEnumerable<XElement> keyValues = keyValues1.Elements("KEY");
                if (checkForCompatablety(typeof(Key).ToString().typeInfo(), typeof(Data).ToString().typeInfo(), key.Value.ToString(), payloat_t.Value.ToString()))
                {
                    foreach (var keyValue in keyValues)
                    {
                        if (keyValue.Name.ToString().Equals("KEY"))
                        {
                            DBElement<Key, Data> newele = new DBElement<Key, Data>();
                            Key value_data = (Key)Convert.ChangeType(keyValue.Attribute("key").Value, typeof(Key));
                            XElement Value_ele = keyValue.Element("Payload");
                            newele.populateDBElement<Key, Data>(keyValue);
                            IEnumerable<XElement> children = keyValue.Elements("child");
                            if (children.Count() > 0)
                            {
                                foreach (var ch_c in children)
                                {
                                    Key child = (Key)Convert.ChangeType(ch_c.Value, typeof(Key));
                                    newele.children.Add(child);
                                }
                            }
                            db.insert(value_data, newele);
                        }
                    }

                }
                else
                    "Given input XML file is not compatable to load into the this database types".writeToConsole();
            }
            catch (Exception e) { ("File " + filename + " Loading error please check for file in path before loading\n Error:" + e.ToString()).writeToConsole(); }
            return db;
        }

        public static bool checkForCompatablety(string inputDBKey, string inputDBValue, string storedDBKey, string storedDBvalue)
        {
            if ((inputDBKey == storedDBKey) && (inputDBValue == storedDBvalue))
                return true;
            return false;
        }

    }
#if (TEST_DBPERSISTENGINE)
    class TestDemoPackage
  {
    
    //<<---    This is just a test stub    -->>
    static void Main(string[] args)
    {
            DBEngine<int, DBElement<int, string>> db1 = new DBEngine<int, DBElement<int, string>>("NativeDatabase1.xml");
            DBEngine<string, DBElement<string, List<string>>> db2 = new DBEngine<string, DBElement<string, List<string>>>("EnumerableDatabase.xml");
            ("\nPersisting Native Database into XML file with name \"" + db1.filename + "\"").writeToConsole();
            db1.dbStoreToStringType1<int, string, DBElement<int, string>>();
            string aug_filename2 = "db2.xml";
            ("\nAugmenting Collection payload Database by XML file with name \"" + aug_filename2 + "\" that containing the following data").writeToConsole();
            XDocument augXml2 = new XDocument(); augXml2 = XDocument.Load(aug_filename2);
            augXml2.ToString().writeToConsole();
            db2.getFromFile<string, List<string>, string>(aug_filename2);
            "\nNative Database after augmenting".writeToConsole();
            db2.showEnumerableDB();
        }
  }
#endif
}
