using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler
{
	public abstract class OrchidModGamblerDie : OrchidModItem
	{
		public int diceID = -1;
		public int diceCost = 0;
		public int diceDuration = 0;

		public virtual void SafeSetDefaults() {}
			public virtual void SafeHoldItem() {}

		public sealed override void SetDefaults() {
			SafeSetDefaults();
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
			item.noMelee = true;
			item.maxStack = 1;
			item.useStyle = 4;
			item.UseSound = SoundID.Item35;
			item.useAnimation = 20;
			item.useTime = 20;
			item.autoReuse = false;
		}  
		
		public override bool CloneNewInstances {
			get
			{
				return true;
			}
		}
		
		public sealed override void HoldItem(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerUIFightDisplay = true;
			SafeHoldItem();
		}
		
		public override bool UseItem(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			OrchidModGamblerHelper.removeGamblerChip(100, this.diceCost, player, modPlayer, mod);
			OrchidModGamblerHelper.rollGamblerDice(this.diceID, this.diceDuration, player, modPlayer, mod);
			return true;
		}
		
		public override bool CanUseItem(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			
			if (modPlayer.gamblerChips < this.diceCost || modPlayer.gamblerCardCurrent.type == 0) {
				return false;
			}
			return base.CanUseItem(player);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null) {
				tooltips.Insert(1, new TooltipLine(mod, "ClassTag", "-Gambler Class-")
				{
					overrideColor = new Color(255, 200, 0)
				});
			}
			
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null) tooltips.Remove(tt);
			
			tt = tooltips.FirstOrDefault(x => x.Name == "CritChance" && x.mod == "Terraria");
			if (tt != null) tooltips.Remove(tt);
			
			tt = tooltips.FirstOrDefault(x => x.Name == "Knockback" && x.mod == "Terraria");
			if (tt != null) tooltips.Remove(tt);
			
			tt = tooltips.FirstOrDefault(x => x.Name == "Speed" && x.mod == "Terraria");
			if (tt != null) tooltips.Remove(tt);
			
			int index = tooltips.FindIndex(ttip => ttip.mod.Equals("Terraria") && ttip.Name.Equals("Tooltip0"));
			if (index != -1)
			{
				tooltips.Insert(index, new TooltipLine(mod, "DiceDuration", "Lasts " + this.diceDuration + " seconds"));
				
				if (this.diceCost < 2) {
					tooltips.Insert(index, new TooltipLine(mod, "DiceCost", "Uses " + this.diceCost + " chip"));
				} else {
					tooltips.Insert(index, new TooltipLine(mod, "DiceCost", "Uses " + this.diceCost + " chips"));
				}
			}
		}
	}
}
