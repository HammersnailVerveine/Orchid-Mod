using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Circle
{
	public class GraniteEnergyScepterOrbProj : OrchidModShamanProjectile
	{
		private int pos;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Orb");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 18;
			projectile.height = 18;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.magic = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.timeLeft = 12960000;
			projectile.extraUpdates = 1;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			switch (projectile.damage)
			{
				case 1:
					this.pos = 1;
					projectile.damage = (int)(30 * modPlayer.shamanDamage);
					break;
				case 2:
					this.pos = 2;
					projectile.damage = (int)(30 * modPlayer.shamanDamage);
					break;
				case 3:
					this.pos = 3;
					projectile.damage = (int)(30 * modPlayer.shamanDamage);
					break;
				case 4:
					this.pos = 4;
					projectile.damage = (int)(30 * modPlayer.shamanDamage);
					break;
				default:
					if (projectile.damage < 5)
					{
						projectile.Kill();
					}
					break;
			}

			if (!(player.FindBuffIndex(mod.BuffType("GraniteAura")) > -1))
			{
				projectile.Kill();
			}

			float x = projectile.position.X - projectile.velocity.X / 10f;
			float y = projectile.position.Y - projectile.velocity.Y / 10f;
			int index2 = Dust.NewDust(new Vector2(x, y), projectile.width, projectile.height, 172, 0.0f, 0.0f, 0, new Color(), 1f);
			Main.dust[index2].alpha = projectile.alpha;
			Main.dust[index2].scale = (float)Main.rand.Next(70, 110) * 0.013f;
			Main.dust[index2].velocity *= 0f;
			Main.dust[index2].noGravity = true;

			double deg = (double)projectile.ai[1];
			double rad = deg * (Math.PI / 180);
			double rad2 = rad + (90 * (Math.PI / 180));
			double dist = 128;

			switch (this.pos)
			{
				case 1:
					projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width / 2;
					projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height / 2;
					break;
				case 2:
					projectile.position.X = player.Center.X - (int)(Math.Cos(rad2) * dist) - projectile.width / 2;
					projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad2) * dist) - projectile.height / 2;
					break;
				case 3:
					projectile.position.X = player.Center.X + (int)(Math.Cos(rad) * dist) - projectile.width / 2;
					projectile.position.Y = player.Center.Y + (int)(Math.Sin(rad) * dist) - projectile.height / 2;
					break;
				case 4:
					projectile.position.X = player.Center.X + (int)(Math.Cos(rad2) * dist) - projectile.width / 2;
					projectile.position.Y = player.Center.Y + (int)(Math.Sin(rad2) * dist) - projectile.height / 2;
					break;
			}

			projectile.ai[1] += 1f;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
			}
		}
	}
}