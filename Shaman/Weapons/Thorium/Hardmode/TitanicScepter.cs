using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrchidMod.Interfaces;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
    public class TitanicScepter : OrchidModShamanItem, ICrossmodItem
    {
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 53;
			item.width = 54;
			item.height = 54;
			item.useTime = 45;
			item.useAnimation = 45;
			item.knockBack = 4.65f;
			item.rare = ItemRarityID.LightPurple;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.UseSound = SoundID.Item117;
			item.autoReuse = true;
			item.shootSpeed = 15f;
			item.shoot = mod.ProjectileType("TitanicScepterProj");
			this.empowermentType = 4;
			this.empowermentLevel = 4;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titan Scepter");
			Tooltip.SetDefault("Shoots a potent titanic energy bolt, hitting your enemy 3 times"
							+"\nHitting the same target with all 3 shots will grant you an titan orb"
							+"\nIf you have 5 orbs, your next hit will boost your critical strikes abilities for a while"
							+"\nCritical strikes will deal additional damage");
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			int numberProjectiles = 3;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
			}
			return false;
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(thoriumMod);
				recipe.AddTile(thoriumMod.TileType("SoulForge"));		
				recipe.AddIngredient(thoriumMod, "TitanBar", 8);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
        }
    }
}
