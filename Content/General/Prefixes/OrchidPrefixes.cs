namespace OrchidMod.Content.General.Prefixes
{
	// Accessory

	public class BrewingPrefix : AccessoryPrefix { public BrewingPrefix() : base(1, 0, 0) { } }
	public class LoadedPrefix : AccessoryPrefix { public LoadedPrefix() : base(0, 2, 0) { } }
	public class BlockingPrefix : AccessoryPrefix { public BlockingPrefix() : base(0, 0, 1) { } }

	// Shaman - Damage, Bond Loading, Bond Duration, Critical Strike Chance, Velocity

	/*
	public class CursedPrefix : ShamanPrefix { public CursedPrefix() : base(0.85f, 1.00f, -1, 0, 0.90f) { } }
	public class PossessedPrefix : ShamanPrefix { public PossessedPrefix() : base(1.00f, 0.85f, 0, 0, 0.90f) { } }
	public class JinxedPrefix : ShamanPrefix { public JinxedPrefix() : base(1.00f, 0.90f, -2, 0, 1.00f) { } }
	public class HexxedPrefix : ShamanPrefix { public HexxedPrefix() : base(1.15f, 0.90f, 0, 1, 1.00f) { } }
	public class BewitchedPrefix : ShamanPrefix { public BewitchedPrefix() : base(0.85f, 1.15f, 0, 0, 1.00f) { } }
	public class VoodooedPrefix : ShamanPrefix { public VoodooedPrefix() : base(1.10f, 1.00f, 0, 3, 1.00f) { } }
	public class OccultPrefix : ShamanPrefix { public OccultPrefix() : base(1.00f, 1.10f, 0, 0, 1.15f) { } }
	public class FocusedPrefix : ShamanPrefix { public FocusedPrefix() : base(1.00f, 1.15f, 0, 0, 1.10f) { } }
	public class FerventPrefix : ShamanPrefix { public FerventPrefix() : base(1.00f, 1.00f, 2, 0, 1.05f) { } }
	public class SpiritedPrefix : ShamanPrefix { public SpiritedPrefix() : base(1.10f, 1.05f, 1, 2, 1.05f) { } }
	public class EffervescentPrefix : ShamanPrefix { public EffervescentPrefix() : base(1.10f, 1.00f, 2, 0, 1.00f) { } }
	public class EtherealPrefix : ShamanPrefix { public EtherealPrefix() : base(1.15f, 1.10f, 2, 5, 1.10f) { } }
	*/

	// Guardian - Damage, Knockback, Block Duration, Critical Strike Chance, Speed

	public class HaidexPrefix : GuardianPrefix { public HaidexPrefix() : base(1.00f, 1.00f, 0.85f, 20, 0.85f) { } } // Easter Egg
	public class FlimsyPrefix : GuardianPrefix { public FlimsyPrefix() : base(0.85f, 1.00f, 0.90f, 0, 0.90f) { } } // Bad
	public class FeeblePrefix : GuardianPrefixNoBlockDuration { public FeeblePrefix() : base(1.00f, 0.85f, 1.00f, 0, 0.90f) { } }
	public class FragilePrefix : GuardianPrefix { public FragilePrefix() : base(1.00f, 0.90f, 0.85f, 0, 1.00f) { } }
	public class ResolutePrefix : GuardianPrefixNoBlockDuration { public ResolutePrefix() : base(1.15f, 0.90f, 1.00f, 1, 1.00f) { } } // Mitigated
	public class StoutPrefix : GuardianPrefixNoBlockDuration { public StoutPrefix() : base(0.85f, 1.15f, 1f, 0, 1.00f) { } }
	public class UnyieldingPrefix : GuardianPrefixNoBlockDuration { public UnyieldingPrefix() : base(1.10f, 1.00f, 1.00f, 3, 1.00f) { } } // Good
	public class SturdyPrefix : GuardianPrefix { public SturdyPrefix() : base(1.00f, 1.10f, 1.15f, 0, 1.00f) { } }
	public class SteadfastPrefix : GuardianPrefixNoBlockDuration { public SteadfastPrefix() : base(1.00f, 1.15f, 1.00f, 0, 1.10f) { } }
	public class ImpregnablePrefix : GuardianPrefix { public ImpregnablePrefix() : base(1.00f, 1.15f, 1.10f, 0, 1.00f) { } }
	public class ToweringPrefix : GuardianPrefix { public ToweringPrefix() : base(1.00f, 1.00f, 1.15f, 0, 1.05f) { } }
	public class SpartanPrefix : GuardianPrefix { public SpartanPrefix() : base(1.10f, 1.05f, 1.1f, 2, 1.05f) { } } // Very good
	public class AngelicPrefix : GuardianPrefixNoBlockDuration { public AngelicPrefix() : base(1.15f, 1.00f, 1.00f, 5, 1.10f) { } }
	public class HulkingPrefix : GuardianPrefix { public HulkingPrefix() : base(1.15f, 1.05f, 1.15f, 0, 1.00f) { } }
	public class EmpyreanPrefix : GuardianPrefix { public EmpyreanPrefix() : base(1.15f, 1.10f, 1.15f, 5, 1.10f) { } }

	// Shapeshifter - Damage, Knockback, Attack Speed, Critical Strike Chance, Move Speed
	public class TimidPrefix : ShapeshifterPrefix { public TimidPrefix() : base(0.85f, 1.00f, -0.15f, 0, -0.05f) { } } // Bad
	public class BoarishPrefix : ShapeshifterPrefix { public BoarishPrefix() : base(1.00f, 0.85f, -0.10f, 0, -0f) { } }
	public class BullishPrefix : ShapeshifterPrefix { public BullishPrefix() : base(1.00f, 0.90f, -0.15f, 0, 0f) { } }
	public class EnragedPrefix : ShapeshifterPrefix { public EnragedPrefix() : base(1.15f, 0.90f, 0f, 1, 0f) { } } // Mitigated
	public class BestialPrefix : ShapeshifterPrefix { public BestialPrefix() : base(0.85f, 1.15f, 0f, 0, 0f) { } }
	public class VoraciousPrefix : ShapeshifterPrefix { public VoraciousPrefix() : base(1.10f, 1.00f, 0f, 3, 0f) { } } // Good
	public class UntamedPrefix : ShapeshifterPrefix { public UntamedPrefix() : base(1.00f, 1.10f, 0f, 0, 0.15f) { } }
	public class FiercePrefix : ShapeshifterPrefix { public FiercePrefix() : base(1.00f, 1.15f, 0f, 0, 0.10f) { } }
	public class FeralPrefix : ShapeshifterPrefix { public FeralPrefix() : base(1.00f, 1.00f, 0.15f, 0, 0.05f) { } }
	public class MonstrousPrefix : ShapeshifterPrefix { public MonstrousPrefix() : base(1.15f, 1.05f, 0.15f, 0, 0f) { } } // Very good 
	public class PrimalPrefix : ShapeshifterPrefix { public PrimalPrefix() : base(1.10f, 1.05f, 0.1f, 2, 0.05f) { } }
	public class DivinePrefix : ShapeshifterPrefix { public DivinePrefix() : base(1.15f, 1.10f, 0.15f, 5, 0.10f) { } }
}