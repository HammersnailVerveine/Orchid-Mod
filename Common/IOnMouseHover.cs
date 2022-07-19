using Terraria.ModLoader;

namespace OrchidMod.Common
{
	public interface IOnMouseHover
	{
		void OnMouseHover(int context);

		private class HookLoader : ILoadable
		{
			void ILoadable.Load(Mod mod)
			{
				On.Terraria.UI.ItemSlot.OverrideHover_ItemArray_int_int += (orig, inv, context, slot) =>
				{
					orig(inv, context, slot);

					var item = inv[slot];

					if (item.ModItem is IOnMouseHover obj)
					{
						obj.OnMouseHover(context);
					}
				};
			}

			void ILoadable.Unload() { }
		}
	}
}