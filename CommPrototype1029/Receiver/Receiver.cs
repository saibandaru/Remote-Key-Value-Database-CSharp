/*/////////////////////////////////////////////////////////////
// Receiver.cs - Used as the receiver component for Client   //
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
 * This package Used as the receiver component for Client and server blocks of Project 4 
 *
 *  Public Interface
 *===================
 *
 *ServiceHost CreateListener()		creates listener but does not start it
 *
 *bool StartService()			Create CommService and listener and start it
 *
 *Action defaultServiceAction()		serviceAction defines what happens to received messages
 *
 *void serverProcessMessage(Message msg)	one way to define Receiver functionality
 *
 *void doService(Action serviceAction)	run the service action
 *
 *void doService()			runs defaultServiceAction
 *
 *Message getMessage()			application hosting Receiver calls this method
 *
 *void shutDown()				send closeReceiver message to local Receiver
 *
 *void ProcessCommandLine(string[] args)	quick way to grab ports and addresses from commandline
 *
 */
/*
 * Maintenance History:
 * --------------------
 * ver 2.1 : 29 Oct 2015
 * - added comment prologue to shutDown()
 * ver 2.0 : 24 Oct 2015
 * - Provided mechanism to define new Receiver msg processing.
 *   See methods defaultServiceAction, serverProcessMessage, and doService.
 * - added verbose mode to support debugging and learning
 * - to see more detail about what is going on in Sender and Receiver
 *   set Utilities.verbose = true
 * ver 1.0 : 18 Oct 2015
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
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;

namespace Project4Starter
{
  using Util = Utilities;

  ///////////////////////////////////////////////////////////////////////
  // Receiver class
  // - provides a method, Message getMessage(), to retrieve messages
  //   sent to its instances
  // - has public string properties for listener address and port
  //
  public class Receiver
  {
    public string address { get; set; }
    public string port { get; set; }
    CommService svc = null;
    ServiceHost host = null;
    public Receiver(string Port = "8080", string Address = "localhost")
    {
      address = Address;
      port = Port;
    }

    //----< creates listener but does not start it >---------------------
    public ServiceHost CreateListener()
    {
      string url = "http://" + this.address + ":" + this.port + "/CommService";
      BasicHttpBinding binding = new BasicHttpBinding();
      Uri address = new Uri(url);
      Type service = typeof(CommService);
      ServiceHost host = new ServiceHost(service, address);
      host.AddServiceEndpoint(typeof(ICommService), binding, address);
      return host;
    }
    
    //----< Create CommService and listener and start it >---------------     
    public bool StartService()
    {
      if(Util.verbose)
        Console.Write("\n  Receiver starting service");
      try
      {
        host = CreateListener();
        host.Open();
        svc = new CommService();
      }
      catch (Exception ex)
      {
        Console.Write("\n\n --- creation of Receiver listener failed ---\n");
        Console.Write("\n    {0}", ex.Message);
        Console.Write("\n    exiting\n\n");
        return false;
      }
      return true;
    }

    //----< serviceAction defines what happens to received messages >----
    public Action defaultServiceAction()
    {
      Action serviceAction = () =>
      {
        if(Util.verbose)
          Console.Write("\n  starting Receiver.defaultServiceAction");
        Message msg = null;
        while (true)
        {
          msg = getMessage();   // note use of non-service method to deQ messages
          Console.Write("\n  Received message:");
          Console.Write("\n  sender is {0}", msg.fromUrl);
          Console.Write("\n  content is {0}\n", msg.content);
          serverProcessMessage(msg);
          if (msg.content == "closeReceiver")
            break;
        }
      };
      return serviceAction;
    }

    //----< one way to define Receiver functionality >-------------------
    public virtual void serverProcessMessage(Message msg)
    {
        //nothing defined
    }
    //----< run the service action >-------------------------------------
    public void doService(Action serviceAction)
    {
      ThreadStart ts = () =>
      {
        if(Util.verbose)
          Console.Write("\n  doService thread started");
        serviceAction.Invoke();  // usually has while loop that runs until closed
      };
      Thread t = new Thread(ts);
      t.IsBackground = true;
      t.Start();
    }
    
    //----< runs defaultServiceAction >----------------------------------
    public void doService()
    {
      doService(defaultServiceAction());
    }
    
    //----< application hosting Receiver calls this method >-------------
    public Message getMessage()
    {
      if(Util.verbose)
        Console.Write("\n  calling CommService.getMessage()");
      Message msg = svc.getMessage();
      if (Util.verbose)
        Console.Write("\n  returned from CommService.getMessage()");
      return msg;
    }
    
    //----< send closeReceiver message to local Receiver >---------------
    public void shutDown()
    {
      Console.Write("\n  local receiver shutting down");
      Message msg = new Message();
      msg.content = "closeReceiver";
      msg.toUrl = Util.makeUrl(address, port);
      msg.fromUrl = msg.toUrl;
      Util.showMessage(msg);
      svc.sendMessage(msg);
      host.Close();
    }
    
    //----< quick way to grab ports and addresses from commandline >-----
    public void ProcessCommandLine(string[] args)
    {
      if (args.Length > 0)
      {
        port = args[0];
      }
      if (args.Length > 1)
      {
        address = args[1];
      }
    }
    
    //----< Test Stub >--------------------------------------------------
#if (TEST_RECEIVER)

    static void Main(string[] args)
    {
      Util.verbose = true;

      Console.Title = "CommService Receiver";
      Console.Write("\n  Starting CommService Receiver");
      Console.Write("\n ===============================\n");

      Receiver rcvr = new Receiver();
      rcvr.ProcessCommandLine(args);

      Console.Write("\n  Receiver url = {0}\n", Util.makeUrl(rcvr.address, rcvr.port));

      // serviceAction defines what the server does with received messages

      if (rcvr.StartService())
      {
        //rcvr.doService();
        rcvr.doService(rcvr.defaultServiceAction());  // equivalent to rcvr.doService()
      }
      Console.Write("\n  press any key to exit: ");
      Console.ReadKey();
      Console.Write("\n\n");
    }
#endif
  }
}
