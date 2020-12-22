using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Chat;
 
namespace OrchidMod.Shaman.Weapons
{
    public class BloodCaller : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 34;
			item.width = 36;
			item.height = 38;
			item.useTime = 45;
			item.useAnimation = 45;
			item.knockBack = 5.5f;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 60, 0);
			item.UseSound = SoundID.Item45;
			item.autoReuse = true;
			item.shootSpeed = 9.5f;
			item.shoot = mod.ProjectileType("CrimsonScepterProj");
			this.empowermentType = 1;
			this.empowermentLevel = 1;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Blood Caller");
		  Tooltip.SetDefault("\nHitting an enemy will grant you a crimson heart"
							+"\nIf you have 3 crimson hearts, your next hit will recover some life");
		}
			
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 50f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
			position += muzzleOffset;
			}
			return true;
		}
    }
}
