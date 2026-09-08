using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Character;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Mechanics;

[HarmonyPatch(typeof(NCombatUi), nameof(NCombatUi.Activate))]
public static class SpecimenUiPatch
{
    private const string CounterName = "PlagueBringerSpecimenCounter";
    private const string Tooltip =
        "Specimen\nA persistent combat resource gained mainly from Exhausting cards. Persists between turns, up to 6. Lost at the end of combat.";

    [HarmonyPostfix]
    public static void Postfix(NCombatUi __instance, CombatState state)
    {
        var player = LocalContext.GetMe(state);
        if (player?.Character is not PlagueBringer)
            return;

        var energy = __instance.EnergyCounterContainer
            .GetChildren()
            .OfType<NEnergyCounter>()
            .FirstOrDefault();
        if (energy == null)
            return;

        Attach(energy, player);
    }

    private static void Attach(NEnergyCounter energy, Player player)
    {
        var counter = energy.GetNodeOrNull<Control>(CounterName);
        if (counter == null)
        {
            counter = new Control
            {
                Name = CounterName,
                Size = new Vector2(76, 76),
                MouseFilter = Control.MouseFilterEnum.Pass,
                TooltipText = Tooltip
            };

            var icon = new TextureRect
            {
                ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                Texture = GD.Load<Texture2D>("res://Brad8381PlagueBringer/images/charui/specimen_counter.png"),
                MouseFilter = Control.MouseFilterEnum.Ignore
            };

            var label = new Label
            {
                Name = "Amount",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                MouseFilter = Control.MouseFilterEnum.Ignore
            };
            label.AddThemeColorOverride("font_color", new Color("f2eee4"));
            label.AddThemeColorOverride("font_outline_color", Colors.Black);
            label.AddThemeConstantOverride("outline_size", 5);
            label.AddThemeFontSizeOverride("font_size", 28);

            counter.AddChild(icon);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            counter.AddChild(label);
            label.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);

            energy.ClipContents = false;
            energy.AddChild(counter);

            void Refresh() => label.Text = SpecimenActions.Count(player.Creature).ToString();
            Refresh();

            Action<PowerModel> applied = power =>
            {
                if (power is SpecimenPower) Refresh();
            };
            Action<PowerModel, int, bool> increased = (power, _, _) =>
            {
                if (power is SpecimenPower) Refresh();
            };
            Action<PowerModel, bool> decreased = (power, _) =>
            {
                if (power is SpecimenPower) Refresh();
            };
            Action<PowerModel> removed = power =>
            {
                if (power is SpecimenPower) Refresh();
            };

            player.Creature.PowerApplied += applied;
            player.Creature.PowerIncreased += increased;
            player.Creature.PowerDecreased += decreased;
            player.Creature.PowerRemoved += removed;

            counter.TreeExiting += () =>
            {
                player.Creature.PowerApplied -= applied;
                player.Creature.PowerIncreased -= increased;
                player.Creature.PowerDecreased -= decreased;
                player.Creature.PowerRemoved -= removed;
            };
        }

        counter.Position = new Vector2(energy.Size.X + 8f, (energy.Size.Y - counter.Size.Y) / 2f);
    }
}

