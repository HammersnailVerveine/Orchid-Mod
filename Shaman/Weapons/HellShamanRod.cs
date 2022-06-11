using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class HellShamanRod : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 32;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.knockBack = 0f;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.UseSound = SoundID.Item65;
			Item.shootSpeed = 8f;
			Item.shoot = ModContent.ProjectileType<Projectiles.HellShamanRodProj>();
			empowermentType = 4;
			this.energy = 35;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Depths Weaver Rod");
			Tooltip.SetDefault("Shoots lingering fire leaves" +
							   "\nOnly one set can be active at once" +
							   "\nHaving 2 or more active shamanic bonds increases damage and slows on hit");
		}

		public override void SafeModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			if (OrchidModShamanHelper.getNbShamanicBonds(player, player.GetModPlayer<OrchidModPlayer>(), Mod) > 1) mult *= modPlayer.shamanDamage * 2f;
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			ShamanRod.RemoveAllShamanRodProjs(player);

			for (int i = 0; i < 3; i++)
				this.NewShamanProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, ai1: i + 1);

			return false;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ModContent.ItemType<ShamanRod>(), 1);
			recipe.AddIngredient(ItemID.HellstoneBar, 12);
			recipe.Register();
			recipe.AddRecipe();
		}
	}
}
