using Microsoft.Xna.Framework;
using OrchidMod.Shaman.Projectiles.OreOrbs.Big;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class PalladiumScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 33;
			Item.width = 46;
			Item.height = 46;
			Item.useTime = 50;
			Item.useAnimation = 50;
			Item.knockBack = 4.15f;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 1, 76, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = ModContent.ProjectileType<PalladiumScepterProj>();
			this.Element = 4;
			this.energy = 12;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Palladium Scepter");
			/* Tooltip.SetDefault("Shoots a potent palladium bolt, hitting your enemy 3 times"
							  + "\nHitting the same target with all 3 shots will grant you a palladium orb"
							  + "\nIf you have 5 palladium orbs, your next attack will replenish 25 life on hit"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 3; i++)
				this.NewShamanProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.PalladiumBar, 10)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
