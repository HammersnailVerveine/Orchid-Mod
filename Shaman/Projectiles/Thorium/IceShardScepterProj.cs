using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Projectiles.Thorium
{
    public class IceShardScepterProj : OrchidModShamanProjectile
    {
		private Vector2 storeVelocity;
		private int storeDamage;
		private float dustScale = 0;
		
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Bolt");
        } 
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.aiStyle = 29;
			projectile.timeLeft = 80;
            projectile.friendly = true;
            projectile.tileCollide = true;
			projectile.scale = 1f;
			aiType = ProjectileID.Bullet;
            this.empowermentType = 2;
            this.empowermentLevel = 1;
            this.spiritPollLoad = 0;
        }
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
			
		public override void AI()
		{
			projectile.alpha += 30;
			
			if (projectile.timeLeft == 80) {
				storeVelocity = projectile.velocity;
				storeDamage = projectile.damage;
			}
			
			if (projectile.timeLeft > 35) {
				projectile.velocity *= 0f;
				projectile.damage = 0;
				dustScale += 0.0195f;
			}
			
			if (projectile.timeLeft == 35) {
				projectile.damage = storeDamage;
				projectile.velocity = storeVelocity;
				projectile.extraUpdates = 1;

				
				spawnDustCircle(92, 20);
			}
			
			for (int i = 0 ; i < 3 ; i ++) {
				Vector2 Position = projectile.position;
				int index2 = Dust.NewDust(Position, projectile.width, projectile.height, 92);
				Main.dust[index2].scale = (float) 90 * 0.010f + dustScale/3;
				Main.dust[index2].velocity *= 0.2f;
				Main.dust[index2].noGravity = true;
			}
			
			if (!this.initialized) {
				this.initialized = true;
				Player player = Main.player[projectile.owner];
				OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
				int newCrit = 10 * modPlayer.getNbShamanicBonds() + modPlayer.shamanCrit + player.inventory[player.selectedItem].crit;
				OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
				modProjectile.baseCritChance = newCrit;
			}
		}
		
		public void spawnDustCircle(int dustType, int distToCenter) {
			for (int i = 0 ; i < 20 ; i ++ )
			{
				double dustDeg = (double) projectile.ai[1] * (i * (36 + 5 - Main.rand.Next(10)));
				double dustRad = dustDeg * (Math.PI / 180);
				
				float posX = projectile.Center.X - (int)(Math.Cos(dustRad) * distToCenter) - projectile.width/2 + 4;
				float posY = projectile.Center.Y - (int)(Math.Sin(dustRad) * distToCenter) - projectile.height/2 + 4;
				
				Vector2 dustPosition = new Vector2(posX, posY);
				
				int index1 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
				
				Main.dust[index1].velocity *= 0.2f;
				Main.dust[index1].fadeIn = 1f;
				Main.dust[index1].scale = 1.5f;
				Main.dust[index1].noGravity = true;
			}
		}
		
		public override void Kill(int timeLeft)
		{
            for(int i=0; i<5; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 92);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 2f;
				Main.dust[dust].velocity *= 2f;
            }
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (Main.rand.Next(3) == 0) target.AddBuff((44), 3 * 60); // Frostburn
		}
	}
}