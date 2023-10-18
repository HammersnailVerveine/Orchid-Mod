using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Alchemist.Armors.Mushroom
{
	[AutoloadEquip(EquipType.Head)]
	public class MushroomBandana : OrchidModAlchemistEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 12;
			Item.value = Item.sellPrice(0, 0, 3, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 2;
		}

		public override void SetStaticDefaults()
		{
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;

			// DisplayName.SetDefault("Phosphorescent Bandana");
			// Tooltip.SetDefault("5% increased potency regeneration");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			modPlayer.alchemistRegenPotency -= 3;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == Mod.Find<ModItem>("MushroomTunic").Type && legs.type == Mod.Find<ModItem>("MushroomLeggings").Type;
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			player.setBonus = "Maximum number of simultaneous alchemical elements increased by 1";
			modPlayer.alchemistNbElementsMax += 1;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Silk, 4);
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
