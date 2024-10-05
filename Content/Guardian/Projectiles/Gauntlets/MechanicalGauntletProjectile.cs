using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Gauntlets
{
	public class MechanicalGauntletProjectile : OrchidModGuardianProjectile
	{
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void AltSetDefaults()
		{
			Projectile.width = 64;
			Projectile.height = 64;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 17;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			Player owner = Owner;
			Projectile.Center = owner.Center;
			Dust.NewDust(owner.position, owner.width, owner.height, DustID.Torch); 
		}
	}
}