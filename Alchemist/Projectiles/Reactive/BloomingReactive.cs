using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod;
using OrchidMod.Dusts;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Reactive
{

	public class BloomingReactive : ModProjectile
	{
		float startX = 0;
		float startY = 0;
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flower");
        } 
		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 32;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.timeLeft = 12960000;
			projectile.scale = 1f;
			projectile.tileCollide = false;
			// projectile.alpha = 64;
			Main.projFrames[projectile.type] = 9;
		}
		
		// public override Color? GetAlpha(Color lightColor)
        // {
            // return Color.White;
        // }
		
		public override bool? CanCutTiles() {
			return false;
		}
		
        public override void AI()
        {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
		
			projectile.rotation += 0.02f;
		
			if (player != Main.player[Main.myPlayer]) {
				projectile.active = false;
			}
			if (modPlayer.alchemistFlower != projectile.ai[1]) {
				projectile.ai[1] = modPlayer.alchemistFlower;
				projectile.netUpdate = true;
			}
			projectile.frame = (int)projectile.ai[1];
			
			if (projectile.ai[1] == 0)
				projectile.active = false;

			if (projectile.timeLeft == 12960000) {
				startX = projectile.position.X - player.position.X;
				startY = projectile.position.Y - player.position.Y;
			}
			projectile.velocity.X = player.velocity.X;
			projectile.position.X = player.position.X + startX;
			projectile.position.Y = player.position.Y + startY;
			
			if (Main.rand.Next(80) == 0) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustType<BloomingDust>());
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0.1f;
			}
        }
		
		public override void Kill(int timeLeft)
        {
			for(int i=0; i<5; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustType<BloomingDust>());
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 5f;
            }
		}
    }
}
 