using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter.Buffs;
using OrchidMod.Content.Shapeshifter.Projectiles.Sage;
using OrchidMod.Content.Shapeshifter.Projectiles.Warden;
using OrchidMod.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Warden
{
	public class WardenSlime : OrchidModShapeshifterShapeshift
	{
		public bool LateralMovement = false;
		public int jumpCooldown = 0;
		public float JumpCharge = 0f;
		public bool ChargeCue = false;

		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 0, 35, 0);
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item154; // use item167 for slam
			Item.useTime = 30;
			Item.shootSpeed = 10f;
			Item.knockBack = 10f;
			Item.damage = 30;
			ShapeshiftWidth = 26;
			ShapeshiftHeight = 28;
			ShapeshiftType = ShapeshifterShapeshiftType.Warden;
			Grounded = true;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.Frame = 0;
			anchor.Timespent = 0;
			projectile.direction = player.direction;
			projectile.spriteDirection = player.direction;
			jumpCooldown = 0;
			JumpCharge = 0f;
			ChargeCue = false;

			for (int i = 0; i < 10; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}
		}

		public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			for (int i = 0; i < 7; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// ai[0] stores the lateral movement speed cap (increased after a jump, defaults to 2.5f)

			bool grounded = IsGrounded(projectile, player, 4f);
			float speedMult = GetSpeedMult(player, shapeshifter, anchor, grounded);
			jumpCooldown--;

			projectile.ai[0] -= 0.1f;
			if (projectile.ai[0] < 2.5f)
			{
				projectile.ai[0] = 2.5f;
			}

			// MISC EFFECTS & ANIMATION

			if (JumpCharge > 0f)
			{ // charging
				if (JumpCharge > 20f)
				{
					anchor.Frame = 2;
				}
				else
				{
					anchor.Frame = 1;
				}
			}
			else if (projectile.velocity.Y < 0f)
			{ // ascending
				if (anchor.Timespent < 0)
				{
					anchor.Frame = 6;
				}
				else
				{
					anchor.Frame = 5;
				}
			}
			else if (grounded)
			{
				anchor.Frame = 0;
			}
			else
			{ // descending
				if (projectile.velocity.Y < 4f)
				{
					anchor.Frame = 3;
				}
				else
				{
					anchor.Frame = 4;
				}
			}

			// ATTACK & MOVEMENT

			Vector2 intendedVelocity = projectile.velocity;
			GravityCalculations(ref intendedVelocity, player);

			// Jump Attack Charge
			if (anchor.IsLeftClick && grounded)
			{ // Charges the jump while left clicking & grounded. This also prevents normal movement
				if (JumpCharge == 0f)
				{
					SoundEngine.PlaySound(SoundID.Item65, projectile.Center);
					ChargeCue = false;
				}

				JumpCharge += shapeshifter.GetShapeshifterMeleeSpeed();

				if (JumpCharge >= 60 && !ChargeCue)
				{
					ChargeCue = true;
					anchor.Blink(true);
				}
			}
			else
			{
				if (JumpCharge >= 60)
				{ // Full charge jump
					anchor.Timespent = -15;
					intendedVelocity.Y = -15f;
					jumpCooldown = 10;
					projectile.ai[0] = 5.5f;

					if (anchor.IsInputLeft && !anchor.IsInputRight)
					{ // Left movement
						TryAccelerate(ref intendedVelocity, -projectile.ai[0], speedMult, 5.5f);
						projectile.direction = -1;
						projectile.spriteDirection = -1;
						LateralMovement = true;
					}
					else if (anchor.IsInputRight && !anchor.IsInputLeft)
					{ // Right movement
						TryAccelerate(ref intendedVelocity, projectile.ai[0], speedMult, 5.5f);
						projectile.direction = 1;
						projectile.spriteDirection = 1;
						LateralMovement = true;
					}

					for (int i = 0; i < 15; i++)
					{
						Dust dust = Dust.NewDustDirect(projectile.position + new Vector2(0f, projectile.height), projectile.width, 0, DustID.Smoke);
						dust.scale *= Main.rand.NextFloat(0.6f, 0.8f);
						dust.velocity *= 0.25f;
						dust.velocity.Y -= 0.65f;
					}

					SoundStyle sound = SoundID.Item154;
					sound.Pitch += Main.rand.NextFloat(0.2f, 0.3f);
					sound.Volume *= 1.2f;
					SoundEngine.PlaySound(sound, projectile.Center);
				}
				else if (JumpCharge > 0)
				{ // Jump not fully charged, do a normal one
					anchor.Timespent = -12;
					intendedVelocity.Y = -10f;

					for (int i = 0; i < 8; i++)
					{
						Dust dust = Dust.NewDustDirect(projectile.position + new Vector2(0f, projectile.height), projectile.width, 0, DustID.Smoke);
						dust.scale *= Main.rand.NextFloat(0.6f, 0.8f);
						dust.velocity *= 0.25f;
						dust.velocity.Y -= 0.75f;
					}

					SoundEngine.PlaySound(SoundID.Item154, projectile.Center);
				}

				JumpCharge = 0;
			}

			// Normal movement
			if ((anchor.IsInputLeft || anchor.IsInputRight || player.controlJump) && JumpCharge <= 0f)
			{ // Player is inputting a movement key
				if (anchor.IsInputLeft && !anchor.IsInputRight)
				{ // Left movement
					TryAccelerate(ref intendedVelocity, -projectile.ai[0], speedMult, 0.1f);
					projectile.direction = -1;
					projectile.spriteDirection = -1;
					LateralMovement = true;
				}
				else if (anchor.IsInputRight && !anchor.IsInputLeft)
				{ // Right movement
					TryAccelerate(ref intendedVelocity, projectile.ai[0], speedMult, 0.1f);
					projectile.direction = 1;
					projectile.spriteDirection = 1;
					LateralMovement = true;
				}
				else
				{ // Both keys pressed = no movement
					LateralMovement = false;
					intendedVelocity.X *= 0.7f;
				}

				if (grounded && intendedVelocity.Y > 0f && jumpCooldown <= 0)
				{
					jumpCooldown = 10;

					if (player.controlJump)
					{
						anchor.Timespent = -12;
						intendedVelocity.Y = -10f;

						for (int i = 0; i < 8; i++)
						{
							Dust dust = Dust.NewDustDirect(projectile.position + new Vector2(0f, projectile.height), projectile.width, 0, DustID.Smoke);
							dust.scale *= Main.rand.NextFloat(0.6f, 0.8f);
							dust.velocity *= 0.25f;
							dust.velocity.Y -= 0.75f;
						}

						SoundEngine.PlaySound(SoundID.Item154, projectile.Center);
					}
					else
					{
						anchor.Timespent = -10;
						intendedVelocity.Y = -6f;

						for (int i = 0; i < 5; i++)
						{
							Dust dust = Dust.NewDustDirect(projectile.position + new Vector2(0f, projectile.height), projectile.width, 0, DustID.Smoke);
							dust.scale *= Main.rand.NextFloat(0.6f, 0.8f);
							dust.velocity *= 0.25f;
							dust.velocity.Y -= 0.5f;
						}

						SoundStyle sound = SoundID.Item154;
						sound.Pitch -= Main.rand.NextFloat(0.2f, 0.3f);
						sound.Volume *= 0.5f;
						SoundEngine.PlaySound(sound, projectile.Center);
					}
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
				if (anchor.OldPosition.Count > (projectile.ai[0] > 2.5f ? 6 : 4))
				{
					anchor.OldPosition.RemoveAt(0);
					anchor.OldRotation.RemoveAt(0);
					anchor.OldFrame.RemoveAt(0);
				}
			}
		}
		public override Color GetColorGlow(ref bool drawPlayerAsAdditive, Color lightColor, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => player.GetImmuneAlphaPure(lightColor * 2.5f, 0f);
	}
}