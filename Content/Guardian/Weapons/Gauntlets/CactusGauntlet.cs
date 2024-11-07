using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class CactusGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 30;
			Item.knockBack = 2.8f;
			Item.damage = 33;
			Item.value = Item.sellPrice(0, 0, 3, 25);
			Item.rare = ItemRarityID.White;
			Item.useTime = 30;
			strikeVelocity = 14f;
			parryDuration = 50;
		}

		public override void OnParryNPC(Player player, OrchidGuardian guardian, NPC npc, Player.HurtInfo info)
		{
			int damage = guardian.GetGuardianDamage(Item.damage);
			int crit = guardian.GetGuardianCrit(Item.crit);
			player.ApplyDamageToNPC(npc, damage, Item.knockBack * 2f, player.direction, Main.rand.Next(100) < crit, ModContent.GetInstance<GuardianDamageClass>());
		}

		public override Color GetColor(bool offHand)
		{
			return new Color(169, 195, 41);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(ItemID.Cactus, 10);
			recipe.Register();
		}
	}
}
