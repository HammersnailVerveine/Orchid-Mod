using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace OrchidMod.Alchemist.Projectiles.Nature
{
	public class LivingSapVialProj : OrchidModAlchemistProjectile
	{
		public int heal = 0;

		public override void SafeSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 600;
			Projectile.scale = 1f;
			Projectile.alpha = 128;
			Projectile.penetrate = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sap Bubble");
		}

		public override void AI()
		{
			Projectile.velocity *= 0.95f; ;
			Projectile.rotation += 0.02f;

			if (Projectile.damage != 0)
			{
				this.heal += Projectile.damage;
				Projectile.damage = 0;
			}

			if (Main.rand.Next(20) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 102);
				Main.dust[dust].velocity *= 0.1f;
				Main.dust[dust].scale *= 1f;
			}


			//Player player = Main.player[projectile.owner];
			Player player = Main.player[Main.myPlayer]; // < TEST MP
			Vector2 center = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
			float offsetX = player.Center.X - center.X;
			float offsetY = player.Center.Y - center.Y;
			float distance = (float)Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
			if (distance < 50f && Projectile.position.X < player.position.X + player.width && Projectile.position.X + Projectile.width > player.position.X && Projectile.position.Y < player.position.Y + player.height && Projectile.position.Y + Projectile.height > player.position.Y)
			{
				if (!Main.LocalPlayer.moonLeech)
				{
					int damage = player.statLifeMax2 - player.statLife;
					if (heal > damage)
					{
						this.heal = damage;
					}
					if (this.heal > 0)
					{
						player.HealEffect(this.heal, true);
						player.statLife += this.heal;
						Projectile.Kill();
					}
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 102);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}
		}
	}
}