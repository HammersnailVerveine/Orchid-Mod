using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class PaladinGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 38;
			Item.height = 40;
			Item.knockBack = 10f;
			Item.damage = 392;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.useTime = 30;
			strikeVelocity = 25f;
			parryDuration = 120;
		}

		public override Color GetColor(bool offHand)
		{
			return new Color(238, 218, 122);
		}

		public override void SafeHoldItem(Player player)
		{
			Vector2 intendedVelocity = player.velocity * 0.05f;
			Vector2 addedVelocity = Vector2.Zero;

			for (int i = 0; i < 10; i++)
				addedVelocity += Collision.TileCollision(player.position + addedVelocity, intendedVelocity, player.width, player.height, false, false, (int)player.gravDir);

			player.position += addedVelocity;
		}

		public override void OnHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, NPC.HitInfo hit, bool charged)
		{
			guardian.modPlayer.TryHeal(charged ? 5 : 2);
		}

		public override void OnParry(Player player, OrchidGuardian guardian, Player.HurtInfo info)
		{
			guardian.modPlayer.TryHeal(5);
		}
	}
}
