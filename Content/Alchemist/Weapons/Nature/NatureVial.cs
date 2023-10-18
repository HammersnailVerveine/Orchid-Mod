using Microsoft.Xna.Framework;
using OrchidMod.Content.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Weapons.Nature
{
	public class NatureVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 5;
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.Blue;
			this.potencyCost = 1;
			this.element = AlchemistElement.NATURE;
			this.rightClickDust = 3;
			this.colorR = 75;
			this.colorG = 117;
			this.colorB = 0;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nature Vial");
			// Tooltip.SetDefault("\n[c/FF0000:Test Item]");
		}

		public override void KillSecond(int timeLeft, Player player, OrchidAlchemist modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int projType = ProjectileType<Projectiles.Sigil.NatureSigil>();
			Vector2 pos = new Vector2(projectile.Center.X, projectile.Center.Y - 50);
			SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), pos, Vector2.Zero, projType, 0, 3f, projectile.owner);
			player.HealEffect(10, true);
		}
	}
}
