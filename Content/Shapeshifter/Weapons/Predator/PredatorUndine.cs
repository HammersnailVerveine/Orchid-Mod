using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter.Projectiles.Predator;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Predator
{
	public class PredatorUndine : OrchidModShapeshifterShapeshift
	{
		public bool LateralMovement = false;

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 34;
			Item.value = Item.sellPrice(0, 1, 62, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.NPCDeath4;
			Item.useTime = 15;
			Item.shootSpeed = 150f;
			Item.knockBack = 5f;
			Item.damage = 27;
			ShapeshiftWidth = 30;
			ShapeshiftHeight = 24;
			ShapeshiftType = ShapeshifterShapeshiftType.Predator;
			ShapeshiftTypeUI = ShapeshifterShapeshiftTypeUI.List;
			AutoReuseRight = true;
			GroundedWildshape = true;
		}

		public override void ShapeshiftGetUIInfo(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter, ref int uiCount, ref int uiCountMax)
		{
			uiCount = (int)Math.Floor(projectile.ai[0] * 0.5f);
			uiCountMax = 5;
		}

		public override void ShapeshiftAnchorOnShapeshiftFast(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			projectile.ai[0] = 4f;
		}

		public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			anchor.Frame = 1;
			anchor.Timespent = 0;
			projectile.direction = player.direction;
			projectile.spriteDirection = player.direction;
			LateralMovement = false;

			for (int i = 0; i < 5; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.WaterCandle, Scale: Main.rand.NextFloat(1.5f, 2f));
				dust.noGravity = true;
				dust.noLightEmittence = true;
			}

			for (int i = 0; i < 8; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}
		}

		public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.WaterCandle, Scale: Main.rand.NextFloat(1.5f, 2f));
				dust.noGravity = true;
				dust.noLightEmittence = true;
			}

			for (int i = 0; i < 8; i++)
			{
				Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
			}
		}

		public override bool ShapeshiftCanLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => anchor.IsLeftClick && anchor.CanLeftClick && (!anchor.IsRightClick || anchor.RightCLickCooldown > 0);

		public override void ShapeshiftOnLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			Vector2 position = projectile.Center;
			Vector2 offSet = Vector2.Normalize(Main.MouseWorld - projectile.Center).RotatedByRandom(MathHelper.ToRadians(5f)) * Item.shootSpeed * Main.rand.NextFloat(0.8f, 1.2f) / 25f;

			bool foundTarget = false;
			for (int i = 0; i < 25; i++)
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

			int projectileType = ModContent.ProjectileType<PredatorUndineProj>();
			ShapeshifterNewProjectile(shapeshifter, position, offSet * 0.001f, projectileType, Item.damage, Item.crit, Item.knockBack, player.whoAmI);
			anchor.LeftCLickCooldown = Item.useTime;
		}

		public override bool ShapeshiftCanRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => anchor.IsRightClick && anchor.CanRightClick;

		public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			int projectileType = ModContent.ProjectileType<PredatorUndineProjAlt>();

			float damage = 1f;
			int crit = shapeshifter.GetShapeshifterCrit(Item.crit);
			if (crit > 100)
			{ // damage increased by 200% of excess crit
				damage += (crit - 100) * 0.02f;
			}
			damage = damage * Item.damage;

			float closestDistance = 80f;
			NPC closestTarget = shapeshifter.modPlayer.LastHitNPC;

			foreach (NPC npc in Main.npc)
			{
				bool closestBoss = false;
				if (closestTarget != null)
				{
					if (closestTarget.boss)
					{
						closestBoss = true;
					}
				}

				if (OrchidModProjectile.IsValidTarget(npc))
				{
					float distance = Main.MouseWorld.Distance(npc.Center);
					Point point = new Point((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y);
					if ((distance < closestDistance || (closestTarget == null && npc.Hitbox.Contains(point)) || (distance < 80f && npc.boss)) && (!closestBoss || npc.boss))
					{
						closestTarget = npc;
						closestDistance = distance;
					}
				}
			}

			if (closestTarget != null)
			{
				while (projectile.ai[0] >= 1f)
				{
					projectile.ai[0]--;
					Vector2 velocity = Vector2.Normalize(projectile.Center - closestTarget.Center).RotatedByRandom(1f) * 3.8f; // 3.8f is a magic number that makes the trail look good ...
					ShapeshifterNewProjectile(shapeshifter, projectile.Center, velocity, projectileType, damage, Item.crit, Item.knockBack, player.whoAmI, ai0: projectile.ai[0] * 15f, ai2: closestTarget.whoAmI);
				}

				anchor.RightCLickCooldown = 60;
			}
		}

		public override void ShapeshiftBuffs(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			Item.crit = (int)(projectile.ai[0] * 10f);
			player.GetAttackSpeed(DamageClass.Melee) += 0.05f * projectile.ai[1];
			shapeshifter.ShapeshifterMoveSpeedBonus += 0.05f * projectile.ai[1];
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			OrchidShapeshifter shapeshifter = player.GetModPlayer<OrchidShapeshifter>();
		}

		public override void UpdateInventory(Player player)
		{
			OrchidShapeshifter shapeshifter = player.GetModPlayer<OrchidShapeshifter>();
			if (shapeshifter.IsShapeshifted)
			{
				if (shapeshifter.Shapeshift.Type != Type)
				{
					Item.crit = 0;
				}
			}
			else
			{
				Item.crit = 0;
			}
		}

		public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
		{
			// ai[0] holds passive stacks
			// ai[1] holds the number of recent crits
			// ai[2] holds the timer before crits reset

			// MISC EFFECTS

			if (projectile.ai[2] <= 0f)
			{
				projectile.ai[1] = 0f;
			}
			else
			{
				projectile.ai[2] --;
			}

			bool grounded = IsGrounded(projectile, player, 4f);
			float speedMult = GetSpeedMult(player, shapeshifter, anchor, grounded);

			// ANIMATION

			if (grounded)
			{
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
				{ // idle frame
					anchor.Timespent = 0;
					anchor.Frame = 0;
				}
			}
			else
			{ // Falling frame
				anchor.Timespent = 0;
				anchor.Frame = 5;
			}

			// MOVEMENT

			Vector2 intendedVelocity = projectile.velocity;
			GravityCalculations(ref intendedVelocity, player, shapeshifter);

			if (anchor.IsInputJump)
			{ // Jump while no charge ready
				TryJump(ref intendedVelocity, 9f, player, shapeshifter, anchor, true);
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

			// POSITION AND ROTATION VISUALS

			anchor.OldPosition.Add(projectile.Center);
			anchor.OldRotation.Add(projectile.rotation);
			anchor.OldFrame.Add(anchor.Frame);

			for (int i = 0; i < 2; i++)
			{
				if (anchor.OldPosition.Count > (projectile.ai[1] > 5 ? 8 : 5))
				{
					anchor.OldPosition.RemoveAt(0);
					anchor.OldRotation.RemoveAt(0);
					anchor.OldFrame.RemoveAt(0);
				}
			}
		}
	}
}