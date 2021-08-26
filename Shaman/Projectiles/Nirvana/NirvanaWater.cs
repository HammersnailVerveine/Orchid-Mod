using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Nirvana
{
	public class NirvanaWater : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
			projectile.scale = 0f;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.tileCollide = false;
			projectile.timeLeft = 200;
            projectile.extraUpdates = 5;		
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nirvana Water Element");
        } 
		
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
        public override void AI()
        {
            if (projectile.timeLeft == 200) {
				for(int i=0; i<10; i++)
				{
					int SDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 59);
					Main.dust[SDust].velocity *= 2f;
					Main.dust[SDust].scale = (float) Main.rand.Next(70, 110) * 0.025f;
					Main.dust[SDust].noGravity = true;
				}
			}
			
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 59);
			Main.dust[dust].velocity /= 3f;
			Main.dust[dust].scale = (float) Main.rand.Next(70, 110) * 0.013f;
			Main.dust[dust].noGravity = true;
			
            if (projectile.timeLeft == 199) 
				projectile.velocity.X *= -1;	
			
			if (projectile.localAI[0] == 0f)
			{
				AdjustMagnitude(ref projectile.velocity);
				projectile.localAI[0] = 1f;
			}
			Vector2 move = Vector2.Zero;
			float distance = 500f;
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
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
    }
}