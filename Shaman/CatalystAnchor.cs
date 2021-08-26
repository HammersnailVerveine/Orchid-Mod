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
		public bool active = true;
		
		public override void AltSetDefaults() {
            projectile.width = 2;
            projectile.height = 2;
            projectile.friendly = false;
			projectile.tileCollide = false;
            projectile.aiStyle = 0;
			projectile.timeLeft = 60;
            projectile.penetrate = -1;
			projectile.netImportant = true;
			projectile.alpha = 255;
		}
		
		public override void AI() {
			if (active) {
				Player player = Main.player[projectile.owner];
				OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
				bool netUpdate = false;
				
				switch (modPlayer.shamanCatalystType) {
					case ShamanCatalystType.IDLE:
						projectile.rotation = projectile.velocity.X * 0.035f;
						projectile.rotation = projectile.rotation > 0.35f ? 0.35f : projectile.rotation;
						projectile.rotation = projectile.rotation < - 0.35f ? - 0.35f : projectile.rotation;
						break;
					case ShamanCatalystType.AIM:							
						// Vector2 aimVector = mousePosition - projectile.Center;
						// projectile.rotation = aimVector.ToRotation();
						// projectile.direction = projectile.spriteDirection;
						break;
					case ShamanCatalystType.ROTATE:
						projectile.rotation += 0.05f;
						break;
				}

				if (Main.myPlayer == projectile.owner) {
					Vector2 mousePosition = Main.MouseWorld;
					int mouseDir = mousePosition.X < player.Center.X ? -1 : 1;
					int mouseUnderValid = mousePosition.Y > player.Center.Y + 30 && Collision.CanHit(player.Center, 0, 0, player.Center + (mousePosition - player.Center), 0, 0) ? 2 : 0;
					bool tooFar = mousePosition.X < player.Center.X - 500 || mousePosition.X > player.Center.X + 500;
					
					if (mousePosition.X < player.Center.X + 50 && mousePosition.X > player.Center.X - 50) {
						netUpdate = projectile.ai[1] != (0f + mouseUnderValid * 2); ; 
						projectile.ai[1] = (0f + mouseUnderValid * 2);
					} else if ((mousePosition.Y < player.position.Y - 50 || mouseUnderValid != 0) && !tooFar) {
						netUpdate = projectile.ai[1] != (1f + mouseUnderValid) * mouseDir; 
						projectile.ai[1] = (1f + mouseUnderValid) * mouseDir;
					} else {
						netUpdate = projectile.ai[1] != 2f * mouseDir; 
						projectile.ai[1] = 2f * mouseDir;
					}
				}
				
				modPlayer.shamanCatalystPosition = projectile.Center;
				Vector2 angleVector = new Vector2(0f, - 60f).RotatedBy(MathHelper.ToRadians(45 * projectile.ai[1]));
				angleVector.X *= 0.8f;
				angleVector.Y += 10;
				Vector2 aimedLocation = player.position + angleVector;
				
				Vector2 newMove = aimedLocation - projectile.position;
				float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
				if (distanceTo > 1000f) {
					projectile.position = aimedLocation;
					projectile.netUpdate = true;
				} else if (distanceTo > 0.01f) {
					newMove.Normalize();
					float vel = ((distanceTo * 0.075f) + (player.velocity.Length() / 2)) * (player.HasBuff(ModContent.BuffType<Shaman.Buffs.ShamanicEmpowerment>()) ? 1.5f : 1f);
					vel = vel > 50f ? 50f : vel;
					newMove *= vel;
					projectile.velocity = newMove;
				} else {
					if (projectile.velocity.Length() > 0f) {
						projectile.velocity *= 0f;
					}
				}
				
				if (netUpdate) {
					projectile.netUpdate = true;
				}
				
				if (modPlayer.shamanCatalyst > 0) {
					projectile.timeLeft = 30;
				} else {
					active = false;
				}
			} else {
				projectile.velocity *= 0.95f;
			}
		}
		
		public override void Kill(int timeLeft) {
			for (int i = 0 ; i < 3 ; i ++) {
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.25f;
			}
		}
		
		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor){
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			Color color = Lighting.GetColor((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f), Color.White);
				
			if (modPlayer.shamanCatalystTexture != null) {
				Vector2 drawPosition = projectile.Center - Main.screenPosition;
				SpriteEffects spriteEffect = projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Texture2D texture = modPlayer.shamanCatalystTexture;
				projectile.width = texture.Width;
				projectile.height = texture.Height;

				spriteBatch.Draw(texture, drawPosition, null, color, projectile.rotation, projectile.Size * 0.5f, projectile.scale, spriteEffect, 0f);
			}
			
			if (modPlayer.shamanDrawWeapon > 0) {
				Texture2D texture = Main.itemTexture[player.HeldItem.type];
				if (texture != null) {	
					float diagonalBy2 = ((float)Math.Sqrt(texture.Width * texture.Width + texture.Height * texture.Width) / 2);
					Vector2 drawPosition = player.position + new Vector2((player.width / 2) - diagonalBy2 + (12f * player.direction), 0f) - Main.screenPosition;
					drawPosition += player.direction == 1 ? Vector2.Zero : new Vector2(diagonalBy2, - diagonalBy2);
					drawPosition.Y += 10f;
					SpriteEffects spriteEffect = player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
					spriteBatch.Draw(texture, drawPosition, null, color, -0.8f * player.direction, Vector2.Zero, 1f, spriteEffect, 0f);
					//spriteBatch.Draw(texture, drawPosition, color);
				}	
			}
			
			return false;
		}
    }
}