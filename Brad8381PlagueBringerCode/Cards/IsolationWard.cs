using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class IsolationWard : PlagueBringerCard, IPlagueCard
{
    public IsolationWard() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithVars(new DynamicVar("BlockOnPlague", 4));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<IsolationWardPower>(choiceContext, Owner.Creature, DynamicVars["BlockOnPlague"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}
