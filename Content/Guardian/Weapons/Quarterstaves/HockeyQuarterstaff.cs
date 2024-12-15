using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian.Buffs.Debuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Quarterstaves
{
	public class HockeyQuarterstaff : OrchidModGuardianQuarterstaff
	{
		public override void SafeSetDefaults()
		{
			Item.width = 54;
			Item.height = 42;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.useTime = 10;
			ParryDuration = 60;
			Item.knockBack = 12f;
			Item.damage = 114;
			GuardStacks = 1;
			SingleSwing = true;
			SwingSpeed = 1.5f;
			JabSpeed = 1.3f;
			SwingDamage = 3f;
		}

		public override void OnHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, NPC.HitInfo hit, bool jabAttack, bool counterAttack)
		{
			if (!jabAttack && !counterAttack && target.knockBackResist > 0f)
			{
				Vector2 velocity = Vector2.UnitY.RotatedBy(projectile.ai[1]) * 20f;
				target.velocity = velocity;
				target.AddBuff(ModContent.BuffType<HockeyQuarterstaffDebuff>(), 60);
			}
		}

		public override void QuarterstaffModifyHitNPC(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, ref NPC.HitModifiers modifiers, bool jabAttack, bool counterAttack, bool firstHit)
		{
			if (!jabAttack && !counterAttack && target.knockBackResist > 0f && firstHit && target.life > 1)
			{
				modifiers.SetMaxDamage(target.life < 10 ? 1 : target.life - 10);
			}
		}
	}
}
