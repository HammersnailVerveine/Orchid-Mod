using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Hardmode
{
    public class ShroomiteScepter : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 93;
			item.width = 44;
			item.height = 44;
			item.useTime = 40;
			item.useAnimation = 40;
			item.knockBack = 1.15f;
			item.rare = 8;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.UseSound = SoundID.Item45; // CHANGE THIS
			item.autoReuse = false;
			item.shootSpeed = 15f;
			item.shoot = mod.ProjectileType("ShroomiteScepterProj1");
			this.empowermentType = 4;
			this.empowermentLevel = 4;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Bloom Shroom");
		  Tooltip.SetDefault("Summons a protective shroom, harming nearby enemies"
							+"\nHaving 3 or more active shamanic bonds weakens hit targets"
							+"\nHaving 5 active shamanic bonds increases nearby players health regeneration");
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f; 
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;
			
			
            for (int l = 0; l < Main.projectile.Length; l++) {
                Projectile proj = Main.projectile[l];
                if (proj.active && proj.type == mod.ProjectileType("ShroomiteScepterProj1") && proj.owner == player.whoAmI)
                {
					proj.Kill();
                }
            }
			
			Vector2 target = Main.screenPosition + new Vector2((float)Main.mouseX - 8, (float)Main.mouseY);
			int newProj = Projectile.NewProjectile(target.X, target.Y, 0f, 10f, mod.ProjectileType("ShroomiteScepterProj1"), damage, knockBack, player.whoAmI);
			Main.projectile[newProj].ai[1] = nbBonds;
			Main.projectile[newProj].netUpdate = true;
			return false;
		}		
    }
}
