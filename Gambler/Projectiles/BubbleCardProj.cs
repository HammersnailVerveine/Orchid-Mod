using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
    public class BubbleCardProj : OrchidModGamblerProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 300;
			projectile.scale = 1f;
			projectile.alpha = 128;
			this.gamblingChipChance = 10;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble");
        } 
		
		public override void SafeAI()
        {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			projectile.rotation += 0.02f;
			projectile.velocity.X *= 0.98f;
			projectile.damage += (projectile.timeLeft % 20 == 0 && projectile.timeLeft > 150) ? 1 : 0;
			
			if (projectile.ai[1] <= 0f) {
				projectile.velocity.Y -= 0.012f;
				if (Main.myPlayer == projectile.owner) {
					if (!Main.mouseLeft && modPlayer.GamblerDeckInHand) {
						Vector2 newMove = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - projectile.Center;
						newMove.Normalize();
						newMove *= 10f;
						projectile.velocity = newMove;
						projectile.ai[1] = 1f;;
						projectile.netUpdate = true;
					}
				}
			} else {
				projectile.velocity.Y *= 0.98f;
			}
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<4; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 217);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
            }
			Main.PlaySound(2, (int)projectile.Center.X ,(int)projectile.Center.Y - 200, 54);
        }
    }
}