using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace OrchidMod.Gambler
{
	public class GamblerReset : OrchidModItem
	{
		public	override void SetDefaults() {
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
			item.noMelee = true;
			item.maxStack = 1;
			item.width = 20;
			item.height = 26;
			item.useStyle = 4;
			item.UseSound = SoundID.Item64;
			item.useAnimation = 20;
			item.useTime = 20;
			item.rare = -12;
		}
		
		public override bool AltFunctionUse(Player player) {
			return true;
		}
		
		public override bool CanUseItem(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			
			if (player.altFunctionUse == 2) {
				return false;
			} else {
				OrchidModGamblerHelper.clearGamblerCards(player, modPlayer);
				OrchidModGamblerHelper.onRespawnGambler(player, modPlayer);
			}
			return base.CanUseItem(player);
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : RESET");
		    Tooltip.SetDefault("Test item : resets gambling cards"
							+  "\n[c/FF0000:Test Item]");
		}
	}
}
