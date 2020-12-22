using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Armors.Abyss
{
	[AutoloadEquip(EquipType.Legs)]
    public class AbyssalGreaves : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 26;
            item.height = 18;
            item.value = 0;
            item.rare = 10;
            item.defense = 20;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Abyssal Greaves");
		  Tooltip.SetDefault("10% increased shamanic damage"
							+"\n10% increased movement speed");
		}

		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
		public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
            player.armorEffectDrawShadowSubtle = true;
        }
		
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            color = drawPlayer.GetImmuneAlphaPure(Color.White, shadow);
        }
		
        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
            player.moveSpeed += 0.1f;
			modPlayer.shamanDamage += 0.1f;
			Lighting.AddLight(player.position, 0.15f, 0.15f, 0.8f);		
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddIngredient(null, "AbyssFragment", 15);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
