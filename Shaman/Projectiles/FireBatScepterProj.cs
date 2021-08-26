using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles
{
	public class FireBatScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.friendly = true;
			projectile.aiStyle = 1;
			projectile.timeLeft = 35;
			projectile.penetrate = 15;
			projectile.scale = 1f;
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fire Bat");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			if (player.GetModPlayer<OrchidModPlayer>().timer120 % 3 == 0)
				projectile.frame++;
			if (projectile.frame == 5)
				projectile.frame = 0;

			for (int i = 0; i < 1; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
				Main.dust[dust].velocity /= 10f;
				Main.dust[dust].scale = 1f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = false;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) { }
	}
}