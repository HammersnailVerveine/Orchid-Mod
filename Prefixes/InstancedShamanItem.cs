using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace OrchidMod.Prefixes
{
	public class InstancedShamanItem : GlobalItem
	{
		public float pDamage;
		public float pMana;
		public float pUseTime;
		public float pVelocity;
		public float pKnockback;

		public InstancedShamanItem()
		{
			pDamage = 0;
			pMana = 0;
			pUseTime = 0;
			pVelocity = 0;
			pKnockback = 0;
		}

		public override bool InstancePerEntity => true;

		public override GlobalItem Clone(Item item, Item itemClone)
		{
			InstancedShamanItem myClone = (InstancedShamanItem)base.Clone(item, itemClone);
			myClone.pDamage = pDamage;
			myClone.pMana = pMana;
			myClone.pUseTime = pUseTime;
			myClone.pVelocity = pVelocity;
			myClone.pKnockback = pKnockback;

			return myClone;
		}

		public override bool PreReforge(Item item)
		{
			pDamage = 0;
			pMana = 0;
			pUseTime = 0;
			pVelocity = 0;
			pKnockback = 0;
			return base.PreReforge(item);
		}

		public override int ChoosePrefix(Item item, UnifiedRandom rand)
		{
			if (item.damage > 0 && !item.accessory && item.type != 0)
			{
				OrchidModGlobalItem modItem = item.GetGlobalItem<OrchidModGlobalItem>();
				if (modItem.shamanWeapon)
				{

					int randValue = rand.Next(35);

					if (modItem.shamanWeaponNoUsetimeReforge)
					{
						while ((randValue >= 14 && randValue <= 23) || (randValue == 26) || (randValue == 29) || (randValue == 30) || (randValue == 32))
						{
							randValue = rand.Next(33);
						}
					}

					if (modItem.shamanWeaponNoVelocityReforge)
					{
						while ((randValue >= 11 && randValue <= 13) || (randValue == 0) || (randValue == 16) || (randValue == 18) || (randValue == 23) || (randValue == 30))
						{
							randValue = rand.Next(33);
						}
					}

					switch (randValue)
					{                           // DMG - KNB - MNA - USE - VEL
						case 0:
							return Mod.Find<ModPrefix>("Voodoo").Type;        //  /	 /     /     /     + // Keen
						case 1:
							return Mod.Find<ModPrefix>("Superior").Type;      //  +	 +     /     /     /
						case 2:
							return Mod.Find<ModPrefix>("Forceful").Type;      //  /	 +     /     /     /
						case 3:
							return Mod.Find<ModPrefix>("Broken").Type;        //  -	 -     /     /     /
						case 4:
							return Mod.Find<ModPrefix>("Damaged").Type;       //  -	 /     /     /     /
						case 5:
							return Mod.Find<ModPrefix>("Shoddy").Type;        //  -	 /     /     /     /
						case 6:
							return Mod.Find<ModPrefix>("Hurtful").Type;       //  +	 /     /     /     /
						case 7:
							return Mod.Find<ModPrefix>("Strong").Type;        //  /	 +     /     /     /
						case 8:
							return Mod.Find<ModPrefix>("Unpleasant").Type;    //  +	 +     /     /     /
						case 9:
							return Mod.Find<ModPrefix>("Weak").Type;          //  /	 -     /     /     /
						case 10:
							return Mod.Find<ModPrefix>("Ruthless").Type;      //  +	 -     /     /     /
						case 11:
							return Mod.Find<ModPrefix>("Occult").Type;        //  +	 +     /     /     + // Godly
						case 12:
							return Mod.Find<ModPrefix>("Diabolic").Type;      //  +	 /     /     /     + // Demonic
						case 13:
							return Mod.Find<ModPrefix>("Spirited").Type;      //  /	 /     /     /     + // Zealous
						case 14:
							return Mod.Find<ModPrefix>("Quick").Type;         //  /	 /     /     +     /
						case 15:
							return Mod.Find<ModPrefix>("Deadly").Type;        //  +	 /     /     +     /
						case 16:
							return Mod.Find<ModPrefix>("Magnetic").Type;      //  /	 /     /     +     + // Agile
						case 17:
							return Mod.Find<ModPrefix>("Nimble").Type;        //  /	 /     /     +     /
						case 18:
							return Mod.Find<ModPrefix>("Runic").Type;         //  +	 /     /     +     + // Murderous
						case 19:
							return Mod.Find<ModPrefix>("Slow").Type;          //  /	 /     /     -     /
						case 20:
							return Mod.Find<ModPrefix>("Sluggish").Type;      //  /	 /     /     -     /
						case 21:
							return Mod.Find<ModPrefix>("Lazy").Type;          //  /	 /     /     -     /
						case 22:
							return Mod.Find<ModPrefix>("Annoying").Type;      //  -	 /     /     -     /
						case 23:
							return Mod.Find<ModPrefix>("Conjuring").Type;     //  +	 +     /     -     + // Nasty
						case 24:
							return Mod.Find<ModPrefix>("Studious").Type;      //  +	 /     +     /     /
						case 25:
							return Mod.Find<ModPrefix>("Unique").Type;        //  +	 +     +     /     /
						case 26:
							return Mod.Find<ModPrefix>("Balanced").Type;      //  /	 +     -     +     /
						case 27:
							return Mod.Find<ModPrefix>("Hopeful").Type;       //  /	 /     +     /     /
						case 28:
							return Mod.Find<ModPrefix>("Enraged").Type;       //  +	 +     -     /     /
						case 29:
							return Mod.Find<ModPrefix>("Effervescent").Type;  //  +	 +     +     -     /
						case 30:
							return Mod.Find<ModPrefix>("Ethereal").Type;      //  +	 +     +     +     +
						case 31:
							return Mod.Find<ModPrefix>("Focused").Type;       //  +	 /     +     /     /
						case 32:
							return Mod.Find<ModPrefix>("Complex").Type;       //  -	 /     +     +     /
						default:
							break;
					}
				}
			}
			return -1;
		}

		public override void NetSend(Item item, BinaryWriter writer)
		{
			writer.Write(pDamage);
			writer.Write(pKnockback);
			writer.Write(pUseTime);
			writer.Write(pMana);
			writer.Write(pVelocity);
		}

		public override void NetReceive(Item item, BinaryReader reader)
		{
			pDamage = reader.ReadSingle();
			pKnockback = reader.ReadSingle();
			pUseTime = reader.ReadSingle();
			pMana = reader.ReadSingle();
			pVelocity = reader.ReadSingle();
		}
	}
}
