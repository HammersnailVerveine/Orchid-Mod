using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Alchemist.Projectiles.Fire
{
	public class EmberVialProj : OrchidModAlchemistProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 650;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = 3;
			Main.projFrames[Projectile.type] = 4;
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 600)
			{
				Projectile.velocity.X = 0f;
				Projectile.velocity.Y = 1f;
				Vector2 initialVelocity = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(90));
				Projectile.velocity = initialVelocity;
				Projectile.frame = Main.rand.Next(4);
				Projectile.friendly = true;
				Projectile.netUpdate = true;
			}

			if (Main.rand.Next(3) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
				Main.dust[dust].scale *= 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}

			Vector2 projectileVelocity = (new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(3)));
			Projectile.velocity = projectileVelocity;
			Projectile.velocity.Y += 0.01f;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity *= 0f;
			return false;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ember");
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.OnFire, 60 * 3);
		}
	}
}