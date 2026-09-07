using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Relics;

public sealed class SporeJar : PlagueBringerRelic
{
    public override List<(string, string)>? Localization =>
        new RelicLoc(
            "Spore Jar",
            "At the end of your turn, apply 1 [gold]Plague[/gold] to ALL enemies.",
            "Do not shake before opening."
        );

    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override async Task BeforeSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != Owner.Creature.Side || Owner.Creature.CombatState == null) return;

        Flash();
        foreach (var enemy in Owner.Creature.CombatState.GetOpponentsOf(Owner.Creature).Where(enemy => enemy.IsAlive).ToArray())
            await PowerCmd.Apply<PlaguePower>(choiceContext, enemy, 1, Owner.Creature, null);
    }
}
