using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.NPCs.Hostile.DownpourElemental.Projectiles
{
	public class DownpourElementalCloud : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 50;
			Projectile.hostile = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 60;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thunder Cloud");
		}

		public override void AI()
		{
			int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 16);
			Main.dust[dust2].scale = 1.2f;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].velocity *= 1f;
		}
	}
}