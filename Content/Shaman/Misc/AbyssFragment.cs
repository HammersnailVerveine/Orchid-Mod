using Microsoft.Xna.Framework;
using OrchidMod.Common.Attributes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Misc
{
	[ClassTag(Common.ClassTags.Shaman)]
	public class AbyssFragment : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Cyan;
		}

		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemIconPulse[Item.type] = true;
			ItemID.Sets.ItemNoGravity[Item.type] = true;

			// DisplayName.SetDefault("Abyss Fragment");
			// Tooltip.SetDefault("'A feeling of pure immensity emanates from this fragment'");
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.Blue.ToVector3() * 0.55f * Main.essScale);
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}
