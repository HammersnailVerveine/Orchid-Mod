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

		public override bool NewPreReforge(Item item)
		{
			pDamage = 0;
			pMana = 0;
			pUseTime = 0;
			pVelocity = 0;
			pKnockback = 0;
			return base.NewPreReforge(item);
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
							return mod.PrefixType("Voodoo");        //  /	 /     /     /     + // Keen
						case 1:
							return mod.PrefixType("Superior");      //  +	 +     /     /     /
						case 2:
							return mod.PrefixType("Forceful");      //  /	 +     /     /     /
						case 3:
							return mod.PrefixType("Broken");        //  -	 -     /     /     /
						case 4:
							return mod.PrefixType("Damaged");       //  -	 /     /     /     /
						case 5:
							return mod.PrefixType("Shoddy");        //  -	 /     /     /     /
						case 6:
							return mod.PrefixType("Hurtful");       //  +	 /     /     /     /
						case 7:
							return mod.PrefixType("Strong");        //  /	 +     /     /     /
						case 8:
							return mod.PrefixType("Unpleasant");    //  +	 +     /     /     /
						case 9:
							return mod.PrefixType("Weak");          //  /	 -     /     /     /
						case 10:
							return mod.PrefixType("Ruthless");      //  +	 -     /     /     /
						case 11:
							return mod.PrefixType("Occult");        //  +	 +     /     /     + // Godly
						case 12:
							return mod.PrefixType("Diabolic");      //  +	 /     /     /     + // Demonic
						case 13:
							return mod.PrefixType("Spirited");      //  /	 /     /     /     + // Zealous
						case 14:
							return mod.PrefixType("Quick");         //  /	 /     /     +     /
						case 15:
							return mod.PrefixType("Deadly");        //  +	 /     /     +     /
						case 16:
							return mod.PrefixType("Magnetic");      //  /	 /     /     +     + // Agile
						case 17:
							return mod.PrefixType("Nimble");        //  /	 /     /     +     /
						case 18:
							return mod.PrefixType("Runic");         //  +	 /     /     +     + // Murderous
						case 19:
							return mod.PrefixType("Slow");          //  /	 /     /     -     /
						case 20:
							return mod.PrefixType("Sluggish");      //  /	 /     /     -     /
						case 21:
							return mod.PrefixType("Lazy");          //  /	 /     /     -     /
						case 22:
							return mod.PrefixType("Annoying");      //  -	 /     /     -     /
						case 23:
							return mod.PrefixType("Conjuring");     //  +	 +     /     -     + // Nasty
						case 24:
							return mod.PrefixType("Studious");      //  +	 /     +     /     /
						case 25:
							return mod.PrefixType("Unique");        //  +	 +     +     /     /
						case 26:
							return mod.PrefixType("Balanced");      //  /	 +     -     +     /
						case 27:
							return mod.PrefixType("Hopeful");       //  /	 /     +     /     /
						case 28:
							return mod.PrefixType("Enraged");       //  +	 +     -     /     /
						case 29:
							return mod.PrefixType("Effervescent");  //  +	 +     +     -     /
						case 30:
							return mod.PrefixType("Ethereal");      //  +	 +     +     +     +
						case 31:
							return mod.PrefixType("Focused");       //  +	 /     +     /     /
						case 32:
							return mod.PrefixType("Complex");       //  -	 /     +     +     /
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
