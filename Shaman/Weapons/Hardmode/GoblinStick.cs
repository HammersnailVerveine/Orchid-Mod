using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Hardmode
{
    public class GoblinStick : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 54;
			item.width = 30;
			item.height = 30;
			item.useTime = 25;
			item.useAnimation = 60;
			item.knockBack = 3.15f;
			item.rare = 5;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.UseSound = SoundID.Item103;
			item.autoReuse = false;
			item.shootSpeed = 3f;
			item.shoot = mod.ProjectileType("GoblinStickProj");
			this.empowermentType = 3;
			this.empowermentLevel = 3;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goblin Stick");
			Tooltip.SetDefault("Channels a volley of shadowflame balls"
							  +"\nThe number of projectiles shot during the channeling depends on the number of active shamanic bonds");
		}
		
		public override void UpdateInventory(Player player) {
			switch (Main.player[Main.myPlayer].GetModPlayer<OrchidModPlayer>().getNbShamanicBonds()) {
				case 0 :
					item.useTime = 60;
					break;
				case 1 :
					item.useTime = 30;
					break;
				case 2 :
					item.useTime = 20;
					break;
				case 3 :
					item.useTime = 15;
					break;
				case 4 :
					item.useTime = 12;
					break;
				case 5 :
					item.useTime = 10;
					break;
			}
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			int numberProjectiles = 1 + Main.rand.Next(2);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
    }
}
