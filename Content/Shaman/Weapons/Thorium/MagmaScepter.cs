using Microsoft.Xna.Framework;
using OrchidMod.Common.Attributes;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Thorium
{
	[CrossmodContent("ThoriumMod")]
	public class MagmaScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 15;
			Item.width = 46;
			Item.height = 46;
			Item.useTime = 48;
			Item.useAnimation = 48;
			Item.knockBack = 4.35f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 25, 0);
			Item.UseSound = SoundID.Item21;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			//Item.shoot = ModContent.ProjectileType<MagmaScepterProj>();
			this.Element = 4;
			this.energy = 7;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Magma Scepter");
			/* Tooltip.SetDefault("Shoots a magma bolt, hitting your enemy 2 times"
							+ "\nHitting the same target twice will grant you a magma orb"
							+ "\nIf you have 5 orbs, your next hit will explode"
							+ "\nAttacks might singe the target, causing extra damage"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 2; i++)
				this.NewShamanProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
		}

		//TODO Thorium: MagmaCore removed, alongside other magma content, including associated projectiles and buffs
		/*
		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.Anvils);
				recipe.AddIngredient(thoriumMod, "MagmaCore", 8);
				recipe.Register();
			}
		}
		*/
	}
}
