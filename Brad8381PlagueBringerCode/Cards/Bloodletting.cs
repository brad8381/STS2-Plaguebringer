using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class Bloodletting : PlagueBringerCard, IPlagueCard
{
    public Bloodletting() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithVars(
            new DynamicVar("PlagueCap", 6),
            new DynamicVar("BlockPerPlague", 2));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target)
            return;

        var removed = await PlagueCardUtils.RemovePlague(
            choiceContext,
            target,
            DynamicVars["PlagueCap"].IntValue,
            this);

        if (removed <= 0)
            return;

        await CreatureCmd.GainBlock(
            Owner.Creature,
            removed * DynamicVars["BlockPerPlague"].IntValue,
            ValueProp.Unpowered,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["PlagueCap"].UpgradeValueBy(2m);
    }
}
