using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

[ClassLibrary( "weapon_cockfingers" )]
partial class Gun : BaseWeapon
{ 
	public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "weapons/rust_pistol/rust_pistol.vmdl" );
	}

	/// <summary>
	/// Lets make primary attack semi automatic
	/// </summary>
	public override bool CanPrimaryAttack( Player owner )
	{
		if ( !owner.Input.Pressed( InputButton.Attack1 ) ) 
			return false;

		return base.CanPrimaryAttack( owner );
	}

	public override void Reload( Player owner )
	{
		base.Reload( owner );

		ViewModelEntity?.SetAnimParam( "reload", true );
	}

	public override void AttackPrimary( Player owner )
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		//
		// Tell the clients to play the shoot effects
		//
		ShootEffects();


		bool InWater = Physics.TestPointContents( owner.EyePos, CollisionLayer.Water );
		var forward = owner.EyeRot.Forward * (InWater ? 500 : 4000);

		//
		// ShootBullet is coded in a way where we can have bullets pass through shit
		// or bounce off shit, in which case it'll return multiple results
		//
		foreach ( var tr in TraceBullet( owner.EyePos, owner.EyePos + owner.EyeRot.Forward * 5000 ) )
		{
			tr.Surface.DoBulletImpact( tr );

			if ( !IsServer ) continue;
			if ( !tr.Entity.IsValid() ) continue;

			//
			// We turn predictiuon off for this, so aany exploding effects don't get culled etc
			//
			using ( Prediction.Off() )
			{
				var damage = DamageInfo.FromBullet( tr.EndPos, forward.Normal * 100, 15 )
					.UsingTraceResult( tr )
					.WithAttacker( owner )
					.WithWeapon( this );

				tr.Entity.TakeDamage( damage );
			}
		}
	}

	[Client]
	void ShootEffects()
	{
		Host.AssertClient();

		PlaySound( "rust_pistol.shoot" );
		Particles.Create( "particles/pistol_muzzleflash.vpcf", EffectEntity, "muzzle" );

		ViewModelEntity?.SetAnimParam( "fire", true );
	}

	public override void AttackSecondary( Player owner ) 
	{
		AttackPrimary( owner );
	}

}
