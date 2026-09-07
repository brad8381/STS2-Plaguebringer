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

    // BaseLib uses CustomPortraitPath for the actual card portrait texture.
    // STS2's high-resolution individual portraits are 1000x760, so use the
    // normal portrait instead of the tall 606x852 full-art image. Feeding the
    // tall image into the normal portrait slot makes it letterbox and look tiny.
    public override string CustomPortraitPath => PortraitFileName.CardImagePath();
    public override string PortraitPath => PortraitFileName.CardImagePath();
    public override string BetaPortraitPath => $"beta/{PortraitFileName}".CardImagePath();
}
