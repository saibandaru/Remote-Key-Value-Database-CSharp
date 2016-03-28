///////////////////////////////////////////////////////////////
// DBExtension.cs - This package defines helper extensions   //
//                    for both DBElement and DBEngine        //
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
 * This package defines helper extensions for both DBElement and DBEngine
 *
 *  Public Interface
 *===================
 *
 *(1) keygen_int<Data>(this DBElement<int, Data> elem) 	Generates integer key
 *
 *(2) keygen_string<Data>(this DBElement<string, Data> elem)	Generates string key
 *
 *(3) showMetaData<Key, Data>(this DBElement<Key, Data> elem)	write metadata to string
 *
 *(4) showChildren<Key, Data>(this DBElement<Key, Data> elem)	Get children information and append into a string
 *
 *(5) showElement<Key, Data>(this DBElement<Key, Data> elem)	write details of element with simple Data to string
 *
 *(6) showEnumerableElement<Key, Data, T>(this DBElement<Key, Data> elem)	write details of an Enumerable element to string
 *
 *(7) showEnumerableValueType2<Key, Data, T>(this DBElement<Key, Data> elem,out XElement payload_element)	Convert DBElement payload into XElement instance
 *
 *(8) showEnumerableValueType1<Key, Data>(this DBElement<Key, Data> elem, out XElement payload_element)		Convert DBElement payload into XElement instance
 *
 *(9) populateDBElement<Key, Data,T>(this DBElement<Key, Data> elem, XElement keyvalue)	Populate DBElement from Xelement object	
 *
 *(10) parseValues<Key,Data,T>(this XElement payload) where Data : IEnumerable<T>	Get values from XElement instance
 *
 *(11) typeInfo(this string obj)		To identify the types and check if we can populate database
 *
 *(12) IsBetween<Key,Data>(this DBElement<Key, Data> elem, DateTime startTime, DateTime endTime)		Check if the timestamp falls in between the start and end dates given
 *
 *(13) getStringFromKeyList<Key>(this List<Key> keys)	Convert a list of keys into one continuous string
 *
 *(14) show<Key, Value, Data>(this DBEngine<Key, Value> db)	write simple db elements out to Console
 *
 *(15) showEnumerable<Key, Value, Data, T>(this DBEngine<Key, Value> db)	write enumerable db elements out to Console
 *
 *(16) keyStartsWith<Key, Data>(this DBEngine<Key, DBElement<Key, Data>> db, Key search) 	Search for the key that starts with some part of the key
 *
 */
/*
 * Maintenance:
 * ------------
 * Required Files: DBExtension.cs
 *                      DBElement.cs, DBEngine.cs and UtilityExtensions.cs
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
  /////////////////////////////////////////////////////////////////////
  // Extension methods class 
  // - Extension methods are static methods of a static class
  //   that extend an existing class by adding functionality
  //   not part of the original class.
  // - These methods are all extending the DBElement<Key, Data> class.
  //
  public static class DBElementExtensions
  {
      private static int seed = 0;
      private static int key = 0;
      //----< Generates integer key for a db element object >-----
      public static int keygen_int<Data>(this DBElement<int, Data> elem)
     {
            Func<int> keyGen = () => { ++key; return key; };
            return keyGen();
     }
     //----< Generates string key for a db element object >-----
    public static string keygen_string<Data>(this DBElement<string, Data> elem)
    {
            string skey = seed.ToString();
            Func<string> skeyGen = () =>
            {
                ++seed;
                skey = "string1001" + seed.ToString();
                skey = skey.GetHashCode().ToString();
                return skey;
            };            
            return skeyGen().ToString();            
     }
    //----< write metadata to string >--------------------------------
    public static string showMetaData<Key, Data>(this DBElement<Key, Data> elem)
    {
      StringBuilder accum = new StringBuilder();
      accum.Append(String.Format("\n  name: {0}", elem.name));
      accum.Append(String.Format("\n  desc: {0}", elem.descr));
      accum.Append(String.Format("\n  time: {0}", elem.timeStamp));
      if (elem.children.Count() > 0)
      {
        accum.Append(String.Format("\n  Children: "));
        bool first = true;
        foreach (Key key in elem.children)
        {
          if (first)
          {
            accum.Append(String.Format("{0}", key.ToString()));
            first = false;
          }
          else
            accum.Append(String.Format(", {0}", key.ToString()));
        }
      }
      return accum.ToString();
    }
    //----< Get children information and append into a string and return back >--------------------------------
    public static string showChildren<Key, Data>(this DBElement<Key, Data> elem)
    {
            StringBuilder accum = new StringBuilder();
            if (elem.children.Count() > 0)
            {
                bool first = true;
                foreach (Key key in elem.children)
                {
                    if (first)
                    {
                        accum.Append(String.Format("{0}", key.ToString()));
                        first = false;
                    }
                    else
                        accum.Append(String.Format(", {0}", key.ToString()));
                }
                return accum.ToString();
            }
            else
                return "Element doesn't have a child";
    }
    //----< write details of element with simple Data to string >-----
    public static string showElement<Key, Data>(this DBElement<Key, Data> elem)
    {
      StringBuilder accum = new StringBuilder();
      accum.Append(elem.showMetaData());
      if (elem.payload != null)
        accum.Append(String.Format("\n  payload: {0}", elem.payload.ToString()));
      return accum.ToString();
    }
    //----< write details of an Enumerable element to string >--------
    public static string showEnumerableElement<Key, Data, T>(this DBElement<Key, Data> elem)
      where Data : IEnumerable<T>  // constraint clause
    {
      StringBuilder accum = new StringBuilder();
      accum.Append(elem.showMetaData());
      if (elem.payload != null)
      {
        bool first = true;
        accum.Append(String.Format("\n  payload:\n  "));
        foreach (var item in elem.payload)  // won't compile without constraint clause
        {
          if (first)
          {
            accum.Append(String.Format("{0}", item));
            first = false;
          }
          else
            accum.Append(String.Format(", {0}", item));
        }
      }
      return accum.ToString();
    }
    //----< Convert DBElement payload into XElement instance >--------
    public static void showEnumerableValueType2<Key, Data, T>(this DBElement<Key, Data> elem,out XElement payload_element)
        where Data : IEnumerable<T>  // constraint clause
    {
            payload_element=new XElement("Payload");
            if (elem.payload != null)
            {
                
                foreach (var item in elem.payload)  // won't compile without constraint clause
                {
                    XElement payload_ele = new XElement("value", item.ToString());
                    payload_element.Add(payload_ele);
                }
            }           
    }
    //----< Convert DBElement payload into XElement instance >--------
    public static void showEnumerableValueType1<Key, Data>(this DBElement<Key, Data> elem, out XElement payload_element)
    {
            payload_element = new XElement("Payload");
            if (elem.payload != null)
            {
                XElement payload_ele = new XElement("value", elem.payload.ToString());
                payload_element.Add(payload_ele);               
            }
    }
    //----< Populate DBElement from Xelement object >--------
    public static void populateDBElement<Key, Data,T>(this DBElement<Key, Data> elem, XElement keyvalue)
             where Data : List<T>
    {
            elem.name = keyvalue.Attribute("name").Value.ToString();
            elem.descr = keyvalue.Attribute("description").Value.ToString();
            elem.timeStamp = DateTime.Parse(keyvalue.Attribute("timestamp").Value.ToString());
            XElement payload = keyvalue.Element("Payload");
            IEnumerable<XElement> values = payload.Elements("value");
            List<T> dummy = new List<T>();
            foreach (var value in values)
            {             
                T value_data = (T)Convert.ChangeType(value.Value, typeof(T));
                dummy.Add(value_data);
            }
            elem.payload = dummy as Data;
    }
    //----< Populate DBElement from Xelement object >--------
    public static void populateDBElement<Key, Data>(this DBElement<Key, Data> elem, XElement keyvalue)         
    {
            elem.name = keyvalue.Attribute("name").Value.ToString();
            elem.descr = keyvalue.Attribute("description").Value.ToString();
            elem.timeStamp = DateTime.Parse(keyvalue.Attribute("timestamp").Value.ToString());
            XElement payload = keyvalue.Element("Payload");
            XElement value = payload.Element("value");
            Data dummy;
            dummy = (Data)Convert.ChangeType(value.Value, typeof(Data));
            elem.payload = dummy;
    }
    //----< Get values from XElement instance >--------
    public static void parseValues<Key,Data,T>(this XElement payload) where Data : IEnumerable<T>
    {
            IEnumerable<XElement> values = payload.Elements("value");            
            if (typeof(Data).ToString()==typeof(List<string>).ToString())
            {
                //elem as  DBElement<Key, List<string>>();
                List<string> values_data=new List<string>();
                foreach (var value in values)
                {
                    string value_data = value.Value;
                    values_data.Add(value_data);
                }
                //elem.payload = values_data;
            }
    }
    //----< To identify the types and check if we can populate database >--------
    public static string typeInfo(this string obj)
    {
            if(obj ==typeof( int).ToString())
            {
                return "int";
            }
            else if(obj == typeof(string).ToString())
            {
                return "string";
            }
            else if(obj == typeof(List<int>).ToString())
            {
                return "List<int>";
            }
            else if(obj == typeof(List<string>).ToString())
            {
                return "List<string>";
            }
            else
            {
                return "undefined type";
            }
    }
    //----< Check if the timestamp falls in between the start and end dates given >--------
    public static bool IsBetween<Key,Data>(this DBElement<Key, Data> elem, DateTime startTime, DateTime endTime)
    {
            //Console.WriteLine("start" + startTime);
            DateTime time = elem.timeStamp;
            if (time.TimeOfDay == startTime.TimeOfDay) return true;
            if (time.TimeOfDay == endTime.TimeOfDay) return true;

            if (startTime.TimeOfDay <= endTime.TimeOfDay)
                return (time.TimeOfDay >= startTime.TimeOfDay && time.TimeOfDay <= endTime.TimeOfDay);
            else
                return !(time.TimeOfDay >= endTime.TimeOfDay && time.TimeOfDay <= startTime.TimeOfDay);
     }
     //----< Convert a list of keys into one continuous string >--------
     public static string getStringFromKeyList<Key>(this List<Key> keys)
     {
            StringBuilder accum = new StringBuilder();
            if (keys.Count() > 0)
            {
                bool first = true;
                foreach (var key_q in keys)
                {
                    if (first)
                    {
                        accum.Append(String.Format("{0}", key_q.ToString()));
                        first = false;
                    }
                    else
                        accum.Append(String.Format(", {0}", key_q.ToString()));
                }                
            }
            else
                accum.Append(String.Format("No Keys matching search text"));
            return accum.ToString();
        }
    }
    public static class DBEngineExtensions
    {
    
    //----< write simple db elements out to Console >------------------
    public static void show<Key, Value, Data>(this DBEngine<Key, Value> db)
    {
      foreach (Key key in db.Keys())
      {
        Value value;
        db.getValue(key, out value);
        DBElement<Key, Data> elem = value as DBElement<Key, Data>;
        Write("\n\n  -- key = {0} --", key);
        Write(elem.showElement());
      }
    }
    //----< write enumerable db elements out to Console >--------------
    public static void showEnumerable<Key, Value, Data, T>(this DBEngine<Key, Value> db)
      where Data : IEnumerable<T>
    {
      foreach (Key key in db.Keys())
      {
        Value value;
        db.getValue(key, out value);
        DBElement<Key, Data> elem = value as DBElement<Key, Data>;
        Write("\n\n  -- key = {0} --", key);
        Write(elem.showEnumerableElement<Key, Data, T>());
      }
    }
    //----< Search for the key that starts with some part of the key >--------------
    public static List<Key> keyStartsWith<Key, Data>(this DBEngine<Key, DBElement<Key, Data>> db, Key search) 
    {
            List<Key> result_keys = new List<Key>();
            foreach (Key key in db.Keys())
            {
               
                if (key.ToString().StartsWith(search.ToString()))
                    result_keys.Add(key);
            }
            return result_keys;
    }
}

#if (TEST_DBEXTENSIONS)
  class TestDBExtensions
  {
    //----< This is just a test stub for DBExtensions >--------
    static void Main(string[] args)
    {
      "Testing DBExtensions Package".title('=');
      WriteLine();

      Write("\n --- Test DBElement<int,string> ---");
      DBElement<int, string> elem1 = new DBElement<int, string>();
      elem1.payload = "a payload";
      Write(elem1.showElementWithTestType1<int>());
      WriteLine();

      Write("\n --- Test DBElement<string,List<string>> ---");
      DBElement<string, List<string>> newelem1 = new DBElement<string, List<string>>();
      newelem1.name = "newelem1";
      newelem1.descr = "test new type";
      newelem1.children = new List<string> { "Key1", "Key2" };
      newelem1.payload = new List<string> { "one", "two", "three" };
      Write(newelem1.showElementWithTestType2<string>());
      WriteLine();

      Write("\n\n");
    }
  }
#endif
}
