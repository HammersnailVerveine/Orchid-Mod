using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.General.Items.Tools
{
	public class AbyssHamaxe : ModItem, IGlowingItem
	{
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.LunarHamaxeSolar);
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyss Hamaxe");
		}

		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddIngredient(ModContent.ItemType<Shaman.Misc.AbyssFragment>(), 14);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			OrchidHelper.DrawSimpleItemGlowmaskInWorld(item, spriteBatch, ModContent.GetTexture("OrchidMod/Glowmasks/AbyssHamaxe_Glowmask"), Color.White, rotation, scale);
		}

		public void DrawItemGlowmask(PlayerDrawInfo drawInfo)
		{
			OrchidHelper.DrawSimpleItemGlowmaskOnPlayer(drawInfo, ModContent.GetTexture("OrchidMod/Glowmasks/AbyssHamaxe_Glowmask"));
		}
	}
}
