
public class SimulatedRelayController : IRelayController
{
    private readonly Dictionary<int, bool> _relayStates;

    public SimulatedRelayController(int channelCount)
    {
        if (channelCount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(channelCount),
                "Channel count must be greater than zero."
            );
        }

        _relayStates = new Dictionary<int, bool>();

        for (int channel = 1; channel <= channelCount; channel++)
        {
            _relayStates[channel] = false;
        }
    }

    public Task TurnOnAsync(int channel)
    {
        ValidateChannel(channel);

        _relayStates[channel] = true;

        Console.WriteLine($"Relay {channel}: ON");

        return Task.CompletedTask;
    }

    public Task TurnOffAsync(int channel)
    {
        ValidateChannel(channel);

        _relayStates[channel] = false;

        Console.WriteLine($"Relay {channel}: OFF");

        return Task.CompletedTask;
    }

    public Task<bool> GetStatusAsync(int channel)
    {
        ValidateChannel(channel);

        bool status = _relayStates[channel];

        return Task.FromResult(status);
    }

    private void ValidateChannel(int channel)
    {
        if (!_relayStates.ContainsKey(channel))
        {
            throw new ArgumentOutOfRangeException(
                nameof(channel),
                $"Relay channel {channel} does not exist."
            );
        }
    }
}
