using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Misc
{
	// TODO: At the moment it is considered a scepter ... We need to fix this
	public class AbyssFragment : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.maxStack = 999;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = ItemRarityID.Cyan;
		}

		public override void SafeSetStaticDefaults()
		{
			ItemID.Sets.ItemIconPulse[item.type] = true;
			ItemID.Sets.ItemNoGravity[item.type] = true;

			DisplayName.SetDefault("Abyss Fragment");
			Tooltip.SetDefault("'A feeling of pure immensity emanates from this fragment'");
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, Color.Blue.ToVector3() * 0.55f * Main.essScale);
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}
