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
			Item.value = Item.sellPrice(0, 0, 34, 0);
			Item.rare = ItemRarityID.Green;
			Item.useTime = 35;
			strikeVelocity = 15f;
			parryDuration = 75;
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
				tt.Text = "Deals " + damageValue + "% of defense as " + Language.GetTextValue(ModContent.GetInstance<OrchidMod>().GetLocalizationKey("DamageClasses.GuardianDamageClass.DisplayName"));
			}

			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
			tooltips.Insert(index + 1, new TooltipLine(Mod, "ParryDuration", OrchidUtils.FramesToSeconds((int)(parryDuration * Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration())) + " second parry duration"));

			string click = ModContent.GetInstance<OrchidClientConfig>().SwapGauntletImputs ? "Left" : "Right";
			tooltips.Insert(index + 2, new TooltipLine(Mod, "ClickInfo", click + " click to parry")
			{
				OverrideColor = new Color(175, 255, 175)
			});
		}

		public override void ExtraAIGauntlet(Projectile projectile)
		{
			if (projectile.ai[2] > 0)
			{
				Main.player[projectile.owner].GetModPlayer<OrchidGuardian>().GuardianGauntletCharge += 0.3f / Item.useTime * Main.player[projectile.owner].statDefense;
			}
		}

		public override bool OnPunch(Player player, OrchidGuardian guardian, Projectile projectile, bool charged, ref int damage)
		{
			damage = (int)Math.Max(1, damage * player.statDefense / 100f);
			//for some god forsaken reason this always gets rounded which leads to 150 defense being a massive breakpoint. needs to be looked into more
			strikeVelocity = 15f * (1f + player.statDefense / 100f);
			return true;
		}
		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.WorkBenches);
				recipe.AddIngredient(thoriumMod, "aDarksteelAlloy", 10);
				recipe.Register();
			}
		}
	}
}
