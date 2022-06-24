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
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.aiStyle = 2;
			Projectile.timeLeft = 300;
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
				Projectile.velocity.Y = Projectile.velocity.Y > -10f ? -10f : Projectile.velocity.Y;
				this.initialized = true;
			}
			if (Projectile.timeLeft == 180)
			{
				int dustType = 6;
				Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
				Main.dust[Dust.NewDust(pos, Projectile.width, Projectile.height, dustType)].velocity *= 0.25f;
			}
			if (Main.rand.Next(3) == 0)
			{
				int index1 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
				Main.dust[index1].velocity = Projectile.velocity * 0.5f;
				Main.dust[index1].fadeIn = 1f;
				Main.dust[index1].scale *= 1.5f;
				Main.dust[index1].noGravity = true;
			}
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidGambler modPlayer)
		{
			if (Projectile.ai[1] != 1f && Projectile.owner == Main.myPlayer)
			{
				modPlayer.gamblerSeedCount += 16 + (modPlayer.gamblerLuckySprout ? 5 : 0);
				if (modPlayer.gamblerSeedCount > 99) {
					modPlayer.gamblerSeedCount = 0;
					Vector2 vel = (new Vector2(0f, -3f).RotatedBy(MathHelper.ToRadians(10)));
					int projType = ProjectileType<Gambler.Projectiles.HellCardProjAlt>();
					bool dummy = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
					int newProjectile = DummyProjectile(Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.Center.Y, vel.X, vel.Y, projType, Projectile.damage, Projectile.knockBack, Projectile.owner), dummy);
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
	}
}