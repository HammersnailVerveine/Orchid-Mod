using Microsoft.Xna.Framework;
using OrchidMod.Common.Attributes;
using OrchidMod.Content.Shaman.Projectiles.Thorium;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Thorium.Hardmode
{
	[CrossmodContent("ThoriumMod")]
	public class StrangePlatingScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 65;
			Item.width = 50;
			Item.height = 50;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.knockBack = 1.25f;
			Item.rare = ItemRarityID.LightPurple;
			Item.value = Item.sellPrice(0, 6, 0, 0);
			Item.UseSound = SoundID.Item12;
			Item.autoReuse = true;
			Item.shootSpeed = 20f;
			//Item.shoot = ModContent.ProjectileType<StrangePlatingScepterProj>();
			this.Element = 1;
			this.energy = 7;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Prime's Laser");
			/* Tooltip.SetDefault("The weapon itself can critically strike, releasing a powerful blast"
							+ "\nThe more shamanic bonds you have, the higher the chances of critical strike"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			/*
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int nbBonds = modPlayer.GetNbShamanicBonds();

			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(3));

			if (Main.rand.Next(101) < 4 + nbBonds * 4)
			{
				type = ModContent.ProjectileType<StrangePlatingScepterProjAlt>();
				damage *= 2;
			}

			this.NewShamanProjectile(player, source, position, velocity, type, damage, knockback);
			*/
			return false;
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(thoriumMod, "StrangePlating", 12);
				recipe.Register();
			}
		}
	}
}

