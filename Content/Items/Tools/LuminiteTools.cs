using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Interfaces;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Items.Tools
{
	public class AbyssHamaxe : LuminiteTool
	{
		public AbyssHamaxe() : base(name: "Abyss Hamaxe", lightColor: new Color(69, 66, 237), itemCloneType: ItemID.LunarHamaxeSolar) { }

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(Mod);
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddIngredient(ModContent.ItemType<Shaman.Misc.AbyssFragment>(), 14);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class AbyssPickaxe : LuminiteTool
	{
		public AbyssPickaxe() : base(name: "Abyss Pickaxe", lightColor: new Color(69, 66, 237), itemCloneType: ItemID.SolarFlarePickaxe) { }

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(Mod);
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(ModContent.ItemType<Shaman.Misc.AbyssFragment>(), 12);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	// ...

	public abstract class LuminiteTool : OrchidItem, IGlowingItem
	{
		private readonly Color _lightColor;
		private readonly int _itemCloneType;
		private readonly string _name;

		// ...

		public LuminiteTool(string name, Color lightColor, int itemCloneType)
		{
			_lightColor = lightColor;
			_itemCloneType = itemCloneType;
			_name = name;
		}

		// ...

		public sealed override void SetStaticDefaults()
		{
			DisplayName.SetDefault(_name);
		}

		public sealed override void SetDefaults()
		{
			Item.CloneDefaults(_itemCloneType);
		}

		public sealed override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			Lighting.AddLight(player.itemLocation, _lightColor.ToVector3() * 0.2f);
		}

		public sealed override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, _lightColor.ToVector3() * 0.2f);
		}

		public sealed override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			spriteBatch.DrawSimpleItemGlowmaskInWorld(Item, ModContent.GetTexture(this.Texture + "_Glow"), Color.White, rotation, scale);
		}

		// ...

		void IGlowingItem.DrawItemGlowmask(PlayerDrawInfo drawInfo)
		{
			OrchidHelper.DrawSimpleItemGlowmaskOnPlayer(drawInfo, ModContent.GetTexture(this.Texture + "_Glow"));
		}
	}
}
