using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Chat;
using System;
 
 
namespace OrchidMod.Shaman.Weapons
{
    public class AvalancheScepter : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 17;
			item.width = 36;
			item.height = 36;
			item.useTime = 30;
			item.useAnimation = 30;
			item.knockBack = 3f;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.UseSound = SoundID.Item28;
			item.autoReuse = true;
			item.shootSpeed = 10f;
			item.shoot = mod.ProjectileType("IceSpearScepterProj");
			this.empowermentType = 2;
		}
		
		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Avalanche");
		  Tooltip.SetDefault("Hitting will spawn and empower a giant icicle above your head"
							+ "\nAfter enough hits, the icicle will be launched, dealing massive damage");
		}
		
		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X + Main.rand.Next(50) - 35 , position.Y - Main.rand.Next(14) + 7 , speedX, speedY, type, damage, knockBack, player.whoAmI);
			return false;
		}
    }
}
