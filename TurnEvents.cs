using Godot;
using System;

public static partial class TurnEvents
{

	public static Action OnPlayerTurnEnd;

	#region Static Methods
	public static void PlayerTurnEnd()
	{
		OnPlayerTurnEnd?.Invoke();
	}
	#endregion 

	#region Public Properties
	
	#endregion
}
