using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Water
{
	public class WaterVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 5;
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.Blue;
			this.potencyCost = 1;
			this.element = AlchemistElement.WATER;
			this.rightClickDust = 29;
			this.colorR = 1;
			this.colorG = 139;
			this.colorB = 252;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Water Vial");
			// Tooltip.SetDefault("\n[c/FF0000:Test Item]");
		}

		public override void KillSecond(int timeLeft, Player player, OrchidAlchemist modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int projType = ProjectileType<Projectiles.Sigil.WaterSigil>();
			Vector2 pos = new Vector2(projectile.Center.X, projectile.Center.Y - 50);
			SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), pos, Vector2.Zero, projType, 0, 3f, projectile.owner);
			player.HealEffect(10, true);
		}
	}
}
