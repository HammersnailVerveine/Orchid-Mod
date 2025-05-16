using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter;
using OrchidMod.Content.Shapeshifter.Projectiles.Misc;
using OrchidMod.Content.Shapeshifter.Projectiles.Predator;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod
{
	public class OrchidShapeshifter : ModPlayer
	{
		public OrchidPlayer modPlayer;
		public ShapeshifterShapeshiftAnchor ShapeshiftAnchor;
		public OrchidModShapeshifterShapeshift Shapeshift;
		public bool IsShapeshifted => ShapeshiftAnchor != null && ShapeshiftAnchor.Projectile.active;
		public int GetShapeshifterDamage(float damage) => (int)(Player.GetDamage<ShapeshifterDamageClass>().ApplyTo(damage) + Player.GetDamage(DamageClass.Generic).ApplyTo(damage) - damage);
		public int GetShapeshifterCrit(int additionalCritChance = 0) => (int)(Player.GetCritChance<ShapeshifterDamageClass>() + Player.GetCritChance<GenericDamageClass>() + additionalCritChance);
		public float GetShapeshifterMeleeSpeed(float additionalMeleeSpeed = 0) => Player.GetTotalAttackSpeed(DamageClass.Melee) + ShapeshifterMeleeSpeedBonus + additionalMeleeSpeed;
		public int GetShapeshifterHealing(float healing) => (int)Math.Ceiling(healing * ShapeshifterHealingBonus);

		// Can be edited by gear (Set effects, accessories, misc)

		public int ShapeshifterPredatorBleedPotency = 0; // Damage per stack of the predator attack bleed
		public int ShapeshifterPredatorBleedMaxStacks = 0; // Maximum stacks of the predator attack bleed 
		public float ShapeshifterMeleeSpeedBonus = 0f;
		public float ShapeshifterMoveSpeedBonus = 0f; // Additive, Scales logarithmically, can be used as shapeshifter-only alternative to player.movespeed, should be preferred
		public float ShapeshifterMoveSpeedBonusFlat = 0f; // Additive, added to the final movespeed
		public float ShapeshifterMoveSpeedBonusFinal = 0f; // Multiplicative, used before adding ShapeshifterMoveSpeedBonusFlat
		public float ShapeshifterMoveSpeedBonusGrounded = 1f; // Multiplicative, used for effects that increase grounded speed like magiluminescence
		public float ShapeshifterMoveSpeedBonusNotGrounded = 1f; // Multiplicative, used for effects that increase the movespeed of "flying" wildshapes, at all times
		public float ShapeshifterMoveSpeedMiscOverride = 1f; // Multiplies other means of movement, like dashes. Should generally not be edited as dashes that should be affected by movespeed already are.
		public float ShapeshifterHealingBonus = 1f; // Multiplicative, affects the direct healing provided by shapeshifter effects
		public float ShapeshifterJumpSpeed = 1f; // Multiplicative, affects most jumps for various wildshapes
		public float ShapeshifterGravity = 1f; // Multiplicative, mostly used for movement in liquids
		public float ShapeshifterMaxFallSpeed = 1f; // Multiplicative, mostly used for movement in liquids

		public bool ShapeshifterSetHarpy = false; // Harpy armor set bonus (causes feathers to fall when attacking from above)
		public bool ShapeshifterSageDamageOnHit = false; // if true, hitting new targets increase feral damage

		// Dynamic gameplay and UI fields

		public int ShapeshifterSageFoxSpeed = 0;
		public int ShapeshifterSageDamageOnHitCount = 0;
		public int ShapeshifterSageDamageOnHitTimer = 0;
		public int[] ShapeshifterSageDamageOnHitTargets;
		public int ShapeshifterSetHarpyDamagePool = 0;
		public int ShapeshifterSetHarpyTimer = 0;

		public override void HideDrawLayers(PlayerDrawSet drawInfo)
		{
			if (ShapeshiftAnchor != null)
			{
				foreach (var layer in PlayerDrawLayerLoader.DrawOrder)
				{
					layer.Hide();
				}
			}
		}

		public override void Initialize()
		{
			modPlayer = Player.GetModPlayer<OrchidPlayer>();
			ShapeshifterSageDamageOnHitTargets = [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1];
		}

		public override void ResetEffects()
		{
			if (ShapeshiftAnchor != null && ShapeshiftAnchor.Projectile.active)
			{
				if (Player.mount.Active || Player.grappling[0] >= 0 || Player.timeShimmering > 0)
				{ // Disable the shapeshift if the player is mounted, shimmered or uses a hook
					ShapeshiftAnchor.Projectile.Kill();
				}

				if (ShapeshiftAnchor.NeedKill && Player.whoAmI == Main.myPlayer)
				{ // Kills the anchor properly if NeedKill was set to true, avoiding issues caused by killing it randomly
					ShapeshiftAnchor.NeedKill = false;
					ShapeshiftAnchor.Projectile.Kill();
				}
			}

			// Reset gameplay fields

			ShapeshifterPredatorBleedPotency = 0;
			ShapeshifterPredatorBleedMaxStacks = 0;
			ShapeshifterMeleeSpeedBonus = 0f;
			ShapeshifterMoveSpeedBonus = 0f;
			ShapeshifterMoveSpeedBonusFlat = 0f;
			ShapeshifterMoveSpeedBonusFinal = 1f;
			ShapeshifterMoveSpeedBonusGrounded = 1f;
			ShapeshifterMoveSpeedBonusNotGrounded = 1f;
			ShapeshifterMoveSpeedMiscOverride = 1f;
			ShapeshifterHealingBonus = 1f;
			ShapeshifterJumpSpeed = 1f;
			ShapeshifterGravity = 1f;
			ShapeshifterMaxFallSpeed = 1f;

			ShapeshifterSetHarpy = false;
			ShapeshifterSageDamageOnHit = false;
		}

		public override void PostUpdateEquips()
		{
			if (IsShapeshifted)
			{
				Shapeshift.ShapeshiftBuffs(ShapeshiftAnchor.Projectile, ShapeshiftAnchor, Player, this);

				// Cancels some equipment effects to prevent visual & audio issues

				Player.rocketBoots = 0;
				Player.vanityRocketBoots = 0;
				Player.accRunSpeed = 3f;
				Player.ExtraJumps.Clear();
				
				if (Player.wingTime > 0)
				{
					Player.wingTime = 0;
				}

				// Grants stats to make some equipment compatible with shapeshifter

				if (Player.hasMagiluminescence)
				{
					ShapeshifterMoveSpeedBonusGrounded += 0.15f;
				}

				if (Player.shadowArmor)
				{
					ShapeshifterMoveSpeedBonusGrounded += 0.15f;
				}
			}

			// Misc Effects that should be called before Shapeshifter Core mechanics (eg : stat changes that should affect the shapeshifted player)

			if (ShapeshifterSageFoxSpeed > 0)
			{
				ShapeshifterSageFoxSpeed--;
				Player.moveSpeed += ShapeshifterSageFoxSpeed * 0.003f;
			}

			if (ShapeshifterSageDamageOnHitTimer > 0)
			{
				ShapeshifterSageDamageOnHitTimer--;

				if (ShapeshifterSageDamageOnHitTimer <= 0)
				{
					ShapeshifterSageDamageOnHitCount = 0;
					ShapeshifterSageDamageOnHitTargets = [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1];
				}
			}
		}

		public override void PostUpdate()
		{
			// Shapeshifter core stuff

			if (IsShapeshifted)
			{
				// Environmental movement speed changes (honey, liquids, etc)

				if (Player.sticky)
				{ // honey
					ShapeshifterMoveSpeedBonusFinal *= 0.3f;
					ShapeshifterMoveSpeedBonusFlat *= 0.3f;
				}

				if ((Player.wet && !Player.ignoreWater) || Player.lavaWet)
				{ // in lava on water
					ShapeshifterMaxFallSpeed *= 0.2f;
					ShapeshifterGravity *= 0.5f;
					ShapeshifterJumpSpeed *= 0.5f;
					ShapeshifterMoveSpeedBonusFinal *= 0.5f;
					ShapeshifterMoveSpeedBonusFlat *= 0.5f;
					ShapeshifterMoveSpeedMiscOverride *= 0.5f;
				}
				
				if (Player.wet && Player.ignoreWater)
				{ // I don't know why either, but this is needed to replicate normal movement
					ShapeshifterGravity *= 2f;
				}

				if (Player.honeyWet)
				{ // in honey (not vanilla accurate but good enough)
					ShapeshifterMaxFallSpeed *= 0.2f;
					ShapeshifterGravity *= 5f;
					//ShapeshifterJumpSpeed *= 0.1f;
					ShapeshifterMoveSpeedBonusFinal *= 0.25f;
					ShapeshifterMoveSpeedBonusFlat *= 0.25f;
					ShapeshifterMoveSpeedMiscOverride *= 0.25f;
				}

				if (Player.shimmerWet)
				{ // in shimmer
					ShapeshifterMaxFallSpeed *= 0.2f;
					ShapeshifterGravity *= 0.375f;
					ShapeshifterJumpSpeed *= 0.375f;
					ShapeshifterMoveSpeedBonusFinal *= 0.375f;
					ShapeshifterMoveSpeedBonusFlat *= 0.375f;
					ShapeshifterMoveSpeedMiscOverride *= 0.375f;
				}

				if (Player.powerrun && Shapeshift.GroundedWildshape)
				{ // asphalt
					ShapeshifterMoveSpeedBonusGrounded *= 3f;
				}

				// Jump speed has to be edited here, because its fields are updated after PostUpdateEquips(). it also appears that vanilla does this after environment checks
				// This makes it so a wildshape gets slightly worse benefits as a normal player from the Shiny Red Balloon and Frog Leg accessories
				// However, because wildshapes jumps are also enhanced by their movement speed, this is fine, it simply avoids movement getting out of hand
				ShapeshifterJumpSpeed += (Player.jumpHeight - 15f) * 0.025f; // the base Player.jumpHeight value is 15
				ShapeshifterJumpSpeed += (Player.jumpSpeed - 5.01f) * 0.1f; // the base Player.jumpSpeed value is 5.01

				// Runs the shapeshift AI and adjust player position accordingly
				Player.width = Shapeshift.ShapeshiftWidth;
				Player.height = Shapeshift.ShapeshiftHeight;

				Projectile projectile = ShapeshiftAnchor.Projectile;

				if (Player.whoAmI == Main.myPlayer)
				{ // Shapeshift inputs
					ShapeshiftAnchor.CheckInputs(Player); // mostly used to sync inputs in mp

					if (Shapeshift.ShapeshiftCanLeftClick(projectile, ShapeshiftAnchor, Player, this))
					{
						Shapeshift.ShapeshiftOnLeftClick(projectile, ShapeshiftAnchor, Player, this);
					}

					if (Shapeshift.ShapeshiftCanRightClick(projectile, ShapeshiftAnchor, Player, this))
					{
						Shapeshift.ShapeshiftOnRightClick(projectile, ShapeshiftAnchor, Player, this);
					}

					if (Shapeshift.ShapeshiftCanJump(projectile, ShapeshiftAnchor, Player, this))
					{
						Shapeshift.ShapeshiftOnJump(projectile, ShapeshiftAnchor, Player, this);
					}

					if (Player.Center.Distance(projectile.Center) > 64f && projectile.velocity.Length() < 32f)
					{ // the player is far away from the projectile center, which is abnormal -> they likely teleported
						Shapeshift.ShapeshiftTeleport(Player.Center, projectile, ShapeshiftAnchor, Player, this);
					}
				}

				Shapeshift.ShapeshiftAnchorAI(projectile, ShapeshiftAnchor, Player, this);
				ShapeshiftAnchor.ExtraAI(Player, this);

				Player.velocity = projectile.velocity;
				Player.Center = projectile.Center + projectile.velocity;
			}
			else if (ShapeshiftAnchor != null || Shapeshift != null)
			{ // Failsafe in case the anchor isn't properly killed
				Player.width = Player.defaultWidth;
				Player.height = Player.defaultHeight;
				Shapeshift = null;
				ShapeshiftAnchor = null;
			}

			// Misc Effects that should be called after shapeshifter core mechanics (eg: that depend of the player width and height to be correct)

			if (ShapeshifterSageFoxSpeed > 0)
			{
				if (Main.rand.NextBool((int)(30 - ShapeshifterSageFoxSpeed / 6f) + 1))
				{
					Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.IceTorch, Scale: Main.rand.NextFloat(1f, 1.4f));
					dust.noGravity = true;
					dust.noLight = true;
				}
			}

			if (ShapeshifterSageDamageOnHitCount > 0)
			{ // increases damage for each individual enemy hit by a sage attack
				Player.GetDamage<ShapeshifterDamageClass>() += ShapeshifterSageDamageOnHitCount * 0.02f;
			}

			if (ShapeshifterSetHarpyDamagePool > 0)
			{ // if ShapeshifterSetHarpyDamagePool is above 20, empty it gradually, spawning damaging feathers
				ShapeshifterSetHarpyTimer--;
				if (ShapeshifterSetHarpyDamagePool >= 20 && ShapeshifterSetHarpyTimer <= 0 && modPlayer.LastHitNPC != null)
				{
					ShapeshifterSetHarpyDamagePool -= 20;
					ShapeshifterSetHarpyTimer = 10;
					int projectileType = ModContent.ProjectileType<ShapeshifterHarpyProj>();
					NPC target = modPlayer.LastHitNPC;
					int damage = GetShapeshifterDamage(15);
					Vector2 velocity = Vector2.UnitY.RotatedByRandom(MathHelper.ToRadians(20)) * 8f;
					Vector2 position = target.Center + target.velocity * 20f - velocity * 90f;
					Projectile newProjectile = Projectile.NewProjectileDirect(Player.GetSource_FromAI(), position, velocity, projectileType, damage, 0f, Player.whoAmI, ai2: target.Center.Y);
					newProjectile.CritChance = GetShapeshifterCrit();
				}
			}
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (ShapeshifterSetHarpy && OrchidModProjectile.IsValidTarget(target) && Player.whoAmI == Main.myPlayer && IsShapeshifted)
			{
				int projectileType = ModContent.ProjectileType<ShapeshifterHarpyProj>();
				if (target.Center.Y /* - target.width * 0.5f */ > Player.Center.Y + Player.height * 0.5f && proj.type != projectileType)
				{ // attacks from above store 50% of the damage dealt in ShapeshifterSetHarpyDamagePool
					ShapeshifterSetHarpyDamagePool += (int)(damageDone * 0.5f);
					if (ShapeshifterSetHarpyDamagePool > 100)
					{
						ShapeshifterSetHarpyDamagePool = 100;
					}
				}
			}
		}

		public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
		{
			if (IsShapeshifted && !Player.noKnockback)
			{
				Shapeshift.ShapeshiftOnHitByAnything(hurtInfo, ShapeshiftAnchor.Projectile, ShapeshiftAnchor, Player, this);
				Shapeshift.ShapeshiftOnHitByProjectile(proj, hurtInfo, ShapeshiftAnchor.Projectile, ShapeshiftAnchor, Player, this);

				if (!Player.noKnockback)
				{ // Player knockback on hit
					ShapeshiftAnchor.Projectile.velocity = new Vector2(3f * hurtInfo.HitDirection, -3f);
				}
			}
		}

		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
		{
			if (IsShapeshifted)
			{
				Shapeshift.ShapeshiftOnHitByAnything(hurtInfo, ShapeshiftAnchor.Projectile, ShapeshiftAnchor, Player, this);
				Shapeshift.ShapeshiftOnHitByNPC(npc, hurtInfo, ShapeshiftAnchor.Projectile, ShapeshiftAnchor, Player, this);

				if (!Player.noKnockback)
				{ // Player knockback on hit
					ShapeshiftAnchor.Projectile.velocity = new Vector2(3f * hurtInfo.HitDirection, -3f);
				}
			}
		}

		public override void ModifyHurt(ref Player.HurtModifiers modifiers)
		{
			if (IsShapeshifted)
			{
				Shapeshift.ShapeshiftModifyHurt(ref modifiers, ShapeshiftAnchor.Projectile, ShapeshiftAnchor, Player, this);
			}
		}

		public override bool FreeDodge(Player.HurtInfo info)
		{
			if (IsShapeshifted)
			{
				return Shapeshift.ShapeshiftFreeDodge(info, ShapeshiftAnchor.Projectile, ShapeshiftAnchor, Player, this);
			}
			return false;
		}
	}
}
