using Dapper;
using Microsoft.Data.Sqlite;

namespace Craft.Sqlite;

/// <summary>
/// インベントリー
/// </summary>
public class Inventory(string connectionString, string languageCode) : IInventory
{
	#region Fields

	/// <summary>
	/// 接続文字列
	/// </summary>
	private readonly string _connectionString = connectionString;

	/// <summary>
	/// 言語コード
	/// </summary>
	private readonly string _languageCode = languageCode;

	/// <summary>
	/// アイテム物質のコレクション
	/// </summary>
	private readonly List<ItemMatter> _itemMatters = [];

	#endregion

	#region Methods

	/// <summary>
	/// アイテムを追加します。
	/// </summary>
	/// <param name="itemId">アイテムID</param>
	/// <param name="quantity">数量</param>
	public void AddItem(ItemId itemId, Quantity quantity)
	{
		ItemMatter? itemMatter = _itemMatters.FirstOrDefault(x => x.Item.ItemId == itemId);

		if (itemMatter is not null)
		{
			itemMatter.AddQuantity(quantity);
		}
		else
		{
			DefaultTypeMap.MatchNamesWithUnderscores = true;

			using SqliteConnection connection = new(_connectionString);
			connection.Open();

			ItemMatterId resItemMatterId = ItemMatterId.Create();

			ItemRepository itemRepository = new(connection, _languageCode);
			Item resItem = itemRepository.Find(itemId);

			itemMatter = new ItemMatter(resItemMatterId, resItem, quantity);
			_itemMatters.Add(itemMatter);
		}
	}

	/// <summary>
	/// 現在のオブジェクトを表す文字列を返します。
	/// </summary>
	/// <returns>現在のオブジェクトを表す文字列。</returns>
	public override string ToString()
	{
		string str = $"{nameof(Inventory)} {{ {nameof(_itemMatters.Count)} = {_itemMatters.Count} }}";

		return str;
	}

	#endregion
}
