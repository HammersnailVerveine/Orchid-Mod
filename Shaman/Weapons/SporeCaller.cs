using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class SporeCaller : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 6;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.knockBack = 3.15f;
			Item.rare = 3;
			Item.value = Item.sellPrice(0, 0, 54, 0);
			Item.UseSound = SoundID.Item43;
			Item.shootSpeed = 5f;
			Item.shoot = Mod.Find<ModProjectile>("SporeCallerProj").Type;
			this.empowermentType = 3;
			this.catalystType = ShamanCatalystType.ROTATE;
			this.energy = 25;
		}

		public override void SafeModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			mult *= player.GetModPlayer<OrchidModPlayer>().shamanDamage;
			if (Main.LocalPlayer.FindBuffIndex(Mod.Find<ModBuff>("SporeEmpowerment").Type) > -1)
				add += 2f;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Spore Caller");
			Tooltip.SetDefault("Spits out a stack of life-seeking spores, growing stronger with time"
							  + "\nOnly one stack of spores can be active at once"
							  + "\nThe number of spores depends on your number of active shamanic bonds"
							  + "\nIf the projectiles last for long enough before hitting an opponent, your next attack with this weapon will deal increased damage");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);

			player.ClearBuff(Mod.Find<ModBuff>("SporeEmpowerment").Type);

			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == Item.shoot && proj.owner == player.whoAmI)
				{
					proj.active = false;
				}
			}

			for (int i = 0; i < nbBonds + 2; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(30));
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				perturbedSpeed = perturbedSpeed * scale;
				this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);

			var tooltip = tooltips.Find(i => i.Name.Equals("Damage") && i.Mod == "Terraria");
			if (tooltip != null)
			{
				string[] split = tooltip.Text.Split(' ');
				if (Int32.TryParse(split[0], out int dmg2))
				{
					dmg2 += 45;
					split[0] = split[0] + " - " + dmg2;
					tooltip.Text = String.Join(" ", split);
				}
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(Mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.JungleSpores, 8);
			recipe.AddIngredient(ItemID.Stinger, 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
