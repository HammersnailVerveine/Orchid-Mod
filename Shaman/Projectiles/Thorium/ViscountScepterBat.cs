using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
    public class ViscountScepterBat : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 150;
			projectile.scale = 1f;
			projectile.penetrate = 1;
			Main.projFrames[projectile.type] = 5;
			ProjectileID.Sets.Homing[projectile.type] = true;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bat");
        }
		
        public override void AI()
        {  
			Player player = Main.player[projectile.owner];
			
			if (player.GetModPlayer<OrchidModPlayer>().timer120 % 3 == 0)
				projectile.frame ++;
			if (projectile.frame == 5)
				projectile.frame = 0;
			
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
				projectile.velocity = (20 * projectile.velocity + move) / 7f;
				AdjustMagnitude(ref projectile.velocity);
			}
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<4; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 16);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale *= 1.5f;
				Main.dust[dust].velocity = projectile.velocity / 2;
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
    }
}