using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Big
{
	public class CobaltScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 25;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Bolt");
		}

		public override void AI()
		{
			int dust = Dust.NewDust(Projectile.Center, 1, 1, 29);
			Main.dust[dust].velocity /= 10f;
			Main.dust[dust].scale = 1f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = false;
			int dust2 = Dust.NewDust(Projectile.Center, 1, 1, 206);
			Main.dust[dust2].velocity /= 1f;
			Main.dust[dust2].scale = 1.7f;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].noLight = true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 29);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (modPlayer.shamanOrbBig != ShamanOrbBig.COBALT)
			{
				modPlayer.shamanOrbBig = ShamanOrbBig.COBALT;
				modPlayer.orbCountBig = 0;
			}
			modPlayer.orbCountBig++;
			//modPlayer.sendOrbCountPackets();

			if (modPlayer.orbCountBig == 3)
			{
				Projectile.NewProjectile(player.Center.X - 30, player.position.Y - 30, 0f, 0f, Mod.Find<ModProjectile>("CobaltOrb").Type, 0, 0, Projectile.owner, 0f, 0f);

				if (player.FindBuffIndex(Mod.Find<ModBuff>("ShamanicBaubles").Type) > -1)
				{
					modPlayer.orbCountBig += 3;
					Projectile.NewProjectile(player.Center.X - 15, player.position.Y - 38, 0f, 0f, Mod.Find<ModProjectile>("CobaltOrb").Type, 1, 0, Projectile.owner, 0f, 0f);
					player.ClearBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type);
					//modPlayer.sendOrbCountPackets();
				}
			}
			if (modPlayer.orbCountBig == 6)
				Projectile.NewProjectile(player.Center.X - 15, player.position.Y - 38, 0f, 0f, Mod.Find<ModProjectile>("CobaltOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 9)
				Projectile.NewProjectile(player.Center.X, player.position.Y - 40, 0f, 0f, Mod.Find<ModProjectile>("CobaltOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 12)
				Projectile.NewProjectile(player.Center.X + 15, player.position.Y - 38, 0f, 0f, Mod.Find<ModProjectile>("CobaltOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 15)
				Projectile.NewProjectile(player.Center.X + 30, player.position.Y - 30, 0f, 0f, Mod.Find<ModProjectile>("CobaltOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig > 15)
			{
				int maxBufftimer = 60 * modPlayer.shamanBuffTimer;
				int toAdd = 180;
				modPlayer.shamanFireTimer = modPlayer.shamanFireTimer == 0 ? 0 : modPlayer.shamanFireTimer + toAdd > maxBufftimer ? maxBufftimer : modPlayer.shamanFireTimer + toAdd;
				modPlayer.shamanWaterTimer = modPlayer.shamanWaterTimer == 0 ? 0 : modPlayer.shamanWaterTimer + toAdd > maxBufftimer ? maxBufftimer : modPlayer.shamanWaterTimer + toAdd;
				modPlayer.shamanAirTimer = modPlayer.shamanAirTimer == 0 ? 0 : modPlayer.shamanAirTimer + toAdd > maxBufftimer ? maxBufftimer : modPlayer.shamanAirTimer + toAdd;
				modPlayer.shamanEarthTimer = modPlayer.shamanEarthTimer == 0 ? 0 : modPlayer.shamanEarthTimer + toAdd > maxBufftimer ? maxBufftimer : modPlayer.shamanEarthTimer + toAdd;
				modPlayer.orbCountBig = -3;
			}
		}
	}
}