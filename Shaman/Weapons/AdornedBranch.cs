using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Chat;
 
namespace OrchidMod.Shaman.Weapons
{
	public class AdornedBranch : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 5;
			item.width = 34;
			item.height = 32;
			item.useTime = 35;
			item.useAnimation = 35;
			item.knockBack = 1.25f;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.UseSound = SoundID.Item8;
			item.shootSpeed = 3f;
			item.shoot = mod.ProjectileType("AdornedBranchProj");
			this.empowermentType = 1;
			this.empowermentLevel = 1;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Adorned Branch");
		  Tooltip.SetDefault("Shoots a burst of splinters"
							+ "\nShoots more projectiles if you have 2 or more active shamanic bonds");
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			
			for (int i = 0; i < Main.rand.Next(3)+3; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(30));
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				perturbedSpeed = perturbedSpeed * scale; 
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}

			if (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod) > 1) {
				for (int i = 0; i < Main.rand.Next(3); i++)
				{
					Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(30));
					float scale = 1f - (Main.rand.NextFloat() * .3f);
					perturbedSpeed = perturbedSpeed * scale; 
					Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				}
			}
			return false;
		}
    }
}

