using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
	public class MartianBeamerProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 29;
			projectile.timeLeft = 100;
			projectile.scale = 0f;
            projectile.extraUpdates = 10;	
            this.empowermentType = 1;
            this.empowermentLevel = 4;
            this.spiritPollLoad = 0;		
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Martian Beam");
        } 
		
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
        public override void AI()
        {
            if (projectile.alpha < 170)
            {
                for (int index1 = 0; index1 < 9; ++index1)
                {	
					if (index1%3 ==0) {
						float x = projectile.position.X - projectile.velocity.X / 10f * (float) index1;
						float y = projectile.position.Y - projectile.velocity.Y / 10f * (float) index1;
						int index2 = Dust.NewDust(new Vector2(x, y), 1, 1, 226, 0.0f, 0.0f, 0, new Color(), 1f);
						Main.dust[index2].alpha = projectile.alpha;
						Main.dust[index2].position.X = x;
						Main.dust[index2].position.Y = y;
						Main.dust[index2].scale = (float) Main.rand.Next(1, 11) * 0.13f;
						Main.dust[index2].velocity = projectile.velocity;
						Main.dust[index2].noGravity = true;
					}
                }
			}
			
			if (projectile.localAI[0] == 0f)
			{
				AdjustMagnitude(ref projectile.velocity);
				projectile.localAI[0] = 1f;
			}
			Vector2 move = Vector2.Zero;
			float distance = 150f;
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
				projectile.velocity = (20 * projectile.velocity + move) / 10f;
				AdjustMagnitude(ref projectile.velocity);
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
            for(int i=0; i<3; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 229);
				Main.dust[dust].noGravity = true;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
    }
}