using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod.Dusts.Thorium;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
    public class YewWoodScepterPortalProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 8;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 210;
			projectile.scale = 1f;
			projectile.extraUpdates = 1;
			projectile.alpha = 128;
			ProjectileID.Sets.Homing[projectile.type] = true;
			aiType = ProjectileID.Bullet; 
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadowflame Bolt");
        } 

        public override void AI()
        {
			if ((projectile.timeLeft % 10) % 2 == 0) {
				projectile.ai[0] = ((((float)Main.rand.Next(40) / 10f) - 2f) * 3);
					
				if (projectile.localAI[0] == 0f)
				{
					AdjustMagnitude(ref projectile.velocity);
					projectile.localAI[0] = 1f;
				}
					
				Vector2 move = Vector2.Zero;
				float distance = 600f;
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
					projectile.velocity = (20 * projectile.velocity + move);
					AdjustMagnitude(ref projectile.velocity);
				}
			} else {
				Vector2 projectileVelocity = ( new Vector2(projectile.velocity.X, projectile.velocity.Y ).RotatedBy(MathHelper.ToRadians(projectile.ai[0])));
				projectile.velocity = projectileVelocity;
				projectile.netUpdate = true;
			}
			
			if (Main.rand.Next(2) == 0)
			{
				int DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 27, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
				Main.dust[DustID].noGravity = true;
			}
		}
		
		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 3f)
			{
				vector *= 3f / magnitude;
			}
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<4; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 27);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = projectile.velocity / 2;
            }
        }
    }
}