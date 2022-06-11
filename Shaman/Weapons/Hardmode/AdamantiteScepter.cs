using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class AdamantiteScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 51;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 50;
			Item.useAnimation = 50;
			Item.knockBack = 4.15f;
			Item.rare = 4;
			Item.value = Item.sellPrice(0, 2, 70, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = Mod.Find<ModProjectile>("AdamantiteScepterProj").Type;
			this.empowermentType = 4;
			this.energy = 12;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Adamantite Scepter");
			Tooltip.SetDefault("Shoots a potent adamantite bolt, hitting your enemy 3 times"
							  + "\nHitting the same target with all 3 shots will grant you an adamantite orb"
							  + "\nIf you have 5 adamantite orbs, your attack will be empowered, dealing double damage");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 3;
			if (player.GetModPlayer<OrchidModPlayer>().orbCountBig >= 15 && player.GetModPlayer<OrchidModPlayer>().shamanOrbBig == ShamanOrbBig.ADAMANTITE)
			{
				for (int i = 0; i < 3; i++)
				{
					Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
					this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, Mod.Find<ModProjectile>("AdamantiteScepterProj").Type, damage * 2, knockBack, player.whoAmI);
				}
				player.GetModPlayer<OrchidModPlayer>().orbCountBig = -3;
			}
			else
			{
				for (int i = 0; i < numberProjectiles; i++)
				{
					this.NewShamanProjectile(position.X, position.Y, speedX, speedY, Mod.Find<ModProjectile>("AdamantiteScepterProj").Type, damage, knockBack, player.whoAmI);
				}
			}
			return false;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.AdamantiteBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
			recipe.AddRecipe();
		}
	}
}
