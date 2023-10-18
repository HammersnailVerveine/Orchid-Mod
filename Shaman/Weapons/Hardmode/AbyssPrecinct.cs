using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.PlayerDrawLayers;
using OrchidMod.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class AbyssPrecinct : OrchidModShamanItem
	{
		public override void SafeSetStaticDefaults()
		{
			HeldItemLayer.RegisterDrawMethod(Type, OrchidUtils.DrawSimpleItemGlowmaskOnPlayer);

			// DisplayName.SetDefault("Abyss Precinct");
			/* Tooltip.SetDefault("Shoots an abyssal vortex, pulsating with energy"
								+ "\nHitting an enemy grants you an abyss fragment"
								+ "\nIf you have 5 abyss fragments, your next hit will increase shamanic damage by 20% for 30 seconds"); */
		}

		public override void SafeSetDefaults()
		{
			Item.damage = 120;
			Item.width = 38;
			Item.height = 38;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.knockBack = 6.15f;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.UseSound = SoundID.Item122;
			Item.autoReuse = true;
			//Item.shoot = ModContent.ProjectileType<Projectiles.OreOrbs.Big.AbyssPrecinctProj>();
			Item.shootSpeed = 10f;

			this.Element = 2;
			this.energy = 10;
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int numberProjectiles = 3;
			Vector2 newPosition = position - new Vector2(4, 4);
			for (int i = 0; i < numberProjectiles; i++)
				this.NewShamanProjectile(player, source, newPosition, velocity, type, damage, knockback);
			return false;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ModContent.ItemType<Misc.AbyssFragment>(), 18)
			.AddTile(TileID.LunarCraftingStation)
			.Register();

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			spriteBatch.DrawSimpleItemGlowmaskInWorld(Item, Color.White, rotation, scale);
		}
	}
}
