using Godot;
using System;
using System.ComponentModel;

public partial class HealthBar2 : Control
{
	[Export] private int StartingHealth = 0;
	[Export] private int MaxHealth = 0;
	private int CurrentHealth = 0;
	private TextureProgressBar healthBarProgress;
	private Label CurrentHealthLabel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CurrentHealth = StartingHealth;
		CurrentHealthLabel = GetNode<Label>("CurrentHealth");
		CurrentHealthLabel.Text = $"{CurrentHealth}/{MaxHealth}";
		GameManager.Instance.GetBus().Subscribe<HealthChangeEvent>(OnHealthChangeEventHandler);
		healthBarProgress = GetNode<TextureProgressBar>("Sprite2D/TextureProgressBar");
		healthBarProgress.Value = (CurrentHealth/MaxHealth) * 100;
	}

	private void OnHealthChangeEventHandler(HealthChangeEvent e)
	{
		int healthDelta = 0;
		if(e.IsHealthGain)
			healthDelta = (CurrentHealth+e.HealthChangeAmount);
		else
			healthDelta = (CurrentHealth-e.HealthChangeAmount);

		if(healthDelta <= 0)
		{
			CurrentHealth = 0;
			GameManager.Instance.GetBus().Publish(new DeathEvent());//<--I think this event can check to see who's turn it is, then determine who's death event this is

		}
		else
		{
			CurrentHealth = Math.Clamp(healthDelta, 1, MaxHealth);
			GD.Print(CurrentHealth);
			CurrentHealthLabel.Text = $"{CurrentHealth}/{MaxHealth}";
			GD.Print(healthBarProgress.Value);
			int healthProgress = (CurrentHealth/MaxHealth);
			GD.Print(MaxHealth);
			healthBarProgress.Value = ((float)CurrentHealth/(float)MaxHealth) * 100;	
			GD.Print(healthBarProgress.Value);	
		}
	}

	public int GetCurrentHealth => CurrentHealth;
}
