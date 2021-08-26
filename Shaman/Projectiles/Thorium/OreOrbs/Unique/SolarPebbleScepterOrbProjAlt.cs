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
			DisplayName.SetDefault("Ecliptic Flame");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = true;
			projectile.aiStyle = 1;
			projectile.timeLeft = 200;
			projectile.scale = 1f;
			projectile.extraUpdates = 2;
			projectile.alpha = 255;
			aiType = ProjectileID.Bullet;
			projectile.tileCollide = false;
			projectile.penetrate = 3;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(projectile.Center, 1, 1, 6);
			Main.dust[dust].velocity = projectile.velocity;
			Main.dust[dust].scale = 0.8f + ((projectile.timeLeft) / 135f) * 1.8f;
			Main.dust[dust].noGravity = true;

			if (projectile.localAI[0] == 0f)
			{
				AdjustMagnitude(ref projectile.velocity);
				projectile.localAI[0] = 1f;
			}
			Vector2 move = Vector2.Zero;
			float distance = 1000f;
			bool target = false;
			for (int k = 0; k < 200; k++)
			{
				if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy)
				{
					Vector2 newMove = Main.npc[k].Center - projectile.Center;
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
				projectile.velocity = (20 * projectile.velocity + move) / 10f;
				AdjustMagnitude(ref projectile.velocity);
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