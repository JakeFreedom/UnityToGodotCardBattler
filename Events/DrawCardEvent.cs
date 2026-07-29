using System.Data.Common;
using System.Dynamic;

public class DrawCardEvent : GameEvent
{
    public DrawCardEvent(CardData cardData, Card card) {EventCardData = cardData; EventCard = card;}

    public CardData EventCardData {get;private set;}
    public Card EventCard{get; private set;}

}