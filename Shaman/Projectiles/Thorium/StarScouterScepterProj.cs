using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Content.Trails;
using OrchidMod.Effects;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class StarScouterScepterProj : OrchidModShamanProjectile
	{
		public bool IsGreen { get { return projectile.ai[0] == 1; } set { projectile.ai[0] = value.ToInt(); } }

		public ref float GreenLightProgress => ref projectile.ai[1];

		private SimpleTrail _trail;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orbital Mine");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 18;
			projectile.height = 18;
			projectile.friendly = true; // ...
			projectile.aiStyle = 0;
			projectile.timeLeft = 120;

		}

		public override void OnSpawn()
		{
			_trail = new SimpleTrail(target: projectile, length: 16 * 6, width: (progress) => 18, color: (progress) => Color.Lerp(new Color(198, 61, 255), new Color(107, 61, 255), progress) * (1 - progress));
			_trail.SetMaxPoints(15);
			_trail.SetEffectTexture(OrchidHelper.GetExtraTexture(4));

			PrimitiveTrailSystem.NewTrail(_trail);

			projectile.friendly = false;
			this.IsGreen = false;
		}

		public override void AI()
		{
			projectile.rotation = (float)Math.Sin(projectile.timeLeft * 0.05f) * 0.5f;

			if (!this.IsGreen)
			{
				if (projectile.timeLeft <= 100) projectile.velocity *= 0.9f;
				if (projectile.velocity.Length() <= 0.2f)
				{
					projectile.friendly = true;
					projectile.velocity = Vector2.Zero;
					_trail.StartDissolving();

					this.IsGreen = true;
					this.GreenLightProgress = 0.85f;
				}

				Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y), 0.53f * 0.35f, 0.12f * 0.35f, 0.35f);
				return;
			}

			if (projectile.timeLeft == 1) this.UpdatePreKill();
			if (this.GreenLightProgress > 0.5f) this.GreenLightProgress -= 0.035f;

			Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y), 0.44f * 0.35f, 0.92f * 0.35f, 0f);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (!this.IsGreen)
			{
				if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
				if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;

				Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 10);
			}
			return false;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			projectile.timeLeft = 1;
		}

		public void UpdatePreKill()
		{
			Vector2 center = projectile.Center;
			projectile.width = 110;
			projectile.height = 110;
			projectile.Center = center;
			projectile.penetrate = -1;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 91);

			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);

			// Spawn Dusts
			{
				Action<Dust> spawnDustAction = new Action<Dust>((dust) =>
				{
					dust.velocity *= 0.05f;
					dust.fadeIn = 1f;
					dust.scale = 0.8f;
					dust.noGravity = true;
				});

				OrchidHelper.SpawnDustCircle(center: projectile.Center, type: 62, radius: 55, count: 35, onSpawn: spawnDustAction);
				OrchidHelper.SpawnDustCircle(center: projectile.Center, type: 62, radius: 42, count: 27, onSpawn: spawnDustAction);

				for (int i = 0; i < 3; i++)
				{
					float rotation = i / (float)3 * MathHelper.TwoPi + (float)Main.time;
					for (int j = 0; j < 3; j++)
					{
						var dust = Dust.NewDustPerfect(projectile.Center + new Vector2(43 + j * 4, 0).RotatedBy(rotation), 62);
						spawnDustAction?.Invoke(dust);
					}
				}
			}

			if (nbBonds >= 3)
			{
				for (int i = 0; i < 3; i++)
				{
					Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(30));
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<StarScouterScepterProjAlt>(), (int)(projectile.damage * 0.70), 0.0f, player.whoAmI, 0.0f, 0.0f);
				}
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawPos = projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY);
			Color color = projectile.GetAlpha(Lighting.GetColor((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16, Color.White));
			Texture2D texture = OrchidHelper.GetExtraTexture(11);

			SetSpriteBatch(spriteBatch: spriteBatch, blendState: BlendState.Additive);
			{
				spriteBatch.Draw(texture, drawPos, null, new Color(0.44f, 0.92f, 0f) * 0.5f, 0f, texture.Size() * 0.5f, projectile.scale * this.GreenLightProgress, SpriteEffects.None, 0f);
			}
			SetSpriteBatch(spriteBatch: spriteBatch);

			texture = Main.projectileTexture[projectile.type];
			float val = (float)Math.Sin(Main.GlobalTime + MathHelper.Pi);

			for (int i = 0; i < 4; i++)
			{
				spriteBatch.Draw(texture, drawPos + new Vector2(5.5f, 0).RotatedBy(Main.GlobalTime + MathHelper.PiOver2 * i) * val, null, color * 0.22f, projectile.rotation, texture.Size() * 0.5f, projectile.scale, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(texture, drawPos, null, color, projectile.rotation, texture.Size() * 0.5f, projectile.scale, SpriteEffects.None, 0f);

			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawPos = projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY);
			Rectangle rect = new Rectangle(0, 20 * this.IsGreen.ToInt(), 20, 20);
			Vector2 origin = new Vector2(10, 10);

			float val = (float)Math.Sin(Main.GlobalTime);
			for (int i = 0; i < 4; i++)
			{
				spriteBatch.Draw(OrchidHelper.GetExtraTexture(1), drawPos + new Vector2(3.5f, 0).RotatedBy(Main.GlobalTime + MathHelper.PiOver2 * i) * val, rect, Color.White * 0.35f, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(OrchidHelper.GetExtraTexture(1), drawPos, rect, Color.White, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0f);
		}
	}
}
