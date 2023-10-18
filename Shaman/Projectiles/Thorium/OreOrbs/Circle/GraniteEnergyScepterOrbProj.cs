using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Circle
{
	public class GraniteEnergyScepterOrbProj : OrchidModShamanProjectile
	{
		private int pos;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Granite Orb");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 12960000;
			Projectile.extraUpdates = 1;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();

			switch (Projectile.damage)
			{
				case 1:
					this.pos = 1;
					Projectile.damage = (int)player.GetDamage<ShamanDamageClass>().ApplyTo(30);
					break;
				case 2:
					this.pos = 2;
					Projectile.damage = (int)player.GetDamage<ShamanDamageClass>().ApplyTo(30);
					break;
				case 3:
					this.pos = 3;
					Projectile.damage = (int)player.GetDamage<ShamanDamageClass>().ApplyTo(30);
					break;
				case 4:
					this.pos = 4;
					Projectile.damage = (int)player.GetDamage<ShamanDamageClass>().ApplyTo(30);
					break;
				default:
					if (Projectile.damage < 5)
					{
						Projectile.Kill();
					}
					break;
			}

			if (!(player.FindBuffIndex(Mod.Find<ModBuff>("GraniteAura").Type) > -1))
			{
				Projectile.Kill();
			}

			float x = Projectile.position.X - Projectile.velocity.X / 10f;
			float y = Projectile.position.Y - Projectile.velocity.Y / 10f;
			int index2 = Dust.NewDust(new Vector2(x, y), Projectile.width, Projectile.height, 172, 0.0f, 0.0f, 0, new Color(), 1f);
			Main.dust[index2].alpha = Projectile.alpha;
			Main.dust[index2].scale = (float)Main.rand.Next(70, 110) * 0.013f;
			Main.dust[index2].velocity *= 0f;
			Main.dust[index2].noGravity = true;

			double deg = (double)Projectile.ai[1];
			double rad = deg * (Math.PI / 180);
			double rad2 = rad + (90 * (Math.PI / 180));
			double dist = 128;

			switch (this.pos)
			{
				case 1:
					Projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
					Projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;
					break;
				case 2:
					Projectile.position.X = player.Center.X - (int)(Math.Cos(rad2) * dist) - Projectile.width / 2;
					Projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad2) * dist) - Projectile.height / 2;
					break;
				case 3:
					Projectile.position.X = player.Center.X + (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
					Projectile.position.Y = player.Center.Y + (int)(Math.Sin(rad) * dist) - Projectile.height / 2;
					break;
				case 4:
					Projectile.position.X = player.Center.X + (int)(Math.Cos(rad2) * dist) - Projectile.width / 2;
					Projectile.position.Y = player.Center.Y + (int)(Math.Sin(rad2) * dist) - Projectile.height / 2;
					break;
			}

			Projectile.ai[1] += 1f;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 172);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
			}
		}
	}
}