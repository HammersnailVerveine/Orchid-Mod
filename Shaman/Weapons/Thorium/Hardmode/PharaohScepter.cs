using Microsoft.Xna.Framework;
using OrchidMod.Common.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	public class PharaohScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			Item.damage = 35;
			Item.width = 52;
			Item.height = 52;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.knockBack = 3.25f;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 7, 50, 0);
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = true;
			Item.shootSpeed = 12f;
			Item.shoot = Mod.Find<ModProjectile>("PharaohScepterProj").Type;
			this.empowermentType = 3;
			this.energy = 7;
		}

		public override void SafeSetStaticDefaults()
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
				if (proj.active && proj.type == Mod.Find<ModProjectile>("PharaohScepterPortal").Type && proj.owner == player.whoAmI)
				{
					Vector2 target = Main.MouseWorld;
					Vector2 heading = target - proj.position;
					heading.Normalize();
					heading *= new Vector2(speedX, speedY).Length();
					float speedXAlt = heading.X;
					float speedYAlt = heading.Y + Main.rand.Next(-40, 41) * 0.02f;

					this.NewShamanProjectile(proj.Center.X, proj.Center.Y, speedXAlt, speedYAlt, Mod.Find<ModProjectile>("PharaohScepterProjAlt").Type, damage, knockBack, player.whoAmI);
				}
			}

			return true;
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(Mod);
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 2);
				recipe.AddIngredient(thoriumMod, "PharaohsBreath", 8);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
	}
}

