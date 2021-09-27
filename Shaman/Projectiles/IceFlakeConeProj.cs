using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles
{
	public class IceFlakeConeProj : OrchidModShamanProjectile
	{
		public static readonly Color EffectColor = new Color(106, 210, 255);

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Flake");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.friendly = true;
			projectile.penetrate = 20;
			projectile.timeLeft = 300;
			projectile.extraUpdates = 1;
		}

		public override void OnSpawn()
		{
			var trail = new Content.Trails.RoundedTrail(target: projectile, length: 16 * 7, width: (p) => 16 * (1 - p * 0.8f), color: (p) => Color.Lerp(EffectColor, new Color(11, 26, 138), p) * (1 - p) * 0.4f, blendState: BlendState.Additive, smoothness: 15);
			trail.SetDissolveSpeed(0.35f);
			trail.SetEffectTexture(OrchidHelper.GetExtraTexture(5));
			PrimitiveTrailSystem.NewTrail(trail);
		}

		public override void AI()
		{
			this.VanillaAI_003(freeFlightTime: 35, turnSpeed: 0.27f);

			projectile.rotation += 0.6f;

			if (Main.rand.NextBool(7))
			{
				var dust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 67, projectile.velocity.X, projectile.velocity.Y, 100, new Color(), 1f)];
				dust.noGravity = true;
				dust.velocity *= 0.5f;
				dust.noLight = true;
				dust.scale *= Main.rand.NextFloat(0.6f, 1f);
			}

			Lighting.AddLight(projectile.Center, EffectColor.ToVector3() * 0.25f);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			this.VanillaAI_003__Hit();
			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y, 1, 1f, 0f);
			return false;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			this.VanillaAI_003__Hit();

			if (Main.rand.Next(5) == 0)
			{
				target.AddBuff(44, 360);
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			this.VanillaAI_003__Hit();
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var drawPos = projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);

			SetSpriteBatch(spriteBatch: spriteBatch, blendState: BlendState.Additive);
			{
				var texture = OrchidHelper.GetExtraTexture(14);
				spriteBatch.Draw(texture, drawPos, null, EffectColor * 0.2f, projectile.timeLeft * 0.1f, texture.Size() * 0.5f, projectile.scale * 0.65f, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos, null, EffectColor * 0.4f, projectile.timeLeft * 0.2f, texture.Size() * 0.5f, projectile.scale * 0.5f, SpriteEffects.None, 0);

				texture = Main.projectileTexture[projectile.type];
				spriteBatch.Draw(texture, drawPos, null, new Color(220, 220, 220, 230), projectile.rotation, texture.Size() * 0.5f, projectile.scale, SpriteEffects.None, 0);
			}
			SetSpriteBatch(spriteBatch: spriteBatch);

			return false;
		}

		private void VanillaAI_003(float freeFlightTime = 30f, float turnSpeed = 0.4f)
		{
			if (projectile.ai[0] == 0f)
			{
				projectile.ai[1] += 1f;

				if (projectile.ai[1] >= freeFlightTime)
				{
					projectile.ai[0] = 1f;
					projectile.ai[1] = 0f;
					projectile.netUpdate = true;
				}
			}
			else
			{
				projectile.tileCollide = false;

				float num42 = 9f;
				float num43 = turnSpeed;

				Vector2 vector2 = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
				var catalystPos = Main.player[projectile.owner].GetOrchidPlayer().shamanCatalystPosition;

				float num44 = catalystPos.X - vector2.X;
				float num45 = catalystPos.Y - vector2.Y;
				float num46 = (float)Math.Sqrt(num44 * num44 + num45 * num45);

				if (num46 > 3000f)
				{
					projectile.Kill();
				}

				num46 = num42 / num46;
				num44 *= num46;
				num45 *= num46;

				if (projectile.velocity.X < num44)
				{
					projectile.velocity.X = projectile.velocity.X + num43;

					if (projectile.velocity.X < 0f && num44 > 0f)
					{
						projectile.velocity.X = projectile.velocity.X + num43;
					}
				}
				else if (projectile.velocity.X > num44)
				{
					projectile.velocity.X = projectile.velocity.X - num43;

					if (projectile.velocity.X > 0f && num44 < 0f)
					{
						projectile.velocity.X = projectile.velocity.X - num43;
					}
				}

				if (projectile.velocity.Y < num45)
				{
					projectile.velocity.Y = projectile.velocity.Y + num43;

					if (projectile.velocity.Y < 0f && num45 > 0f)
					{
						projectile.velocity.Y = projectile.velocity.Y + num43;
					}
				}
				else if (projectile.velocity.Y > num45)
				{
					projectile.velocity.Y = projectile.velocity.Y - num43;

					if (projectile.velocity.Y > 0f && num45 < 0f)
					{
						projectile.velocity.Y = projectile.velocity.Y - num43;
					}
				}

				if (Main.myPlayer == projectile.owner)
				{
					Rectangle rectangle = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
					Rectangle value2 = new Rectangle((int)catalystPos.X - 9, (int)catalystPos.Y - 9, 18, 18);

					if (rectangle.Intersects(value2))
					{
						projectile.Kill();
					}
				}

				projectile.rotation += 0.4f * projectile.direction;
			}
		}

		private void VanillaAI_003__Hit()
		{
			if (projectile.ai[0] == 0f)
			{
				projectile.velocity.X = -projectile.velocity.X;
				projectile.velocity.Y = -projectile.velocity.Y;
				projectile.netUpdate = true;
			}
			projectile.ai[0] = 1f;
		}
	}
}