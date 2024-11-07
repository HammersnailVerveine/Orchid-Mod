using OrchidMod.Common;
using OrchidMod.Common.Attributes;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian
{
	[ClassTag(ClassTags.Guardian)]
	public abstract class OrchidModGuardianItem : ModItem
	{
		public bool IsLocalPlayer(Player player) => player.whoAmI == Main.myPlayer;

		public virtual void SafeSetDefaults() { }

		public override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<GuardianDamageClass>();
			Item.noMelee = true;
			Item.maxStack = 1;
			SafeSetDefaults();
		}

		protected override bool CloneNewInstances => true;

		public override bool CanUseItem(Player player)
		{
			//OrchidPlayer modPlayer = player.GetModPlayer<OrchidPlayer>();
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
		}
	}

	public abstract class OrchidModGuardianParryItem : OrchidModGuardianItem
	{
		public virtual void OnParry(Player player, OrchidGuardian guardian, Player.HurtInfo info) { }
		public virtual void OnParryNPC(Player player, OrchidGuardian guardian, NPC npc, Player.HurtInfo info) { }
		public virtual void OnParryProjectile(Player player, OrchidGuardian guardian,  Projectile projectile, Player.HurtInfo info) { }
	}
}
