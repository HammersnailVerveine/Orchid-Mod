using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrchidMod.Interfaces;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
    public class ValadiumScepter : OrchidModShamanItem, ICrossmodItem
    {
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 45;
			item.width = 54;
			item.height = 54;
			item.useTime = 40;
			item.useAnimation = 40;
			item.knockBack = 0f;
			item.rare = ItemRarityID.Pink;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.UseSound = SoundID.Item117;
			item.autoReuse = true;
			item.shootSpeed = 13f;
			item.shoot = mod.ProjectileType("ValadiumScepterProj");
			this.empowermentType = 4;
			this.empowermentLevel = 3;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Valadium Scepter");
			Tooltip.SetDefault("Shoots a potent valadium bolt, hitting your enemy 3 times"
							+"\nHitting the same target with all 3 shots will grant you a valadium orb"
							+"\nIf you have 5 orbs, your next hit will deal massive damage and send your target flying"
							+"\nAttacks will blast enemies with inter-dimensional energy briefly");
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
				int dmg = player.GetModPlayer<OrchidModPlayer>().orbCountBig >= 15 ? damage * 2 : damage;
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, dmg, knockBack, player.whoAmI, 0f, 0f);
			}
			return false;
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(thoriumMod);
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(thoriumMod, "ValadiumIngot", 8);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
        }
    }
}
