using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Hardmode
{
	public class ChlorophyteScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 56;
			Item.width = 50;
			Item.height = 50;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.knockBack = 4.25f;
			Item.rare = ItemRarityID.Lime;
			Item.value = Item.sellPrice(0, 5, 45, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			//Item.shoot = ModContent.ProjectileType<ChlorophyteScepterProj>();
			this.Element = 4;
			this.energy = 12;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Chlorophyte Scepter");
			/* Tooltip.SetDefault("Shoots a potent chlorophyte bolt, hitting your enemy 3 times"
							  + "\nHitting the same target with all 3 shots will grant you a leaf crystal"
							  + "\nIf you have 5 leaf crystals, your next hit will release harmful clouds of gas at your opponents"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 3; i++)
				this.NewShamanProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.ChlorophyteBar, 12)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
