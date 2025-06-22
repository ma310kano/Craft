using Dapper;
using Microsoft.Data.Sqlite;

namespace Craft.Sqlite;

/// <summary>
/// インベントリー
/// </summary>
/// <param name="connectionString">接続文字列</param>
/// <param name="languageCode">言語コード</param>
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
	/// アイテム物質を追加します。
	/// </summary>
	/// <param name="itemMatter">アイテム物質</param>
	public void AddItemMatter(ItemMatter itemMatter)
	{
		ItemMatter? foundItemMatter = _itemMatters.FirstOrDefault(x => x.Item == itemMatter.Item);

		if (foundItemMatter is not null)
		{
			foundItemMatter.AddQuantity(itemMatter.Quantity);
		}
		else
		{
			_itemMatters.Add(itemMatter);
		}
	}

	/// <summary>
	/// アイテムを除去します。
	/// </summary>
	/// <param name="itemId">アイテムID</param>
	/// <param name="quantity">数量</param>
	public ItemMatter RemoveItem(ItemId itemId, Quantity quantity)
	{
		ItemMatter result = _itemMatters.First(x => x.Item.ItemId == itemId);

		if (result.Quantity > quantity)
		{
			result = result.Take(quantity);
		}
		else if (result.Quantity == quantity)
		{
			_itemMatters.Remove(result);
		}
		else
		{
			throw new InvalidOperationException($"{result.Item.ItemName}の数量が不足しています。");
		}

		return result;
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
