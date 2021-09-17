using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace OrchidMod.Content
{
	public abstract class OrchidTile : ModTile
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "OrchidMod/Assets/Textures/Tiles/" + name;
			return true;
		}

		// ...

		public void CreateMapEntry(string name, Color color, Func<string, int, int, string> nameFunc = null)
		{
			this.CreateMapEntry(null, name, color, nameFunc);
		}

		public void CreateMapEntry(string key, string name, Color color, Func<string, int, int, string> nameFunc = null)
		{
			ModTranslation translation = CreateMapEntryName(key);
			translation.SetDefault(name);

			if (nameFunc != null) this.AddMapEntry(color, translation, nameFunc);
			else this.AddMapEntry(color, translation);
		}
	}
}
