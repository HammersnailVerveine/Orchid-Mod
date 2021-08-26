using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Misc
{
	public class AbyssFragment : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{


			item.width = 24;
			item.height = 24;
			item.maxStack = 999;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = 9;
			ItemID.Sets.ItemIconPulse[item.type] = true;
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyss Fragment");
			Tooltip.SetDefault("'A feeling of pure immensity emanates from this fragment'");
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, Color.Blue.ToVector3() * 0.55f * Main.essScale);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}
