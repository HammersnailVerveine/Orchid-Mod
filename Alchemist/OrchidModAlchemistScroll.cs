using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist
{
	public abstract class OrchidModAlchemistScroll : ModItem
	{
		public int hintLevel = 0;
		
		public virtual void SafeSetDefaults() {}

		public sealed override void SetDefaults() {
			SafeSetDefaults();
			item.width = 36;
			item.height = 32;
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
			item.noMelee = true;
			item.useStyle = 4;
			item.UseSound = SoundID.Item64;
			item.consumable = true;
			item.autoReuse = false;
			item.useAnimation = 10;
			item.useTime = 10;
			item.maxStack = 99;
		}
		
		public override bool AltFunctionUse(Player player) {
			return true;
		}
		
		public override bool UseItem(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			AlchemistHiddenReactionHelper.addAlchemistHint(player, modPlayer, this.hintLevel);
			return true;
		}
		
		// public override bool CloneNewInstances {
			// get
			// {
				// return true;
			// }
		// }

		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				tooltips.Insert(1, new TooltipLine(mod, "ClassTag", "-Alchemist Class-")
				{
					overrideColor = new Color(155, 255, 55)
				});
			}
		}
	}
}
