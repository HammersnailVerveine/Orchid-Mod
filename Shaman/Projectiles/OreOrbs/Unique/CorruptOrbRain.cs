using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{
	public class CorruptOrbRain : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Flame");
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
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void AI()
		{
			int dust;
			for (int i = 0; i < 2; i++)
			{
				switch (Main.rand.Next(3))
				{
					case 0:
						dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 75);
						Main.dust[dust].velocity.X = Projectile.velocity.X;
						Main.dust[dust].velocity.Y = Projectile.velocity.Y;
						Main.dust[dust].scale = 1f;
						Main.dust[dust].noGravity = true;
						break;
					case 1:
						dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 74);
						Main.dust[dust].velocity.X = Projectile.velocity.X;
						Main.dust[dust].velocity.Y = Projectile.velocity.Y;
						Main.dust[dust].scale = 1f;
						Main.dust[dust].noGravity = true;
						break;
					case 2:
						dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 61);
						Main.dust[dust].velocity.X = Projectile.velocity.X;
						Main.dust[dust].velocity.Y = Projectile.velocity.Y;
						Main.dust[dust].scale = 1f;
						Main.dust[dust].noGravity = true;
						break;
				}
			}
		}
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			target.AddBuff(39, 600);
		}
	}
}