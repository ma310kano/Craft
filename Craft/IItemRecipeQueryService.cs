namespace Craft;

/// <summary>
/// アイテムレシピの問い合わせサービス
/// </summary>
public interface IItemRecipeQueryService
{
	#region Methods

	/// <summary>
	/// アイテムレシピを問い合わせします。
	/// </summary>
	/// <param name="itemRecipeId">アイテムレシピID</param>
	/// <returns>問い合わせしたアイテムレシピを返します。</returns>
	ItemRecipe QuerySingle(ItemRecipeId itemRecipeId);

	#endregion
}
