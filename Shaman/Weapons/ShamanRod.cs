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
			Item.damage = 16;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.knockBack = 0f;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.UseSound = SoundID.Item65;
			Item.shootSpeed = 7f;
			Item.shoot = ModContent.ProjectileType<Projectiles.ShamanRodProj>();
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

			if (OrchidModShamanHelper.getNbShamanicBonds(player, player.GetModPlayer<OrchidModPlayer>(), Mod) > 1) mult *= modPlayer.shamanDamage * 2f;
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
