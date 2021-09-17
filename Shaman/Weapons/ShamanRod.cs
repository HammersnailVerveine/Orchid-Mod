using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class ShamanRod : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 16;
			item.width = 42;
			item.height = 42;
			item.useTime = 20;
			item.useAnimation = 20;
			item.knockBack = 0f;
			item.rare = ItemRarityID.Green;
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.UseSound = SoundID.Item65;
			item.shootSpeed = 7f;
			item.shoot = ModContent.ProjectileType<Projectiles.ShamanRodProj>();
			empowermentType = 4;
			this.energy = 35;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Shaman Rod");
			Tooltip.SetDefault("Shoots lingering razor-sharp leaves" +
							   "\nOnly one set can be active at once" +
							   "\nHaving 2 or more active shamanic bonds increases damage and slows on hit");
		}

		public override void SafeModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			if (OrchidModShamanHelper.getNbShamanicBonds(player, player.GetModPlayer<OrchidModPlayer>(), mod) > 1) mult *= modPlayer.shamanDamage * 2f;
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			RemoveAllShamanRodProjs(player);
			for (int i = 0; i < 3; i++) this.NewShamanProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, ai1: i + 1);

			return false;
		}

		public static void RemoveAllShamanRodProjs(Player owner)
		{
			HashSet<int> shamanRods = new HashSet<int>()
			{
				ModContent.ProjectileType<Projectiles.ShamanRodProj>(),
				ModContent.ProjectileType<Projectiles.HellShamanRodProj>()
			};

			foreach (var elem in Main.projectile)
			{
				if (elem.active && shamanRods.Contains(elem.type) && elem.owner == owner.whoAmI) elem.Kill();
			}
		}
	}
}
