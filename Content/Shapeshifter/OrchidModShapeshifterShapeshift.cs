﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Content.General.Prefixes;
using OrchidMod.Content.Guardian;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter
{
	public abstract class OrchidModShapeshifterShapeshift : OrchidModShapeshifterItem
	{
		public int ShapeshiftWidth;
		public int ShapeshiftHeight;
		public bool AutoReuseLeft;
		public bool AutoReuseRight;
		public ShapeshifterShapeshiftType ShapeshiftType;

		public virtual string LeftClickTooltip => Language.GetTextValue(Mod.GetLocalizationKey("Misc.ShapeshifterLeftClick")) + Language.GetTextValue(Mod.GetLocalizationKey("Items." + GetType().Name + ".LeftClick"));
		public virtual string RightClickTooltip => Language.GetTextValue(Mod.GetLocalizationKey("Misc.ShapeshifterRightClick")) + Language.GetTextValue(Mod.GetLocalizationKey("Items." + GetType().Name + ".RightClick"));
		public virtual string JumpTooltip => Language.GetTextValue(Mod.GetLocalizationKey("Misc.ShapeshifterJump")) + Language.GetTextValue(Mod.GetLocalizationKey("Items." + GetType().Name + ".Jump"));
		public virtual string PassiveTooltip => Language.GetTextValue(Mod.GetLocalizationKey("Misc.ShapeshifterPassive")) + Language.GetTextValue(Mod.GetLocalizationKey("Items." + GetType().Name + ".Passive"));
		public virtual string TypeTooltip => Language.GetTextValue(Mod.GetLocalizationKey("Misc.Shapeshifter" + ShapeshiftType.ToString()));
		public virtual string ShapeshiftTexture => Texture + "_Shapeshift";
		public virtual string IconTexture => Texture + "_Icon";
		public virtual void PreDrawShapeshift(SpriteBatch spriteBatch, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Vector2 drawPosition, Rectangle drawRectangle, SpriteEffects effect, Player player, Color lightColor) { } // Called before drawing the shapeshift anchor if it is going to be drawn
		public virtual void PostDrawShapeshift(SpriteBatch spriteBatch, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Vector2 drawPosition, Rectangle drawRectangle, SpriteEffects effect, Player player, Color lightColor) { } // Called after drawing the shapeshift anchor
		public virtual bool ShouldDrawShapeshift(SpriteBatch spriteBatch, Projectile projectile, Player player, ref Color lightColor) { return true; } // Called before drawing the shapeshift anchor, return false to prevent it
		public virtual void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) { } // Very important, this is the AI of the shapeshift
		public virtual void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) { } // Makes stuff happen when the player just transformed
		public virtual void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor) { } // Makes stuff happen when the anchor disappears (player leaves shapeshift form)
		public virtual bool CanLeftClick(ShapeshifterShapeshiftAnchor anchor) => Main.mouseLeft && (Main.mouseLeftRelease || AutoReuseLeft) && anchor.CanLeftClick; // left click has priority over right click
		public virtual bool CanRightClick(ShapeshifterShapeshiftAnchor anchor) => Main.mouseRight && (Main.mouseRightRelease || AutoReuseRight) && anchor.CanRightClick && anchor.CanLeftClick && !Main.mouseLeft;
		public virtual void SafeHoldItem(Player player) { }

		private static int AllowGFXOffY; // used to prevent visual issues with slopes

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) => false;


		public sealed override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<ShapeshifterDamageClass>();
			Item.noMelee = true;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.maxStack = 1;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.shootSpeed = 0f;
			ShapeshiftWidth = 10;
			ShapeshiftHeight = 10;
			AutoReuseLeft = true;
			AutoReuseRight = false;

			SafeSetDefaults();
			Item.useAnimation = Item.useTime;
		}

		public sealed override void HoldItem(Player player)
		{
			var projectileType = ModContent.ProjectileType<ShapeshifterShapeshiftAnchor>();

			if (player.ownedProjectileCounts[projectileType] != 0)
			{
				Projectile proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType);
				if (proj != null && proj.ModProjectile is ShapeshifterShapeshiftAnchor anchor)
				{
					if (anchor.ShapeshifterItem != Item)
					{
						proj.Kill();
					}
				}
			}
			SafeHoldItem(player);
		}

		public override bool CanUseItem(Player player)
		{
			int projectileType = ModContent.ProjectileType<ShapeshifterShapeshiftAnchor>();
			if (player.ownedProjectileCounts[projectileType] != 1)
			{
				if (IsLocalPlayer(player))
				{
					foreach (Projectile projectile in Main.projectile)
					{
						if (projectile.active && projectile.owner == player.whoAmI && projectile.type == projectileType)
						{
							projectile.Kill();
						}
					}

					var index = Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center.X, player.Center.Y, 0f, 0f, projectileType, 0, 0f, player.whoAmI);

					var proj = Main.projectile[index];
					if (proj.ModProjectile is not ShapeshifterShapeshiftAnchor anchor)
					{
						proj.Kill();
					}
					else
					{
						if (player.mount.Active)
						{
							player.mount.Dismount(player);
						}

						player.RemoveAllGrapplingHooks();
						anchor.OnChangeSelectedItem(player);
						anchor.NeedNetUpdate = true;
					}
				}
			}
			return false;
		}


		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				tt.Text = damageValue + " " + Language.GetTextValue(ModContent.GetInstance<OrchidMod>().GetLocalizationKey("DamageClasses.ShapeshifterDamageClass.DisplayName"));
			}

			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
			tooltips.Insert(index + 1, new TooltipLine(Mod, "ToolTipType", TypeTooltip)
			{
				OverrideColor = OrchidColors.GetClassTagColor(ClassTags.Shapeshifter)
			});
			tooltips.Insert(index + 2, new TooltipLine(Mod, "ToolTipLeftClick", LeftClickTooltip));
			tooltips.Insert(index + 3, new TooltipLine(Mod, "ToolTipRightClick", RightClickTooltip));
			tooltips.Insert(index + 4, new TooltipLine(Mod, "ToolTipJump", JumpTooltip));
			tooltips.Insert(index + 5, new TooltipLine(Mod, "ToolTipPassive", PassiveTooltip));
		}

		// Custom methods

		public void FinalVelocityCalculations(ref Vector2 intendedVelocity, Projectile projectile, Player player, bool stepUpDown = false, bool cancelSlopeOffet = true)
		{ // Prevents the player from phashing through tiles after all othe velocity calculations
			if (stepUpDown)
			{ // Allows the projectiles to "step" up and down tiles like a walking player would
				if (StepUpTiles(intendedVelocity, projectile, player))
				{
					intendedVelocity.Y = 0;
				}
			}

			if (cancelSlopeOffet)
			{ // Prevents issues with player phasing through slopes
				if (projectile.gfxOffY != 0)
				{ // fuck slopes all my homies hate slopes
					if (intendedVelocity.Y > -0.5f)
					{
						intendedVelocity.Y = -0.5f;
					}
					projectile.gfxOffY = 0;
					//player.gfxOffY = 0;
				}

				if (player.gfxOffY != 0 && AllowGFXOffY <= 0)
				{ // I hate slopes
					player.gfxOffY = 0;
				}

				if (AllowGFXOffY > 0)
				{
					AllowGFXOffY--;
				}
			}

			Vector2 finalVelocity = Vector2.Zero;
			intendedVelocity /= 10f;
			for (int i = 0; i < 10; i++)
			{
				finalVelocity += Collision.TileCollision(projectile.position + finalVelocity, intendedVelocity, projectile.width, projectile.height, false, false, (int)player.gravDir);
			}

			projectile.velocity = finalVelocity;
		}

		public bool CanGoUp(Vector2 intendedVelocity, Projectile projectile, Player player)
		{ // false if the player is unable to go upbecause of a tile above their hitbox
			float velocityY = intendedVelocity.Y;
			if (intendedVelocity.Y > -1f)
			{
				intendedVelocity.Y = -1f;
			}

			return (Collision.TileCollision(projectile.position, Vector2.UnitY * intendedVelocity.Y, projectile.width, projectile.height, false, false, (int)player.gravDir) == Vector2.UnitY * intendedVelocity.Y);
		}

		public bool StepUpTiles(Vector2 intendedVelocity, Projectile projectile, Player player)
		{
			if (CanGoUp(intendedVelocity, projectile, player))
			{
				Vector2 finalVelocity = Vector2.Zero;
				intendedVelocity.Y = 0;
				intendedVelocity /= 10f;
				for (int i = 0; i < 10; i++)
				{
					finalVelocity += Collision.TileCollision(projectile.position + finalVelocity, intendedVelocity, projectile.width, projectile.height, false, false, (int)player.gravDir);
					if (Math.Abs(finalVelocity.X - intendedVelocity.X * (i + 1)) > 0.001f)
					{ // A tile was hit on the X axis
						Vector2 newPosition = projectile.position;
						newPosition.Y -= 16;
						Vector2 finalVelocity2 = Vector2.Zero;
						float offY;
						for (int j = 0; j < 10; j++)
						{
							finalVelocity2 += Collision.TileCollision(newPosition + finalVelocity2, intendedVelocity, projectile.width, projectile.height, false, false, (int)player.gravDir);
							if (Math.Abs(finalVelocity2.X - intendedVelocity.X * (j + 1)) > 0.001f)
							{ // A tile was hit on the X axis above the first tested tile
								if (finalVelocity2.X > finalVelocity.X)
								{ // Player will hit a tile from the new position, but could go further on the X axis than previously. Go up a tile
									newPosition = projectile.position + finalVelocity2;
									newPosition.Y -= 16f;
									offY = 16f - Collision.TileCollision(newPosition, Vector2.UnitY * 16f, projectile.width, projectile.height, false, false, (int)player.gravDir).Y;
									projectile.position.Y -= offY;
								}
								AllowGFXOffY = 10;
								return true;
							}
						}

						// The tile above is empty and the player can go up the tile
						newPosition = projectile.position + finalVelocity2;
						newPosition.Y -= 16f;
						offY = 16f - Collision.TileCollision(newPosition, Vector2.UnitY * 16f, projectile.width, projectile.height, false, false, (int)player.gravDir).Y;
						projectile.position.Y -= offY;
						AllowGFXOffY = 10;
						return true;
					}
				}
			}
			return false;
		}

		/*
		public bool CanGoDown(Vector2 intendedVelocity, Projectile projectile, Player player)
		{ // false if the player is unable to go upbecause of a tile above their hitbox
			float velocityY = intendedVelocity.Y;
			if (intendedVelocity.Y < 8f)
			{
				intendedVelocity.Y = 8f;
			}

			return (Collision.TileCollision(projectile.position, Vector2.UnitY * intendedVelocity.Y, projectile.width, projectile.height, false, false, (int)player.gravDir) == Vector2.UnitY * intendedVelocity.Y);
		}


		public void StepDownTiles(Vector2 intendedVelocity, Projectile projectile, Player player)
		{
			if (CanGoDown(intendedVelocity, projectile, player))
			{
				Vector2 finalVelocity = Vector2.Zero;
				intendedVelocity.Y = 1.6f;
				for (int i = 0; i < 11; i++)
				{
					finalVelocity += Collision.TileCollision(projectile.position + finalVelocity, intendedVelocity, projectile.width, projectile.height, false, false, (int)player.gravDir);
					if (Math.Abs(finalVelocity.Y - intendedVelocity.Y * (i + 1)) > 0.001f)
					{ // A tile was hit on the Y axis, warp the player down
						Main.NewText("Down");
						Vector2 newPosition = projectile.position;
						projectile.position.Y += finalVelocity.Y;
						player.gfxOffY = finalVelocity.Y;
						return;
					}
				}
			}
		}
		*/
	}
}
