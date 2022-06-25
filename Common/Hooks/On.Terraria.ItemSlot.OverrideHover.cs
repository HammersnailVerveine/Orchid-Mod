using Terraria;

namespace OrchidMod.Common.Hooks
{
	public partial class HookLoader
	{
		private static void On_Terraria_ItemSlot_OverrideHover(On.Terraria.UI.ItemSlot.orig_OverrideHover_ItemArray_int_int orig, Item[] inv, int context, int slot)
		{
			orig(inv, context, slot);

			var item = inv[slot];

			if (item.ModItem is IOnMouseHover obj)
			{
				obj.OnMouseHover(context);
			}
		}
	}
}
