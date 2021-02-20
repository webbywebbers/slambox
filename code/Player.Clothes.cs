﻿using Sandbox;
using System;
using System.Linq;

partial class DeathmatchPlayer
{
	ModelEntity pants;
	ModelEntity jacket;
	ModelEntity shoes;
	ModelEntity hat;

	bool dressed = false;

	/// <summary>
	/// Just gonna keep the citizens for now, but I want the players to be rust guys.
	/// </summary>
	public void Dress()
	{
		if ( dressed ) return;
		dressed = true;

		if ( Rand.Int( 0, 3 ) != 1 )
		{
			var model = Rand.FromArray( new[]
			{
				"models/citizen_clothes/trousers/trousers.jeans.vmdl",
				"models/citizen_clothes/dress/dress.kneelength.vmdl",
				"models/citizen/clothes/trousers_tracksuit.vmdl",
				"models/citizen_clothes/shoes/shorts.cargo.vmdl",
				"models/citizen_clothes/trousers/trousers.lab.vmdl"
			} );

			pants = new ModelEntity();
			pants.SetModel( model );
			pants.FollowEntity( this, true );
			pants.EnableShadowInFirstPerson = true;
			pants.EnableHideInFirstPerson = true;

			if ( model.Contains( "dress" ) )
				jacket = pants;
		}

		if ( Rand.Int( 0, 3 ) != 1 && jacket == null )
		{
			var model = Rand.FromArray( new[]
			{
				"models/citizen_clothes/jacket/labcoat.vmdl",
				"models/citizen_clothes/jacket/jacket.red.vmdl",
				"models/citizen_clothes/gloves/gloves_workgloves.vmdl"
			} );

			jacket = new ModelEntity();
			jacket.SetModel( model );
			jacket.FollowEntity( this, true );
			jacket.EnableShadowInFirstPerson = true;
			jacket.EnableHideInFirstPerson = true;
		}

		if ( Rand.Int( 0, 3 ) != 1 )
		{
			shoes = new ModelEntity();
			shoes.SetModel( "models/citizen_clothes/shoes/shoes.workboots.vmdl" );
			shoes.FollowEntity( this, true );
			shoes.EnableShadowInFirstPerson = true;
			shoes.EnableHideInFirstPerson = true;
		}

		if ( Rand.Int( 0, 3 ) != 1 )
		{
			var model = Rand.FromArray( new[]
			{
				"models/citizen_clothes/hat/hat_hardhat.vmdl",
				"models/citizen_clothes/hat/hat_woolly.vmdl",
				"models/citizen_clothes/hat/hat_securityhelmet.vmdl",
				"models/citizen_clothes/hair/hair_malestyle02.vmdl",
				"models/citizen_clothes/hair/hair_femalebun.black.vmdl"
			} );

			hat = new ModelEntity();
			hat.SetModel( model );
			hat.FollowEntity( this, true );
			hat.EnableShadowInFirstPerson = true;
			hat.EnableHideInFirstPerson = true;
		}
	}
}
