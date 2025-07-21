using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter.Projectiles.Predator;
using OrchidMod.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Predator
{
	public class PredatorGoblin : OrchidModShapeshifterShapeshift
	{
		public bool LateralMovement = false;

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 34;
			Item.value = Item.sellPrice(0, 0, 48, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.NPCHit6;
			Item.useTime = 30;
			Item.shootSpeed = 100f;
			Item.knockBack = 10f;
			Item.damage = 25;
			ShapeshiftWidth = 28;
			ShapeshiftHeight = 30;
			ShapeshiftType = ShapeshifterShapeshiftType.Predator;
			GroundedWildshape = true;
		}

		public override void ShapeshiftAnchorOnShapeshiftFast(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			projectile.ai[0] = 4f;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.Frame = 1;
			anchor.Timespent = 0;
			projectile.direction = player.direction;
			projectile.spriteDirection = player.direction;
			LateralMovement = false;

			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Shadowflame);
				dust.scale *= Main.rand.NextFloat(2f, 2.5f);
				dust.velocity *= Main.rand.NextFloat(0.25f, 0.5f);
				dust.noGravity = true;
				dust.noLightEmittence = true;
			}

			for (int i = 0; i < 8; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}
		}

		public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Shadowflame);
				dust.scale *= Main.rand.NextFloat(2f, 2.5f);
				dust.velocity *= Main.rand.NextFloat(0.25f, 0.5f);
				dust.noGravity = true;
				dust.noLightEmittence = true;
			}

			for (int i = 0; i < 8; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}
		}
		public override void ShapeshiftOnLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
		}

		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// anchor.ai[2] holds bonus jumps

			// MISC EFFECTS
			bool grounded = IsGrounded(projectile, player, 4f);
			float speedMult = GetSpeedMult(player, shapeshifter, anchor, grounded);


			if (grounded)
			{
				anchor.ai[2] = 2;
			}

			// ANIMATION

			if (grounded)
			{
				if (LateralMovement)
				{ // Player is moving left or right, cycle through frames
					if (anchor.Timespent % 4 == 0 && anchor.Timespent > 0)
					{
						anchor.Frame++;
						if (anchor.Frame >= 9)
						{
							anchor.Frame = 1;
						}
					}
				}
				else
				{ // idle frame
					anchor.Timespent = 0;
					anchor.Frame = 0;
				}
			}
			else
			{ // Falling frame
				anchor.Timespent = 0;
				anchor.Frame = 9;
			}

			// MOVEMENT

			Vector2 intendedVelocity = projectile.velocity;
			GravityCalculations(ref intendedVelocity, player, shapeshifter);

			if (anchor.IsInputJump)
			{
				if (!grounded && anchor.ai[2] > 0 && anchor.JumpWithControlRelease(player))
				{ // double jump
					anchor.ai[2]--;

					TryJump(ref intendedVelocity, 5.5f, player, shapeshifter, anchor, speedEfficiency: 0f);
					intendedVelocity.X = 10f * projectile.spriteDirection * speedMult * shapeshifter.ShapeshifterMoveSpeedMiscOverride;

					SoundEngine.PlaySound(SoundID.DoubleJump, projectile.Center);

					for (int i = 0; i < 5; i++)
					{
						Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
					}

					for (int i = 0; i < 3; i++)
					{
						Gore gore = Gore.NewGoreDirect(projectile.GetSource_FromAI(), projectile.Center + new Vector2(Main.rand.NextFloat(-24f, 0f), Main.rand.NextFloat(-24f, 0f)), Vector2.UnitY.RotatedByRandom(MathHelper.Pi), 61 + Main.rand.Next(3));
						gore.rotation = Main.rand.NextFloat(MathHelper.Pi);
						gore.velocity.Y += Main.rand.NextFloat(0.25f, 0.5f);
					}
				}
				else
				{ // grounded jump
					anchor.JumpWithControlRelease(player);
					TryJump(ref intendedVelocity, 9.3f, player, shapeshifter, anchor, true);
				}
			}

			// Normal movement
			if (anchor.IsInputLeft || anchor.IsInputRight)
			{ // Player is inputting a movement key
				float acceleration = speedMult;
				if (!grounded) acceleration *= 0.5f;

				if (anchor.IsInputLeft && !anchor.IsInputRight)
				{ // Left movement
					TryAccelerate(ref intendedVelocity, -4.25f, speedMult, 0.3f, acceleration);
					projectile.direction = -1;
					projectile.spriteDirection = -1;
					LateralMovement = true;
				}
				else if (anchor.IsInputRight && !anchor.IsInputLeft)
				{ // Right movement
					TryAccelerate(ref intendedVelocity, 4.25f, speedMult, 0.3f, acceleration);
					projectile.direction = 1;
					projectile.spriteDirection = 1;
					LateralMovement = true;
				}
				else
				{ // Both keys pressed = no movement
					LateralMovement = false;
					intendedVelocity.X *= 0.7f;
				}
			}
			else
			{ // no movement input
				LateralMovement = false;
				intendedVelocity.X *= 0.7f;
			}

			FinalVelocityCalculations(ref intendedVelocity, projectile, player, true);

			// POSITION AND ROTATION VISUALS

			anchor.OldPosition.Add(projectile.Center);
			anchor.OldRotation.Add(projectile.rotation);
			anchor.OldFrame.Add(anchor.Frame);

			for (int i = 0; i < 2; i++)
			{
				if (anchor.OldPosition.Count > (projectile.ai[1] > 5 ? 7 : 5))
				{
					anchor.OldPosition.RemoveAt(0);
					anchor.OldRotation.RemoveAt(0);
					anchor.OldFrame.RemoveAt(0);
				}
			}
		}
	}
}