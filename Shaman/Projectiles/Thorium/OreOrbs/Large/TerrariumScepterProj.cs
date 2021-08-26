using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Large
{
	public class TerrariumScepterProj : OrchidModShamanProjectile
	{
		int dustType = 0;
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Magic");
        } 
		
		public override void SafeSetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = 44;
			projectile.friendly = true;
			projectile.scale = 1f;
			projectile.extraUpdates = 1;
			projectile.alpha = 255;
			projectile.timeLeft = 120;
		}
		
        public override void AI()
        {   
			if (dustType == 0) {
				dustType = Main.rand.Next(6) + 59;	
				spawnDustCircle(this.dustType, 15);
			}
			projectile.rotation += 0.1f;

			int dust = Dust.NewDust(projectile.position, 0, 0, this.dustType, 0f, 0f);
			Main.dust[dust].velocity = projectile.velocity / 3;
			Main.dust[dust].scale = 1.5f;
			Main.dust[dust].noGravity = true;
			
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
		
		public void spawnDustCircle(int dustType, int distToCenter) {
			for (int i = 0 ; i < 15 ; i ++ ) {
				double deg = (i * (48 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);
					 
				float posX = projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - 4;
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - 4;
					
				Vector2 dustPosition = new Vector2(posX, posY);
					
				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
					
				Main.dust[index2].velocity = projectile.velocity / 2;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.5f;
				Main.dust[index2].noGravity = true;
			}
		}
		
		public override void Kill(int timeLeft)
        {
			spawnDustCircle(this.dustType, 15);
			spawnDustCircle(this.dustType, 10);
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null) {
				target.AddBuff((thoriumMod.BuffType("TerrariumMix")), 2 * 60);
			}
			
			if (modPlayer.shamanOrbLarge != ShamanOrbLarge.TERRARIUM) {
				modPlayer.shamanOrbLarge = ShamanOrbLarge.TERRARIUM;
				modPlayer.orbCountLarge = 0;
			}
			modPlayer.orbCountLarge ++;
			
			float orbX = player.position.X + player.width / 2;
			float orbY = player.position.Y;
			
			if (player.FindBuffIndex(mod.BuffType("ShamanicBaubles")) > -1 && modPlayer.orbCountLarge < 5)
			{
				modPlayer.orbCountLarge += 5;
				Projectile.NewProjectile(orbX - 43, orbY - 38, 0f, 0f, mod.ProjectileType("TerrariumScepterOrb"), 0, 0, projectile.owner, 0f, 0f);
				player.ClearBuff(mod.BuffType("ShamanicBaubles"));
			}
			
			if (modPlayer.orbCountLarge == 5)
				Projectile.NewProjectile(orbX - 43, orbY - 38, 0f, 0f, mod.ProjectileType("TerrariumScepterOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountLarge == 10)
				Projectile.NewProjectile(orbX - 30, orbY - 48, 0f, 0f, mod.ProjectileType("TerrariumScepterOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountLarge == 15)
				Projectile.NewProjectile(orbX - 15, orbY - 53, 0f, 0f, mod.ProjectileType("TerrariumScepterOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountLarge == 20)
				Projectile.NewProjectile(orbX, orbY - 55, 0f, 0f, mod.ProjectileType("TerrariumScepterOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountLarge == 25)
				Projectile.NewProjectile(orbX + 15, orbY - 53, 0f, 0f, mod.ProjectileType("TerrariumScepterOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountLarge == 30)
				Projectile.NewProjectile(orbX + 30, orbY - 48, 0f, 0f, mod.ProjectileType("TerrariumScepterOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountLarge == 35)
				Projectile.NewProjectile(orbX + 43, orbY - 38, 0f, 0f, mod.ProjectileType("TerrariumScepterOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountLarge > 35) {
				int dmg = (int)(80 * player.GetModPlayer<OrchidModPlayer>().shamanDamage);
				Projectile.NewProjectile(orbX - 43, orbY - 38, -3f, -5f, mod.ProjectileType("TerrariumScepterOrbProj"), dmg, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(orbX - 30, orbY - 48, -2f, -5f, mod.ProjectileType("TerrariumScepterOrbProj"), dmg, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(orbX - 15, orbY - 53, -1f, -5f, mod.ProjectileType("TerrariumScepterOrbProj"), dmg, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(orbX, orbY - 55, 0f, -5f, mod.ProjectileType("TerrariumScepterOrbProj"), dmg, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(orbX + 15, orbY - 53, 1f, -5f, mod.ProjectileType("TerrariumScepterOrbProj"), dmg, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(orbX + 30, orbY - 48, 2f, -5f, mod.ProjectileType("TerrariumScepterOrbProj"), dmg, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(orbX + 43, orbY - 38, 3f, -5f, mod.ProjectileType("TerrariumScepterOrbProj"), dmg, 0f, projectile.owner, 0f, 0f);
				modPlayer.orbCountLarge = 0;
			}
		}
    }
}
