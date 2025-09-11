using Microsoft.Xna.Framework;
using OrchidMod.Common;
using OrchidMod.Common.Attributes;
using OrchidMod.Content.General.Prefixes;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	[CrossmodContent("ThoriumMod")]
	public class ThoriumDarksteelGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 36;
			Item.height = 40;
			Item.knockBack = 7f;
			Item.damage = 400;
			Item.value = Item.sellPrice(0, 0, 35, 0);
			Item.rare = ItemRarityID.Green;
			Item.useTime = 35;
			StrikeVelocity = 15f;
			ParryDuration = 75;
		}

		public override Color GetColor(bool offHand)
		{
			return new Color(255, 200, 200);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				tt.Text = Language.GetTextValue("Mods.OrchidMod.UI.GuardianItem.DamageOfDefense", damageValue);
			}

			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
			tooltips.Insert(index + 1, new TooltipLine(Mod, "ParryDuration", Language.GetTextValue("Mods.OrchidMod.UI.GuardianItem.ParryDuration", OrchidUtils.FramesToSeconds((int)(ParryDuration * Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration())))));

			string click = ModContent.GetInstance<OrchidClientConfig>().GuardianSwapGauntletImputs ? Language.GetTextValue("Mods.OrchidMod.UI.GuardianItem.LeftClick") : Language.GetTextValue("Mods.OrchidMod.UI.GuardianItem.RightClick");
			tooltips.Insert(index + 2, new TooltipLine(Mod, "ClickInfo", Language.GetTextValue("Mods.OrchidMod.UI.GuardianItem.Parry", click))
			{
				OverrideColor = new Color(175, 255, 175)
			});
		}

		public override void ExtraAIGauntlet(Projectile projectile)
		{
			if (projectile.ai[2] > 0)
			{
				Main.player[projectile.owner].GetModPlayer<OrchidGuardian>().GuardianItemCharge += 0.3f / Item.useTime * Main.player[projectile.owner].statDefense;
			}
		}

		public override bool OnPunch(Player player, OrchidGuardian guardian, Projectile projectile, bool charged, ref int damage)
		{
			damage = (int)Math.Max(1, damage * 0.01f * player.statDefense);
			StrikeVelocity = 15f + (0.15f * player.statDefense);
			return true;
		}
		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.Anvils);
				recipe.AddIngredient(thoriumMod, "aDarksteelAlloy", 10);
				recipe.Register();
			}
		}
	}
}
