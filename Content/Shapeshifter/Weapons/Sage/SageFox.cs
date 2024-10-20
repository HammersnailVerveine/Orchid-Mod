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

namespace OrchidMod.Content.Shapeshifter.Weapons.Sage
{
	public class SageFox : OrchidModShapeshifterShapeshift
	{
		public bool LateralMovement = false;

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.value = Item.sellPrice(0, 2, 25, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.NPCDeath4;
			Item.useTime = 40;
			Item.shootSpeed = 48f;
			Item.knockBack = 5f;
			Item.damage = 34;
			ShapeshiftWidth = 30;
			ShapeshiftHeight = 24;
			ShapeshiftType = ShapeshifterShapeshiftType.Sage;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			shapeshifter.ShapeshifterSageFoxSpeed = 180;

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
			shapeshifter.ShapeshifterSageFoxSpeed = 180;

			for (int i = 0; i < 5; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// MISC EFFECTS
			// ANIMATION

			if (anchor.Projectile.ai[2] > 0)
			{ // Left click animation
				anchor.Projectile.ai[2]--;
				Color color = Color.Aqua * (float)Math.Sin(projectile.ai[2] * 0.1046f) * 0.2f;
				Lighting.AddLight(projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
			}

			if (LateralMovement)
			{ // Player is moving left or right, cycle through frames
				if (anchor.Timespent % 4 == 0 && anchor.Timespent > 0)
				{
					anchor.Frame++;
					if (anchor.Frame == 8)
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
			if (player.controlLeft || player.controlRight)
			{ // Player is inputting a movement key
				float speedMult = player.moveSpeed;

				if (player.controlLeft && !player.controlRight)
				{ // Left movement
					intendedVelocity.X -= 0.2f * speedMult;
					if (intendedVelocity.X < -5f * speedMult) intendedVelocity.X = -5f * speedMult;
					projectile.direction = -1;
					projectile.spriteDirection = -1;
					LateralMovement = true;
				}
				else if (player.controlRight && !player.controlLeft)
				{ // Right movement
					intendedVelocity.X += 0.2f * speedMult;
					if (intendedVelocity.X > 5f * speedMult) intendedVelocity.X = 5f * speedMult;
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

			// ATTACK

			if (IsLocalPlayer(player))
			{
				if (CanLeftClick(anchor))
				{ // Left click attack - regularly spawns homing flames
					int projectileType = ModContent.ProjectileType<SageFoxProj>();
					float ai1 = 0f;
					float ai0 = 0f;
					int count = 0;
					foreach (Projectile proj in Main.projectile)
					{
						if (proj.active && proj.owner == player.whoAmI && proj.type == projectileType)
						{
							count++;
							ai0 = proj.ai[0];
							if (proj.ai[1] == 0f) ai1 = 1f;
							if (proj.ai[1] == 1f && ai1 == 1f) ai1 = 2f;
						}
					}

					if (count < 3)
					{ // Spawn a flame if less than 3 currently exist
						int damage = shapeshifter.GetShapeshifterDamage(Item.damage);
						Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center, Vector2.Zero, projectileType, damage, 0f, player.whoAmI, ai0, ai1);
						newProjectile.CritChance = shapeshifter.GetShapeshifterCrit(Item.crit);

						projectile.ai[2] = 30;
						anchor.LeftCLickCooldown = Item.useTime;
						anchor.NeedNetUpdate = true;
						SoundEngine.PlaySound(SoundID.DD2_DarkMageAttack, projectile.Center);


						for (int i = 0; i < 5; i++)
						{
							Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.IceTorch, Scale: Main.rand.NextFloat(0.8f, 1.2f));
							dust.noGravity = true;
							dust.noLight = true;
						}
					}
				}

				if (CanRightClick(anchor))
				{ // Right click attack
					SoundEngine.PlaySound(SoundID.DD2_DarkMageCastHeal, projectile.Center);
				}

				if (player.controlJump)
				{
				}
			}

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
			if (projectile.ai[2] > 0)
			{
				spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
				spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

				float scalemult = (float)Math.Sin(projectile.ai[2] * 0.1046f) * 0.125f + 1f;
				spriteBatch.Draw(anchor.TextureShapeshift, drawPosition, drawRectangle, new Color(54, 150, 248) * 0.75f, projectile.rotation, drawRectangle.Size() * 0.5f, projectile.scale * scalemult, effect, 0f);

				spriteBatch.End();
				spriteBatch.Begin(spriteBatchSnapshot);
			}
		}
	}
}