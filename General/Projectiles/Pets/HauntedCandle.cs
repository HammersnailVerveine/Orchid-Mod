using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.General.Projectiles.Pets
{
	public class HauntedCandle : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Haunted Candle");
			Main.projFrames[projectile.type] = 1;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
			ProjectileID.Sets.LightPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;
			projectile.penetrate = -1;
			projectile.netImportant = true;
			projectile.timeLeft *= 5;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.scale = 1f;
			projectile.tileCollide = false;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (!player.active)
			{
				projectile.active = false;
				return;
			}
			if (player.dead)
			{
				modPlayer.hauntedCandle = false;
			}
			if (modPlayer.hauntedCandle)
			{
				projectile.timeLeft = 2;
			}
			
			
			if (Main.rand.Next(3) <= 1) projectile.ai[0]++;
			
			if (projectile.ai[0] <= 155) projectile.ai[0]+=3;
			if (projectile.ai[0] >= 365) projectile.ai[0]+=3;
			
			if (projectile.ai[0] <= 255)
			{	
				if (projectile.ai[0] >= 50) {
					if (Main.rand.Next(255 - (int)projectile.ai[0]) <= 20) {
						Vector2 pos = new Vector2(projectile.position.X + 8, projectile.position.Y - 5);
						Main.dust[Dust.NewDust(pos, projectile.width/10 , projectile.height/10, 269, 0f, 0f, 6, default(Color), 0.8f)].velocity *= 0.1f;
					}
					if (Main.rand.Next(20) == 0)
					{
						int dustType = 16;
						Vector2 pos = new Vector2(projectile.position.X+3, projectile.position.Y + 5);
						Main.dust[Dust.NewDust(pos, projectile.width/2, projectile.height/2, dustType)].velocity *= 0.01f;
					}	
				}
				projectile.alpha = (int)(255 - projectile.ai[0]);
			}
			
			if (projectile.ai[0] > 255 && projectile.ai[0] < 750)
			{
				if (projectile.ai[0] <= 450) {
					if (Main.rand.Next(255 + ((int) projectile.ai[0])-255) <= 20) {
						Vector2 pos = new Vector2(projectile.position.X + 8, projectile.position.Y - 5);
						Main.dust[Dust.NewDust(pos, projectile.width/10 , projectile.height/10, 269, 0f, 0f, 6, default(Color), 0.8f)].velocity *= 0.1f;
					}
					if (Main.rand.Next(20) == 0)
					{
						int dustType = 16;
						Vector2 pos = new Vector2(projectile.position.X+3, projectile.position.Y + 5);
						Main.dust[Dust.NewDust(pos, projectile.width/2, projectile.height/4, dustType)].velocity *= 0.01f;
					}		
				}
				projectile.alpha = (int)(projectile.ai[0] - 255);
			}
			
			
			if (projectile.ai[0] >= 510) {
				projectile.ai[0] = 0;
				if (Main.rand.Next(2) == 0) projectile.position.X = player.position.X - 20;
				else projectile.position.X = player.position.X + 20;
				projectile.position.Y = player.position.Y;
			}
			
			projectile.velocity.X = 0;
			projectile.velocity.Y = 0;
		}
	}
}