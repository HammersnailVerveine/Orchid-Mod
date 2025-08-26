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

		public override void SafeSetDefaults()
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
			Strong = true;
		}

		public override void AI()
		{
			Player owner = Owner;
			Projectile.Center = owner.Center;
			Dust dust = Dust.NewDustPerfect(owner.Center, DustID.Torch);
			dust.scale = Main.rand.NextFloat(0.5f, 1.2f);
			dust.velocity = Projectile.velocity * Main.rand.NextFloat(0.2f) + Main.rand.NextVector2Circular(1, 1);
			if (Main.rand.NextBool())
			{
				dust.noGravity = true;
				dust.scale += 2 + Main.rand.NextFloat(2f);
			}
			Dust.NewDustPerfect(Projectile.Center, DustID.TheDestroyer, Projectile.velocity, Scale: 0.08f * Projectile.timeLeft).noGravity = true;

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