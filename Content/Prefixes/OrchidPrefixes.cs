namespace OrchidMod.Content.Prefixes
{
	// Accessory

	public class SpiritualPrefix : AccessoryPrefix { public SpiritualPrefix() : base(2, 0, 0, 0) { } }
	public class BrewingPrefix : AccessoryPrefix { public BrewingPrefix() : base(0, 1, 0, 0) { } }
	public class LoadedPrefix : AccessoryPrefix { public LoadedPrefix() : base(0, 0, 2, 0) { } }
	public class BlockingPrefix : AccessoryPrefix { public BlockingPrefix() : base(0, 0, 0, 1) { } }

	// Shaman - Damage, Bond Loading, Bond Duration, Critical Strike Chance, Velocity

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

	// Guardian - Damage, Knockback, Block Duration, Critical Strike Chance, Slam Distance

	public class FlimsyPrefix : GuardianPrefix { public FlimsyPrefix() : base(0.85f, 1.00f, 0.90f, 0, 0.90f) { } }
	public class FeeblePrefix : GuardianPrefix { public FeeblePrefix() : base(1.00f, 0.85f, 1.00f, 0, 0.90f) { } }
	public class FragilePrefix : GuardianPrefix { public FragilePrefix() : base(1.00f, 0.90f, 0.85f, 0, 1.00f) { } }
	public class ResolutePrefix : GuardianPrefix { public ResolutePrefix() : base(1.15f, 0.90f, 1f, 1, 1.00f) { } }
	public class StoutPrefix : GuardianPrefix { public StoutPrefix() : base(0.85f, 1.15f, 1f, 0, 1.00f) { } }
	public class UnyieldingPrefix : GuardianPrefix { public UnyieldingPrefix() : base(1.10f, 1.00f, 1f, 3, 1.00f) { } }
	public class SturdyPrefix : GuardianPrefix { public SturdyPrefix() : base(1.00f, 1.10f, 1f, 0, 1.15f) { } }
	public class SteadfastPrefix : GuardianPrefix { public SteadfastPrefix() : base(1.00f, 1.15f, 1f, 0, 1.10f) { } }
	public class ToweringPrefix : GuardianPrefix { public ToweringPrefix() : base(1.00f, 1.00f, 1.15f, 0, 1.05f) { } }
	public class SpartanPrefix : GuardianPrefix { public SpartanPrefix() : base(1.10f, 1.05f, 1.1f, 2, 1.05f) { } }
	public class HulkingPrefix : GuardianPrefix { public HulkingPrefix() : base(1.10f, 1.00f, 1.15f, 0, 1.00f) { } }
	public class EmpyreanPrefix : GuardianPrefix { public EmpyreanPrefix() : base(1.15f, 1.10f, 1.15f, 5, 1.10f) { } }
}