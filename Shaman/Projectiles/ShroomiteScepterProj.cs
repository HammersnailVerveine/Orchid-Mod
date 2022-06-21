using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Globals.NPCs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
	public class ShroomiteScepterProj : OrchidModShamanProjectile
	{
		public Texture2D zoneTexture;

		public int maxRadius;

		public ref float RadiusProgress => ref Projectile.ai[0];
		public ref float NbBonds => ref Projectile.ai[1];

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shroomy Totem");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = false;
			Projectile.timeLeft = 1800;
			Projectile.scale = 1f;
			Projectile.hide = true; // DrawBehind()
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
			Player owner = Main.player[Projectile.owner];

			Projectile.velocity.X = 0f;
			Projectile.velocity.Y += +0.2f;
			if (Projectile.velocity.Y > 16f) Projectile.velocity.Y = 16f;

			// DPS Zone
			if (Projectile.timeLeft % 120 == 0)
			{
				foreach (var target in Main.npc)
				{
					if (!target.active || !target.CanBeChasedBy() || Vector2.Distance(Projectile.Center, target.Center) > pr) continue;

					target.StrikeNPCNoInteraction(Projectile.damage, 0f, 0);
					if (NbBonds >= 3)
					{
						OrchidModGlobalProjectile modProjectile = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
						target.GetGlobalNPC<OrchidGlobalNPC>().ShamanShroom = 300;
						OrchidModShamanHelper.addShamanicEmpowerment(modProjectile.shamanEmpowermentType, owner, owner.GetModPlayer<OrchidModPlayer>(), Mod);
					}
					target.netUpdate = true;
				}
			}

			// Buff Zone
			if (NbBonds >= 5)
			{
				Player buffPlayer = Main.player[Main.myPlayer]; // I hope it works

				if (Vector2.Distance(buffPlayer.Center, Projectile.Center) <= pr) buffPlayer.AddBuff(ModContent.BuffType<Buffs.ShroomHeal>(), 1);
			}

			Color color = new Color(36, 129, 234) * 0.4f;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			drawCacheProjsBehindNPCs.Add(index);
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawPosition = Projectile.position - Main.screenPosition + Projectile.Size * 0.5f;

			// Light Effect
			SetSpriteBatch(spriteBatch: spriteBatch, blendState: BlendState.Additive);
			{
				Texture2D radialGradient = OrchidAssets.GetExtraTexture(11).Value;
				spriteBatch.Draw(radialGradient, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + new Vector2(0, -11), null, new Color(36, 129, 234) * 0.35f, 0f, radialGradient.Size() * 0.5f, 0.75f * Projectile.scale, SpriteEffects.None, 0f);
			}

			// Aura
			var effect = OrchidAssets.GetEffect("ShroomiteScepter");
			SetSpriteBatch(spriteBatch: spriteBatch, spriteSortMode: SpriteSortMode.Immediate, blendState: BlendState.Additive, samplerState: SamplerState.PointClamp, effect: effect);
			{
				this.SetEffectParameters(ref effect);
				if (zoneTexture != null) spriteBatch.Draw(zoneTexture, drawPosition, null, Color.White, Projectile.rotation, zoneTexture.Size() * 0.5f, 2f * MathHelper.SmoothStep(0, 1, RadiusProgress), SpriteEffects.None, 0f);
			}

			SetSpriteBatch(spriteBatch: spriteBatch);
			return false; // Let's draw the projectile ourselves
		}

		public override void PostDraw(Color lightColor)
		{
			Vector2 offset = new Vector2(-7, -22);
			Vector2 drawPosition = Projectile.position - Main.screenPosition + Projectile.Size * 0.5f;
			Color color = Lighting.GetColor((int)(Projectile.position.X / 16f), (int)(Projectile.position.Y / 16f), Color.White);
			SpriteEffects spriteEffect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPosition + offset, null, color, Projectile.rotation, Projectile.Size * 0.5f, Projectile.scale, spriteEffect, 0f);
			SetSpriteBatch(spriteBatch: spriteBatch, blendState: BlendState.Additive);
			{
				spriteBatch.Draw(ModContent.GetTexture("OrchidMod/Glowmasks/ShroomiteScepterProj_Glowmask"), drawPosition + offset, null, new Color(250, 250, 250, 150), Projectile.rotation, Projectile.Size * 0.5f, Projectile.scale, spriteEffect, 0f);
			}
			SetSpriteBatch(spriteBatch: spriteBatch);
		}

		public override bool OnTileCollide(Vector2 oldVelocity) => false;

		public override void Kill(int timeLeft)
		{
			zoneTexture = null;

			for (int i = 0; i < 10; i++)
			{
				var dust = Main.dust[Dust.NewDust(Projectile.position + new Vector2(-8, -26), TextureAssets.Projectile[Projectile.type].Value.Width, TextureAssets.Projectile[Projectile.type].Value.Height, ModContent.DustType<Content.Dusts.StrangeSmokeDust>())];
				dust.velocity = new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.25f, 0.75f));
				dust.alpha = 200;
				dust.scale = Main.rand.NextFloat(2f, 3.5f);
				dust.rotation = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
			}
		}

		private void SetEffectParameters(ref Effect effect)
		{
			effect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly);
			effect.Parameters["radius"].SetValue(maxRadius);
			effect.Parameters["thickness"].SetValue(2.5f);
			effect.Parameters["color"].SetValue(new Vector4(73, 76, 219, 85) / 255f * RadiusProgress);
			effect.Parameters["color2"].SetValue(new Vector4(73, 110, 219, 85) / 255f * RadiusProgress);
		}
	}
}