using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Alchemist.Projectiles.Fire
{
	public class HellSlimeFlaskProj : OrchidModAlchemistProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 650;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = 5;
		}

		public override void AI()
		{
			Projectile.rotation += Main.rand.Next(5) / 10f;
			Projectile.velocity.Y += 0.1f;

			if (Main.rand.Next(3) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
				Main.dust[dust].scale *= 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity *= 0f;
			return false;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Slimy Ember");
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.OnFire, 60 * 3);
		}
	}
}