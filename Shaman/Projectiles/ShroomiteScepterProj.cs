using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod.Shaman;
using System.Collections.Generic;

namespace OrchidMod.Shaman.Projectiles
{
    public class ShroomiteScepterProj : OrchidModShamanProjectile
    {
		private double dustVal = 0;
		private int dustSize = 20;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shroomy Totem");
		}

		public override void SafeSetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = false;
            projectile.aiStyle = 1;
			projectile.timeLeft = 1800;	
			projectile.scale = 1f;

			projectile.aiStyle = 53;
        }

		private void OldAI()
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int range = 200 + (int)projectile.ai[1] * 50;

			if (projectile.velocity.Y < 1f)
			{
				spawnDust(172, range);
				if (projectile.timeLeft % 2 == 0)
				{
					spawnDust(172, dustSize);
				}

				if (projectile.timeLeft % 120 == 0)
				{
					for (int i = 0; i < Main.npc.Length; i++)
					{
						NPC target = Main.npc[i];
						Vector2 center = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
						float offsetX = target.Center.X - center.X;
						float offsetY = target.Center.Y - center.Y;
						float distance = (float)Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
						if (!target.friendly && distance < 500f && projectile.position.X - range + 30 < target.position.X + target.width && projectile.position.X + projectile.width + range + 30 > target.position.X
						&& projectile.position.Y - range + 30 < target.position.Y + target.height && projectile.position.Y + projectile.height + range + 30 > target.position.Y)
						{
							target.StrikeNPCNoInteraction(projectile.damage, 0f, 0);
							if (projectile.ai[1] >= 3)
							{
								OrchidModGlobalNPC modTarget = target.GetGlobalNPC<OrchidModGlobalNPC>();
								modTarget.shamanShroom = 300;
								OrchidModShamanHelper.addShamanicEmpowerment(4, 4, player, modPlayer, mod);
							}
							target.netUpdate = true;
						}
					}
				}
			}

			if (projectile.ai[1] == 5)
			{
				Vector2 center = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
				float offsetX = player.Center.X - center.X;
				float offsetY = player.Center.Y - center.Y;
				float distance = (float)Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
				if (distance < 500f && projectile.position.X - range + 30 < player.position.X + player.width && projectile.position.X + projectile.width + range + 30 > player.position.X
				&& projectile.position.Y - range + 30 < player.position.Y + player.height && projectile.position.Y + projectile.height + range + 30 > player.position.Y)
				{
					player.AddBuff(mod.BuffType("ShroomHeal"), 1);
				}
			}

			this.dustVal++;
			this.dustSize = this.dustSize < range ? this.dustSize + 1 : 20;
		}
		
        public override void AI()
        {
			OldAI();

			Color color = new Color(0.37f, 0.8f, 1f) * 0.5f;
			Lighting.AddLight(projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 offset = new Vector2(-7, -20);
			Color color = Lighting.GetColor((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f), Color.White);
			SpriteEffects effect = projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.position - Main.screenPosition + projectile.Size * 0.5f + offset, null, color, projectile.rotation, projectile.Size * 0.5f, projectile.scale, effect, 0f);
			spriteBatch.Draw(ModContent.GetTexture("OrchidMod/Glowmasks/ShroomiteScepterProj_Glowmask"), projectile.position - Main.screenPosition + projectile.Size * 0.5f + offset, null, new Color(250, 250, 250), projectile.rotation, projectile.Size * 0.5f, projectile.scale, effect, 0f);

			return false; // Let's draw the projectile ourselves
		}

		public override bool OnTileCollide(Vector2 oldVelocity) => false;
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<5; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172);
				Main.dust[dust].velocity = projectile.velocity / 2;
				Main.dust[dust].noGravity = true;
            }
        }

		public void spawnDust(int dustType, int distToCenter)
		{
			for (int i = 0; i < 3; i++)
			{
				double deg = (4 * (42 + this.dustVal) + i * 120);
				double rad = deg * (Math.PI / 180);

				float posX = projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) + projectile.velocity.X - 4;
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) + projectile.velocity.Y - 4;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity = projectile.velocity / 2;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = projectile.velocity.X == 0 ? 1.5f : (float)Main.rand.Next(70, 110) * 0.013f;
				Main.dust[index2].noGravity = true;
			}
		}

	}
}