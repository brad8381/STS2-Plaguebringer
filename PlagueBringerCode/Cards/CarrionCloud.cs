using PB.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class CarrionCloud : PlagueBringerCard, IPlagueCard
{
    public CarrionCloud() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVars(new DynamicVar("SpreadPercent", 50));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<CarrionCloudPower>(
            choiceContext,
            Owner.Creature,
            DynamicVars["SpreadPercent"].IntValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["SpreadPercent"].UpgradeValueBy(25m);
    }
}
