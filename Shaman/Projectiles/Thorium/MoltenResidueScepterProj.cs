using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class MoltenResidueScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 90;
			Projectile.scale = 1f;
			AIType = ProjectileID.Bullet;
			Projectile.alpha = 128;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Molten Bolt");
		}

		public override void AI()
		{
			Projectile.velocity *= 1.031f;
			int index1 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
			Main.dust[index1].velocity *= 0.2f;
			Main.dust[index1].fadeIn = 1f;
			Main.dust[index1].scale = 1.5f;
			Main.dust[index1].noGravity = true;

			if (Projectile.timeLeft % 10 == 0 || Projectile.timeLeft % 10 == 8)
			{
				spawnDustCircle(6, 12);
			}
			else if (Projectile.timeLeft % 10 == 3)
			{
				spawnDustCircle(6, 16);
			}
		}

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			for (int i = 0; i < 20; i++)
			{
				double deg = (i * (36 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);

				float posX = Projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) + Projectile.velocity.X - 4;
				float posY = Projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) + Projectile.velocity.Y - 4;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity = Projectile.velocity / 2;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = Projectile.velocity.X == 0 ? 1.5f : (float)Main.rand.Next(70, 110) * 0.013f;
				Main.dust[index2].noGravity = true;
			}
		}

		public override void OnKill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int nbBonds = modPlayer.GetNbShamanicBonds();

			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, 0f, 0f, Mod.Find<ModProjectile>("MoltenResidueScepterProjExplosion").Type, Projectile.damage + (5 * nbBonds), 0.0f, Projectile.owner, 0.0f, 0.0f);
			SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

			float oldVelocityX = 0 + Projectile.velocity.X / 2;
			float oldVelocityY = 0 + Projectile.velocity.Y / 2;
			Projectile.velocity.X *= 0;
			Projectile.velocity.Y *= 0;

			for (int i = 1; i < nbBonds + 1; i++)
			{
				spawnDustCircle(6, 15 * i);
			}

			Projectile.velocity.X = oldVelocityX;
			Projectile.velocity.Y = oldVelocityY;

			Projectile.position.X += Projectile.velocity.X;
			Projectile.position.Y += Projectile.velocity.Y;
			spawnDustCircle(6, nbBonds * 15 + 15);
			Projectile.position.X += Projectile.velocity.X * 3;
			Projectile.position.Y += Projectile.velocity.Y * 3;
			spawnDustCircle(6, nbBonds * 10 + 10);
			Projectile.position.X += Projectile.velocity.X * 3;
			Projectile.position.Y += Projectile.velocity.Y * 3;
			spawnDustCircle(6, nbBonds * 5 + 5);
			Projectile.velocity *= 0;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer) { }
	}
}