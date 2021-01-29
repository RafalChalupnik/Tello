using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Raven.Tello.Udp
{
  public class UdpServer : IDisposable
  {
    private const int _bufferSize = 8 * 1024;

    private static readonly Encoding _encoding = Encoding.ASCII;

    private readonly Socket _socket;
    private readonly byte[] _buffer;
    private Func<string, string> _responseFunc;

    private EndPoint _endpoint;

    public UdpServer(string address, int port, Func<string, string> responseFunc)
    {
      _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
      _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, optionValue: true);
      _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));

      _buffer = new byte[_bufferSize];
      _endpoint = new IPEndPoint(IPAddress.Any, port: 0);

      _socket.BeginReceiveFrom(_buffer, offset: 0, _bufferSize, SocketFlags.None, ref _endpoint, OnReceive, _buffer);
    }

    public void Dispose()
    {
      _socket.Dispose();
    }

    private void OnReceive(IAsyncResult asyncResult)
    {
      var receivedBytes = _socket.EndReceiveFrom(asyncResult, ref _endpoint);
      _socket.BeginReceiveFrom(_buffer, 0, _bufferSize, SocketFlags.None, ref _endpoint, OnReceive, _bufferSize);

      var receivedMessage = _encoding.GetString(_buffer, 0, receivedBytes);
      var messageToSend = _responseFunc(receivedMessage);

      var bytesToSend = _encoding.GetBytes(messageToSend);
      _socket.Send(bytesToSend);

      OnRequest?.Invoke(this, new RequestEventArgs(receivedMessage, messageToSend));
    }

    public event OnRequestEventHandler OnRequest;

    public delegate void OnRequestEventHandler(object source, RequestEventArgs args);

    public class RequestEventArgs : EventArgs
    {
      public string MessageReceived { get; }

      public string ResponseSent { get; }

      public RequestEventArgs(string messageReceived, string responseSent)
      {
        MessageReceived = messageReceived;
        ResponseSent = responseSent;
      }
    }
  }
}