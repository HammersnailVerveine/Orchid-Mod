using Microsoft.Xna.Framework;
using Terraria;


namespace OrchidMod.Shaman.Projectiles.Equipment
{
	public class LavaDroplet : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Lava Droplet");
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 10;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.timeLeft = 100;
			Projectile.light = 0.10f;
			Projectile.alpha = 126;
			Projectile.extraUpdates = 1;
			Projectile.scale = 1f;
			AIType = 1;
		}
		public override void AI()
		{
			if (Main.rand.Next(3) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
				Main.dust[dust].velocity /= 10f;
				Main.dust[dust].scale = 1f;
				Main.dust[dust].noGravity = true;
			}
		}
		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 2; i++)
			{
				int index2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, Projectile.velocity.X, Projectile.velocity.Y, 100, new Color(), 1f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.5f;
				Main.dust[index2].scale = 1.35f;
			}
		}
	}
}