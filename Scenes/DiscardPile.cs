using Godot;
using System;
using System.Collections.Generic;

public partial class DiscardPile : Node2D
{
    [Export]
    private PackedScene baseCard;


    //This will be very similar to the deck class
    List<CardData> discardedCards;
    //private CardData m_cardData;
    //private Card m_cardVisuals;
    public override void _Ready()
    {
        discardedCards = new List<CardData>();
        //Find the discard Mouse detector

        GetNode<Area2D>("DiscardMouseDetector").MouseEntered += DiscardPile_AreaEntered;
    }

    private void DiscardPile_AreaEntered()
    {
        GD.Print("Mouse Entered the Discard Pile");
    }

    public void Discard(CardData card, Card cardVisual)
    {
        if(discardedCards == null)
            discardedCards = new List<CardData>();

        //m_cardData = card; ;
        //m_cardVisuals = cardVisual;
       

        discardedCards.Add(card);
        //GD.Print(discardedCards.Count);
        //Draw Cards to Screen
        DrawCardsToScreen();
    }

    private void DrawCardsToScreen() {
        //GD.Print("Draw discard pile");
        ClearCardsFromZone();//Clear the zone first and redraw it

        //Get the zone position
        Node2D zone = GetNode<Node2D>("Zone");
        int loopIndex = 0;
        //Loop through discardedCards
        foreach(CardData c in discardedCards)
        {
            Card card = baseCard.Instantiate() as Card;
            card.GetNode<Area2D>("MouseHoverArea").ProcessMode = ProcessModeEnum.Disabled;//This will disable any card interaction while in the graveyard. <-- This will cause issues. We need to figure out how to do the raycast.
            GetNode<Node2D>("Zone").AddChild(card);
            card.GlobalPosition = new Vector2(zone.GlobalPosition.X + (loopIndex * 1.5f), zone.GlobalPosition.Y + (loopIndex * 1.5f));
            card.SetupCard(c, 0);
            loopIndex++;
        }
    }
    private void ClearCardsFromZone() {
        foreach(Node2D node in GetNode<Node2D>("Zone").GetChildren())
            node.CallDeferred("queue_free");
    }
}
