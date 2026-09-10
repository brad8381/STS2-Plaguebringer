using BaseLib.Abstracts;
using PB.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PB.Cards;

public sealed class CatalyticStrain : PlagueBringerCard, IPlagueCard
{
    public CatalyticStrain() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
    }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Catalytic Strain",
            "Whenever you apply [gold]Plague[/gold] to an enemy, trigger its Plague, then add 10% of its current Plague, rounded up."
        );

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<CatalyticStrainPower>(
            choiceContext,
            Owner.Creature,
            1,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
