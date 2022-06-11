using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Interfaces;
using OrchidMod.Common.PlayerDrawLayers;
using OrchidMod.Shaman.Misc;
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
		public override string Texture => OrchidAssets.AbyssSetItemsPath + Name;

		public AbyssHamaxe() : base(name: "Abyss Hamaxe", lightColor: new Color(69, 66, 237), itemCloneType: ItemID.LunarHamaxeSolar) { }

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddIngredient(ModContent.ItemType<AbyssFragment>(), 14);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}

	public class AbyssPickaxe : LuminiteTool
	{
		public override string Texture => OrchidAssets.AbyssSetItemsPath + Name;

		public AbyssPickaxe() : base(name: "Abyss Pickaxe", lightColor: new Color(69, 66, 237), itemCloneType: ItemID.SolarFlarePickaxe) { }

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(ModContent.ItemType<AbyssFragment>(), 12);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}

	// ...

	public abstract class LuminiteTool : ModItem
	{
		private readonly Color lightColor;
		private readonly int itemCloneType;
		private readonly string name;

		// ...

		public LuminiteTool(string name, Color lightColor, int itemCloneType)
		{
			this.lightColor = lightColor;
			this.itemCloneType = itemCloneType;
			this.name = name;
		}

		public override string Texture => OrchidAssets.ItemsPath + Name;

		// ...

		public sealed override void SetStaticDefaults()
		{
			HeldItemLayer.RegisterDrawMethod(Type, DrawUtils.DrawSimpleItemGlowmaskOnPlayer);

			DisplayName.SetDefault(name);
		}

		public sealed override void SetDefaults()
		{
			Item.CloneDefaults(itemCloneType);
		}

		public sealed override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			Lighting.AddLight(player.itemLocation, lightColor.ToVector3() * 0.2f);
		}

		public sealed override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, lightColor.ToVector3() * 0.2f);
		}

		public sealed override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			spriteBatch.DrawSimpleItemGlowmaskInWorld(Item, Color.White, rotation, scale);
		}
	}
}
