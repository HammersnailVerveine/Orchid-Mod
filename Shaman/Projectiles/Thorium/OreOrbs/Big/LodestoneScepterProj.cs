using Terraria.ID;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Big
{
	public class LodestoneScepterProj : OrchidModShamanProjectile
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
			DisplayName.SetDefault("Lodestone Bolt");
		}

		public override void AI()
		{
			int dust = Dust.NewDust(Projectile.position, 1, 1, 90);
			Main.dust[dust].velocity /= 10f;
			Main.dust[dust].scale = 1f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = false;
			int dust2 = Dust.NewDust(Projectile.position, 1, 1, 182);
			Main.dust[dust2].velocity /= 1f;
			Main.dust[dust2].scale = 0.8f;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].noLight = true;

			if (Main.rand.Next(2) == 0)
			{
				int dust3 = Dust.NewDust(Projectile.position, 1, 1, 38);
				Main.dust[dust3].velocity /= 1.5f;
				Main.dust[dust3].scale = 1f;
				Main.dust[dust3].noGravity = true;
				Main.dust[dust3].noLight = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 90);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null && Main.rand.Next(5) == 0)
			{
				target.AddBuff((thoriumMod.Find<ModBuff>("Sunder").Type), 4 * 60);
			}

			if (modPlayer.shamanOrbBig != ShamanOrbBig.LODESTONE)
			{
				modPlayer.shamanOrbBig = ShamanOrbBig.LODESTONE;
				modPlayer.orbCountBig = 0;
			}
			modPlayer.orbCountBig++;

			if (modPlayer.orbCountBig == 3)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 30, player.position.Y - 30, 0f, 0f, Mod.Find<ModProjectile>("LodestoneOrb").Type, 0, 0, Projectile.owner, 0f, 0f);

				if (player.FindBuffIndex(Mod.Find<ModBuff>("ShamanicBaubles").Type) > -1)
				{
					modPlayer.orbCountBig += 3;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 15, player.position.Y - 38, 0f, 0f, Mod.Find<ModProjectile>("LodestoneOrb").Type, 1, 0, Projectile.owner, 0f, 0f);
					player.ClearBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type);
				}
			}
			if (modPlayer.orbCountBig == 6)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 15, player.position.Y - 38, 0f, 0f, Mod.Find<ModProjectile>("LodestoneOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 9)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 40, 0f, 0f, Mod.Find<ModProjectile>("LodestoneOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 12)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 15, player.position.Y - 38, 0f, 0f, Mod.Find<ModProjectile>("LodestoneOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 15)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 30, player.position.Y - 30, 0f, 0f, Mod.Find<ModProjectile>("LodestoneOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig > 15)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, 0f, 0f, Mod.Find<ModProjectile>("LodestoneScepterExplosion").Type, Projectile.damage * 3, 0.0f, Projectile.owner, 0.0f, 0.0f);
				SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
				modPlayer.orbCountBig = -3;
			}
		}
	}
}