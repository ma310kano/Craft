namespace Craft;

/// <summary>
/// 装備
/// </summary>
public class Equipment : IReadOnlyEquipment
{
	#region Fields

	/// <summary>
	/// 部位とアイテム物質のディクショナリー
	/// </summary>
	private readonly Dictionary<EquipmentParts, ItemMatter> _partItemMatters = [];

	#endregion

	#region Methods

	/// <summary>
	/// アイテム物質を追加します。
	/// </summary>
	/// <param name="part">部位</param>
	/// <param name="itemMatter">アイテム物質</param>
	public void AddItemMatter(EquipmentParts part, ItemMatter itemMatter)
	{
		bool exists = _partItemMatters.TryGetValue(part, out ItemMatter? removedItemMatter);
		if (exists)
		{
			_partItemMatters.Remove(part);

			if (ItemMatterRemoved is not null)
			{
				ItemMatterRemovedEventArgs e = new(removedItemMatter!);

				ItemMatterRemoved(this, e);
			}
		}

		_partItemMatters.Add(part, itemMatter);
	}

	/// <summary>
	/// 部位に関連付けられているアイテム物質を取得します。
	/// </summary>
	/// <param name="part">部位</param>
	/// <param name="itemMatter">アイテム物質</param>
	/// <returns>部位にアイテム物質が関連付けされている場合は、<c>true</c>。それ以外の場合は、<c>false</c>。</returns>
	public bool TryGetItemMatter(EquipmentParts part, out ItemMatter? itemMatter)
	{
		bool result = _partItemMatters.TryGetValue(part, out itemMatter);

		return result;
	}

	#endregion

	#region Events

	/// <summary>
	/// アイテム物質を除去すると発生します。
	/// </summary>
	public event EventHandler<ItemMatterRemovedEventArgs>? ItemMatterRemoved;

	#endregion
}
