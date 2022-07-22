using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.UI;

namespace OrchidMod.Common.UIs
{
	public abstract class OrchidUIState : UIState
	{
		private readonly string name;

		public OrchidUIState()
		{
			name = GetType().Name;
		}

		// ...

		public Mod Mod { get; internal set; }
		public string Name { get => name; }

		public virtual InterfaceScaleType ScaleType { get; set; } = InterfaceScaleType.UI;
		public virtual bool Visible { get; set; } = true;
		public virtual float Priority { get; set; } = 0f;
		public abstract int InsertionIndex(List<GameInterfaceLayer> layers);

		public virtual void Unload() { }
		public virtual void OnResolutionChanged(int width, int height) { }
		public virtual void OnUIScaleChanged() { }
	}
}