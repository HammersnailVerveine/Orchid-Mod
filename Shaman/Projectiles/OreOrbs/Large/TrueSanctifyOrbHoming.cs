using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Large
{
	public class TrueSanctifyOrbHoming : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Sanctify Orb");
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
			Main.projFrames[projectile.type] = 24;
			ProjectileID.Sets.Homing[projectile.type] = true;
			projectile.timeLeft = 350;
            this.empowermentType = 5;
		}
		
		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
        public override void AI()
        {         					
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			
			if (modPlayer.timer120 == 65)
				projectile.frame = 1;
			if (modPlayer.timer120 == 70)
				projectile.frame = 2;
			if (modPlayer.timer120 == 75)
				projectile.frame = 3;
		   	if (modPlayer.timer120 == 80)
				projectile.frame = 4;
			if (modPlayer.timer120 == 85)
				projectile.frame = 5;
		    if (modPlayer.timer120 == 90)
				projectile.frame = 6;
			if (modPlayer.timer120 == 95)
				projectile.frame = 7;
			if (modPlayer.timer120 == 100)
				projectile.frame = 8;
			if (modPlayer.timer120 == 105)
				projectile.frame = 9;
			if (modPlayer.timer120 == 110)
				projectile.frame = 10;
			if (modPlayer.timer120 == 115)
				projectile.frame = 11;
			if (modPlayer.timer120 == 0)
				projectile.frame = 12;
			if (modPlayer.timer120 == 5)
				projectile.frame = 13;
			if (modPlayer.timer120 == 10)
				projectile.frame = 14;
			if (modPlayer.timer120 == 15)
				projectile.frame = 15;
			if (modPlayer.timer120 == 20)
				projectile.frame = 16;
			if (modPlayer.timer120 == 25)
				projectile.frame = 17;
			if (modPlayer.timer120 == 30)
				projectile.frame = 18;
			if (modPlayer.timer120 == 35)
				projectile.frame = 19;
			if (modPlayer.timer120 == 40)
				projectile.frame = 20;
			if (modPlayer.timer120 == 45)
				projectile.frame = 21;
			if (modPlayer.timer120 == 50)
				projectile.frame = 22;
			if (modPlayer.timer120 == 55)
				projectile.frame = 23;
			if (modPlayer.timer120 == 60)
				projectile.frame = 0;

			if (Main.rand.Next(5) == 0) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 254);
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
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 254);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
    }
}
