public class PlayerTurnEndEvent : GameEvent
{
    public PlayerTurnEndEvent() {}

    public Card CardPlayed{get;set;}
}