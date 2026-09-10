using BaseLib.Utils;
using PB.Mechanics;
using PB.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class Quarantine : PlagueBringerCard, IPlagueCard
{
    public Quarantine() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(5);
        WithVars(new PowerVar<SwayPower>("Sway", 1));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);

        var combatState = CombatState;
        if (combatState == null)
            return;

        foreach (var enemy in combatState
                     .GetOpponentsOf(Owner.Creature)
                     .Where(enemy => enemy.IsAlive && PlagueCardUtils.GetPlague(enemy) > 0)
                     .ToArray())
        {
            await SwayActions.Apply(
                choiceContext,
                enemy,
                DynamicVars["Sway"].IntValue,
                Owner.Creature,
                this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2m);
        DynamicVars["Sway"].UpgradeValueBy(1m);
    }
}
