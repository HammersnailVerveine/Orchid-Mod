using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Global.Projectiles;
using OrchidMod.Common.ModObjects;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Gambler.Projectiles
{

	public class DungeonCardProjAlt : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Soul Flame");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 26;
			Projectile.aiStyle = 0;
			Projectile.friendly = false;
			Projectile.timeLeft = 600;
			Projectile.scale = 1f;
			Projectile.tileCollide = true;
			Main.projFrames[Projectile.type] = 8;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Player player = Main.player[Main.myPlayer]; // < TEST MP
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			NPC target = Main.npc[(int)Projectile.ai[1]];
			Projectile.velocity *= 0.95f;

			if (modPlayer.modPlayer.Timer120 % 5 == 0)
				Projectile.frame++;
			if (Projectile.frame == 8)
				Projectile.frame = 1;

			if (target.active == false)
			{
				Projectile.Kill();
			}

			if (Projectile.timeLeft < 540)
			{
				for (int k = 0; k < Main.player.Length; k++)
				{
					Player playerMove = Main.player[k];
					Vector2 newMove = playerMove.Center - Projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo < 100f)
					{
						newMove *= 3f / distanceTo;
						Projectile.velocity = newMove;
						Projectile.netUpdate = true;
					}

					if (Projectile.owner == Main.myPlayer)
					{
						if (playerMove.Hitbox.Intersects(Projectile.Hitbox))
						{
							bool dummy = Projectile.GetGlobalProjectile<OrchidGlobalProjectile>().gamblerDummyProj;
							if (!dummy)
							{
								modPlayer.TryAddGamblerChip(50);
							}

							bool crit = (Main.rand.Next(101) <= Projectile.CritChance);
							player.ApplyDamageToNPC(target, Main.DamageVar(Projectile.damage), 0.1f, player.direction, crit);
							OrchidModProjectile.spawnDustCircle(Projectile.Center, 29, 10, 10, true, 1.3f, 1f, 5f, true, true, false, 0, 0, true);
							OrchidModProjectile.spawnDustCircle(target.Center, 29, 10, 10, true, 1.3f, 1f, 5f, true, true, false, 0, 0, true);
							SoundEngine.PlaySound(SoundID.Item45, Projectile.Center);
							Projectile.Kill();
						}
					}
				}
			}
		}

		public override void SafePostAI()
		{
			for (int num46 = Projectile.oldPos.Length - 5; num46 > 0; num46--)
			{
				Projectile.oldPos[num46] = Projectile.oldPos[num46 - 1];
			}
			Projectile.oldPos[0] = Projectile.position;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			Texture2D flameTexture = ModContent.Request<Texture2D>("OrchidMod/Content/Gambler/Projectiles/DungeonCardProjAlt_Glow").Value;
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 1f, Projectile.height * 1f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				drawPos.X += Main.rand.Next(6) - 3 - Main.player[Projectile.owner].velocity.X;
				drawPos.Y += Main.rand.Next(6) - 3 - Main.player[Projectile.owner].velocity.Y;
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k * 5) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(flameTexture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0.3f);
			}
			return true;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 29);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
			return false;
		}
	}
}

