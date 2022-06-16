using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.Abyss
{
	[AutoloadEquip(EquipType.Head)]
	public class AbyssalHelm : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.value = 0;
			Item.rare = ItemRarityID.Red;
			Item.defense = 18;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyssal Helm");
			Tooltip.SetDefault("Your shamanic bonds will last 5 seconds longer"
								+ "\n7% increased shamanic damage and critical strike chance");
		}

		public override void UpdateEquip(Player player)
		{
			Main.player[Main.myPlayer].GetModPlayer<OrchidModPlayer>().shamanBuffTimer += 5;
			Main.player[Main.myPlayer].GetModPlayer<OrchidModPlayer>().shamanCrit += 7;
			Main.player[Main.myPlayer].GetModPlayer<OrchidModPlayer>().shamanDamage += 0.07f;
			Lighting.AddLight(player.position, 0.15f, 0.15f, 0.8f);
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowLokis = true;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == Mod.Find<ModItem>("AbyssalChestplate").Type && legs.type == Mod.Find<ModItem>("AbyssalGreaves").Type;
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			player.armorEffectDrawShadow = true;
			String dir = Main.ReversedUpDownArmorSetBonuses ? "DOWN" : "UP";
			player.setBonus = "\n             Double tap " + dir + " to summon an abyss portal"
							+ "\n             Portal damage grants an air shamanic bond";

			//modPlayer.shamanFireBonus += 1;
			//modPlayer.shamanWaterBonus += 1;
			//modPlayer.shamanAirBonus += 1;
			//modPlayer.shamanEarthBonus += 1;
			//modPlayer.shamanSpiritBonus += 1;
			modPlayer.abyssSet = true;
		}

		public override bool DrawHead()
		{
			return true;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawHair = drawAltHair = false;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LunarBar, 8);
			recipe.AddIngredient(ModContent.ItemType<Misc.AbyssFragment>(), 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
			recipe.Register();
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			spriteBatch.DrawSimpleItemGlowmaskInWorld(Item, Color.White, rotation, scale);
		}
	}
}
