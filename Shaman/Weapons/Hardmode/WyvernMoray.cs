using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Hardmode
{
    public class WyvernMoray : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 32;
			item.width = 52;
			item.height = 52;
			item.useTime = 40;
			item.useAnimation = 40;
			item.knockBack = 3.8f;
			item.rare = 5;
			item.value = Item.sellPrice(0, 4, 0, 0);
			item.UseSound = SoundID.Item21;
			item.autoReuse = true;
			item.shootSpeed = 15f;
			item.shoot = mod.ProjectileType("WyvernMorayProj");
			this.empowermentType = 3;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Wyvern Moray");
		  Tooltip.SetDefault("Spits lingering cloud energy"
							+ "\nThe weapon itself can critically strike, releasing a more powerful projectile"
							+ "\nThe more shamanic bonds you have, the higher the chances of critical strike");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			
			if (Main.rand.Next(101) < 4 + nbBonds * 4) {
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("WyvernMorayProjAlt"), damage * 2, knockBack, player.whoAmI);
			} else {
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}		
    }
}
