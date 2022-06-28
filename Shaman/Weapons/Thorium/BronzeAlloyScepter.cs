using Microsoft.Xna.Framework;
using OrchidMod.Common.Attributes;
using OrchidMod.Shaman.Projectiles.Thorium;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium
{
	[CrossmodContent("ThoriumMod")]
	public class BronzeAlloyScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 23;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.knockBack = 2.75f;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 14f;
			Item.shoot = ModContent.ProjectileType<BronzeAlloyScepterProj>();
			this.empowermentType = 4;
			this.energy = 5;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Basilisk's Fang");
			Tooltip.SetDefault("Fires out a poisonous basilisk fang"
							+ "\nThe number of active shamanic bonds will increase the poison duration"
							+ "\nThe weapon itself can critically strike, releasing a petrifying projectile"
							+ "\nThe more shamanic bonds you have, the higher the chances of critical strike");
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int nbBonds = modPlayer.GetNbShamanicBonds();

			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(3));

			if (Main.rand.Next(101) < 5 + nbBonds * 5)
			{
				type = ModContent.ProjectileType<BronzeAlloyScepterProjAlt>();
				damage *= 2;
				velocity *= 1.2f;
			}

			this.NewShamanProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.Anvils);
				recipe.AddIngredient(thoriumMod, "BronzeFragments", 10);
				recipe.Register();
			}
		}
	}
}

