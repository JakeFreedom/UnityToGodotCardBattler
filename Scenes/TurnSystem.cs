using Godot;
using System;
using System.Threading.Tasks;

public partial class TurnSystem : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//GameManager.OnCardPlayed += HandleOnCardPlayed;
		GameManager.Instance.GetBus().Subscribe<PlayerTurnEndEvent>(OnPlayerTurnEndEventHandler);
		GameManager.Instance.GetBus().Subscribe<BossTurnStartEvent>(OnBossTurnStartEventHandler);
	}


	private async void OnPlayerTurnEndEventHandler(PlayerTurnEndEvent e)
	{
		
		GD.Print("Player Turn End");
		await ToSignal(GetTree().CreateTimer(2.0f), SceneTreeTimer.SignalName.Timeout);//We'll figure something else out later
		GameManager.Instance.GetBus().Publish(new BossTurnStartEvent());
	}

	private void OnBossTurnStartEventHandler(BossTurnStartEvent e)
	{
		GD.Print("Boss turn");
		//This really doesn't need to be here.
	}

}
