using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter.Buffs;
using OrchidMod.Content.Shapeshifter.Projectiles.Predator;
using OrchidMod.Content.Shapeshifter.Projectiles.Warden;
using OrchidMod.Utilities;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Predator
{
	public class PredatorFossil : OrchidModShapeshifterShapeshift
	{
		public bool LateralMovement = false;

		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 40;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Zombie47;
			Item.useTime = 40;
			Item.shootSpeed = 48f;
			Item.knockBack = 5f;
			Item.damage = 34;
			ShapeshiftWidth = 30;
			ShapeshiftHeight = 40;
			ShapeshiftType = ShapeshifterShapeshiftType.Predator;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
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
			for (int i = 0; i < 8; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}
		}

		public override void ShapeshiftOnLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			Vector2 position = projectile.Center;
			Vector2 offSet = Vector2.Normalize(Main.MouseWorld - projectile.Center).RotatedByRandom(MathHelper.ToRadians(5f)) * Item.shootSpeed * Main.rand.NextFloat(0.8f, 1.2f) / 15f;

			bool foundTarget = false;
			for (int i = 0; i < (player.HasBuff<WardenTortoiseBuff>() ? 45 : 15); i++)
			{
				position += Collision.TileCollision(position, offSet, 2, 2, true, true, (int)player.gravDir);

				foreach (NPC npc in Main.npc)
				{
					if (OrchidModProjectile.IsValidTarget(npc))
					{
						if (position.Distance(npc.Center) < npc.width + 32f) // if the NPC is close to the projectile path, snaps to it.
						{
							foundTarget = true;
							position = npc.Center;
							break;
						}
					}
				}

				if (foundTarget)
				{
					break;
				}
			}

			int projectileType = ModContent.ProjectileType<PredatorFossilProj>();
			int damage = shapeshifter.GetShapeshifterDamage(Item.damage);
			Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), position, offSet * 0.001f, projectileType, damage, Item.knockBack, player.whoAmI);
			newProjectile.CritChance = shapeshifter.GetShapeshifterCrit(Item.crit);
			SoundEngine.PlaySound(SoundID.Zombie33, projectile.Center);

			anchor.LeftCLickCooldown = Item.useTime;
			anchor.Projectile.ai[0] = 10;
			anchor.Projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
			anchor.NeedNetUpdate = true;

			anchor.Frame = 8;
		}

		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.LeftCLickCooldown = Item.useTime;
			anchor.Projectile.ai[0] = 10;
			anchor.Projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
			anchor.NeedNetUpdate = true;


			anchor.RightCLickCooldown = 180;
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// MISC EFFECTS

			float speedMult = GetSpeedMult(player, shapeshifter);
			bool grounded = IsGrounded(projectile, player);

			// ANIMATION

			if (grounded)
			{
				if (anchor.Projectile.ai[0] != 0)
				{ // Override animation during left and right click attack
					projectile.direction = (int)anchor.Projectile.ai[1];
					projectile.spriteDirection = projectile.direction;

					if (anchor.Projectile.ai[0] > 0)
					{ // Left Click
						anchor.Projectile.ai[0]--;
						anchor.Frame = (anchor.Projectile.ai[0] > 5 ? 8 : 9);
					}
					else
					{ // Right Click
						/*
						anchor.Projectile.ai[0]++;
						anchor.Frame = (anchor.Projectile.ai[0] < -5 && anchor.Projectile.ai[0] > -295 ? 6 : 5);
						*/
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
						if (anchor.Frame > 7)
						{
							anchor.Frame = 0;
						}

						anchor.Frame++;

						if (anchor.Frame == 7)
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
				anchor.Frame = 7;
			}

			// MOVEMENT

			Vector2 intendedVelocity = projectile.velocity;
			GravityCalculations(ref intendedVelocity, player);

			if (anchor.IsInputJump)
			{ // Jump
				if (IsGrounded(projectile, player, 4f))
				{
					intendedVelocity.Y = -9.35f;
				}
			}

			// Normal movement
			if (anchor.IsInputLeft || anchor.IsInputRight)
			{ // Player is inputting a movement key
				float acceleration = speedMult;
				if (!grounded) acceleration *= 0.5f;

				if (anchor.IsInputLeft && !anchor.IsInputRight)
				{ // Left movement
					TryAccelerate(ref intendedVelocity, -5f, speedMult, 0.3f, acceleration);
					projectile.direction = -1;
					projectile.spriteDirection = -1;
					LateralMovement = true;
				}
				else if (anchor.IsInputRight && !anchor.IsInputLeft)
				{ // Right movement
					TryAccelerate(ref intendedVelocity, 5f, speedMult, 0.3f, acceleration);
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