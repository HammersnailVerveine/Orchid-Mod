using Microsoft.Xna.Framework;
using OrchidMod.Content.General.Prefixes;
using OrchidMod.Content.Guardian.Buffs;
using OrchidMod.Content.Guardian.Projectiles.Gauntlets;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class SpiderGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 38;
			Item.height = 44;
			Item.knockBack = 4.5f;
			Item.damage = 148;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.useTime = 25;
			Item.crit = 6;
			strikeVelocity = 20f;
			parryDuration = 60;
		}

		public override Color GetColor(bool offHand)
		{
			if (offHand) return new Color(163, 121, 109);
			return new Color(224, 109, 54);
		}

		public override void OnParryGauntlet(Player player, OrchidGuardian guardian, Entity aggressor, Projectile anchor)
		{
			player.AddBuff(ModContent.BuffType<GuardianSpiderGauntletBuff>(), 480);
		}

		public override bool OnPunch(Player player, OrchidGuardian guardian, Projectile projectile, bool charged, ref int damage)
		{
			if (player.HasBuff<GuardianSpiderGauntletBuff>())
			{
				int projectileType = ModContent.ProjectileType<SpiderGauntletProjectile>();
				float speed = strikeVelocity * (charged ? 1f : 0.75f) * Item.GetGlobalItem<GuardianPrefixItem>().GetSlamDistance() * Main.rand.NextFloat(0.85f, 1.15f);
				Vector2 velocity = Vector2.UnitY.RotatedBy((Main.MouseWorld - player.Center).ToRotation() - MathHelper.PiOver2).RotatedByRandom(MathHelper.ToRadians(5));
				int spikeDamage = (int)(guardian.GetGuardianDamage(Item.damage) * (charged ? 1.15f : 0.4f));
				Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center, velocity * speed, projectileType, spikeDamage, Item.knockBack, player.whoAmI, charged ? 1f : 0f);
				newProjectile.CritChance = (int)(player.GetCritChance<GuardianDamageClass>() + player.GetCritChance<GenericDamageClass>() + Item.crit);
				newProjectile.position += newProjectile.velocity * 0.5f;
				newProjectile.rotation = newProjectile.velocity.ToRotation();
				newProjectile.netUpdate = true;

				if (charged)
				{
					SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, player.Center);
				}
				else SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundMiss, player.Center);
				return false;
			}
			return true;
		}

		public override void OnHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, NPC.HitInfo hit, bool charged)
		{
			if (Main.rand.NextBool(5) || charged) target.AddBuff(BuffID.Venom, 240 + Main.rand.Next(120));
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.SpiderFang, 16);
			recipe.Register();
		}
	}
}
