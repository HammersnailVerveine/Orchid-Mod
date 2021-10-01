using Microsoft.Xna.Framework;
using System;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Air
{
	public class KeystoneOfTheConventProj : OrchidModAlchemistProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Convent Bolt");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 2;
			projectile.height = 2;
			projectile.friendly = false;
			projectile.scale = 1f;
			projectile.alpha = 0;
			projectile.aiStyle = 0;
			projectile.timeLeft = 10;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
		}

		public override void AI()
		{
			if (Main.rand.Next(2) == 0)
			{
				float x = projectile.position.X - projectile.velocity.X / 10f;
				float y = projectile.position.Y - projectile.velocity.Y / 10f;
				int rand = Main.rand.Next(2) == 0 ? 21 : 70;
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, rand, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 100, default(Color), 3.5f);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].velocity *= 0.5f;
				Main.dust[dust].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
		}
	}
}