using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class ThoriumScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 30;
			Projectile.scale = 1f;
			AIType = ProjectileID.Bullet;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thorium Bolt");
		}

		public override void AI()
		{
			if (Main.rand.Next(2) == 0)
			{
				int DustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 15, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
				Main.dust[DustID].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 15);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Projectile.velocity / 2;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			Vector2 projectileVelocity = (new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(20)));
			Vector2 projectileVelocity2 = (new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(-20)));

			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + projectileVelocity.X, Projectile.position.Y + Projectile.velocity.Y, projectileVelocity.X, projectileVelocity.Y, Mod.Find<ModProjectile>("ThoriumScepterProjSplit").Type, Projectile.damage, 0.0f, Projectile.owner, 0.0f, 0.0f);
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + projectileVelocity.X, Projectile.position.Y + Projectile.velocity.Y, projectileVelocity2.X, projectileVelocity2.Y, Mod.Find<ModProjectile>("ThoriumScepterProjSplit").Type, Projectile.damage, 0.0f, Projectile.owner, 0.0f, 0.0f);
		}
	}
}