using Sandbox;
using System;
using System.Linq;

partial class DeathmatchPlayer : BasePlayer
{
	TimeSince timeSinceDropped;

	public DeathmatchPlayer()
	{
		Inventory = new BaseInventory( this );
		EnableClientsideAnimation = true;
	}

	public override void Respawn()
	{
		SetModel( "models/citizen/citizen.vmdl" );

		Controller = new WalkController();
		Animator = new StandardPlayerAnimator();
		Camera = new FirstPersonCamera();
		  
		EnableAllCollisions = true; 
		EnableDrawing = true; 
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
		EnableClientsideAnimation = true;

		Dress();

		Inventory.Add( new Gun(), true );

		base.Respawn();
	}
	public override void OnKilled()
	{
		base.OnKilled();

		//
		Inventory.DropActive();

		//
		// Delete any items we didn't drop
		//
		Inventory.DeleteContents();

		BecomeRagdollOnClient();

		// TODO - clear decals

		Controller = null;
		Camera = new SpectateRagdollCamera();

		EnableAllCollisions = false;
		EnableDrawing = false;
	}


	protected override void Tick()
	{
		base.Tick();

		if ( Input.Pressed( InputButton.Slot1 ) ) Inventory.SetActiveSlot( 0, true );
		if ( Input.Pressed( InputButton.Slot2 ) ) Inventory.SetActiveSlot( 1, true );
		if ( Input.Pressed( InputButton.Slot3 ) ) Inventory.SetActiveSlot( 2, true );
		if ( Input.Pressed( InputButton.Slot4 ) ) Inventory.SetActiveSlot( 3, true );
		if ( Input.Pressed( InputButton.Slot5 ) ) Inventory.SetActiveSlot( 4, true );
		if ( Input.Pressed( InputButton.Slot6 ) ) Inventory.SetActiveSlot( 5, true );

		if ( Input.MouseWheel != 0 ) Inventory.SwitchActiveSlot( Input.MouseWheel, true );

		if ( LifeState != LifeState.Alive )
			return;

		if ( Input.Pressed( InputButton.View ) )
		{
			if ( Camera is ThirdPersonCamera )
			{
				Camera = new FirstPersonCamera();
			}
			else
			{
				Camera = new ThirdPersonCamera();
			}
		}

		if ( Input.Pressed( InputButton.Drop ) )
		{
			var dropped = Inventory.DropActive();
			if ( dropped != null )
			{
				timeSinceDropped = 0;
			}
		}
	}


	public override void StartTouch( Entity other )
	{
		if ( IsClient ) return;
		if ( timeSinceDropped < 1 ) return;

		Inventory.Add( other, Inventory.Active == null );
	}
}
