using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;
 
namespace OrchidMod.Shaman.Weapons.Hardmode
{
    public class SunRay : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 60;
			item.channel = true;
			item.width = 30;
			item.height = 30;
			item.useTime = 6;
			item.useAnimation = 30;
			item.knockBack = 4.15f;
			item.rare = 8;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.UseSound = SoundID.Item15;
			item.autoReuse = true;
			item.shootSpeed = 15f;
			item.shoot = mod.ProjectileType("SunRayProj");
			this.empowermentType = 1;
			this.energy = 10;
		}
		
		public override void SafeModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			mult *= modPlayer.shamanDamage + (nbBonds * 0.1f);
			if (!Main.dayTime) add -= 0.1f;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Sun Ray");
		  Tooltip.SetDefault("Shoots a continuous sun beam"
						   + "\nDamage scales with the number of active shamanic bonds"
						   + "\n10% increased damage during the day");
		}
    }
}
