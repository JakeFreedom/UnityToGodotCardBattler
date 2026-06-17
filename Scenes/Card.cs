using Godot;
using System;

public partial class Card : Node2D
{
	[Export] CardData cardData;
	[Export] Label CardName;
	[Export] Label CardDescription;
	[Export] Sprite2D AbilityImage;
	// Called when the node enters the scene tree for the first time.

	private Vector2 originalScale;
	private Vector2 originalPosition;
	private bool canMouseDrag = false;
	private GameManager gameManager;
	public override void _Ready()
	{

        GetNode<Area2D>("MouseHoverArea").MouseEntered += Card_MouseEntered;
        GetNode<Area2D>("MouseHoverArea").MouseExited += Card_MouseExited;

		originalScale = GlobalScale;
		originalPosition = GlobalPosition;
		gameManager = GetParent().GetParent().GetParent().GetParent() as GameManager;
		//gameManager.IsPlayerDragginCard = false;
	}

    private void Card_MouseExited()
    {
		if(Name == gameManager.CardNameBeingDragged)
		{
			canMouseDrag = false;
			gameManager.CardNameBeingDragged = "";
			gameManager.IsPlayerDragginCard = false;
			ZIndex = 0;
			GlobalScale = originalScale;
			GlobalPosition = originalPosition;
		}
    }

    private void Card_MouseEntered()
    {
		if (!gameManager.IsPlayerDragginCard)
		{
			canMouseDrag = true;
			gameManager.IsPlayerDragginCard = true;
			gameManager.CardNameBeingDragged = Name;
			ZIndex = 10;
			GlobalScale = new Vector2(1.0f, 1.0f);
			GlobalPosition = new Vector2(GlobalPosition.X, GlobalPosition.Y + -40);
		}
    }

	private Vector2 GetMousePosition()
	{
		return GetViewport().GetMousePosition();
	}
	public void SetupCard(CardData cardData)
	{
		CardName.Text = cardData.CardName;
		CardDescription.Text = cardData.CardDescription;
		AbilityImage.Texture = cardData.AbilityImage;

		GetNode<Label>("CostBackGround/Cost").Text = cardData.ActivationCost.ToString();
	}
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if (Input.IsMouseButtonPressed(MouseButton.Left) && canMouseDrag && Name == gameManager.CardNameBeingDragged)
		{
			GlobalPosition = GetMousePosition();
			//gameManager.IsPlayerDragginCard = true;
		}
		else
		{
			GlobalPosition = originalPosition;
			GlobalScale = originalScale;
			//gameManager.IsPlayerDragginCard = false;
		}
}

    public override void _Input(InputEvent inputEvent)
    {
		////GD.Print(inputEvent);
		//if(inputEvent is InputEventMouseButton && canMouseDrag && inputEvent.IsActionPressed("Left_Mouse_Button") && inputEvent is InputEventMouseMotion)
  //      {
  //          GlobalPosition = GetMousePosition();
  //      }
  //      else
  //      {
  //          GlobalPosition = originalPosition;
  //      }
    }
}
