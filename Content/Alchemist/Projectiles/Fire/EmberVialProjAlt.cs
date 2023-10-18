using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Content.Alchemist.Projectiles.Fire
{
	public class EmberVialProjAlt : OrchidModAlchemistProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 60;
			Projectile.alpha = 255;
			Projectile.scale = 1f;
		}

		public override void AI()
		{
			Projectile.velocity *= 0.95f;
			Projectile.velocity.Y += 0.02f;
			Projectile.rotation += 0.1f;
			if (Main.rand.Next(3) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
				Main.dust[dust].scale *= 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}

			Vector2 projectileVelocity = (new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(5)));
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			return false;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ember");
		}
	}
}