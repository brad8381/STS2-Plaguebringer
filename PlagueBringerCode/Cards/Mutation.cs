using PB.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class Mutation : PlagueBringerCard, IPlagueCard
{
    public Mutation() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVars(new PowerVar<MutationPower>("Percent", 25));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<MutationPower>(choiceContext, Owner.Creature, DynamicVars["Percent"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Percent"].UpgradeValueBy(15m);
    }
}
