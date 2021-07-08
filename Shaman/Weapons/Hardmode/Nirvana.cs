using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Hardmode
{
    public class Nirvana : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 200;
			item.magic = true;
			item.width = 64;
			item.height = 64;
			item.useTime = 35;
			item.useAnimation = 35;
			item.knockBack = 6.15f;
			item.rare = 10;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.UseSound = SoundID.Item70;
			item.autoReuse = true;
			item.shootSpeed = 5f;
			item.shoot = mod.ProjectileType("NirvanaMain");
			this.empowermentType = 5;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Nirvana");
		  Tooltip.SetDefault("Shoots a bolt of elemental energy, calling all four elements upon impact"
							+"\nIf you have 3 or more active shamanic bonds, more elements will be called");
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			return true;
		}

		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
