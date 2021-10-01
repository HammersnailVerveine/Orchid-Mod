using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class HellCardProj : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pepper Mortar");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.friendly = true;
			projectile.aiStyle = 2;
			projectile.timeLeft = 300;
			this.gamblingChipChance = 5;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void SafeAI()
		{
			if (!this.initialized)
			{
				projectile.velocity.Y = projectile.velocity.Y > -10f ? -10f : projectile.velocity.Y;
				this.initialized = true;
			}
			if (projectile.timeLeft == 180)
			{
				int dustType = 6;
				Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
				Main.dust[Dust.NewDust(pos, projectile.width, projectile.height, dustType)].velocity *= 0.25f;
			}
			if (Main.rand.Next(3) == 0)
			{
				int index1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
				Main.dust[index1].velocity = projectile.velocity * 0.5f;
				Main.dust[index1].fadeIn = 1f;
				Main.dust[index1].scale *= 1.5f;
				Main.dust[index1].noGravity = true;
			}
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (projectile.ai[1] != 1f && projectile.owner == Main.myPlayer)
			{
				modPlayer.gamblerSeedCount += 16 + (modPlayer.gamblerLuckySprout ? 5 : 0);
				if (modPlayer.gamblerSeedCount > 99) {
					modPlayer.gamblerSeedCount = 0;
					Vector2 vel = (new Vector2(0f, -3f).RotatedBy(MathHelper.ToRadians(10)));
					int projType = ProjectileType<Gambler.Projectiles.HellCardProjAlt>();
					bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
					OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, projType, projectile.damage, projectile.knockBack, projectile.owner), dummy);
					for (int i = 0; i < 5; i++)
					{
						int dustType = 31;
						Main.dust[Dust.NewDust(player.Center, 10, 10, dustType)].velocity *= 0.25f;
					}
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 6, 25, 10, true, 1.5f, 1f, 5f);
			int projType = ProjectileType<Gambler.Projectiles.MushroomCardProjExplosion>();
			bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
			OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 0f, projType, projectile.damage, 3f, projectile.owner, 0.0f, 0.0f), dummy);
			
			int dustType = 6;
			for (int i = 0; i < 3; i++)
			{
				Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType)].noGravity = true;
			}
		}
	}
}