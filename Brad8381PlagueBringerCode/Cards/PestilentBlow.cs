using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class PestilentBlow : PlagueBringerCard, IPlagueCard
{
    public PestilentBlow() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithVars(new DynamicVar("Multiplier", 2));
        WithKeywords(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target) return;

        var plague = target.GetPower<PlaguePower>();
        if (plague == null || plague.Amount <= 0) return;

        var multiplier = DynamicVars["Multiplier"].IntValue;
        var extra = plague.Amount * (multiplier - 1);
        if (extra > 0)
            await PowerCmd.ModifyAmount(choiceContext, plague, extra, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Multiplier"].UpgradeValueBy(1m);
        RemoveKeyword(CardKeyword.Exhaust);
    }
}
