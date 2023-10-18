using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria.Audio;
using Terraria.DataStructures;

namespace OrchidMod.Content.Gambler.Projectiles
{
	public class SkyCardProjAlt : OrchidModGamblerProjectile
	{
		private int animDirection;
		private int shootProj = -1;
		NPC target = null;
		private Texture2D arrowTexture;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Star");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 720;
			this.bonusTrigger = true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			float lightMult = 0.25f + Math.Abs((1f * Main.player[Main.myPlayer].GetModPlayer<OrchidPlayer>().timer120 - 60) / 30f);
			return lightColor * lightMult;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Cloud);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1.5f;
			}
		}

		public override void OnSpawn(IEntitySource source)
		{
			animDirection = (Main.rand.NextBool(2) ? 1 : -1);
			arrowTexture ??= ModContent.Request<Texture2D>("OrchidMod/Content/Gambler/Projectiles/SkyCardProjAlt_Arrow", AssetRequestMode.ImmediateLoad).Value;
		}

		public override void SafeAI()
		{
			Projectile.velocity *= 0.95f;
			shootProj--;

			Projectile.rotation +=  (0.05f * (0.2f - Math.Abs(Projectile.rotation)) + 0.001f) * animDirection;
			if (Math.Abs(Projectile.rotation) >= 0.2f)
			{
				Projectile.rotation = 0.2f * animDirection;
				animDirection *= -1;
			}

			if (Main.rand.NextBool(10))
			{
				int dustType = DustID.YellowStarDust;
				Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
				Dust dust = Main.dust[Dust.NewDust(pos, Projectile.width, Projectile.height, dustType)];
			}

			if (shootProj < 0)
				target = null;

			float distance = 500f;
			foreach (NPC npc in Main.npc)
			{
				if (homingCheckGambler(npc))
				{
					float newDistance = Vector2.Distance(npc.Center, Projectile.Center);
					if (newDistance < distance)
					{
						target = npc;
						distance = newDistance;
					}
				}
			}

			if (target != null)
			{
				foreach (Projectile proj in Main.projectile)
				{
					if (proj.active && proj.type == ModContent.ProjectileType<SkyCardProj>() && proj.frame == 0 && proj.ai[1] == 2f)
					{
						float distToCenter = proj.Center.Distance(Projectile.Center);
						if (distToCenter < 10f)
						{
							proj.Kill();
							shootProj = 75;
						}
						else if (distToCenter < 100f)
						{
							proj.velocity *= 0.9f;
							Vector2 newVelocity = Projectile.Center - proj.Center;
							newVelocity *= 0.025f;
							proj.velocity += newVelocity;
							proj.timeLeft ++;
						}
					}
				}

				if (shootProj > 0 && shootProj % 15 == 0)
				{
					SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
					Vector2 velocity = target.Center - Projectile.Center;
					velocity.Normalize();
					velocity *= 10f;

					int projType = ProjectileType<Content.Gambler.Projectiles.SkyCardProj>();
					int newProjectile = (DummyProjectile(Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, projType, Projectile.damage, Projectile.knockBack, Projectile.owner), getDummy()));
					Main.projectile[newProjectile].ai[1] = 3f;
					Main.projectile[newProjectile].ai[0] = Projectile.ai[0];
					Main.projectile[newProjectile].alpha = 0;
					Main.projectile[newProjectile].tileCollide = true;
					Main.projectile[newProjectile].friendly = true;
					Main.projectile[newProjectile].netUpdate = true;
				}
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			arrowTexture ??= ModContent.Request<Texture2D>("OrchidMod/Content/Gambler/Projectiles/SkyCardProjAlt_Arrow", AssetRequestMode.ImmediateLoad).Value;
			Vector2 position = Projectile.Center - Main.screenPosition;
			position.X -= arrowTexture.Width / 2f;
			position.Y -= 40 + Math.Abs((1f * Main.player[Main.myPlayer].GetModPlayer<OrchidPlayer>().timer120 - 60) / 5f);
			if (target != null) spriteBatch.Draw(arrowTexture, position, null, Color.White);
			return true;
		}
	}
}