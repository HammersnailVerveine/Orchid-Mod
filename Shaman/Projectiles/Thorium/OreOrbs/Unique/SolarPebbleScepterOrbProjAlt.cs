using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Unique
{
	public class SolarPebbleScepterOrbProjAlt : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ecliptic Flame");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 200;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 2;
			Projectile.alpha = 255;
			AIType = ProjectileID.Bullet;
			Projectile.tileCollide = false;
			Projectile.penetrate = 3;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(Projectile.Center, 1, 1, 6);
			Main.dust[dust].velocity = Projectile.velocity;
			Main.dust[dust].scale = 0.8f + ((Projectile.timeLeft) / 135f) * 1.8f;
			Main.dust[dust].noGravity = true;

			if (Projectile.localAI[0] == 0f)
			{
				AdjustMagnitude(ref Projectile.velocity);
				Projectile.localAI[0] = 1f;
			}
			Vector2 move = Vector2.Zero;
			float distance = 1000f;
			bool target = false;
			for (int k = 0; k < 200; k++)
			{
				if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy)
				{
					Vector2 newMove = Main.npc[k].Center - Projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo < distance)
					{
						move = newMove;
						distance = distanceTo;
						target = true;
					}
				}
			}
			if (target)
			{
				AdjustMagnitude(ref move);
				Projectile.velocity = (20 * Projectile.velocity + move) / 10f;
				AdjustMagnitude(ref Projectile.velocity);
			}
		}

		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 6f)
			{
				vector *= 6f / magnitude;
			}
		}
	}
}