using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Projectiles.Gauntlets
{
	public class MechanicalGauntletProjectile : OrchidModGuardianProjectile
	{
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void AltSetDefaults()
		{
			Projectile.width = 64;
			Projectile.height = 64;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 22;
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

			if (!IsLocalOwner && Projectile.timeLeft == 22)
			{ // Lazy way of syncing the dash without making my own packet
				OrchidPlayer orchidPlayer = owner.GetModPlayer<OrchidPlayer>();
				orchidPlayer.ForcedVelocityVector = Projectile.velocity;
				orchidPlayer.ForcedVelocityTimer = 20;
				orchidPlayer.PlayerImmunity = 20;
				orchidPlayer.ForcedVelocityUpkeep = 0.3f;
			}
		}
	}
}