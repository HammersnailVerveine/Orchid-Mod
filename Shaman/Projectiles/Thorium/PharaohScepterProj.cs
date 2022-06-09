using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class PharaohScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 45;
			Projectile.scale = 1f;
			Projectile.alpha = 128;
			AIType = ProjectileID.Bullet;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Bolt");
		}

		public override void AI()
		{
			int DustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 64, -Projectile.velocity.X / 3, -Projectile.velocity.Y / 3, 125, default(Color), 1.25f);
			Main.dust[DustID].noGravity = true;

			if (Projectile.timeLeft % 5 == 0)
			{
				spawnDustCircle(64, 15, false);
				spawnDustCircle(64, 10, true);
			}
		}

		public void spawnDustCircle(int dustType, int distToCenter, bool backwards)
		{
			for (int i = 0; i < 20; i++)
			{
				double deg = (i * (36 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);

				float posX = Projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - Projectile.width / 2 + Projectile.velocity.X + 4;
				float posY = Projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - Projectile.height / 2 + Projectile.velocity.Y + 4;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity = backwards ? Projectile.velocity / 2 : Projectile.velocity * 1.5f;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.3f;
				Main.dust[index2].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Projectile.velocity / 2;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			float randX = target.position.X + (target.width / 2) + Main.rand.Next(700) - 350;
			float randY = target.position.Y + (target.height / 2) - Main.rand.Next(200) - 100;

			if (Main.rand.Next(20) < OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod))
			{
				Projectile.NewProjectile(randX, randY, 0f, 0f, Mod.Find<ModProjectile>("PharaohScepterPortal").Type, 0, 0.0f, Projectile.owner, 0.0f, 0.0f);
			}
		}
	}
}