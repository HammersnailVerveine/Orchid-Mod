using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Hardmode
{
    public class IceFlakeCone : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 72;
			item.width = 42;
			item.height = 42;
			item.useTime = 10;
			item.useAnimation = 10;
			item.knockBack = 1.15f;
			item.rare = 8;
			item.value = Item.sellPrice(0, 4, 80, 0);
			item.UseSound = SoundID.Item117;
			item.autoReuse = true;
			item.shootSpeed = 12f;
			item.shoot = mod.ProjectileType("IceFlakeConeProj");
			this.empowermentType = 2;
			this.energy = 10;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Ice Flake");
		  Tooltip.SetDefault("Shoots returning ice blades"
							+"\nThe maximum number of projectiles launched depends on the number of active shamanic bonds");
		}
		
		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int BuffsCount = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			
			int numberProjectiles = 1 + Main.rand.Next(2 + BuffsCount);
		
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(7));
				this.newShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
    }
}
