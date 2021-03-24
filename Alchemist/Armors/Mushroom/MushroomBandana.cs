using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Interfaces;

namespace OrchidMod.Alchemist.Armors.Mushroom
{
	[AutoloadEquip(EquipType.Head)]
    public class MushroomBandana : OrchidModAlchemistEquipable, IDrawOnPlayer
    {
        public override void SafeSetDefaults()
        {
            item.width = 24;
            item.height = 12;
            item.value = Item.sellPrice(0, 0, 3, 0);
            item.rare = ItemRarityID.Blue;
            item.defense = 2;
        }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phosphorescent Bandana");
			Tooltip.SetDefault("5% increased potency regeneration");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistRegenPotency -= 3;
        }
		
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("MushroomTunic") && legs.type == mod.ItemType("MushroomLeggings");
        }
		
        public override void UpdateArmorSet(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
            player.setBonus = "Maximum number of simultaneous alchemical elements increased by 1";
			modPlayer.alchemistNbElementsMax += 1;
        }

		public override bool DrawHead() => true;
		
		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
			drawAltHair = false;
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 4);
			recipe.AddIngredient(ItemID.GlowingMushroom, 5);
			recipe.AddIngredient(null, "MushroomThread", 1);
			recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			Color color = new Color(63, 67, 207) * 0.2f * OrchidWorld.alchemistMushroomArmorProgress;
			Lighting.AddLight(item.Center, color.R / 255f, color.G / 255f, color.B / 255f);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			OrchidHelper.DrawSimpleItemGlowmaskInWorld(item, spriteBatch, ModContent.GetTexture("OrchidMod/Glowmasks/MushroomBandana_Glowmask"), new Color(250, 250, 250, 200) * OrchidWorld.alchemistMushroomArmorProgress, rotation, scale);
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			Color color = new Color(63, 67, 207) * 0.2f * OrchidWorld.alchemistMushroomArmorProgress;
			Lighting.AddLight(player.Center, color.R / 255f, color.G / 255f, color.B / 255f);
		}

		public void DrawOnPlayer(PlayerDrawInfo drawInfo)
		{
			OrchidHelper.DrawSimplePlayerGlowmask(EquipType.Head, drawInfo, ModContent.GetTexture("OrchidMod/Glowmasks/MushroomBandana_Head_Glowmask"), new Color(250, 250, 250, 200) * OrchidWorld.alchemistMushroomArmorProgress);
		}
	}
}
