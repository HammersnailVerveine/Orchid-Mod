using Microsoft.Xna.Framework;
using OrchidMod.Shaman.Projectiles.OreOrbs.Big;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class OrichalcumScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 43;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 50;
			Item.useAnimation = 50;
			Item.knockBack = 4.15f;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 2, 41, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = ModContent.ProjectileType<OrichalcumScepterProj>();
			this.empowermentType = 4;
			this.energy = 12;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Orichalcum Scepter");
			Tooltip.SetDefault("Shoots a potent orichalcum bolt, hitting your enemy 3 times"
							  + "\nHitting the same target with all 3 shots will grant you an orichalcum orb"
							  + "\nIf you have 5 orichalcum orbs, your next hit will release a burst of damaging petals");
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 3; i++)
				this.NewShamanProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
		}
		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.OrichalcumBar, 10)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
