using Microsoft.Xna.Framework;
using OrchidMod.Common.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	public class AbyssionScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			Item.damage = 250;
			Item.width = 50;
			Item.height = 50;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.knockBack = 4.25f;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 8f;
			Item.shoot = Mod.Find<ModProjectile>("AbyssionScepterProj").Type;
			this.empowermentType = 2;
			this.energy = 3;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Will of the Lurker");
			Tooltip.SetDefault("Summons bolts of dark energy that lingers for a while before returning to you"
							+ "\nCosts 25 health to cast, reduced by 5 for each active shamanic bond");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			int damageCost = 25 - OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod) * 5;
			if (player.statLife - damageCost > 0)
			{
				player.statLife -= damageCost;
			}
			else
			{
				player.statLife = 1;
			}

			return true;
		}
	}
}

