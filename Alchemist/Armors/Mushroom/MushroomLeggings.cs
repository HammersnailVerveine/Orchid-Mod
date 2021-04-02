using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Interfaces;

namespace OrchidMod.Alchemist.Armors.Mushroom
{
	[AutoloadEquip(EquipType.Legs)]
    public class MushroomLeggings : OrchidModAlchemistEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 4, 0);
            item.rare = ItemRarityID.Blue;
            item.defense = 3;
        }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phosphorescent Leggings");
			Tooltip.SetDefault("5% increased potency regeneration");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistRegenPotency -= 3;
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 6);
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
			OrchidHelper.DrawSimpleItemGlowmaskInWorld(item, spriteBatch, ModContent.GetTexture("OrchidMod/Glowmasks/MushroomLeggings_Glowmask"), new Color(250, 250, 250, 200) * OrchidWorld.alchemistMushroomArmorProgress, rotation, scale);
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			Color color = new Color(63, 67, 207) * 0.2f * OrchidWorld.alchemistMushroomArmorProgress;
			Lighting.AddLight(player.Center, color.R / 255f, color.G / 255f, color.B / 255f);
		}
	}
}
