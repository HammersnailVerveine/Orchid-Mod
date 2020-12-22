using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{
	public class TerraScepterOrbHoming1 : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terra Orb");
        } 
		public override void SafeSetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.timeLeft = 12960000;
			projectile.scale = 1f;
			projectile.tileCollide = false;
			projectile.extraUpdates = 1;
			Main.projFrames[projectile.type] = 10;
			ProjectileID.Sets.Homing[projectile.type] = true;
			projectile.timeLeft = 350;
            this.empowermentType = 5;
            this.empowermentLevel = 4;
            this.spiritPollLoad = 0;
		}
		
		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
        public override void AI()
        {         					
			Player player = Main.player[projectile.owner];
			
			if (projectile.timeLeft == 349 && player.GetModPlayer<OrchidModPlayer>().shamanOrbUnique == ShamanOrbUnique.TERRA) {
				player.GetModPlayer<OrchidModPlayer>().orbCountUnique = 0;
			}
			
			if (player.GetModPlayer<OrchidModPlayer>().timer120 % 5 == 0)
				projectile.frame ++;
			if (projectile.frame == 10)
				projectile.frame = 0;

			if (Main.rand.Next(4) == 0) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 157);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
			}
			if (Main.rand.Next(4) == 0) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 269);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
			}
			
			if (projectile.localAI[0] == 0f)
			{
				AdjustMagnitude(ref projectile.velocity);
				projectile.localAI[0] = 1f;
			}
			
			Vector2 move = Vector2.Zero;
			float distance;
			if (projectile.timeLeft < 300)
				distance = 1000f;
			else distance = 10f;
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
				projectile.velocity = (10 * projectile.velocity + move) / 7f;
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
			for(int i=0; i<5; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 269);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 5f;
				int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 157);
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity *= 5f;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
    }
}
