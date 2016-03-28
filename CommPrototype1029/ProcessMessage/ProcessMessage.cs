/*/////////////////////////////////////////////////////////////
// ProcessMessage.cs - Used for the processing of            //
//       different  XML messages that can be communicated    //
//                  between server and client                //
// Ver 1.0                                                   //
// Application: Demonstration for CSE681-SMA, Project#4      //
// Language:    C#, ver 6.0, Visual Studio 2015              //
// Platform:    ASUS SonicMaster, Core-i3, Windows 10        //
// Author:      Sai Krishna, Syracuse University             //
//              (813) 940-8083, sbandaru@syr.edu             //
///////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package processing of different   
 *                 XML messages that can be communicated   
 *                  between server and client 
 *
 *  Public Interface
 *===================
 *
 *void constructListXEle(List<string> list, string name, ref XElement root)	constructor list of XElement to root from List of strings
 *
 *List<string> constructListfromXEle(XElement root, string name)			constructor list of strings from XElement root
 *
 *string parseRequestMessageForString(Message msg, string reqInfo)		Get required information by giving its tag
 *
 *List<string> parseRequestMessageForList(Message msg, string reqInfo)		get list from Message object
 *
 **Message Operate(Message ipmsg,ulong preformance)				main server operations are processed
 *
 */
/*
 * Maintenance History:
 * --------------------
 * ver 1.0 : 29 Oct 2015
 * - first release
*
* Build Process:  devenv CommPrototype.sln /Rebuild debug
*                 Run from Developer Command Prompt
*                 To find: search for developer
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace Project4Starter
{
    ////////////////////////////////////////////////////////
    //ProcessMessage is used to process server side messages
    //
    public class ProcessMessage
    {
        private DBEngine<string, DBElement<string, List<string>>> noSQL;

        //----< constructor to initalize noSQL database >--------
        public ProcessMessage(DBEngine<string, DBElement<string, List<string>>> noSQL)
        {
            this.noSQL = noSQL;
        }

        //----< constructor list of XElement to root from List of strings >--------
        public void constructListXEle(List<string> list, string name, ref XElement root)
        {
            foreach (string value in list)
            {
                XElement ele = new XElement(name, value);
                root.Add(ele);
            }
        }

        //----< constructor list of strings from XElement root >--------
        public List<string> constructListfromXEle(XElement root, string name)
        {
            List<string> list = new List<string>();
            XElement param = root.Element("PARAMS");
            IEnumerable<XElement> values = param.Elements(name);
            foreach (XElement value in values)
                list.Add(value.Value.ToString());            
            return list;
        }

        //----< Get required information by giving its tag >--------
        public string parseRequestMessageForString(Message msg, string reqInfo) //COMMAND NAME DESP KEY
        {
            String info = "!exists"; //Console.WriteLine("bsahda{0}", msg.content);
            XElement content = XElement.Parse(msg.content);
            XElement cmd = content.Element("COMMAND");
            XElement paramets= content.Element("PARAMS");
            XElement name = paramets.Element("NAME");
            XElement desp = paramets.Element("DESP");
            XElement key = paramets.Element("KEY");
            XElement type = paramets.Element("TYPE");
            XElement infoe = paramets.Element("INFO");
            XElement infoe1 = paramets.Element("INFO1");
            XElement infoe2 = paramets.Element("INFO2");
            if (reqInfo == "COMMAND" && cmd != null)
                info = cmd.Value.ToString();
            else if (reqInfo == "NAME" && name != null)
                info = name.Value.ToString();
            else if (reqInfo == "DESP" && desp != null)
                info = desp.Value.ToString();
            else if (reqInfo == "KEY" && key != null)
            {      info = key.Value.ToString(); }
            else if (reqInfo == "TYPE" && type != null)
            { info = type.Value.ToString(); }
            else if (reqInfo == "INFO" && infoe != null)
            { info = infoe.Value.ToString(); }
            else if (reqInfo == "INFO1" && infoe1 != null)
            { info = infoe1.Value.ToString(); }
            else if (reqInfo == "INFO2" && infoe2 != null)
            { info = infoe2.Value.ToString(); }
            if (info == "!exists") Console.WriteLine("Wrong parse string {0}p", reqInfo);
            return info;
        }

        //----< get list from Message object >--------
        public List<string> parseRequestMessageForList(Message msg, string reqInfo) //CHILDREN PAYLOAD
        {
            List<string> list;            
            XElement content = XElement.Parse(msg.content);
            if (reqInfo == "CHILDREN")
                list = constructListfromXEle(content, "CHILDREN");
            else if (reqInfo == "PAYLOAD")
                list = constructListfromXEle(content, "PAYLOAD");
            else
                list = new List<string>();            
            return list;
        }

        public string getCommand(Message msg)
        {
            return this.parseRequestMessageForString(msg, "COMMAND"); ;
        }
        //----< main server operations are processed >--------
        public Message Operate(Message ipmsg,ulong preformance)
        {
            Message msg = ipmsg;
            String response="result";
            String command = this.parseRequestMessageForString(ipmsg, "COMMAND");
            //send_Event()
            if (command == "INSERT" || command == "DELETE" || command == "EDIT" || command == "EDIT_I")//INSERT DELETE EDIT
                response = this.WriteOperate(ipmsg, command);
            else if (command == "QUERY")
                response = this.ReadOperate(ipmsg, command);
            else if (command == "SAVE_RESTORE")
                response = this.SaveRestoreOperate(ipmsg, command);
            else if (command == "P_TIME")
            { response = this.performance(ipmsg, preformance.ToString());  }//this.PrefOperate(ipmsg, preformance.ToString()); }
            else if (command == "NOTIFY")
                response = this.notified(ipmsg);
            msg.content = response;

            Utilities.swapUrls(ref msg);
            return msg;
        }
        public string notified(Message ipmsg)
        {
            String address = this.parseRequestMessageForString(ipmsg, "TYPE");
            Console.WriteLine("\nThe new server component registeration has been notified\n with address {0}", address);
            return "notify";
        }

        public Message OperateCSM(Message ipmsg, ulong preformance,List<string> registered,Sender sndr)
        {
            Message msg = ipmsg;
            String response = "result";
            String command = this.parseRequestMessageForString(ipmsg, "COMMAND");
            //send_Event()
            if (command == "REGISTER")//INSERT DELETE EDIT
            {
                response = this.WriteOperate(ipmsg, command);
                //String addressURL = this.parseRequestMessageForString(ipmsg, "TYPE");
                MessageMaker mm = new MessageMaker();
                Message newmsg = new Message();
                Console.WriteLine("\nA new Server registered at Centralized Server Manager\n with address: {0}", ipmsg.fromUrl);
                foreach (string addredd in registered)
                {
                    newmsg.content = mm.notify(ipmsg.fromUrl);
                    newmsg.toUrl = addredd;
                    newmsg.fromUrl = ipmsg.toUrl;
                    sndr.sendMessage(newmsg);
                }
                registered.Add(ipmsg.fromUrl);
            }
            else { }

            msg.content = response;
            Utilities.swapUrls(ref msg);
            return msg;
        }

        //----< write server operations are processed >--------
        public void send_Event(Message ipmsg,string command)
        {

        }

        //----< write server operations are processed >--------
        private string WriteOperate(Message ipmsg, string command)
        {
            MessageMaker mm = new MessageMaker();
            string key, responce = "no exe";
            if (command == "INSERT")            {
                if (insert(ipmsg, out key))
                    responce = "Inserted with key " + key;
                else responce = "Insert failed";                
            }
            else if (command == "DELETE")            {
                if (delete(ipmsg, out key))
                    responce = "Element with Key: " + key + " is deleted";
                else responce = "Delete failed";
            }
            else if (command == "EDIT")            {
                if (edit(ipmsg, out key))
                    responce = "Element with Key: " + key + " is edited";
                else responce = "Edit failed";
            }
            else if (command == "EDIT_I")            {
                if (edit_i(ipmsg, out key))
                    responce = "Element with Key: " + key + " is edited";
                else responce = "Edit failed";
            }
            return mm.makeMessageReply(ipmsg, responce);           
        }

        //----< performance server operation are processed >--------
        private string performance(Message ipmsg, string preformance)
        {
            MessageMaker mm = new MessageMaker();            
            return mm.makeMessageReply(ipmsg, preformance);
        }

        //----< read server operations are processed >--------
        private string ReadOperate(Message ipmsg, string command)
        {
            MessageMaker mm = new MessageMaker();
            string context,key,starttime,endtime, responce = "no exe";
            string type=this.parseRequestMessageForString(ipmsg, "TYPE");
            QueryPackage<string, List<string>> query = new QueryPackage<string, List<string>>(noSQL);
            if (type == "KEY_S"|| type == "KEY_S_E")  {
                if (keySearch(ipmsg, query, out context,out key))
                    responce = context;
                else responce = "Key "+key+" Not Found to get value";                
            }
            else if (type == "CHILD_S") {
                if (childSearch(ipmsg, query, out context, out key))
                    responce = context;
                else responce = "Key " + key + " Not Found to get children";
            }
            else if (type == "PATTERN_S")   {
                if (patternSearch(ipmsg, query, out context, out key))
                    responce = context;
                else responce = "No Keys found that match with text "+ key;
            }
            else if (type == "METADATA_S")  {
                if (metadataSearch(ipmsg, query, out context, out key))
                    responce = context;
                else responce = "No Keys found that match with text " + key+"in therir metadata";
            }
            else if (type == "TIME_S")  {
                if (timeSearch(ipmsg, query, out context, out starttime,out endtime))
                    responce = context;
                else responce = "No Keys found between "+starttime+" and " + endtime;
            }
            return mm.makeMessageReply(ipmsg, responce);
        }

        //----< save/restore server operations are processed >--------
        private string SaveRestoreOperate(Message ipmsg, string command)
        {
            MessageMaker mm = new MessageMaker();
            string filename,responce = "no exe";
            string type = this.parseRequestMessageForString(ipmsg, "TYPE");            
            if (type == "SAVE")
            {
                if (save(ipmsg,out filename))
                    responce = "Database is saved to "+filename+" file";
                else responce = "Error occurred while saving Database to " + filename + " file";
            }
            else if (type == "PERSIST")
            {
                if (restore(ipmsg, out filename))
                    responce = "Database is augmented from " + filename + " file";
                else responce = "Error occurred while Database is augmented from " + filename + " file";
            }
            return mm.makeMessageReply(ipmsg, responce);
        }

        //----< save server operations are processed >--------
        private bool save(Message ipmsg, out string filename)
        {
            filename = this.parseRequestMessageForString(ipmsg, "INFO1");
            try
            {
                noSQL.dbStoreToStringType22<string, List<string>, DBElement<string, List<string>>, string>(filename);
                return true;
            }
            catch (Exception e) { string exception = e.ToString(); return false; }
        }

        //----< restore server operations are processed >--------
        private bool restore(Message ipmsg, out string filename)
        {
            filename = this.parseRequestMessageForString(ipmsg, "INFO1");
            bool processed;
            try
            {
                noSQL.getFromFile1<string, List<string>, string>(filename,out processed);
                return processed;
            }
            catch (Exception e) { string exception = e.ToString(); return false; }           
        }

        //----< process performance request in server >--------
        private string PrefOperate(Message ipmsg,string pref_time)
        {
            MessageMaker mm = new MessageMaker();
            return mm.makeMessageReply(ipmsg, pref_time);
        }

        //----< insert server operations are processed >--------
        private bool insert(Message ipmsg, out string key)
        {
            DBElement<string, List<string>> newelem1 = new DBElement<string, List<string>>();
            newelem1.name = this.parseRequestMessageForString(ipmsg, "NAME");
            newelem1.descr = this.parseRequestMessageForString(ipmsg, "DESP");
            newelem1.payload = this.parseRequestMessageForList(ipmsg, "PAYLOAD");
            newelem1.children = this.parseRequestMessageForList(ipmsg, "CHILDREN");
            key = newelem1.keygen_string();            
            return noSQL.addKeyValuePair<string, List<string>, string>(newelem1, key);
        }

        //----< delete server operations are processed >--------
        private bool delete(Message ipmsg, out string key)
        {
            key = this.parseRequestMessageForString(ipmsg, "KEY");
            return noSQL.deleteKeyValuePair<string, List<string>>(key);
        }

        //----< edit server operations are processed >--------
        private bool edit(Message ipmsg, out string key)
        {
            key = this.parseRequestMessageForString(ipmsg, "KEY");
            string typeOfEdit= this.parseRequestMessageForString(ipmsg, "TYPE");            
            if (typeOfEdit == "NAME")
            {
                return noSQL.edit_Meta_NameDesc<string, List<string>>(this.parseRequestMessageForString(ipmsg, "INFO"), key, true);//INFO
            }
            else if (typeOfEdit == "DESP")
            {
                return noSQL.edit_Meta_NameDesc<string, List<string>>(this.parseRequestMessageForString(ipmsg, "INFO"), key, false);//INFO
            }
            else if (typeOfEdit == "ADDCHILD")
            {
                return noSQL.addOrDel_relation<string, List<string>>(this.parseRequestMessageForString(ipmsg, "INFO"), key, true);//INFO
            }
            else if (typeOfEdit == "REMOVECHILD")
            {
                return noSQL.addOrDel_relation<string, List<string>>(this.parseRequestMessageForString(ipmsg, "INFO"), key, false);//INFO
            }
            else return false;
        }

        //----< edit value server operations are processed >--------
        private bool edit_i(Message ipmsg, out string key)
        {
            DBElement<string, List<string>> newelem1 = new DBElement<string, List<string>>();
            newelem1.name = this.parseRequestMessageForString(ipmsg, "NAME");
            newelem1.descr = this.parseRequestMessageForString(ipmsg, "DESP");
            newelem1.payload = this.parseRequestMessageForList(ipmsg, "PAYLOAD");
            newelem1.children = this.parseRequestMessageForList(ipmsg, "CHILDREN");
            key = this.parseRequestMessageForString(ipmsg, "KEY");            
            return noSQL.edit_Value<string, List<string>, string>(newelem1, key);            
        }

        //----< key query server operations are processed >--------
        private bool keySearch(Message ipmsg, QueryPackage<string, List<string>> query, out string context,out string key)
        {
            context = "!exists";
            key= this.parseRequestMessageForString(ipmsg, "INFO1");            
            DBElement<string, List<string>> value;
            value = query.queryValueWithKey(key);
            if (value != null)
            { context = value.showEnumerableElement<string, List<string>,string>(); return true; }
            else
                return false;
        }

        //----< query children server operations are processed >--------
        private bool childSearch(Message ipmsg, QueryPackage<string, List<string>> query, out string context, out string key)
        {
            context = "!exists";
            key = this.parseRequestMessageForString(ipmsg, "INFO1");
            DBElement<string, List<string>> value;
            value = query.queryValueWithKey(key);
            //return value.showChildren();
            if (value != null)
            {
                if (value.children.Count() > 0)
                {
                    context = "Children for key "+key+"\n"+value.showChildren(); return true;
                }
                else
                {
                    context = "No Children for the key " + key; return true;
                }
            }
            else
                return false;
        }

        //----< Query pattern server operations are processed >--------
        private bool patternSearch(Message ipmsg, QueryPackage<string, List<string>> query, out string context, out string pattern)
        {
            context = "!exists";
            pattern = this.parseRequestMessageForString(ipmsg, "INFO1");
            List<string> key_list = query.querySearchMatchWithKey(pattern);
            if (key_list.Count != 0)
            { context = "The list of Keys that match pattern are: "+key_list.getStringFromKeyList(); return true; }
            else
                return false;
        }

        //----< Query metadata server operations are processed >--------
        private bool metadataSearch(Message ipmsg, QueryPackage<string, List<string>> query, out string context, out string meta)
        {
            context = "!exists";
            meta = this.parseRequestMessageForString(ipmsg, "INFO1");
            List<string> key_list = query.querySearchMetadata(meta);
            if (key_list.Count != 0)
            { context = "The list of Keys that has string in metadata are: "+ key_list.getStringFromKeyList(); return true; }
            else
                return false;
        }

        //----< Query time server operations are processed >--------
        private bool timeSearch(Message ipmsg, QueryPackage<string, List<string>> query, out string context, out string startT,out string endT)
        {
            context = "!exists";
            startT = this.parseRequestMessageForString(ipmsg, "INFO1");
            DateTime startdt = Convert.ToDateTime(startT);
            endT = this.parseRequestMessageForString(ipmsg, "INFO2");
            DateTime stopdt = Convert.ToDateTime(endT);
            List<string> key_list = query.querySearchInTime(startdt, stopdt);
            if (key_list.Count != 0)
            { context = key_list.getStringFromKeyList(); return true; }
            else
                return false;
        }

#if (TEST_PROCESSMESSAGE)
        static void Main(string[] args)
        {
            DBEngine<string, DBElement<string, List<string>>> noSQL= new DBEngine<string, DBElement<string, List<string>>>("Database.xml", false);
            ProcessMessage pm = new ProcessMessage(noSQL);
            Message msg = new Message();
            ulong long_n=10;
            pm.Operate(msg,long_n);
        }
#endif
    }
}
