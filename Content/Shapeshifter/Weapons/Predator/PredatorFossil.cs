using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter.Buffs;
using OrchidMod.Content.Shapeshifter.Projectiles.Predator;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Predator
{
	public class PredatorFossil : OrchidModShapeshifterShapeshift
	{
		public bool LateralMovement = false;
		public Vector2 OffsetToHookedTarget = Vector2.Zero;
		public bool ImprovedDash = false;

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
			Item.damage = 28;
			ShapeshiftWidth = 30;
			ShapeshiftHeight = 40;
			ShapeshiftType = ShapeshifterShapeshiftType.Predator;
			Grounded = true;
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

		public override bool ShapeshiftCanLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => base.ShapeshiftCanLeftClick(projectile, anchor, player, shapeshifter) && projectile.ai[0] == 0f;

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
			projectile.ai[0] = 10;
			if (projectile.ai[2] == 0) projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
			anchor.NeedNetUpdate = true;

			anchor.Frame = 8;
		}

		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, projectile.Center);

			Vector2 offSet = Vector2.Normalize(Main.MouseWorld - projectile.Center);
			anchor.Projectile.ai[2] = offSet.ToRotation();
			anchor.LeftCLickCooldown = 30;
			anchor.Projectile.ai[0] = -20;
			anchor.Projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
			anchor.RightCLickCooldown = 600;
			anchor.NeedNetUpdate = true;
			projectile.rotation = 0f;

			for (int i = 0; i < 14; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.6f;
			}
		}

		public override bool ShapeshiftCanJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => projectile.ai[2] != 0 && projectile.ai[0] == 0 && player.controlJump;

		public override void ShapeshiftOnJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{ // same as right click, but is an improved dash, dealing damage
			SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, projectile.Center);

			Vector2 offSet = Vector2.Normalize(Main.MouseWorld - projectile.Center);
			anchor.Projectile.ai[2] = offSet.ToRotation();
			anchor.LeftCLickCooldown = 30;
			anchor.Projectile.ai[0] = -20;
			anchor.Projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
			anchor.RightCLickCooldown = 600;
			anchor.NeedNetUpdate = true;
			projectile.rotation = 0f;

			ImprovedDash = true;
			projectile.damage = shapeshifter.GetShapeshifterDamage(Item.damage * 1.5f);
			projectile.CritChance = shapeshifter.GetShapeshifterCrit(Item.crit);
			projectile.friendly = true;

			for (int i = 0; i < 14; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.6f;
			}
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// ai[0] holds the timer for a bite animation (>0) and dash (<0)
			// ai[1] holds the directions of a left click attack if grounded, whoami of hooked enemy if hooked
			// ai[2] holds the dash angle (ai[0] < 0) or hook timer (ai[0] >= 0)

			// MISC EFFECTS

			bool grounded = IsGrounded(projectile, player, 4f);
			float speedMult = GetSpeedMult(player, shapeshifter, anchor, grounded);

			if (grounded && anchor.RightCLickCooldown > 60 && anchor.RightCLickCooldown < 540)
			{ // Right click cd is set to 10 seconds when used, this makes it to touching the ground "resets" it
				anchor.RightCLickCooldown = 60;
			}

			if (projectile.ai[2] != 0f)
			{ // Increased attack speed while latched
				player.GetAttackSpeed(DamageClass.Melee) += 0.5f;
			}
			else
			{ // Redundant reset of fields when not dashing/latching
				ImprovedDash = false;
				projectile.friendly = false;
				projectile.rotation = 0f;
			}

			// ANIMATION

			if (anchor.Projectile.ai[0] < 0)
			{ // dashing
				anchor.Timespent = 0;
				anchor.Frame = 11;

				projectile.direction = (int)anchor.Projectile.ai[1];
				projectile.spriteDirection = projectile.direction;
			}
			else if (anchor.Projectile.ai[0] > 0)
			{ // Override animation during left click attack
				anchor.Projectile.ai[0]--;

				if (anchor.Projectile.ai[2] != 0)
				{ // Is hooked to a target
					projectile.direction = (Main.npc[(int)projectile.ai[1]].Center.X - projectile.Center.X) > 0 ? 1 : -1;
					anchor.Frame = (anchor.Projectile.ai[0] > 5 ? 10 : 11);
				}
				else
				{ // Is not hooked
					projectile.direction = (int)anchor.Projectile.ai[1];
					anchor.Frame = (anchor.Projectile.ai[0] > 5 ? 8 : 9);
				}

				if (anchor.Projectile.ai[0] < 0)
				{
					anchor.Projectile.ai[0] = 0;
				}

				if (anchor.Projectile.ai[0] == 0)
				{ // Puts the animation back on track
					anchor.Frame = 0;
				}

				projectile.spriteDirection = projectile.direction;
			}
			else if (anchor.Projectile.ai[2] != 0)
			{ // Is hooked to a target & not attacking
				anchor.Frame = 10;
				anchor.Timespent = 0;
				projectile.direction = (Main.npc[(int)projectile.ai[1]].Center.X - projectile.Center.X) > 0 ? 1 : -1;
				projectile.spriteDirection = projectile.direction;
			}
			else if (!grounded)
			{ // falling frame
				anchor.Timespent = 0;
				anchor.Frame = 7;
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

			// MOVEMENT

			if (anchor.Projectile.ai[2] == 0)
			{ // Normal movement, not dashing or hooked
				Vector2 intendedVelocity = projectile.velocity;
				GravityCalculations(ref intendedVelocity, player);

				if (anchor.IsInputJump)
				{ // Jump
					if (grounded)
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
						TryAccelerate(ref intendedVelocity, -4f, speedMult, 0.3f, acceleration);
						projectile.direction = -1;
						projectile.spriteDirection = -1;
						LateralMovement = true;
					}
					else if (anchor.IsInputRight && !anchor.IsInputLeft)
					{ // Right movement
						TryAccelerate(ref intendedVelocity, 4f, speedMult, 0.3f, acceleration);
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
			}
			else if (projectile.ai[2] != 0)
			{ // dashing or hooking
				// cancels fall damage
				player.fallStart = (int)(player.position.Y / 16f);
				player.fallStart2 = (int)(player.position.Y / 16f);
				anchor.RightCLickCooldown = 600;

				if (projectile.ai[0] < 0)
				{ // dashing
					Vector2 intendedVelocity = projectile.ai[2].ToRotationVector2() * 9f * speedMult;
					FinalVelocityCalculations(ref intendedVelocity, projectile, player, false, true, false, true);

					projectile.ai[0]++;
					if (projectile.ai[0] >= 0f)
					{ // resets values at the end of a dash
						projectile.ai[0] = 0f;
						projectile.ai[2] = 0f;
						shapeshifter.modPlayer.PlayerImmunity = 10;
						player.immuneTime = 10;
						player.immune = true;
						ImprovedDash = false;
					}

					Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.4f;
				}
				else
				{
					NPC npc = Main.npc[(int)projectile.ai[1]];
					if (npc.active && !npc.friendly && npc.life > 0)
					{ // player is hooked to a NPC; follow its movement
						projectile.gfxOffY = 0;
						player.gfxOffY = 0;
						projectile.position = npc.Center + OffsetToHookedTarget + new Vector2(0f, npc.gfxOffY);
						projectile.rotation = OffsetToHookedTarget.ToRotation() + MathHelper.PiOver2;

						// makes it so the player lets go of the NPC if it (player) would go through tiles
						Vector2 finalVelocity = Vector2.Zero;
						Vector2 intendedVelocity = npc.velocity / 10f;
						for (int i = 0; i < 10; i++)
						{
							finalVelocity += Collision.TileCollision(projectile.position + finalVelocity, intendedVelocity, projectile.width, projectile.height, true, false, (int)player.gravDir);
						}
						bool throughTiles = Math.Abs((finalVelocity - npc.velocity).Length()) > 0.1f;

						projectile.ai[2]--;
						if (projectile.ai[2] <= 0f || throughTiles)
						{ // resets value when letting of of hooked NPC
							projectile.ai[2] = 0f;
							projectile.ai[1] = 0f;
							projectile.velocity = Vector2.Zero;
							projectile.rotation = 0f;
							shapeshifter.modPlayer.PlayerImmunity = throughTiles ? 60 : 30; // gives more time to react if abruptly cancelled by going through tiles
							player.immuneTime = throughTiles ? 60 : 30;
							player.immune = true;
						}
					}
					else
					{ // resets value when letting of of hooked NPC
						projectile.ai[2] = 0f;
						projectile.ai[1] = 0f;
						projectile.velocity = Vector2.Zero;
						projectile.rotation = 0f;
						shapeshifter.modPlayer.PlayerImmunity = 30;
						player.immuneTime = 30;
						player.immune = true;
					}
				}
			}

			// POSITION AND ROTATION VISUALS

			anchor.OldPosition.Add(projectile.Center);
			anchor.OldRotation.Add(projectile.rotation);
			anchor.OldFrame.Add(anchor.Frame);

			for (int i = 0; i < 2; i++)
			{
				if (anchor.OldPosition.Count > (projectile.ai[0] < 0 ? 6 : 4))
				{
					anchor.OldPosition.RemoveAt(0);
					anchor.OldRotation.RemoveAt(0);
					anchor.OldFrame.RemoveAt(0);
				}
			}
		}

		public override bool ShapeshiftFreeDodge(Player.HurtInfo info, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			if (ImprovedDash) return true;

			info.DamageSource.TryGetCausingEntity(out var entity);
			if (entity != null && projectile.ai[2] != 0)
			{
				if (entity is NPC targetNPC)
				{
					if (projectile.ai[0] < 0)
					{ // dashing, hooks on targetNPC
						projectile.ai[0] = 0f;
						projectile.ai[1] = targetNPC.whoAmI;
						projectile.ai[2] = 180;
						projectile.velocity = Vector2.Zero;
						OffsetToHookedTarget = projectile.position - targetNPC.Center;
						anchor.LeftCLickCooldown = 10;
						anchor.RightCLickCooldown = 30;
						anchor.NeedNetUpdate = true;
						return true;
					}
					else if (targetNPC.whoAmI == projectile.ai[1])
					{ // currently hooked to that NPC, take no damage
						return true;
					}
				}
			}

			return false;
		}

		public override void ShapeshiftOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			ShapeshifterGlobalNPC globalNPC = target.GetGlobalNPC<ShapeshifterGlobalNPC>();
			if (globalNPC.PredatorFossilStack < 10) globalNPC.PredatorFossilStack ++ ;
			globalNPC.PredatorFossilTimer = 900; // 15 sec
		}

		public override void ShapeshiftModifyHurt(ref Player.HurtModifiers modifiers, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			if (modifiers.DamageSource.SourceOtherIndex == 0)
			{ // fall damage
				modifiers.FinalDamage *= 0.75f;
				modifiers.SetMaxDamage(50);
			}
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.FossilOre, 15);
			recipe.AddIngredient(ItemID.Amber, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}