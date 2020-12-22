using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Circle
{

	public class ReviverofSoulsFlame : OrchidModShamanProjectile
	{
		float startX = 0;
		float startY = 0;
		float hoverY = 0;
		bool hoverD = false;
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul Flame");
        } 
		public override void SafeSetDefaults()
		{
			projectile.width = 16;
			projectile.height = 26;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.timeLeft = 12960000;
			projectile.scale = 1f;
			projectile.tileCollide = false;
			Main.projFrames[projectile.type] = 7;
		}
		
		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
        public override void AI()
        {         					
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			
			if (player != Main.player[Main.myPlayer]) {
				projectile.active = false;
			}
			
			if (Main.LocalPlayer.FindBuffIndex(mod.BuffType("SpiritualBurst")) > -1)
				switch (Main.rand.Next(5)) {
					case 1:
						projectile.scale = 1.1f;
						break;
					case 2:
						projectile.scale = 1.2f;
						break;
					case 3:
						projectile.scale = 1.3f;
						break;
					case 4:
						projectile.scale = 1.4f;
						break;
					case 5:
						projectile.scale = 1.5f;
						break;
				}
			else
				projectile.scale = 1f;
			if (Main.time % 5 == 0)
				projectile.frame ++;
			if (projectile.frame == 7)
				projectile.frame = 0;
			
			if (modPlayer.timer120 % 60 == 0)
				hoverD = !hoverD;
			
			if (hoverD == false)
				hoverY -= 0.3f;
			else hoverY += 0.3f;
			
			if (modPlayer.shamanOrbCircle != ShamanOrbCircle.REVIVER || modPlayer.orbCountCircle <= 0)
				projectile.Kill();

			if (projectile.timeLeft == 12960000) {
				startX = projectile.position.X - player.position.X + player.velocity.X;
				startY = projectile.position.Y - player.position.Y + player.velocity.Y;
			}
			projectile.velocity.X = player.velocity.X;
			projectile.position.X = player.position.X + startX;
			projectile.position.Y = player.position.Y + startY - hoverY;
			
			if (Main.player[projectile.owner].FindBuffIndex(mod.BuffType("SpiritualBurst")) > -1) {
				if (Main.rand.Next(10) == 0) {
					int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172);
					Main.dust[dust2].velocity *= 2f;
					Main.dust[dust2].scale = 1.5f;
					Main.dust[dust2].noGravity = true;
					Main.dust[dust2].noLight = true;
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
		
		public override void SafePreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D flameTexture = ModContent.GetTexture("OrchidMod/Shaman/Projectiles/OreOrbs/Circle/ReviverOfSoulsFlameTexture");
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 1f, projectile.height * 1f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				drawPos.X += Main.rand.Next(6) - 3 - Main.player[projectile.owner].velocity.X;
				drawPos.Y += Main.rand.Next(6) - 3 - Main.player[projectile.owner].velocity.Y;
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k*5) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(flameTexture, drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0.3f);
			}
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<5; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
            }
        }
    }
}
 