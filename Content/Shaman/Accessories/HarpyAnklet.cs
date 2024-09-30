using OrchidMod.Content.Items.Consumables;
using OrchidMod.Content.Shaman.Projectiles.Equipment;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Accessories
{
	public class HarpyAnklet : OrchidModShamanEquipable
	{
		public bool harpySpaceKeyReleased = false;
		public int jumpHeightCheck = -1;

		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 0, 11, 50);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
			Item.damage = 12;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Harpy Anklet");
			/* Tooltip.SetDefault("Releases a burst of feathers around you when using a double jump"
							  + "\nAllows you to double jump, if you cannot already"
							  + "\nDamage increased under the effect of cloud burst potion"
							 + "\nThese effects will only occur if you have an active shamanic air bond"); */
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidShaman shaman = player.GetModPlayer<OrchidShaman>();

			if (shaman.IsShamanicBondReleased(ShamanElement.AIR))
			{
				if (!player.controlJump) harpySpaceKeyReleased = true;

				if (!(player.GetJumpState(ExtraJump.CloudInABottle).Enabled || player.GetJumpState(ExtraJump.TsunamiInABottle).Enabled || player.GetJumpState(ExtraJump.SandstormInABottle).Enabled
				|| player.GetJumpState(ExtraJump.BlizzardInABottle).Enabled || player.GetJumpState(ExtraJump.FartInAJar).Enabled || player.GetJumpState(ExtraJump.UnicornMount).Enabled))
					player.GetJumpState(ExtraJump.CloudInABottle).Enable();

				if (player.velocity.Y == 0 || player.grappling[0] >= 0 && !player.controlJump)
				{
					if (player.GetJumpState(ExtraJump.CloudInABottle).Available)
					{
						jumpHeightCheck = (int)(Player.jumpHeight * 0.75);
					}
					if (player.GetJumpState(ExtraJump.TsunamiInABottle).Available)
					{
						jumpHeightCheck = (int)(Player.jumpHeight * 1.25);
					}
					if (player.GetJumpState(ExtraJump.FartInAJar).Available)
					{
						jumpHeightCheck = Player.jumpHeight * 2;
					}
					if (player.GetJumpState(ExtraJump.BlizzardInABottle).Available)
					{
						jumpHeightCheck = (int)(Player.jumpHeight * 1.5);
					}
					if (player.GetJumpState(ExtraJump.SandstormInABottle).Available)
					{
						jumpHeightCheck = Player.jumpHeight * 3;
					}
					if (player.GetJumpState(ExtraJump.UnicornMount).Available)
					{
						jumpHeightCheck = Player.jumpHeight * 2;
					}
				}

				if (player.GetJumpState(ExtraJump.CloudInABottle).Available && player.jump == (int)(Player.jumpHeight * 0.75))
					player.jump--;

				if ((player.jump == jumpHeightCheck && harpySpaceKeyReleased == true))
				{
					harpySpaceKeyReleased = false;
					int dmg = (int)player.GetDamage<ShamanDamageClass>().ApplyTo(22);
					if (player.FindBuffIndex(ModContent.BuffType<HarpyPotionBuff>()) > -1)
						dmg += (int)player.GetDamage<ShamanDamageClass>().ApplyTo(12);

					for (float dirX = -1; dirX < 2; dirX++)
					{
						for (float dirY = -1; dirY < 2; dirY++)
						{
							bool ankletCanShoot = !(dirX == 0 && dirY == 0 && dirX == dirY);
							float ankletSpeed = 10f;
							if (dirX != 0 && dirY != 0) ankletSpeed = 7.5f;
							if (ankletCanShoot)
							{
								Projectile.NewProjectile(null, player.Center.X, player.Center.Y, (dirX * ankletSpeed), (dirY * ankletSpeed), ModContent.ProjectileType<HarpyAnkletProj>(), dmg, 0.0f, player.whoAmI, 0.0f, 0.0f);
							}
						}
					}
				}
			}
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			if (Main.LocalPlayer.FindBuffIndex(ModContent.BuffType<HarpyPotionBuff>()) > -1)
				damage += 1.1f;
		}

		/*
		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(null, "HarpyTalon", 1);
			recipe.AddIngredient(ItemID.Feather, 4);
			recipe.Register();
		}
		*/
	}
}
