using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter.Buffs.Debuffs;
using OrchidMod.Content.Shapeshifter.Projectiles.Sage;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Sage
{
	public class SageBatHell : OrchidModShapeshifterShapeshift
	{
		public int Jumps = 0;
		public float AttackCharge = 0;
		public float AttackCharge2 = 0;
		public bool LateralMovement = false;
		public bool ChargeCue = false; // Triggers a noise at full change
		public bool ChargeCue2 = false; // Triggers a noise when echolocations are going to expire

		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 1, 75, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.NPCDeath4;
			Item.useTime = 40;
			Item.shootSpeed = 8f;
			Item.knockBack = 3f;
			Item.damage = 84;
			ShapeshiftWidth = 18;
			ShapeshiftHeight = 22;
			ShapeshiftType = ShapeshifterShapeshiftType.Sage;
			MeleeSpeedRight = true;
			AutoReuseRight = true;
		}

		public override void ShapeshiftAnchorOnShapeshiftFast(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			Jumps = 1;
			anchor.ai[0] = 20;
			anchor.ai[1] = 1;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.Frame = 2;
			anchor.Timespent = 0;
			projectile.direction = player.direction;
			projectile.spriteDirection = player.direction;

			Jumps = 0;
			AttackCharge = 0;
			AttackCharge2 = 0;
			LateralMovement = false;
			ChargeCue = false;
			ChargeCue2 = false;

			for (int i = 0; i < 8; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Torch);
					dust.velocity *= 0.5f;
					dust.scale *= Main.rand.NextFloat(1f, 2f);
					dust.noGravity = true;
					dust.noLight = true;
			}
		}

		public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Torch);
				dust.velocity *= 0.5f;
				dust.scale *= Main.rand.NextFloat(1f, 2f);
				dust.noGravity = true;
				dust.noLight = true;
			}
		}
		
		public override bool ShapeshiftCanRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => base.ShapeshiftCanRightClick(projectile, anchor, player, shapeshifter) && AttackCharge <= 0 && projectile.ai[2] != -5;

		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			int projectileType = ModContent.ProjectileType<SageBatProj>();
			Vector2 velocity = Vector2.Normalize(Main.MouseWorld - projectile.Center) * Item.shootSpeed;
			ShapeshifterNewProjectile(shapeshifter, projectile.Center, velocity, projectileType, Item.damage * 0.25f, Item.crit, Item.knockBack, player.whoAmI, ai2:1f);

			anchor.RightCLickCooldown = Item.useTime * 3f;
			anchor.Projectile.ai[0] = 10;
			anchor.Projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
			anchor.NeedNetUpdate = true;

			SoundEngine.PlaySound(SoundID.NPCDeath4, projectile.Center);
		}

		public override bool ShapeshiftCanJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => anchor.JumpWithControlRelease(player) && Jumps > 0;

		public override void ShapeshiftOnJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			if (projectile.ai[2] > -5)
			{
				projectile.ai[2] = 4;
				anchor.NeedNetUpdate = true;
				Jumps--;
				SoundEngine.PlaySound(SoundID.Item32, projectile.Center);

				for (int i = 0; i < 3; i++)
				{
					Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
				}

				for (int i = 0; i < 3; i++)
				{
					Dust dust = Dust.NewDustDirect(projectile.Center, 0, 0, DustID.Torch);
					dust.velocity *= 0.5f;
					dust.scale *= Main.rand.NextFloat(1f, 2f);
					dust.noGravity = true;
					dust.noLight = true;
				}
			}
			else
			{
				projectile.ai[2] = -6;
				anchor.NeedNetUpdate = true;
			}
		}

		public override void ShapeshiftBuffs(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			if (projectile.ai[2] <= -5)
			{ // Attached to a ceiling
				AttackCharge = 0;
				AttackCharge2 = 0;
				player.aggro -= 500;
				player.lifeRegen += 10;
			}
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			float speedMult = GetSpeedMult(player, shapeshifter, anchor);
			player.fallStart = (int)(player.position.Y / 16f);
			player.fallStart2 = (int)(player.position.Y / 16f);
			player.noFallDmg = true;
			anchor.ai[0]++;

			if (projectile.ai[2] > -4) projectile.ai[2]--;

			GravityMult = 0.85f;
			if (anchor.IsInputDown) GravityMult += 0.15f;

			// ANIMATION

			if (anchor.Timespent % 5 == 0)
			{ // Animation frames
				anchor.Frame++;

				if (anchor.Frame == 4)
				{
					anchor.Frame = 0;
					anchor.Timespent = -3;
				}

				if (anchor.Frame >= 9)
				{
					anchor.Frame = 5;
					anchor.Timespent = -3;
				}
			}
 
			if (Main.rand.NextBool(20))
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Torch);
				dust.velocity *= 0.5f;
				dust.scale *= Main.rand.NextFloat(1f, 2f);
				dust.noGravity = true;
				dust.noLight = true;
			}

			// ATTACK

			if (anchor.IsLeftClick && projectile.ai[2] != -5)
			{
				if (anchor.ai[0] >= 30)
				{
					if (AttackCharge == 0)
					{
						SoundEngine.PlaySound(SoundID.Item65, projectile.Center);
						ChargeCue2 = false;
						ChargeCue = false;

						if (anchor.ai[0] == 30 && anchor.ai[0] == 1)
						{
							AttackCharge = 10;
						}
					}

					AttackCharge += shapeshifter.GetShapeshifterMeleeSpeed();

					if (AttackCharge < 7)
					{
						anchor.Frame = 4;
						anchor.Timespent = 0;
					}
					else if (anchor.Frame < 5)
					{
						anchor.Frame = 5;
						anchor.Timespent = 0;
					}

					if (AttackCharge >= 60 && !ChargeCue)
					{
						ChargeCue = true;
						anchor.Blink(true);
					}

					AttackCharge2 ++;

					if (AttackCharge2 >= 30)
					{
						AttackCharge2 = 0;
						bool fired = false;
						int projectileType = ModContent.ProjectileType<SageBatHellProj>();

						foreach (NPC npc in Main.npc)
						{
							if (OrchidModProjectile.IsValidTarget(npc))
							{
								if (npc.HasBuff<SageBatDebuff>() && npc.Center.Distance(projectile.Center) < 400f)
								{
									Vector2 velocity = Vector2.Normalize(projectile.Center - npc.Center).RotatedByRandom(1f) * Item.shootSpeed;
									ShapeshifterNewProjectile(shapeshifter, projectile.Center, velocity, projectileType, Item.damage * 0.9f, Item.crit, Item.knockBack, player.whoAmI, ai2: npc.whoAmI);
									fired = true;

									if (npc.buffTime[npc.FindBuffIndex(ModContent.BuffType<SageBatDebuff>())] < 120 && !ChargeCue2)
									{
										ChargeCue2 = true;
										anchor.Blink(true);
									}
								}
							}
						}

						if (fired)
						{
							SoundEngine.PlaySound(SoundID.Item20, projectile.Center);
						}
					}
				}
			}
			else
			{
				if (anchor.ai[0] == 1)
				{
					anchor.ai[0] = 30;
				}

				if (AttackCharge >= 60 && IsLocalPlayer(player))
				{
					int projectileType = ModContent.ProjectileType<SageBatProjAlt>();
					Vector2 velocity = Vector2.Normalize(Main.MouseWorld - projectile.Center) * Item.shootSpeed;
					ShapeshifterNewProjectile(shapeshifter, projectile.Center, velocity, projectileType, Item.damage, Item.crit, Item.knockBack, player.whoAmI, ai2: 1f);
					SoundEngine.PlaySound(SoundID.Item131, projectile.Center);
				}

				if (anchor.Frame >= 4)
				{ // failsafe
					anchor.Frame = 0;
					anchor.Timespent = -3;
				}

				AttackCharge = 0;
				AttackCharge2 = 0;
			}

			// MOVEMENT
			Vector2 intendedVelocity = projectile.velocity;

			if (projectile.ai[2] <= -5)
			{ // Attached to a ceiling
				AttackCharge = 0;
				AttackCharge2 = 0;
				player.aggro -= 500;
				player.lifeRegen += 60000;

				if (Main.rand.NextBool(45) && player.statLife < player.statLifeMax2)
				{
					Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.HealingPlus).velocity *= 0.25f;
				}

				if (projectile.ai[2] == -5 && !anchor.IsInputDown && !anchor.IsInputLeft && !anchor.IsInputRight)
				{
					anchor.Frame = 10;
					anchor.Timespent = 0;
					Jumps = 4;
					intendedVelocity = Vector2.Zero;
				}
				else
				{
					intendedVelocity = Vector2.UnitY;
					projectile.ai[2] = -4;
					anchor.NeedNetUpdate = true;
				}
			}
			else
			{
				GravityCalculations(ref intendedVelocity, player, shapeshifter);

				if (anchor.IsInputJump && intendedVelocity.Y >= 1.2f)
				{ // Gliding
					intendedVelocity.Y = 1.2f;
					if (anchor.Timespent % 4 == 0)
					{
						anchor.Timespent += 2;
					}
				}

				if (anchor.IsInputLeft || anchor.IsInputRight)
				{ // Player is inputting a movement key
					if (anchor.IsInputLeft && !anchor.IsInputRight)
					{ // Left movement
						TryAccelerate(ref intendedVelocity, -4f, speedMult, 0.25f);
						projectile.direction = -1;
						projectile.spriteDirection = -1;
						LateralMovement = true;
					}
					else if (anchor.IsInputRight && !anchor.IsInputLeft)
					{ // Right movement
						TryAccelerate(ref intendedVelocity, 4f, speedMult, 0.25f);
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


				if (projectile.ai[2] > 0)
				{ // Dashing
					TryJump(ref intendedVelocity, 8f, player, shapeshifter, anchor);
					projectile.direction = intendedVelocity.X > 0 ? 1 : -1;
					projectile.spriteDirection = projectile.direction;

					if (!CanGoUp(intendedVelocity, projectile, player) && AttackCharge <= 0 && !LateralMovement)
					{
						SoundEngine.PlaySound(SoundID.Dig, player.position);
						projectile.ai[2] = -5; // Attach the player to the ceiling
						anchor.NeedNetUpdate = true;
					}
 
					if (Main.rand.NextBool())
					{
						Dust dust = Dust.NewDustDirect(projectile.Center, 0, 0, DustID.Torch);
						dust.velocity *= 0.5f;
						dust.scale *= Main.rand.NextFloat(1f, 2f);
						dust.noGravity = true;
						dust.noLight = true;
					}
				}
				else if (projectile.ai[2] <= 0 && !CanGoUp(intendedVelocity * 3f, projectile, player) && AttackCharge <= 0 && !LateralMovement)
				{
					intendedVelocity.Y = -24f * speedMult;
					SoundEngine.PlaySound(SoundID.Dig, player.position);
					projectile.ai[2] = -5; // Attach the player to the ceiling
					anchor.NeedNetUpdate = true;
				}
				else if (projectile.ai[2] < -3)
				{ // Normal Y movement
					float intendedDistance = 24f;
					if (anchor.IsInputDown) intendedDistance -= 16f;
					if (IsGrounded(projectile, player, intendedDistance, anchor.IsInputDown, anchor.IsInputDown))
					{ // Pushes away from the ground
						Jumps = 4;
						intendedVelocity.Y -= player.gravity * 2f;
						if (intendedVelocity.Y < -2f)
						{
							intendedVelocity.Y = -2f;
						}
					}
					else if (IsGrounded(projectile, player, intendedDistance + 4f, anchor.IsInputDown, anchor.IsInputDown) && intendedVelocity.Y < 2f)
					{ // Locks up so the screen doesn't shake constantly
						Jumps = 4;
						intendedVelocity.Y *= 0f;
					}
					else if (!CanGoUp(intendedVelocity * 3f, projectile, player) && intendedVelocity.Y < 0f && AttackCharge <= 0 && !LateralMovement)
					{
						intendedVelocity.Y = -24f * speedMult;
						SoundEngine.PlaySound(SoundID.Dig, player.position);
						projectile.ai[2] = -5; // Attach the player to the ceiling
						anchor.NeedNetUpdate = true;
					}

					if (projectile.ai[0] > 0)
					{ // Override animation during attack
						projectile.ai[0]--;
						if (projectile.ai[2] < -45)
						{
							projectile.direction = (int)projectile.ai[1];
							projectile.spriteDirection = projectile.direction;
						}

						if (projectile.ai[0] > 5)
						{
							anchor.Frame = 5;
						}
						else
						{
							anchor.Frame = 4;
						}
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

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HellstoneBar, 10);
			recipe.AddIngredient<SageBat>();
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}