using Microsoft.Xna.Framework;
using OrchidMod.Content.Shaman.Misc;
using OrchidMod.Content.Shaman.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons
{
	public class FeatherScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 13;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 33;
			Item.useAnimation = 33;
			Item.knockBack = 0f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			//Item.shoot = ModContent.ProjectileType<FeatherScepterProj>();
			this.Element = 3;
			this.energy = 6;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Feather Scepter");
			/* Tooltip.SetDefault("Shoots dangerous spinning feathers"
							  + "\nThe projectiles gain in damage after a while"
							  + "\nHaving 3 or more active shamanic bonds will result in more projectiles shot"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int nbBonds = modPlayer.GetNbShamanicBonds();

			if (nbBonds > 2)
			{
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15)) / 2f;
				this.NewShamanProjectile(player, source, position, newVelocity, type, damage, knockback);
			}
			return true;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ModContent.ItemType<HarpyTalon>(), 2)
			.AddIngredient(ItemID.Feather, 5)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
