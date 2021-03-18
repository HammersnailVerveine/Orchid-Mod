using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace OrchidMod.Alchemist.Projectiles.Air
{
    public class SunplateFlaskProj : OrchidModAlchemistProjectile
    {	
		public bool hasTarget = false;
		public Vector2 orbitPoint = Vector2.Zero;
	
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Air Spore");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
        } 
		
        public override void SafeSetDefaults() {
            projectile.width = 22;
            projectile.height = 22;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.alpha = 64;
			projectile.timeLeft = 900;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void OnSpawn() {
            orbitPoint = projectile.Center;
        }

        public override void AI() {		
			projectile.ai[1] = projectile.ai[1] + 1f + projectile.ai[0] >= 360f ? 0f : projectile.ai[1] + 1 + projectile.ai[0];
			projectile.rotation += 0.1f + (projectile.ai[0] / 30f);
			
			if (Main.rand.Next(30) == 0) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 21);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}

			if (projectile.timeLeft <= 880) {
				if (projectile.timeLeft == 880) {
					projectile.friendly = true;
					projectile.netUpdate = true;
				} else {	
					Vector2 move = Vector2.Zero;
					float distance = 2000f;
					bool target = false;
					for (int k = 0; k < 200; k++)
					{
						if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 
						&& OrchidModAlchemistNPC.AttractiteCanHome(Main.npc[k])) {
							Vector2 newMove = Main.npc[k].Center - projectile.Center;
							float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
							if (distanceTo < distance) {
								distance = distanceTo;
								orbitPoint = Main.npc[k].Center;
							}
						}
					}
					
					move = orbitPoint - projectile.Center + new Vector2(0f, 100f).RotatedBy(MathHelper.ToRadians(projectile.ai[1]));
					distance = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
					move.Normalize();
					float vel = (1f + (distance * 0.05f));
					vel = vel > 10f ? 10f : vel;
					move *= vel;
					projectile.velocity = move;
				}
			}
        }

        public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor) {
			Texture2D texture = ModContent.GetTexture("OrchidMod/Alchemist/Projectiles/Air/SunplateFlaskProjEffect");

			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + new Vector2(0f, projectile.gfxOffY) + projectile.Size * 0.5f;
				float progress = (float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length;
				Color color = Color.Lerp(new Color(98, 9, 92), new Color(255, 240, 0), progress) * progress;
				spriteBatch.Draw(texture, drawPos, null, color, projectile.rotation + Math.Sign(projectile.rotation) * k * (float)Math.PI / 3f, texture.Size() * 0.5f, projectile.scale, SpriteEffects.None, 0f);
			}

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor) {
            Texture2D texture = Main.projectileTexture[projectile.type];
			Texture2D effect = ModContent.GetTexture("OrchidMod/Alchemist/Projectiles/Air/SunplateFlaskProjEffect");

			//float progress = (float)Math.Abs(Math.Sin(Main.GlobalTime));
			//spriteBatch.Draw(effect, projectile.position - Main.screenPosition + projectile.Size * 0.5f, null, new Color(255, 240, 0, 50 * progress), projectile.rotation, texture.Size() * 0.5f, projectile.scale * (1.2f + progress / 5f), projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			spriteBatch.Draw(texture, projectile.position - Main.screenPosition + projectile.Size * 0.5f, null, Color.White, projectile.rotation, texture.Size() * 0.5f, projectile.scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
        }

        public override void Kill(int timeLeft)
        {
            for(int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 21);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
            }
		}
    }
}