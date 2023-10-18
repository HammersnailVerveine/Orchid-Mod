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
	public class TitanicScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 53;
			Item.width = 54;
			Item.height = 54;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.knockBack = 4.65f;
			Item.rare = ItemRarityID.LightPurple;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = ModContent.ProjectileType<TitanicScepterProj>();
			this.Element = 4;
			this.energy = 12;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Titan Scepter");
			/* Tooltip.SetDefault("Shoots a potent titanic energy bolt, hitting your enemy 3 times"
							+ "\nHitting the same target with all 3 shots will grant you an titan orb"
							+ "\nIf you have 5 orbs, your next hit will boost your critical strikes abilities for a while"
							+ "\nCritical strikes will deal additional damage"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 3; i++)
				this.NewShamanProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(thoriumMod.Find<ModTile>("SoulForge").Type);
				recipe.AddIngredient(thoriumMod, "TitanBar", 8);
				recipe.Register();
			}
		}
	}
}
