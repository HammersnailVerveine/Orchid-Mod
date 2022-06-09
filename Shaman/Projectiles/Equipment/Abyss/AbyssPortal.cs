using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Equipment.Abyss
{
	public class AbyssPortal : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyssal Gate");
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = 172;
			Projectile.height = 139;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.timeLeft = 250;
			Projectile.penetrate = 100;
			Projectile.scale = 1f;
			Main.projFrames[Projectile.type] = 4;
			Projectile.timeLeft = 1000;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void AI()
		{
			Projectile.frameCounter++;

			if (Projectile.frameCounter > 5)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}
			if (Projectile.frame > 3)
			{
				Projectile.frame = 0;
			}

			if (Projectile.timeLeft == 1000)
			{
				for (int i = 0; i < 100; i++)
				{
					int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 172);
					Main.dust[dust2].velocity /= 10f;
					Main.dust[dust2].scale = 2.5f;
					Main.dust[dust2].noGravity = true;
					Main.dust[dust2].noLight = true;
				}
			}

			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 172);
			Main.dust[dust].velocity /= 10f;
			Main.dust[dust].scale = 1.5f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = true;

			Projectile.ai[1]++;
			if (Projectile.ai[1] >= 0)
			{
				Player player = Main.player[Projectile.owner];
				OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

				int dmg = (int)(150 * modPlayer.shamanDamage + 5E-06f);
				Projectile.NewProjectile(Projectile.position.X, Projectile.Center.Y, 0f, 14f, Mod.Find<ModProjectile>("AbyssPortalLaser").Type, dmg, 0f, Projectile.owner, Projectile.whoAmI, 0f);
				Projectile.ai[1] = -360;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 172, Projectile.velocity.X * 0f, Projectile.velocity.Y * 0f);
				Main.dust[dust].velocity /= 10f;
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
			}
			Main.player[Projectile.owner].GetModPlayer<OrchidModPlayer>().doubleTapCooldown = 0;
		}
	}
}
