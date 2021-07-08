using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
    public class IceMimicScepterProj : OrchidModShamanProjectile
    {
		private float rotationSpeed = 0f;
		private bool faster = false;
		private int slowdelay = 0;
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Spear");
        } 
		
        public override void SafeSetDefaults()
        {
            projectile.width = 52;
            projectile.height = 52;
            projectile.friendly = true;
			projectile.timeLeft = 1200;
			projectile.penetrate = 15;	
			this.empowermentType = 2;
        }
		
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
		public override void AI()
		{
			bool moving = !(projectile.velocity.X < 1f && projectile.velocity.X > -1f && projectile.velocity.Y < 1f && projectile.velocity.Y > -1f);
			this.slowdelay -= this.slowdelay > 0 ? 1 : 0;
			this.projectileTrail = projectile.ai[1] == 0f || projectile.ai[1] == 2f;
			
			if (Main.rand.Next(4) == 0) {
				int index = Dust.NewDust(projectile.position - projectile.velocity * 0.25f, projectile.width, projectile.height, 59, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(80, 110) * 0.013f);
				Main.dust[index].velocity *= 0.2f;
				Main.dust[index].scale *= 1.5f;
				Main.dust[index].noGravity = true;
			}
			
			if (projectile.timeLeft == 3600)
			{
				if (projectile.ai[1] == 3f) {
					this.faster = true;
				}
				projectile.ai[1] = 2f;
			}
			
			if (projectile.ai[1] == 0f || projectile.ai[1] == 2f)
			{
				projectile.velocity *= 0.95f;
				projectile.rotation = projectile.velocity.ToRotation();
				projectile.direction = projectile.spriteDirection;
				
				this.rotationSpeed = this.rotationSpeed == 0f ? this.rotationSpeed : 0f;
				if (!moving && this.slowdelay <= 0)
				{
					projectile.velocity *= 0f;
					projectile.ai[1] = 1f;
					projectile.netUpdate = true;
				}
			}
			
			if (projectile.ai[1] == 1f)
			{
				float spinValue = 0.005f;
				projectile.rotation += this.rotationSpeed;
				this.rotationSpeed += this.faster ? spinValue * 2 : spinValue;
				if (this.rotationSpeed >= spinValue * 150)
				{
					Vector2 move = Vector2.Zero;
					float distance = 500f;
					bool targetFound = false;
					for (int w = 0; w < Main.npc.Length; w++)
					{
						if (Main.npc[w].active && !Main.npc[w].dontTakeDamage && !Main.npc[w].friendly && Main.npc[w].lifeMax > 5 && Main.npc[w].type != NPCID.TargetDummy)
						{
							Vector2 newMove = Main.npc[w].Center - projectile.Center;
							float distancenewMove = (float)Math.Sqrt((newMove.X * newMove.X) + (newMove.Y * newMove.Y));
							if (distancenewMove < distance)
							{
								move = newMove;
								distance = distancenewMove;
								targetFound = true;
							}
						
							if (targetFound)
							{
								projectile.ai[1] = 0f;
								move.Normalize();
								move *= 25f;
								projectile.velocity = move;
								this.rotationSpeed = 0f;
								this.slowdelay = 90;
								projectile.netUpdate = true;
							}
						}
					}
					
					this.rotationSpeed -= this.faster ? spinValue * 2 : spinValue;
				}
			}
		}
		
		public override bool? CanHitNPC(NPC target) {
			if (target.friendly || target.dontTakeDamage) {
				return false;
			}
			OrchidModGlobalNPC modTarget = target.GetGlobalNPC<OrchidModGlobalNPC>();
			return modTarget.shamanSpearDamage <= 0;
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X/2;
            if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y/2;
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 50);
			return false;
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<13; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 59);
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
        {
			OrchidModGlobalNPC modTarget = target.GetGlobalNPC<OrchidModGlobalNPC>();
			modTarget.shamanSpearDamage = 60;
		}
    }
}