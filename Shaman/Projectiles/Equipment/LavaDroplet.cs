using Microsoft.Xna.Framework;
using Terraria;


namespace OrchidMod.Shaman.Projectiles.Equipment
{
	public class LavaDroplet : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lava Droplet");
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void SafeSetDefaults()
		{
			projectile.width = 6;
			projectile.height = 10;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.timeLeft = 100;
			projectile.light = 0.10f;
			projectile.alpha = 126;
			projectile.extraUpdates = 1;
			projectile.scale = 1f;
			aiType = 1;
		}
		public override void AI()
		{
			if (Main.rand.Next(3) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
				Main.dust[dust].velocity /= 10f;
				Main.dust[dust].scale = 1f;
				Main.dust[dust].noGravity = true;
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 2; i++)
			{
				int index2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, projectile.velocity.X, projectile.velocity.Y, 100, new Color(), 1f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.5f;
				Main.dust[index2].scale = 1.35f;
			}
		}
	}
}