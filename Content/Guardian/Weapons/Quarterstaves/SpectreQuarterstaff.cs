using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian.Projectiles.Quarterstaves;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Quarterstaves
{
	public class SpectreQuarterstaff : OrchidModGuardianQuarterstaff
	{
		private int Timer = 0;

		public override void SafeSetDefaults()
		{
			Item.width = 60;
			Item.height = 66;
			Item.value = Item.sellPrice(0, 4, 29, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.useTime = 35;
			ParryDuration = 100;
			Item.knockBack = 8f;
			Item.damage = 221;
			GuardStacks = 1;
			SlamStacks = 2;
			CounterHits = 5;
			CounterDamage = 0.6f;
		}

		public override void SafeHoldItem(Player player)
		{
			Timer++;
			if (Timer > 60)
			{
				Timer = 0;
				int projectileType = ModContent.ProjectileType<SpectreQuarterstaffProj>();
				Vector2 position = player.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * Main.rand.NextFloat(32f, 160f);
				Projectile.NewProjectile(player.GetSource_ItemUse(Item), position, Vector2.Zero, projectileType, 0, 0, player.whoAmI);
			}
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddIngredient(ItemID.SpectreBar, 18);
			recipe.Register();
		}
	}
}
