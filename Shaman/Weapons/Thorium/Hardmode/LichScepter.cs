using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrchidMod.Interfaces;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	public class LichScepter : OrchidModShamanItem, ICrossmodItem
    {
		public string CrossmodName => "Thorium Mod";
		
		public override void SafeSetDefaults()
		{
			item.damage = 75;
			item.width = 50;
			item.height = 50;
			item.useTime = 42;
			item.useAnimation = 42;
			item.knockBack = 2.75f;
			item.rare = ItemRarityID.LightPurple;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.UseSound = SoundID.Item43;
			item.autoReuse = true;
			item.shootSpeed = 1f;
			item.shoot = mod.ProjectileType("LichScepterProj");
			this.empowermentType = 3;
			this.energy = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reliquary Candle");
			Tooltip.SetDefault("Fires out a bolt of spiritual fire, dividing upon hitting a foe"
							+ "\nIf you have 4 or more active shamanic bonds, the bonus projectiles will home at nearby enemies");
		}
    }
}

