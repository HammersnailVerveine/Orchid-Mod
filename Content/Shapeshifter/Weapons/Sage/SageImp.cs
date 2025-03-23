using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shapeshifter.Misc;
using OrchidMod.Content.Shapeshifter.Projectiles.Sage;
using OrchidMod.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace OrchidMod.Content.Shapeshifter.Weapons.Sage
{
	public class SageImp : OrchidModShapeshifterShapeshift
	{
		public bool LateralMovement = false;
		public bool CanDash = false;

		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 2, 25, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Zombie29;
			Item.useTime = 40;
			Item.shootSpeed = 8f;
			Item.knockBack = 3f;
			Item.damage = 48;
			ShapeshiftWidth = 24;
			ShapeshiftHeight = 26;
			ShapeshiftType = ShapeshifterShapeshiftType.Sage;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			CanDash = false;
			anchor.Frame = 1;
			anchor.Timespent = 0;
			projectile.direction = player.direction;
			projectile.spriteDirection = player.direction;

			LateralMovement = false;

			for (int i = 0; i < 8; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}
		}

		public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			for (int i = 0; i < 5; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			float speedMult = GetSpeedMult(player, shapeshifter);
			player.fallStart = (int)(player.position.Y / 16f);
			player.fallStart2 = (int)(player.position.Y / 16f);
			player.noFallDmg = true;

			if (projectile.ai[2] > -4) projectile.ai[2]--;

			GravityMult = 0.85f;
			if (anchor.IsInputDown) GravityMult += 0.15f;

			// ANIMATION

			if (anchor.Timespent % 5 == 0 && anchor.Timespent > 0)
			{ // Animation frames
				anchor.Frame++;

				if (anchor.Frame == 0)
				{
					anchor.Timespent = -5;
				}

				if (anchor.Frame == 1)
				{
					anchor.Timespent = -3;
				}

				if (anchor.Frame == 6)
				{
					anchor.Frame = 1;
				}
			}

			// ATTACK

			// MOVEMENT

			Vector2 intendedVelocity = projectile.velocity;
			GravityCalculations(ref intendedVelocity, player);

			if (anchor.IsInputJump && intendedVelocity.Y >= 1.2f)
			{ // Gliding
				intendedVelocity.Y = 1.2f;
				if (anchor.Timespent % 4 == 0)
				{
					anchor.Timespent += 2;
				}
			}

			if (projectile.ai[2] > 0)
			{ // Dashing
				intendedVelocity = Vector2.UnitY.RotatedBy(projectile.ai[1]) * -10f * speedMult;
				projectile.direction = intendedVelocity.X > 0 ? 1 : -1;
				projectile.spriteDirection = projectile.direction;

				if (Main.rand.NextBool())
				{
					Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Honey)].noGravity = true;
				}
			}
			else
			{
				if (anchor.IsInputLeft || anchor.IsInputRight)
				{ // Player is inputting a movement key
					if (anchor.IsInputLeft && !anchor.IsInputRight)
					{ // Left movement
						TryAccelerate(ref intendedVelocity, -4f, speedMult, 0.2f);
						projectile.direction = -1;
						projectile.spriteDirection = -1;
						LateralMovement = true;
					}
					else if (anchor.IsInputRight && !anchor.IsInputLeft)
					{ // Right movement
						TryAccelerate(ref intendedVelocity, 4f, speedMult, 0.2f);
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

				float intendedDistance = 32f;
				if (anchor.IsInputDown) intendedDistance -= 16f;
				if (IsGrounded(projectile, player, intendedDistance, anchor.IsInputDown, anchor.IsInputDown))
				{ // Pushes away from the ground
					CanDash = true;
					intendedVelocity.Y -= player.gravity * 1.4f;
					if (intendedVelocity.Y < -2.5f)
					{
						intendedVelocity.Y = -2.5f;
					}
				}
				else if (IsGrounded(projectile, player, intendedDistance + 2f, anchor.IsInputDown, anchor.IsInputDown) && intendedVelocity.Y < 1f)
				{ // Locks up so the screen doesn't shake constantly
					CanDash = true;
					intendedVelocity.Y *= 0f;
				}


				if (projectile.ai[0] > 0)
				{ // Override animation during attack
					projectile.ai[0]--;
					if (projectile.ai[2] < -45)
					{
						projectile.direction = (int)projectile.ai[1];
						projectile.spriteDirection = projectile.direction;
					}
				}
			}

			FinalVelocityCalculations(ref intendedVelocity, projectile, player);

			// POSITION AND ROTATION VISUALS

			anchor.OldPosition.Add(projectile.Center);
			anchor.OldRotation.Add(projectile.rotation);
			anchor.OldFrame.Add(anchor.Frame);

			for (int i = 0; i < 2; i++)
			{
				if (anchor.OldPosition.Count > 5)
				{
					anchor.OldPosition.RemoveAt(0);
					anchor.OldRotation.RemoveAt(0);
					anchor.OldFrame.RemoveAt(0);
				}
			}
		}
	}
}