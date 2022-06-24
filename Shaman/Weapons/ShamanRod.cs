using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
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

		public override void SafeModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();

			if (modPlayer.GetNbShamanicBonds() > 1) 
				damage *= modPlayer.shamanDamage * 2f;
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			RemoveAllShamanRodProjs(player);
			for (int i = 0; i < 3; i++)
				this.NewShamanProjectile(player, source, position, velocity, type, damage, knockback, ai1: i + 1);

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
