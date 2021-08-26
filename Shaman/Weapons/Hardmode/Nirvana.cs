using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class Nirvana : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 200;
			item.magic = true;
			item.width = 64;
			item.height = 64;
			item.useTime = 35;
			item.useAnimation = 35;
			item.knockBack = 6.15f;
			item.rare = 10;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.UseSound = SoundID.Item70;
			item.autoReuse = true;
			item.shootSpeed = 5f;
			item.shoot = mod.ProjectileType("NirvanaMain");
			this.empowermentType = 5;
			this.catalystType = ShamanCatalystType.ROTATE;
			this.energy = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nirvana");
			Tooltip.SetDefault("Shoots a bolt of elemental energy, calling all four elements upon impact"
							  + "\nIf you have 3 or more active shamanic bonds, more elements will be called");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}
