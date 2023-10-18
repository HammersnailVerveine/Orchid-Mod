using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.UI;
using OrchidMod.Common;
using OrchidMod.Common.Attributes;
using OrchidMod.Common.UIs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Misc
{
	[ClassTag(ClassTags.Alchemist)]
	public class UIItemNew : ModItem
	{
		public override string Texture => "OrchidMod/Alchemist/Misc/UIItem";

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 34;
			Item.maxStack = 1;
			Item.value = Item.sellPrice(0, 0, 2, 0);
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noUseGraphic = true;
			Item.rare = ItemRarityID.Expert;
			Item.UseSound = SoundID.Item7;
			Item.shoot = ProjectileType<Alchemist.Projectiles.AlchemistRightClick>();
		}

		public override Color? GetAlpha(Color lightColor)
			=> Main.DiscoColor;

		public override bool AltFunctionUse(Player player)
			=> true;

		/*public override bool CanUseItem(Player player)
		{
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			if (player.altFunctionUse == 2)
			{
				return !modPlayer.alchemistSelectUIDisplay && Main.mouseRightRelease;
			} // else {
			  // return !modPlayer.alchemistSelectUIDisplay && Main.mouseLeftRelease;
			  // }
			return base.CanUseItem(player);
		}*/

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (Main.LocalPlayer.whoAmI.Equals(player.whoAmI))
			{
				if (Environment.UserName.Equals("SPladison"))
				{
					if (player.altFunctionUse == 2)
					{
						var uiState = UISystem.GetUIState<AlchemistSelectUI>();

						if (!uiState.Visible) uiState.Visible = true;

						player.itemAnimation = 0;
					}
					else
					{
						var alchemist = player.GetModPlayer<OrchidAlchemist>();

						if (alchemist.alchemistNbElements > 0)
						{
							alchemist.alchemistShootProjectile = true;
							SoundEngine.PlaySound(SoundID.Item106);
						}
					}
				}
				else
				{
					Main.NewText("[E] You're not S-Pladison! The interface is not available...");
				}
			}

			/*OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();

			if (!modPlayer.alchemistSelectUIDisplay)
			{
				modPlayer.alchemistSelectUIDisplay = true;
				modPlayer.alchemistSelectUIInitialize = true;
			}*/

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

		public override void HoldItem(Player player)
		{
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			modPlayer.alchemistSelectUIItem = true;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("The Alchemist's Cookbook [S-Pladison's Edition]");
			/* Tooltip.SetDefault("Allows mixing alchemical weapons by clicking"
							+ "\nRight click on an item icon to mix it"
							+ "\nLeft click to launch the attack"
							+ "\nUp to 17 items can be displayed at once"); */
		}

	}
}