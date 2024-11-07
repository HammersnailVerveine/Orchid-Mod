using Microsoft.Xna.Framework;
using OrchidMod.Common.Global.Items;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Guardian
{
	public abstract class OrchidModGuardianHammer : OrchidModGuardianItem
	{
		public int Range;
		public int SlamStacks;
		public int BlockStacks;
		public bool Penetrate;
		public bool TileCollide;
		public float ReturnSpeed;
		public virtual void OnMeleeHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit) { }
		public virtual void OnMeleeHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit) { }
		public virtual void OnThrowHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool Weak) { }
		public virtual void OnThrowHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool Weak) { }
		public virtual void ExtraAI(Player player, OrchidGuardian guardian, Projectile projectile) { }
		public virtual bool ThrowAI(Player player, OrchidGuardian guardian, Projectile projectile, bool Weak) => true;

		public sealed override void SetDefaults()
		{
			Item.DamageType = GetInstance<GuardianDamageClass>();
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.maxStack = 1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 30;
			Item.knockBack = 10f;
			Item.shootSpeed = 10f;
			Range = 0;
			Penetrate = false;
			TileCollide = true;
			SlamStacks = 0;
			ReturnSpeed = 1f;

			OrchidGlobalItemPerEntity orchidItem = Item.GetGlobalItem<OrchidGlobalItemPerEntity>();
			orchidItem.guardianWeapon = true;

			SafeSetDefaults();

			Item.useAnimation = Item.useTime;
		}

		public override bool MeleePrefix() => true;

		public sealed override void HoldItem(Player player)
		{
			var guardian = player.GetModPlayer<OrchidGuardian>();
			guardian.GuardianDisplayUI = 300;
		}

		public override bool? UseItem(Player player)
		{
			var guardian = player.GetModPlayer<OrchidGuardian>();
			int projType = ProjectileType<GuardianHammerAnchor>();

			int damage = guardian.GetGuardianDamage(Item.damage);
			Projectile projectile = Projectile.NewProjectileDirect(Item.GetSource_FromThis(), player.Center, Vector2.Zero, projType, damage, Item.knockBack, player.whoAmI);
			projectile.CritChance = (int)(player.GetCritChance<GuardianDamageClass>() + player.GetCritChance<GenericDamageClass>() + Item.crit);

			guardian.GuardianHammerCharge = 0f;
			return true;
		}
		
		public override bool CanUseItem(Player player)
		{
			int projType = ProjectileType<GuardianHammerAnchor>();
			if (player.ownedProjectileCounts[projType] > 0) return false;
			return base.CanUseItem(player);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				tt.Text = damageValue + " " + Language.GetTextValue(ModContent.GetInstance<OrchidMod>().GetLocalizationKey("DamageClasses.GuardianDamageClass.DisplayName"));
			}

			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
			if (BlockStacks > 0) {
				tooltips.Insert(index + 1, new TooltipLine(Mod, "ShieldStacks", "Grants " + this.BlockStacks + " guard charge" + (this.BlockStacks > 1 ? "s" : ""))
				{
					OverrideColor = new Color(175, 255, 175)
				});
			}

			if (SlamStacks > 0)
			{
				tooltips.Insert(index + 1, new TooltipLine(Mod, "ShieldSlams", "Grants " + this.SlamStacks + " slam charge" + (this.SlamStacks > 1 ? "s" : ""))
				{
					OverrideColor = new Color(175, 255, 175)
				});
			}

			tooltips.Insert(index + 1, new TooltipLine(Mod, "Swing", "Charge to throw, right click to swing while charging")
			{
				OverrideColor = new Color(175, 255, 175)
			});
		}
	}
}
