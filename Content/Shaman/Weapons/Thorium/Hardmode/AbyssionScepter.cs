using Microsoft.Xna.Framework;
using OrchidMod.Common.Attributes;
using OrchidMod.Content.Shaman.Projectiles.Thorium;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Thorium.Hardmode
{
	[CrossmodContent("ThoriumMod")]
	public class AbyssionScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 250;
			Item.width = 50;
			Item.height = 50;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.knockBack = 4.25f;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 8f;
			//Item.shoot = ModContent.ProjectileType<AbyssionScepterProj>();
			this.Element = ShamanElement.WATER;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Will of the Lurker");
			/* Tooltip.SetDefault("Summons bolts of dark energy that lingers for a while before returning to you"
							+ "\nCosts 25 health to cast, reduced by 5 for each active shamanic bond"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();

			int damageCost = 25 - modPlayer.CountShamanicBonds() * 5;
			if (player.statLife - damageCost > 0)
				player.statLife -= damageCost;
			else
				player.statLife = 1;

			return true;
		}
	}
}

