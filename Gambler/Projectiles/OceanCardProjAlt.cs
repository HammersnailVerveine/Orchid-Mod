using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class OceanCardProjAlt : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fartling ball");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			projectile.friendly = false;
			projectile.aiStyle = 0;
			projectile.tileCollide = false;
			projectile.timeLeft = 600;
			Main.projFrames[projectile.type] = 5;
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
			if (modPlayer.timer120 == 0 || modPlayer.timer120 > 49) projectile.frame = 0;
			if (modPlayer.timer120 == 10) projectile.frame = 1;
			if (modPlayer.timer120 == 20) projectile.frame = 2;
			if (modPlayer.timer120 == 30) projectile.frame = 3;
			if (modPlayer.timer120 == 40) projectile.frame = 4;
		}

		public override void BonusProjectiles(Player player, OrchidModPlayer modPlayer, Projectile projectile, OrchidModGlobalProjectile modProjectile, bool dummy)
		{
			if (modProjectile.gamblerInternalCooldown == 0)
			{
				modProjectile.gamblerInternalCooldown = 50;
				int projType = ProjectileType<Gambler.Projectiles.OceanCardProj>();
				Vector2 target = Main.MouseWorld;
				Vector2 heading = target - projectile.position;
				heading.Normalize();
				heading *= new Vector2(0f, 5f).Length();
				int newProjectile = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, heading.X, heading.Y, projType, projectile.damage, projectile.knockBack, player.whoAmI), dummy);
				Main.projectile[newProjectile].ai[1] = 1f;
				Main.projectile[newProjectile].netUpdate = true;
				OrchidModProjectile.spawnDustCircle(projectile.Center, 31, 10, 10, true, 1.5f, 1f, 3f, true, true, false, 0, 0, true);
			}
		}
	}
}