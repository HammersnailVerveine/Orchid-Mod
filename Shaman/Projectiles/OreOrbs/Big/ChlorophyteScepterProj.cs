using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Big
{
	public class ChlorophyteScepterProj : OrchidModShamanProjectile
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
			DisplayName.SetDefault("Chlorophyte Bolt");
		}

		public override void AI()
		{
			int dust = Dust.NewDust(Projectile.Center, 1, 1, 61);
			Main.dust[dust].velocity /= 10f;
			Main.dust[dust].scale = 1f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = false;
			int dust2 = Dust.NewDust(Projectile.Center, 1, 1, 44);
			Main.dust[dust2].velocity /= 1f;
			Main.dust[dust2].scale = 1.3f;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].noLight = true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 44);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			if (modPlayer.shamanOrbBig != ShamanOrbBig.CHLOROPHYTE)
			{
				modPlayer.shamanOrbBig = ShamanOrbBig.CHLOROPHYTE;
				modPlayer.orbCountBig = 0;
			}
			modPlayer.orbCountBig++;
			//modPlayer.sendOrbCountPackets();

			if (modPlayer.orbCountBig == 3)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 30, player.position.Y - 30, 0f, 0f, Mod.Find<ModProjectile>("ChlorophyteOrb").Type, 0, 0, Projectile.owner, 0f, 0f);

				if (player.FindBuffIndex(Mod.Find<ModBuff>("ShamanicBaubles").Type) > -1)
				{
					modPlayer.orbCountBig += 3;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 15, player.position.Y - 38, 0f, 0f, Mod.Find<ModProjectile>("ChlorophyteOrb").Type, 1, 0, Projectile.owner, 0f, 0f);
					player.ClearBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type);
					//modPlayer.sendOrbCountPackets();
				}
			}
			if (modPlayer.orbCountBig == 6)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 19, player.position.Y - 38, 0f, 0f, Mod.Find<ModProjectile>("ChlorophyteOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 9)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 2, player.position.Y - 40, 0f, 0f, Mod.Find<ModProjectile>("ChlorophyteOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 12)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 15, player.position.Y - 38, 0f, 0f, Mod.Find<ModProjectile>("ChlorophyteOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 15)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 32, player.position.Y - 30, 0f, 0f, Mod.Find<ModProjectile>("ChlorophyteOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig > 15)
			{
				Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.Center.Y - 50, perturbedSpeed.X, perturbedSpeed.Y, 228, Projectile.damage, 0.0f, player.whoAmI, 0.0f, 0.0f);

				perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 16, player.Center.Y - 48, perturbedSpeed.X, perturbedSpeed.Y, 228, Projectile.damage, 0.0f, player.whoAmI, 0.0f, 0.0f);

				perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 30, player.Center.Y - 40, perturbedSpeed.X, perturbedSpeed.Y, 228, Projectile.damage, 0.0f, player.whoAmI, 0.0f, 0.0f);

				perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 16, player.Center.Y - 48, perturbedSpeed.X, perturbedSpeed.Y, 228, Projectile.damage, 0.0f, player.whoAmI, 0.0f, 0.0f);

				perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 30, player.Center.Y - 40, perturbedSpeed.X, perturbedSpeed.Y, 228, Projectile.damage, 0.0f, player.whoAmI, 0.0f, 0.0f);
				modPlayer.orbCountBig = -3;
			}
		}
	}
}