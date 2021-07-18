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
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chiroptera");
			Tooltip.SetDefault("Fires out a bolt of blood magic"
							+ "\nHas a chance to spawn a homing bat on hit"
							+ "\nThe chance for a bat to spawn increases with the number of active shamanic bonds");
		}
		
		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f; 
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;
			
			return true;
		}
    }
}

