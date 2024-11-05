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

namespace OrchidMod.Content.Shapeshifter.Weapons.Sage
{
	public class SageBat : OrchidModShapeshifterShapeshift
	{
		public int Jumps = 0;
		public int AttackCharge = 0;
		public int BlinkEffect = 0;
		public bool LateralMovement = false;
		public bool Ceiling = false;

		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 2, 25, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.NPCDeath4;
			Item.useTime = 40;
			Item.shootSpeed = 8f;
			Item.knockBack = 3f;
			Item.damage = 19;
			ShapeshiftWidth = 18;
			ShapeshiftHeight = 22;
			ShapeshiftType = ShapeshifterShapeshiftType.Sage;
			MeleeSpeedLeft = true;
			MeleeSpeedRight = true;
			AutoReuseRight = true;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.Frame = 2;
			anchor.Timespent = 0;
			BlinkEffect = 20;
			projectile.direction = player.direction;
			projectile.spriteDirection = player.direction;

			Jumps = 0;
			AttackCharge = 0;
			LateralMovement = false;
			Ceiling = false;

			for (int i = 0; i < 8; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}

			for (int i = 0; i < 4; i++)
			{
				Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Honey)].noGravity = true;
			}
		}

		public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			for (int i = 0; i < 5; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}

			for (int i = 0; i < 4; i++)
			{
				Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Honey)].noGravity = true;
			}
		}

		/*
		public override bool ShapeshiftCanLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => base.ShapeshiftCanLeftClick(projectile, anchor, player, shapeshifter);
		public override void ShapeshiftOnLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			int projectileType = ModContent.ProjectileType<SageBeeProj>();
			Vector2 offset = new Vector2(Main.rand.NextFloat(-4f, 4f), 14f + Main.rand.NextFloat(-4f, 4f));
			Vector2 velocity = Vector2.Normalize(Main.MouseWorld - (projectile.Center + offset)).RotatedByRandom(MathHelper.ToRadians(3f)) * Item.shootSpeed;
			int damage = shapeshifter.GetShapeshifterDamage(Item.damage * 2.5f);
			Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center + offset, velocity, projectileType, damage, Item.knockBack, player.whoAmI);
			newProjectile.CritChance = shapeshifter.GetShapeshifterCrit(Item.crit);
			newProjectile.netUpdate = true;

			anchor.LeftCLickCooldown = Item.useTime;
			anchor.RightCLickCooldown = Item.useTime;
			anchor.Projectile.ai[0] = 10;
			anchor.Projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
			anchor.NeedNetUpdate = true;

			SoundEngine.PlaySound(SoundID.Item17, projectile.Center);
		}
		*/
		public override bool ShapeshiftCanRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => base.ShapeshiftCanRightClick(projectile, anchor, player, shapeshifter) && AttackCharge <= 0;

		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			int damage = shapeshifter.GetShapeshifterDamage(Item.damage);
			int projectileType = ModContent.ProjectileType<SageBatProj>();
			Vector2 velocity = Vector2.Normalize(Main.MouseWorld - projectile.Center) * Item.shootSpeed;
			Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center, velocity, projectileType, damage, Item.knockBack, player.whoAmI);
			newProjectile.CritChance = shapeshifter.GetShapeshifterCrit(Item.crit);
			newProjectile.netUpdate = true;

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
				SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, projectile.Center);

				for (int i = 0; i < 5; i++)
				{
					Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
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
				player.aggro -= 500;
				player.lifeRegen += 10;
			}
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			float speedMult = GetSpeedMult(player, shapeshifter);
			player.fallStart = (int)(player.position.Y / 16f);
			player.fallStart2 = (int)(player.position.Y / 16f);
			player.noFallDmg = true;

			if (projectile.ai[2] > -4) projectile.ai[2]--;
			BlinkEffect++;


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

			// ATTACK

			if (anchor.IsLeftClick)
			{
				AttackCharge++;

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

				if (AttackCharge == 60) 
				{
					SoundEngine.PlaySound(SoundID.MaxMana, projectile.Center);
					BlinkEffect = 0;
				}
			}
			else
			{
				if (AttackCharge >= 60 && IsLocalPlayer(player))
				{
					// Attack
				}

				if (anchor.Frame >= 4)
				{ // failsafe
					anchor.Frame = 0;
					anchor.Timespent = -3;
				}

				AttackCharge = 0;
			}

			// MOVEMENT
			Vector2 intendedVelocity = projectile.velocity;

			if (projectile.ai[2] <= -5)
			{ // Attached to a ceiling
				AttackCharge = 0;
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
					Jumps = 3;
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
				GravityCalculations(ref intendedVelocity, player);

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


				if (projectile.ai[2] > 0)
				{ // Dashing
					intendedVelocity.Y = -8f * speedMult;
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
						Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].noGravity = true;
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
					float intendedDistance = 22f;
					if (anchor.IsInputDown) intendedDistance -= 16f;
					if (IsGrounded(projectile, player, intendedDistance, anchor.IsInputDown, anchor.IsInputDown))
					{ // Pushes away from the ground
						Jumps = 3;
						intendedVelocity.Y -= player.gravity * 1.4f;
						if (intendedVelocity.Y < -2.5f)
						{
							intendedVelocity.Y = -2.5f;
						}
					}
					else if (IsGrounded(projectile, player, intendedDistance + 4f, anchor.IsInputDown, anchor.IsInputDown) && intendedVelocity.Y < 2f)
					{ // Locks up so the screen doesn't shake constantly
						Jumps = 3;
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

		public override void PreDrawShapeshift(SpriteBatch spriteBatch, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Vector2 drawPosition, Rectangle drawRectangle, SpriteEffects effect, Player player, Color lightColor)
		{
			if (BlinkEffect < 20)
			{ // bling animation at full charge
				spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
				spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

				float scalemult = (float)Math.Sin(BlinkEffect * 0.157f) * 0.2f + 1f;
				spriteBatch.Draw(anchor.TextureShapeshift, drawPosition, drawRectangle, Color.White.MultiplyRGBA(lightColor) * 1.5f, projectile.rotation, drawRectangle.Size() * 0.5f, projectile.scale * scalemult, effect, 0f);

				spriteBatch.End();
				spriteBatch.Begin(spriteBatchSnapshot);
			}
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient<ShapeshifterBlankEffigy>();
			recipe.AddIngredient(ItemID.BeeWax, 14);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}