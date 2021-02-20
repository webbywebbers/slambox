using Sandbox;

/// <summary>
/// This is the heart of the gamemode. It's responsible
/// for creating the player and stuff.
/// </summary>
[ClassLibrary( "slambox", Title = "Slambox" )]
partial class SlamboxGame : Game
{
	public SlamboxGame()
	{
		//<summary>
		// Create the HUD entity. This is always broadcast to all clients
		//<summary>
		if ( IsServer )
		{
			new SlamboxPlayer();
		}
	}

	/// <summary>
	/// Called when a player joins and wants a player entity. We create
	/// our own class so we can control what happens.
	/// </summary>
	public override Player CreatePlayer()
	{
		return new SlamboxPlayer();
	}
}
