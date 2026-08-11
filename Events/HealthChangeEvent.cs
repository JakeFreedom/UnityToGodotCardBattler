public class HealthChangeEvent : GameEvent
{
    
    public HealthChangeEvent(int Amount, bool IsGain=false)
    {
        HealthChangeAmount = Amount;
        IsHealthGain = IsGain;
    }

    public int HealthChangeAmount{get; protected set;}
    public bool IsHealthGain{get; protected set;}
}