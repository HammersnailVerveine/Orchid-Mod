using Microsoft.Xna.Framework;
using OrchidMod.Common.Attributes;
using OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Big;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium
{
	[CrossmodContent("ThoriumMod")]
	public class Monowai : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 30;
			Item.width = 44;
			Item.height = 44;
			Item.useTime = 38;
			Item.useAnimation = 38;
			Item.knockBack = 4.75f;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.UseSound = SoundID.Item21;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = ModContent.ProjectileType<MonowaiProj>();
			this.Element = 4;
			this.energy = 8;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Monowai"); // Named after an undersea volcano
			/* Tooltip.SetDefault("Shoots elemental bolts, hitting your enemy 2 times"
							+ "\nHitting the same target twice will grant you a volcanic orb"
							+ "\nIf you have 5 orbs, your next hit will explode, throwing enemies in the air"
							+ "\nAttacks might singe the target, causing extra damage"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 2; i++)
				this.NewShamanProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
		}

		//TODO Thorium: Monitor, aDarksteelAlloy will get renamed to DarksteelAlloy sometime in the future
		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.Anvils);
				recipe.AddIngredient(ModContent.ItemType<MagmaScepter>(), 1);
				recipe.AddIngredient(ModContent.ItemType<AquaiteScepter>(), 1);
				recipe.AddIngredient(thoriumMod, "aDarksteelAlloy", 10);
				recipe.Register();
			}
		}
	}
}
