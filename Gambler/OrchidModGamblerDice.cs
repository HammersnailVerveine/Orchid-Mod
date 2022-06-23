using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler
{
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
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerUIFightDisplay = true;
			SafeHoldItem();
		}

		public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			OrchidModGamblerHelper.removeGamblerChip(100, this.diceCost, player, modPlayer, Mod);
			OrchidModGamblerHelper.rollGamblerDice(this.diceID, this.diceDuration, player, modPlayer, Mod);
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			if (modPlayer.gamblerChips < this.diceCost || modPlayer.gamblerCardCurrent.type == 0)
			{
				return false;
			}
			return base.CanUseItem(player);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				tooltips.Insert(1, new TooltipLine(Mod, "ClassTag", "-Gambler Class-")
				{
					OverrideColor = new Color(255, 200, 0)
				});
			}

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
