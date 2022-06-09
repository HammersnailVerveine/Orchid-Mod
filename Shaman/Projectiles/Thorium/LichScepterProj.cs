using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class LichScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 70;
			Projectile.scale = 1f;
			AIType = ProjectileID.Bullet;
			this.projectileTrail = true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Necrotic Bolt");
		}

		public override void AI()
		{
			if (Projectile.timeLeft > 42) Projectile.velocity *= 1.1f;

			if (Main.rand.Next(2) == 0)
			{
				int DustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 15, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
				Main.dust[DustID].noGravity = true;
			}

			if (Main.rand.Next(2) == 0)
			{
				int DustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 127, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
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
			for (int i = 0; i < 4; i++)
			{
				Vector2 projectileVelocity = (new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(-40 + (25 * i))));
				Projectile.NewProjectile(Projectile.position.X + projectileVelocity.X, Projectile.position.Y + Projectile.velocity.Y, projectileVelocity.X / 2, projectileVelocity.Y / 2, Mod.Find<ModProjectile>("LichScepterProjAlt").Type, (int)(Projectile.damage * 0.75), 0.0f, Projectile.owner, 0.0f, 0.0f);
			}
		}
	}
}