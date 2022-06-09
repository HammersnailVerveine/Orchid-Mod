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
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 90;
			Projectile.extraUpdates = 5;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.alpha = 255;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismatic Bream");
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int buffs = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);

			if (Projectile.timeLeft == 90 && buffs < 4) Projectile.timeLeft = 60;
			if (Projectile.timeLeft % 30 == 0) Projectile.netUpdate = true;

			int index1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 63, 0.0f, 0.0f, 0, buffs > 3 ? new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB) : Color.White, 2.5f);
			Main.dust[index1].velocity = Projectile.velocity / 2;
			Main.dust[index1].scale = 0.8f + ((Projectile.timeLeft) / 90f) * 1.8f;
			Main.dust[index1].noGravity = true;

			if (Projectile.timeLeft == 90 || Projectile.timeLeft == 85 || Projectile.timeLeft == 80)
			{

				for (int i = 0; i < 20; i++)
				{

					double dist = Projectile.timeLeft - 60;

					double deg = (double)Projectile.ai[1] * (i * (36 + 5 - Main.rand.Next(10)));
					double rad = deg * (Math.PI / 180);

					float posX = Projectile.Center.X - (int)(Math.Cos(rad) * dist) + Projectile.velocity.X - 4;
					float posY = Projectile.Center.Y - (int)(Math.Sin(rad) * dist) + Projectile.velocity.Y - 4;

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