using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles
{
	public class BulbScepterProj : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spiky Bulbyball");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 38;
			Projectile.height = 38;
			Projectile.friendly = true;
			Projectile.aiStyle = 2;
			Projectile.timeLeft = 150;
			Projectile.scale = 0.7f;
			Projectile.penetrate = 3;
		}

		public override void AI()
		{
			for (int index1 = 0; index1 < 1; ++index1)
			{
				Projectile.velocity = Projectile.velocity * 1.002f;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 3; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 248);
				int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 248);
				Main.dust[dust].noGravity = true;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust2].scale = 1.7f;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer) { }
	}
}