using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.UI;

namespace OrchidMod.Common.UIs
{
	public abstract class OrchidUIState : UIState
	{
		public Mod Mod { get; internal set; }

		public virtual InterfaceScaleType ScaleType { get; set; } = InterfaceScaleType.UI;
		public virtual bool Visible { get; set; } = true;
		public abstract int InsertionIndex(List<GameInterfaceLayer> layers);

		public virtual void Unload() { }
		public virtual void OnResolutionChanged(int width, int height) { }
		public virtual void OnUIScaleChanged() { }
	}
}