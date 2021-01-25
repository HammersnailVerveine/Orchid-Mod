using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	public class BoreanStriderScepter : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 35;
			item.width = 56;
			item.height = 56;
			item.useTime = 34;
			item.useAnimation = 34;
			item.knockBack = 2.75f;
			item.rare = 5;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.UseSound = SoundID.Item43;
			item.autoReuse = true;
			item.shootSpeed = 10f;
			item.shoot = mod.ProjectileType("BoreanStriderScepterProj");
			this.empowermentType = 2;
			this.empowermentLevel = 3;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Taiga Truncheon");
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod == null) {
				Tooltip.SetDefault("[c/FF0000:Thorium Mod is not loaded]"
								+ "\n[c/970000:This is a cross-content weapon]");
				return;
			}
			Tooltip.SetDefault("Fires out a damaging frost ball"
							+ "\nReleases icicles on impact, based on your number of active shamanic bonds");
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 50f; 
			
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;
			
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(3));
			Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			return false;
		}
    }
}

