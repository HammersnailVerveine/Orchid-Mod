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
			// DisplayName.SetDefault("Seed");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 600;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			this.projectileTrail = true;
		}

		public override void AI()
		{
			Projectile.rotation += 0.1f;
			Projectile.velocity.X *= 0.99f;
			Projectile.velocity.Y += 0.1f;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
				return false;
			}

			if (Projectile.velocity.Y < 0f)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			else
			{
				int dmg = Projectile.damage;
				int projType1 = ProjectileType<Alchemist.Projectiles.Nature.SunflowerFlaskProj2>();
				int projType2 = ProjectileType<Alchemist.Projectiles.Nature.SunflowerFlaskProj3>();
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, projType1, 0, 0f, Projectile.owner);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y - 16, 0f, 0f, projType2, dmg, 0f, Projectile.owner);
				Projectile.Kill();
			}
			return false;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 3);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}
	}
}