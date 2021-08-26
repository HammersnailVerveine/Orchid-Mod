using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrchidMod.Interfaces;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	public class PharaohScepter : OrchidModShamanItem, ICrossmodItem
    {
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 35;
			item.width = 52;
			item.height = 52;
			item.useTime = 35;
			item.useAnimation = 35;
			item.knockBack = 3.25f;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.sellPrice(0, 7, 50, 0);
			item.UseSound = SoundID.Item45;
			item.autoReuse = true;
			item.shootSpeed = 12f;
			item.shoot = mod.ProjectileType("PharaohScepterProj");
			this.empowermentType = 3;
			this.energy = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Isis' Command");
			Tooltip.SetDefault("Fires out an ancient magic spell"
							+ "\nHitting may summon a golden miror, replicating your shots"
							+ "\nThe more shamanic bonds you have, the greater the chance to summon a mirror");
		}
		
		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            for (int l = 0; l < Main.projectile.Length; l++)
            {  
                Projectile proj = Main.projectile[l];
                if (proj.active && proj.type == mod.ProjectileType("PharaohScepterPortal") && proj.owner == player.whoAmI)
                {
					Vector2 target = Main.MouseWorld;
					Vector2 heading = target - proj.position;
					heading.Normalize();
					heading *= new Vector2(speedX, speedY).Length();
					float speedXAlt = heading.X;
					float speedYAlt = heading.Y + Main.rand.Next(-40, 41) * 0.02f;

                    this.newShamanProjectile(proj.Center.X, proj.Center.Y, speedXAlt, speedYAlt, mod.ProjectileType("PharaohScepterProjAlt"), damage, knockBack, player.whoAmI);
                }
            }
			
			return true;
		}
		
		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 2);
				recipe.AddIngredient(thoriumMod, "PharaohsBreath", 8);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
        }
    }
}

