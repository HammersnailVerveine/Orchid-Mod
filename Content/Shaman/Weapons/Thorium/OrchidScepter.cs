using Microsoft.Xna.Framework;
using OrchidMod.Common.Attributes;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Thorium
{
	[CrossmodContent("ThoriumMod")]
	public class OrchidScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 18;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.knockBack = 4f;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.shootSpeed = 13f;
			//Item.shoot = Mod.Find<ModProjectile>("OrchidScepterProj").Type;
			this.Element = ShamanElement.EARTH;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Orchid Scepter");
			/* Tooltip.SetDefault("Shoots a volley of piercing petals"
							+ "\nThe number of petals increase with active shamanic bonds"
							+ "\nHaving 3 or more bonds will allow the petals to pierce more enemies"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int nbBonds = modPlayer.CountShamanicBonds();
			int numberProjectiles = 1 + Main.rand.Next(2);
			numberProjectiles += nbBonds > 1 ? nbBonds > 3 ? 2 : 1 : 0;

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(4));
				this.NewShamanProjectile(player, source, position, newVelocity, type, damage, knockback);
			}
			return false;
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(thoriumMod.Find<ModTile>("ArcaneArmorFabricator").Type);
				recipe.AddIngredient(thoriumMod, "Petal", 8);
				recipe.Register();
			}
		}
	}
}

