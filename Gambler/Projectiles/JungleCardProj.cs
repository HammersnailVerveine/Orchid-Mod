	using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace OrchidMod.Gambler.Projectiles
{
    public class JungleCardProj: OrchidModGamblerProjectile
    {
		bool started = false;
		int count = 0;
		
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jungle Spore");
        } 
		
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.alpha = 126;
			projectile.timeLeft = 180;
			ProjectileID.Sets.Homing[projectile.type] = true;
			this.gamblingChipChance = 10;
			this.projectileTrail = true;
        }
		
		public override void SafeAI()
        {
			if (!this.initialized) {
				this.initialized = true;
				for (int l = 0; l < Main.projectile.Length; l++)
				{  
					Projectile proj = Main.projectile[l];
					if (proj.active && proj.owner == Main.myPlayer) {
						if (proj.type == ProjectileType<Gambler.Projectiles.JungleCardProj>()) {	
							proj.damage ++;
						}
					}
				}
			}
			
			this.count++;
			
			if (projectile.timeLeft > 120) {
				projectile.velocity.Y += 0.01f;
				projectile.velocity.X *= 0.95f;
			}
			
			if (Main.rand.Next(3) == 0) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 44);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
			
			if (started == false) {
				if (count == 30) started = true;
			}
			if (started == true) {
				projectile.ai[1]++;
				if (projectile.ai[1] == 10)
				{	
					projectile.ai[1] = 0;
					projectile.netUpdate = true;
					switch (Main.rand.Next(4)) {	
						case 0:
						projectile.velocity.Y =  1;
						projectile.velocity.X =  1;
						break;
						case 1:
						projectile.velocity.Y =  -1;
						projectile.velocity.X =  -1;
						break;
						case 2:
						projectile.velocity.Y =  -1;
						projectile.velocity.X =  1;
						break;
						case 3:
						projectile.velocity.Y =  1;
						projectile.velocity.X =  -1;
						break;
					}
				}
				
				if (projectile.alpha > 70)
				{
					projectile.alpha -= 15;
					if (projectile.alpha < 70)
					{
						projectile.alpha = 70;
					}
				}
				
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
					if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy && projectile.timeLeft < 240)
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
					projectile.velocity = (5 * projectile.velocity + move) / 1f;
					AdjustMagnitude(ref projectile.velocity);
				}
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
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
            for(int i=0; i<3; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 44);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
				Main.dust[dust].velocity *= 3f;
            }
			Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
			int rand = 10 - (modPlayer.gamblerLuckySprout ? 2 : 0);
			if (Main.rand.Next(rand) == 0 && projectile.localAI[1] != 1f && projectile.owner == Main.myPlayer) {
				Vector2 vel = (new Vector2(0f, -3f).RotatedBy(MathHelper.ToRadians(10)));
				int projType = ProjectileType<Gambler.Projectiles.JungleCardProjAlt>();
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, vel.X, vel.Y, projType, projectile.damage, projectile.knockBack, projectile.owner);
				for (int i = 0 ; i < 5 ; i ++) {
					Main.dust[Dust.NewDust(pos, projectile.width, projectile.height, 44)].velocity *= 0.25f;
				}
			}
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			target.AddBuff(20, 60 * 1);
        }
    }
}