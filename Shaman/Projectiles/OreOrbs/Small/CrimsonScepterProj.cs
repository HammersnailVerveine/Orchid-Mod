using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Small
{
	public class CrimsonScepterProj : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Demonite Bolt");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 90;
			projectile.scale = 1f;
			projectile.penetrate = 2;
			this.projectileTrail = true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			if (Main.rand.Next(5) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 258);
				Main.dust[dust].velocity /= 3f;
				Main.dust[dust].scale *= 1.3f;
				Main.dust[dust].noGravity = true;
			}
			
			if (projectile.timeLeft < 40) {
				projectile.velocity *= 0.9f;
			}
		}

		public override void Kill(int timeLeft)
		{
			OrchidModProjectile.spawnDustCircle(projectile.Center, 258, 5, 8, true, 1.5f, 1f, 4f, true, true, false, 0, 0, true);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 258, 5, 8, true, 1.5f, 1f, 2.5f, true, true, false, 0, 0, true);
			OrchidModProjectile.spawnGenericExplosion(projectile, projectile.damage, projectile.knockBack, 75, 1, false, 27);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			projectile.friendly = false;
			projectile.timeLeft = projectile.timeLeft > 40 ? 40 : projectile.timeLeft;
			
			if (modPlayer.shamanOrbSmall != ShamanOrbSmall.CRIMSON)
			{
				modPlayer.shamanOrbSmall = ShamanOrbSmall.CRIMSON;
				modPlayer.orbCountSmall = 0;
			}
			modPlayer.orbCountSmall++;

			if (modPlayer.orbCountSmall == 1)
			{
				Projectile.NewProjectile(player.Center.X - 15, player.position.Y - 20, 0f, 0f, mod.ProjectileType("CrimsonOrb"), 0, 0, projectile.owner, 0f, 0f);

				if (player.FindBuffIndex(mod.BuffType("ShamanicBaubles")) > -1)
				{
					modPlayer.orbCountSmall++;
					Projectile.NewProjectile(player.Center.X, player.position.Y - 25, 0f, 0f, mod.ProjectileType("CrimsonOrb"), 1, 0, projectile.owner, 0f, 0f);
					player.ClearBuff(mod.BuffType("ShamanicBaubles"));
				}
			}
			if (modPlayer.orbCountSmall == 2)
				Projectile.NewProjectile(player.Center.X, player.position.Y - 25, 0f, 0f, mod.ProjectileType("CrimsonOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountSmall == 3)
				Projectile.NewProjectile(player.Center.X + 15, player.position.Y - 20, 0f, 0f, mod.ProjectileType("CrimsonOrb"), 0, 0, projectile.owner, 0f, 0f);

			if (modPlayer.orbCountSmall > 3)
			{
				if (Main.myPlayer == player.whoAmI)
					player.HealEffect(10, true);
				player.statLife += 10;
				modPlayer.orbCountSmall = 0;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X / 2;
			if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y / 2;
			return false;
		}
	}
}