﻿using Microsoft.Xna.Framework;
using OrchidMod.Common;
using OrchidMod.Common.Attributes;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler
{
	[ClassTag(ClassTags.Gambler)]
	public abstract class OrchidModGamblerDie : OrchidModItem
	{
		public int diceID = -1;
		public int diceCost = 0;
		public int diceDuration = 0;

		public virtual void SafeSetDefaults() { }
		public virtual void SafeHoldItem() { }

		public sealed override void SetDefaults()
		{
			SafeSetDefaults();
			Item.DamageType = ModContent.GetInstance<GamblerChipDamageClass>();
			Item.noMelee = true;
			Item.maxStack = 1;
			Item.useStyle = 4;
			Item.UseSound = SoundID.Item35;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.autoReuse = false;
		}

		protected override bool CloneNewInstances => true;

		public sealed override void HoldItem(Player player)
		{
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			modPlayer.gamblerUIFightDisplay = true;
			SafeHoldItem();
		}

		public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */
		{
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			modPlayer.RemoveGamblerChip(100, this.diceCost);
			modPlayer.RollGamblerDice(this.diceID, this.diceDuration);
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();

			if (modPlayer.gamblerChips < this.diceCost || modPlayer.gamblerCardCurrent.type == 0)
			{
				return false;
			}
			return base.CanUseItem(player);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null) tooltips.Remove(tt);

			tt = tooltips.FirstOrDefault(x => x.Name == "CritChance" && x.Mod == "Terraria");
			if (tt != null) tooltips.Remove(tt);

			tt = tooltips.FirstOrDefault(x => x.Name == "Knockback" && x.Mod == "Terraria");
			if (tt != null) tooltips.Remove(tt);

			tt = tooltips.FirstOrDefault(x => x.Name == "Speed" && x.Mod == "Terraria");
			if (tt != null) tooltips.Remove(tt);

			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Tooltip0"));
			if (index != -1)
			{
				tooltips.Insert(index, new TooltipLine(Mod, "DiceDuration", "Lasts " + this.diceDuration + " seconds"));

				if (this.diceCost < 2)
				{
					tooltips.Insert(index, new TooltipLine(Mod, "DiceCost", "Uses " + this.diceCost + " chip"));
				}
				else
				{
					tooltips.Insert(index, new TooltipLine(Mod, "DiceCost", "Uses " + this.diceCost + " chips"));
				}
			}
		}
	}
}
