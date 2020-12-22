using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Chat;
 
namespace OrchidMod.Shaman.Weapons
{
    public class EnchantedScepter : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 10;
			item.width = 42;
			item.height = 42;
			item.useTime = 28;
			item.useAnimation = 28;
			item.knockBack = 3.15f;
			item.rare = 1;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.UseSound = SoundID.Item43;
			item.autoReuse = true;
			item.shootSpeed = 9f;
			item.shoot = mod.ProjectileType("EnchantedScepterProj");
			this.empowermentType = 1;
			this.empowermentLevel = 1;
		}
		
		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Enchanted Scepter");
		  Tooltip.SetDefault("Weapon damage increases with the number of active shamanic bonds");
		}
		
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			flat += (modPlayer.getNbShamanicBonds() * 3f);
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
    }
}
