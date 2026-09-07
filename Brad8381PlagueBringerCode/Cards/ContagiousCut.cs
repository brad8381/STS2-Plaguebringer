using BaseLib.Utils;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class ContagiousCut : PlagueBringerCard, IPlagueCard
{
    public ContagiousCut() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(7);
        WithVars(new PowerVar<PlaguePower>("Plague", 2));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true }) return;

        var combatState = Owner.Creature.CombatState;
        if (combatState == null) return;

        await CommonActions.CardAttack(this, play).Execute(choiceContext);

        foreach (var enemy in combatState.GetOpponentsOf(Owner.Creature).Where(enemy => enemy.IsAlive).ToArray())
            await PowerCmd.Apply<PlaguePower>(choiceContext, enemy, DynamicVars["Plague"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars["Plague"].UpgradeValueBy(1m);
    }
}
