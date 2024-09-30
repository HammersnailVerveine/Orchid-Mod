using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Hardmode
{
	public class CobaltScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 31;
			Item.width = 46;
			Item.height = 46;
			Item.useTime = 50;
			Item.useAnimation = 50;
			Item.knockBack = 4.15f;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 1, 35, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			//Item.shoot = ModContent.ProjectileType<CobaltScepterProj>();
			this.Element = ShamanElement.EARTH;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Cobalt Scepter");
			/* Tooltip.SetDefault("Shoots a potent cobalt bolt, hitting your enemy 3 times"
							  + "\nHitting the same target with all 3 shots will grant you a cobalt orb"
							  + "\nIf you have 5 cobalt orbs, your next attack will partially fill all active shamanic bonds"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 3; i++)
				this.NewShamanProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
		}

		/*
		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.CobaltBar, 10)
			.AddTile(TileID.Anvils)
			.Register();
		*/
	}
}
