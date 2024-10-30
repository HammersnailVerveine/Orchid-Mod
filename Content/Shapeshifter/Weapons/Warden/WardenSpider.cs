using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter.Buffs;
using OrchidMod.Content.Shapeshifter.Projectiles.Warden;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Warden
{
	public class WardenSpider : OrchidModShapeshifterShapeshift
	{
		public bool LateralMovement = false;

		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.NPCHit24;
			Item.useTime = 30;
			Item.shootSpeed = 48f;
			Item.knockBack = 5f;
			Item.damage = 26;
			ShapeshiftWidth = 24;
			ShapeshiftHeight = 24;
			ShapeshiftType = ShapeshifterShapeshiftType.Warden;
			MeleeSpeedLeft = true;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.Frame = 0;
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


		public override void ShapeshiftOnLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.LeftCLickCooldown = Item.useTime;
			anchor.Projectile.ai[0] = 15;
			anchor.Projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
			anchor.NeedNetUpdate = true;

			anchor.Frame = 7;
		}

		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.NeedNetUpdate = true;
			anchor.RightCLickCooldown = 360;
			//anchor.Projectile.ai[0] = -300;
			anchor.Projectile.ai[1] = projectile.spriteDirection;
			projectile.velocity.X = 0f;
			SoundEngine.PlaySound(SoundID.NPCHit24, projectile.Center);
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// MISC EFFECTS & ANIMATION

			float speedMult = GetSpeedMult(player, shapeshifter);

			if (anchor.Projectile.ai[0] != 0)
			{ // Override animation during left and right click attack
				projectile.direction = (int)anchor.Projectile.ai[1];
				projectile.spriteDirection = projectile.direction;

				if (anchor.Projectile.ai[0] > 0)
				{ // Left Click
					anchor.Projectile.ai[0]--;
					anchor.Projectile.ai[0]--;
					anchor.Frame = (anchor.Projectile.ai[0] > 8 ? 4 : 5);

					if (anchor.Projectile.ai[0] < 0)
					{
						anchor.Projectile.ai[0] = 0;
					}
				}

				if (anchor.Projectile.ai[0] == 0)
				{ // Puts the animation back on track
					anchor.Frame = 0;
				}
			}
			else if (LateralMovement)
			{ // Player is moving left or right, cycle through frames
				if (anchor.Timespent % 4 == 0 && anchor.Timespent > 0)
				{
					anchor.Frame++;
					if (anchor.Frame == 4)
					{
						anchor.Frame = 1;
					}
				}
			}
			else
			{
				anchor.Timespent = 0;
				anchor.Frame = 0;
			}

			// MOVEMENT

			Vector2 intendedVelocity = projectile.velocity;
			GravityCalculations(ref intendedVelocity, player);

			// Normal movement
			if ((anchor.IsInputLeft || anchor.IsInputRight) && projectile.ai[0] > -270)
			{ // Player is inputting a movement key and didn't just start blocking
				if (projectile.ai[0] < -5)
				{ // Cancels the block if the player was blocking
					projectile.ai[0] = -5;
					anchor.RightCLickCooldown = 60;
					anchor.NeedNetUpdate = true;
				}
				else
				{
					if (anchor.Projectile.ai[2] > 0)
					{
						speedMult *= 1.75f;
					}

					if (anchor.IsInputLeft && !anchor.IsInputRight)
					{ // Left movement
						TryAccelerateX(ref intendedVelocity, -4f, speedMult, 0.4f);
						projectile.direction = -1;
						projectile.spriteDirection = -1;
						LateralMovement = true;
					}
					else if (anchor.IsInputRight && !anchor.IsInputLeft)
					{ // Right movement
						TryAccelerateX(ref intendedVelocity, 4f, speedMult, 0.4f);
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