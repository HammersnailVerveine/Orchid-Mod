using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Warhammers
{
	public class Warhammer : OrchidModGuardianHammer
	{

		public override void SafeSetDefaults()
		{
			Item.width = 36;
			Item.height = 36;
			Item.value = Item.sellPrice(0, 0, 0, 50);
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.knockBack = 5f;
			Item.shootSpeed = 8f;
			Item.damage = 27;
			this.range = 20;
			this.blockStacks = 1;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Warhammer");
			// Tooltip.SetDefault("Hurls a heavy hammer");
		}

		public override bool ThrowAI(Player player, OrchidGuardian guardian, bool weak)
		{
			return true;
		}

		public override void OnThrowHit(Player player, OrchidGuardian guardian, NPC target, float knockback, bool crit, bool Weak)
		{
		}

		public override void OnThrowHitFirst(Player player, OrchidGuardian guardian, NPC target, float knockback, bool crit, bool Weak)
		{
		}
	}
}
