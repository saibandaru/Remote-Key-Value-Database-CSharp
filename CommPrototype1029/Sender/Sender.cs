/*/////////////////////////////////////////////////////////////
// Sender.cs - Used as the sender component for Client       //
//                  and server blocks in Project 4           //
//                                                           //
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
 * This package Used as the sender component for Client and server blocks of Project 4 
 *
 *  Public Interface
 *===================
 *
 *bool isConnected(string url)		is sender connected to the specified url
 *
 *bool Connect(string remoteUrl)		Connect repeatedly tries to send messages to service
 *
 *void sendMsgNotify(string msg)		overridable message annunciator
 *
 *void sendAttemptNotify(int attemptNumber)	overridable attemptHandler
 *
 *void CloseConnection()			close connection - not used in this demo
 *
 *void setAction(Action sendAct)		set send action
 *
 *void startSender()			send messages to remote Receivers
 *
 *bool sendMessage(Message msg)		 send a message to remote Receiver
 *
 *SWTools.BlockingQueue<Message> defineSendProcessing()	defines SendThread and its operations
 *
 *void sendExceptionNotify(Exception ex, string msg = "")	overridable exception annunciator
 *
 *void processCommandLine(string[] args)	sets urls from CommandLine if defined there
 *
 *void shutdown()				send closeSender message to local sender
 *
 */
/*
 * Maintenance History:
 * --------------------
 * ver 2.1 : 29 Oct 2015
 * - added statement to store proxy in connect
 * - moved proxyStore to data member instead of local variable
 * - renamed svc to proxy
 * - moved message creation out of connect attempt loop
 * - added overridable notifiers
 * - added shutDown()
 * ver 2.0 : 24 Oct 2015
 * - added sender queue and thread
 * - now, user just uses sendMessage(msg).  The sendThread examines
 *   msg destination and routes to the appropriate proxy, creating
 *   one if necessary.
 * - several helper functions added
 * - added verbose mode to support debugging and learning
 * - to see more detail about what is going on in Sender and Receiver
 *   set Utilities.verbose = true
 * ver 1.0 : 20 Oct 2015
 * - first release
 *
 * Build Process:  devenv CommPrototype.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for develope
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ServiceModel;

namespace Project4Starter
{
  using Util = Utilities;

    ///////////////////////////////////////////////////////////////////////
    // Sender class
    // - provides a method, Message sendMessage(), to send messages
    //   to remote Receivers
    // - has public string properties for both local and remote 
    //   addresses and ports
    // - also has property to set the max number of connection retries
    //   before quitting
    //
    public class ProxyAndDeadLetter
    {
        public ICommService svc { set; get; }
        public List<Message> dead_letter;
        public ProxyAndDeadLetter()
        {
            dead_letter = new List<Message>();
        }
    }
    public class Sender
  {
    public string localUrl { get; set; } = "http://localhost:8081/CommService";
    public string remoteUrl { get; set; } = "http://localhost:8080/CommService";
    public int MaxConnectAttempts { get; set; } = 2;

    ICommService proxy = null;
    SWTools.BlockingQueue<Message> sendQ = null;
    Dictionary<string, ProxyAndDeadLetter> proxyAndDeadStore = new Dictionary<string, ProxyAndDeadLetter>();
    Action sendAction = null;

    //----< define send thread processing and start thread >-------------
    public Sender(string LocalUrl="http://localhost:8081/CommServer")
    {
      localUrl = LocalUrl;
      sendQ = defineSendProcessing();
      startSender();
    }


        //----< Proxy implements the service interface >---------------------
        ICommService CreateProxy(string remoteUrl)
    {
      BasicHttpBinding binding = new BasicHttpBinding();
      EndpointAddress address = new EndpointAddress(remoteUrl);
      ChannelFactory<ICommService> factory = new ChannelFactory<ICommService>(binding, address);
      return factory.CreateChannel();
    }

    //----< is sender connected to the specified url? >------------------
    public bool isConnected(string url)
    {
      return proxyAndDeadStore.ContainsKey(url);
    }
       public static ProxyAndDeadLetter createProxyDead(ICommService svc,Message deadMsg=null)
        {
            ProxyAndDeadLetter proxMsg = new ProxyAndDeadLetter();
            if (deadMsg != null)
            {
                proxMsg.dead_letter.Add(deadMsg);
            }
            proxMsg.svc = svc;
            return proxMsg;
        }
        public void sendTheDead(ICommService svc,string address)
        {           
                foreach (Message msg in this.proxyAndDeadStore[address].dead_letter)
                {
                
                if (!msg.content.Equals("dontSend"))
                { svc.sendMessage(msg);  }
                }

        }

        //----< Connect repeatedly tries to send messages to service >-------
        public bool Connect(string remoteUrl)
    {
      if(Util.verbose)
        sendMsgNotify("attempting to connect");
      proxy = CreateProxy(remoteUrl);
      int attemptNumber = 0;
      Message startMsg = new Message();
      startMsg.fromUrl = localUrl;
      startMsg.toUrl = remoteUrl;
      startMsg.content = "connection start message";
      while (attemptNumber < MaxConnectAttempts)
      {
        try
        {
          proxy.sendMessage(startMsg);    // will throw if server isn't listening yet
          proxyAndDeadStore[remoteUrl] = createProxyDead(proxy);  // remember this proxy
                    sendMsgNotify("Connected to server "+remoteUrl);
          if (Util.verbose)
            sendMsgNotify("connected");
          return true;
        }
        catch
        {
          ++attemptNumber;
          sendAttemptNotify(attemptNumber);
          Thread.Sleep(100);
        }
      }
      return false;
    }

    //----< overridable message annunciator >----------------------------
    public virtual void sendMsgNotify(string msg)
    {
      Console.Write("\n  {0}\n", msg);
    }

    //----< overridable attemptHandler >---------------------------------
    public virtual void sendAttemptNotify(int attemptNumber)
    {
      Console.Write("\n  connection attempt #{0}", attemptNumber);
    }

    //----< close connection - not used in this demo >-------------------
    public void CloseConnection()
    {
      proxy = null;
    }

    //----< set send action >--------------------------------------------
    public void setAction(Action sendAct)
    {
      sendAction = sendAct;
    }

    //----< send messages to remote Receivers >--------------------------
    public void startSender()
    {
      sendAction.Invoke();
    }

    //----< send a message to remote Receiver >--------------------------
    public bool sendMessage(Message msg)
    {
      sendQ.enQ(msg);
      return true;
    }

    //----< defines SendThread and its operations >----------------------
    /*
     * - asynchronous function defines Sender sendThread processing
     * - creates BlockingQueue<Message> to use inside Sender.sendMessage()
     * - creates and starts a thread executing that processing
     * - uses msg.toUrl to find or create a proxy for url destination
     */
    public virtual SWTools.BlockingQueue<Message> defineSendProcessing()
    {
      SWTools.BlockingQueue<Message> sendQ = new SWTools.BlockingQueue<Message>();
      Action sendAction = () => {ThreadStart sendThreadProc = () => {
          while (true) {
              Message temp=new Message(); temp.content = "dontSend";
            try { Message smsg = sendQ.deQ(); temp = smsg;
                  if (proxyAndDeadStore.ContainsKey(smsg.toUrl)) {
                                   
                      proxyAndDeadStore[smsg.toUrl].svc.sendMessage(smsg);
                      sendTheDead(proxy, smsg.toUrl);
                      if (smsg.content== "connection start message")
                        sendMsgNotify(String.Format("Connected to Server {0}", smsg.toUrl));
               }
              else {                
                if (this.Connect(smsg.toUrl)) {                  
                  proxyAndDeadStore[smsg.toUrl] = createProxyDead(proxy); 
                  proxy.sendMessage(smsg);
                          sendTheDead(proxy, smsg.toUrl);
                      }
                else {
                  sendMsgNotify(String.Format("could not connect to {0} ",smsg.toUrl));
                  proxyAndDeadStore[remoteUrl] = createProxyDead(proxy, smsg);
                          continue;
                }
              }
            }
            catch(Exception ex) {
                  sendMsgNotify(String.Format("Request sending failed but \n captured"));
              //sendExceptionNotify(ex);                  
                  proxyAndDeadStore[temp.toUrl].dead_letter.Add(temp);
                  continue;
            }
          }
        };
        Thread t = new Thread(sendThreadProc);  // start the sendThread
        t.IsBackground = true;
        t.Start();
      };
      this.setAction(sendAction);
      return sendQ;
    }

    //----< overridable exception annunciator >--------------------------
    public virtual void sendExceptionNotify(Exception ex, string msg = "")
    {
      Console.Write("\n --- {0} ---\n", ex.Message);
    }

    //----< sets urls from CommandLine if defined there >----------------
    public void processCommandLine(string[] args)
    {
      if (args.Length > 0)
      {
        localUrl = Util.processCommandLineForLocal(args, localUrl);
        remoteUrl = Util.processCommandLineForRemote(args, remoteUrl);
      }
    }
    
    //----< send closeSender message to local sender >-------------------
    public void shutdown()
    {
      Message sdmsg = new Message();
      sdmsg.fromUrl = localUrl;
      sdmsg.toUrl = localUrl;
      sdmsg.content = "closeSender";
      Console.Write("\n  shutting down local sender");
      sendMessage(sdmsg);
    }

    //----< Test Stub >--------------------------------------------------
#if (TEST_SENDER)
    static void Main(string[] args)
    {
      Util.verbose = false;
      Console.Write("\n  starting CommService Sender");
      Console.Write("\n =============================\n");
      Console.Title = "CommService Sender";
      Sender sndr = new Sender("http://localhost:8081/CommService");
      sndr.processCommandLine(args);       
      int numMsgs = 5;
      int counter = 0;
      Message msg = null;
      while (true) {
        msg = new Message();
        msg.fromUrl = sndr.localUrl;
        msg.toUrl = sndr.remoteUrl;
        msg.content = "Message #" + (++counter).ToString();
        Console.Write("\n  sending {0}", msg.content);
        sndr.sendMessage(msg);
        Thread.Sleep(30);
        if (counter >= numMsgs)
          break;
      }
      msg = new Message();
      msg.fromUrl = sndr.localUrl;
      msg.toUrl = "http://localhost:9999/CommService";
      msg.content = "no listener for this message";
      Console.Write("\n  sending {0}", msg.content);
      sndr.sendMessage(msg);
      msg = new Message();
      msg.fromUrl = sndr.localUrl;
      msg.toUrl = sndr.remoteUrl;
      msg.content = "Message #" + (++counter).ToString();
      Console.Write("\n  sending {0}", msg.content);
      sndr.sendMessage(msg);
      msg = new Message();
      msg.fromUrl = sndr.localUrl;
      msg.toUrl = sndr.remoteUrl;
      msg.content = "closeSender";  // message for self and Receiver
      Console.Write("\n  sending {0}", msg.content);
      sndr.sendMessage(msg);
    }
#endif
  }
}
