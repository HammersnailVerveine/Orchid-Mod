using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter.Misc;
using OrchidMod.Content.Shapeshifter.Projectiles.Warden;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Warden
{
	public class WardenEater : OrchidModShapeshifterShapeshift
	{
		private static List<int> VegetalTileTypes;
		private bool BackwardsAnimation = false;
		private bool MaxRangeSoundCue = false;
		public bool ChargeCue = false;
		public float AttackCharge = 0f;
		public float LastSyncedRotation = 0f;
		public int KeepAttackAngle = 0;

		public override void SetStaticDefaults()
		{
			VegetalTileTypes = GetVegetalTilesTypes();
		}

		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 74, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Grass;
			Item.useTime = 30;
			Item.shootSpeed = 75f;
			Item.knockBack = 15f;
			Item.damage = 37;
			ShapeshiftWidth = 22;
			ShapeshiftHeight = 22;
			ShapeshiftType = ShapeshifterShapeshiftType.Warden;
			GroundedWildshape = false;
			MeleeSpeedLeft = false;
		}

		public override bool ShapeshiftCanRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{ // can only create more fruits if >= 3 unripe fruits exist from that player
			int count = 0;
			foreach (Projectile proj in Main.projectile)
			{
				if (proj.type == ModContent.ProjectileType<WardenEaterProjAlt>() && projectile.owner == proj.owner && proj.active && proj.frame == 0)
				{
					count++;
				}
			}

			if (count >= 3) return false;
			return base.ShapeshiftCanRightClick(projectile, anchor, player, shapeshifter);
		}

		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{ // creates a fruit
			int projectileType = ModContent.ProjectileType<WardenEaterProjAlt>();
			Vector2 velocity = Vector2.Normalize(Main.MouseWorld - projectile.Center).RotatedByRandom(MathHelper.ToRadians(7.5f)) * 2.5f;
			ShapeshifterNewProjectile(shapeshifter, projectile.Center, velocity, projectileType, Item.damage, Item.crit, Item.knockBack * 0.33f, player.whoAmI);

			anchor.ai[1] = 20f;
			projectile.ai[2] = velocity.ToRotation() - MathHelper.PiOver2;

			anchor.LeftCLickCooldown = Item.useTime;
		}

		public override bool ShapeshiftCanJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => anchor.IsInputJump && anchor.CanLeftClick && anchor.ai[4] <= 0f;

		public override void ShapeshiftOnJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{ // dash
			Vector2 offSet = Vector2.Normalize(Main.MouseWorld - projectile.Center);
			projectile.ai[2] = offSet.ToRotation() - MathHelper.PiOver2;
			projectile.velocity *= 0f;
			anchor.ai[0] = 30;
			anchor.ai[4] = Item.useTime * 2f;
			//anchor.LeftCLickCooldown = Item.useTime * 2f;
			anchor.NeedNetUpdate = true;

			projectile.knockBack = 0f;
			projectile.damage = shapeshifter.GetShapeshifterDamage(Item.damage);
			projectile.CritChance = shapeshifter.GetShapeshifterCrit(Item.crit);
			projectile.friendly = true;

			SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, projectile.Center);
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.Frame = 0;
			anchor.Timespent = 0;
			projectile.direction = 1;
			projectile.spriteDirection = 1;
			BackwardsAnimation = false;
			MaxRangeSoundCue = false;
			ChargeCue = false;
			AttackCharge = 0f;
			LastSyncedRotation = 0f;
			KeepAttackAngle = 0;

			if (IsLocalPlayer(player))
			{
				int projectileType = ModContent.ProjectileType<WardenEaterStem>();
				foreach (Projectile proj in Main.projectile)
				{ // Kills existing stems, redundant but fixes a case where there could be two
					if (proj.type == projectileType && proj.owner == player.whoAmI && proj.active)
					{
						proj.Kill();
						break;
					}
				}

				Point playerCenterTile = new Point((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f));
				Vector2 validTileCoordinates = Vector2.Zero;
				Vector2 validTileCoordinatesVegetal = Vector2.Zero;

				// Check all the tiles in a square radius around the player center for a suitable one
				for (int i = -10; i <= 10; i++)
				{
					for (int j = -10; j <= 10; j++)
					{
						Tile tile = Framing.GetTileSafely(playerCenterTile.X + i, playerCenterTile.Y + j);
						if (tile.HasTile && (WorldGen.SolidTile(tile) || TileID.Sets.Platforms[tile.TileType]))
						{
							Vector2 tileCoordinates = new Vector2((playerCenterTile.X + i) * 16f + 8f, (playerCenterTile.Y + j) * 16f - 8f);

							if (validTileCoordinatesVegetal == Vector2.Zero)
							{ // no "vegetal" tile detected yet
								if (projectile.Center.Distance(tileCoordinates) < projectile.Center.Distance(validTileCoordinates))
								{ // replaces the target tile with the closest one
									validTileCoordinates = tileCoordinates;

									if (TileID.Sets.Platforms[tile.TileType])
									{ // slight offset when hooked to a platform
										validTileCoordinates.Y -= 4f;
									}
								}
							}

							if (VegetalTileTypes.Contains(tile.TileType) && projectile.Center.Distance(tileCoordinates) < projectile.Center.Distance(validTileCoordinatesVegetal)) {
								validTileCoordinatesVegetal = tileCoordinates;
							}
						}
					}
				}

				if (validTileCoordinates != Vector2.Zero)
				{
					Vector2 latchPosition = validTileCoordinates;
					if (validTileCoordinatesVegetal != Vector2.Zero)
					{
						latchPosition = validTileCoordinatesVegetal;
						projectile.ai[1] = 1f; // is hooked to plant
					}

					latchPosition.Y += 16f; // idk why it's offset by 1 tile upwards

					float maxRange = 240f * GetSpeedMult(player, shapeshifter, anchor); // 15 tiles * movespeed
					ShapeshifterNewProjectile(shapeshifter, latchPosition, Vector2.Zero, projectileType, 0, 0, 0f, player.whoAmI, Main.rand.Next(1000), maxRange + 8f);
					projectile.ai[0] = maxRange;
					anchor.NeedNetUpdate = true;
				}
				else
				{
					SoundEngine.PlaySound(SoundID.Item16, projectile.Center);
					anchor.NeedKill = true;
				}
			}

			for (int i = 0; i < 8; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}

			for (int i = 0; i < 10; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.JungleGrass)].velocity.Y -= 1.25f;
			}

			for (int i = 0; i < 5; i++)
			{
				Gore.NewGoreDirect(player.GetSource_ItemUse(Item), projectile.Center + new Vector2(Main.rand.NextFloat(-8f, 8f), Main.rand.NextFloat(-8f, 8f)), Vector2.UnitY.RotatedByRandom(MathHelper.Pi), GoreID.TreeLeaf_Jungle);
			}
		}

		public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			for (int i = 0; i < 5; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}

			for (int i = 0; i < 10; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.JungleGrass)].velocity.Y -= 1.25f;
			}
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// ai[0] contains the max distance to stem
			// ai[1] is == 1f if hooked to a vegetal tile, 0f else
			// ai[2] holds the angle of head during the various attacks
			// anchor.ai[0] holds the duration of the "jump" dash
			// anchor.ai[1] holds the animation duration of the right click attack
			// anchor.ai[2] holds the duration of the "bite" dash
			// anchor.ai[3] holds number of "fruit" stacks - improving left click attacks (max 3)
			// anchor.ai[4] holds the "jump" dash cooldown

			anchor.ai[1]--;
			anchor.ai[4]--;
			KeepAttackAngle--;

			// MISC EFFECTS & ANIMATION

			if (AttackCharge > 0f)
			{ // attack charge
				if (AttackCharge > 20f)
				{
					anchor.Frame = 4;
				}
				else
				{
					anchor.Frame = 3;
				}
			}
			else if (anchor.ai[0] > 0f)
			{ // shoot animation
				anchor.Frame = 4;
			}
			else if (anchor.ai[1] > 0f)
			{ // "jump" dash animation
				anchor.Frame = 4;
			}
			else if (anchor.ai[2] > 0f)
			{ // "bite" dash animation
				anchor.Frame = 0;
				BackwardsAnimation = false;
			}
			else if (anchor.Timespent % 5 == 0)
			{ // Animation frames
				if (anchor.Frame > 2)
				{ // resets the animation after a dash or attack
					anchor.Frame = 2;
					BackwardsAnimation = true;
				}

				if (BackwardsAnimation)
				{ // normal animation loop
					anchor.Frame--;

					if (anchor.Frame <= 0)
					{
						BackwardsAnimation = false;
					}
				}
				else
				{
					anchor.Frame++;

					if (anchor.Frame >= 2)
					{
						BackwardsAnimation = true;
					}
				}
			}

			if (projectile.ai[1] == 1f)
			{ // hooked to a vegetal tile
				shapeshifter.ShapeshifterMoveSpeedBonusFinal += 0.2f;
			}

			float speedMult = GetSpeedMult(player, shapeshifter, anchor);
			Projectile stem = null;

			foreach (Projectile proj in Main.projectile)
			{
				if (proj.active && proj.type == ModContent.ProjectileType<WardenEaterStem>() && proj.owner == player.whoAmI)
				{
					stem = proj;

					float maxRange = 240f * GetSpeedMult(player, shapeshifter, anchor, ignoreBonuses:true); // 15 tiles * movespeed

					if (projectile.ai[1] == 1f)
					{ // set here, the wildshape ignore bonuses for stem range calculation to avoid weird behaviour when touching liquids
						maxRange *= 1.2f;
					}

					if (projectile.ai[0] != maxRange)
					{ // Dynamically adapts the stem range to the player movespeed
						projectile.ai[0] = maxRange;
						stem.ai[1] = maxRange + 8f;
						anchor.NeedNetUpdate = true;
						stem.netUpdate = true;
					}

					break;
				}
			}

			if (stem == null)
			{ // unshift if there is a problem with the stem
				anchor.NeedKill= true;
				return;
			}
			else
			{ // else handle the projectile rotation
				if (anchor.ai[0] > 0f || anchor.ai[1] > 0f || anchor.ai[2] > 0f || AttackCharge > 0f || KeepAttackAngle > 0f)
				{ // dash
					projectile.rotation = projectile.ai[2] + MathHelper.Pi;
				}
				else
				{ // normal rotation, relative to stem
					projectile.rotation = (stem.Center - projectile.Center).ToRotation() - MathHelper.PiOver2;
				}
			}

			// ATTACK 

			if ((anchor.IsLeftClick || (anchor.ai[0] > 0f && AttackCharge > 0)) && anchor.LeftCLickCooldown <= 0f && anchor.ai[1] <= 0f && anchor.ai[2] <= 0f)
			{
				if (AttackCharge == 0)
				{
					SoundEngine.PlaySound(SoundID.Item65, projectile.Center);
					ChargeCue = false;
				}

				AttackCharge += shapeshifter.GetShapeshifterMeleeSpeed();

				if (anchor.ai[0] <= 0f && IsLocalPlayer(player))
				{ // not dashing = rotate the camera towards the cursor
					projectile.ai[2] = (Main.MouseWorld - projectile.Center).ToRotation() - MathHelper.PiOver2;

					if (Math.Abs(projectile.ai[2] - LastSyncedRotation) > 0.05f)
					{
						LastSyncedRotation = projectile.ai[2];
						anchor.NeedNetUpdate = true;
					}
				}

				if (AttackCharge >= 60 && !ChargeCue)
				{
					ChargeCue = true;
					anchor.Blink(true);
				}
			}
			else
			{
				if (AttackCharge >= 60 && anchor.LeftCLickCooldown <= 0f)
				{
					KeepAttackAngle = 20;
					if (anchor.ai[3] > 0)
					{ // reinforced = different sound
						SoundEngine.PlaySound(SoundID.Item108, projectile.Center);
					}
					else
					{
						SoundEngine.PlaySound(SoundID.DD2_JavelinThrowersAttack, projectile.Center);
					}

					if (IsLocalPlayer(player))
					{ 
						// spawn projectile
						Vector2 position = projectile.Center;
						Vector2 offSet = Vector2.Normalize(Main.MouseWorld - projectile.Center).RotatedByRandom(MathHelper.ToRadians(5f)) * Item.shootSpeed;

						float damage = Item.damage * 2f;
						float knockBack = Item.knockBack * 0.33f;
						if (anchor.ai[3] > 0)
						{ // reinforced if a fruit was consumed for more range & damage
							anchor.ai[3]--;
							damage *= 3;
							offSet *= 1.5f;
							knockBack *= 1.5f;
						}

						// projectile snaps to nearby NPCs
						bool foundTarget = false;
						offSet /= 15f;
						for (int i = 0; i < 15; i++)
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

						int projectileType = ModContent.ProjectileType<WardenEaterProj>();
						ShapeshifterNewProjectile(shapeshifter, position, offSet * 0.001f, projectileType, damage, Item.crit, knockBack, player.whoAmI);

						// mini dash

						Vector2 offSetDash = Vector2.Normalize(Main.MouseWorld - projectile.Center);
						projectile.ai[2] = offSetDash.ToRotation() - MathHelper.PiOver2;
						projectile.velocity *= 0f;
						anchor.ai[2] = 15;

						anchor.LeftCLickCooldown = Item.useTime * 0.25f;
						anchor.NeedNetUpdate = true;
					}
				}

				AttackCharge = 0;
			}

			// MOVEMENT

			Vector2 intendedVelocity = projectile.velocity;

			if (anchor.ai[0] > 0f)
			{ // jump dash
				float dashmult = 1f;

				if (anchor.ai[0] < 10f)
				{
					dashmult = anchor.ai[0] / 10f;
				}

				intendedVelocity = Vector2.UnitY.RotatedBy(projectile.ai[2]) * speedMult * 8f * dashmult * shapeshifter.ShapeshifterMoveSpeedMiscOverride;
				anchor.ai[0]--;
				anchor.ai[2]--; // redundant, just in case both dashes happen at once (?)

				int projType = ModContent.ProjectileType<WardenEaterProjAlt>();
				foreach(Projectile proj in Main.projectile)
				{
					if (projectile.Hitbox.Intersects(proj.Hitbox) && proj.type == projType && proj.frame == 1 && proj.ai[0] == 0f)
					{
						anchor.ai[3]++;
						if (anchor.ai[3] > 3f)
						{
							anchor.ai[3] = 3f;
						}

						if (IsLocalPlayer(player))
						{
							shapeshifter.modPlayer.TryHeal(shapeshifter.GetShapeshifterHealing(20));
						}

						proj.ai[0] = 1f;
						if (Main.LocalPlayer.whoAmI == proj.owner)
						{ // proj sync has to be done by its owner
							proj.netUpdate = true;
						}
						break;
					}
				}

				foreach (NPC npc in Main.npc)
				{
					if (npc.knockBackResist > 0f && OrchidModProjectile.IsValidTarget(npc) && npc.Hitbox.Intersects(projectile.Hitbox))
					{
						npc.velocity = Vector2.Normalize(projectile.velocity) * 20f * npc.knockBackResist;
						npc.velocity.Y -= 3f;
						npc.netUpdate = true;
					}
				}

				if (anchor.ai[0] <= 0f)
				{ // dash end
					anchor.ai[0] = 0f;
					projectile.friendly = false;
				}
			}
			else if (anchor.ai[2] > 0f)
			{ // bite dash
				float dashmult = 1f;

				if (anchor.ai[2] < 6f)
				{
					dashmult = anchor.ai[2] / 6f;
				}

				intendedVelocity = Vector2.UnitY.RotatedBy(projectile.ai[2]) * speedMult * 1.5f * dashmult;
				anchor.ai[2]--;

				if (anchor.ai[2] <= 0f)
				{ // dash end
					anchor.ai[2] = 0f;
				}
			}
			else
			{
				// 8 direction movement
				float velocityX = 0f;
				float velocityY = 0f;

				if (anchor.IsInputUp && !anchor.IsInputDown)
				{ // Top movement
					velocityY = -4f;
				}
				else if (anchor.IsInputDown && !anchor.IsInputUp)
				{ // Bottom movement
					velocityY = 4f;
				}
				else
				{ // Both keys pressed or no key pressed = no Y movement
					intendedVelocity.Y *= 0.85f;
				}

				if (anchor.IsInputLeft && !anchor.IsInputRight)
				{ // Left movement
					velocityX = -4f;
				}
				else if (anchor.IsInputRight && !anchor.IsInputLeft)
				{ // Right movement
					velocityX = 4f;
				}
				else
				{ // Both keys pressed or no key pressed = no X movement
					intendedVelocity.X *= 0.85f;
				}

				if (velocityX != 0f && velocityY != 0f)
				{ // diagonal movement, multiply both velocities so the speed isn't faster diagonally
					velocityX *= 0.70725f; // approx
					velocityY *= 0.70725f;
				}

				if (velocityX != 0f)
				{
					TryAccelerate(ref intendedVelocity, velocityX, speedMult, 0.35f);
				}

				if (velocityY != 0f)
				{
					TryAccelerate(ref intendedVelocity, velocityY, speedMult, 0.35f, Yaxis: true);
				}
			}

			if (stem != null)
			{
				float distance = stem.Center.Distance(projectile.Center);
				if (distance > projectile.ai[0])
				{ // Keeps the player inside the max range
					Vector2 pullBack = Vector2.Normalize(stem.Center - projectile.Center) * (stem.Center.Distance(projectile.Center) - projectile.ai[0]);
					intendedVelocity += pullBack * 0.1f;

					if (distance - projectile.ai[0] < 4f)
					{
						if (!MaxRangeSoundCue)
						{
							SoundEngine.PlaySound(SoundID.Grass, projectile.Center);
							MaxRangeSoundCue = true;
						}
					}
					else
					{
						MaxRangeSoundCue = false;
					}
				}
				else
				{
					if (intendedVelocity.Length() > 5f * speedMult)
					{ // helps descelerate the player after being yoinked back
						intendedVelocity = intendedVelocity * 0.8f;
						if (intendedVelocity.Length() < 5f * speedMult)
						{
							intendedVelocity = Vector2.Normalize(intendedVelocity) * 5f * speedMult;
						}
					}

					MaxRangeSoundCue = false;
				}
			}

			FinalVelocityCalculations(ref intendedVelocity, projectile, player, forceFallThrough:true);

			// POSITION AND ROTATION VISUALS

			anchor.OldPosition.Add(projectile.Center);
			anchor.OldRotation.Add(projectile.rotation);
			anchor.OldFrame.Add(anchor.Frame);

			for (int i = 0; i < 2; i++)
			{
				if (anchor.OldPosition.Count > (anchor.ai[0] > 0f ? 7 : 4))
				{
					anchor.OldPosition.RemoveAt(0);
					anchor.OldRotation.RemoveAt(0);
					anchor.OldFrame.RemoveAt(0);
				}
			}
		}

		/* Doesn't work in mp without manual sync, moved to the dash code
		public override void ShapeshiftOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{ 
			if (target.knockBackResist > 0f)
			{
				target.velocity = Vector2.Normalize(projectile.velocity) * 20f * target.knockBackResist;
				target.velocity.Y -= 3f;
				target.netUpdate = true;
			}
		}
		*/

		public override bool ShapeshiftFreeDodge(Player.HurtInfo info, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			if (anchor.ai[0] > 0) return true;
			return base.ShapeshiftFreeDodge(info, projectile, anchor, player, shapeshifter);
		}

		public override void ShapeshiftTeleport(Vector2 position, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter, bool updateFallStart = true)
		{
			if (position.Distance(projectile.Center) > projectile.ai[0])
			{ // player is teleporting too far from the stem, kill the anchor
				int projectileType = ModContent.ProjectileType<WardenEaterStem>();
				foreach (Projectile proj in Main.projectile)
				{ // Kills existing stems
					if (proj.type == projectileType && proj.owner == player.whoAmI && proj.active)
					{
						proj.Kill();
						break;
					}
				}

				anchor.NeedKill = true;
			}

			base.ShapeshiftTeleport(position, projectile, anchor, player, shapeshifter);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient<ShapeshifterBlankEffigy>();
			recipe.AddIngredient(ItemID.Vine, 5);
			recipe.AddIngredient(ItemID.JungleSpores, 15);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}

		public List<int> GetVegetalTilesTypes()
		{
			List<int> VegetalTypes = new List<int>()
			{
				TileID.Grass,
				TileID.AshGrass,
				TileID.CorruptGrass,
				TileID.CorruptJungleGrass,
				TileID.CrimsonGrass,
				TileID.CrimsonJungleGrass,
				TileID.HallowedGrass,
				TileID.JungleGrass,
				TileID.MushroomGrass,
				TileID.GolfGrass,
				TileID.GolfGrassHallowed,
				TileID.ArgonMoss,
				TileID.ArgonMossBrick,
				TileID.BlueMoss,
				TileID.BlueMossBrick,
				TileID.BrownMoss,
				TileID.BrownMossBrick,
				TileID.GreenMoss,
				TileID.GreenMossBrick,
				TileID.KryptonMoss,
				TileID.KryptonMossBrick,
				TileID.LavaMoss,
				TileID.LavaMossBrick,
				TileID.PurpleMoss,
				TileID.PurpleMossBrick,
				TileID.RainbowMoss,
				TileID.RainbowMossBrick,
				TileID.RedMoss,
				TileID.RedMossBrick,
				TileID.VioletMoss,
				TileID.VioletMossBrick,
				TileID.XenonMoss,
				TileID.XenonMossBrick,
				TileID.LeafBlock,
				TileID.LivingMahoganyLeaves,
				TileID.LivingWood,
				TileID.LivingMahogany
			};

			if (OrchidMod.ThoriumMod != null)
			{
				VegetalTypes.Add(OrchidMod.ThoriumMod.Find<ModTile>("MossyMarineBlock").Type);
				VegetalTypes.Add(OrchidMod.ThoriumMod.Find<ModTile>("LeakyMossyMarineBlock").Type);
				VegetalTypes.Add(OrchidMod.ThoriumMod.Find<ModTile>("SynthGold").Type);
				VegetalTypes.Add(OrchidMod.ThoriumMod.Find<ModTile>("SynthPlatinum").Type);
				VegetalTypes.Add(OrchidMod.ThoriumMod.Find<ModTile>("CherryAstroturf").Type);
				VegetalTypes.Add(OrchidMod.ThoriumMod.Find<ModTile>("GrimAstroturf").Type);
				VegetalTypes.Add(OrchidMod.ThoriumMod.Find<ModTile>("MarshyAstroturf").Type);
				VegetalTypes.Add(OrchidMod.ThoriumMod.Find<ModTile>("SpookyAstroturf").Type);
				VegetalTypes.Add(OrchidMod.ThoriumMod.Find<ModTile>("SnowyAstroturf").Type);
			}

			return VegetalTypes;
		}
	}
}