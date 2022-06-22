using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Interfaces;
using OrchidMod.Common.PlayerDrawLayers;
using OrchidMod.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class AbyssShredder : OrchidModShamanItem
	{
		//public override string Texture => OrchidAssets.AbyssSetPath + Name; [SP]

		public override void SafeSetDefaults()
		{
			Item.damage = 110;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.knockBack = 1.15f;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.UseSound = SoundID.Item122;
			Item.autoReuse = true;
			Item.shootSpeed = 10f;
			Item.shoot = ModContent.ProjectileType<Projectiles.AbyssShardS>();
			this.empowermentType = 1;

			OrchidModGlobalItem orchidItem = Item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeaponNoUsetimeReforge = true;
			this.energy = 5;
		}

		public override void SafeSetStaticDefaults()
		{
			HeldItemLayer.RegisterDrawMethod(Type, DrawUtils.DrawSimpleItemGlowmaskOnPlayer);

			DisplayName.SetDefault("Abyss Stormcaller");
			Tooltip.SetDefault("Shoots abyss energy thunderbolts"
								+ "\nIncreases weapon speed for each active shamanic bond");
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// What the fuck is this
			Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));
			this.NewShamanProjectile(player, source, position, newVelocity, ModContent.ProjectileType<Projectiles.AbyssShard>(), damage, knockback);
			this.NewShamanProjectile(player, source, position, newVelocity, ModContent.ProjectileType<Projectiles.AbyssShardS>(), damage, knockback);
			this.NewShamanProjectile(player, source, position, velocity, ModContent.ProjectileType<Projectiles.AbyssShardD>(), damage, knockback);
			return false;
		}

		public override void UpdateInventory(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);

			Item.useTime = 18 - (2 * nbBonds);
			Item.useAnimation = 18 - (2 * nbBonds);
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.Blue.ToVector3() * 0.55f * Main.essScale);
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
