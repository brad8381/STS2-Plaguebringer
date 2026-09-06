using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Relics;

public sealed class OpenCenser : PlagueBringerRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        if (player != Owner || Owner.PlayerCombatState is not { TurnNumber: 1 }) return;
        Flash();
        foreach (var enemy in combatState.GetOpponentsOf(Owner.Creature).Where(enemy => enemy.IsAlive).ToArray())
            await PowerCmd.Apply<PlaguePower>(enemy, 1, Owner.Creature, null);
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner || Owner.PlayerCombatState is not { TurnNumber: 1 }) return;
        var drawPile = PileType.Draw.GetPile(Owner);
        // Take the first three eligible cards in pile order, preserving multiplayer determinism.
        var candidates = drawPile.Cards.Where(card => card is IPlagueCard).Take(3).ToList();
        if (candidates.Count == 0) return;

        var prompt = new LocString("relics", "BRAD8381PLAGUEBRINGER-OPEN_CENSER.selectionScreenPrompt");
        var chosen = await CardSelectCmd.FromSimpleGrid(choiceContext, candidates, Owner,
            new CardSelectorPrefs(prompt, 1));
        foreach (var card in chosen)
        {
            if (drawPile.Cards.Contains(card))
                await CardPileCmd.Add(card, PileType.Hand);
        }
        MainFile.Logger.Debug($"Open Censer selection: owner={Owner}, candidates={candidates.Count}");
    }
}
