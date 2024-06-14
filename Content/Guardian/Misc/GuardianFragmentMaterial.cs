using Microsoft.Xna.Framework;
using OrchidMod.Common.Attributes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Misc
{
	[ClassTag(Common.ClassTags.Guardian)]
	public class GuardianFragmentMaterial : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Cyan;
		}

		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemIconPulse[Item.type] = true;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.Teal.ToVector3() * 0.55f * Main.essScale);
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}
