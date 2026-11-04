
using System;
using System.Threading.Tasks;

namespace RelayControl;

class Program
{
    static async Task Main()
    {
        IRelayController relay = new SimulatedRelayController(4);

        await relay.TurnOnAsync(1);

        bool status = await relay.GetStatusAsync(1);

        Console.WriteLine($"Relay 1 Status: {status}");

        await relay.TurnOffAsync(1);

        status = await relay.GetStatusAsync(1);

        Console.WriteLine($"Relay 1 Status: {status}");
    }
}
