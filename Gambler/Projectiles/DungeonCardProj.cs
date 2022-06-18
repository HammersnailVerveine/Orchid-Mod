using Microsoft.Xna.Framework;
using OrchidMod.Common.Globals.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
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
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 90;
			Projectile.penetrate = 1;
			Projectile.alpha = 128;
			this.gamblingChipChance = 5;
			this.baseCritChance = 10;
		}

		public override void SafeAI()
		{
			Projectile.rotation += 0.2f;
			Projectile.velocity *= 1.03f;

			for (int i = 0; i < 3; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 29);
				Main.dust[dust].scale *= 1.5f;
				Main.dust[dust].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 29, 5, 5, true, 1.3f, 1f, 3f, true, true, false, 0, 0, true);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			OrchidGlobalNPC modTarget = target.GetGlobalNPC<OrchidGlobalNPC>();
			if (Projectile.owner == Main.myPlayer)
			{
				modTarget.GamblerDungeonCardCount++;
				if (modTarget.GamblerDungeonCardCount >= 3)
				{
					modTarget.GamblerDungeonCardCount = 0;
					OrchidModProjectile.spawnDustCircle(Projectile.Center, 29, 10, 15, true, 1.3f, 1f, 8f, true, true, false, 0, 0, true);
					SoundEngine.PlaySound(SoundID.Item45, Projectile.Center);
					int projType = ProjectileType<Gambler.Projectiles.DungeonCardProjAlt>();
					float scale = 10f - (Main.rand.NextFloat() * 2.5f);
					Vector2 vel = (Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(20)));
					vel.Normalize();
					vel *= scale;
					bool dummy = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
					int newProjInt = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, vel.X, vel.Y, projType, (int)(Projectile.damage * 5), 0.1f, Projectile.owner), dummy);
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