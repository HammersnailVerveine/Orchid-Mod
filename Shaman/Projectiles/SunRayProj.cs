using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace OrchidMod.Shaman.Projectiles
{
	public class SunRayProj : OrchidModShamanProjectile
	{
		public int sizeBonus = 0;

		public override void SafeSetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 45;
			Projectile.penetrate = 100;
			Projectile.extraUpdates = 5;
			Projectile.ignoreWater = true;
			Projectile.alpha = 255;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sun beam");
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int nbBonds = modPlayer.GetNbShamanicBonds();
			if (Projectile.timeLeft == 45)
			{
				for (int i = 0; i < nbBonds; i++)
				{
					sizeBonus += 2;
					Projectile.damage += 5;
				}
				Projectile.netUpdate = true;
			}

			for (int index1 = 0; index1 < 3; ++index1)
			{
				int index2 = Dust.NewDust(Projectile.Center, 0, 0, 169, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].scale = (float)(Main.rand.Next(70, 110) * 0.013f) + ((sizeBonus) * 0.15f);
				Main.dust[index2].velocity = Projectile.velocity;
				Main.dust[index2].noGravity = true;
			}

			if (Projectile.timeLeft % 5 == 0) {
				int range = Projectile.timeLeft - 10;
				range = range > 20 ? 20 : range;
				spawnDustCircle(169, range);
			}
		}

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			int angle = (int)(360 / 30);
			for (int i = 0; i < 30; i++)
			{
				double dustDeg = i * angle;
				double dustRad = dustDeg * (Math.PI / 180);

				float posX = Projectile.Center.X - (int)(Math.Cos(dustRad) * distToCenter);
				float posY = Projectile.Center.Y - (int)(Math.Sin(dustRad) * distToCenter);

				Vector2 dustPosition = new Vector2(posX, posY);
				int dust = Dust.NewDust(dustPosition, 1, 1, dustType);
				Dust myDust = Main.dust[dust];

				myDust.velocity = Projectile.velocity / 2;
				myDust.fadeIn = 1f;
				myDust.scale = 1.5f;
				myDust.noGravity = true;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			target.AddBuff((24), 5 * 60); // On fire
		}
	}
}