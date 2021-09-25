using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Effects;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
	public class ShroomiteScepterProj : OrchidModShamanProjectile
	{
		public Texture2D zoneTexture;

		public int maxRadius;

		public ref float RadiusProgress => ref projectile.ai[0];
		public ref float NbBonds => ref projectile.ai[1];

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shroomy Totem");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.friendly = false;
			projectile.timeLeft = 1800;
			projectile.scale = 1f;
			projectile.hide = true; // DrawBehind()
		}

		public override void OnSpawn()
		{
			maxRadius = 200 + (int)NbBonds * 50;
			RadiusProgress = 0;

			zoneTexture = new Texture2D(Main.graphics.GraphicsDevice, maxRadius, maxRadius);
		}

		public override void AI()
		{
			if (RadiusProgress < 1) RadiusProgress += 0.075f;
			else if (RadiusProgress != 1) RadiusProgress = 1;

			float pr = MathHelper.SmoothStep(0, maxRadius, RadiusProgress);
			Player owner = Main.player[projectile.owner];

			projectile.velocity.X = 0f;
			projectile.velocity.Y += +0.2f;
			if (projectile.velocity.Y > 16f) projectile.velocity.Y = 16f;

			// DPS Zone
			if (projectile.timeLeft % 120 == 0)
			{
				foreach (var target in Main.npc)
				{
					if (!target.active || !target.CanBeChasedBy() || Vector2.Distance(projectile.Center, target.Center) > pr) continue;

					target.StrikeNPCNoInteraction(projectile.damage, 0f, 0);
					if (NbBonds >= 3)
					{
						OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
						target.GetGlobalNPC<OrchidModGlobalNPC>().shamanShroom = 300;
						OrchidModShamanHelper.addShamanicEmpowerment(modProjectile.shamanEmpowermentType, owner, owner.GetModPlayer<OrchidModPlayer>(), mod);
					}
					target.netUpdate = true;
				}
			}

			// Buff Zone
			if (NbBonds >= 5)
			{
				Player buffPlayer = Main.player[Main.myPlayer]; // I hope it works

				if (Vector2.Distance(buffPlayer.Center, projectile.Center) <= pr) buffPlayer.AddBuff(ModContent.BuffType<Buffs.ShroomHeal>(), 1);
			}

			Color color = new Color(36, 129, 234) * 0.4f;
			Lighting.AddLight(projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
		}

		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			drawCacheProjsBehindNPCs.Add(index);
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawPosition = projectile.position - Main.screenPosition + projectile.Size * 0.5f;

			// Light Effect
			SetSpriteBatch(spriteBatch: spriteBatch, blendState: BlendState.Additive);
			{
				Texture2D radialGradient = OrchidHelper.GetExtraTexture(11);
				spriteBatch.Draw(radialGradient, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY) + new Vector2(0, -11), null, new Color(36, 129, 234) * 0.35f, 0f, radialGradient.Size() * 0.5f, 0.75f * projectile.scale, SpriteEffects.None, 0f);
			}

			// Aura
			var effect = EffectsManager.ShroomiteZoneEffect;
			SetSpriteBatch(spriteBatch: spriteBatch, spriteSortMode: SpriteSortMode.Immediate, blendState: BlendState.Additive, samplerState: SamplerState.PointClamp, effect: effect);
			{
				this.SetEffectParameters(ref effect);
				if (zoneTexture != null) spriteBatch.Draw(zoneTexture, drawPosition, null, Color.White, projectile.rotation, zoneTexture.Size() * 0.5f, 2f * MathHelper.SmoothStep(0, 1, RadiusProgress), SpriteEffects.None, 0f);
			}

			SetSpriteBatch(spriteBatch: spriteBatch);
			return false; // Let's draw the projectile ourselves
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 offset = new Vector2(-7, -22);
			Vector2 drawPosition = projectile.position - Main.screenPosition + projectile.Size * 0.5f;
			Color color = Lighting.GetColor((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f), Color.White);
			SpriteEffects spriteEffect = projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPosition + offset, null, color, projectile.rotation, projectile.Size * 0.5f, projectile.scale, spriteEffect, 0f);
			SetSpriteBatch(spriteBatch: spriteBatch, blendState: BlendState.Additive);
			{
				spriteBatch.Draw(ModContent.GetTexture("OrchidMod/Glowmasks/ShroomiteScepterProj_Glowmask"), drawPosition + offset, null, new Color(250, 250, 250, 150), projectile.rotation, projectile.Size * 0.5f, projectile.scale, spriteEffect, 0f);
			}
			SetSpriteBatch(spriteBatch: spriteBatch);
		}

		public override bool OnTileCollide(Vector2 oldVelocity) => false;

		public override void Kill(int timeLeft)
		{
			zoneTexture = null;

			for (int i = 0; i < 10; i++)
			{
				var dust = Main.dust[Dust.NewDust(projectile.position + new Vector2(-8, -26), Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height, ModContent.DustType<Content.Dusts.StrangeSmokeDust>())];
				dust.velocity = new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.25f, 0.75f));
				dust.alpha = 200;
				dust.scale = Main.rand.NextFloat(2f, 3.5f);
				dust.rotation = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
			}
		}

		private void SetEffectParameters(ref Effect effect)
		{
			effect.Parameters["time"].SetValue(Main.GlobalTime);
			effect.Parameters["radius"].SetValue(maxRadius);
			effect.Parameters["thickness"].SetValue(2.5f);
			effect.Parameters["color"].SetValue(new Vector4(73, 76, 219, 85) / 255f * RadiusProgress);
			effect.Parameters["color2"].SetValue(new Vector4(73, 110, 219, 85) / 255f * RadiusProgress);
		}
	}
}