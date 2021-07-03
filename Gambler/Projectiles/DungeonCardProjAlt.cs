using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod;

namespace OrchidMod.Gambler.Projectiles
{

	public class DungeonCardProjAlt : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults() {
            DisplayName.SetDefault("Soul Flame");
        } 
		
		public override void SafeSetDefaults() {
			projectile.width = 16;
			projectile.height = 26;
			projectile.aiStyle = 0;
			projectile.friendly = false;
			projectile.timeLeft = 600;
			projectile.scale = 1f;
			projectile.tileCollide = true;
			Main.projFrames[projectile.type] = 7;
			this.gamblingChipChance = 100;
			this.baseCritChance = 10;
		}
		
		public override Color? GetAlpha(Color lightColor) {
            return Color.White;
        }
		
        public override void AI() {
			Player player = Main.player[Main.myPlayer]; // < TEST MP
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			NPC target = Main.npc[(int)projectile.ai[1]];
			projectile.velocity *= 0.95f;
			
			if (Main.time % 5 == 0)
				projectile.frame ++;
			if (projectile.frame == 7)
				projectile.frame = 0;
				
			if (target.active == false) {
				projectile.Kill();
			}
			
			if (projectile.timeLeft < 540) {
				for (int k = 0; k < Main.player.Length; k++) {
					Player playerMove = Main.player[k];
					Vector2 newMove = playerMove.Center - projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo < 100f) {
						newMove *= 3f / distanceTo;
						projectile.velocity = newMove;
						projectile.netUpdate = true;
					}
					
					if (projectile.owner == Main.myPlayer) {
						if (playerMove.Hitbox.Intersects(projectile.Hitbox)) {
							bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
							if (!dummy) {
								OrchidModGamblerHelper.addGamblerChip(this.gamblingChipChance, player, modPlayer);
							}
							
							bool crit = (Main.rand.Next(101) <= modPlayer.gamblerCrit + this.baseCritChance);
							player.ApplyDamageToNPC(target, Main.DamageVar(projectile.damage), 0.1f, player.direction, crit);
							OrchidModProjectile.spawnDustCircle(projectile.Center, 29, 10, 10, true, 1.3f, 1f, 5f, true, true, false, 0, 0, true);
							OrchidModProjectile.spawnDustCircle(target.Center, 29, 10, 10, true, 1.3f, 1f, 5f, true, true, false, 0, 0, true);
							Main.PlaySound(2, (int)projectile.Center.X ,(int)projectile.Center.Y, 45);
							projectile.Kill();
						}
					}
				}
			}
        }
		
		public override void SafePostAI()
        {
            for (int num46 = projectile.oldPos.Length - 5; num46 > 0; num46--)
            {
                projectile.oldPos[num46] = projectile.oldPos[num46 - 1];
            }
            projectile.oldPos[0] = projectile.position;
        }
		
		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D flameTexture = ModContent.GetTexture("OrchidMod/Gambler/Projectiles/DungeonCardProjAlt_Glow");
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 1f, projectile.height * 1f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				drawPos.X += Main.rand.Next(6) - 3 - Main.player[projectile.owner].velocity.X;
				drawPos.Y += Main.rand.Next(6) - 3 - Main.player[projectile.owner].velocity.Y;
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k*5) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(flameTexture, drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0.3f);
			}
			return true;
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<5; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 29);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
            }
        }
		
		public override bool OnTileCollide(Vector2 oldVelocity)  {
			if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = - oldVelocity.X;
			if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = - oldVelocity.Y;
            return false;
        }
    }
}
 
