using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Thorium
{
	public class OrchidScepter : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 18;
			item.width = 30;
			item.height = 30;
			item.useTime = 25;
			item.useAnimation = 25;
			item.knockBack = 4f;
			item.rare = 2;
			item.value = Item.sellPrice(0, 0, 30, 0);
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shootSpeed = 13f;
			item.shoot = mod.ProjectileType("OrchidScepterProj");
			this.empowermentType = 4;
			this.empowermentLevel = 2;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orchid Scepter");
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod == null) {
				Tooltip.SetDefault("[c/FF0000:Thorium Mod is not loaded]"
								+ "\n[c/970000:This is a cross-content weapon]");
				return;
			}
			Tooltip.SetDefault("Shoots a volley of piercing petals"
							+ "\nThe number of petals increase with active shamanic bonds"
							+ "\nHaving 3 or more bonds will allow the petals to pierce more enemies");
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 1 + Main.rand.Next(2);
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			numberProjectiles += nbBonds > 1 ? nbBonds > 3 ? 2 : 1 : 0;
		
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(4));
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
		
		public override void AddRecipes()
		{
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				ModRecipe recipe = new ModRecipe(thoriumMod);
				recipe.AddTile(thoriumMod.TileType("ArcaneArmorFabricator"));		
				recipe.AddIngredient(null, "Petal", 8);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
        }
    }
}

