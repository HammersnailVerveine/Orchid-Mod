using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod.Shaman;
using System.Collections.Generic;
using OrchidMod.Effects;

namespace OrchidMod.Shaman.Projectiles
{
	public class ShroomiteScepterProj : OrchidModShamanProjectile
	{
		public Texture2D zoneTexture;

		public ref float Radius => ref projectile.ai[0];
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

			/*empowermentType = 4;
			empowermentLevel = 4;*/
		}

		public override void OnSpawn()
		{
			Radius = 200 + (int)NbBonds * 50;

			zoneTexture = new Texture2D(Main.graphics.GraphicsDevice, (int)Radius * 2, (int)Radius * 2);
		}

		public override void AI()
		{
			Player owner = Main.player[projectile.owner];

			projectile.velocity.X = 0f;
			projectile.velocity.Y += +0.2f;
			if (projectile.velocity.Y > 16f) projectile.velocity.Y = 16f;

			// DPS Zone
			if (projectile.timeLeft % 120 == 0)
			{
				foreach (var target in Main.npc)
				{
					if (!target.active || !target.CanBeChasedBy() || Vector2.Distance(projectile.Center, target.Center) > Radius) continue;

					target.StrikeNPCNoInteraction(projectile.damage, 0f, 0);
					if (NbBonds >= 3)
					{
						target.GetGlobalNPC<OrchidModGlobalNPC>().shamanShroom = 300;
						OrchidModShamanHelper.addShamanicEmpowerment(4, 4, owner, owner.GetModPlayer<OrchidModPlayer>(), mod);
					}
					target.netUpdate = true;
				}
			}

			// Buff Zone
			if (NbBonds >= 5)
			{
				Player buffPlayer = Main.player[Main.myPlayer]; // I hope it works

				if (Vector2.Distance(buffPlayer.Center, projectile.Center) <= Radius) buffPlayer.AddBuff(ModContent.BuffType<Buffs.ShroomHeal>(), 1);
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
			Vector2 offset = new Vector2(-7, -22);
			Vector2 drawPosition = projectile.position - Main.screenPosition + projectile.Size * 0.5f;
			Color color = Lighting.GetColor((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f), Color.White);
			SpriteEffects spriteEffect = projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			// Light Effect
			EffectsManager.SetSpriteBatchEffectSettings(spriteBatch, blendState: BlendState.Additive);
			{
				Texture2D radialGradient = EffectsManager.RadialGradientTexture;
				spriteBatch.Draw(radialGradient, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY) + new Vector2(0, -11), null, new Color(36, 129, 234) * 0.35f, 0f, radialGradient.Size() * 0.5f, 0.75f * projectile.scale, SpriteEffects.None, 0f);
			}

			// Aura
			var effect = EffectsManager.ShroomiteZoneEffect;
			EffectsManager.SetSpriteBatchEffectSettings(spriteBatch, effect, blendState: BlendState.Additive);
			{
				effect.Parameters["time"].SetValue(Main.GlobalTime);
				effect.Parameters["radius"].SetValue(Radius);
				effect.Parameters["thickness"].SetValue(2.5f);
				effect.Parameters["color"].SetValue(new Vector4(73, 76, 219, 85) / 255f);

				if (zoneTexture != null) spriteBatch.Draw(zoneTexture, drawPosition, null, Color.White, projectile.rotation, zoneTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
			}
			EffectsManager.SetSpriteBatchVanillaSettings(spriteBatch);

			// Projectile and Glowmask
			spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPosition + offset, null, color, projectile.rotation, projectile.Size * 0.5f, projectile.scale, spriteEffect, 0f);
			EffectsManager.SetSpriteBatchEffectSettings(spriteBatch, blendState: BlendState.Additive);
			{
				spriteBatch.Draw(ModContent.GetTexture("OrchidMod/Glowmasks/ShroomiteScepterProj_Glowmask"), drawPosition + offset, null, new Color(250, 250, 250, 100), projectile.rotation, projectile.Size * 0.5f, projectile.scale, spriteEffect, 0f);
			}
			EffectsManager.SetSpriteBatchVanillaSettings(spriteBatch);

			return false; // Let's draw the projectile ourselves
		}

		public override bool OnTileCollide(Vector2 oldVelocity) => false;

		public override void Kill(int timeLeft)
		{
			zoneTexture = null;

			for (int i = 0; i < 10; i++)
			{
				var dust = Main.dust[Dust.NewDust(projectile.position + new Vector2(-8, -26), Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height, ModContent.DustType<Dusts.StrangeSmokeDust>())];
				dust.velocity = new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.25f, 0.75f));
				dust.alpha = 200;
				dust.scale = Main.rand.NextFloat(2f, 3.5f);
				dust.rotation = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
			}
		}
	}
}