using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrchidMod.Interfaces;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Thorium
{
	public class QueenJellyfishScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 20;
			item.width = 42;
			item.height = 42;
			item.useTime = 42;
			item.useAnimation = 42;
			item.knockBack = 5.5f;
			item.rare = ItemRarityID.Green;
			item.value = Item.sellPrice(0, 0, 30, 0);
			item.UseSound = SoundID.Item43;
			item.autoReuse = true;
			item.shootSpeed = 5f;
			item.shoot = mod.ProjectileType("QueenJellyfishScepterProj");
			this.empowermentType = 2;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jellyfish Dynamizer");
			Tooltip.SetDefault("Launches a bouncy jellyfish, gaining in damage with each rebound"
							+ "\nDamage increase depends on your number of active shamanic bonds"
							+ "\n'Apparently waterproof'");
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f; 
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;
			
			return true;
		}
    }
}

