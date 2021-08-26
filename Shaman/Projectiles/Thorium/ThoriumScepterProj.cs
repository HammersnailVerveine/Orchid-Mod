using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class ThoriumScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 8;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.aiStyle = 1;
			projectile.timeLeft = 30;
			projectile.scale = 1f;
			aiType = ProjectileID.Bullet;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thorium Bolt");
		}

		public override void AI()
		{
			if (Main.rand.Next(2) == 0)
			{
				int DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 15, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
				Main.dust[DustID].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 15);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = projectile.velocity / 2;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			Vector2 projectileVelocity = (new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(20)));
			Vector2 projectileVelocity2 = (new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(-20)));

			Projectile.NewProjectile(projectile.position.X + projectileVelocity.X, projectile.position.Y + projectile.velocity.Y, projectileVelocity.X, projectileVelocity.Y, mod.ProjectileType("ThoriumScepterProjSplit"), projectile.damage, 0.0f, projectile.owner, 0.0f, 0.0f);
			Projectile.NewProjectile(projectile.position.X + projectileVelocity.X, projectile.position.Y + projectile.velocity.Y, projectileVelocity2.X, projectileVelocity2.Y, mod.ProjectileType("ThoriumScepterProjSplit"), projectile.damage, 0.0f, projectile.owner, 0.0f, 0.0f);
		}
	}
}