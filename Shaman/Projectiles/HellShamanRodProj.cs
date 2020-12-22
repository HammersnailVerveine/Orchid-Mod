using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
	public class HellShamanRodProj : OrchidModShamanProjectile
	{
		private Vector2 storedVelocity = new Vector2(0f, 0f);
		private Vector2 posStart = new Vector2(0f, 0f);
		private Vector2 posEnd = new Vector2(0f, 0f);
		private bool slow = false;
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Burning Leaf");
        } 
		
		public override void SafeSetDefaults()
		{
            projectile.width = 24;
            projectile.height = 24;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 1801;
            projectile.penetrate = -1;
			this.empowermentType = 4;
			this.empowermentLevel = 2;
			this.spiritPollLoad = 0;
			this.projectileTrail = true;
		}
		
        public override void AI()
        {	
			projectile.friendly = !(projectile.ai[1] > 0);
		
			if (projectile.timeLeft == 1801) {
				if (projectile.ai[1] >= 4) {
					projectile.ai[1] -= 4;
					this.slow = true;
				}
				
				projectile.rotation += (float)Main.rand.Next(20);
				this.posStart = projectile.position * 1;
				this.posEnd = projectile.position + (projectile.velocity * 90);
				this.storedVelocity.X += projectile.velocity.X;
				this.storedVelocity.Y += projectile.velocity.Y;
				if (projectile.ai[1] > 0) {
					projectile.velocity *= 0f;
				}
			}
			
			projectile.rotation -= 0.35f;	
			
			if (projectile.timeLeft % 90 == 0 && projectile.timeLeft != 1800) {
				projectile.velocity *= -1f;
			}
			
			if (projectile.ai[1] > 0 && projectile.timeLeft % 60 == 0 && projectile.timeLeft != 1800) {
				projectile.ai[1] --;
				projectile.timeLeft = 1800;
				if (projectile.ai[1] <= 0) {
					projectile.velocity = this.storedVelocity;
					Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 65);
				}
			}
			
			if (Main.rand.Next(4) == 0) {
				int dust = Dust.NewDust(this.posStart, projectile.width, projectile.height, 6);
				Main.dust[dust].velocity = this.storedVelocity / -2f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale *= 1.3f;
			}
			
			if (Main.rand.Next(4) == 0) {
				int dust = Dust.NewDust(this.posEnd, projectile.width, projectile.height, 6);
				Main.dust[dust].velocity = this.storedVelocity / 2f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale *= 1.3f;
			}
			
			if (Main.rand.Next(6) == 0) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale *= 1.5f;
			}
		}
		
		public override void Kill(int timeLeft)
        {
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 65);
			
            for(int i=0; i<5; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 273);
				Main.dust[dust].velocity = projectile.velocity / 2f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale *= 1.3f;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			target.AddBuff(24, 60 * 5);
			if (!(target.boss || target.type == NPCID.TargetDummy) && target.knockBackResist > 0f && this.slow) {
				target.AddBuff(mod.BuffType("LeafSlow"), 60 * 5);
			}
		}
	}
}