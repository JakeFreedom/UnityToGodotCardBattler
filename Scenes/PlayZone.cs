using Godot;
using System;

public partial class PlayZone : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        GetNode<Area2D>("PlayZoneArea").AreaEntered += PlayZone_AreaEntered;
        GetNode<Area2D>("PlayZoneArea").AreaExited += PlayZone_AreaExited;
	}

    private void PlayZone_AreaExited(Area2D otherArea)
    {
        if (otherArea == null)
            return;

        if(otherArea.GetParent() is Card)
        {
            Card card = (Card)otherArea.GetParent();
            GD.Print($"Card {card.GetCardName()} has left the play zone.");
        }
    }

    private void PlayZone_AreaEntered(Area2D otherArea)
    {
        if (otherArea == null)
            return;

        if(otherArea.GetParent() is Card)
        {
            Card card = (Card)otherArea.GetParent();
            card.PlayCard();
            //Card detected.
            //GD.Print(card.Name + " " + card.GetCardName());
        }
        //Need to detect if this is a card or not.
        //GD.Print((otherArea.GetParent() is Card));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
