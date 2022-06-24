using Terraria;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{
	public class IchorOrbRain : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ichor");
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 300;
			Projectile.scale = 0.5f;
			Projectile.extraUpdates = 1;
			Projectile.penetrate = 15;
			Projectile.alpha = 255;
		}

		public override void AI()
		{
			int dust;
			for (int i = 0; i < 3; i++)
			{
				switch (Main.rand.Next(3))
				{
					case 0:
						dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 169);
						Main.dust[dust].velocity.X = Projectile.velocity.X;
						Main.dust[dust].velocity.Y = Projectile.velocity.Y;
						Main.dust[dust].scale = 1f;
						Main.dust[dust].noGravity = true;
						break;
					case 1:
						dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 162);
						Main.dust[dust].velocity.X = Projectile.velocity.X;
						Main.dust[dust].velocity.Y = Projectile.velocity.Y;
						Main.dust[dust].scale = 1.3f;
						Main.dust[dust].noGravity = true;
						break;
					case 2:
						dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 228);
						Main.dust[dust].velocity.X = Projectile.velocity.X;
						Main.dust[dust].velocity.Y = Projectile.velocity.Y;
						Main.dust[dust].scale = 1f;
						Main.dust[dust].noGravity = true;
						break;
				}
			}
		}
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayerShaman modPlayer)
		{
			target.AddBuff(69, 600);
		}
	}
}