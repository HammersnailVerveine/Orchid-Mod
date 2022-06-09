using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class TheCore : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 100;
			Item.magic = true;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.knockBack = 4.15f;
			Item.rare = 10;
			Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 10f;
			Item.shoot = Mod.Find<ModProjectile>("TheCoreProj").Type;
			this.empowermentType = 4;
			this.energy = 8;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("The Core");
			Tooltip.SetDefault("Shoots life-seeking essence bolts"
							  + "\nThe number of projectiles depends on the number of active shamanic bonds"
							  + "\n'You can feel heartbeats emanating from the staff'");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int BuffsCount = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);

			int numberProjectiles = 2 + BuffsCount;

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
				this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
	}
}
