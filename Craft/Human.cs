namespace Craft;

/// <summary>
/// 人間
/// </summary>
/// <param name="humanId">人間ID</param>
/// <param name="firstName">個人名</param>
/// <param name="family">家族</param>
/// <param name="itemRecipes">アイテムレシピのコレクション</param>
/// <param name="inventory">インベントリー</param>
public class Human(HumanId humanId, FirstName firstName, Family family, ICollection<ItemRecipe> itemRecipes, IInventory inventory) : IEquatable<Human>
{
    #region Properties

    /// <summary>
    /// 人間IDを取得します。
    /// </summary>
    public HumanId HumanId { get; } = humanId;

    /// <summary>
    /// 個人名を取得します。
    /// </summary>
    public FirstName FirstName { get; } = firstName;

    /// <summary>
    /// 家族を取得します。
    /// </summary>
    public Family Family { get; } = family;

    /// <summary>
    /// アイテムレシピのコレクションを取得します。
    /// </summary>
    public ICollection<ItemRecipe> ItemRecipes { get; } = itemRecipes;

    /// <summary>
    /// インベントリーを取得します。
    /// </summary>
    public IInventory Inventory { get; } = inventory;

    #endregion

    #region Operators

    /// <summary>
    /// オペランドが等しい場合には <c>true</c> を返し、それ以外の場合は <c>false</c> を返します。
    /// </summary>
    /// <param name="lhs">左辺</param>
    /// <param name="rhs">右辺</param>
    /// <returns>オペランドが等しい場合は、 <c>true</c>。それ以外の場合は、 <c>false</c>。</returns>
    public static bool operator ==(Human lhs, Human rhs)
    {
        if (lhs is null) return rhs is null;

        bool result = lhs.Equals(rhs);

        return result;
    }

    /// <summary>
    /// オペランドが等しくない場合には <c>true</c> を返し、それ以外の場合は <c>false</c> を返します。
    /// </summary>
    /// <param name="lhs">左辺</param>
    /// <param name="rhs">右辺</param>
    /// <returns>オペランドが等しくない場合は、 <c>true</c>。それ以外の場合は、 <c>false</c>。</returns>
    public static bool operator !=(Human lhs, Human rhs)
    {
        bool result = !(lhs == rhs);

        return result;
    }

    #endregion

    #region Methods

    /// <summary>
    /// 指定されたオブジェクトが現在のオブジェクトと等しいかどうかを判断します。
    /// </summary>
    /// <param name="obj">現在のオブジェクトと比較するオブジェクト。</param>
    /// <returns>指定したオブジェクトが現在のオブジェクトと等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
    public override bool Equals(object? obj)
    {
        bool result = obj switch
        {
            Human other => Equals(other),
            _ => base.Equals(obj),
        };

        return result;
    }
    /// <summary>
    /// 現在のオブジェクトが、同じ型の別のオブジェクトと等しいかどうかを示します。
    /// </summary>
    /// <param name="other">このオブジェクトと比較するオブジェクト。</param>
    /// <returns>現在のオブジェクトが <c>other</c> パラメーターと等しい場合は <c>true</c>、それ以外の場合は <c>false</c> です。</returns>
    public bool Equals(Human? other)
    {
        if (other is null) return false;

        bool result = HumanId == other.HumanId;

        return result;
    }

    /// <summary>
    /// 既定のハッシュ関数として機能します。
    /// </summary>
    /// <returns>現在のオブジェクトのハッシュ コード。</returns>
    public override int GetHashCode()
    {
        int result = HashCode.Combine(HumanId);

        return result;
    }

    /// <summary>
    /// アイテムを作成します。
    /// </summary>
    /// <param name="itemRecipeId">アイテムレシピID</param>
    public void MakeItem(ItemRecipeId itemRecipeId)
    {
        ItemRecipe itemRecipe = ItemRecipes.Single(x => x.ItemRecipeId == itemRecipeId);

        foreach (RecipeIngredient ingredient in itemRecipe.Ingredients)
        {
            Inventory.RemoveItem(ingredient.Item.ItemId, ingredient.Quantity);
        }

        Inventory.AddItem(itemRecipe.Item.ItemId, itemRecipe.Quantity);
    }

    /// <summary>
    /// アイテムを拾得します。
    /// </summary>
    /// <param name="itemId">アイテムID</param>
    /// <param name="quantity">数量</param>
    public void PickUpItem(ItemId itemId, Quantity quantity)
    {
		if (ItemPickedUp is null) throw new InvalidOperationException("イベントが登録されていません。");

		ItemPickedUpEventArgs e = new(itemId, quantity);

        ItemPickedUp(this, e);
    }

    /// <summary>
    /// 現在のオブジェクトを表す文字列を返します。
    /// </summary>
    /// <returns>現在のオブジェクトを表す文字列。</returns>
    public override string ToString()
    {
        string str = $"{nameof(Human)} {{ {nameof(HumanId)} = {HumanId}, {nameof(FirstName)} = {FirstName}, {nameof(Family)} = {Family}, {nameof(ItemRecipes)} = {ItemRecipes}, {nameof(Inventory)} = {Inventory} }}";

        return str;
    }

    #endregion

    #region Nested types

    /// <summary>
    /// アイテムを拾得すると発生します。
    /// </summary>
    public event EventHandler<ItemPickedUpEventArgs>? ItemPickedUp;

	#endregion
}
