using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using OrchidMod.Common.ModObjects;

namespace OrchidMod.Content.Guardian.Weapons.Shields
{
	public class MoonLordShield : OrchidModGuardianShield
	{
		public float playerVelocity;
		public float originalHeight;
		public int TimeSpent;

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.width = 40;
			Item.height = 52;
			Item.UseSound = SoundID.Item115;
			Item.knockBack = 6f;
			Item.damage = 963;
			Item.rare = ItemRarityID.Red;
			Item.useTime = 60;
			distance = 28f;
			slamDistance = 150f;
			blockDuration = 600;
		}

		public override void Slam(Player player, Projectile shield)
		{
			Player owner = Main.player[shield.owner];
			if (shield.ModProjectile is GuardianShieldAnchor anchor)
			{
				if (owner.mount.Type == MountID.None)
				{
					OrchidPlayer orchidPlayer = player.GetModPlayer<OrchidPlayer>();
					Vector2 playerDashVelocity = Vector2.UnitY.RotatedBy((player.Center - Main.MouseWorld).ToRotation() - MathHelper.PiOver2) * 20f;
					orchidPlayer.ForcedVelocityVector = playerDashVelocity;
					orchidPlayer.ForcedVelocityTimer = 5;
					orchidPlayer.ForcedVelocityUpkeep = 1f;
				}
			}
		}


		public override void BlockStart(Player player, Projectile shield)
		{
			playerVelocity = 0;
			player.velocity *= 0f;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			spriteBatch.DrawSimpleItemGlowmaskInWorld(Item, Main.DiscoColor, rotation, scale);
		}

		public override void ExtraAIShield(Projectile projectile)
		{
			TimeSpent++;
			if (projectile.ai[1] > 0f) // is slamming
			{

				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.RainbowMk2, newColor: Main.DiscoColor);
				dust.noGravity = true;
				dust.velocity *= 0.5f;
				dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.RainbowMk2, newColor: Main.DiscoColor);
				dust.noGravity = true;
				dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Smoke, newColor: Main.DiscoColor);
				dust.noGravity = true;
				dust.velocity *= 0.5f;
				dust.scale *= Main.rand.NextFloat(1f, 1.5f);
				dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Smoke, newColor: Main.DiscoColor);
				dust.noGravity = true;
				dust.scale *= Main.rand.NextFloat(1f, 1.5f);
			}

			if (projectile.ai[0] > 0f && projectile.ModProjectile is GuardianShieldAnchor anchor) // is blocking
			{
				Player owner = Main.player[projectile.owner];
				Vector2 collision = Collision.TileCollision(owner.position + new Vector2((owner.width - Item.width) * 0.5f, owner.height), Vector2.UnitY * 12f, Item.width, 14, false, false, (int)owner.gravDir);
				if (playerVelocity != 0f && ((owner.velocity.X == 0f || owner.grapCount > 0 && owner.mount.Type != MountID.None) || collision != Vector2.UnitY * 12f)) // Player hit a tile, stop skating
				{
					projectile.ai[0] = 1f;
				}

				// playing is aiming down has no hook or mount, and is falling. Place the shield down and allow skating
				if (anchor.aimedLocation.Y > owner.Center.Y && (Math.Abs(anchor.aimedLocation.X - owner.Center.X) < 48f) && owner.grapCount == 0 && owner.mount.Type == MountID.None)
				{
					if (projectile.ai[2] != -MathHelper.PiOver2)
					{
						anchor.aimedLocation = owner.Center.Floor() - new Vector2(projectile.width / 2f, projectile.height / 2f) + Vector2.UnitY * distance;
						projectile.ai[2] = -MathHelper.PiOver2; // networkedrotation
						projectile.rotation = -MathHelper.PiOver2;
						originalHeight = owner.position.Y;
						SoundEngine.PlaySound(SoundID.Item114, projectile.Center);
					}

					owner.velocity.Y = -0.4f;
					if (originalHeight != 0f) owner.position.Y = originalHeight;
					if (TimeSpent % 21 == 0) SoundEngine.PlaySound(SoundID.Item24, projectile.Center);

					Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(13, 1), 6, 6, DustID.Smoke, newColor: Main.DiscoColor);
					dust.noGravity = true;
					dust.velocity *= 0.5f;
					dust.velocity.Y = 1f;
					dust.scale *= Main.rand.NextFloat(1f, 1.5f);
					dust = Dust.NewDustDirect(projectile.Center + new Vector2(7, 1), 6, 6, DustID.Smoke, newColor: Main.DiscoColor);
					dust.noGravity = true;
					dust.velocity *= 0.5f;
					dust.velocity.Y = 1f;
					dust.scale *= Main.rand.NextFloat(1f, 1.5f);

					if (playerVelocity != 0)
					{
						owner.velocity.Y -= -0.00001f;
						owner.fallStart = (int)(owner.position.Y / 16f);
						owner.fallStart2 = (int)(owner.position.Y / 16f);
						owner.velocity.X = playerVelocity;

						dust = Dust.NewDustDirect(projectile.Center - new Vector2(13, 1), 6, 6, DustID.RainbowMk2, -playerVelocity * 0.5f, 0.5f, newColor: Main.DiscoColor);
						dust.noGravity = true;
						dust = Dust.NewDustDirect(projectile.Center + new Vector2(7, 1), 6, 6, DustID.RainbowMk2, -playerVelocity * 0.5f, 0.5f, newColor: Main.DiscoColor);
						dust.noGravity = true;
					}
					else
					{
						playerVelocity = owner.velocity.X;
						if (playerVelocity != 0f)
						{
							if (Math.Abs(playerVelocity) < 8f) playerVelocity = 15f * Math.Sign(playerVelocity);
							SoundEngine.PlaySound(SoundID.Item115, projectile.Center);
						}
					}
				}
			}
		}
	}
}
