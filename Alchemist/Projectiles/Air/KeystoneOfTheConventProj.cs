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
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.friendly = false;
			Projectile.scale = 1f;
			Projectile.alpha = 0;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 10;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			if (Main.rand.Next(2) == 0)
			{
				float x = Projectile.position.X - Projectile.velocity.X / 10f;
				float y = Projectile.position.Y - Projectile.velocity.Y / 10f;
				int rand = Main.rand.Next(2) == 0 ? 21 : 70;
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, rand, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 100, default(Color), 3.5f);
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