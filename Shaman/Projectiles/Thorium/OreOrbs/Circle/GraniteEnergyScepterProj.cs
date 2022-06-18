using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Circle
{
	public class GraniteEnergyScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 35;
			Projectile.scale = 1f;
			Projectile.tileCollide = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Surge");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			for (int i = 0; i < 2; i++)
			{
				float x = Projectile.position.X - Projectile.velocity.X / 10f * (float)i;
				float y = Projectile.position.Y - Projectile.velocity.Y / 10f * (float)i;
				int index2 = Dust.NewDust(new Vector2(x, y), Projectile.width, Projectile.height, 172, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].alpha = Projectile.alpha;
				Main.dust[index2].scale = (float)Main.rand.Next(70, 110) * 0.013f;
				Main.dust[index2].velocity *= 0.0f;
				Main.dust[index2].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 172);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 3f;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				target.AddBuff((thoriumMod.Find<ModBuff>("GraniteSurge").Type), 3 * 60);
			}

			if (Main.LocalPlayer.FindBuffIndex(Mod.Find<ModBuff>("GraniteAura").Type) > -1 || Main.LocalPlayer.FindBuffIndex(Mod.Find<ModBuff>("SpiritualBurst").Type) > -1)
			{
				return;
			}

			if (modPlayer.shamanOrbCircle != ShamanOrbCircle.GRANITE)
			{
				modPlayer.shamanOrbCircle = ShamanOrbCircle.GRANITE;
				modPlayer.orbCountCircle = 0;
			}
			modPlayer.orbCountCircle++;

			if (modPlayer.orbCountCircle == 1)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 100, 0f, 0f, Mod.Find<ModProjectile>("GraniteEnergyScepterOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			}

			if (modPlayer.orbCountCircle == 2)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 110, player.position.Y + 10, 0f, 0f, Mod.Find<ModProjectile>("GraniteEnergyScepterOrb").Type, 0, 0, Projectile.owner, 0f, 0f);


			if (modPlayer.orbCountCircle == 3)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y + 120, 0f, 0f, Mod.Find<ModProjectile>("GraniteEnergyScepterOrb").Type, 0, 0, Projectile.owner, 0f, 0f);


			if (modPlayer.orbCountCircle == 4)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 110, player.position.Y + 10, 0f, 0f, Mod.Find<ModProjectile>("GraniteEnergyScepterOrb").Type, 0, 0, Projectile.owner, 0f, 0f);


			if (modPlayer.orbCountCircle == 5)
			{
				modPlayer.orbCountCircle = 0;

				player.AddBuff(Mod.Find<ModBuff>("GraniteAura").Type, 60 * 30);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 100, 0f, 0f, Mod.Find<ModProjectile>("GraniteEnergyScepterOrbProj").Type, 1, 0, Projectile.owner, 0f, 0f);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 110, player.position.Y + 10, 0f, 0f, Mod.Find<ModProjectile>("GraniteEnergyScepterOrbProj").Type, 2, 0, Projectile.owner, 0f, 0f);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y + 120, 0f, 0f, Mod.Find<ModProjectile>("GraniteEnergyScepterOrbProj").Type, 3, 0, Projectile.owner, 0f, 0f);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 110, player.position.Y + 10, 0f, 0f, Mod.Find<ModProjectile>("GraniteEnergyScepterOrbProj").Type, 4, 0, Projectile.owner, 0f, 0f);
			}
		}
	}
}