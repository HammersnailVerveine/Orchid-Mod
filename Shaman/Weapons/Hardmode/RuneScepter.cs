using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Hardmode
{
    public class RuneScepter : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 43;
			item.width = 30;
			item.height = 30;
			item.useTime = 18;
			item.useAnimation = 18;
			item.knockBack = 3f;
			item.rare = 5;
			item.value = Item.sellPrice(0, 3, 10, 0);
			item.UseSound = SoundID.Item43;
			item.autoReuse = true;
			item.shootSpeed = 13f;
			item.shoot = mod.ProjectileType("RuneScepterProj");
			this.empowermentType = 1;
			OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeaponNoVelocityReforge = true;
			this.energy = 10;
		}
		
		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Rune Scepter");
		  Tooltip.SetDefault("Shoots short ranged rune bolts"
							+"\nProjectile range and damage scales with the number of active shamanic bonds");
		}
		
		public override void SafeModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			mult *= modPlayer.shamanDamage + (nbBonds * 0.1f);
		}
		
		public override void UpdateInventory(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			item.shootSpeed = 13f + (2f * nbBonds);
		}
    }
}
