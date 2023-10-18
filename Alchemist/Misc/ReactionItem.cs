using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.UI;
using OrchidMod.Common;
using OrchidMod.Common.Attributes;
using OrchidMod.Common.UIs;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Misc
{
	[ClassTag(ClassTags.Alchemist)]
	public class ReactionItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 38;
			Item.maxStack = 1;
			Item.value = Item.sellPrice(0, 0, 2, 0);
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = 1;
			Item.noUseGraphic = true;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item7;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			if (player.altFunctionUse == 2 && Main.mouseRightRelease)
			{
				if (!modPlayer.alchemistBookUIDisplay) UISystem.GetUIState<AlchemistBookUIState>().OpenBook();
				else modPlayer.alchemistBookUIDisplay = false;
				return false;
			}
			else if (modPlayer.alchemistNbElements < 2 || player.FindBuffIndex(Mod.Find<ModBuff>("ReactionCooldown").Type) > -1 || modPlayer.alchemistBookUIDisplay)
			{
				return false;
			}
			return base.CanUseItem(player);
		}

		public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */
		{
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			AlchemistHiddenReactionHelper.triggerAlchemistReaction(Mod, player, modPlayer);
			return true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				tooltips.Insert(1, new TooltipLine(Mod, "ClassTag", "-Alchemist Class-")
				{
					OverrideColor = new Color(155, 255, 55)
				});
			}
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hidden Reactions Codex");
			/* Tooltip.SetDefault("Left click to trigger alchemist hidden reactions"
							+ "\nThe 'Hidden Reaction' key can be used instead of this item"
							+ "\nRight click to open the hidden reactions codex"); */
		}

	}
}
