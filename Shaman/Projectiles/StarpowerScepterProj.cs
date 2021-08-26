using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Effects;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
	public class StarpowerScepterProj : OrchidModShamanProjectile
	{
		private Color mainColor = Color.White;
		private float startRotation = 0f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star");

			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 15;
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 120;
			projectile.penetrate = -1; // Don't delete it, pls
		}

		public override void OnSpawn()
		{
			// I hate it
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int newCrit = 10 * OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod) + modPlayer.shamanCrit + player.inventory[player.selectedItem].crit;
			OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			modProjectile.baseCritChance = newCrit;

			switch (Main.rand.Next(3))
			{
				case 1:
					mainColor = new Color(231, 120, 255, 100);
					break;
				case 2:
					mainColor = new Color(120, 247, 255, 100);
					break;
				default:
					mainColor = new Color(215, 255, 120, 100);
					break;
			}

			projectile.ai[0] = 0.35f;
			projectile.ai[1] = 0; // Death Type
			startRotation = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.Length() + startRotation;
			projectile.velocity *= 0.95f;
			projectile.scale = MathHelper.SmoothStep(1.4f, 0f, projectile.ai[0]);

			if (projectile.timeLeft < 25 && projectile.ai[1] == 0) projectile.ai[1] = 2;
			if (projectile.ai[1] > 0) DeathFunction();
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			SetSpriteBatch(spriteBatch: spriteBatch, blendState: BlendState.Additive);
			{
				// Light Effect
				Texture2D lightTexture = Effects.EffectsManager.RadialGradientTexture;
				Vector2 drawPosition = projectile.Center + new Vector2(0, projectile.gfxOffY) - Main.screenPosition;
				spriteBatch.Draw(lightTexture, drawPosition, null, mainColor, projectile.rotation, lightTexture.Size() * 0.5f, projectile.scale * 0.5f, SpriteEffects.None, 0);

				// Lens Flare
				if (projectile.timeLeft < 15 && projectile.ai[1] == 2)
				{
					Texture2D texture = Effects.EffectsManager.LensFlareTexture;
					float size = MathHelper.SmoothStep(1, 0, Math.Abs(1f - projectile.timeLeft * 2 / 15)) * 0.9f;
					Color color = mainColor;
					color.A = 200;
					spriteBatch.Draw(texture, drawPosition, null, color, 0f, texture.Size() * 0.5f, size, SpriteEffects.None, 0);
				}

				// Trail
				for (int k = 0; k < projectile.oldPos.Length; k++)
				{
					drawPosition = projectile.oldPos[k] + projectile.Size * 0.5f - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
					float num = ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
					Color color = mainColor * num * 0.8f;
					spriteBatch.Draw(EffectsManager.ExtraTextures[0], drawPosition, null, color, projectile.oldRot[k], EffectsManager.ExtraTextures[0].Size() * .5f, projectile.scale * num, SpriteEffects.None, 0f);
				}
			}
			SetSpriteBatch(spriteBatch: spriteBatch);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawPosition = projectile.Center + new Vector2(0, projectile.gfxOffY) - Main.screenPosition;
			Texture2D texture = Main.projectileTexture[projectile.type];
			spriteBatch.Draw(texture, drawPosition, null, Lighting.GetColor((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16, Color.White), projectile.rotation, texture.Size() * .5f, projectile.scale, SpriteEffects.None, 0f);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.timeLeft = 24;
			projectile.ai[1] = 1;
			return false;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			projectile.timeLeft = 24;
			projectile.ai[1] = 1;
		}

		private void DeathFunction()
		{
			projectile.damage = 0;
			projectile.tileCollide = false;

			if (projectile.timeLeft < 25)
			{
				if (projectile.timeLeft >= 20) projectile.ai[0] -= 0.035f;
				else projectile.ai[0] += 0.06f;
				if (projectile.ai[0] > 1f) projectile.Kill();
			}
		}
	}
}