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
    public class ShamanRod : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 8;
			item.width = 42;
			item.height = 42;
			item.useTime = 20;
			item.useAnimation = 20;
			item.knockBack = 0f;
			item.rare = 2;
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.UseSound = SoundID.Item65;
			item.shootSpeed = 7f;
			item.shoot = mod.ProjectileType("ShamanRodProj");
			this.empowermentType = 4;
			this.empowermentLevel = 2;
		}
		
		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Shaman Rod");
		  Tooltip.SetDefault("Shoots lingering razor-sharp leaves"
							+ "\nOnly one set can be active at once"
							+ "\nHaving 2 or more active shamanic bonds increases damage and slows on hit");
		}
		
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod) > 1) {
				mult *= modPlayer.shamanDamage * 2f;
			}
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			
            for (int l = 0; l < Main.projectile.Length; l++)
            {  
                Projectile proj = Main.projectile[l];
				int deleteproj = mod.ProjectileType("HellShamanRodProj");
                if (proj.active && (proj.type == item.shoot || proj.type == deleteproj) && proj.owner == player.whoAmI)
                {
                    proj.active = false;
                }
            }
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			for (int i = 0; i < 3; i++) {
				int newProj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
				Main.projectile[newProj].ai[1] = i;
				if (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod) > 1) {
					Main.projectile[newProj].ai[1] += 4;
				}
				Main.projectile[newProj].netUpdate = true;
			}
			return false;
		}
    }
}
