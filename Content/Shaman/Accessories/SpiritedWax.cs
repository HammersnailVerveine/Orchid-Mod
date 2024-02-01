using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Accessories
{
	public class SpiritedWax : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 55, 0);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}

		public override void OnReleaseShamanicBond(Player player, OrchidShaman shaman, ShamanElement element, Projectile catalyst)
		{
			if (element == ShamanElement.FIRE || element == ShamanElement.WATER)
			{
				shaman.modPlayer.TryHeal(30);

				int dmg = (int)player.GetDamage<ShamanDamageClass>().ApplyTo(10);
				EntitySource_ItemUse source = (EntitySource_ItemUse)player.GetSource_ItemUse(Item);
				for (int i = 0; i < 15; i++)
				{
					if (player.strongBees && Main.rand.NextBool(2))
						Projectile.NewProjectile(source, catalyst.Center, Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * 5f, ProjectileID.GiantBee, (int)(dmg * 1.15f), 0f, player.whoAmI);
					else
						Projectile.NewProjectile(source, catalyst.Center, Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * 5f, ProjectileID.Bee, dmg, 0f, player.whoAmI);
				}
			}
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<SpiritedWater>(), 1);
			recipe.AddIngredient(ModContent.ItemType<WaxyVial>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}
