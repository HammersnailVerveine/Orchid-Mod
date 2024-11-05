using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Assets;
using OrchidMod.Common.PlayerDrawLayers;
using OrchidMod.Content.Shaman.Misc;
using OrchidMod.Utilities;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.General.Misc
{
	// Abyss Tools

	public class AbyssHamaxe : LuminiteTool
	{
		public override string Texture => OrchidAssets.ItemsPath + Name;

		public AbyssHamaxe() : base(lightColor: new(69, 66, 237), itemCloneType: ItemID.LunarHamaxeSolar) { }

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
		public override string Texture => OrchidAssets.ItemsPath + Name;

		public AbyssPickaxe() : base(lightColor: new(69, 66, 237), itemCloneType: ItemID.SolarFlarePickaxe) { }

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(ModContent.ItemType<AbyssFragment>(), 12);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}

	public class AbyssDrill : LuminiteTool
	{
		public override string Texture => OrchidAssets.ItemsPath + Name;

		public AbyssDrill() : base(lightColor: new(69, 66, 237), itemCloneType: ItemID.SolarFlareDrill) { }

		public override int GetProjectileType()
			=> ModContent.ProjectileType<AbyssDrillProjectile>();

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(ModContent.ItemType<AbyssFragment>(), 12);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}

		// ...

		private class AbyssDrillProjectile : LuminiteToolProjectile
		{
			public AbyssDrillProjectile() : base(ProjectileID.SolarFlareDrill) { }
			public override string Texture => OrchidAssets.ItemsPath + Name.Replace("Projectile", "");
		}
	}

	// Abstract Classes

	public abstract class LuminiteTool : ModItem
	{
		private readonly Color lightColor;
		private readonly int itemCloneType;

		// ...

		public LuminiteTool(Color lightColor, int itemCloneType)
		{
			this.lightColor = lightColor;
			this.itemCloneType = itemCloneType;
		}

		// ...

		public virtual void SafeSetDefaults() { }
		public virtual int GetProjectileType()
			=> ProjectileID.None;

		public sealed override void SetStaticDefaults()
		{
			HeldItemLayer.RegisterDrawMethod(Type, OrchidUtils.DrawSimpleItemGlowmaskOnPlayer);

			// DisplayName.SetDefault(name);
		}

		public sealed override void SetDefaults()
		{
			Item.CloneDefaults(itemCloneType);
			Item.glowMask = -1;
			Item.shoot = GetProjectileType();

			var texture = TextureAssets.Item[Type];

			if (texture is not null)
			{
				Item.width = texture.Width();
				Item.height = texture.Height();
			}

			SafeSetDefaults();
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

	public abstract class LuminiteToolProjectile : ModProjectile
	{
		private readonly int projectileCloneType;

		// ...

		public LuminiteToolProjectile(int projectileCloneType)
		{
			this.projectileCloneType = projectileCloneType;
		}

		// ...

		public override string GlowTexture => Texture + "_Glow";

		public sealed override void SetStaticDefaults()
		{
			// DisplayName.SetDefault(name);
		}

		public sealed override void SetDefaults()
		{
			Projectile.CloneDefaults(projectileCloneType);
			Projectile.glowMask = -1;

			var texture = TextureAssets.Projectile[Type];

			if (texture is not null)
			{
				Projectile.width = texture.Width();
				Projectile.height = texture.Height();
			}
		}

		public sealed override void AI()
		{
			var owner = Main.player[Projectile.owner];
			Projectile.rotation += MathHelper.PiOver2 * -owner.direction * owner.gravDir;
		}
	}
}