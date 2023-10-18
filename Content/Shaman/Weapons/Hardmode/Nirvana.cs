using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Hardmode
{
	public class Nirvana : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 130;
			Item.width = 64;
			Item.height = 64;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.knockBack = 6.15f;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.UseSound = SoundID.Item70;
			Item.autoReuse = true;
			Item.shootSpeed = 5f;
			//Item.shoot = ModContent.ProjectileType<NirvanaMain>();
			this.Element = 5;
			this.catalystType = ShamanCatalystType.ROTATE;
			this.energy = 10;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Nirvana");
			/* Tooltip.SetDefault("Shoots a bolt of elemental energy, calling all four elements upon impact"
							  + "\nIf you have 3 or more active shamanic bonds, more elements will be called"); */
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}
