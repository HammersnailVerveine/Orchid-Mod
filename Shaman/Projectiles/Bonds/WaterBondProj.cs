using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Bonds
{
	public class WaterBondProj : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Water Bond");
        } 
		public override void SafeSetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.timeLeft = 12960000;
			projectile.scale = 1f;
			projectile.tileCollide = false;
			projectile.extraUpdates = 1;
			ProjectileID.Sets.Homing[projectile.type] = true;
			projectile.timeLeft = 350;
			projectile.alpha = 126;
		}
		
        public override void AI()
        {         					
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			
			projectile.rotation += (float)(Main.rand.Next(5) / 10);
			
			projectile.friendly = projectile.timeLeft < 300;
			
			if (projectile.ai[0] > 0 && projectile.knockBack != 1f) {
				projectile.knockBack = 1f;
				projectile.netUpdate = true;
			}
			
			if (Main.rand.Next(3) == 0) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 29);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
				Main.dust[dust].scale *= 2f;
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
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 29);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (projectile.ai[0] > 1) {
				if (Main.myPlayer == player.whoAmI)
					player.HealEffect(5, true);
				player.statLife += 5;	
			}
			
			if (projectile.ai[0] > 2) {
				OrchidModGlobalNPC modTarget = target.GetGlobalNPC<OrchidModGlobalNPC>();
				modTarget.shamanWater = true;
			}
			
			if (projectile.ai[0] > 3 && Main.rand.Next(3) == 0) {		
				Vector2 heading = new Vector2(target.position.X, target.position.Y - 10f);
				heading = heading - target.position;
				heading.Normalize();
				heading *= new Vector2(-5f, -5f).Length();

				Vector2 vel = (new Vector2(heading.X, heading.Y).RotatedByRandom(MathHelper.ToRadians(20)));
				int waterProj = Projectile.NewProjectile(target.Center.X, target.Center.Y, vel.X, vel.Y, mod.ProjectileType("WaterBondProj"), damage, 0f, projectile.owner);
				Main.projectile[waterProj].ai[0] = projectile.ai[0];
				Main.projectile[waterProj].netUpdate = true;
			}
		}
    }
}
