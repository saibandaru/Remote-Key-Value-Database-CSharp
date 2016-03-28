/*/////////////////////////////////////////////////////////////
// MakeMessage.cs - Used for the construction of different   //
//                  XML messages that can be communicated    //
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
 * This package Used for the construction of different   
 *                 XML messages that can be communicated   
 *                  between server and client 
 *
 *  Public Interface
 *===================
 *
 *Message makeMessage(string fromUrl, string toUrl)	make a Dummy message
 *
 *Message makeMessage(string fromUrl,string toUrl,string context)		Construct message with some content provided
 *
 *string makeMessageReply(Message ipmsg,string response)		Construct reply message with some responce message
 *
 *List<string> makeList(string value,string delimiter=";")	Construct a list from string delimited by some character
 *
 *string getMessageReply(Message rplymsg, out string type,out string thru)	Construct reply with some responce message
 *
 *void getEditSearch(Message msg, out string name, out string description, out string children, out string payload)	Construct edit message back
 *
 *void constructListXEle(List<string> list,string name,ref XElement root)		Construct list if XEelement with input string message add it to root
 *
 *List<string> constructListfromXEle(XElement root, string name)	Construct list if XEelement with input string message and return it
 *
 *string constructInsertContext(string name,string desp,List<string> children,List<string> payload)	Construct insert message
 *
 *string constructDeleteContext(string key)	Construct delete message
 *
 *string constructEditContext(string key,string type,string info)		Construct edit message
 *
 *string constructPersistContext(string name)	Construct persist message
 *
 *string constructRestoreContext(string name)	Construct restore message
 *
 *Message constructprefrequest(string fromUrl, string toUrl)	Construct preformance request message
 *
 *string constructSaveRestoreContext(string type,string info1)	Construct save/restore message
 *
 *string constructQueryContext(string type, string info1, string info2 = "nothing")	Construct Query message
 *
 *string parseRequestMessageForString(Message msg, string reqInfo)	parse value using tag
 *
 *void savePref(string filename,string preformance)	Save latency of different clients 
 *
 *void saveClear(string filename)		clear file latency used by different clients
 *
 *string getLat(string filename)	parse average latency from file
 *
 *string getLat(string filename)				parse average latency from file
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
using System.Text.RegularExpressions;
using System.Threading;


namespace Project4Starter
{
    /////////////////////////////////////////////////////////////
    //MessageMaker is used to construct send messages messages
    //
  public class MessageMaker
  {        
        public static int msgCount { get; set; } = 0;

        //----< make a Dummy message >--------
        public Message makeMessage(string fromUrl, string toUrl)
        {
            Message msg = new Message();
            msg.fromUrl = fromUrl;
            msg.toUrl = toUrl;
            msg.content = String.Format("\n  message #{0}", ++msgCount);
            return msg;
        }

        //----< Construct message with some content provided >--------
        public Message makeMessage(string fromUrl,string toUrl,string context)
        {
            Message msg = new Message();
            msg.toUrl = toUrl;
            msg.fromUrl = fromUrl;
            msg.content = context;
            return msg;
        }

        //----< Construct reply message with some responce message >--------
        public string makeMessageReply(Message ipmsg,string response)
        {
            string command = this.parseRequestMessageForString(ipmsg, "COMMAND");            
            XElement root = new XElement("RESPONSE");
            XElement commandele = new XElement("COMMAND", command);
            XElement paramsele = new XElement("PARAMS");
            XElement resp = new XElement("RESULT", response);
            root.Add(commandele);
            root.Add(paramsele); paramsele.Add(resp);
            if (command=="QUERY")
            {
                string type = this.parseRequestMessageForString(ipmsg, "TYPE"); 
                XElement typele = new XElement("TYPE", type); paramsele.Add(typele);
            }                     
            return root.ToString();
        }

        //----< Construct a list from string delimited by some character>--------
        public List<string> makeList(string value,string delimiter=",")
        {
            List<string> list = new List<string>();
            string[] lines = Regex.Split(value, delimiter);

            foreach (string line in lines)
            {
                list.Add(line);
            }
            return list;
        }

        //----< Construct reply with some responce message >--------
        public string getMessageReply(Message rplymsg, out string type,out string thru)
        {
            string reply_text;
            type ="!exists"; thru = "!exists";
            XDocument docx = new XDocument();
            try {
                docx = XDocument.Parse(rplymsg.content);
                string command = this.parseRequestMessageForString(rplymsg, "COMMAND");               
                reply_text = command + ":";                
                XElement paramsele = docx.Root.Element("PARAMS");
                if (paramsele != null) {
                    XElement rply = paramsele.Element("RESULT");
                    if (command == "QUERY"){
                        type = this.parseRequestMessageForString(rplymsg, "TYPE");
                        if (type == "KEY_S_E")
                            reply_text += "wait";
                        else { reply_text += this.parseRequestMessageForString(rplymsg, "RESULT"); }
                    }                        
                    else      {
                        if (command == "P_TIME")
                        { thru= this.parseRequestMessageForString(rplymsg, "RESULT"); reply_text += thru; type = command; }
                        else reply_text += this.parseRequestMessageForString(rplymsg, "RESULT");
                    }
                }
                else {
                    reply_text = "No Reply Content";
                }
            }
            catch(Exception E)  { string exception = E.ToString();
                reply_text = "No Reply Message";
            }
           return reply_text;
        }

        //----< Construct edit message back  >--------
        public void getEditSearch(Message msg, out string name, out string description, out string children, out string payload)
        {
            string reply_text; name = "NA"; description = "NA"; children = "NA"; payload = "NA";
            XDocument docx = new XDocument();
            try
            {
                docx = XDocument.Parse(msg.content);
                XElement paramsele = docx.Root.Element("PARAMS");
                if (paramsele != null)
                {
                    XElement rply = paramsele.Element("RESULT");
                    reply_text = this.parseRequestMessageForString(msg, "RESULT");
                    List<string> list = makeList(reply_text, "\n"); reply_text = "";
                    name = this.getsubstring(list[1]);//                        list[1].Substring(list[1].LastIndexOf(':') + 2);
                    description = this.getsubstring(list[2]);//list[2].Substring(list[2].LastIndexOf(':') + 2);
                    children = this.getsubstring(list[4]);//list[4].Substring(list[4].LastIndexOf(':') + 2);
                    payload = list[6];
                }
            }
            catch (Exception E) { string exception = E.ToString(); }
        }

        //----< to get substring >--------
        private string getsubstring(string value)
        {
            return value.Substring(value.LastIndexOf(':') + 2);
        }

        //----< Construct list if XEelement with input string message add it to root >--------
        public void constructListXEle(List<string> list,string name,ref XElement root)
        {
            foreach(string value in list)
            {
                XElement ele = new XElement(name, value);
                root.Add(ele);
            }
        }

        //----< Construct list if XEelement with input string message and return it >--------
        public List<string> constructListfromXEle(XElement root, string name)
        {
            List<string> list = new List<string>();
            IEnumerable<XElement> values = root.Elements(name);
            foreach (XElement value in values)
                list.Add(value.Value.ToString());
            return list;
        }

        //----< Construct insert message  >--------
        public string constructInsertContext(string name,string desp,List<string> children,List<string> payload)
        {            
            XDocument xml = new XDocument();
            XElement root = new XElement("REQUEST");
            XElement command = new XElement("COMMAND","INSERT");
            XElement parameters = new XElement("PARAMS");
            XElement nameele = new XElement("NAME",name);
            XElement despele = new XElement("DESP",desp);
            parameters.Add(nameele);
            parameters.Add(despele);
            this.constructListXEle(children, "CHILDREN",ref parameters);
            this.constructListXEle(payload, "PAYLOAD", ref parameters);
            root.Add(command, parameters);
            xml.Add(root);
            return xml.ToString();
        }

        public string constructInsertasEdit(string key,string name, string desp, List<string> children, List<string> payload)
        {
            XDocument xml = new XDocument();
            XElement root = new XElement("REQUEST");
            XElement command = new XElement("COMMAND", "EDIT_I");
            XElement parameters = new XElement("PARAMS");
            XElement keyele = new XElement("KEY", key);
            XElement nameele = new XElement("NAME", name);
            XElement despele = new XElement("DESP", desp);
            parameters.Add(keyele);
            parameters.Add(nameele);
            parameters.Add(despele);
            this.constructListXEle(children, "CHILDREN", ref parameters);
            this.constructListXEle(payload, "PAYLOAD", ref parameters);
            root.Add(command, parameters);
            xml.Add(root);
            return xml.ToString();
        }

        //----< Construct delete message  >--------
        public string constructDeleteContext(string key)
        {
            XDocument xml = new XDocument();
            XElement root = new XElement("REQUEST");
            XElement command = new XElement("COMMAND", "DELETE");
            XElement parameters = new XElement("PARAMS");
            XElement keyele = new XElement("KEY", key);
            parameters.Add(keyele);
            root.Add(command, parameters);
            xml.Add(root);
            return xml.ToString();
        }

        //----< Construct edit message  >--------
        public string constructEditContext(string key,string type,string info)
        {
            XDocument xml = new XDocument();
            XElement root = new XElement("REQUEST");
            XElement command = new XElement("COMMAND", "EDIT");
            XElement parameters = new XElement("PARAMS");
            XElement keyele = new XElement("KEY", key);
            XElement typele = new XElement("TYPE", type);
            XElement infoele = new XElement("INFO", info);
            parameters.Add(keyele);            parameters.Add(typele);            parameters.Add(infoele);
            root.Add(command, parameters);
            xml.Add(root);
             return xml.ToString();
        }

        public string constructLog(string req, string command_t)
        {
            XDocument xml = new XDocument();
            XElement root = new XElement("REQUEST");
            XElement command = new XElement("COMMAND", "LOG");
            XElement parameters = new XElement("PARAMS");
            XElement keyele = new XElement("REQ", req);
            XElement typele = new XElement("COMM", command_t);            
            parameters.Add(keyele); parameters.Add(typele);
            root.Add(command, parameters);
            xml.Add(root);
            return xml.ToString();
        }
        //----< Construct persist message  >--------
        public string constructPersistContext(string name)
        {
            XDocument xml = new XDocument();
            XElement root = new XElement("REQUEST");
            XElement command = new XElement("COMMAND", "PERSIST");
            XElement parameters = new XElement("PARAMS");
            XElement keyele = new XElement("FILENAME", name);            
            parameters.Add(keyele); 
            root.Add(command, parameters);
            xml.Add(root);
            return xml.ToString();
        }

        //----< Construct restore message  >--------
        public string constructRestoreContext(string name)
        {
            XDocument xml = new XDocument();
            XElement root = new XElement("REQUEST");
            XElement command = new XElement("COMMAND", "RESTORE");
            XElement parameters = new XElement("PARAMS");
            XElement keyele = new XElement("FILENAME", name);
            parameters.Add(keyele);
            root.Add(command, parameters);
            xml.Add(root);
            return xml.ToString();
        }

        //----< Construct preformance request message  >--------
        public Message constructprefrequest(string fromUrl, string toUrl)
        {
            Message msg = new Message();
            msg.fromUrl = fromUrl;
            msg.toUrl = toUrl;
            XElement root=new XElement("REQUEST");
            XElement cmd = new XElement("COMMAND", "P_TIME");
            XElement parames = new XElement("PARAMS");
            root.Add(cmd); root.Add(parames);
            msg.content = root.ToString();//"<REQUEST><COMMAND>P_TIME</COMMAND></REQUEST>";
            return msg;
        }

        //----< Construct save/restore message  >--------
        public string constructSaveRestoreContext(string type,string info1)
        {
            XDocument xml = new XDocument();
            XElement root = new XElement("REQUEST");
            XElement command = new XElement("COMMAND", "SAVE_RESTORE");
            XElement parameters = new XElement("PARAMS");
            XElement typeele = new XElement("TYPE", type);
            XElement infoele1 = new XElement("INFO1", info1);                         
            parameters.Add(typeele); parameters.Add(infoele1); 
            root.Add(command, parameters);
            xml.Add(root);
            return xml.ToString();
        }
        public string register()
        {
            XDocument xml = new XDocument();
            XElement root = new XElement("REQUEST");
            XElement command = new XElement("COMMAND", "REGISTER");
            XElement parameters = new XElement("PARAMS");            
            root.Add(command, parameters);
            xml.Add(root);
            return xml.ToString();
        }
        public string notify(string url)
        {
            XDocument xml = new XDocument();
            XElement root = new XElement("REQUEST");
            XElement command = new XElement("COMMAND", "NOTIFY");
            XElement parameters = new XElement("PARAMS");
            XElement typeele = new XElement("TYPE", url);
            XElement res = new XElement("RESULT", "NOTIFICATION");
            root.Add(command, parameters); parameters.Add(typeele); parameters.Add(res);
            xml.Add(root);
            return xml.ToString();
        }

        //----< Construct Query message  >--------
        public string constructQueryContext(string type, string info1, string info2 = "nothing")
        {
            XDocument xml = new XDocument();
            XElement root = new XElement("REQUEST");
            XElement command = new XElement("COMMAND", "QUERY");
            XElement parameters = new XElement("PARAMS");
            XElement typeele = new XElement("TYPE", type);
            XElement infoele1 = new XElement("INFO1", info1);
            if (info2 != "nothing")
            {
                XElement infoele2 = new XElement("INFO2", info2);
                parameters.Add(infoele2);
            }
            parameters.Add(typeele); parameters.Add(infoele1);
            root.Add(command, parameters);
            xml.Add(root);
            return xml.ToString();
        }

        //----< parse value using tag >--------
        public string parseRequestMessageForString(Message msg, string reqInfo) //COMMAND NAME DESP KEY
        {
            String info = "!exists";
            XElement content = XElement.Parse(msg.content);
            XElement cmd = content.Element("COMMAND");
            XElement paramets = content.Element("PARAMS");            
                XElement name = paramets.Element("NAME");
                XElement desp = paramets.Element("DESP");
                XElement key = paramets.Element("KEY");
                XElement type = paramets.Element("TYPE");
                XElement infoe = paramets.Element("INFO");
                XElement infoe1 = paramets.Element("INFO1");
                XElement infoe2 = paramets.Element("INFO2");
                XElement result = paramets.Element("RESULT");
                XElement req = paramets.Element("REQ");
                XElement com2 = paramets.Element("COMM");
            if (reqInfo == "COMMAND" && cmd != null)
                info = cmd.Value.ToString();
            else if (reqInfo == "NAME" && name != null)
                info = name.Value.ToString();
            else if (reqInfo == "REQ" && req != null)
                info = req.Value.ToString();
            else if (reqInfo == "COMM" && com2 != null)
                info = com2.Value.ToString();
            else if (reqInfo == "DESP" && desp != null)
                info = name.Value.ToString();
            else if (reqInfo == "KEY" && key != null)
            { info = key.Value.ToString(); }
            else if (reqInfo == "TYPE" && type != null)
            { info = type.Value.ToString(); }
            else if (reqInfo == "INFO" && infoe != null)
            { info = infoe.Value.ToString(); }
            else if (reqInfo == "INFO1" && infoe1 != null)
            { info = infoe1.Value.ToString(); }
            else if (reqInfo == "INFO2" && infoe2 != null)
            { info = infoe2.Value.ToString(); }
            else if (reqInfo == "RESULT" && result != null)
            { info = result.Value.ToString(); }
            if (info == "!exists") Console.WriteLine("Wrong parse string {0}w", reqInfo);
            return info;
        }

        //----< Save latency of different clients  >--------
        public void savePref(string filename,string preformance)
        {
            XDocument xdoc = new XDocument();
            try {
                xdoc = XDocument.Load(filename);
                XElement pref = xdoc.Root;
                if (pref != null)                {
                    XElement preformancele = new XElement("PREF", preformance);
                    pref.Add(preformancele);
                }
                else                {
                    XElement newpref = new XElement("LATENCY");
                    XElement preformancele = new XElement("PREF", preformance);
                    newpref.Add(preformancele);
                    xdoc.Add(newpref);
                }                
            }
            catch(Exception E)  {
                string exception = E.ToString();
                XElement newpref = new XElement("LATENCY");
                XElement preformancele = new XElement("PREF", preformance);
                newpref.Add(preformancele);
                xdoc.Add(newpref);                
            }int count = 0;
            while (count<=100)            {
                try { xdoc.Save(filename); break; }
                catch (Exception e) { string exception = e.ToString(); }
                Thread.Sleep(count*100); count++;//Console.WriteLine("Exception :{0}", "sss");
            }
        }

        //----< clear file latency used by different clients  >--------
        public void saveClear(string filename)
        {
            XDocument xdoc = new XDocument();
            try
            {
                xdoc = XDocument.Load(filename);
                XDocument xdoc1 = new XDocument();
                XElement newpref = new XElement("LATENCY");
                xdoc1.Add(newpref);
                xdoc1.Save(filename);
            }
            catch (Exception E)
            {   string exception = E.ToString();
                XDocument xdoc1 = new XDocument();
                XElement newpref = new XElement("LATENCY");
                xdoc1.Add(newpref);
                xdoc1.Save(filename);
            }
        }

        //----< parse average latency from file  >--------
        public string getLat(string filename)
        {
            XDocument xdoc = new XDocument();
            int sum = 0, count = 0;
            try
            {
                xdoc = XDocument.Load(filename);
                XElement pref = xdoc.Root;
                if (pref != null)
                {                    
                    IEnumerable<XElement> latvalues = pref.Elements("PREF");
                    foreach(var prefv in latvalues)
                    {
                        string str = prefv.Value.ToString();
                        int latlocal = 0;
                        int.TryParse(str, out latlocal);
                        sum += latlocal;
                        count++;
                    }
                    if (count != 0 && sum != 0)
                        return (sum / count).ToString();
                    else
                        return "Need Read/Write Client Result";
                }
                else
                {
                    return "Test not perfrmed";
                }                               
            }
            catch (Exception E)
            {                string exception = E.ToString();
                return "Test not perfrmed";
            }
        }


#if (TEST_MESSAGEMAKER)
        //----< Test stub for MakeMessage  >--------
        static void Main(string[] args)
        {
            MessageMaker mm = new MessageMaker();
            Message msg = mm.makeMessage("fromFoo", "toBar");
            Utilities.showMessage(msg);
            Console.Write("\n\n");
            XDocument write = new XDocument();
            XElement root = new XElement("OPERATIONS");
            root.Add(mm.constructInsertContext("NEW ELEMENT1", "NEW ELEMENT1 DESP", new List<string> { }, new List<string> { "CONTENT11", "CONTENT12" }));
            root.Add(mm.constructInsertContext("NEW ELEMENT2", "NEW ELEMENT2 DESP", new List<string> { }, new List<string> { "CONTENT21", "CONTENT22" }));
            root.Add(mm.constructDeleteContext("NEW ELEMENT1"));
            root.Add(mm.constructEditContext("NEW ELEMENT2", "NAME", "NEW ELEMENT2 EDIT"));
            write.Add(root);
            write.Save("write.xml");
            XDocument read = new XDocument();
            XElement root1 = new XElement("OPERATIONS");
            root1.Add(mm.constructQueryContext("KEY_S", "KEY123"));
            root1.Add(mm.constructQueryContext("KEY_S", "KEY124"));
            root1.Add(mm.constructQueryContext("CHILD_S", "KEY123"));
            root1.Add(mm.constructQueryContext("PATTERN_S", "KEY*"));
            root1.Add(mm.constructQueryContext("METADATA_S", "META*"));
            root1.Add(mm.constructQueryContext("TIME_S", "START","END"));
            read.Add(root1);
            read.Save("read.xml");
        }
#endif
  }
}
