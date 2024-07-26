using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Global.Items;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian
{
	public abstract class OrchidModGuardianStandard : OrchidModGuardianItem
	{
		public int slamStacks; // Stam Stacks given by the item
		public int guardStacks; // Block Stacks given by the item
		public int flagOffset; // Number of diagonal pixels from the top-right of the sprite to the base of the flag
		public float auraRange; // Flag effect range in tiles
		public bool affectNearbyPlayers; // Flag has an effect on nearby players
		public bool affectNearbyNPCs; // Flag has an effect on nearby npcs
		public int duration; // Effect duration in ticks

		public virtual string ShaftTexture => Texture + "_Shaft";
		public virtual string FlagUpTexture => Texture + "_FlagUp";
		public virtual string FlagQuarterTexture => Texture + "_FlagQuarter";
		public virtual string FlagTwoQuarterTexture => Texture + "_FlagTwoQuarter";
		public virtual string FlagEndTexture => Texture + "_FlagEnd";
		public virtual void NearbyPlayerEffect(Player player, OrchidGuardian guardian, bool isLocalPlayer, bool charged) { } // isLocalPlayer is true when this is ran on the client being affected
		public virtual void NearbyNPCEffect(Player player, OrchidGuardian guardian, NPC npc, bool isLocalPlayer, bool charged) { } // isLocalPlayer is true when this is ran by the guardian with the flag active
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
			guardStacks = 0;
			flagOffset = 0;
			auraRange = 0;
			duration = 1800; // 30 sec
			affectNearbyPlayers = false;
			affectNearbyNPCs = false;

			OrchidGlobalItemPerEntity orchidItem = Item.GetGlobalItem<OrchidGlobalItemPerEntity>();
			orchidItem.guardianWeapon = true;

			SafeSetDefaults();

			auraRange *= 16f;
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
					if (proj != null && proj.ModProjectile is GuardianStandardAnchor standard && guardian.GuardianStandardCharge == 0f)
					{
						proj.ai[0] = 1f;
						proj.netUpdate = true;
						guardian.GuardianStandardCharge++;
						SoundEngine.PlaySound(SoundID.Item7, player.Center);
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

			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("ItemName"));

			if (guardStacks > 0)
			{
				tooltips.Insert(index + 1, new TooltipLine(Mod, "ShieldStacks", "Grants " + this.guardStacks + " shield block" + (this.guardStacks > 1 ? "s" : ""))
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
