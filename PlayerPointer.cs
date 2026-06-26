using Godot;
using System;

public partial class PlayerPointer : Node2D
{

	private RayCast2D cast;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cast = new RayCast2D();
		AddChild(cast);
		cast.Enabled = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        // Get mouse position in world coordinates
        Vector2 mousePos = GetGlobalMousePosition();

        GlobalPosition = mousePos;

        // Get this node's position
        Vector2 nodePos = GlobalPosition;

        // Calculate direction from node to mouse
        Vector2 direction = (mousePos - nodePos).Normalized();

        // Set raycast target position (distance of 500)
        cast.TargetPosition = direction * 500;

        // Check collision
        if (cast.IsColliding())
        {
            Node collider = cast.GetCollider() as Node;
            Vector2 hitPoint = cast.GetCollisionPoint();

            //GD.Print($"Hit: {collider.Name} at {hitPoint}");

            // Draw a line to the hit point (for visualization)
            //QueueRedraw();
        }

    }
}
