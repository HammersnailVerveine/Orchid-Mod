using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Misc
{
	// TODO: At the moment it is considered a scepter ... We need to fix this
	public class AbyssFragment : OrchidModShamanItem
	{
		public override string Texture => OrchidAssets.AbyssSetPath + Name;

		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Cyan;
		}

		public override void SafeSetStaticDefaults()
		{
			ItemID.Sets.ItemIconPulse[Item.type] = true;
			ItemID.Sets.ItemNoGravity[Item.type] = true;

			DisplayName.SetDefault("Abyss Fragment");
			Tooltip.SetDefault("'A feeling of pure immensity emanates from this fragment'");
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.Blue.ToVector3() * 0.55f * Main.essScale);
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}
