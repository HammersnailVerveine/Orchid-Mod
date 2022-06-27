using Microsoft.Xna.Framework;
using OrchidMod.Common;
using OrchidMod.Common.Attributes;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Misc
{
	[ClassTag(ClassTags.Alchemist)]
	public class UIItemKeys : OrchidModItem
	{
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 34;
			Item.maxStack = 1;
			Item.value = Item.sellPrice(0, 0, 2, 0);
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = 1;
			Item.noUseGraphic = true;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item7;
			Item.shoot = ProjectileType<Alchemist.Projectiles.AlchemistRightClick>();
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
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

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			if (!modPlayer.alchemistSelectUIKeysDisplay)
			{
				modPlayer.alchemistSelectUIKeysDisplay = true;
				modPlayer.alchemistSelectUIKeysInitialize = true;
			}
			return true;
		}

		/*
		public override bool UseItem(Player player) {
			OrchidPlayer modPlayer = player.GetModPlayer<OrchidPlayer>();
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
				tooltips.Insert(1, new TooltipLine(Mod, "ClassTag", "-Alchemist Class-")
				{
					OverrideColor = new Color(155, 255, 55)
				});
			}
		}

		public override void HoldItem(Player player)
		{
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			modPlayer.alchemistSelectUIKeysItem = true;

			if (modPlayer.alchemistSelectUIKeysDisplay)
			{ // Prevents the game from switching items when inputing hotbar keys while menu is open
				modPlayer.modPlayer.keepSelected = player.selectedItem;
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
