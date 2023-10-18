using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace OrchidMod.Alchemist.Projectiles.Nature
{
	public class LivingSapVialProj : OrchidModAlchemistProjectile
	{
		public int heal = 0;
		private int animDirection;

		public override void SafeSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 600;
			Projectile.scale = 1f;
			Projectile.alpha = 64;
			Projectile.penetrate = 10;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Sap Bubble");
		}

		public override void OnSpawn(IEntitySource source)
		{
			animDirection = (Main.rand.NextBool(2) ? 1 : -1);
		}

		public override void AI()
		{
			if (Projectile.velocity.Y < 0.5f) Projectile.velocity.Y += 0.02f;
			Projectile.velocity.X *= 0.95f;
			Projectile.rotation += (0.05f * (0.2f - Math.Abs(Projectile.rotation)) + 0.001f) * animDirection;
			if (Math.Abs(Projectile.rotation) >= 0.2f)
			{
				Projectile.rotation = 0.2f * animDirection;
				animDirection *= -1;
			}

			if (Projectile.damage != 0)
			{
				this.heal += Projectile.damage;
				Projectile.damage = 0;
			}

			if (Main.rand.NextBool(20))
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.DesertWater2);
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
				if (!Main.LocalPlayer.moonLeech && Projectile.velocity.Y > 0.5f)
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

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.DesertWater2);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}
		}
	}
}