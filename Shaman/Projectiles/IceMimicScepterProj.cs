using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles
{
	public class IceMimicScepterProj : OrchidModShamanProjectile
	{
		private float rotationSpeed = 0f;
		private bool faster = false;
		private int slowdelay = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Spear");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 52;
			Projectile.height = 52;
			Projectile.friendly = true;
			Projectile.timeLeft = 1200;
			Projectile.penetrate = 15;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			bool moving = !(Projectile.velocity.X < 1f && Projectile.velocity.X > -1f && Projectile.velocity.Y < 1f && Projectile.velocity.Y > -1f);
			this.slowdelay -= this.slowdelay > 0 ? 1 : 0;
			this.projectileTrail = Projectile.ai[1] == 0f || Projectile.ai[1] == 2f;

			if (Main.rand.Next(4) == 0)
			{
				int index = Dust.NewDust(Projectile.position - Projectile.velocity * 0.25f, Projectile.width, Projectile.height, 59, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(80, 110) * 0.013f);
				Main.dust[index].velocity *= 0.2f;
				Main.dust[index].scale *= 1.5f;
				Main.dust[index].noGravity = true;
			}

			if (!this.initialized)
			{
				if (Projectile.ai[1] == 3f)
				{
					this.faster = true;
				}
				Projectile.ai[1] = 2f;
				this.initialized = true;
			}

			if (Projectile.ai[1] == 0f || Projectile.ai[1] == 2f)
			{
				Projectile.velocity *= 0.95f;
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.direction = Projectile.spriteDirection;

				this.rotationSpeed = this.rotationSpeed == 0f ? this.rotationSpeed : 0f;
				if (!moving && this.slowdelay <= 0)
				{
					Projectile.velocity *= 0f;
					Projectile.ai[1] = 1f;
					Projectile.netUpdate = true;
				}
			}

			if (Projectile.ai[1] == 1f)
			{
				float spinValue = 0.005f;
				Projectile.rotation += this.rotationSpeed;
				this.rotationSpeed += this.faster ? spinValue * 2 : spinValue;
				if (this.rotationSpeed >= spinValue * 150)
				{
					Vector2 move = Vector2.Zero;
					float distance = 500f;
					bool targetFound = false;
					for (int w = 0; w < Main.npc.Length; w++)
					{
						if (Main.npc[w].active && !Main.npc[w].dontTakeDamage && !Main.npc[w].friendly && Main.npc[w].lifeMax > 5 && Main.npc[w].type != NPCID.TargetDummy)
						{
							Vector2 newMove = Main.npc[w].Center - Projectile.Center;
							float distancenewMove = (float)Math.Sqrt((newMove.X * newMove.X) + (newMove.Y * newMove.Y));
							if (distancenewMove < distance)
							{
								move = newMove;
								distance = distancenewMove;
								targetFound = true;
							}

							if (targetFound)
							{
								Projectile.ai[1] = 0f;
								move.Normalize();
								move *= 25f;
								Projectile.velocity = move;
								this.rotationSpeed = 0f;
								this.slowdelay = 90;
								Projectile.netUpdate = true;
							}
						}
					}

					this.rotationSpeed -= this.faster ? spinValue * 2 : spinValue;
				}
			}
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (target.friendly || target.dontTakeDamage)
			{
				return false;
			}
			OrchidModGlobalNPC modTarget = target.GetGlobalNPC<OrchidModGlobalNPC>();
			return modTarget.shamanSpearDamage <= 0;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X / 2;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y / 2;
			SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 50);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 13; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 59);
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			OrchidModGlobalNPC modTarget = target.GetGlobalNPC<OrchidModGlobalNPC>();
			modTarget.shamanSpearDamage = 60;
		}
	}
}