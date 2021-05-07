using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace OrchidMod.Alchemist.Misc
{
	public class ReactionItem : OrchidModItem
	{
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 38;
			item.maxStack = 1;
			item.value = Item.sellPrice(0, 0, 2, 0);
			item.useAnimation = 30;
			item.useTime = 30;
			item.useStyle = 1;
			item.noUseGraphic = true;
			item.rare = 1;
			item.UseSound = SoundID.Item7;
		}
		
		public override bool AltFunctionUse(Player player) {
			return true;
		}
		
		public override bool CanUseItem(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (player.altFunctionUse == 2 && Main.mouseRightRelease) {
				Main.PlaySound(modPlayer.alchemistBookUIDisplay ? 11 : 10, (int)player.Center.X ,(int)player.Center.Y, 0);
				modPlayer.alchemistBookUIDisplay = !modPlayer.alchemistBookUIDisplay;
				return false;
			} else if (modPlayer.alchemistNbElements < 2 || player.FindBuffIndex(mod.BuffType("ReactionCooldown")) > -1 || modPlayer.alchemistBookUIDisplay) {
				return false;
			}
			return base.CanUseItem(player);
		}
		
		public override bool UseItem(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			AlchemistHiddenReactionHelper.triggerAlchemistReaction(mod, player, modPlayer);
			return true;
		}
		
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				tooltips.Insert(1, new TooltipLine(mod, "ClassTag", "-Alchemist Class-")
				{
					overrideColor = new Color(155, 255, 55)
				});
			}
		}
		
		public override void HoldItem(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistBookUIItem = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hidden Reactions Codex");
			Tooltip.SetDefault("Left click to trigger alchemist hidden reactions"
							+  "\nThe 'Hidden Reaction' key can be used instead of this item"
							+  "\nRight click to open the hidden reactions codex");
		}

	}
}
