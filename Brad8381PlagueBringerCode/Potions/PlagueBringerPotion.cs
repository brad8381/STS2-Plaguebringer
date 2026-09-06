using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Character;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Extensions;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Potions;

[Pool(typeof(PlagueBringerPotionPool))]
public abstract class PlagueBringerPotion : CustomPotionModel
{
	public override string? CustomPackedImagePath =>
		$"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();
	public override string? CustomPackedOutlinePath =>
		$"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionOutlineImagePath();
}