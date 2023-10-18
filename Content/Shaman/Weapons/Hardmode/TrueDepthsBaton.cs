using Microsoft.Xna.Framework;
using OrchidMod.Content.Shaman.Misc;
using OrchidMod.Content.Shaman.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Shaman.Weapons.Hardmode
{
	public class TrueDepthsBaton : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 62;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 42;
			Item.useAnimation = 42;
			Item.knockBack = 1.15f;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			//Item.shoot = ModContent.ProjectileType<TrueDepthProj>();
			this.Element = 5;
			this.energy = 14;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("True Depths Baton");
			/* Tooltip.SetDefault("Shoots 3 bolts of dark energy"
							  + "\nThe number of projectiles shot scales with the number of active shamanic bonds"
							  + "\nHitting at maximum range deals increased damage"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int nbBonds = modPlayer.GetNbShamanicBonds();

			int nbProjectiles = 1;
			while (nbBonds > 0)
			{
				nbBonds -= 2;
				nbProjectiles++;
			}

			for (int i = 0; i < nbProjectiles; i++)
			{
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(4));
				this.NewShamanProjectile(player, source, position, newVelocity, type, damage, knockback);
			}
			return false;
		}

		public override void AddRecipes()
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;

			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<DepthsBaton>(), 1);
			recipe.AddIngredient((thoriumMod != null) ? thoriumMod.Find<ModItem>("BrokenHeroFragment").Type : ItemType<BrokenHeroScepter>(), (thoriumMod != null) ? 2 : 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
