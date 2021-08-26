using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Nature
{
	public class SunflowerFlaskProj1 : OrchidModAlchemistProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seed");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = false;
			projectile.aiStyle = 0;
			projectile.timeLeft = 600;
			ProjectileID.Sets.Homing[projectile.type] = true;
			this.projectileTrail = true;
		}

		public override void AI()
		{
			projectile.rotation += 0.1f;
			projectile.velocity.X *= 0.99f;
			projectile.velocity.Y += 0.1f;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
				return false;
			}

			if (projectile.velocity.Y < 0f)
			{
				projectile.velocity.Y = -oldVelocity.Y;
			}
			else
			{
				int dmg = projectile.damage;
				int projType1 = ProjectileType<Alchemist.Projectiles.Nature.SunflowerFlaskProj2>();
				int projType2 = ProjectileType<Alchemist.Projectiles.Nature.SunflowerFlaskProj3>();
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, projType1, 0, 0f, projectile.owner);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 16, 0f, 0f, projType2, dmg, 0f, projectile.owner);
				projectile.Kill();
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 3);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}
	}
}