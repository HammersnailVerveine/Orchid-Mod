using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrchidMod.Interfaces;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Thorium
{
	public class ViscountScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 28;
			item.width = 30;
			item.height = 30;
			item.useTime = 25;
			item.useAnimation = 25;
			item.knockBack = 3.75f;
			item.rare = 2;
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.UseSound = SoundID.Item43;
			item.autoReuse = true;
			item.shootSpeed = 8f;
			item.shoot = mod.ProjectileType("ViscountScepterProj");
			this.empowermentType = 3;
			this.energy = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chiroptera");
			Tooltip.SetDefault("Fires out a bolt of blood magic"
							+ "\nHas a chance to spawn a homing bat on hit"
							+ "\nThe chance for a bat to spawn increases with the number of active shamanic bonds");
		}
    }
}

