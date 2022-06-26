using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.NPCs.Town
{
	public class ChemistProjectile : ModProjectile
	{
		public override string Texture => OrchidAssets.ProjectilesPath + Name;

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.aiStyle = 2;
			Projectile.timeLeft = 300;
			Projectile.scale = 1f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alchemical Solution");
		}

		public override void Kill(int timeLeft)
		{
			int proj = ModContent.ProjectileType<AlchemistSmoke1>();
			Vector2 vel;

			int randR = Main.rand.Next(255);
			int randG = Main.rand.Next(255);
			int randB = Main.rand.Next(255);

			for (int i = 0; i < 3; i++)
			{
				vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj1 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel, proj, 0, 0f, Main.myPlayer);
				Main.projectile[smokeProj1].localAI[0] = randR;
				Main.projectile[smokeProj1].localAI[1] = randG;
				Main.projectile[smokeProj1].ai[1] = randB;
			}

			proj = ModContent.ProjectileType<AlchemistSmoke2>();

			for (int i = 0; i < 2; i++)
			{
				vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel, proj, 0, 0f, Main.myPlayer);
				Main.projectile[smokeProj2].localAI[0] = randR;
				Main.projectile[smokeProj2].localAI[1] = randG;
				Main.projectile[smokeProj2].ai[1] = randB;
			}

			proj = ModContent.ProjectileType<AlchemistSmoke3>();
			vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));

			int smokeProj3 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel, proj, 0, 0f, Main.myPlayer);

			Main.projectile[smokeProj3].localAI[0] = randR;
			Main.projectile[smokeProj3].localAI[1] = randG;
			Main.projectile[smokeProj3].ai[1] = randB;
		}
	}
}