using Godot;
using System;

public partial class PlayZone : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        GetNode<Area2D>("PlayZoneArea").AreaEntered += PlayZone_AreaEntered;
        GetNode<Area2D>("PlayZoneArea").AreaExited += PlayZone_AreaExited;
        // GameManager.Instance.GetBus().Subscribe<PlayZoneEnteredEvent>(OnPlayZoneEnteredEventHandler);
	}

    private void PlayZone_AreaExited(Area2D otherArea)
    {
        if (otherArea == null)
            return;

        if(otherArea.GetParent() is Card)
        {
            Card card = (Card)otherArea.GetParent();
            //GD.Print($"Card {card.GetCardName()} has left the play zone.");
        }
    }

    private void PlayZone_AreaEntered(Area2D otherArea)
    {
        GD.Print("Play Zone Entered");
        if (otherArea == null)
            return;

        if(otherArea.GetParent() is Card)
        {
            Card card = (Card)otherArea.GetParent();
            //This should be ran through the event bus
            //card.PlayCard();
            
            GameManager.Instance.GetBus().Publish(new PlayZoneEnteredEvent{ PlayedCard = card});
            //GD.Print($"Play zone entered, call teh GameManager and let everyone else know Card ID {card.GetCardID}");
        }
        //Need to detect if this is a card or not.
        //GD.Print((otherArea.GetParent() is Card));
    }

}
