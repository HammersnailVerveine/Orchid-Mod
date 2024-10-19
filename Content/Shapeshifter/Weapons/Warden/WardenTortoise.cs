using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Shapeshifter.Buffs;
using OrchidMod.Content.Shapeshifter.Projectiles.Warden;
using System.Linq;
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
			Item.width = 32;
			Item.height = 32;
			Item.value = Item.sellPrice(0, 2, 25, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.NPCHit24;
			Item.useTime = 30;
			Item.shootSpeed = 48f;
			Item.knockBack = 5f;
			Item.damage = 53;
			ShapeshiftWidth = 26;
			ShapeshiftHeight = 28;
			ShapeshiftType = ShapeshifterShapeshiftType.Warden;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.Frame = 2;
			anchor.Timespent = 0;
			projectile.direction = player.direction;
			projectile.spriteDirection = player.direction;
			LateralMovement = false;
		}

		public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor)
		{
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// MISC EFFECTS

			if (anchor.Projectile.ai[2] > 0)
			{
				anchor.Projectile.ai[2]--;
			}

			// ANIMATION

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

			intendedVelocity.Y += 0.2f;

			// Normal movement
			if (player.controlLeft || player.controlRight && projectile.ai[0] > -270)
			{ // Player is inputting a movement key and didn't just start blocking
				if (projectile.ai[0] < -5)
				{ // Cancels the block if the player was blocking
					projectile.ai[0] = -5;
					anchor.RightCLickCooldown = 60;
					anchor.NeedNetUpdate = true;
				}
				else
				{
					float speedMult = 1f;
					if (anchor.Projectile.ai[2] > 0)
					{
						speedMult *= 1.75f;
					}

					if (player.controlLeft && !player.controlRight)
					{ // Left movement
						intendedVelocity.X -= 0.1f * speedMult;
						if (intendedVelocity.X < -1.75f * speedMult) intendedVelocity.X = -1.75f * speedMult;
						projectile.direction = -1;
						projectile.spriteDirection = -1;
						LateralMovement = true;
					}
					else if (player.controlRight && !player.controlLeft)
					{ // Right movement
						intendedVelocity.X += 0.1f * speedMult;
						if (intendedVelocity.X > 1.75f * speedMult) intendedVelocity.X = 1.75f * speedMult;
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

			// ATTACK

			if (IsLocalPlayer(player))
			{
				if (CanLeftClick(anchor) && anchor.Projectile.ai[2] <= 0)
				{ // Left click attack
					Vector2 position = projectile.Center;
					Vector2 offSet = Vector2.Normalize(Main.MouseWorld - projectile.Center).RotatedByRandom(MathHelper.ToRadians(5f)) * Item.shootSpeed * Main.rand.NextFloat(0.8f, 1.2f) / 15f;

					for (int i = 0; i < (player.HasBuff<WardenTortoiseBuff>() ? 45 : 15); i++)
					{
						position += Collision.TileCollision(position, offSet, 2, 2, true, false, (int)player.gravDir);

						foreach (NPC npc in Main.npc)
						{
							if (OrchidModProjectile.IsValidTarget(npc))
							{
								if (position.Distance(npc.Center) < npc.width + 32f) // if the NPC is close to the projectile path, snaps to it.
								{
									position = npc.Center;
									break;
								}
							}
						}
					}

					if (player.HasBuff<WardenTortoiseBuff>())
					{
						player.ClearBuff(ModContent.BuffType<WardenTortoiseBuff>());
						int projectileType = ModContent.ProjectileType<WardenTortoiseProj>();
						int damage = shapeshifter.GetShapeshifterDamage(Item.damage * 5f);
						Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), position, Vector2.Zero, projectileType, damage, Item.knockBack * 2f, player.whoAmI, 1f);
						newProjectile.CritChance = shapeshifter.GetShapeshifterCrit(Item.crit);
						SoundEngine.PlaySound(SoundID.Item108, projectile.Center);
					}
					else
					{
						int projectileType = ModContent.ProjectileType<WardenTortoiseProj>();
						int damage = shapeshifter.GetShapeshifterDamage(Item.damage);
						Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), position, Vector2.Zero, projectileType, damage, Item.knockBack, player.whoAmI);
						newProjectile.CritChance = shapeshifter.GetShapeshifterCrit(Item.crit);
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
					//SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, projectile.Center);
				}

				if (CanRightClick(anchor))
				{ // Right click attack
					anchor.NeedNetUpdate = true;
					anchor.RightCLickCooldown = 360;
					anchor.Projectile.ai[0] = -300;
					anchor.Projectile.ai[1] = projectile.spriteDirection;
					projectile.velocity.X = 0f;
					SoundEngine.PlaySound(SoundID.NPCHit24, projectile.Center);
				}

				if (player.controlJump)
				{
					if (anchor.Projectile.ai[2] <= 0)
					{ // Starts sprinting for at least 1 second when pressed
						anchor.Projectile.ai[2] = 60;
						SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, projectile.Center);
					}
					else if (anchor.Projectile.ai[2] <= 5)
					{ // Maintains sprint while pressed
						anchor.Projectile.ai[2] = 5; 
					}
				}
			}

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
				shapeshifter.modPlayer.PlayerImmunity = 40;
				player.immuneTime = 60;
				player.immune = true;
				anchor.RightCLickCooldown = 60;

				player.AddBuff(ModContent.BuffType<WardenTortoiseBuff>(), 600);
				return true;
			}

			return false;
		}
	}
}