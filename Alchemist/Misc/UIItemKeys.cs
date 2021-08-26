using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Misc
{
	public class UIItemKeys : OrchidModItem
	{
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 34;
			item.maxStack = 1;
			item.value = Item.sellPrice(0, 0, 2, 0);
			item.useAnimation = 30;
			item.useTime = 30;
			item.useStyle = 1;
			item.noUseGraphic = true;
			item.rare = 1;
			item.UseSound = SoundID.Item7;
			item.shoot = ProjectileType<Alchemist.Projectiles.AlchemistRightClick>();
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (player.altFunctionUse == 2)
			{
				return Main.mouseRightRelease && base.CanUseItem(player);
			}
			else
			{
				return Main.mouseLeftRelease && base.CanUseItem(player);
			}
			// return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (!modPlayer.alchemistSelectUIKeysDisplay)
			{
				modPlayer.alchemistSelectUIKeysDisplay = true;
				modPlayer.alchemistSelectUIKeysInitialize = true;
			}
			return true;
		}

		/*
		public override bool UseItem(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (!modPlayer.alchemistSelectUIKeysDisplay) {
				modPlayer.alchemistSelectUIKeysDisplay = true;
				modPlayer.alchemistSelectUIKeysInitialize = true;
			}
			return true;
		}
		*/

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				tooltips.Insert(1, new TooltipLine(mod, "ClassTag", "-Alchemist Class-")
				{
					overrideColor = new Color(155, 255, 55)
				});
			}
		}

		public override void HoldItem(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistSelectUIKeysItem = true;

			if (modPlayer.alchemistSelectUIKeysDisplay)
			{ // Prevents the game from switching items when inputing hotbar keys while menu is open
				modPlayer.keepSelected = player.selectedItem;
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brewing for Dummies");
			Tooltip.SetDefault("Allows mixing alchemical weapons with the hotbar keys"
							+ "\nPress the corresponding key to mix a specific item"
							+ "\nUse the mouse wheel to change the displayed element"
							+ "\nUp to 10 items can be displayed at once");
		}

	}
}
