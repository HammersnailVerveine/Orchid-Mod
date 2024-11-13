using Microsoft.Xna.Framework;
using OrchidMod.Common;
using OrchidMod.Content.General.Prefixes;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class ThoriumDarksteelGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 38;
			Item.height = 42;
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

			int tooltipSeconds = Math.DivRem((int)(parryDuration * Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration()), 60, out int tooltipTicks);

			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
			tooltips.Insert(index + 1, new TooltipLine(Mod, "ParryDuration", tooltipSeconds + "." + (int)(tooltipTicks * (100 / 60f)) + " parry duration"));

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
			strikeVelocity = 15f * (1 + player.statDefense / 100f);
			return true;
		}
	}
}
