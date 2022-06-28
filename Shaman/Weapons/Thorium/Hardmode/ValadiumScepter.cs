using Microsoft.Xna.Framework;
using OrchidMod.Common.Attributes;
using OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Big;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	[CrossmodContent("ThoriumMod")]
	public class ValadiumScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 45;
			Item.width = 54;
			Item.height = 54;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.knockBack = 0f;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 13f;
			Item.shoot = ModContent.ProjectileType<ValadiumScepterProj>();
			this.empowermentType = 4;
			this.energy = 12;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Valadium Scepter");
			Tooltip.SetDefault("Shoots a potent valadium bolt, hitting your enemy 3 times"
							+ "\nHitting the same target with all 3 shots will grant you a valadium orb"
							+ "\nIf you have 5 orbs, your next hit will deal massive damage and send your target flying"
							+ "\nAttacks will blast enemies with inter-dimensional energy briefly");
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 3; i++)
			{
				int newDamage = player.GetModPlayer<OrchidShaman>().orbCountBig >= 15 ? damage * 2 : damage;
				this.NewShamanProjectile(player, source, position, velocity, type, newDamage, knockback);

			}
			return false;
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(thoriumMod, "ValadiumIngot", 8);
				recipe.Register();
			}
		}
	}
}
