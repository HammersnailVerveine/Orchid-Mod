using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Unique
{
	public class ThunderScepterOrbProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 34;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 100;
			Projectile.scale = 1f;
			Main.projFrames[Projectile.type] = 6;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thunder Bolt");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (player.GetModPlayer<OrchidModPlayer>().timer120 % 10 == 0)
				Projectile.frame++;
			if (Projectile.frame == 6)
				Projectile.frame = 0;

			if (Main.rand.Next(3) == 0)
			{
				Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
				int dust = Dust.NewDust(pos, Projectile.width, Projectile.height, 229, 0f, 0f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1f;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 229);
				Main.dust[dust].velocity = Projectile.velocity / 2;
				Main.dust[dust].noGravity = true;
			}

			Player player = Main.player[Projectile.owner];
			int dmg = (int)(35 * player.GetModPlayer<OrchidModPlayer>().shamanDamage);

			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y + 10, 5f, 0f, Mod.Find<ModProjectile>("ThunderScepterTornado").Type, dmg, 0.0f, Projectile.owner, 0.0f, 0.0f);
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y + 10, -5f, 0f, Mod.Find<ModProjectile>("ThunderScepterTornado").Type, dmg, 0.0f, Projectile.owner, 0.0f, 0.0f);
		}
	}
}