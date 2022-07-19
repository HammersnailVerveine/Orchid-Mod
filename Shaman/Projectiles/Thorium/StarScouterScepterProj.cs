using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class StarScouterScepterProj : OrchidModShamanProjectile
	{
		public bool IsGreen { get { return Projectile.ai[0] == 1; } set { Projectile.ai[0] = value.ToInt(); } }

		public ref float GreenLightProgress => ref Projectile.ai[1];

		//private SimpleTrail _trail; [SP]

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orbital Mine");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true; // ...
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 120;

		}

		public override void OnSpawn(IEntitySource source)
		{
			/* [SP]
			_trail = new SimpleTrail(target: Projectile, length: 16 * 6, width: (progress) => 18, color: (progress) => Color.Lerp(new Color(198, 61, 255), new Color(107, 61, 255), progress) * (1 - progress));
			_trail.SetMaxPoints(15);
			_trail.SetEffectTexture(OrchidAssets.GetExtraTexture(4).Value);

			PrimitiveTrailSystem.NewTrail(_trail);
			*/
			Projectile.friendly = false;
			this.IsGreen = false;
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Sin(Projectile.timeLeft * 0.05f) * 0.5f;

			if (!this.IsGreen)
			{
				if (Projectile.timeLeft <= 100) Projectile.velocity *= 0.9f;
				if (Projectile.velocity.Length() <= 0.2f)
				{
					Projectile.friendly = true;
					Projectile.velocity = Vector2.Zero;
					//_trail.StartDissolving(); [SP]

					this.IsGreen = true;
					this.GreenLightProgress = 0.85f;
				}

				Lighting.AddLight(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0.53f * 0.35f, 0.12f * 0.35f, 0.35f);
				return;
			}

			if (Projectile.timeLeft == 1) this.UpdatePreKill();
			if (this.GreenLightProgress > 0.5f) this.GreenLightProgress -= 0.035f;

			Lighting.AddLight(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0.44f * 0.35f, 0.92f * 0.35f, 0f);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (!this.IsGreen)
			{
				if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
				if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;

				SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
			}
			return false;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			Projectile.timeLeft = 1;
		}

		public void UpdatePreKill()
		{
			Vector2 center = Projectile.Center;
			Projectile.width = 110;
			Projectile.height = 110;
			Projectile.Center = center;
			Projectile.penetrate = -1;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item91, Projectile.Center);

			Player player = Main.player[Projectile.owner];
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int nbBonds = modPlayer.GetNbShamanicBonds();

			// Spawn Dusts
			{
				Action<Dust> spawnDustAction = new Action<Dust>((dust) =>
				{
					dust.velocity *= 0.05f;
					dust.fadeIn = 1f;
					dust.scale = 0.8f;
					dust.noGravity = true;
				});

				OrchidUtils.SpawnDustCircle(center: Projectile.Center, type: 62, radius: 55, count: 35, onSpawn: spawnDustAction);
				OrchidUtils.SpawnDustCircle(center: Projectile.Center, type: 62, radius: 42, count: 27, onSpawn: spawnDustAction);

				for (int i = 0; i < 3; i++)
				{
					float rotation = i / (float)3 * MathHelper.TwoPi + (float)Main.time;
					for (int j = 0; j < 3; j++)
					{
						var dust = Dust.NewDustPerfect(Projectile.Center + new Vector2(43 + j * 4, 0).RotatedBy(rotation), 62);
						spawnDustAction?.Invoke(dust);
					}
				}
			}

			if (nbBonds >= 3)
			{
				for (int i = 0; i < 3; i++)
				{
					Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(30));
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<StarScouterScepterProjAlt>(), (int)(Projectile.damage * 0.70), 0.0f, player.whoAmI, 0.0f, 0.0f);
				}
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);
			Color color = Projectile.GetAlpha(Lighting.GetColor((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, Color.White));
			Texture2D texture = OrchidAssets.GetExtraTexture(11).Value;

			SetSpriteBatch(spriteBatch: spriteBatch, blendState: BlendState.Additive);
			{
				spriteBatch.Draw(texture, drawPos, null, new Color(0.44f, 0.92f, 0f) * 0.5f, 0f, texture.Size() * 0.5f, Projectile.scale * this.GreenLightProgress, SpriteEffects.None, 0f);
			}
			SetSpriteBatch(spriteBatch: spriteBatch);

			texture = TextureAssets.Projectile[Projectile.type].Value;
			float val = (float)Math.Sin(Main.GlobalTimeWrappedHourly + MathHelper.Pi);

			for (int i = 0; i < 4; i++)
			{
				spriteBatch.Draw(texture, drawPos + new Vector2(5.5f, 0).RotatedBy(Main.GlobalTimeWrappedHourly + MathHelper.PiOver2 * i) * val, null, color * 0.22f, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);

			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);
			Rectangle rect = new Rectangle(0, 20 * this.IsGreen.ToInt(), 20, 20);
			Vector2 origin = new Vector2(10, 10);

			float val = (float)Math.Sin(Main.GlobalTimeWrappedHourly);
			for (int i = 0; i < 4; i++)
			{ // [SP] spriteBatch.Draw --> Main.spriteBatch.Draw ...
				Main.spriteBatch.Draw(OrchidAssets.GetExtraTexture(1).Value, drawPos + new Vector2(3.5f, 0).RotatedBy(Main.GlobalTimeWrappedHourly + MathHelper.PiOver2 * i) * val, rect, Color.White * 0.35f, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(OrchidAssets.GetExtraTexture(1).Value, drawPos, rect, Color.White, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
		}
	}
}
