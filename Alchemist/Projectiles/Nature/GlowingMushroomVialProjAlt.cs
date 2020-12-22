using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Nature
{
    public class GlowingMushroomVialProjAlt : OrchidModAlchemistProjectile
    {
		private double dustVal = 0;
		
        public override void SafeSetDefaults()
        {
            projectile.width = 18;
            projectile.height = 16;
            projectile.aiStyle = 0;
			projectile.timeLeft = 120;
			projectile.scale = 1f;
			projectile.penetrate = -1;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mushroom");
        }
		
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor) {
			Texture2D texture = ModContent.GetTexture("OrchidMod/Alchemist/Projectiles/Nature/GlowingMushroomVialProjAlt_Glow");
			OrchidModProjectile.DrawProjectileGlowmask(projectile, spriteBatch, texture, Color.White);
		}
		
        public override void AI()
        {
			Player player = Main.player[Main.myPlayer];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			
			int range = 100;
			if (modPlayer.timer120 % 2 == 0) {
				this.spawnDust(172, range);
			}
			this.dustVal --;
			
			if (projectile.damage > 0) {
				projectile.timeLeft = 60 * projectile.damage * 3;
				projectile.damage = 0;
				projectile.netUpdate = true;
			}
			
			if (Main.rand.Next(30) == 0) {
				int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172);
				Main.dust[dust2].scale = 1.2f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity.X *= 2;
				Main.dust[dust2].velocity.Y *= 2;
			}
			
			Vector2 center = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
			float offsetX = player.Center.X - center.X;
			float offsetY = player.Center.Y - center.Y;
			float distance = (float)Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
			if (distance < 100f) {
				player.AddBuff(BuffType<Alchemist.Buffs.MushroomHeal>(), 1);
			}
		}
		
		public void spawnDust(int dustType, int distToCenter) {
			for (int i = 0 ; i < 3 ; i ++ ) {
				double deg = (4 * (42 + this.dustVal) + i * 120);
				double rad = deg * (Math.PI / 180);
						 
				float posX = projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) + projectile.velocity.X - 4;
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) + projectile.velocity.Y - 4;
						
				Vector2 dustPosition = new Vector2(posX, posY);
						
				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
						
				Main.dust[index2].velocity = projectile.velocity / 2;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = projectile.velocity.X == 0 ? 1.5f :(float) Main.rand.Next(70, 110) * 0.013f;
				Main.dust[index2].noGravity = true;
			}
		}
    }
}