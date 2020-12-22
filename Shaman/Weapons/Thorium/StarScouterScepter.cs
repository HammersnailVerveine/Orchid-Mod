using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Thorium
{
	public class StarScouterScepter : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 40;
			item.width = 42;
			item.height = 42;
			item.useTime = 40;
			item.useAnimation = 40;
			item.knockBack = 3.25f;
			item.rare =3;
			item.value = Item.sellPrice(0, 0, 54, 0);
			item.UseSound = SoundID.Item43;
			item.autoReuse = false;
			item.shootSpeed = 7f;
			item.shoot = mod.ProjectileType("StarScouterScepterProj");
			this.empowermentType = 3;
			this.empowermentLevel = 2;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orbital Minefield");
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod == null) {
				Tooltip.SetDefault("[c/FF0000:Thorium Mod is not loaded]"
								+ "\n[c/970000:This is a cross-content weapon]");
				return;
			}
			Tooltip.SetDefault("Launches an orbital mine, activating after a while"
							+ "\nIf you have 3 or more bonds, the explosion will release additional bombs");
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f; 
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;
			
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(4));
			Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			return false;
		}
    }
}

