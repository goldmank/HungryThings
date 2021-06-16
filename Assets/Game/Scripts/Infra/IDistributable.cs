namespace Game.Scripts.Infra
{
    public interface IDistributable
    {
        object Value { get; }
        int Amount { get; }
    }
}