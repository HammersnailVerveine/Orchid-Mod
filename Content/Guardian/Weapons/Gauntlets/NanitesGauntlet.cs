using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class NanitesGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 42;
			Item.height = 38;
			Item.knockBack = 5f;
			Item.damage = 415;
			Item.value = Item.sellPrice(0, 7, 50, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.useTime = 10;
			Item.crit = 10;
			strikeVelocity = 30f;
			parryDuration = 90;
			hasArm = true;
			hasShoulder = true;
		}

		public override Color GetColor(bool offHand)
		{
			return new Color(0, 255, 255);
		}

		public override void SafeHoldItem(Player player)
		{
			if (player.mount.Type == MountID.None)
			{
				Vector2 intendedVelocity = player.velocity * 0.05f;
				Vector2 addedVelocity = Vector2.Zero;

				for (int i = 0; i < 10; i++)
					addedVelocity += Collision.TileCollision(player.position + addedVelocity, intendedVelocity, player.width, player.height, false, false, (int)player.gravDir);

				if (addedVelocity.Length() > 0.1f)
				{
					player.position += addedVelocity;
				}
			}
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
