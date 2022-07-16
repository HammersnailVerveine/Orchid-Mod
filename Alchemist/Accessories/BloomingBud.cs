using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Accessories
{
	public class BloomingBud : OrchidModAlchemistEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
			Item.damage = 25;
		}

		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Blooming Bud");
			Tooltip.SetDefault("Periodically releases catalytic blooming buds when attacking");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			if (modPlayer.alchemistPotencyDisplayTimer > 0 && Main.rand.NextBool(600))
			{
				int dmg = (int)player.GetDamage<AlchemistDamageClass>().ApplyTo(25);
				Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(20));
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				perturbedSpeed = perturbedSpeed * scale;
				Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center.X, player.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<Alchemist.Projectiles.Reactive.FlowerReactive>(), dmg, 0, player.whoAmI, 0f, 0f);
			}
		}
	}
}