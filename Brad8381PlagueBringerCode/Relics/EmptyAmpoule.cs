using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Relics;

public sealed class EmptyAmpoule : PlagueBringerRelic
{
    public override List<(string, string)>? Localization =>
        new RelicLoc(
            "Empty Ampoule",
            "At the end of your turn, if no enemy has [gold]Plague[/gold], apply 20 [gold]Plague[/gold] to an enemy.",
            "Empty does not mean clean."
        );

    public override RelicRarity Rarity => RelicRarity.Rare;

    public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Creature.Side || Owner.Creature.CombatState == null) return;
        var enemies = Owner.Creature.CombatState.GetOpponentsOf(Owner.Creature).Where(enemy => enemy.IsAlive).ToArray();
        if (enemies.Length == 0 || enemies.Any(enemy => (enemy.GetPower<PlaguePower>()?.Amount ?? 0) > 0)) return;

        Flash();
        await PowerCmd.Apply<PlaguePower>(choiceContext, enemies[0], 20, Owner.Creature, null);
    }
}
