using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.Thorium.OreHelms
{
	[AutoloadEquip(EquipType.Head)]
	public class TitanSpangenhelm : OrchidModShamanEquipable
	{


		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 2, 28, 0);
			Item.rare = 6;
			Item.defense = 14;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titan Spangenhelm");
			Tooltip.SetDefault("Your shamanic bonds will last 4 seconds longer");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanBuffTimer += 4;
			//modPlayer.shamanFireBonus += 1;
			//modPlayer.shamanWaterBonus += 1;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				return body.type == thoriumMod.Find<ModItem>("TitanBreastplate").Type && legs.type == thoriumMod.Find<ModItem>("TitanGreaves").Type;
			}
			else
			{
				return false;
			}
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Damage done increased by 18%!";
			player.allDamage += 0.18f;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawHair = drawAltHair = false;
		}

		public override bool DrawHead()
		{
			return true;
		}

		public override void AddRecipes()
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(thoriumMod.Find<ModTile>("SoulForge").Type);
				recipe.AddIngredient(thoriumMod, "TitanBar", 12);
				recipe.Register();
				recipe.AddRecipe();
			}
		}
	}
}
