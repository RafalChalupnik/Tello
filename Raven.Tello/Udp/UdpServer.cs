//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;

//namespace Raven.Tello.Udp
//{
//  public class UdpServer
//  {
//    private readonly UdpClient _udpClient;
//    private readonly Func<string, string> _responseFunc;

//    public UdpServer(UdpClient udpClient, Func<string, string> responseFunc)
//    {
//      _udpClient = udpClient;
//      _responseFunc = responseFunc;

//      _udpClient.RequestReceived += OnReceive;
//    }

//    private void OnReceive(object sender, UdpClient.RequestEventArgs args)
//    {
//      var receivedMessage = args.MessageReceived;
//      var messageToSend = _responseFunc(receivedMessage);

//      _udpClient.Send(messageToSend);
//      RequestReceived?.Invoke(this, new ServerEventArgs(receivedMessage, messageToSend));
//    }

//    public event RequestReceivedEventHandler RequestReceived;

//    public delegate void RequestReceivedEventHandler(object source, ServerEventArgs args);

//    public class ServerEventArgs : EventArgs
//    {
//      public string MessageReceived { get; }

//      public string ResponseSent { get; }

//      public ServerEventArgs(string messageReceived, string responseSent)
//      {
//        MessageReceived = messageReceived;
//        ResponseSent = responseSent;
//      }
//    }
//  }
//}