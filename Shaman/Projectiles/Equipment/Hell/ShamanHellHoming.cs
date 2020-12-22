using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Equipment.Hell
{
	public class ShamanHellHoming : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 300;
			projectile.scale = 1f;
            projectile.extraUpdates = 2;			
			projectile.tileCollide = false;
			projectile.alpha = 255;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hellish Fire");
        } 
		
        public override void AI()
        {
            for (int index1 = 0; index1 < 9; ++index1)
            {	
				if (index1%3 ==0) {
					float x = projectile.position.X - projectile.velocity.X / 10f * (float) index1;
					float y = projectile.position.Y - projectile.velocity.Y / 10f * (float) index1;
					int index2 = Dust.NewDust(new Vector2(x, y), 1, 1, 6, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].alpha = projectile.alpha;
					Main.dust[index2].position.X = x;
					Main.dust[index2].position.Y = y;
					Main.dust[index2].scale = (float) Main.rand.Next(10, 110) * 0.013f;
					Main.dust[index2].velocity = projectile.velocity / 2;
					Main.dust[index2].noGravity = true;
				}
            }
			
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
				if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy && !Main.npc[k].HasBuff(mod.BuffType("HellHit")))
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
			} else {
				projectile.timeLeft -= 50;
			}
        }
		
		public void spawnDustCircle(int dustType, int distToCenter) {
			for (int i = 0 ; i < 15 ; i ++ )
			{
				double dustDeg = (i * (27));//    + 5 - Main.rand.Next(10)));
				double dustRad = dustDeg * (Math.PI / 180);
				
				float posX = projectile.Center.X - (int)(Math.Cos(dustRad) * distToCenter);
				float posY = projectile.Center.Y - (int)(Math.Sin(dustRad) * distToCenter);
				
				Vector2 dustPosition = new Vector2(posX, posY);
				
				int index1 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
				
				Main.dust[index1].velocity = projectile.velocity / 2;
				Main.dust[index1].fadeIn = 1f;
				Main.dust[index1].scale = 1.2f;
				Main.dust[index1].noGravity = true;
			}
		}
		
		public override bool? CanHitNPC(NPC target) {
			return !(target.HasBuff(mod.BuffType("HellHit")) || target.friendly);
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
        {
			spawnDustCircle(6, 12);
			spawnDustCircle(6, 8);
			spawnDustCircle(6, 4);
			target.AddBuff((mod.BuffType("HellHit")), 60 *3);
			target.AddBuff(24, 60 * 5); // On fire
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