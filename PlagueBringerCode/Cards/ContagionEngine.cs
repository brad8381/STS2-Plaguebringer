using PB.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class ContagionEngine : PlagueBringerCard, IPlagueCard
{
    public ContagionEngine() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVars(new PowerVar<ContagionEnginePower>("Plague", 2));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<ContagionEnginePower>(choiceContext, Owner.Creature, DynamicVars["Plague"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Plague"].UpgradeValueBy(1m);
    }
}
