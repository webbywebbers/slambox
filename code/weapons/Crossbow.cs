using Sandbox;


partial class Crossbow : BaseDmWeapon
{ 
	public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";

	public override float PrimaryRate => 1;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "weapons/rust_pistol/rust_pistol.vmdl" );
	}

	public override void AttackPrimary( Player owner )
	{
	//	base.AttackPrimary( owner );

		if ( IsServer )
		using ( Prediction.Off() )
		{
			var bolt = new CrossbowBolt();
			bolt.Pos = owner.EyePos;
			bolt.Rot = owner.EyeRot;
			bolt.Owner = owner;
			bolt.Velocity = Owner.EyeRot.Forward * 100;
		}
	}

	// TODO right click zoom toggle
}
