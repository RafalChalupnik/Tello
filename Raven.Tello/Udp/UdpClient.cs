using System;
using System.Net;
using System.Text;
using SystemUdpClient = System.Net.Sockets.UdpClient;

namespace Raven.Tello.Udp
{
  public class UdpClient : IDisposable
  {
    private static readonly Encoding _encoding = Encoding.ASCII;

    private readonly SystemUdpClient _udpClient;
    
    public UdpClient(string address, int port)
    {
      _udpClient = new SystemUdpClient(port);
      _udpClient.Connect(address, port);
    }

    public string Send(string command)
    {
      var commandBytes = _encoding.GetBytes(command);
      _udpClient.SendAsync(commandBytes, commandBytes.Length);

      var ipEndpoint = new IPEndPoint(IPAddress.Any, 0);
      var receivedBytes = _udpClient.Receive(ref ipEndpoint);

      return _encoding.GetString(receivedBytes);
    }

    public void Dispose()
    {
      _udpClient.Dispose();
    }
  }
}
