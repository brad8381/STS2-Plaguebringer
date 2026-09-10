using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PB.Powers;

namespace PB.Cards;

public sealed class CrossImmunity : PlagueBringerCard
{
    public CrossImmunity()
        : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<CrossImmunityPower>(
            choiceContext,
            Owner.Creature,
            IsUpgraded ? 2 : 1,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
    }
}
