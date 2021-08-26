using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class DesertCardProj : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acorn");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 10;
			projectile.height = 12;
			projectile.friendly = true;
			projectile.aiStyle = 1;
			projectile.timeLeft = 180;
			this.gamblingChipChance = 5;
		}

		public override void SafeAI()
		{
			if (projectile.timeLeft == 180)
			{
				int dustType = 31;
				Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
				Main.dust[Dust.NewDust(pos, projectile.width, projectile.height, dustType)].velocity *= 0.25f;
			}
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int dustType = 31;
			Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
			int rand = 50 - (modPlayer.gamblerLuckySprout ? 10 : 0);
			if (Main.rand.Next(rand) == 0 && projectile.ai[1] != 1f && projectile.owner == Main.myPlayer)
			{
				Vector2 vel = (new Vector2(0f, -3f).RotatedBy(MathHelper.ToRadians(10)));
				int projType = ProjectileType<Gambler.Projectiles.DesertCardProjAlt>();
				bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
				OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(projectile.position.X, projectile.position.Y, vel.X, vel.Y, projType, projectile.damage, projectile.knockBack, projectile.owner), dummy);
				for (int i = 0; i < 5; i++)
				{
					Main.dust[Dust.NewDust(pos, projectile.width, projectile.height, dustType)].velocity *= 0.25f;
				}
			}
			Main.dust[Dust.NewDust(pos, projectile.width, projectile.height, dustType)].velocity *= 0.25f;
		}
	}
}