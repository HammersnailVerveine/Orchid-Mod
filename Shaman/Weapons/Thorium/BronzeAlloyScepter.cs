using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Thorium
{
	public class BronzeAlloyScepter : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 23;
			item.width = 30;
			item.height = 30;
			item.useTime = 18;
			item.useAnimation = 18;
			item.knockBack = 2.75f;
			item.rare = 3;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.UseSound = SoundID.Item43;
			item.autoReuse = true;
			item.shootSpeed = 14f;
			item.shoot = mod.ProjectileType("BronzeAlloyScepterProj");
			this.empowermentType = 4;
			this.empowermentLevel = 2;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Basilisk's Fang");
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod == null) {
				Tooltip.SetDefault("[c/FF0000:Thorium Mod is not loaded]"
								+ "\n[c/970000:This is a cross-content weapon]");
				return;
			}
			Tooltip.SetDefault("Fires out a poisonous basilisk fang"
							+ "\nThe number of active shamanic bonds will increase the poison duration"
							+ "\nThe weapon itself can critically strike, releasing a petrifying projectile"
							+ "\nThe more shamanic bonds you have, the higher the chances of critical strike");
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 50f; 
			
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;
			
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(3));		
			if (Main.rand.Next(101) < 5 + nbBonds * 5) {
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X * 1.2f, perturbedSpeed.Y * 1.2f, mod.ProjectileType("BronzeAlloyScepterProjAlt"), damage * 2, knockBack, player.whoAmI);
			} else {
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
		
		public override void AddRecipes()
		{
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				ModRecipe recipe = new ModRecipe(thoriumMod);
				recipe.AddTile(TileID.Anvils);		
				recipe.AddIngredient(null, "BronzeFragments", 10);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
        }
    }
}

