using BaseLib.Abstracts;
using BaseLib.Extensions;
using PB.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace PB.Powers;

/// <summary>
/// Base class for Plaguebringer powers.
/// Loads power art from the mod resources and keeps the HUD counter separate
/// from the power's internal Amount value.
/// </summary>
public abstract class PlagueBringerPower : CustomPowerModel
{
    private int _displayStacks;

    // Loads from PlagueBringer/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();

    /// <summary>
    /// Power cards use Amount for their actual effect strength (Plague, Block, percent, etc.).
    /// The character HUD should instead show how many copies of that Power card have been applied.
    /// Non-card powers such as Plague, Specimen and Sway continue to display Amount normally.
    /// </summary>
    public override int DisplayAmount => _displayStacks > 0 ? _displayStacks : Amount;

    public override Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (ReferenceEquals(power, this) && amount > 0m && cardSource?.Type == CardType.Power)
        {
            _displayStacks++;
            InvokeDisplayAmountChanged();
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Whether this power is a buff or debuff.
    /// </summary>
    public abstract override PowerType Type { get; }

    /// <summary>
    /// How this power stacks mechanically. This remains independent from DisplayAmount.
    /// </summary>
    public abstract override PowerStackType StackType { get; }
}
