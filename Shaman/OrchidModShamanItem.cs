﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using OrchidMod.Common.Attributes;
using OrchidMod.Common;
using Terraria.GameContent.Creative;

namespace OrchidMod.Shaman
{
	public enum ShamanCatalystType : int
	{
		IDLE = 0,
		AIM = 1,
		ROTATE = 2
	}

	[ClassTag(ClassTags.Shaman)]
	public abstract class OrchidModShamanItem : ModItem
	{
		public int Element = 0;
		public int energy = 1;
		public Color? catalystEffectColor = null; // TODO: ...
		public ShamanCatalystType catalystType = ShamanCatalystType.IDLE;

		// ...

		protected override bool CloneNewInstances => true;

		public sealed override void SetStaticDefaults()
		{
			Item.staff[Item.type] = true;
			this.SafeSetStaticDefaults();
		}

		public sealed override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<ShamanDamageClass>();

			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Thrust;

			OrchidModGlobalItem orchidItem = Item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeapon = true;

			this.SafeSetDefaults();

			orchidItem.shamanWeaponElement = this.Element;
		}

		public sealed override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman shaman = player.GetModPlayer<OrchidShaman>();
			Vector2 mousePosition = Main.MouseWorld;
			Vector2? catalystCenter = shaman.ShamanCatalystPosition;
			shaman.UIDisplayTimer = shaman.UIDisplayDelay;

			//if (catalystCenter != null && Collision.CanHit(position, 0, 0, position + (catalystCenter.Value - position), 0, 0))
			if (catalystCenter != null)
				position = catalystCenter.Value;

			Vector2 newMove = mousePosition - position;
			newMove.Normalize();
			newMove *= velocity.Length();
			velocity = newMove;

			if (this.SafeShoot(player, source, position, velocity, type, damage, knockback))
			{
				this.NewShamanProjectile(player, source, position, velocity, type, damage, knockback);
			}

			return false;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			OrchidShaman shamanPlayer = player.GetModPlayer<OrchidShaman>();
			if (player.altFunctionUse == 2)
			{ // Right click
				if (shamanPlayer.GetShamanicBondValue((ShamanElement)Element) >= 100 && !shamanPlayer.IsShamanicBondReleased((ShamanElement)Element))
				{
					shamanPlayer.ReleaseShamanicBond(this);
				}
				else
				{
					switch (this.Element)
					{
						case 1:
							if (shamanPlayer.ShamanFireBondReleased)
							{
								shamanPlayer.ShamanFireBondReleased = false;
								shamanPlayer.ShamanFireBond = 0;
							}
							break;
						case 2:
							if (shamanPlayer.ShamanWaterBondReleased)
							{
								shamanPlayer.ShamanWaterBondReleased = false;
								shamanPlayer.ShamanWaterBond = 0;
							}
							break;
						case 3:
							if (shamanPlayer.ShamanAirBondReleased)
							{
								shamanPlayer.ShamanAirBondReleased = false;
								shamanPlayer.ShamanAirBond = 0;
							}
							break;
						case 4:
							if (shamanPlayer.ShamanEarthBondReleased)
							{
								shamanPlayer.ShamanEarthBondReleased = false;
								shamanPlayer.ShamanEarthBond = 0;
							}
							break;
						case 5:
							if (shamanPlayer.ShamanSpiritBondReleased)
							{
								shamanPlayer.ShamanSpiritBondReleased = false;
								shamanPlayer.ShamanSpiritBond = 0;
							}
							break;
						default:
							break;
					}
				}
				return false;
			}
			else
			{ // Left click
				switch (this.Element)
				{
					case 1:
						if (shamanPlayer.ShamanFireBondReleased) return false;
						break;
					case 2:
						if (shamanPlayer.ShamanWaterBondReleased) return false;
						break;
					case 3:
						if (shamanPlayer.ShamanAirBondReleased) return false;
						break;
					case 4:
						if (shamanPlayer.ShamanEarthBondReleased) return false;
						break;
					case 5:
						if (shamanPlayer.ShamanSpiritBondReleased) return false;
						break;
					default:
						break;
				}
			}
			return base.CanUseItem(player);
		}

		public sealed override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			player.itemLocation = (player.MountedCenter + new Vector2(player.direction * 12, player.gravDir * 24)).Floor();
			player.itemRotation = -player.direction * player.gravDir * MathHelper.PiOver4;
		}

		public sealed override void HoldItem(Player player)
		{
			var shaman = player.GetModPlayer<OrchidShaman>();
			var catalystType = ModContent.ProjectileType<CatalystAnchor>();

			if (player.ownedProjectileCounts[catalystType] == 0)
			{
				var index = Projectile.NewProjectile(null, player.Center, Vector2.Zero, catalystType, 0, 0f, player.whoAmI); // change source to something that's not null ?
				shaman.shamanCatalystIndex = index;

				var proj = Main.projectile[index];
				if (!(proj.ModProjectile is CatalystAnchor catalyst))
				{
					proj.Kill();
					shaman.shamanCatalystIndex = -1;
				}
				else catalyst.OnChangeSelectedItem(player);
			}
			else
			{
				var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == catalystType);
				if (proj != null && proj.ModProjectile is CatalystAnchor catalyst)
				{
					if (catalyst.SelectedItem != player.selectedItem)
					{
						catalyst.OnChangeSelectedItem(player);
					}
				}
				else shaman.shamanCatalystIndex = -1;
			}

			this.SafeHoldItem();
		}

		public sealed override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			this.SafeModifyWeaponDamage(player, ref damage);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.Text = damageValue + " shamanic damage";
			}

			if (Element > 0)
			{
				Color[] colors = new Color[5]
				{
					new Color(194, 38, 31),
					new Color(0, 119, 190),
					new Color(75, 139, 59),
					new Color(255, 255, 102),
					new Color(138, 43, 226)
				};

				string[] strType = new string[5] { "Fire", "Water", "Air", "Earth", "Spirit" };

				int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
				if (index != -1) tooltips.Insert(index + 1, new TooltipLine(Mod, "BondType", $"Bond type: [c/{Terraria.ID.Colors.AlphaDarken(colors[Element - 1]).Hex3()}:{strType[Element - 1]}]"));
			}
		}

		// ...

		public virtual void SafeSetStaticDefaults() { }
		public virtual void SafeSetDefaults() { }
		public virtual void SafeHoldItem() { }
		public virtual void SafeModifyWeaponDamage(Player player, ref StatModifier damage) { }
		public virtual bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) => true;
		public virtual void ExtraAICatalyst(Projectile projectile, bool after) { }
		public virtual void PostAICatalyst(Projectile projectile) { }
		public virtual void PostDrawCatalyst(SpriteBatch spriteBatch, Projectile projectile, Player player, Color lightColor) { }
		public virtual bool PreAICatalyst(Projectile projectile) { return true; }
		public virtual bool PreDrawCatalyst(SpriteBatch spriteBatch, Projectile projectile, Player player, ref Color lightColor) { return true; }
		public virtual void AIReleased(Projectile projectile) { }
		public virtual void PostAIReleased(Projectile projectile) { }
		public virtual void OnRelease(Projectile projectile) { }

		public virtual string CatalystTexture => "OrchidMod/Shaman/CatalystTextures/" + this.Name + "_Catalyst";

		// ...

		public int NewShamanProjectile(Player player, EntitySource_ItemUse source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float ai0 = 0.0f, float ai1 = 0.0f)
		{
			int newProjectileIndex = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, ai0, ai1);
			Projectile newProjectile = Main.projectile[newProjectileIndex];
			OrchidModProjectile.setShamanBond(newProjectile, this.Element);
			return newProjectileIndex;
		}
	}
}
