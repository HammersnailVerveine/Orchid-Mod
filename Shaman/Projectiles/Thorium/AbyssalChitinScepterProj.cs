using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Projectiles.Thorium
{
    public class AbyssalChitinScepterProj : OrchidModShamanProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abyssal Bubble");
        } 
		
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.alpha = 196;
			projectile.timeLeft = 1800;
			ProjectileID.Sets.Homing[projectile.type] = true;
        }
		
        // public override Color? GetAlpha(Color lightColor)
        // {
            // return Color.White;
        // }
		
		public override void AI()
        {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			
			projectile.rotation += 0.1f;

			if (projectile.timeLeft % (15 - OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod) * 2) == 0) {
				if (projectile.damage < (150) * modPlayer.shamanDamage) {
					spawnDustCircle(111, 10);
					projectile.damage ++;
				} else {
					spawnDustCircle(111, 15);
				}
			}
			
			if (Main.rand.Next(10) == 0) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 101);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;	
			}
			
			if (projectile.timeLeft < 1760) {
				projectile.ai[1]++;
				if (projectile.ai[1] == 10)
				{	
					projectile.ai[1] = 0;
					projectile.netUpdate = true;
					switch (Main.rand.Next(4)) {	
						case 0:
						projectile.velocity.Y =  1;
						projectile.velocity.X =  1;
						break;
						case 1:
						projectile.velocity.Y =  -1;
						projectile.velocity.X =  -1;
						break;
						case 2:
						projectile.velocity.Y =  -1;
						projectile.velocity.X =  1;
						break;
						case 3:
						projectile.velocity.Y =  1;
						projectile.velocity.X =  -1;
						break;
					}
				}
				for (int index1 = 0; index1 < 1; ++index1)
				{	
					projectile.velocity = projectile.velocity * 0.75f;		
				}
				if (projectile.alpha > 70)
				{
					projectile.alpha -= 15;
					if (projectile.alpha < 70)
					{
						projectile.alpha = 70;
					}
				}
				if (projectile.localAI[0] == 0f)
				{
					AdjustMagnitude(ref projectile.velocity);
					projectile.localAI[0] = 1f;
				}
				Vector2 move = Vector2.Zero;
				float distance = 200f;
				bool target = false;
				for (int k = 0; k < 200; k++)
				{
					if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy)
					{
						Vector2 newMove = Main.npc[k].Center - projectile.Center;
						float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
						if (distanceTo < distance)
						{
							move = newMove;
							distance = distanceTo;
							target = true;
						}
					}
				}
				if (target)
				{
					AdjustMagnitude(ref move);
					projectile.velocity = (5 * projectile.velocity + move) / 1f;
					AdjustMagnitude(ref projectile.velocity);
				}
			}
        }
		
		public void spawnDustCircle(int dustType, int distToCenter) {
			for (int i = 0 ; i < 10 ; i ++ ) {
				double deg = (i * (72 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);
					 
				float posX = projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - projectile.width/2 + projectile.velocity.X + 4;
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - projectile.height/2 + projectile.velocity.Y + 4;
					
				Vector2 dustPosition = new Vector2(posX, posY);
					
				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
					
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].fadeIn = 0.75f;
				Main.dust[index2].scale = 1f;
				Main.dust[index2].noGravity = true;
			}
		}
		
		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 6f)
			{
				vector *= 6f / magnitude;
			}
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<10; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 101);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
            }
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
    }
}