using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using ReLogic.Utilities;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Projectiles.Misc
{
	public class StarKO : OrchidModGuardianProjectile
	{
		public override string Texture => $"Terraria/Images/Projectile_79";

		public override void SafeSetDefaults()
		{
			Projectile.tileCollide = false;
			Projectile.timeLeft = 120;
		}

		Vector2 screenTargetPosition;
		Vector2 offs;
		SoundStyle? enemySound = null;
		SlotId soundSlot;
		bool draw = false;

		public override void AI()
		{
			if (Projectile.timeLeft == 120)
			{
				Player player = Main.LocalPlayer;
				if (Main.LocalPlayer.Center.Y < Main.worldSurface * 16)
				{
					draw = true;
					float toNearScreenEdge = (Main.screenPosition.X - player.Center.X) * -0.8f;
					screenTargetPosition = new Vector2((float)Math.Clamp(Projectile.Center.X + Projectile.velocity.X * 40, player.Center.X - toNearScreenEdge, player.Center.X + toNearScreenEdge), MathHelper.Lerp(player.Center.Y, Main.screenPosition.Y, 0.75f)) - Main.screenPosition;
					offs = (Projectile.Center - (Main.screenPosition + screenTargetPosition)).SafeNormalize(Vector2.Zero) * 2f;
					enemySound = Main.npc[(int)Projectile.ai[0]].DeathSound;
					//can't get the trackable sound working for some reason
				}
				Projectile.velocity = Vector2.Zero;
			}
			else if (draw && Projectile.timeLeft == 40)
			{
				SoundEngine.PlaySound(SoundID.Item4.WithVolumeScale(0.3f));
			}
			Projectile.rotation += 0.05f;
			SoundEngine.TryGetActiveSound(soundSlot, out ActiveSound activeSound);
			if (activeSound == null)
			{
				if (enemySound != null)
				{
					float basePitch = enemySound.Value.Pitch;
					float baseVolume = enemySound.Value.Volume;
					var tracker = new ProjectileAudioTracker(Projectile);
					soundSlot = SoundEngine.PlaySound(SoundID.Item29, Main.LocalPlayer.Center, sound =>
					{
						sound.Position = Main.LocalPlayer.Center;
						sound.Pitch = MathHelper.Lerp(-1f, 1f, Math.Min(Projectile.timeLeft - 80, 0) / 40f);
						sound.Volume = MathHelper.Lerp(0f, 1f, Math.Min(Projectile.timeLeft - 80, 0) / 40f);
						return tracker.IsActiveAndInGame();
					});
				}
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			if (draw)
			{
				if (Projectile.timeLeft > 40)
				{
					spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, screenTargetPosition + offs * (Projectile.timeLeft - 40), null, Color.White with {A = 0}, Projectile.rotation, new Vector2(27), (float)Math.Cos(Projectile.timeLeft * 0.039) * -0.4f, SpriteEffects.None, 0);
				}
				else spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, screenTargetPosition, null, Color.White with {A = 0}, Projectile.rotation, new Vector2(27), (float)Math.Sin(Projectile.timeLeft * 0.078f), SpriteEffects.None, 0);
			}
			return false;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			behindNPCsAndTiles.Add(index);
		}
	}
}