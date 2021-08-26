using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class DesertCardProjAlt : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cactus");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 20;
			projectile.height = 24;
			projectile.friendly = false;
			projectile.aiStyle = 0;
			projectile.tileCollide = false;
			projectile.timeLeft = 600;
			Main.projFrames[projectile.type] = 4;
			this.gamblingChipChance = 5;
			this.bonusTrigger = true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 31);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}
		}

		public override void SafeAI()
		{
			projectile.velocity *= 0.95f;
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (modPlayer.timer120 % 10 == 0)
			{
				projectile.frame = projectile.frame + 1 == 4 ? 0 : projectile.frame + 1;
			}
		}

		public override void BonusProjectiles(Player player, OrchidModPlayer modPlayer, Projectile projectile, OrchidModGlobalProjectile modProjectile, bool dummy)
		{
			if (modProjectile.gamblerInternalCooldown == 0)
			{
				modProjectile.gamblerInternalCooldown = 10;
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				int projType = ProjectileType<Gambler.Projectiles.DesertCardProj>();
				Vector2 target = Main.MouseWorld;
				Vector2 heading = target - projectile.position;
				heading.Normalize();
				heading *= new Vector2(0f, 8f).Length();
				Vector2 vel = heading.RotatedByRandom(MathHelper.ToRadians(20));
				vel = vel * scale;
				int newProjectile = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, projType, projectile.damage, projectile.knockBack, player.whoAmI), dummy);
				Main.projectile[newProjectile].ai[1] = 1f;
				Main.projectile[newProjectile].netUpdate = true;
				OrchidModProjectile.spawnDustCircle(projectile.Center, 31, 5, 4, true, 1f, 1f, 5f, true, true, false, 0, 0, true);
			}
		}
	}
}