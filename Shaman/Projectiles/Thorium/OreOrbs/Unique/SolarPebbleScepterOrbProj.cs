using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Unique
{
	public class SolarPebbleScepterOrbProj : OrchidModShamanProjectile
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
			projectile.timeLeft = 50;
			projectile.scale = 1f;
			projectile.extraUpdates = 1;
			projectile.alpha = 255;
			aiType = ProjectileID.Bullet;
			projectile.penetrate = 3;
		}

		public override void AI()
		{
			if (projectile.timeLeft == 50)
			{
				spawnDustCircle(6, 20 + Main.rand.Next(20));
				projectile.timeLeft -= Main.rand.Next(20);
			}

			int dust = Dust.NewDust(projectile.Center, 1, 1, 6);
			Main.dust[dust].velocity = projectile.velocity;
			Main.dust[dust].scale = 0.8f + ((projectile.timeLeft) / 45f) * 1.8f;
			Main.dust[dust].noGravity = true;
		}

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			for (int i = 0; i < 10; i++)
			{
				double dustDeg = (i * (36)) + 5 - Main.rand.Next(10);
				double dustRad = dustDeg * (Math.PI / 180);

				float posX = projectile.Center.X - (int)(Math.Cos(dustRad) * distToCenter) - projectile.width / 4;
				float posY = projectile.Center.Y - (int)(Math.Sin(dustRad) * distToCenter) - projectile.height / 4;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index1 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index1].velocity = projectile.velocity / 2;
				Main.dust[index1].fadeIn = 1f;
				Main.dust[index1].scale = 0.8f + ((projectile.timeLeft) / 90f) * 1.3f; ;
				Main.dust[index1].noGravity = true;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				target.AddBuff((thoriumMod.BuffType("Melting")), 2 * 60);
			}
		}
	}
}