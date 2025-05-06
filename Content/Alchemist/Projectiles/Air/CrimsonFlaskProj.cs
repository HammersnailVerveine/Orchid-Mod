using Microsoft.Xna.Framework;
using OrchidMod.Common.Global.Projectiles;
using OrchidMod.Common.ModObjects;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Projectiles.Air
{
	public class CrimsonFlaskProj : OrchidModAlchemistProjectile
	{
		private int sporeType = 127;
		private int sporeDamage = 0;

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 18;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 60;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Main.projFrames[Projectile.type] = 3;
			this.catalytic = true;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Mushroom");
		}

		public override void AI()
		{
			Player player = Main.player[Main.myPlayer];
			OrchidPlayer modPlayer = player.GetModPlayer<OrchidPlayer>();
			Projectile.velocity *= 0.95f;

			if (!this.Initialized)
			{
				this.Initialized = true;
				Projectile.frame = Main.rand.Next(3);
			}

			if (Projectile.timeLeft % 10 == 0) Projectile.frame = Projectile.frame == 2 ? 0 : Projectile.frame + 1;

			if (Projectile.ai[1] != 0f)
			{
				sporeType = (int)Projectile.ai[1];
				this.sporeDamage = (int)Projectile.ai[0];
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			this.Bounce(oldVelocity);
			return false;
		}

		public override void Catalyze(Player player, Projectile projectile, OrchidGlobalProjectile modProjectile)
		{
			if (this.Initialized) projectile.Kill();
		}

		public override void OnKill(int timeLeft)
		{
			int range = 50;
			OrchidModProjectile.spawnDustCircle(Projectile.Center, sporeType, (int)(range / 3), 5, true, 1.25f, 1f, 4f, true, true, false, 0, 0, true);
			spawnGenericExplosion(Projectile, Projectile.damage, Projectile.knockBack, range * 3, 2, false, true);

			if (Main.rand.NextBool(2))
			{
				int spawnProj = 0;
				int spawnProj2 = 0;

				switch (sporeType)
				{
					case 6:
						spawnProj = ProjectileType<Content.Alchemist.Projectiles.Fire.FireSporeProj>();
						spawnProj2 = ProjectileType<Content.Alchemist.Projectiles.Fire.FireSporeProjAlt>();
						break;
					case 59:
						spawnProj = ProjectileType<Content.Alchemist.Projectiles.Water.WaterSporeProj>();
						spawnProj2 = ProjectileType<Content.Alchemist.Projectiles.Water.WaterSporeProjAlt>();
						break;
					case 61:
						spawnProj = ProjectileType<Content.Alchemist.Projectiles.Nature.NatureSporeProj>();
						spawnProj2 = ProjectileType<Content.Alchemist.Projectiles.Nature.NatureSporeProjAlt>();
						break;
					case 63:
						spawnProj = ProjectileType<Content.Alchemist.Projectiles.Air.AirSporeProj>();
						spawnProj2 = ProjectileType<Content.Alchemist.Projectiles.Air.AirSporeProjAlt>();
						break;
					default:
						break;
				}

				if (spawnProj != 0)
				{
					Vector2 vel = (new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
					int newSpore = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, vel.X, vel.Y, spawnProj, this.sporeDamage, 0f, Projectile.owner);
					Main.projectile[newSpore].localAI[1] = 1f;
					vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, vel.X, vel.Y, spawnProj2, 0, 0f, Projectile.owner);
				}
			}
		}
	}
}