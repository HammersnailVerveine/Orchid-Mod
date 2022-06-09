using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class IceMimicScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 63;
			Item.width = 48;
			Item.height = 48;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.knockBack = 0f;
			Item.rare = 5;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.UseSound = SoundID.Item28;
			Item.autoReuse = false;
			Item.shootSpeed = 15f;
			Item.shoot = Mod.Find<ModProjectile>("IceMimicScepterProj").Type;
			this.empowermentType = 2;
			OrchidModGlobalItem orchidItem = Item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeaponNoUsetimeReforge = true;
			this.energy = 35;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Cycle");
			Tooltip.SetDefault("Releases a glacial spike, repeatedly impaling the closest enemy"
							  + "\nHaving 3 or more active shamanic bonds increases the spike attack rate");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == Item.shoot && proj.owner == player.whoAmI)
				{
					proj.active = false;
				}
			}

			float speedYalt = new Vector2(speedX, speedY).Length();
			int newProj = this.NewShamanProjectile(position.X, position.Y, 0f, -1f * speedYalt, type, damage, knockBack, player.whoAmI);
			if (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod) > 2)
			{
				Main.projectile[newProj].ai[1] = 3;
			}
			Main.projectile[newProj].netUpdate = true;

			return false;
		}
	}
}
