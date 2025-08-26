using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Shields
{
	public class ThoriumBronzeShield : OrchidModGuardianShield
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.width = 46;
			Item.height = 60;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item71.WithPitchOffset(-1f);
			Item.knockBack = 20f;
			Item.damage = 120;
			Item.rare = ItemRarityID.Orange;
			Item.useTime = 46;
			distance = 60f;
			slamDistance = 80f;
			blockDuration = 300;
		}

		/*public override void SlamHit(Player player, Projectile shield, NPC npc)
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				int buff = npc.FindBuffIndex(thoriumMod.Find<ModBuff>("Petrify").Type);
				if (buff != -1) npc.DelBuff(buff);
			}
		}*/

		float oldVelX;
		float oldVelY;
		float damageBonus;

		public override void BlockStart(Player player, Projectile shield)
		{
			oldVelX = player.velocity.X;
			oldVelY = player.velocity.Y;
			damageBonus = 0;
		}

		public override void ExtraAIShield(Projectile projectile)
		{
			Player player = Main.player[projectile.owner];
			OrchidGuardian guardian = player.GetModPlayer<OrchidGuardian>();
			if (projectile.ai[0] > 0)
			{
				oldVelX = player.velocity.X = MathHelper.Lerp(player.velocity.X, oldVelX, 0.85f);
				if (Math.Abs(oldVelX) > 10) oldVelX *= 0.9f;
				if (player.velocity.Y != 0)
				{
					player.velocity.Y = MathHelper.Lerp(player.velocity.Y, oldVelY, player.velocity.Y < oldVelY ? 0.5f : 0.3f);
				}
				oldVelY = player.velocity.Y;
				if (player.jump > 0) player.jump--;
				//damageBonus += 0.005f * guardian.GuardianGuardRecharge;
				guardian.GuardianGuardRecharge = 0;
				//player.GetDamage<GuardianDamageClass>() += damageBonus;
			}
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.WorkBenches);
				recipe.AddIngredient(thoriumMod, "BronzeAlloyFragments", 10);
				recipe.AddIngredient(ItemID.Wood, 10);
				recipe.Register();
			}
		}
	}
}
