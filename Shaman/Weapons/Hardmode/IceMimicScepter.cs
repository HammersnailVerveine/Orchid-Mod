using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Chat;
using System;
 
 
namespace OrchidMod.Shaman.Weapons.Hardmode
{
    public class IceMimicScepter : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 63;
			item.width = 48;
			item.height = 48;
			item.useTime = 20;
			item.useAnimation = 20;
			item.knockBack = 0f;
			item.rare = 5;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.UseSound = SoundID.Item28;
			item.autoReuse = false;
			item.shootSpeed = 15f;
			item.shoot = mod.ProjectileType("IceMimicScepterProj");
			this.empowermentType = 2;
			this.empowermentLevel = 3;
			OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeaponNoUsetimeReforge = true;
		}
		
		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Ice Cycle");
		  Tooltip.SetDefault("Releases a glacial spike, repeatedly impaling the closest enemy"
							+"\nHaving 3 or more active shamanic bonds increases the spike attack rate");
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{	
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
		
            for (int l = 0; l < Main.projectile.Length; l++)
            {  
                Projectile proj = Main.projectile[l];
                if (proj.active && proj.type == item.shoot && proj.owner == player.whoAmI)
                {
                    proj.active = false;
                }
            }
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			
			float speedYalt = new Vector2(speedX, speedY).Length();
			int newProj = Projectile.NewProjectile(position.X, position.Y, 0f, -1f * speedYalt, type, damage, knockBack, player.whoAmI);
			if (modPlayer.getNbShamanicBonds() > 2) {
				Main.projectile[newProj].ai[1] = 3;
			}
			Main.projectile[newProj].netUpdate = true;

			return false;
		}
    }
}
