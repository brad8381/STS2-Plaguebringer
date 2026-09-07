using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Character;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

[Pool(typeof(PlagueBringerCardPool))]
public abstract class PlagueBringerCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    ConstructedCardModel(cost, type, rarity, target)
{
    private string PortraitFileName => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png";

    public override string CustomPortraitPath
    {
        get
        {
            string requested = PortraitFileName.BigCardImagePath();
            return ResourceLoader.Exists(requested)
                ? requested
                : "card.png".BigCardImagePath();
        }
    }

    public override string PortraitPath
    {
        get
        {
            string requested = PortraitFileName.CardImagePath();
            return ResourceLoader.Exists(requested)
                ? requested
                : "card.png".CardImagePath();
        }
    }

    public override string BetaPortraitPath
    {
        get
        {
            string requested = $"beta/{PortraitFileName}".CardImagePath();
            return ResourceLoader.Exists(requested)
                ? requested
                : PortraitPath;
        }
    }
}
