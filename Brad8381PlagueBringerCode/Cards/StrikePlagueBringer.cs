using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class StrikePlagueBringer : PlagueBringerCard
{
    public StrikePlagueBringer() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
    {
        WithDamage(5);
        WithTags(CardTag.Strike);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
