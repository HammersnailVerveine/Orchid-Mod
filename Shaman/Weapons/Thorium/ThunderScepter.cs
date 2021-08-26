using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Chat;
using OrchidMod.Interfaces;

namespace OrchidMod.Shaman.Weapons.Thorium
{
    public class ThunderScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 12;
			item.width = 42;
			item.height = 42;
			item.useTime = 14;
			item.useAnimation = 14;
			item.knockBack = 3.15f;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.UseSound = SoundID.Item93;
			item.autoReuse = true;
			item.shootSpeed = 12f;
			item.shoot = mod.ProjectileType("ThunderScepterProj");
			this.empowermentType = 3;
			this.energy = 10;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thunder Scepter");
			Tooltip.SetDefault("Rapidly zaps your foes"
							+ "\nHitting will charge up energy above you"
							+ "\nWhen fully loaded, potent wind gusts will be released");
		}
    }
}
