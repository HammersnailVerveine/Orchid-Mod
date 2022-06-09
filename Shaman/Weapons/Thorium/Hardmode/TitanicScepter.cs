using Microsoft.Xna.Framework;
using OrchidMod.Common.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	public class TitanicScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			Item.damage = 53;
			Item.width = 54;
			Item.height = 54;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.knockBack = 4.65f;
			Item.rare = ItemRarityID.LightPurple;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = Mod.Find<ModProjectile>("TitanicScepterProj").Type;
			this.empowermentType = 4;
			this.energy = 12;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Titan Scepter");
			Tooltip.SetDefault("Shoots a potent titanic energy bolt, hitting your enemy 3 times"
							+ "\nHitting the same target with all 3 shots will grant you an titan orb"
							+ "\nIf you have 5 orbs, your next hit will boost your critical strikes abilities for a while"
							+ "\nCritical strikes will deal additional damage");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 3;
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
				recipe.AddTile(thoriumMod.Find<ModTile>("SoulForge").Type);
				recipe.AddIngredient(thoriumMod, "TitanBar", 8);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
	}
}
