using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace OrchidMod.Content
{
	public abstract class OrchidItem : ModItem
	{
		public override string Texture => "OrchidMod/Assets/Textures/Items/" + this.Name;
	}
}
