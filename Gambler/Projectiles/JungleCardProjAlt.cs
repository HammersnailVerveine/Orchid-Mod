using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class JungleCardProjAlt : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spore");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 26;
			projectile.height = 26;
			projectile.friendly = false;
			projectile.aiStyle = 0;
			projectile.tileCollide = false;
			projectile.timeLeft = 600;
			this.bonusTrigger = true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 44);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}
		}

		public override void SafeAI()
		{
			projectile.rotation += 0.01f;
			projectile.velocity *= 0.95f;
		}

		public override void BonusProjectiles(Player player, OrchidModPlayer modPlayer, Projectile projectile, OrchidModGlobalProjectile modProjectile, bool dummy)
		{
			if (modProjectile.gamblerInternalCooldown == 0)
			{
				modProjectile.gamblerInternalCooldown = 30;
				int projType = ProjectileType<Gambler.Projectiles.JungleCardProj>();
				Vector2 target = Main.MouseWorld;
				Vector2 heading = target - projectile.position;
				heading.Normalize();
				heading *= new Vector2(0f, 10f).Length();
				Vector2 vel = heading.RotatedByRandom(MathHelper.ToRadians(15));
				int newProjectile = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, projType, projectile.damage, projectile.knockBack, player.whoAmI), dummy);
				Main.projectile[newProjectile].localAI[1] = 1f;
				Main.projectile[newProjectile].netUpdate = true;
				OrchidModProjectile.spawnDustCircle(projectile.Center - new Vector2(4, 4), 44, 10, 4, false, 1f, 1.5f, 5f, true, true, false, 0, 0, true);
			}
		}
	}
}