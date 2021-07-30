using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Hardmode
{
    public class MartianBeamer : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 70;
			item.width = 30;
			item.height = 30;
			item.useTime = 18;
			item.useAnimation = 18;
			item.knockBack = 1.15f;
			item.rare = 8;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.UseSound = SoundID.Item91;
			item.autoReuse = true;
			item.shootSpeed = 7f;
			item.shoot = mod.ProjectileType("MartianBeamerProj");
			this.empowermentType = 1;
			OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeaponNoUsetimeReforge = true;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Martian Beamer");
		  Tooltip.SetDefault("Shoots martian homing lasers"
							+"\nWeapon speed increases with the number of active shamanic bonds");

			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				thoriumMod.Call("AddMartianItemID", item.type);
			}
		}
		
		public override void UpdateInventory(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			
			item.useTime = 18 - (2 * nbBonds);
			item.useAnimation = 18 - (2 * nbBonds);
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for(int i = 0; i < Main.rand.Next(1, 1); i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				
				perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(-20));
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
    }
}