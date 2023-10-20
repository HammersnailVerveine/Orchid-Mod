namespace OrchidMod.Content.Prefixes
{
	// Accessory

	//public class NaturalPrefix : AccessoryPrefix { public NaturalPrefix() : base("Natural", 1, 0, 0) { } }
	public class SpiritualPrefix : AccessoryPrefix { public SpiritualPrefix() : base("Spiritual", 2, 0, 0) { } }
	public class BrewingPrefix : AccessoryPrefix { public BrewingPrefix() : base("Brewing", 0, 1, 0) { } }
	//public class CrookedPrefix : AccessoryPrefix { public CrookedPrefix() : base("Crooked", 0, 0, 1) { } }
	public class LoadedPrefix : AccessoryPrefix { public LoadedPrefix() : base("Loaded", 0, 0, 2) { } }

	// Shaman

	public class CursedPrefix : ShamanPrefix { public CursedPrefix() : base("Cursed", 0.85f, 1.00f, -1, 0, 0.90f) { } }
	public class PossessedPrefix : ShamanPrefix { public PossessedPrefix() : base("Possessed", 1.00f, 0.85f, 0, 0, 0.90f) { } }
	public class JinxedPrefix : ShamanPrefix { public JinxedPrefix() : base("Jinxed", 1.00f, 0.90f, -2, 0, 1.00f) { } }
	public class HexxedPrefix : ShamanPrefix { public HexxedPrefix() : base("Hexxed", 1.15f, 0.90f, 0, 1, 1.00f) { } }
	public class BewitchedPrefix : ShamanPrefix { public BewitchedPrefix() : base("Bewitched", 0.85f, 1.15f, 0, 0, 1.00f) { } }
	public class VoodooedPrefix : ShamanPrefix { public VoodooedPrefix() : base("Voodooed", 1.10f, 1.00f, 0, 3, 1.00f) { } }
	public class OccultPrefix : ShamanPrefix { public OccultPrefix() : base("Occult", 1.00f, 1.10f, 0, 0, 1.15f) { } }
	public class FocusedPrefix : ShamanPrefix { public FocusedPrefix() : base("Focused", 1.00f, 1.15f, 0, 0, 1.10f) { } }
	public class FerventPrefix : ShamanPrefix { public FerventPrefix() : base("Fervent", 1.00f, 1.00f, 2, 0, 1.05f) { } }
	public class SpiritedPrefix : ShamanPrefix { public SpiritedPrefix() : base("Spirited", 1.10f, 1.05f, 1, 2, 1.05f) { } }
	public class EffervescentPrefix : ShamanPrefix { public EffervescentPrefix() : base("Effervescent", 1.10f, 1.00f, 2, 0, 1.00f) { } }
	public class EtherealPrefix : ShamanPrefix { public EtherealPrefix() : base("Ethereal", 1.15f, 1.10f, 2, 5, 1.10f) { } }
}