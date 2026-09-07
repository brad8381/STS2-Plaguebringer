using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class QuarantineProtocol : PlagueBringerCard, IPlagueCard
{
    public QuarantineProtocol() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithVars(new DynamicVar("BlockPerInfectedEnemy", 4));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var combatState = CombatState;
        if (combatState == null)
            return;

        var infected = combatState
            .GetOpponentsOf(Owner.Creature)
            .Count(enemy => enemy.IsAlive && PlagueCardUtils.GetPlague(enemy) > 0);

        var block = infected * DynamicVars["BlockPerInfectedEnemy"].IntValue;
        if (block <= 0)
            return;

        await CreatureCmd.GainBlock(
            Owner.Creature,
            block,
            ValueProp.Move,
            play,
            false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["BlockPerInfectedEnemy"].UpgradeValueBy(1m);
    }
}
