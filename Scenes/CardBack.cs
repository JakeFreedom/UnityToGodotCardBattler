using Godot;
using System;

public partial class CardBack : Node2D
{
    [Signal]
    public delegate void DrawButtonClickEventHandler();
    public override void _Ready()
    {
		Button drawButton = GetNode<Button>("DrawButton");
        drawButton.Pressed += DrawButton_Pressed;
    }

    private void DrawButton_Pressed()
    {
        //GD.Print("Draw Button Pressed");
        //Signal to interested parties
        EmitSignal("DrawButtonClick");

    }
}
