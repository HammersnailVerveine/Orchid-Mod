using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Alchemist.Projectiles.Fire
{
	public class EmberVialProjAlt : OrchidModAlchemistProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = false;
			projectile.aiStyle = 0;
			projectile.timeLeft = 60;
			projectile.alpha = 255;
			projectile.scale = 1f;
		}

		public override void AI()
		{
			projectile.velocity *= 0.95f;
			projectile.velocity.Y += 0.02f;
			projectile.rotation += 0.1f;
			if (Main.rand.Next(3) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
				Main.dust[dust].scale *= 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}

			Vector2 projectileVelocity = (new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(5)));
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			return false;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ember");
		}
	}
}