using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian.Projectiles.Shields;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Shields
{
	public class EnchantedPavise : OrchidModGuardianShield
	{

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 87, 50);
			Item.width = 28;
			Item.height = 38;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.knockBack = 7f;
			Item.damage = 29;
			Item.rare = ItemRarityID.Blue;
			Item.useTime = 35;
			distance = 40f;
			slamDistance = 55f;
			blockDuration = 100;
		}

		public override void Slam(Player player, Projectile shield)
		{
			Projectile anchor = GetAnchor(player).Projectile;
			int type = ModContent.ProjectileType<EnchantedPaviseProj>();
			Vector2 dir = Vector2.Normalize(Main.MouseWorld - player.Center) * 0.1f;
			Projectile.NewProjectile(Item.GetSource_FromThis(), anchor.Center, dir, type, (int)(shield.damage * 0.75f), Item.knockBack, player.whoAmI);
		}
	}
}
