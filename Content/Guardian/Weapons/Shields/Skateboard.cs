using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Shields
{
	public class Skateboard : OrchidModGuardianShield
	{
		public static readonly SoundStyle SoundTrick = new(OrchidAssets.SoundsPath + "SkateTrick");

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.width = 26;
			Item.height = 42;
			Item.UseSound = SoundID.Item1;
			Item.knockBack = 13f;
			Item.damage = 62;
			Item.rare = ItemRarityID.Green;
			Item.useTime = 35;
			distance = 28f;
			bashDistance = 50f;
			blockDuration = 240;
		}

		public override void BlockStart(Player player, Projectile shield)
		{
			shield.ai[2] = 0;
		}

		public override void SlamHitFirst(Player player, Projectile shield, NPC npc)
		{
			Player owner = Main.player[shield.owner];
			if (shield.ModProjectile is GuardianShieldAnchor anchor)
			{
				if (anchor.aimedLocation.Y > owner.Center.Y && (Math.Abs(anchor.aimedLocation.X - owner.Center.X) < 48f) && owner.grapCount == 0 && owner.mount.Type == MountID.None)
				{
					owner.velocity.Y -= 10f;
					if (owner.velocity.Y > -5) owner.velocity.Y = -5f;
					if (owner.velocity.Y < -14f && !player.controlJump)
					{
						SoundEngine.PlaySound(SoundTrick, shield.Center);
					}
				}
			}
		}

		public override void ExtraAIShield(Projectile projectile)
		{
			if (projectile.ai[0] > 0f && projectile.ModProjectile is GuardianShieldAnchor anchor) // is blocking
			{
				Player owner = Main.player[projectile.owner];
				if (projectile.ai[2] != 0f && (owner.velocity.X == 0f || owner.grapCount > 0 && owner.mount.Type != MountID.None)) // Player hit a tile, stop skating
				{
					projectile.ai[0] = 1f;
					if (Main.rand.NextBool(4)) SoundEngine.PlaySound(SoundID.Item55, projectile.Center);
				}

				// playing is aiming down has no hook or mount, and is falling. Place the shield down and allow skating
				if (anchor.aimedLocation.Y > owner.Center.Y && (Math.Abs(anchor.aimedLocation.X - owner.Center.X) < 32f) && owner.grapCount == 0 && owner.mount.Type == MountID.None && (owner.velocity.Y > 1f || projectile.ai[2] != 0f))
				{
					anchor.aimedLocation = owner.Center - new Vector2(projectile.width / 2f, projectile.height / 2f) + Vector2.UnitY * distance;
					anchor.networkedPosition = anchor.aimedLocation;
					projectile.rotation = -MathHelper.PiOver2;

					// Collision with the ground, do skating stuff
					Vector2 collision = Collision.TileCollision(owner.position + new Vector2(owner.width / 2f, owner.height), Vector2.UnitY * 8f, Item.width, 14, false, false, (int)owner.gravDir);
					if (collision != Vector2.UnitY * 8f)
					{
						owner.position.Y += (collision.Y - 1.7f);
						owner.velocity.X = projectile.ai[2];
						owner.velocity.Y = 0.1f;

						if (Main.rand.NextBool(4)) SoundEngine.PlaySound(SoundID.Item55, projectile.Center);
					}
					else
					{
						if (projectile.ai[2] == 0) SoundEngine.PlaySound(SoundID.Item53, projectile.Center);
						projectile.ai[2] = owner.velocity.X;
						if (Math.Abs(projectile.ai[2]) < 8f) projectile.ai[2] = 8f * Math.Sign(projectile.ai[2]);
					}
				}
			}
		}
	}
}
