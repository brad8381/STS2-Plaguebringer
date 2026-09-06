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

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 70;

    // Use our own combat visual instead of PlaceholderCharacterModel's Ironclad body.
    // Other secondary character assets can continue to fall back to Ironclad until
    // we replace them individually.
    public override string CustomVisualPath =>
        "res://Brad8381PlagueBringer/scenes/plaguebringer_character.tscn";

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
