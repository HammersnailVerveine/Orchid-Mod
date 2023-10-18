using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Big
{
	public class MagmaScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 20;
			Projectile.scale = 1f;
			Projectile.alpha = 128;
			AIType = ProjectileID.Bullet;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Aquaite Bolt");
		}

		public override void AI()
		{

			for (int i = 0; i < 2; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
				Main.dust[dust].velocity = Projectile.velocity / 3;
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = false;
			}

			int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 35);
			Main.dust[dust2].velocity /= 1f;
			Main.dust[dust2].scale = 1.2f;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].noLight = true;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
			}
		}

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			for (int i = 0; i < 20; i++)
			{
				double deg = (i * (36 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);

				float posX = Projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - Projectile.width / 2 + Projectile.velocity.X + 4;
				float posY = Projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - Projectile.height / 2 + Projectile.velocity.Y + 4;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.5f;
				Main.dust[index2].noGravity = true;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null && Main.rand.Next(2) == 0)
			{
				target.AddBuff((thoriumMod.Find<ModBuff>("Singed").Type), 2 * 60);
			}

			if (modPlayer.shamanOrbBig != ShamanOrbBig.MAGMA)
			{
				modPlayer.shamanOrbBig = ShamanOrbBig.MAGMA;
				modPlayer.orbCountBig = 0;
			}
			modPlayer.orbCountBig++;

			if (modPlayer.orbCountBig == 2)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 30, player.position.Y - 30, 0f, 0f, Mod.Find<ModProjectile>("MagmaOrb").Type, 0, 0, Projectile.owner, 0f, 0f);

				if (player.FindBuffIndex(Mod.Find<ModBuff>("ShamanicBaubles").Type) > -1)
				{
					modPlayer.orbCountBig += 2;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 15, player.position.Y - 38, 0f, 0f, Mod.Find<ModProjectile>("MagmaOrb").Type, 1, 0, Projectile.owner, 0f, 0f);
					player.ClearBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type);
				}
			}
			if (modPlayer.orbCountBig == 4)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 15, player.position.Y - 38, 0f, 0f, Mod.Find<ModProjectile>("MagmaOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 6)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 40, 0f, 0f, Mod.Find<ModProjectile>("MagmaOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 8)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 15, player.position.Y - 38, 0f, 0f, Mod.Find<ModProjectile>("MagmaOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 10)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 30, player.position.Y - 30, 0f, 0f, Mod.Find<ModProjectile>("MagmaOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig > 10)
			{

				int dmg = (int)player.GetDamage<ShamanDamageClass>().ApplyTo(50);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, 0f, 0f, Mod.Find<ModProjectile>("MagmaScepterProjExplosion").Type, dmg, 0.0f, Projectile.owner, 0.0f, 0.0f);
				SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

				spawnDustCircle(6, 75);
				spawnDustCircle(6, 50);
				spawnDustCircle(6, 25);

				modPlayer.orbCountBig = -3;
			}
		}
	}
}