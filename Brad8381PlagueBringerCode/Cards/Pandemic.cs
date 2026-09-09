using BaseLib.Utils;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class Pandemic : PlagueBringerCard, IPlagueCard
{
    public Pandemic() : base(2, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithVars(new PowerVar<PlaguePower>("Plague", 3));
        WithKeywords(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var combatState = CombatState;
        if (combatState == null)
            return;

        foreach (var enemy in combatState.HittableEnemies.Where(enemy => enemy.IsAlive).ToArray())
        {
            await PowerCmd.Apply<PlaguePower>(
                choiceContext,
                enemy,
                DynamicVars["Plague"].IntValue,
                Owner.Creature,
                this);
        }

        await PlagueCardUtils.TriggerAllEnemies(choiceContext, combatState, Owner.Creature);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Plague"].UpgradeValueBy(1m);
    }
}
