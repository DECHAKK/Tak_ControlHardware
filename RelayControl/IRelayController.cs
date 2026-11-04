public interface IRelayController
{
    Task TurnOnAsync(int channel);
    Task TurnOffAsync(int channel);
    Task<bool> GetStatusAsync(int channel);
}