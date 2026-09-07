using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Relics;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Character;

public class PlagueBringer : PlaceholderCharacterModel
{
    public const string CharacterId = "Brad8381PlagueBringer";

    public static readonly Color Color = new("c8c3b8");

    public override List<(string, string)>? Localization =>
        new CharacterLoc(
            Title: "The Plaguebringer",
            TitleObject: "the Plaguebringer",
            Description: "A physician with no patients left.\nSmall wounds. A sickness that only grows.",
            PronounObject: "them",
            PronounSubject: "they",
            PronounPossessive: "theirs",
            PossessiveAdjective: "their",
            AromaPrinciple: "Clove, smoke, and something spoiled.",
            EndTurnPingAlive: "The sickness will not wait.",
            EndTurnPingDead: "Keep the censer burning...",
            EventDeathPrevention: "Not yet. There is still work to do.",
            GoldMonologue: "Payment before treatment.",
            CardsModifierTitle: "Plaguebringer Cards",
            CardsModifierDescription: "Plaguebringer cards now appear in rewards and shops.",
            ("unlockText", "Play a run with [pink]{Prerequisite}[/pink] to unlock this character.")
        );

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 70;

    // Static custom combat sprite for now. This can later be replaced by an
    // AnimatedSprite2D or Spine scene without changing the character model.
    public override string CustomVisualPath =>
        "res://Brad8381PlagueBringer/scenes/plaguebringer_character.tscn";

    // Do not inherit Ironclad's full character-select artwork.
    public override string CustomCharacterSelectBg =>
        "res://Brad8381PlagueBringer/scenes/plaguebringer_select_bg.tscn";

    // Keep a known-good vanilla transition until we make a custom one.
    public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_ironclad";

    public override IEnumerable<CardModel> StartingDeck => [
        ModelDb.Card<StrikePlagueBringer>(), ModelDb.Card<StrikePlagueBringer>(),
        ModelDb.Card<StrikePlagueBringer>(), ModelDb.Card<StrikePlagueBringer>(),
        ModelDb.Card<DefendPlagueBringer>(), ModelDb.Card<DefendPlagueBringer>(),
        ModelDb.Card<DefendPlagueBringer>(), ModelDb.Card<DefendPlagueBringer>(),
        ModelDb.Card<InfectedScalpel>(), ModelDb.Card<PlagueVial>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics => [ModelDb.Relic<SealedCenser>()];

    public override CardPoolModel CardPool => ModelDb.CardPool<PlagueBringerCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<PlagueBringerRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<PlagueBringerPotionPool>();

    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
}
