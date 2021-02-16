using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Steamworks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

partial class CrossbowBolt : ModelEntity, IPhysicsUpdate
{
	bool Stuck;
	TimeSince TimeSinceStuck;

	public override void Spawn()
	{
		base.Spawn();

		// TODO BOLT MODEL
		SetModel( "weapons/rust_pistol/rust_pistol.vmdl" );
	}


	public virtual void OnPostPhysicsStep( float dt )
	{
		//DebugOverlay.Box( 0.1f, WorldPos, -0.1f, 1.1f, Host.Color );

		if ( !IsServer )
			return;

		if ( Stuck )
		{
			if ( TimeSinceStuck > 5 )
				Delete();
			return;
		}

		float Speed = 100.0f;
		var velocity = Rot.Forward * Speed;

		var start = WorldPos;
		var end = start + velocity;

		var tr = Trace.Ray( start, end )
				.UseHitboxes()
				//.HitLayer( CollisionLayer.Water, !InWater )
				.Ignore( Owner )
				.Ignore( this )
				.Size( 1.0f )
				.Run();

		// DebugOverlay.Line( start, end, 10.0f );
		// DebugOverlay.Box( 10.0f, WorldPos, -1, 1, Color.Red );
		// DebugOverlay.Box( 10.0f, tr.EndPos, -1, 1, Color.Red );

		if ( tr.Hit )
		{
			// TODO: CLINK NOISE (unless flesh)

			// TODO: SPARKY PARTICLES (unless flesh)

			Stuck = true;
			TimeSinceStuck = 0;
			WorldPos = tr.EndPos + Rot.Forward * -1;

			if ( tr.Entity.IsValid() )
			{
				var damageInfo = DamageInfo.FromBullet( tr.EndPos, tr.Direction * 200, 60.0f )
													.UsingTraceResult( tr )
													.WithAttacker( Owner )
													.WithWeapon( this );

				tr.Entity.TakeDamage( damageInfo );
			}

			// TODO: Parent to bone so this will stick in the meaty heads
			Parent = tr.Entity;
			Owner = null;

			//
			// Surface impact effect
			//
			tr.Normal = Rot.Forward * -1;
			tr.Surface.DoBulletImpact( tr );
			velocity = default;

			//
			// BUG: without this the bolt stops short of the wall on the client.
			//		need to make interp finish itself off even though it's not getting new positions?
			//
			ResetInterpolation();

			// DebugOverlay.Box( 10.0f, WorldPos, -1, 1, Color.Red );
			// DebugOverlay.Box( 10.0f, tr.EndPos, -1, 1, Color.Yellow );
		}
		else
		{
			WorldPos = end;
		}

		
	}
}
