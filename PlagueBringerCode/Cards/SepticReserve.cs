using PB.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class SepticReserve : PlagueBringerCard, IPlagueCard
{
    public SepticReserve() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVars(new DynamicVar("EnergyOnRemove", 1));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<SepticReservePower>(
            choiceContext,
            Owner.Creature,
            DynamicVars["EnergyOnRemove"].IntValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
