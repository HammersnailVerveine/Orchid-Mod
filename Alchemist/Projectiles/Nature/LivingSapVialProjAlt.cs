using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Alchemist.Projectiles.Nature
{
	public class LivingSapVialProjAlt : OrchidModAlchemistProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 16;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 180;
		}

		public override void AI()
		{
			Projectile.rotation += 0.05f;
			Projectile.velocity.Y -= 0.03f;
			Projectile.velocity.X *= 0.95f;
			Projectile.alpha += 3 + Main.rand.Next(3);
			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}
			Projectile.velocity = (Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(5)));
			if (Main.rand.NextBool(10))
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 102);
				Main.dust[dust].noGravity = true;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
			Projectile.ai[1] = Projectile.ai[1] == -1 ? 1 : -1;
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Water_Desert);
				Main.dust[dust].noGravity = true;
			}
		}
	}
}