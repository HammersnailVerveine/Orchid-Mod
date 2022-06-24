using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Unique
{
	public class ThunderScepterProj : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thunder bolt");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.scale = 1f;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 20;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 1;
			Projectile.ignoreWater = true;
			Projectile.alpha = 255;
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 20)
			{
				Projectile.ai[0] = (Main.rand.Next(40) + 20);

				if (Main.player[Projectile.owner].velocity.Y == 0)
				{
					Projectile.ai[0] /= 2;
				}

				if (Main.rand.Next(2) == 0)
				{
					Projectile.ai[0] = -Projectile.ai[0];
				}

				for (int index1 = 0; index1 < 5; ++index1)
				{
					float x = Projectile.position.X - Projectile.velocity.X / 2f * (float)index1;
					float y = Projectile.position.Y - Projectile.velocity.Y / 2f * (float)index1;
					int index2 = Dust.NewDust(new Vector2(x, y), 1, 1, 159, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].alpha = Projectile.alpha;
					Main.dust[index2].position.X = x;
					Main.dust[index2].position.Y = y;
					Main.dust[index2].scale = (float)150 * 0.015f;
					Main.dust[index2].velocity *= 2f;
					Main.dust[index2].noGravity = true;
				}
			}

			for (int index1 = 0; index1 < 9; ++index1)
			{
				if (index1 % 3 == 0)
				{
					float x = Projectile.position.X - Projectile.velocity.X / 10f * (float)index1;
					float y = Projectile.position.Y - Projectile.velocity.Y / 10f * (float)index1;
					int index2 = Dust.NewDust(new Vector2(x, y), 1, 1, 159, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].alpha = Projectile.alpha;
					Main.dust[index2].position.X = x;
					Main.dust[index2].position.Y = y;

					Main.dust[index2].scale = 65f * 0.015f;
					if (Projectile.timeLeft > 12)
					{
						Main.dust[index2].scale = 95f * 0.015f;
					}
					else if (Projectile.timeLeft > 8)
					{
						Main.dust[index2].scale = 80f * 0.015f;
					}

					Main.dust[index2].velocity *= 0.0f;
					Main.dust[index2].noGravity = true;
				}
			}

			if (Projectile.timeLeft == 20)
			{
				Vector2 projectileVelocity = (new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(Projectile.ai[0] / 2)));
				Projectile.velocity = projectileVelocity;
			}

			if (Projectile.timeLeft == 12)
			{
				Vector2 projectileVelocity = (new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(-((Projectile.ai[0]) * 2))));
				Projectile.velocity = projectileVelocity;
			}

			if (Projectile.timeLeft == 8)
			{
				Vector2 projectileVelocity = (new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(MathHelper.ToRadians((Projectile.ai[0]) * 2)));
				Projectile.velocity = projectileVelocity;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayerShaman modPlayer)
		{
			for (int i = 0; i < 4; i++)
			{
				int dust = Dust.NewDust(target.position, target.width, target.height, 159);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = (float)120 * 0.015f;
				Main.dust[dust].velocity *= 1.5f;
			}

			if (modPlayer.shamanOrbUnique != ShamanOrbUnique.GRANDTHUNDERBIRD)
			{
				modPlayer.shamanOrbUnique = ShamanOrbUnique.GRANDTHUNDERBIRD;
				modPlayer.orbCountUnique = 0;
			}
			modPlayer.orbCountUnique++;
			//modPlayer.sendOrbCountPackets();

			if (modPlayer.orbCountUnique == 5)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 79, 0f, 0f, Mod.Find<ModProjectile>("ThunderScepterOrb").Type, 0, 0, Projectile.owner, 0f, 0f);

			if (player.FindBuffIndex(Mod.Find<ModBuff>("ShamanicBaubles").Type) > -1 && modPlayer.orbCountUnique < 5)
			{
				modPlayer.orbCountUnique += 5;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 79, 0f, 0f, Mod.Find<ModProjectile>("ThunderScepterOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
				player.ClearBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type);
				//modPlayer.sendOrbCountPackets();
			}

			if (modPlayer.orbCountUnique == 20)
			{
				modPlayer.orbCountUnique = 0;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 79, 0f, 5f, Mod.Find<ModProjectile>("ThunderScepterOrbProj").Type, 0, 0.0f, Projectile.owner, 0.0f, 0.0f);
			}
		}
	}
}