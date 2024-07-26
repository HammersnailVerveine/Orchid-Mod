﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Standards
{
	public class IceStandard : OrchidModGuardianStandard
	{
		public override void SafeSetDefaults()
		{
			Item.width = 42;
			Item.height = 42;
			Item.value = Item.sellPrice(0, 0, 35, 75);
			Item.rare = ItemRarityID.Blue;
			Item.useTime = 35;
			Item.UseSound = SoundID.DD2_BetsyWindAttack;
			guardStacks = 1;
			flagOffset = 8;
			auraRange = 10;
			duration = 600;
			affectNearbyPlayers = true;
		}

		public override Color GetColor(bool offHand)
		{
			return new Color(106, 210, 255);
		}

		public override void NearbyPlayerEffect(Player player, OrchidGuardian guardian, bool isLocalPlayer, bool charged)
		{
			Main.NewText(charged);
		}
	}
}
