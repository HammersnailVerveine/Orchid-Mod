using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria.Audio;

namespace OrchidMod.Gambler.Projectiles
{
	public class SkyCardProjAlt : OrchidModGamblerProjectile
	{
		private int animDirection;
		private int shootProj = -1;
		NPC target = null;
		private Texture2D arrowTexture;
		private Texture2D glowTexture;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Banana");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 28;
			Projectile.height = 26;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 720;
			this.bonusTrigger = true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Cloud);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1.5f;
			}
		}

		public override void OnSpawn()
		{
			animDirection = (Main.rand.NextBool(2) ? 1 : -1);
			arrowTexture ??= ModContent.Request<Texture2D>("OrchidMod/Gambler/Projectiles/SkyCardProjAlt_Arrow", AssetRequestMode.ImmediateLoad).Value;
			glowTexture ??= ModContent.Request<Texture2D>("OrchidMod/Gambler/Projectiles/SkyCardProjAlt_Glow", AssetRequestMode.ImmediateLoad).Value;
		}

		public override void SafeAI()
		{
			Projectile.velocity *= 0.95f;
			Projectile.rotation += 0.01f * animDirection;
			shootProj--;

			if (Main.rand.NextBool(10))
			{
				int dustType = DustID.YellowStarDust;
				Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
				Dust dust = Main.dust[Dust.NewDust(pos, Projectile.width, Projectile.height, dustType)];
			}

			foreach (Projectile proj in Main.projectile)
			{
				if (proj.active && proj.type == ModContent.ProjectileType<SkyCardProj>() && proj.frame == 0 && proj.ai[1] == 2f && proj.Center.Distance(Projectile.Center) < 25f) 
				{
					float distance = 500f;
					foreach (NPC npc in Main.npc)
					{
						if (homingCheckGambler(npc))
						{
							float newDistance = Vector2.Distance(npc.Center, proj.Center);
							if (newDistance < distance)
							{
								target = npc;
								distance = newDistance;
							}
						}
					}

					if (target != null)
					{
						proj.Kill();
						shootProj = 75;
					}
				}
			}

			if (shootProj > 0 && shootProj % 15 == 0 && target != null)
			{
				SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
				Vector2 velocity = target.Center - Projectile.Center;
				velocity.Normalize();
				velocity *= 10f;

				int projType = ProjectileType<Gambler.Projectiles.SkyCardProj>();
				int newProjectile = (DummyProjectile(Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, projType, Projectile.damage, Projectile.knockBack, Projectile.owner), getDummy()));
				Main.projectile[newProjectile].ai[1] = 3f; 
				Main.projectile[newProjectile].ai[0] = Projectile.ai[0];
				Main.projectile[newProjectile].alpha = 0;
				Main.projectile[newProjectile].tileCollide = true;
				Main.projectile[newProjectile].friendly = true;
				Main.projectile[newProjectile].netUpdate = true;
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			arrowTexture ??= ModContent.Request<Texture2D>("OrchidMod/Gambler/Projectiles/SkyCardProjAlt_Arrow", AssetRequestMode.ImmediateLoad).Value;
			glowTexture ??= ModContent.Request<Texture2D>("OrchidMod/Gambler/Projectiles/SkyCardProjAlt_Glow", AssetRequestMode.ImmediateLoad).Value;
			Color yellow = Color.Yellow * 0.5f;
			Color newLightColor = new Color(yellow.R + lightColor.R, yellow.G + lightColor.G, yellow.B + lightColor.B);
			Vector2 position = Projectile.Center - Main.screenPosition;
			position.X -= arrowTexture.Width / 2f;
			position.Y -= 40 + Math.Abs((1f * Main.player[Main.myPlayer].GetModPlayer<OrchidPlayer>().timer120 - 60) / 5f);
			spriteBatch.Draw(arrowTexture, position, null, newLightColor);
			spriteBatch.Draw(glowTexture, Projectile.position, null, newLightColor, Projectile.rotation, Vector2.Zero, Projectile.scale, SpriteEffects.None, 0f);
			return true;
		}
	}
}