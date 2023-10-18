using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Guardian;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Guardian
{
	public abstract class OrchidModGuardianRune : OrchidModGuardianItem
	{
		public int cost;
		public int duration;
		public float distance;
		public int number;

		public virtual void Activate(Player player, OrchidGuardian guardian, int type, int damage, float knockback, int critChance, int duration, float distance, int number)
		{
			NewRuneProjectiles(player, guardian, duration, type, damage, knockback, critChance, distance, number);
		}

		public sealed override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<GuardianDamageClass>();
			Item.noMelee = true;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.maxStack = 1;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.shootSpeed = 00f;
			cost = 1;
			duration = 60;
			distance = 10f;
			number = 1;

			OrchidModGlobalItem orchidItem = Item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.guardianWeapon = true;

			this.SafeSetDefaults();
		}

		public sealed override void HoldItem(Player player)
		{
			var guardian = player.GetModPlayer<OrchidGuardian>();
			guardian.guardianDisplayUI = 300;
			guardian.slamCostUI = cost;
		}

		public override bool? UseItem(Player player)
		{
			var guardian = player.GetModPlayer<OrchidGuardian>();
			guardian.guardianSlam -= cost;
			foreach (Projectile projectile in guardian.runeProjectiles)
				projectile.Kill();
			Activate(player, guardian, Item.shoot, Item.damage, Item.knockBack, Item.crit + (int)player.GetCritChance<GuardianDamageClass>(), duration, distance, number);
			return true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		
		public override bool CanUseItem(Player player)
		{
			if (player.GetModPlayer<OrchidGuardian>().guardianSlam < cost)
				return false;
			return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return false;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.Text = damageValue + " opposing " + damageWord;
			}

			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback")); // "UseMana"
			tooltips.Insert(index + 1, new TooltipLine(Mod, "UseSlams", "Uses " + this.cost + " shield slams"));
		}

		public Projectile NewRuneProjectile(Player player, OrchidGuardian guardian, int duration, int type, int damage, float knockback, int critChance, float distance = 0f, float angle = 0f)
		{
			Projectile projectile = Main.projectile[Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, Vector2.Zero, type, damage, knockback, player.whoAmI, distance, angle)];
			projectile.timeLeft = duration;
			projectile.CritChance = critChance;
			projectile.netUpdate = true;
			guardian.runeProjectiles.Add(projectile);
			return projectile;
		}

		public List<Projectile> NewRuneProjectiles(Player player, OrchidGuardian guardian, int duration, int type, int damage, float knockback, int critChance, float distance, int number, float angle = 0f)
		{
			List<Projectile> projectiles = new List<Projectile>();

			for (int i = 0; i < number; i++)
				projectiles.Add(NewRuneProjectile(player, guardian, duration, type, damage, knockback, critChance, distance, angle + (360 / number) * i));
			return projectiles;
		}
	}
}
