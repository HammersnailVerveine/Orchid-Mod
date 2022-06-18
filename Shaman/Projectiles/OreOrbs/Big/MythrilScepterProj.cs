using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Big
{
	public class MythrilScepterProj : OrchidModShamanProjectile
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
			DisplayName.SetDefault("Mythril Bolt");
		}

		public override void AI()
		{
			int dust = Dust.NewDust(Projectile.Center, 1, 1, 188);
			Main.dust[dust].velocity /= 1f;
			Main.dust[dust].scale = 1f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = false;
			int dust2 = Dust.NewDust(Projectile.Center, 1, 1, 245);
			Main.dust[dust2].velocity /= 10f;
			Main.dust[dust2].scale = 1f;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].noLight = true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 188);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale /= 2f;
				Main.dust[dust].velocity *= 4f;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (modPlayer.shamanOrbBig != ShamanOrbBig.MYTHRIL)
			{
				modPlayer.shamanOrbBig = ShamanOrbBig.MYTHRIL;
				modPlayer.orbCountBig = 0;
			}
			modPlayer.orbCountBig++;
			//modPlayer.sendOrbCountPackets();

			if (modPlayer.orbCountBig == 3)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 30, player.position.Y - 30, 0f, 0f, Mod.Find<ModProjectile>("MythrilOrb").Type, 0, 0, Projectile.owner, 0f, 0f);

				if (player.FindBuffIndex(Mod.Find<ModBuff>("ShamanicBaubles").Type) > -1)
				{
					modPlayer.orbCountBig += 3;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 15, player.position.Y - 38, 0f, 0f, Mod.Find<ModProjectile>("MythrilOrb").Type, 1, 0, Projectile.owner, 0f, 0f);
					player.ClearBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type);
					//modPlayer.sendOrbCountPackets();
				}
			}
			if (modPlayer.orbCountBig == 6)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 15, player.position.Y - 38, 0f, 0f, Mod.Find<ModProjectile>("MythrilOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 9)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 40, 0f, 0f, Mod.Find<ModProjectile>("MythrilOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 12)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 15, player.position.Y - 38, 0f, 0f, Mod.Find<ModProjectile>("MythrilOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 15)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 30, player.position.Y - 30, 0f, 0f, Mod.Find<ModProjectile>("MythrilOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig > 15)
			{

				player.AddBuff(Mod.Find<ModBuff>("MythrilDefense").Type, 60 * 30);
				modPlayer.orbCountBig = -3;
			}
		}
	}
}