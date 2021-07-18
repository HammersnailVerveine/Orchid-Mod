using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrchidMod.Interfaces;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	public class CoznixScepter : OrchidModShamanItem, ICrossmodItem
    {
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 35;
			item.width = 54;
			item.height = 54;
			item.useTime = 45;
			item.useAnimation = 45;
			item.knockBack = 3.25f;
			item.rare = ItemRarityID.Pink;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.UseSound = SoundID.Item45;
			item.autoReuse = false;
			item.shootSpeed = 10f;
			item.shoot = mod.ProjectileType("CoznixScepterProj");
			this.empowermentType = 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gate to the Fallen");
			Tooltip.SetDefault("Fires out a void bolt"
							+ "\nIf you have 3 or more active shamanic bonds, the bolt will summon a void portal");
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

