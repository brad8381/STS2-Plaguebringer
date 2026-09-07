using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class Incubation : PlagueBringerCard, IPlagueCard
{
    public Incubation() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithVars(new DynamicVar("Percent", 50));
        WithKeywords(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target) return;
        var plague = target.GetPower<PlaguePower>();
        if (plague == null || plague.Amount <= 0) return;

        var increase = Math.Ceiling(plague.Amount * DynamicVars["Percent"].BaseValue / 100m);
        if (increase > 0)
            await PowerCmd.ModifyAmount(choiceContext, plague, increase, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Percent"].UpgradeValueBy(25m);
    }
}
