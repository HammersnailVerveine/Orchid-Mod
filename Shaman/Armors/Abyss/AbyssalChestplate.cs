using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Armors.Abyss
{
	[AutoloadEquip(EquipType.Body)]
    public class AbyssalChestplate : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 34;
            item.height = 22;
            item.value = 0;
            item.rare = 10;
            item.defense = 32;
        }		

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Abyssal Chestplate");
		  Tooltip.SetDefault("9% increased shamanic damage and critical strike chance");
		}


		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            color = drawPlayer.GetImmuneAlphaPure(Color.White, shadow);
        }
		
        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
            modPlayer.shamanCrit += 9;
			modPlayer.shamanDamage += 0.09f;
			Lighting.AddLight(player.position, 0.15f, 0.15f, 0.8f);		
        }
		
		public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
            player.armorEffectDrawShadowSubtle = true;
        }
		
		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 16);
			recipe.AddIngredient(null, "AbyssFragment", 20);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
