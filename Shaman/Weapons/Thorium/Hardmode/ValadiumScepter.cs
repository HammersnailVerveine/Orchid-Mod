using Microsoft.Xna.Framework;
using OrchidMod.Common.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	public class ValadiumScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			Item.damage = 45;
			Item.width = 54;
			Item.height = 54;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.knockBack = 0f;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 13f;
			Item.shoot = Mod.Find<ModProjectile>("ValadiumScepterProj").Type;
			this.empowermentType = 4;
			this.energy = 12;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Valadium Scepter");
			Tooltip.SetDefault("Shoots a potent valadium bolt, hitting your enemy 3 times"
							+ "\nHitting the same target with all 3 shots will grant you a valadium orb"
							+ "\nIf you have 5 orbs, your next hit will deal massive damage and send your target flying"
							+ "\nAttacks will blast enemies with inter-dimensional energy briefly");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 3;
			for (int i = 0; i < numberProjectiles; i++)
			{
				int dmg = player.GetModPlayer<OrchidModPlayer>().orbCountBig >= 15 ? damage * 2 : damage;
				this.NewShamanProjectile(position.X, position.Y, speedX, speedY, type, dmg, knockBack, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(Mod);
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(thoriumMod, "ValadiumIngot", 8);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
	}
}
