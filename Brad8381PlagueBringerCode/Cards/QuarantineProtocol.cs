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
        WithVars(
            new DynamicVar("BaseBlock", 3),
            new DynamicVar("BlockPerThreshold", 4),
            new DynamicVar("PlagueThreshold", 6));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var combatState = CombatState;
        if (combatState == null)
            return;

        var totalPlague = combatState
            .GetOpponentsOf(Owner.Creature)
            .Where(enemy => enemy.IsAlive)
            .Sum(PlagueCardUtils.GetPlague);

        var plagueGroups = totalPlague / DynamicVars["PlagueThreshold"].IntValue;
        var block = DynamicVars["BaseBlock"].IntValue +
                    plagueGroups * DynamicVars["BlockPerThreshold"].IntValue;

        await CreatureCmd.GainBlock(
            Owner.Creature,
            block,
            ValueProp.Move,
            play,
            false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["BlockPerThreshold"].UpgradeValueBy(1m);
    }
}
