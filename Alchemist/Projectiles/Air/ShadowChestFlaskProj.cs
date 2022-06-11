using Microsoft.Xna.Framework;
using System;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Air
{
	public class ShadowChestFlaskProj : OrchidModAlchemistProjectile
	{
		Vector2 startPosition = new Vector2(0, 0);
		Vector2 startVelocity = new Vector2(0, 0);

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alchemical Shadow");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.scale = 1f;
			Projectile.alpha = 172;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 120;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 120)
			{
				this.startPosition = Projectile.position;
				this.startVelocity = Projectile.velocity;
			}
			if (Main.rand.Next(2) == 0)
			{
				float x = Projectile.position.X - Projectile.velocity.X / 10f;
				float y = Projectile.position.Y - Projectile.velocity.Y / 10f;
				int rand = Main.rand.Next(2) == 0 ? 21 : 70;
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, rand, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 100, default(Color), 3.5f);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].velocity *= 0.5f;
				Main.dust[dust].noGravity = true;
			}

			Vector2 newMove = Projectile.Center - this.startPosition;
			float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
			if (distanceTo < 10f && Projectile.timeLeft < 100)
			{
				Projectile.Kill();
			}

			Projectile.rotation += 0.1f;
			Projectile.velocity -= this.startVelocity * 0.02f;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 70);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].velocity *= 3f;
			}

			if (Main.player[Projectile.owner].HasBuff(BuffType<Alchemist.Buffs.DemonBreathFlaskBuff>()) && Projectile.ai[1] != 2f)
			{
				Vector2 vel = Projectile.velocity.RotatedBy(MathHelper.ToRadians(45));
				int newProjInt = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, vel.X, vel.Y, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner);
				Projectile newProj = Main.projectile[newProjInt];
				newProj.ai[1] = 2f;
				newProj.netUpdate = true;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			OrchidModAlchemistNPC modTarget = target.GetGlobalNPC<OrchidModAlchemistNPC>();
			modTarget.alchemistAir = 600;
		}
	}
}