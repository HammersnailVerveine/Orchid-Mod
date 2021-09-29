using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace OrchidMod.Shaman.Projectiles
{
	public class SightScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.penetrate = -1;
			projectile.timeLeft = 90;
			projectile.extraUpdates = 5;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.alpha = 255;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismatic Bream");
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int buffs = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);

			if (projectile.timeLeft == 90 && buffs < 4) projectile.timeLeft = 60;
			if (projectile.timeLeft % 30 == 0) projectile.netUpdate = true;

			int index1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 63, 0.0f, 0.0f, 0, buffs > 3 ? new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB) : Color.White, 2.5f);
			Main.dust[index1].velocity = projectile.velocity / 2;
			Main.dust[index1].scale = 0.8f + ((projectile.timeLeft) / 90f) * 1.8f;
			Main.dust[index1].noGravity = true;

			if (projectile.timeLeft == 90 || projectile.timeLeft == 85 || projectile.timeLeft == 80)
			{

				for (int i = 0; i < 20; i++)
				{

					double dist = projectile.timeLeft - 60;

					double deg = (double)projectile.ai[1] * (i * (36 + 5 - Main.rand.Next(10)));
					double rad = deg * (Math.PI / 180);

					float posX = projectile.Center.X - (int)(Math.Cos(rad) * dist) + projectile.velocity.X - 4;
					float posY = projectile.Center.Y - (int)(Math.Sin(rad) * dist) + projectile.velocity.Y - 4;

					Vector2 dustPosition = new Vector2(posX, posY);

					int index2 = Dust.NewDust(dustPosition, 1, 1, 63, 0.0f, 0.0f, 0, buffs > 3 ? new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB) : Color.White, Main.rand.Next(30, 130) * 0.013f);

					Main.dust[index2].velocity *= 0.2f;
					Main.dust[index2].fadeIn = 1f;
					Main.dust[index2].scale = 1f;
					Main.dust[index2].noGravity = true;
				}
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) { }
	}
}