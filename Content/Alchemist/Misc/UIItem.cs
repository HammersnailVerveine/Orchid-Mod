using Microsoft.Xna.Framework;
using OrchidMod.Common;
using OrchidMod.Common.Attributes;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Misc
{
	[ClassTag(ClassTags.Alchemist)]
	public class UIItem : ModItem
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
			Item.shoot = ProjectileType<Content.Alchemist.Projectiles.AlchemistRightClick>();
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
				return !modPlayer.alchemistSelectUIDisplay && Main.mouseRightRelease;
			} // else {
			  // return !modPlayer.alchemistSelectUIDisplay && Main.mouseLeftRelease;
			  // }
			return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			if (!modPlayer.alchemistSelectUIDisplay)
			{
				modPlayer.alchemistSelectUIDisplay = true;
				modPlayer.alchemistSelectUIInitialize = true;
			}
			return true;
		}

		/*
		public override bool UseItem(Player player) {
			OrchidPlayer modPlayer = player.GetModPlayer<OrchidPlayer>();
			if (!modPlayer.alchemistSelectUIDisplay) {
				modPlayer.alchemistSelectUIDisplay = true;
				modPlayer.alchemistSelectUIInitialize = true;
			}
			return true;
		}
		*/

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				tooltips.Insert(1, new TooltipLine(Mod, "ClassTag", Language.GetTextValue("Mods.OrchidMod.DamageClasses.Alchemist"))
				{
					OverrideColor = new Color(155, 255, 55)
				});
			}
		}

		public override void HoldItem(Player player)
		{
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			modPlayer.alchemistSelectUIItem = true;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("The Alchemist's Cookbook");
			/* Tooltip.SetDefault("Allows mixing alchemical weapons by clicking"
							+ "\nRight click on an item icon to mix it"
							+ "\nLeft click to launch the attack"
							+ "\nUp to 18 items can be displayed at once"); */
		}

	}
}
