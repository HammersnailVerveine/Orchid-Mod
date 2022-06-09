using Microsoft.Xna.Framework;
using OrchidMod.Common.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium
{
	public class MagmaScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			Item.damage = 15;
			Item.width = 46;
			Item.height = 46;
			Item.useTime = 48;
			Item.useAnimation = 48;
			Item.knockBack = 4.35f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 25, 0);
			Item.UseSound = SoundID.Item21;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = Mod.Find<ModProjectile>("MagmaScepterProj").Type;
			this.empowermentType = 4;
			this.energy = 7;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Magma Scepter");
			Tooltip.SetDefault("Shoots a magma bolt, hitting your enemy 2 times"
							+ "\nHitting the same target twice will grant you a magma orb"
							+ "\nIf you have 5 orbs, your next hit will explode"
							+ "\nAttacks might singe the target, causing extra damage");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 2;
			for (int i = 0; i < numberProjectiles; i++)
			{
				this.NewShamanProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(Mod);
				recipe.AddTile(TileID.Anvils);
				recipe.AddIngredient(thoriumMod, "MagmaCore", 8);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
	}
}
