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
			Item.damage = 25;
			Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 25;
			Item.useTime = 25;
			this.distance = 40f;
			this.bashDistance = 110f;
			this.blockDuration = 100;
		}

		public override void Slam(Player player, Projectile shield)
		{
			Projectile anchor = GetAnchor(player).Projectile;
			int type = ModContent.ProjectileType<EnchantedPaviseProj>();
			Vector2 dir = anchor.Center - player.Center;
			dir.Normalize();
			dir *= 0.1f;
			Projectile.NewProjectile(Item.GetSource_FromThis(), anchor.Center, dir, type, (int)(Item.damage * 0.75f), Item.knockBack, player.whoAmI);
		}
	}
}
