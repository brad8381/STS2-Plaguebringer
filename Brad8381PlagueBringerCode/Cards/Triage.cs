using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class Triage : PlagueBringerCard, IPlagueCard
{
    public Triage() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithVars(
            new DynamicVar("BaseBlock", 6),
            new DynamicVar("PlaguePerBlock", 3),
            new DynamicVar("MaxBonusBlock", 8));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target)
            return;

        var plague = PlagueCardUtils.GetPlague(target);
        var bonus = Math.Min(
            DynamicVars["MaxBonusBlock"].IntValue,
            plague / DynamicVars["PlaguePerBlock"].IntValue);

        await CreatureCmd.GainBlock(
            Owner.Creature,
            DynamicVars["BaseBlock"].IntValue + bonus,
            ValueProp.Unpowered,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["BaseBlock"].UpgradeValueBy(2m);
        DynamicVars["MaxBonusBlock"].UpgradeValueBy(2m);
    }
}
