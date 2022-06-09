using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class ReviverofSouls : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 61;
			Item.channel = true;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.knockBack = 4.15f;
			Item.rare = 8;
			Item.value = Item.sellPrice(0, 6, 0, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = Mod.Find<ModProjectile>("ReviverofSoulsProj").Type;
			this.empowermentType = 3;
			this.energy = 12;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Reviver of Souls");
			Tooltip.SetDefault("Summons vengeful souls, damaging your enemies 3 times"
							  + "\nHitting will grant you and empower floating spirit flames"
							  + "\nWeapon damage is increased with the number of spirit flames"
							  + "\nUpon reaching a certain number of flames, they will empower you"
							  + "\nWhile empowered, all your attacks with this weapon will reset every single active shamanic empowerment,"
							  + "\n                                                            and the weapon damage will increase more per shot");
		}

		public override void SafeModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			mult *= player.GetModPlayer<OrchidModPlayer>().shamanDamage;
			add += (((OrchidModPlayer)player.GetModPlayer(Mod, "OrchidModPlayer")).shamanOrbCircle == ShamanOrbCircle.REVIVER) ? ((OrchidModPlayer)player.GetModPlayer(Mod, "OrchidModPlayer")).orbCountCircle * 0.035f : 0;
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 3;
			for (int i = 0; i < numberProjectiles; i++)
			{
				this.NewShamanProjectile(position.X, position.Y, speedX, speedY, Mod.Find<ModProjectile>("ReviverofSoulsProj").Type, damage, knockBack, player.whoAmI);
			}
			return false;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(Mod);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddIngredient(3261, 20);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
