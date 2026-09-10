using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PB.Powers;

public sealed class CarrionCloudPower : PlagueBringerPower
{
    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Carrion Cloud",
            "When an infected enemy dies, apply {Amount}% of its remaining Plague, rounded up, to every living enemy.",
            "When an infected enemy dies, apply {Amount}% of its remaining Plague, rounded up, to every living enemy."
        );

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDeath(
        PlayerChoiceContext choiceContext,
        Creature creature,
        bool wasRemovalPrevented,
        float deathAnimLength)
    {
        if (wasRemovalPrevented || creature.Side == Owner.Side || Owner.CombatState == null)
            return;

        var plague = creature.GetPower<PlaguePower>();
        if (plague == null || plague.Amount <= 0)
            return;

        var spread = (int)Math.Ceiling(plague.Amount * Amount / 100m);
        if (spread <= 0)
            return;

        Flash();
        foreach (var enemy in Owner.CombatState.GetOpponentsOf(Owner).Where(enemy => enemy.IsAlive).ToArray())
            await PB.Mechanics.PlagueActions.Apply(choiceContext, enemy, spread, Owner, null);
    }
}
