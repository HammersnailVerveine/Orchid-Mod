using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Circle
{
	public class ReviverofSoulsProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 25;
			Projectile.scale = 0f;
			Projectile.tileCollide = false;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wisp");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			for (int i = 0; i < 1; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 175);
				Main.dust[dust].velocity /= 10f;
				Main.dust[dust].scale = 1f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = false;
				int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 172);
				Main.dust[dust2].velocity /= 1f;
				Main.dust[dust2].scale = 1f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].noLight = true;
			}

			if (Main.player[Projectile.owner].FindBuffIndex(Mod.Find<ModBuff>("SpiritualBurst").Type) > -1)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 175);
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = false;
				int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 172);
				Main.dust[dust2].velocity *= 2f;
				Main.dust[dust2].scale = 1.5f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].noLight = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 172);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 3f;
				if (Main.player[Projectile.owner].FindBuffIndex(Mod.Find<ModBuff>("SpiritualBurst").Type) > -1)
				{
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 2f;
				}
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			if (Main.LocalPlayer.FindBuffIndex(Mod.Find<ModBuff>("GraniteAura").Type) > -1 || Main.LocalPlayer.FindBuffIndex(Mod.Find<ModBuff>("SpiritualBurst").Type) > -1)
			{
				return;
			}

			if (modPlayer.shamanOrbCircle != ShamanOrbCircle.REVIVER)
			{
				modPlayer.shamanOrbCircle = ShamanOrbCircle.REVIVER;
				modPlayer.orbCountCircle = 0;
			}
			modPlayer.orbCountCircle++;

			if (Main.LocalPlayer.FindBuffIndex(Mod.Find<ModBuff>("SpiritualBurst").Type) > -1)
			{
				modPlayer.orbCountCircle++;

				if (modPlayer.shamanFireTimer > 0)
					modPlayer.shamanFireTimer = 60 * modPlayer.shamanBuffTimer;

				if (modPlayer.shamanEarthTimer > 0)
					modPlayer.shamanEarthTimer = 60 * modPlayer.shamanBuffTimer;

				if (modPlayer.shamanWaterTimer > 0)
					modPlayer.shamanWaterTimer = 60 * modPlayer.shamanBuffTimer;

				if (modPlayer.shamanSpiritTimer > 0)
					modPlayer.shamanSpiritTimer = 60 * modPlayer.shamanBuffTimer;
			}

			if (modPlayer.orbCountCircle == 3)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 110, 0f, 0f, Mod.Find<ModProjectile>("ReviverofSoulsFlame").Type, 0, 0, Projectile.owner, 0f, 0f);
			}

			if (modPlayer.orbCountCircle == 6)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 55, player.position.Y - 95, 0f, 0f, Mod.Find<ModProjectile>("ReviverofSoulsFlame").Type, 0, 0, Projectile.owner, 0f, 0f);

			if (modPlayer.orbCountCircle == 9)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 95, player.position.Y - 55, 0f, 0f, Mod.Find<ModProjectile>("ReviverofSoulsFlame").Type, 0, 0, Projectile.owner, 0f, 0f);

			if (modPlayer.orbCountCircle == 12)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 110, player.position.Y, 0f, 0f, Mod.Find<ModProjectile>("ReviverofSoulsFlame").Type, 0, 0, Projectile.owner, 0f, 0f);

			if (modPlayer.orbCountCircle == 15)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 95, player.position.Y + 55, 0f, 0f, Mod.Find<ModProjectile>("ReviverofSoulsFlame").Type, 0, 0, Projectile.owner, 0f, 0f);

			if (modPlayer.orbCountCircle == 18)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 55, player.position.Y + 95, 0f, 0f, Mod.Find<ModProjectile>("ReviverofSoulsFlame").Type, 0, 0, Projectile.owner, 0f, 0f);

			if (modPlayer.orbCountCircle == 21)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y + 110, 0f, 0f, Mod.Find<ModProjectile>("ReviverofSoulsFlame").Type, 0, 0, Projectile.owner, 0f, 0f);

			if (modPlayer.orbCountCircle == 24)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 55, player.position.Y + 95, 0f, 0f, Mod.Find<ModProjectile>("ReviverofSoulsFlame").Type, 0, 0, Projectile.owner, 0f, 0f);

			if (modPlayer.orbCountCircle == 27)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 95, player.position.Y + 55, 0f, 0f, Mod.Find<ModProjectile>("ReviverofSoulsFlame").Type, 0, 0, Projectile.owner, 0f, 0f);

			if (modPlayer.orbCountCircle == 30)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 110, player.position.Y, 0f, 0f, Mod.Find<ModProjectile>("ReviverofSoulsFlame").Type, 0, 0, Projectile.owner, 0f, 0f);

			if (modPlayer.orbCountCircle == 33)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 95, player.position.Y - 55, 0f, 0f, Mod.Find<ModProjectile>("ReviverofSoulsFlame").Type, 0, 0, Projectile.owner, 0f, 0f);

			if (modPlayer.orbCountCircle == 36)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 55, player.position.Y - 95, 0f, 0f, Mod.Find<ModProjectile>("ReviverofSoulsFlame").Type, 0, 0, Projectile.owner, 0f, 0f);

			if (modPlayer.orbCountCircle == 39)
				player.AddBuff(Mod.Find<ModBuff>("SpiritualBurst").Type, 60 * 15);

		}
	}
}