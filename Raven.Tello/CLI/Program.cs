using System;
using Raven.Tello.Udp;

namespace Raven.Tello.CLI
{
  public static class Program
  {
    public static void Main(string[] _)
    {
      var continueExecution = true;
      using var udpClient = new UdpClient(address: "192.168.10.1", port: 8889);

      while (continueExecution)
      {
        var command = Console.ReadLine();

        if (command.ToUpperInvariant().Contains("EXIT"))
        {
          continueExecution = false;
        }
        else
        {
          var response = udpClient.Send(command);
          Console.WriteLine($"Response received: '{response}'");
        }
      }
    }
  }
}
