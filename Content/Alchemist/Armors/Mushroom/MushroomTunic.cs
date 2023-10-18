using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Alchemist.Armors.Mushroom
{
	[AutoloadEquip(EquipType.Body)]
	public class MushroomTunic : OrchidModAlchemistEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 0, 5, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 3;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Phosphorescent Tunic");
			/* Tooltip.SetDefault("5% increased potency regeneration"
							+ "\nMaximum potency increased by 2"); */
		}

		public override void UpdateEquip(Player player)
		{
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			modPlayer.alchemistRegenPotency -= 3;
			modPlayer.alchemistPotencyMax += 2;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Silk, 8);
			recipe.AddIngredient(ItemID.GlowingMushroom, 5);
			recipe.AddIngredient(null, "MushroomThread", 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			Color color = new Color(63, 67, 207) * 0.2f * OrchidWorld.alchemistMushroomArmorProgress;
			Lighting.AddLight(Item.Center, color.R / 255f, color.G / 255f, color.B / 255f);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			spriteBatch.DrawSimpleItemGlowmaskInWorld(Item, new Color(250, 250, 250, 200) * OrchidWorld.alchemistMushroomArmorProgress, rotation, scale);
		}

		public override void EquipFrameEffects(Player player, EquipType type)
		{
			Color color = new Color(63, 67, 207) * 0.2f * OrchidWorld.alchemistMushroomArmorProgress;
			Lighting.AddLight(player.Center, color.R / 255f, color.G / 255f, color.B / 255f);
		}
	}
}
