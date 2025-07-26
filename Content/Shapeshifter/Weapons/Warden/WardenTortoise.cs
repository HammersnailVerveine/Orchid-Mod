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
	public class WardenTortoise : OrchidModShapeshifterShapeshift
	{
		public bool LateralMovement = false;

		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 1, 55, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.NPCHit24;
			Item.useTime = 30;
			Item.shootSpeed = 48f;
			Item.knockBack = 5f;
			Item.damage = 53;
			ShapeshiftWidth = 26;
			ShapeshiftHeight = 28;
			ShapeshiftType = ShapeshifterShapeshiftType.Warden;
			GroundedWildshape = true;
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

		public override bool ShapeshiftCanLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => base.ShapeshiftCanLeftClick(projectile, anchor, player, shapeshifter) && anchor.Projectile.ai[2] <= 0;

		public override void ShapeshiftOnLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			Vector2 position = projectile.Center;
			Vector2 offSet = Vector2.Normalize(Main.MouseWorld - projectile.Center).RotatedByRandom(MathHelper.ToRadians(5f)) * Item.shootSpeed * Main.rand.NextFloat(0.8f, 1.2f) / 15f;

			bool foundTarget = false;
			for (int i = 0; i < (player.HasBuff<WardenTortoiseBuff>() ? 45 : 15); i++)
			{
				position += TileCollideShapeshifter(position, offSet, 2, 2, true, true, (int)player.gravDir);

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

			if (player.HasBuff<WardenTortoiseBuff>())
			{
				player.ClearBuff(ModContent.BuffType<WardenTortoiseBuff>());
				int projectileType = ModContent.ProjectileType<WardenTortoiseProj>();
				ShapeshifterNewProjectile(shapeshifter, position, offSet * 0.001f, projectileType, Item.damage * 5f, Item.crit, Item.knockBack * 2f, player.whoAmI, 1f);
				SoundEngine.PlaySound(SoundID.Item108, projectile.Center);
			}
			else
			{
				int projectileType = ModContent.ProjectileType<WardenTortoiseProj>();
				ShapeshifterNewProjectile(shapeshifter, position, offSet * 0.001f, projectileType, Item.damage, Item.crit, Item.knockBack, player.whoAmI);
				SoundEngine.PlaySound(SoundID.Zombie33, projectile.Center);
			}

			if (anchor.RightCLickCooldown > 60)
			{
				anchor.RightCLickCooldown = 60;
			}

			anchor.LeftCLickCooldown = Item.useTime;
			anchor.Projectile.ai[0] = 10;
			anchor.Projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
			anchor.NeedNetUpdate = true;

			anchor.Frame = 7;
		}

		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.NeedNetUpdate = true;
			anchor.RightCLickCooldown = 360;
			anchor.Projectile.ai[0] = -300;
			anchor.Projectile.ai[1] = projectile.spriteDirection;
			projectile.velocity.X = 0f;
			SoundEngine.PlaySound(SoundID.NPCHit24, projectile.Center);
		}

		public override void ShapeshiftOnJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			if (anchor.Projectile.ai[2] <= 0)
			{ // Starts sprinting for at least 1 second when pressed
				anchor.Projectile.ai[2] = 60;
				SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, projectile.Center);
				anchor.NeedNetUpdate = true;
			}
			else if (anchor.Projectile.ai[2] <= 5)
			{ // Maintains sprint while pressed
				anchor.Projectile.ai[2] = 5;
			}
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// MISC EFFECTS & ANIMATION

			bool grounded = IsGrounded(projectile, player, 4f);
			float speedMult = GetSpeedMult(player, shapeshifter, anchor, grounded);
			if (anchor.Projectile.ai[2] > 0)
			{
				anchor.Projectile.ai[2]--;
			}

			if (anchor.Projectile.ai[0] != 0)
			{ // Override animation during left and right click attack
				projectile.direction = (int)anchor.Projectile.ai[1];
				projectile.spriteDirection = projectile.direction;

				if (anchor.Projectile.ai[0] > 0)
				{ // Left Click
					anchor.Projectile.ai[0]--;
					anchor.Frame = (anchor.Projectile.ai[0] > 5 ? 7 : 8);
				}
				else
				{ // Right Click
					anchor.Projectile.ai[0]++;
					anchor.Frame = (anchor.Projectile.ai[0] < -5 && anchor.Projectile.ai[0] > -295 ? 6 : 5);
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
					if (anchor.Frame == 5)
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
			GravityCalculations(ref intendedVelocity, player, shapeshifter);

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
						TryAccelerate(ref intendedVelocity, shapeshifter, -1.75f, speedMult, 0.1f);
						projectile.direction = -1;
						projectile.spriteDirection = -1;
						LateralMovement = true;
					}
					else if (anchor.IsInputRight && !anchor.IsInputLeft)
					{ // Right movement
						TryAccelerate(ref intendedVelocity, shapeshifter, 1.75f, speedMult, 0.1f);
						projectile.direction = 1;
						projectile.spriteDirection = 1;
						LateralMovement = true;
					}
					else
					{ // Both keys pressed = no movement
						LateralMovement = false;
						TrySlowDown(ref intendedVelocity, 0.7f, player, shapeshifter, projectile);
					}
				}
			}
			else
			{ // no movement input
				LateralMovement = false;
				TrySlowDown(ref intendedVelocity, 0.7f, player, shapeshifter, projectile);
			}

			FinalVelocityCalculations(ref intendedVelocity, projectile, player, true);

			// POSITION AND ROTATION VISUALS

			anchor.OldPosition.Add(projectile.Center);
			anchor.OldRotation.Add(projectile.rotation);
			anchor.OldFrame.Add(anchor.Frame);

			for (int i = 0; i < 2; i++)
			{
				if (anchor.OldPosition.Count > (anchor.Projectile.ai[2] > 0 ? 6 : 3))
				{
					anchor.OldPosition.RemoveAt(0);
					anchor.OldRotation.RemoveAt(0);
					anchor.OldFrame.RemoveAt(0);
				}
			}
		}

		public override void ShapeshiftModifyHurt(ref Player.HurtModifiers modifiers, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			modifiers.FinalDamage *= 0.8f;
		}

		public override bool ShapeshiftFreeDodge(Player.HurtInfo info, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			if (projectile.ai[0] < 0)
			{
				if (projectile.ai[0] < -5)
				{
					projectile.ai[0] = -5;
				}

				SoundEngine.PlaySound(SoundID.Item37, player.Center);
				shapeshifter.modPlayer.SetDodgeImmuneTime();
				anchor.RightCLickCooldown = 60;

				player.AddBuff(ModContent.BuffType<WardenTortoiseBuff>(), 600);
				return true;
			}

			return false;
		}
	}
}