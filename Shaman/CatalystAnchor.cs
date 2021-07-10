using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod;
using OrchidMod.Effects;

namespace OrchidMod.Shaman
{
    public class CatalystAnchor : OrchidModProjectile
    {
		public Texture2D drawnTexture = null;
		
		public override void SafeSetDefaults()
		{
            projectile.width = 4;
            projectile.height = 4;
            projectile.friendly = false;
			projectile.tileCollide = false;
            projectile.aiStyle = 0;
			projectile.timeLeft = 12960000;
            projectile.penetrate = -1;
			projectile.netImportant = true;
			//projectile.alpha = 255;
			
		}
		
		public override void AI() {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			modPlayer.shamanCatalystPosition = projectile.Center;
			Vector2 aimedLocation = player.Center;
			aimedLocation.X += 40 * player.direction;
			aimedLocation.X += projectile.width / 2;
			aimedLocation.Y += projectile.height / 2;
			
			Vector2 newMove = aimedLocation - projectile.Center;
			float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
			if (distanceTo > 1000f) {
				projectile.position = aimedLocation;
				projectile.netUpdate = true;
			} else if (distanceTo > 0.1f) {
				newMove.Normalize();
				float vel = ((distanceTo * 0.075f) + (player.velocity.Length() / 2));
				vel = vel > 50f ? 50f : vel;
				newMove *= vel;
				projectile.velocity = newMove;
			} else {
				if (projectile.velocity.Length() > 0f) {
					projectile.velocity *= 0f;
				}
			}
			
			if (modPlayer.shamanCatalyst < 1) {
				projectile.Kill();
			}
		}
		
		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor){
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (modPlayer.shamanCatalystTexture != null) {
				Vector2 drawPosition = projectile.position - Main.screenPosition;
				Color color = Lighting.GetColor((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f), Color.White);
				SpriteEffects spriteEffect = projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

				spriteBatch.Draw(modPlayer.shamanCatalystTexture, drawPosition, null, color, projectile.rotation, projectile.Size * 0.5f, projectile.scale, spriteEffect, 0f);
				/*EffectsManager.SetSpriteBatchEffectSettings(spriteBatch, blendState: BlendState.Additive);
				{
					spriteBatch.Draw(ModContent.GetTexture("OrchidMod/Glowmasks/ShroomiteScepterProj_Glowmask"), drawPosition + offset, null, new Color(250, 250, 250, 150), projectile.rotation, projectile.Size * 0.5f, projectile.scale, spriteEffect, 0f);
				}*/
				EffectsManager.SetSpriteBatchVanillaSettings(spriteBatch);
				return false;
			}
			return true;
		}
    }
}