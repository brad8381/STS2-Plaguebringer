using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public abstract class InhaleSpicesChoice : PlagueBringerCard
{
    protected override bool IsPlayable => false;
    public override bool CanBeGeneratedInCombat => false;

    public override string CustomPortraitPath => "card.png".BigCardImagePath();
    public override string PortraitPath => "card.png".CardImagePath();
    public override string BetaPortraitPath => "card.png".CardImagePath();

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
