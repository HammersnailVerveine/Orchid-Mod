using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Attributes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Standards
{
	[CrossmodContent("ThoriumMod")]
	public class ThoriumStarScouterStandard : OrchidModGuardianStandard
	{
		public override void SafeSetDefaults()
		{
			Item.width = 42;
			Item.height = 42;
			Item.value = Item.sellPrice(0, 0, 54, 0);
			Item.rare = ItemRarityID.Orange;
			Item.useTime = 42;
			Item.UseSound = SoundID.Item113;
			GuardStacks = 2;
			FlagOffset = 6;
			AuraRange = 20;
			StandardDuration = 1800;
			AffectNearbyPlayers = true;
			AffectNearbyNPCs = true;
			//BaseSyncedValue = -4f;
		}

		public override Color GetColor()
		{
			return new Color(164, 46, 255);
		}

		public override bool NearbyPlayerEffect(GuardianStandardStats standardStats, Player affectedPlayer, OrchidGuardian guardian, bool isLocalPlayer, bool reinforced)
		{
			affectedPlayer.GetModPlayer<OrchidGuardian>().GuardianStandardStarScouter = guardian.Player.whoAmI;
			if (reinforced && isLocalPlayer)
			{
				guardian.GuardianStandardStarScouterWarp = true;
			}
			//Dust.NewDustPerfect(guardian.Player.Center + new Vector2(-14 * guardian.Player.direction, -28 * guardian.Player.gravDir) - new Vector2(4, 4), DustID.ShadowbeamStaff);
			return true;
		}

		public override void ExtraAIStandardWorn(GuardianStandardAnchor anchor, Projectile projectile, Player player, OrchidGuardian guardian)
		{
			// Turns out this is actually works without manual sync. Leaving code commented just in case and as an example on how to use anchor.SyncedValue

			/*
			if (anchor.SyncedValue != BaseSyncedValue)
			{ // SyncedValue can be automatically synced to all other clients by calling anchor.UpdateAndSyncValue(float valueToSync) somewhere
				if (!IsLocalPlayer(player) && false)
				{ // This will sync the warp on other clients, the sync is called when "GuardianStandardStarScouterWarpCD == 20" in OrchidGuardian
					Vector2 warp = Vector2.Zero;
					if (anchor.SyncedValue > BaseSyncedValue)
					{
						warp = anchor.SyncedValue.ToRotationVector2();
						int clipPrevention = 3 + (warp.X != 0 ? 7 : 0) + (warp.Y != 0 ? 20 : 0);
						int dist = clipPrevention;
						for (int i = 160; i >= 1; i /= 2)
						{
							if (Collision.CanHit(player.Center, 0, 0, player.Center + warp * (dist + i), 0, 0))
								dist += i;
						}
						player.velocity = warp * 8f;
						warp *= dist - clipPrevention;
						dist = (dist - clipPrevention) / 4;
						player.position += warp;
						SoundEngine.PlaySound(SoundID.Item163.WithVolumeScale(0.3f).WithPitchOffset(1), player.position);
						for (int i = 0; i < dist; i++)
						{
							float currPos = i * 1f / dist;
							Dust dust = Dust.NewDustDirect(player.position - warp * currPos - new Vector2(4, 4), player.width, player.height, DustID.ShadowbeamStaff);
							dust.noGravity = true;
							dust.scale *= 2.5f - 1 * currPos;
							dust.velocity += player.velocity * 2;
						}
					}

					Main.NewText("a");
					//guardian.GuardianStandardStarScouterWarpCD = 19;
				}
				anchor.SyncedValue = BaseSyncedValue;
			}
			*/
		}

		public override bool DrawCustomFlag(SpriteBatch spriteBatch, Projectile projectile, Player player, Color lightColor, Vector2 drawPosition, float drawRotation)
		{
			GuardianStandardAnchor anchor = projectile.ModProjectile as GuardianStandardAnchor;
			var effect = projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Texture2D textureUp = ModContent.Request<Texture2D>(FlagUpTexture).Value;
			float oscillation = (float)Math.Sin(projectile.ai[1] * 0.08f) * 2f;
			float intensity = Math.Min(1, projectile.ai[1] / 600f);
			Vector2 offs = new Vector2(oscillation).RotatedBy(drawRotation + MathHelper.PiOver2) - player.velocity * 0.5f;
			offs = offs * intensity + new Vector2(2).RotatedBy(drawRotation + MathHelper.PiOver2) * (1 - intensity);
			spriteBatch.Draw(textureUp, drawPosition + offs, null, lightColor, drawRotation, textureUp.Size() * 0.5f, projectile.scale, effect, 0f);
			if (anchor.Reinforced)
				spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_FlagGlow").Value, drawPosition + offs, null, Color.White * (intensity + -oscillation * 0.25f), drawRotation, textureUp.Size() * 0.5f, projectile.scale, effect, 0f);
			return true;
		}
	}
}
