using Sandbox;
using Sandbox.UI;
using System.Collections.Generic;

public class InventoryBar : Panel
{
	List<InventoryIcon> slots = new();

	public InventoryBar()
	{
		for ( int i=0; i<9; i++ )
		{
			var icon = new InventoryIcon( i+1, this );
			slots.Add( icon );
		}
	}

	public override void Tick()
	{
		base.Tick();

		var player = Player.Local;
		if ( player == null ) return;

		for ( int i=0; i<slots.Count; i++ )
		{
			UpdateIcon( player.Inventory.GetSlot( i ), slots[i], i );
		}
	}

	public void UpdateIcon( Entity ent, InventoryIcon inventoryIcon, int i )
	{
		if ( ent == null )
		{
			inventoryIcon.Clear();
			return;
		}

		inventoryIcon.Label.Text = ent.ToString();
		inventoryIcon.SetClass( "active", ent.IsActiveChild() );
	}
}
