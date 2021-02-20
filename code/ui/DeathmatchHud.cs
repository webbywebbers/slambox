//not gonna fuck with much of this till I understand sbox's scss stuff better
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Threading.Tasks;

[ClassLibrary]
public partial class DeathmatchHud : Hud
{
	public DeathmatchHud()
	{
		if ( !IsClient )
			return;

		RootPanel.StyleSheet = StyleSheet.FromFile( "/ui/DeathmatchHud.scss" );

		//	RootPanel.AddChild<ChatUI>();

		var healthPanel = RootPanel.Add.Panel( "health" ); 
		var icon = healthPanel.Add.Label( "🩸", "icon" );
		var health = healthPanel.Add.Label( "", "value" );
		health.Text = "75"; //ooh 75 health, better be weary

		//	health.BindToMethod( "text", () => Player.Local?.Health );

		RootPanel.AddChild<NameTags>();
		RootPanel.AddChild<CrosshairCanvas>();
		RootPanel.AddChild<InventoryBar>();

		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<KillFeed>();
		//GameFeed = RootPanel.Add.PanelWithClass( "gamefeed" );
	}

	[Client]
	public void OnPlayerDied( string victim, string attacker = null )
	{
		Host.AssertClient();
		
	}

	[Client]
	public void ShowDeathScreen( string attackerName )
	{
		Host.AssertClient();

		Log.Info( "garry death screen pls" );
	}
}
