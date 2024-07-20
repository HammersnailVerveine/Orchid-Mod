using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Global.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.NPC;

namespace OrchidMod.Content.Guardian
{
	public abstract class OrchidModGuardianStandard : OrchidModGuardianItem
	{
		public int slamStacks;
		public int blockStacks;

		public virtual string ShaftTexture => Texture + "_Shaft";
		public virtual string FlagUpTexture => Texture + "_FlagUp";
		public virtual string FlagDownTexture => Texture + "_FlagDown";
		public virtual void OnCharge(Player player, OrchidGuardian guardian) { }
		public virtual void EffectSimple(Player player, OrchidGuardian guardian) { }
		public virtual void EffectUpgrade(Player player, OrchidGuardian guardian) { }
		public virtual void ExtraAIStandard(Projectile projectile) { }
		public virtual void PostDrawStandard(SpriteBatch spriteBatch, Projectile projectile, Player player, Color lightColor) { }
		public virtual bool PreDrawStandard(SpriteBatch spriteBatch, Projectile projectile, Player player, ref Color lightColor) { return true; }
		public virtual Color GetColor(bool offHand) => Color.White;

		public virtual void SafeHoldItem(Player player) { }

		public sealed override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<GuardianDamageClass>();
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.maxStack = 1;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Thrust;
			Item.useTime = 30;
			Item.knockBack = 0f;
			Item.damage = 0;
			slamStacks = 0;
			blockStacks = 0;

			OrchidGlobalItemPerEntity orchidItem = Item.GetGlobalItem<OrchidGlobalItemPerEntity>();
			orchidItem.guardianWeapon = true;

			SafeSetDefaults();
			Item.useAnimation = Item.useTime;
		}

		// public override bool WeaponPrefix() => true;

		/*
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool WeaponPrefix() => true;
		*/

		public override bool CanUseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer && !player.cursed)
			{
				var projectileType = ModContent.ProjectileType<GuardianStandardAnchor>();
				if (player.ownedProjectileCounts[projectileType] > 0)
				{
					var guardian = player.GetModPlayer<OrchidGuardian>();
					var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType);
					if (proj != null && proj.ModProjectile is GuardianStandardAnchor standard)
					{
						// left click
					}
				}
			}
			return false;
		}

		public GuardianShieldAnchor GetAnchor(Player player)
		{
			var projectileType = ModContent.ProjectileType<GuardianShieldAnchor>();
			if (player.ownedProjectileCounts[projectileType] > 0)
			{
				var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType);
				if (proj != null && proj.ModProjectile is GuardianShieldAnchor shield)
				{
					return shield;
				}
			}
			return null;
		}

		public sealed override void HoldItem(Player player)
		{
			var projectileType = ModContent.ProjectileType<GuardianStandardAnchor>();
			var guardian = player.GetModPlayer<OrchidGuardian>();
			guardian.GuardianDisplayUI = 300;

			if (player.ownedProjectileCounts[projectileType] == 0)
			{
				var index = Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center.X, player.Center.Y, 0f, 0f, projectileType, 0, 0f, player.whoAmI);

				var proj = Main.projectile[index];
				if (proj.ModProjectile is not GuardianStandardAnchor standard)
				{
					proj.Kill();
				}
				else
				{
					standard.OnChangeSelectedItem(player);
				}
			}
			else
			{
				var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType);
				if (proj != null && proj.ModProjectile is GuardianStandardAnchor standard)
				{
					if (standard.SelectedItem != player.selectedItem)
					{
						standard.OnChangeSelectedItem(player);
					}
				}
			}
			this.SafeHoldItem(player);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				tt.Text = damageValue + " " + Language.GetTextValue(ModContent.GetInstance<OrchidMod>().GetLocalizationKey("DamageClasses.GuardianDamageClass.DisplayName"));
			}

			/*
			int index = 0;
			tt = tooltips.FirstOrDefault(ttip => ttip.Mod.Equals(Mod) && ttip.Name.Equals("ClassTag"));
			if (tt != null) index = tooltips.FindIndex(ttip => ttip.Mod.Equals(Mod) && ttip.Name.Equals("ClassTag"));
			else index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("ItemName"));
			*/
			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("ItemName"));

			if (blockStacks > 0)
			{
				tooltips.Insert(index + 1, new TooltipLine(Mod, "ShieldStacks", "Grants " + this.blockStacks + " shield block" + (this.blockStacks > 1 ? "s" : ""))
				{
					OverrideColor = new Color(175, 255, 175)
				});
			}

			if (slamStacks > 0)
			{
				tooltips.Insert(index + 1, new TooltipLine(Mod, "ShieldSlams", "Grants " + this.slamStacks + " shield slam" + (this.slamStacks > 1 ? "s" : ""))
				{
					OverrideColor = new Color(175, 255, 175)
				});
			}
		}
	}
}
