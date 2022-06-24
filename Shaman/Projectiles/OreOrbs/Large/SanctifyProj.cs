using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Large
{
	public class SanctifyProj : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Light Magic");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 34;
			Projectile.aiStyle = 44;
			Projectile.friendly = true;
			Projectile.scale = 1f;
			Main.projFrames[Projectile.type] = 4;
			Projectile.timeLeft = 100;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();

			Projectile.rotation += 0.1f;

			if (Projectile.timeLeft == 100)
			{
				spawnDustCircle(169, 30);
				spawnDustCircle(169, 20);
				spawnDustCircle(169, 10);
			}

			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 % 15 == 0)
			{
				Projectile.frame = (Projectile.frame + 1) > 4 ? 0 : Projectile.frame + 1;
			}

			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 169, Projectile.velocity.X / 2, Projectile.velocity.Y / 2);
			Main.dust[dust].scale = 1.2f;
			Main.dust[dust].noGravity = true;
		}

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			for (int i = 0; i < 10; i++)
			{
				double deg = (i * (72 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);

				float posX = Projectile.Center.X - (int)(Math.Cos(rad) * distToCenter);
				float posY = Projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter);

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity = Projectile.velocity * 20 / distToCenter;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.5f;
				Main.dust[index2].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			spawnDustCircle(169, 30);
			spawnDustCircle(169, 20);
			spawnDustCircle(169, 10);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			if (modPlayer.shamanOrbLarge != ShamanOrbLarge.SANCTIFY)
			{
				modPlayer.shamanOrbLarge = ShamanOrbLarge.SANCTIFY;
				modPlayer.orbCountLarge = 0;
			}
			modPlayer.orbCountLarge++;

			float orbX = player.position.X + player.width / 2;
			float orbY = player.position.Y;

			if (player.FindBuffIndex(Mod.Find<ModBuff>("ShamanicBaubles").Type) > -1 && modPlayer.orbCountLarge < 5)
			{
				modPlayer.orbCountLarge += 5;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), orbX - 43, orbY - 38, 0f, 0f, Mod.Find<ModProjectile>("SanctifyOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
				player.ClearBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type);
			}

			if (modPlayer.orbCountLarge == 5)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), orbX - 43, orbY - 38, 0f, 0f, Mod.Find<ModProjectile>("SanctifyOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountLarge == 10)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), orbX - 30, orbY - 48, 0f, 0f, Mod.Find<ModProjectile>("SanctifyOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountLarge == 15)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), orbX - 15, orbY - 53, 0f, 0f, Mod.Find<ModProjectile>("SanctifyOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountLarge == 20)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), orbX, orbY - 55, 0f, 0f, Mod.Find<ModProjectile>("SanctifyOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountLarge == 25)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), orbX + 15, orbY - 53, 0f, 0f, Mod.Find<ModProjectile>("SanctifyOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountLarge == 30)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), orbX + 30, orbY - 48, 0f, 0f, Mod.Find<ModProjectile>("SanctifyOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountLarge == 35)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), orbX + 43, orbY - 38, 0f, 0f, Mod.Find<ModProjectile>("SanctifyOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountLarge > 35)
			{
				int dmg = (int)(30 * player.GetModPlayer<OrchidShaman>().shamanDamage);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), orbX - 43, orbY - 38, -3f, -5f, Mod.Find<ModProjectile>("SanctifyOrbHoming").Type, dmg, 0f, Projectile.owner, 0f, 0f);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), orbX - 30, orbY - 48, -2f, -5f, Mod.Find<ModProjectile>("SanctifyOrbHoming").Type, dmg, 0f, Projectile.owner, 0f, 0f);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), orbX - 15, orbY - 53, -1f, -5f, Mod.Find<ModProjectile>("SanctifyOrbHoming").Type, dmg, 0f, Projectile.owner, 0f, 0f);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), orbX, orbY - 55, 0f, -5f, Mod.Find<ModProjectile>("SanctifyOrbHoming").Type, dmg, 0f, Projectile.owner, 0f, 0f);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), orbX + 15, orbY - 53, 1f, -5f, Mod.Find<ModProjectile>("SanctifyOrbHoming").Type, dmg, 0f, Projectile.owner, 0f, 0f);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), orbX + 30, orbY - 48, 2f, -5f, Mod.Find<ModProjectile>("SanctifyOrbHoming").Type, dmg, 0f, Projectile.owner, 0f, 0f);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), orbX + 43, orbY - 38, 3f, -5f, Mod.Find<ModProjectile>("SanctifyOrbHoming").Type, dmg, 0f, Projectile.owner, 0f, 0f);
				modPlayer.orbCountLarge = 0;
			}
		}
	}
}
