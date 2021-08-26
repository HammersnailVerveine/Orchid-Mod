using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class DungeonCardProj : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Bolt");
		}

		// public override Color? GetAlpha(Color lightColor)  {
		// return Color.White;
		// }

		public override void SafeSetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 90;
			projectile.penetrate = 1;
			projectile.alpha = 128;
			this.gamblingChipChance = 5;
			this.baseCritChance = 10;
		}

		public override void SafeAI()
		{
			projectile.rotation += 0.2f;
			projectile.velocity *= 1.03f;

			for (int i = 0; i < 3; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 29);
				Main.dust[dust].scale *= 1.5f;
				Main.dust[dust].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			OrchidModProjectile.spawnDustCircle(projectile.Center, 29, 5, 5, true, 1.3f, 1f, 3f, true, true, false, 0, 0, true);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			OrchidModGlobalNPC modTarget = target.GetGlobalNPC<OrchidModGlobalNPC>();
			if (projectile.owner == Main.myPlayer)
			{
				modTarget.gamblerDungeonCardCount++;
				if (modTarget.gamblerDungeonCardCount >= 3)
				{
					modTarget.gamblerDungeonCardCount = 0;
					OrchidModProjectile.spawnDustCircle(projectile.Center, 29, 10, 15, true, 1.3f, 1f, 8f, true, true, false, 0, 0, true);
					Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 45);
					int projType = ProjectileType<Gambler.Projectiles.DungeonCardProjAlt>();
					float scale = 10f - (Main.rand.NextFloat() * 2.5f);
					Vector2 vel = (projectile.velocity.RotatedByRandom(MathHelper.ToRadians(20)));
					vel.Normalize();
					vel *= scale;
					bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
					int newProjInt = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, projType, (int)(projectile.damage * 5), 0.1f, projectile.owner), dummy);
					Projectile newProj = Main.projectile[newProjInt];
					newProj.ai[1] = (int)target.whoAmI;
					newProj.netUpdate = true;
				}
			}

			if (modPlayer.gamblerElementalLens)
			{
				target.AddBuff(31, 60 * 3); // Confused
			}
		}
	}
}