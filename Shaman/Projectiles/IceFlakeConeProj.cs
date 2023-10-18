using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Common.Graphics;
using OrchidMod.Common.Graphics.Primitives;
using OrchidMod.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles
{
	public class IceFlakeConeProj : OrchidModShamanProjectile, IDrawOnDifferentLayers
	{
		public static readonly Color EffectColor = new(106, 210, 255);

		private PrimitiveStrip trail;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ice Flake");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.penetrate = 20;
			Projectile.timeLeft = 300;
			Projectile.extraUpdates = 1;
		}

		public override void OnSpawn(IEntitySource source)
		{
			trail = new PrimitiveStrip
			(
				width: progress => 16 * (1 - progress * 0.8f),
				color: progress => Color.Lerp(EffectColor, new Color(11, 26, 138), progress) * (1 - progress) * 0.4f,
				effect: new IPrimitiveEffect.Default(texture: OrchidAssets.GetExtraTexture(5), multiplyColorByAlpha: true),
				headTip: new IPrimitiveTip.Rounded(smoothness: 15),
				tailTip: null
			);
		}

		public override void AI()
		{
			VanillaAI_003(freeFlightTime: 35, turnSpeed: 0.27f);

			Projectile.rotation += 0.6f;

			if (Main.rand.NextBool(7))
			{
				var dust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 67, Projectile.velocity.X, Projectile.velocity.Y, 100, new Color(), 1f)];
				dust.noGravity = true;
				dust.velocity *= 0.5f;
				dust.noLight = true;
				dust.scale *= Main.rand.NextFloat(0.6f, 1f);
			}

			Lighting.AddLight(Projectile.Center, EffectColor.ToVector3() * 0.25f);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			VanillaAI_003__Hit();

			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
			return false;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			VanillaAI_003__Hit();

			if (Main.rand.NextBool(5))
			{
				target.AddBuff(44, 360);
			}
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			VanillaAI_003__Hit();
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor) => false;

		private void VanillaAI_003(float freeFlightTime = 30f, float turnSpeed = 0.4f)
		{
			if (Projectile.ai[0] == 0f)
			{
				Projectile.ai[1] += 1f;

				if (Projectile.ai[1] >= freeFlightTime)
				{
					Projectile.ai[0] = 1f;
					Projectile.ai[1] = 0f;
					Projectile.netUpdate = true;
				}
			}
			else
			{
				Projectile.tileCollide = false;

				float num42 = 9f;
				float num43 = turnSpeed;

				Vector2 vector2 = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
				var shaman = Main.player[Projectile.owner].GetModPlayer<OrchidShaman>();
				var catalystPos = shaman.ShamanCatalystPosition ?? Main.player[Projectile.owner].MountedCenter;

				float num44 = catalystPos.X - vector2.X;
				float num45 = catalystPos.Y - vector2.Y;
				float num46 = (float)Math.Sqrt(num44 * num44 + num45 * num45);

				if (num46 > 3000f)
				{
					Projectile.Kill();
				}

				num46 = num42 / num46;
				num44 *= num46;
				num45 *= num46;

				if (Projectile.velocity.X < num44)
				{
					Projectile.velocity.X = Projectile.velocity.X + num43;

					if (Projectile.velocity.X < 0f && num44 > 0f)
					{
						Projectile.velocity.X = Projectile.velocity.X + num43;
					}
				}
				else if (Projectile.velocity.X > num44)
				{
					Projectile.velocity.X = Projectile.velocity.X - num43;

					if (Projectile.velocity.X > 0f && num44 < 0f)
					{
						Projectile.velocity.X = Projectile.velocity.X - num43;
					}
				}

				if (Projectile.velocity.Y < num45)
				{
					Projectile.velocity.Y = Projectile.velocity.Y + num43;

					if (Projectile.velocity.Y < 0f && num45 > 0f)
					{
						Projectile.velocity.Y = Projectile.velocity.Y + num43;
					}
				}
				else if (Projectile.velocity.Y > num45)
				{
					Projectile.velocity.Y = Projectile.velocity.Y - num43;

					if (Projectile.velocity.Y > 0f && num45 < 0f)
					{
						Projectile.velocity.Y = Projectile.velocity.Y - num43;
					}
				}

				if (Main.myPlayer == Projectile.owner)
				{
					Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
					Rectangle value2 = new Rectangle((int)catalystPos.X - 9, (int)catalystPos.Y - 9, 18, 18);

					if (rectangle.Intersects(value2))
					{
						Projectile.Kill();
					}
				}

				Projectile.rotation += 0.4f * Projectile.direction;
			}
		}

		private void VanillaAI_003__Hit()
		{
			if (Projectile.ai[0] == 0f)
			{
				Projectile.velocity.X = -Projectile.velocity.X;
				Projectile.velocity.Y = -Projectile.velocity.Y;
				Projectile.netUpdate = true;
			}

			Projectile.ai[0] = 1f;
		}

		void IDrawOnDifferentLayers.DrawOnDifferentLayers(DrawSystem system)
		{
			var drawPosition = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
			var texture = OrchidAssets.GetExtraTexture(14);
			var drawData = new DefaultDrawData(texture.Value, drawPosition, null, EffectColor * 0.2f, Projectile.timeLeft * 0.1f, texture.Size() * 0.5f, Projectile.scale * 0.65f, SpriteEffects.None);
			system.AddToAdditive(DrawLayers.Dusts, drawData);

			drawData = new DefaultDrawData(texture.Value, drawPosition, null, EffectColor * 0.4f, Projectile.timeLeft * 0.2f, texture.Size() * 0.5f, Projectile.scale * 0.5f, SpriteEffects.None);
			system.AddToAdditive(DrawLayers.Dusts, drawData);

			texture = TextureAssets.Projectile[Projectile.type];
			drawData = new DefaultDrawData(texture.Value, drawPosition, null, new Color(220, 220, 220, 230), Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None);
			system.AddToAdditive(DrawLayers.Dusts, drawData);

			trail.UpdatePointsAsSimpleTrail(currentPosition: Projectile.Center, maxPoints: 25, maxLength: 16 * 7);
			system.AddToAlphaBlend(layer: DrawLayers.Tiles, data: trail);
		}
	}
}