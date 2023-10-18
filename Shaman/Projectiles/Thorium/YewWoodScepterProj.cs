using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class YewWoodScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 55;
			Projectile.scale = 1f;
			Projectile.alpha = 128;
			AIType = ProjectileID.Bullet;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Shadowflame Bolt");
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 55)
			{
				Projectile.ai[0] = (((float)Main.rand.Next(10) / 10f) - 0.5f);
			}

			Projectile.velocity *= 1.03f;

			Vector2 projectileVelocity = (new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(Projectile.ai[0])));
			Projectile.velocity = projectileVelocity;
			Projectile.netUpdate = true;

			int DustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 27, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
			Main.dust[DustID].noGravity = true;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Projectile.velocity / 2;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			float randX = target.position.X + (target.width / 2) + Main.rand.Next(700) - 350;
			float randY = target.position.Y + (target.height / 2) - Main.rand.Next(300) - 100;

			if (modPlayer.GetNbShamanicBonds() > 2 && Main.rand.Next(7) == 0)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), randX, randY, 0f, 0f, Mod.Find<ModProjectile>("YewWoodScepterPortal").Type, 0, 0.0f, Projectile.owner, 0.0f, 0.0f);
			}
		}
	}
}