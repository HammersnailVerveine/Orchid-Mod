using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace OrchidMod.Shaman.Accessories
{
	public class SinisterPresent : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 28;
			item.height = 28;
            item.value = Item.sellPrice(0, 7, 50, 0);
			item.rare = 8;
			item.accessory = true;
		}
		
		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Sinister Present");
		  Tooltip.SetDefault("15% increased shamanic damage"
						   + "\nYour shamanic bonds will last 10 seconds longer"
						   + "\nIncreases the effectiveness of all your shamanic bonds"
						   + "\nHowever, taking direct damage will reduce their current duration by 5 seconds"
						   + "\nIt will also reverse the effectiveness upgrade and remove the damage increase for 15 seconds");
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (!(Main.LocalPlayer.FindBuffIndex(mod.BuffType("BrokenPower")) > -1))
			{
				modPlayer.shamanFireBonus ++;
				modPlayer.shamanWaterBonus ++;		
				modPlayer.shamanAirBonus ++;	
				modPlayer.shamanEarthBonus ++;	
				modPlayer.shamanSpiritBonus ++;	
				modPlayer.shamanDamage	+= 0.08f;
			}
			modPlayer.shamanSunBelt = true;
			modPlayer.shamanMourningTorch = true;
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "MourningTorch", 1);
			recipe.AddIngredient(null, "FragilePresent", 1);
            recipe.AddTile(114);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}