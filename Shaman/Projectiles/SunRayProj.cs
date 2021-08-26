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
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 45;
			projectile.penetrate = 100;
			projectile.extraUpdates = 5;
			projectile.ignoreWater = true;
			projectile.alpha = 255;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sun beam");
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			if (projectile.timeLeft == 45)
			{
				for (int i = 0; i < nbBonds; i++)
				{
					sizeBonus += 2;
					projectile.damage += 5;
				}
				projectile.netUpdate = true;
			}

			++projectile.localAI[0];
			if ((double)projectile.localAI[0] <= 7.0)
				return;
			for (int index1 = 0; index1 < 3; ++index1)
			{
				Vector2 Position = projectile.position - projectile.velocity * ((float)index1 * 0.25f);
				projectile.alpha = (int)byte.MaxValue;
				int index2 = Dust.NewDust(Position, projectile.width, projectile.height, 169, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].scale = (float)(Main.rand.Next(70, 110) * 0.013f) + ((sizeBonus) * 0.15f);
				Main.dust[index2].velocity = projectile.velocity;
				Main.dust[index2].noGravity = true;
			}

			if (projectile.timeLeft % 5 == 0)
				spawnDustCircle(169, -(int)(-10 - projectile.timeLeft));
		}

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			int angle = (int)(360 / 30);
			for (int i = 0; i < 30; i++)
			{
				double dustDeg = i * angle;
				double dustRad = dustDeg * (Math.PI / 180);

				float posX = projectile.Center.X - (int)(Math.Cos(dustRad) * distToCenter);
				float posY = projectile.Center.Y - (int)(Math.Sin(dustRad) * distToCenter);

				Vector2 dustPosition = new Vector2(posX, posY);
				int dust = Dust.NewDust(dustPosition, 1, 1, dustType);
				Dust myDust = Main.dust[dust];

				myDust.velocity = projectile.velocity / 2;
				myDust.fadeIn = 1f;
				myDust.scale = 1.5f;
				myDust.noGravity = true;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			target.AddBuff((24), 5 * 60); // On fire
		}
	}
}