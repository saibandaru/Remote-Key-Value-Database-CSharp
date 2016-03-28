/////////////////////////////////////////////////////////////////////////
// CommService.cs - Implementation of WCF message-passing service      //
// ver 1.1                                                             //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Project #4    //
/////////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package defines CommServices channel for WCF communication
 *
 *  Public Interface
 *===================
 *
 *Message getMessage()	called by server, blocks caller while empty
 *
 *void sendMessage(Message msg)	called by clients, will only block briefly
 *
 */
/*
 * Maintenance History:
 * --------------------
 * ver 1.1 : 24 Oct 2015
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

namespace Project4Starter
{
  using SWTools;
  using Util = Utilities;
    ///////////////////////////////////////////////////////////////////////
    // CommServices channel for WCF communication
    //

    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerSession)]
  public class CommService : ICommService
  {
    // static rcvrQueue is shared by all instances of this class

    private static SWTools.BlockingQueue<Message> rcvrQueue = 
      new SWTools.BlockingQueue<Message>();

    //----< called by clients, will only block briefly >-----------------

    public void sendMessage(Message msg)
    {
      if(Util.verbose)
        Console.Write("\n  this is CommService.sendMessage");
      rcvrQueue.enQ(msg);
    }
    //----< called by server, blocks caller while empty >----------------
    /*
     * Note: this is NOT a service method - see interface definition
     */
    public Message getMessage()
    {
      if(Util.verbose)
        Console.Write("\n  this is CommService.getMessage");
      return rcvrQueue.deQ();
    }
  }
}
