using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
    public class EnchantedScepterProj : OrchidModShamanProjectile
    {
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enchanted Bolt");
        } 
		
        public override void SafeSetDefaults()
        {
            projectile.width = 10;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 65;
			projectile.penetrate = 2;	
            this.empowermentType = 1;
            this.empowermentLevel = 1;
            this.spiritPollLoad = 0;	
        }
		
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
		public override void AI()
		{
			int Type = Main.rand.Next(3);
			if (Type == 0) Type = 15;
			if (Type == 1) Type = 57;
			if (Type == 2) Type = 58;
			int index2 = Dust.NewDust(projectile.position - projectile.velocity * 0.25f, projectile.width, projectile.height, Type, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(80, 110) * 0.013f);
			Main.dust[index2].velocity *= 0.2f;
			Main.dust[index2].noGravity = true;
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
			projectile.penetrate--;
            if (projectile.penetrate < 0) projectile.Kill();
            if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X/2;
            if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y/2;
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
            return false;
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<13; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 15);
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
    }
}