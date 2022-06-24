using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class YewWoodScepterPortal : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 38;
			Projectile.height = 40;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 500;
			Projectile.scale = 1f;
			Main.projFrames[Projectile.type] = 4;
			Projectile.alpha = 32;
			Projectile.tileCollide = false;
			Projectile.penetrate = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Portal");
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (Projectile.timeLeft == 500)
			{
				for (int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale = 2f;
				}
			}

			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 % 7 == 0)
				Projectile.frame++;
			if (Projectile.frame == 4)
				Projectile.frame = 0;

			if (Main.rand.Next(3) == 0)
			{
				Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
				int dust = Dust.NewDust(pos, Projectile.width, Projectile.height, 27, 0f, 0f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1f;
			}

			if (Projectile.timeLeft % 60 == 0)
			{
				int dmg = (int)(18 * player.GetModPlayer<OrchidModPlayerShaman>().shamanDamage);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + Projectile.width / 2, Projectile.position.Y + Projectile.height / 2, 0f, 0f, Mod.Find<ModProjectile>("YewWoodScepterPortalProj").Type, dmg, 0.0f, Projectile.owner, 0.0f, 0.0f);
				Projectile.netUpdate = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 2f;
			}
		}
	}
}