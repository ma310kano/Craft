namespace Craft;

/// <summary>
/// 読み取り専用の装備
/// </summary>
public interface IReadOnlyEquipment
{
	#region Methods

	/// <summary>
	/// 部位に関連付けられているアイテム物質を取得します。
	/// </summary>
	/// <param name="part">部位</param>
	/// <param name="itemMatter">アイテム物質</param>
	/// <returns>部位にアイテム物質が関連付けされている場合は、<c>true</c>。それ以外の場合は、<c>false</c>。</returns>
	bool TryGetItemMatter(EquipmentParts part, out ItemMatter? itemMatter);

	#endregion
}
