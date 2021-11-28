using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class MushroomCardProj : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosive Mushroom");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.friendly = false;
			projectile.aiStyle = 2;
			projectile.timeLeft = 300;
			projectile.penetrate = 2;
			this.gamblingChipChance = 5;
		}

		public override void SafeAI()
		{
			projectile.friendly = projectile.penetrate < 2;

			if (projectile.timeLeft == 180)
			{
				int dustType = 172;
				Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
				Main.dust[Dust.NewDust(pos, projectile.width, projectile.height, dustType)].velocity *= 0.25f;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.penetrate--;
			projectile.timeLeft = 60;
			if (projectile.penetrate < 0) projectile.Kill();
			if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
			if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			return false;
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (projectile.ai[1] != 1f && projectile.owner == Main.myPlayer)
			{
				modPlayer.gamblerSeedCount += 10 + (modPlayer.gamblerLuckySprout ? 3 : 0);
				if (modPlayer.gamblerSeedCount > 99) {
					modPlayer.gamblerSeedCount = 0;
					Vector2 vel = (new Vector2(0f, -3f).RotatedBy(MathHelper.ToRadians(10)));
					int projType = ProjectileType<Gambler.Projectiles.MushroomCardProjAlt>();
					bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
					int newProjectile = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, projType, projectile.damage, projectile.knockBack, projectile.owner), dummy);
					Main.projectile[newProjectile].ai[1] = 1f;
					Main.projectile[newProjectile].netUpdate = true;
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
			OrchidModProjectile.spawnDustCircle(projectile.Center, 172, 25, 10, true, 1.5f, 1f, 5f);
			int projType = ProjectileType<Gambler.Projectiles.MushroomCardProjExplosion>();
			bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
			OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 0f, projType, (int)(projectile.damage * 0.8), 3f, projectile.owner, 0.0f, 0.0f), dummy);
			int dustType = 172;
			for (int i = 0; i < 3; i++)
			{
				Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType)].noGravity = true;
			}
		}
	}
}