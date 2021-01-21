using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{
    public class TerraSpecterProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 7;
            projectile.height = 7;
            projectile.friendly = true;
            projectile.aiStyle = 0;
            projectile.extraUpdates = 3;
            projectile.timeLeft = 90;	
			projectile.scale = 0.5f;
            this.empowermentType = 5;
            this.empowermentLevel = 4;
            this.spiritPollLoad = 0;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terric Magic");
        } 
		
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
        public override void AI()
        {
			Lighting.AddLight(projectile.Center, 0.010f, 0.010f, 0f);  //this defines the projectile light color
            for(int i=0; i<2; i++)
			{
				Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
				int dust2 = Dust.NewDust(pos, projectile.width, projectile.height/2, 157, projectile.velocity.X * 1f, projectile.velocity.Y * 1f);
			    Main.dust[dust2].noGravity = true;
			    Main.dust[dust2].scale = 1f;
				Main.dust[dust2].velocity /= 10f;
			    Main.dust[dust2].noLight = true;
			}
			
			if (projectile.localAI[0] == 0f)
			{
				AdjustMagnitude(ref projectile.velocity);
				projectile.localAI[0] = 1f;
			}
			Vector2 move = Vector2.Zero;
			float distance = 400f;
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
				projectile.velocity = (10 * projectile.velocity + move) / 1f;
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
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 157);
				Main.dust[dust].noGravity = true;
			}
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
    }
}