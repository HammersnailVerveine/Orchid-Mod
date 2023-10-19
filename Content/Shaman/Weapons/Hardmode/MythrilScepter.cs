using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Hardmode
{
	public class MythrilScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 41;
			Item.width = 46;
			Item.height = 46;
			Item.useTime = 50;
			Item.useAnimation = 50;
			Item.knockBack = 4.15f;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 2, 2, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			//Item.shoot = ModContent.ProjectileType<MythrilScepterProj>();
			this.Element = ShamanElement.EARTH;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Mythril Scepter");
			/* Tooltip.SetDefault("Shoots a potent mythril bolt, hitting your enemy 3 times"
							  + "\nHitting the same target with all 3 shots will grant you a mythril orb"
							  + "\nIf you have 5 mythril orbs, your next hit will increase your defense by 8 for 30 seconds"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 3; i++)
				this.NewShamanProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.MythrilBar, 10)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
