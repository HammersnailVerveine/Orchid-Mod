using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod.Dusts.Thorium;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
    public class ViscountScepterProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 18;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 40;
			projectile.scale = 1f;
			aiType = ProjectileID.Bullet; 
            this.empowermentType = 3;
            this.empowermentLevel = 2;
            this.spiritPollLoad = 0;
        }
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vampire Bolt");
        } 
		
        public override void AI()
        {
			if (Main.rand.Next(2) == 0)
			{
				int DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 258, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
				Main.dust[DustID].noGravity = true;
			}
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<4; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 258);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = projectile.velocity / 2;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (Main.rand.Next(8) < (modPlayer.getNbShamanicBonds() + 1)) {
				Projectile.NewProjectile(player.position.X, player.position.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("ViscountScepterBat"), projectile.damage, 0f, 0, 0f, 0f);
			}
		}
    }
}