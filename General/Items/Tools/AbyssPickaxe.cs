using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.General.Items.Tools
{
	public class AbyssPickaxe : ModItem, IGlowingItem
	{
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.SolarFlarePickaxe);
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyss Pickaxe");
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(ModContent.ItemType<Shaman.Misc.AbyssFragment>(), 12);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			OrchidHelper.DrawSimpleItemGlowmaskInWorld(item, spriteBatch, ModContent.GetTexture("OrchidMod/Glowmasks/AbyssPickaxe_Glowmask"), Color.White, rotation, scale);
		}

		public void DrawItemGlowmask(PlayerDrawInfo drawInfo)
		{
			OrchidHelper.DrawSimpleItemGlowmaskOnPlayer(drawInfo, ModContent.GetTexture("OrchidMod/Glowmasks/AbyssPickaxe_Glowmask"));
		}
	}
}