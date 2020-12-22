using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Hardmode
{
    public class HallowedCone : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 38;
			item.width = 38;
			item.height = 38;
			item.useTime = 35;
			item.useAnimation = 35;
			item.knockBack = 1.15f;
			item.rare = 4;
			item.value = Item.sellPrice(0, 7, 50, 0);
			item.UseSound = SoundID.Item101;
			item.autoReuse = true;
			item.shootSpeed = 12f;
			item.shoot = mod.ProjectileType("CrystalScepterProj");
			this.empowermentType = 5;
			this.empowermentLevel = 3;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Prismatic Resonator");
		  Tooltip.SetDefault("Shoots dividing crystalline beams"
							+"\nHitting with the rupture point deals increased damage"
							+"\nThe number of projectiles caused by the division scales with the number of active shamanic bonds");
		}
		
		public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, Color.Pink.ToVector3() * 0.55f * Main.essScale);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			return true;
		}		
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "RitualScepter", 1);
			recipe.AddIngredient(ItemID.CrystalShard, 20);
			recipe.AddIngredient(ItemID.SoulofLight, 15);			
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}
