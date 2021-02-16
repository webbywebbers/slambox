using Sandbox;
using System;
using System.Linq;

partial class DeathmatchPlayer
{
	// TODO - make ragdolls one per entity
	// TODO - make ragdolls dissapear after a load of seconds
	static EntityLimit RagdollLimit = new EntityLimit { MaxTotal = 20 };

	[Client]
	void BecomeRagdollOnClient()
	{
		// TODO - lets not make everyone write this shit out all the time
		// maybe a CreateRagdoll<T>() on ModelEntity?
		var ent = new ModelEntity();
		ent.Pos = Pos;
		ent.Rot = Rot;
		ent.MoveType = MoveType.Physics;
		ent.UsePhysicsCollision = true;
		ent.EnableAllCollisions = true;
		ent.CollisionGroup = CollisionGroup.Debris;
		ent.SetModel( GetModelName() );
		ent.CopyBonesFrom( this );
		ent.SetRagdollVelocityFrom( this );

		// Copy the clothes over
		foreach ( var child in Children )
		{
			if ( child is ModelEntity e )
			{
				var model = e.GetModelName();
				if ( model != null && !model.Contains( "clothes" ) ) // Uck we 're better than this, entity tags, entity type or something?
					continue;

				var clothing = new ModelEntity();
				clothing.SetModel( model );
				clothing.FollowEntity( ent, true );
			}
		}

		Corpse = ent;

		RagdollLimit.Watch( ent );
	}
}
