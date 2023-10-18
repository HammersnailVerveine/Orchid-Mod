using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Unique
{
	public class SolarPebbleScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 35;
			Projectile.scale = 1f;
			AIType = ProjectileID.Bullet;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Solar Burst");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 0.010f, 0.010f, 0f);
			for (int i = 0; i < 2; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X / 2, Projectile.velocity.Y / 2);
				Main.dust[dust].velocity = Projectile.velocity;
				Main.dust[dust].scale = 0.8f + ((Projectile.timeLeft) / 45f) * 1.8f;
				Main.dust[dust].noGravity = true;
			}

			if (Main.rand.Next(5) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 269);
				Main.dust[dust].scale = 0.8f + ((Projectile.timeLeft) / 45f) * 0.9f;
				Main.dust[dust].velocity.X += Projectile.velocity.X / 2;
				Main.dust[dust].velocity.Y += Projectile.velocity.Y / 2;
			}

			if (Projectile.timeLeft == 35)
			{
				Projectile.ai[0] = (((float)Main.rand.Next(10) / 10f) - 0.5f);
			}
			Projectile.velocity *= 1.03f;
			Vector2 projectileVelocity = (new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(Projectile.ai[0])));
			Projectile.velocity = projectileVelocity;
			Projectile.netUpdate = true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			return true;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 13; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 269);
				Main.dust[dust].velocity.X += Projectile.velocity.X / 2;
				Main.dust[dust].velocity.Y += Projectile.velocity.Y / 2;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				target.AddBuff((thoriumMod.Find<ModBuff>("Melting").Type), 2 * 60);
			}

			if (modPlayer.shamanOrbUnique != ShamanOrbUnique.ECLIPSE)
			{
				modPlayer.shamanOrbUnique = ShamanOrbUnique.ECLIPSE;
				modPlayer.orbCountUnique = 0;
			}
			modPlayer.orbCountUnique++;

			if (modPlayer.orbCountUnique == 1)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 79, 0f, 0f, Mod.Find<ModProjectile>("SolarPebbleScepterOrb").Type, 0, 0, Projectile.owner, 0f, 0f);

			if (player.FindBuffIndex(Mod.Find<ModBuff>("ShamanicBaubles").Type) > -1 && modPlayer.orbCountUnique < 5)
			{
				modPlayer.orbCountUnique += 5;
				player.ClearBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type);
			}

			if (modPlayer.orbCountUnique == 18)
			{

				for (int i = 0; i < 10; i++)
				{
					Vector2 projectileVelocity = (new Vector2(8f, 0f).RotatedByRandom(MathHelper.ToRadians(360)));
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 79, projectileVelocity.X, projectileVelocity.Y, Mod.Find<ModProjectile>("SolarPebbleScepterOrbProj").Type, 0, 0, Projectile.owner, 0f, 0f);
				}

				for (int i = 0; i < 3; i++)
				{
					Vector2 projectileVelocity = (new Vector2(10f, 0f).RotatedByRandom(MathHelper.ToRadians(360)));
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 79, projectileVelocity.X, projectileVelocity.Y, Mod.Find<ModProjectile>("SolarPebbleScepterOrbProjAlt").Type, Projectile.damage * 5, 0, Projectile.owner, 0f, 0f);
				}

				modPlayer.orbCountUnique = 0;
			}
		}
	}
}