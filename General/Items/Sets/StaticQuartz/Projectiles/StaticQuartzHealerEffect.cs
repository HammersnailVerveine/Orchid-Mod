using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.General.Items.Sets.StaticQuartz.Projectiles
{
	public class StaticQuartzHealerEffect : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Static Quartz Scythe");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = -1;
			projectile.tileCollide = false;
			projectile.ownerHitCheck = true;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 24;
		}

		public static Vector2 RotateVector(Vector2 origin, Vector2 vecToRot, float rot)
		{
			float newPosX = (float)(Math.Cos(rot) * (vecToRot.X - origin.X) - Math.Sin(rot) * (vecToRot.Y - origin.Y) + origin.X);
			float newPosY = (float)(Math.Sin(rot) * (vecToRot.X - origin.X) + Math.Cos(rot) * (vecToRot.Y - origin.Y) + origin.Y);
			return new Vector2(newPosX, newPosY);
		}

		public Vector2 rotVec = new Vector2(0, 56);
		public float rot = 0f;

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			if (player.direction > 0)
			{
				rot += 0.20f;
			}
			else
			{
				rot -= 0.20f;
			}

			projectile.Center = player.Center + RotateVector(default(Vector2), rotVec, rot + (projectile.ai[0] * (6.28f / 2)));

			for (int num363 = 0; num363 < 3; num363++)
			{
				float num364 = projectile.velocity.X / 3f * (float)num363;
				float num365 = projectile.velocity.Y / 3f * (float)num363;
				int num366 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 60, 0f, 0f, 0, default(Color), 1.2f);
				Main.dust[num366].position.X = projectile.Center.X - num364;
				Main.dust[num366].position.Y = projectile.Center.Y - num365;
				Main.dust[num366].velocity *= 0f;
				Main.dust[num366].noGravity = true;
				Main.dust[num366].scale = 1.2f;
			}
		}
	}
}
