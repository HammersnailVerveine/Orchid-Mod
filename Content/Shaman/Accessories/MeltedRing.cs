using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Accessories
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class MeltedRing : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 0, 55, 0);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
			Item.damage = 30;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			
			if (modPlayer.IsShamanicBondReleased(ShamanElement.AIR))
			{
				if (Main.rand.NextBool(20))
				{
					int damage = (int)player.GetDamage<ShamanDamageClass>().ApplyTo(Item.damage);
					int type = ModContent.ProjectileType<Projectiles.Equipment.LavaDroplet>();
					Vector2 position = player.position + new Vector2(Main.rand.NextFloat(player.width), player.height);
					Projectile.NewProjectile(Item.GetSource_FromThis(), position, Vector2.UnitY, type, damage, 0f, player.whoAmI);
				}
			}
		}
	}
}
