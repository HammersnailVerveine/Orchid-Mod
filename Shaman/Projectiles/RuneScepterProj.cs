using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles
{
	public class RuneScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 50;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Rune Bolt");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Projectile.rotation += 0.3f;
			for (int index1 = 0; index1 < 2; ++index1)
			{
				int index2 = Dust.NewDust(new Vector2(Projectile.position.X + Projectile.velocity.X / 3, Projectile.position.Y + Projectile.velocity.Y / 3), Projectile.width, Projectile.height, 106, Projectile.velocity.X, Projectile.velocity.Y, 100, new Color(), 1f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= (float)(0.100000001490116 + (double)Main.rand.Next(4) * 0.100000001490116); // wtf
				Main.dust[index2].scale *= (float)(1.0 + (double)Main.rand.Next(5) * 0.100000001490116);
			}
			Projectile.ai[1]++;
			Projectile.velocity.Y *= 0.96f;
			Projectile.velocity.X *= 0.96f;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 3; i++)
			{
				int index2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 106, Projectile.velocity.X, Projectile.velocity.Y, 100, new Color(), 1f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 3f;
				Main.dust[index2].scale *= (float)(1.0 + (double)Main.rand.Next(5) * 0.100000001490116);
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer) { }
	}
}