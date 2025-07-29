namespace Craft;

/// <summary>
/// 装備
/// </summary>
public class Equipment : IReadOnlyEquipment
{
	#region Fields

	/// <summary>
	/// アイテム物質のコレクション
	/// </summary>
	private readonly List<ItemMatter> _itemMatters = [];

	#endregion

	#region Methods

	/// <summary>
	/// アイテム物質を追加します。
	/// </summary>
	/// <param name="itemMatter">アイテム物質</param>
	public void AddItemMatter(ItemMatter itemMatter)
	{
		_itemMatters.Add(itemMatter);
	}

	#endregion
}
