using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PB.Cards;

public abstract class InhaleSpicesChoice : PlagueBringerCard
{
    protected override bool IsPlayable => false;
    public override bool CanBeGeneratedInCombat => false;

    protected InhaleSpicesChoice() : base(0, CardType.Skill, CardRarity.Token, TargetType.Self, false)
    {
    }

    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
    }
}
