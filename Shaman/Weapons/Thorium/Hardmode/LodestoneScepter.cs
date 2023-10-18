using Microsoft.Xna.Framework;
using OrchidMod.Common.Attributes;
using OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Big;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	[CrossmodContent("ThoriumMod")]
	public class LodestoneScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 51;
			Item.width = 54;
			Item.height = 54;
			Item.useTime = 50;
			Item.useAnimation = 50;
			Item.knockBack = 4.15f;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(0, 2, 70, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = ModContent.ProjectileType<LodestoneScepterProj>();
			this.Element = 4;
			this.energy = 12;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Lodestone Scepter");
			/* Tooltip.SetDefault("Shoots a defense sundering bolt, hitting your enemy 3 times"
							+ "\nHitting the same target with all 3 shots will grant you a lodestone orb"
							+ "\nIf you have 5 orbs, your next hit will create an explosion, making hit foes significantly heavier"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 3; i++)
				this.NewShamanProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(thoriumMod, "LodeStoneIngot", 8);
				recipe.Register();
			}
		}
	}
}
