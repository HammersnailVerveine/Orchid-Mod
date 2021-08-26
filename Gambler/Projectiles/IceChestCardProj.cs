using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Gambler.Projectiles
{
	public class IceChestCardProj : OrchidModGamblerProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 14;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.aiStyle = 1;
			projectile.timeLeft = 120;
			projectile.scale = 1f;
			this.projectileTrail = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Spear");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return projectile.timeLeft < 119 ? Color.White : Color.Black;
		}

		public override void SafeAI()
		{
			if (projectile.timeLeft == 120)
			{
				for (int i = 0; i < 3; i++)
				{
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 67);
					Main.dust[dust].velocity = -projectile.velocity / 5;
					Main.dust[dust].scale = 1f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].noLight = true;
				}
			}

			if (Main.rand.Next(6) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 67);
				Main.dust[dust].velocity /= 10f;
				Main.dust[dust].scale = 1f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 67);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].velocity = projectile.velocity / 2;
				Main.dust[dust].noLight = true;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (modPlayer.gamblerElementalLens)
			{
				target.AddBuff(44, 60 * 5); // Frostburn
			}
		}
	}
}