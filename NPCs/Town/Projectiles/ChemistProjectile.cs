using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.NPCs.Town.Projectiles
{
	public class ChemistProjectile : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 18;
			projectile.friendly = true;
			projectile.aiStyle = 2;
			projectile.timeLeft = 300;
			projectile.scale = 1f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alchemical Solution");
		}

		public override void Kill(int timeLeft)
		{
			int proj = ProjectileType<Alchemist.Projectiles.AlchemistSmoke1>();
			Vector2 vel = Vector2.Zero;

			int randR = Main.rand.Next(255);
			int randG = Main.rand.Next(255);
			int randB = Main.rand.Next(255);
			for (int i = 0; i < 3; i++)
			{
				vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj1 = Projectile.NewProjectile(projectile.position.X, projectile.position.Y, vel.X, vel.Y, proj, 0, 0f, Main.myPlayer);
				Main.projectile[smokeProj1].localAI[0] = randR;
				Main.projectile[smokeProj1].localAI[1] = randG;
				Main.projectile[smokeProj1].ai[1] = randB;
			}
			proj = ProjectileType<Alchemist.Projectiles.AlchemistSmoke2>();
			for (int i = 0; i < 2; i++)
			{
				vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj2 = Projectile.NewProjectile(projectile.position.X, projectile.position.Y, vel.X, vel.Y, proj, 0, 0f, Main.myPlayer);
				Main.projectile[smokeProj2].localAI[0] = randR;
				Main.projectile[smokeProj2].localAI[1] = randG;
				Main.projectile[smokeProj2].ai[1] = randB;
			}
			proj = ProjectileType<Alchemist.Projectiles.AlchemistSmoke3>();
			vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
			int smokeProj3 = Projectile.NewProjectile(projectile.position.X, projectile.position.Y, vel.X, vel.Y, proj, 0, 0f, Main.myPlayer);
			Main.projectile[smokeProj3].localAI[0] = randR;
			Main.projectile[smokeProj3].localAI[1] = randG;
			Main.projectile[smokeProj3].ai[1] = randB;
		}
	}
}