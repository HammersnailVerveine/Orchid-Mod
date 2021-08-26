using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.Abyss
{
	[AutoloadEquip(EquipType.Body)]
	public class AbyssalChestplate : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 34;
			item.height = 22;
			item.value = 0;
			item.rare = ItemRarityID.Red;
			item.defense = 32;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyssal Chestplate");
			Tooltip.SetDefault("9% increased shamanic damage and critical strike chance");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanCrit += 9;
			modPlayer.shamanDamage += 0.09f;
			Lighting.AddLight(player.position, 0.15f, 0.15f, 0.8f);
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowLokis = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 16);
			recipe.AddIngredient(ModContent.ItemType<Misc.AbyssFragment>(), 20);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			OrchidHelper.DrawSimpleItemGlowmaskInWorld(item, spriteBatch, ModContent.GetTexture("OrchidMod/Glowmasks/AbyssalChestplate_Glowmask"), Color.White, rotation, scale);
		}
	}
}
