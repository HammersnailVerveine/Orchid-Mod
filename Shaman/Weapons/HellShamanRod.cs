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
			item.damage = 32;
			item.width = 42;
			item.height = 42;
			item.useTime = 20;
			item.useAnimation = 20;
			item.knockBack = 0f;
			item.rare = ItemRarityID.Orange;
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.UseSound = SoundID.Item65;
			item.shootSpeed = 8f;
			item.shoot = ModContent.ProjectileType<Projectiles.HellShamanRodProj>();
			empowermentType = 4;
			this.energy = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Depths Weaver Rod");
			Tooltip.SetDefault("Shoots lingering fire leaves" +
							   "\nOnly one set can be active at once" +
							   "\nHaving 2 or more active shamanic bonds increases damage and slows on hit");
		}

		public override void SafeModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			if (OrchidModShamanHelper.getNbShamanicBonds(player, player.GetModPlayer<OrchidModPlayer>(), mod) > 1) mult *= modPlayer.shamanDamage * 2f;
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			ShamanRod.RemoveAllShamanRodProjs(player);

			for (int i = 0; i < 3; i++)
				this.newShamanProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, ai1: i + 1);

			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ModContent.ItemType<ShamanRod>(), 1);
			recipe.AddIngredient(ItemID.HellstoneBar, 12);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
