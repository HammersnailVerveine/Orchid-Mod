using Microsoft.Xna.Framework;
using OrchidMod.Common.Attributes;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium
{
	[CrossmodContent("ThoriumMod")]
	public class AquaiteScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 20;
			Item.width = 38;
			Item.height = 38;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.knockBack = 4.75f;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.UseSound = SoundID.Item21;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			//Item.shoot = ModContent.ProjectileType<AquaiteScepterProj>();
			this.Element = 4;
			this.energy = 8;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Aquaite Scepter");
			/* Tooltip.SetDefault("Shoots a water bolt, hitting your enemy twice"
							+ "\nHitting the same target twice will grant you a water crystal"
							+ "\nIf you have 5 crystals, your next hit will summon a powerful geyser"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 2; i++)
				this.NewShamanProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.Anvils);
				recipe.AddIngredient(thoriumMod, "AquaiteBar", 14);
				recipe.AddIngredient(thoriumMod, "DepthScale", 6);
				recipe.Register();
			}
		}
	}
}
