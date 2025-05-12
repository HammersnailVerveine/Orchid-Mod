using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter.Buffs;
using OrchidMod.Content.Shapeshifter.Projectiles.Sage;
using OrchidMod.Content.Shapeshifter.Projectiles.Symbiote;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Symbiote
{
	public class SymbioteToad : OrchidModShapeshifterShapeshift
	{
		public int jumpCooldown = 0; // Prevents jump spam when standing in closed spaces (10 frames delay minimum between jumps)
		public int jumpDelay = 0; // Delay between a movement input and its jump
		public int TongueOutBuffer = 0; // used for sprite direction (== 4 if tongue is out)

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 35, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Zombie13;
			Item.useTime = 20;
			Item.shootSpeed = 10f;
			Item.knockBack = 6f;
			Item.damage = 12;
			ShapeshiftWidth = 26;
			ShapeshiftHeight = 20;
			ShapeshiftType = ShapeshifterShapeshiftType.Symbiote;
			GroundedWildshape = true;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			TongueOutBuffer = 0;
			jumpCooldown = 0;
			jumpDelay = 0;
			anchor.Frame = 0;
			anchor.Timespent = 0;
			projectile.direction = player.direction;
			projectile.spriteDirection = player.direction;

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

		public override bool ShapeshiftCanLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => base.ShapeshiftCanLeftClick(projectile, anchor, player, shapeshifter) && TongueOutBuffer < 4;

		public override void ShapeshiftOnLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			projectile.ai[2] = 6f;
			anchor.LeftCLickCooldown = Item.useTime;
			anchor.RightCLickCooldown = Item.useTime;
			anchor.NeedNetUpdate = true;
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// ai[0] holds the max lateral speed of the wildshape
			// ai[1] holds the dirction of the wildshape for animation
			// ai[2] holds the timer before next attack
			// anchor.ai[0] holds the next jump direction
			// anchor.ai[1] is 1 if the player last jumped without lateral input

			bool grounded = IsGrounded(projectile, player, 4f);
			float speedMult = GetSpeedMult(player, shapeshifter, anchor, grounded);
			jumpCooldown--;

			if (jumpDelay > 0)
			{
				jumpDelay++;
				if (anchor.IsInputJump)
				{
					jumpDelay++;
				}
			}

			if (projectile.ai[0] >= 2.6f)
			{ // lateral speed goes back to 2.5f after being modified
				projectile.ai[0] -= 0.1f;
			}
			else if (projectile.ai[0] <= 2.4f)
			{
				projectile.ai[0] += 0.1f;
			}
			else
			{
				projectile.ai[0] = 2.5f;
			}

			// MISC EFFECTS & ANIMATION

			Projectile tongueProjectile = null;
			foreach (Projectile proj in Main.projectile)
			{
				if (proj.type == ModContent.ProjectileType<SymbioteToadProj>() && proj.active && proj.owner == player.whoAmI)
				{
					tongueProjectile = proj;
					TongueOutBuffer = 4;
					break;
				}
			}

			if (projectile.ai[2] > 0f)
			{ // preparing attack
				if (grounded)
				{
					anchor.Frame = 5;
				}
				else
				{
					anchor.Frame = 7;
				}
			}
			else if (tongueProjectile != null)
			{ // attacking
				if (grounded)
				{
					anchor.Frame = 6;
				}
				else
				{
					anchor.Frame = 8;
				}
			}
			else if (jumpDelay > 0)
			{ // just started jumping
				anchor.Frame = 1;
			}
			else if (projectile.velocity.Y < 1f && projectile.velocity.Y > -1f && !grounded)
			{ // apex
				anchor.Frame = 3;
			}
			else if (projectile.velocity.Y < 0f)
			{ // ascending
				anchor.Frame = 2;
			}
			else if (grounded && projectile.velocity.Y >= 0f)
			{ // grounded
				anchor.Frame = 0;
			}
			else
			{ // falling // default
				anchor.Frame = 4;
			}

			// MOVEMENT

			Vector2 intendedVelocity = projectile.velocity;
			GravityCalculations(ref intendedVelocity, player);

			// Normal movement
			if (anchor.IsInputLeft || anchor.IsInputRight || anchor.IsInputJump || jumpDelay > 0)
			{ // Player is inputting a movement key
				if (!grounded || projectile.velocity.Y < 0f)
				{ // airborne movement
					if (anchor.IsInputLeft && !anchor.IsInputRight)
					{ // Left movement
						TryAccelerate(ref intendedVelocity, -projectile.ai[0], speedMult, 0.1f);
						projectile.direction = -1;
						projectile.spriteDirection = -1;
					}
					else if (anchor.IsInputRight && !anchor.IsInputLeft)
					{ // Right movement
						TryAccelerate(ref intendedVelocity, projectile.ai[0], speedMult, 0.1f);
						projectile.direction = 1;
						projectile.spriteDirection = 1;
					}
					else
					{ // Both keys pressed = no movement
						intendedVelocity.X *= 0.7f;

						if (anchor.ai[0] != 0f && intendedVelocity.Y < 0f)
						{ // jump was done while pressing a lateral key, cancel vertical movement if the player released movement keys
							intendedVelocity.Y *= 0.7f;
						}
					}
				}
				else
				{
					intendedVelocity.X *= 0.7f;

					if (jumpDelay <= 0 && jumpCooldown <= 0)
					{
						if (anchor.IsInputLeft && !anchor.IsInputRight)
						{ // Left movement
							projectile.direction = -1;
							projectile.spriteDirection = -1;
							anchor.ai[0] = -1f;
						}
						else if (anchor.IsInputRight && !anchor.IsInputLeft)
						{ // Right movement
							projectile.direction = 1;
							projectile.spriteDirection = 1;
							anchor.ai[0] = 1f;
						}
						else
						{
							anchor.ai[0] = 0f;
						}

						jumpDelay++;
					}

					if (jumpDelay >= 10)
					{
						jumpDelay = 0;
						jumpCooldown = 10;

						if (!anchor.IsInputDown)
						{
							projectile.ai[0] = 6f;
							intendedVelocity.Y = -5f - (10f * speedMult * 0.5f);

							for (int i = 0; i < 8; i++)
							{
								Dust dust = Dust.NewDustDirect(projectile.position + new Vector2(0f, projectile.height), projectile.width, 0, DustID.Smoke);
								dust.scale *= Main.rand.NextFloat(0.6f, 0.8f);
								dust.velocity *= 0.25f;
								dust.velocity.Y -= 0.75f;
							}

							SoundStyle soundStyle = SoundID.Item32;
							soundStyle.Pitch += 1f;
							soundStyle.Volume *= 2.5f;
							SoundEngine.PlaySound(soundStyle, projectile.Center);

							if (!anchor.IsInputJump)
							{
								intendedVelocity.Y *= 0.5f;
							}
							else if (anchor.IsInputUp)
							{
								intendedVelocity.Y *= 1.25f;
							}
						}
						else
						{
							projectile.ai[0] = 4.5f;
							intendedVelocity.Y = -2.5f - (5f * speedMult * 0.5f); ;

							for (int i = 0; i < 5; i++)
							{
								Dust dust = Dust.NewDustDirect(projectile.position + new Vector2(0f, projectile.height), projectile.width, 0, DustID.Smoke);
								dust.scale *= Main.rand.NextFloat(0.6f, 0.8f);
								dust.velocity *= 0.25f;
								dust.velocity.Y -= 0.5f;
							}

							SoundStyle soundStyle = SoundID.Item32;
							soundStyle.Pitch += 1f;
							soundStyle.Volume *= 1.5f;
							SoundEngine.PlaySound(soundStyle, projectile.Center);
						}

						intendedVelocity.X = projectile.ai[0] * speedMult * anchor.ai[0];
						anchor.NeedNetUpdate = true;
					}
				}
			}
			else
			{ // no movement input
				intendedVelocity.X *= 0.7f;

				if (anchor.ai[0] != 0f && intendedVelocity.Y < 0f)
				{ // jump was done while pressing a lateral key, cancel vertical movement if the player released movement keys
					intendedVelocity.Y *= 0.7f;
				}
			}

			FinalVelocityCalculations(ref intendedVelocity, projectile, player, false, preventDownInput:anchor.IsInputJump);

			// ATTACK START (needs intendedVelocity)

			if (projectile.ai[2] > 0)
			{
				projectile.ai[2] -= shapeshifter.GetShapeshifterMeleeSpeed();
				projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);

				if (projectile.ai[2] <= 0)
				{
					projectile.ai[2] = 0;
					SoundStyle soundStyle = SoundID.DD2_OgreSpit;
					soundStyle.Pitch += Main.rand.NextFloat(-0.3f, 0.3f);
					SoundEngine.PlaySound(soundStyle, projectile.Center);

					if (IsLocalPlayer(player))
					{
						Vector2 targetLocation = Main.MouseWorld;
						Vector2 offSet = targetLocation - projectile.Center;

						if (targetLocation.Distance(projectile.Center) > 480f)
						{ // 30 tiles range max
							targetLocation = projectile.Center + Vector2.Normalize(offSet) * 480f;
						}

						if (targetLocation.Distance(projectile.Center) < 80f)
						{ // 5 tiles range main
							targetLocation = projectile.Center + Vector2.Normalize(offSet) * 80f;
						}

						offSet = (targetLocation - projectile.Center) / 60f;
						Vector2 finalTargetLocation = projectile.Center;
						for (int i = 0; i < 60; i++)
						{
							finalTargetLocation += Collision.TileCollision(finalTargetLocation, offSet, 8, 8, true, true, (int)player.gravDir);
						}

						finalTargetLocation -= projectile.Center;

						int projectileType = ModContent.ProjectileType<SymbioteToadProj>();
						int damage = shapeshifter.GetShapeshifterDamage(Item.damage);
						Vector2 spawnPosition = projectile.Center + new Vector2(6f * projectile.ai[1], -2f);
						Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), spawnPosition, Vector2.UnitX * 10f * projectile.ai[1], projectileType, damage, Item.knockBack, player.whoAmI, finalTargetLocation.X, finalTargetLocation.Y);
						newProjectile.CritChance = shapeshifter.GetShapeshifterCrit(Item.crit);
						newProjectile.netUpdate = true;
					}
				}
			}

			if (TongueOutBuffer > 0 || projectile.ai[2] > 0)
			{
				projectile.direction = (int)projectile.ai[1];
				projectile.spriteDirection = projectile.direction;

				if (tongueProjectile == null)
				{
					TongueOutBuffer --;
				}
			}

			// POSITION AND ROTATION VISUALS

			anchor.OldPosition.Add(projectile.Center);
			anchor.OldRotation.Add(projectile.rotation);
			anchor.OldFrame.Add(anchor.Frame);

			for (int i = 0; i < 2; i++)
			{
				if (anchor.OldPosition.Count > 4)
				{
					anchor.OldPosition.RemoveAt(0);
					anchor.OldRotation.RemoveAt(0);
					anchor.OldFrame.RemoveAt(0);
				}
			}
		}

		public override void ShapeshiftModifyHurt(ref Player.HurtModifiers modifiers, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			if (modifiers.DamageSource.SourceOtherIndex == 0)
			{ // fall damage
				modifiers.FinalDamage *= 0.75f;
				modifiers.SetMaxDamage(50);
			}
		}

		public override Color GetColorGlow(ref bool drawPlayerAsAdditive, Color lightColor, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => player.GetImmuneAlphaPure(Color.White, 0f) * 0.5f;
	}
}