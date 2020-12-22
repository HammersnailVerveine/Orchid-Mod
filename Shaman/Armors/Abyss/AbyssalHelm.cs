using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Armors.Abyss
{
	[AutoloadEquip(EquipType.Head)]
    public class AbyssalHelm : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.value = 0;
            item.rare = 10;
            item.defense = 18;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Abyssal Helm");
		  Tooltip.SetDefault("Your shamanic bonds will last 5 seconds longer"
							+"\n7% increased shamanic damage and critical strike chance");
		}


		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
        public override void UpdateEquip(Player player)
        {
			Main.player[Main.myPlayer].GetModPlayer<OrchidModPlayer>().shamanBuffTimer += 5;
            Main.player[Main.myPlayer].GetModPlayer<OrchidModPlayer>().shamanCrit += 7;
			Main.player[Main.myPlayer].GetModPlayer<OrchidModPlayer>().shamanDamage += 0.07f;
			Lighting.AddLight(player.position, 0.15f, 0.15f, 0.8f);			
        }
		
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            color = drawPlayer.GetImmuneAlphaPure(Color.White, shadow);
        }
		
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("AbyssalChestplate") && legs.type == mod.ItemType("AbyssalGreaves");
        }
		
		public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
            player.armorEffectDrawShadowSubtle = true;
        }
		
        public override void UpdateArmorSet(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			player.armorEffectDrawShadow = true;
			String dir = Main.ReversedUpDownArmorSetBonuses ? "DOWN" : "UP";
			player.setBonus = "All shamanic bonds are more effective"
							+ "\n             Double tap " + dir + " to summon an abyss portal"
							+ "\n             Portal damage grants an air V shamanic bond";

			modPlayer.shamanFireBonus += 1;
			modPlayer.shamanWaterBonus += 1;
			modPlayer.shamanAirBonus += 1;
			modPlayer.shamanEarthBonus += 1;
			modPlayer.shamanSpiritBonus += 1;
			modPlayer.abyssSet = true;
        }
		
        public override bool DrawHead()
        {
            return true;
        }
		
		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = drawAltHair = false;
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 8);
			recipe.AddIngredient(null, "AbyssFragment", 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
