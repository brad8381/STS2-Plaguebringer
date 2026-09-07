using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Character;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

/// <summary>
/// Base class for Plaguebringer cards and their portrait paths.
/// </summary>
[Pool(typeof(PlagueBringerCardPool))]
public abstract class PlagueBringerCard(
    int cost,
    CardType type,
    CardRarity rarity,
    TargetType target,
    bool shouldShowInCardLibrary = true) :
    ConstructedCardModel(cost, type, rarity, target, shouldShowInCardLibrary)
{
    private string PortraitFileName => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png";

    private static readonly Dictionary<Type, CardLoc> InlineLocalization = new()
    {
        [typeof(Bloodletting)] = new(
            "Bloodletting",
            "Remove up to {PlagueCap:diff()} [gold]Plague[/gold] from the target. Gain {BlockPerPlague} [gold]Block[/gold] for each Plague removed."),
        [typeof(Autopsy)] = new(
            "Autopsy",
            "Remove ALL [gold]Plague[/gold] from the target. Draw 1 card for every {PlaguePerCard} Plague removed, up to {MaxDraw:diff()} cards."),
        [typeof(SepticReflex)] = new(
            "Septic Reflex",
            "Deal {Damage:diff()} damage. If the target has at least {PlagueThreshold} [gold]Plague[/gold], draw 1 card."),
        [typeof(AcceleratedInfection)] = new(
            "Accelerated Infection",
            "Trigger the target's [gold]Plague[/gold] twice."),
        [typeof(Outbreak)] = new(
            "Outbreak",
            "Trigger [gold]Plague[/gold] on ALL enemies."),
        [typeof(SpentSample)] = new(
            "Spent Sample",
            "Remove {PlagueCost:diff()} [gold]Plague[/gold] from the target. If you do, gain {Energy} [gold]Energy[/gold]."),
        [typeof(Overdose)] = new(
            "Overdose",
            "Apply {Plague:diff()} [gold]Plague[/gold]. If the target was already infected, apply {BonusPlague:diff()} more."),
        [typeof(Triage)] = new(
            "Triage",
            "Gain {BaseBlock:diff()} [gold]Block[/gold] plus 1 Block for every {PlaguePerBlock} Plague on the target, up to +{MaxBonusBlock:diff()}."),
        [typeof(ContaminatedInstruments)] = new(
            "Contaminated Instruments",
            "Whenever you Exhaust a card, apply {PlaguePerExhaust} [gold]Plague[/gold] to ALL enemies."),
        [typeof(CarrionCloud)] = new(
            "Carrion Cloud",
            "When an infected enemy dies, apply {SpreadPercent:diff()}% of its remaining [gold]Plague[/gold], rounded up, to every living enemy."),
        [typeof(NoCure)] = new(
            "No Cure",
            "Whenever an enemy's [gold]Plague[/gold] grows, it grows by {BonusGrowth:diff()} additional Plague."),
        [typeof(QuarantineProtocol)] = new(
            "Quarantine Protocol",
            "Gain {BlockPerInfectedEnemy:diff()} [gold]Block[/gold] for each infected enemy."),
        [typeof(Reinfection)] = new(
            "Reinfection",
            "If the target has no [gold]Plague[/gold], apply {Plague:diff()} Plague. Otherwise, trigger its Plague."),
        [typeof(BurnTheEvidence)] = new(
            "Burn the Evidence",
            "Exhaust a card from your hand. Apply {Plague:diff()} [gold]Plague[/gold] to ALL enemies."),
        [typeof(SurgicalSweep)] = new(
            "Surgical Sweep",
            "Deal {Damage:diff()} damage to ALL enemies. Infected enemies take {InfectedBonusDamage:diff()} additional damage."),
        [typeof(SepticReserve)] = new(
            "Septic Reserve",
            "The first time each turn you remove [gold]Plague[/gold], gain {EnergyOnRemove} [gold]Energy[/gold].")
    };

    public override List<(string, string)>? Localization
    {
        get
        {
            if (!InlineLocalization.TryGetValue(GetType(), out var loc) || loc is null)
                return null;

            return loc;
        }
    }

    // BaseLib uses CustomPortraitPath for the actual card portrait texture.
    // STS2's high-resolution individual portraits are 1000x760, so use the
    // normal portrait instead of the tall 606x852 full-art image. Feeding the
    // tall image into the normal portrait slot makes it letterbox and look tiny.
    public override string CustomPortraitPath => PortraitFileName.CardImagePath();
    public override string PortraitPath => PortraitFileName.CardImagePath();
    public override string BetaPortraitPath => $"beta/{PortraitFileName}".CardImagePath();
}
