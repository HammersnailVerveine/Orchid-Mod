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
			projectile.width = 14;
			projectile.height = 18;
			projectile.friendly = true;
			projectile.aiStyle = 1;
			projectile.timeLeft = 300;
			projectile.scale = 0.5f;
			projectile.extraUpdates = 1;
			projectile.penetrate = 15;
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
						dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 75);
						Main.dust[dust].velocity.X = projectile.velocity.X;
						Main.dust[dust].velocity.Y = projectile.velocity.Y;
						Main.dust[dust].scale = 1f;
						Main.dust[dust].noGravity = true;
						break;
					case 1:
						dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 74);
						Main.dust[dust].velocity.X = projectile.velocity.X;
						Main.dust[dust].velocity.Y = projectile.velocity.Y;
						Main.dust[dust].scale = 1f;
						Main.dust[dust].noGravity = true;
						break;
					case 2:
						dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 61);
						Main.dust[dust].velocity.X = projectile.velocity.X;
						Main.dust[dust].velocity.Y = projectile.velocity.Y;
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