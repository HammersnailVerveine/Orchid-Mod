using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Alchemist.Projectiles.Fire
{
	public class HellSlimeFlaskProj : OrchidModAlchemistProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 650;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = 5;
		}

		public override void AI()
		{
			projectile.rotation += Main.rand.Next(5) / 10f;
			projectile.velocity.Y += 0.1f;

			if (Main.rand.Next(3) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
				Main.dust[dust].scale *= 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.velocity *= 0f;
			return false;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slimy Ember");
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 60 * 3);
		}
	}
}