namespace Craft;

/// <summary>
/// エリア
/// </summary>
/// <param name="areaId">エリアID</param>
/// <param name="areaName">エリア名</param>
/// <param name="items">アイテムのコレクション</param>
/// <param name="humans">人間のコレクション</param>
public class Area(AreaId areaId, AreaName areaName, IReadOnlyCollection<Item> items, IReadOnlyCollection<Human> humans) : IEquatable<Area>
{
    #region Fields

    /// <summary>
    /// 人間のコレクション
    /// </summary>
    private readonly List<Human> _humans = [.. humans];

	#endregion

	#region Properties

	/// <summary>
	/// エリアIDを取得します。
	/// </summary>
	public AreaId AreaId { get; } = areaId;

    /// <summary>
    /// エリア名を取得します。
    /// </summary>
    public AreaName AreaName { get; } = areaName;

    /// <summary>
    /// アイテムのコレクションを取得します。
    /// </summary>
    public IReadOnlyCollection<Item> Items { get; } = items;

    /// <summary>
    /// 人間のコレクションを取得します。
    /// </summary>
    public IReadOnlyCollection<Human> Humans => _humans;

    #endregion

    #region Operators

    /// <summary>
    /// オペランドが等しい場合には <c>true</c> を返し、それ以外の場合は <c>false</c> を返します。
    /// </summary>
    /// <param name="lhs">左辺</param>
    /// <param name="rhs">右辺</param>
    /// <returns>オペランドが等しい場合は、 <c>true</c>。それ以外の場合は、 <c>false</c>。</returns>
    public static bool operator ==(Area lhs, Area rhs)
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
    public static bool operator !=(Area lhs, Area rhs)
    {
        bool result = !(lhs == rhs);

        return result;
    }

    #endregion

    #region Methods

    /// <summary>
    /// 人間を追加します。
    /// </summary>
    /// <param name="human">人間</param>
    public void AddHuman(Human human)
    {
		human.ItemPickedUp += Human_ItemPickedUp;

        _humans.Add(human);
    }

    /// <summary>
    /// 人間を除去します。
    /// </summary>
    /// <param name="human">人間</param>
    public void RemoveHuman(Human human)
    {
        _humans.Remove(human);

        human.ItemPickedUp -= Human_ItemPickedUp;
    }

	/// <summary>
	/// アイテム物質を作成します。
	/// </summary>
	/// <param name="item">アイテム</param>
	/// <param name="quantity">数量</param>
	/// <returns>作成したアイテム物質を返します。</returns>
	private static ItemMatter CreateItemMatter(Item item, Quantity quantity)
    {
        ItemMatter product;
        {
            ItemMatterId itemMatterId = ItemMatterId.Create();

            product = new ItemMatter(itemMatterId, item, quantity);
        }

        return product;
    }

	/// <summary>
	/// 人間がアイテムを拾得した際に呼び出されます。
	/// </summary>
	/// <param name="sender">送信元</param>
	/// <param name="e">イベント引数</param>
	/// <exception cref="InvalidOperationException">人間が指定されていません。</exception>
	private void Human_ItemPickedUp(object? sender, ItemPickedUpEventArgs e)
	{
		Human human = sender as Human ?? throw new InvalidOperationException("人間が指定されていません。");

		Item item = Items.Single(x => x.ItemId == e.ItemId);

        bool requiredSkills = item.Skills.TryGetValue(ItemSkillCategory.PickUp, out IReadOnlyCollection<Skill>? skills);
        if (requiredSkills)
        {
            foreach (Skill skill in skills!)
            {
                bool haveSkill = human.Skills.Any(x => x == skill);

                if (!haveSkill) throw new InvalidOperationException("拾得するスキルを所持していません。");
            }
        }

		ItemMatter itemMatter = CreateItemMatter(item, e.Quantity);

		human.Inventory.AddItemMatter(itemMatter);
	}

	/// <summary>
	/// 指定されたオブジェクトが現在のオブジェクトと等しいかどうかを判断します。
	/// </summary>
	/// <param name="obj">現在のオブジェクトと比較するオブジェクト。</param>
	/// <returns>指定したオブジェクトが現在のオブジェクトと等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
	public override bool Equals(object? obj)
    {
        bool result = obj switch
        {
            Area other => Equals(other),
            _ => base.Equals(obj),
        };

        return result;
    }
    /// <summary>
    /// 現在のオブジェクトが、同じ型の別のオブジェクトと等しいかどうかを示します。
    /// </summary>
    /// <param name="other">このオブジェクトと比較するオブジェクト。</param>
    /// <returns>現在のオブジェクトが <c>other</c> パラメーターと等しい場合は <c>true</c>、それ以外の場合は <c>false</c> です。</returns>
    public bool Equals(Area? other)
    {
        if (other is null) return false;

        bool result = AreaId == other.AreaId;

        return result;
    }

    /// <summary>
    /// 既定のハッシュ関数として機能します。
    /// </summary>
    /// <returns>現在のオブジェクトのハッシュ コード。</returns>
    public override int GetHashCode()
    {
        int result = HashCode.Combine(AreaId);

        return result;
    }

    /// <summary>
    /// 現在のオブジェクトを表す文字列を返します。
    /// </summary>
    /// <returns>現在のオブジェクトを表す文字列。</returns>
    public override string ToString()
    {
        string str = $"{nameof(Area)} {{ {nameof(AreaId)} = {AreaId}, {nameof(AreaName)} = {AreaName}, {nameof(Items)} = {Items}, {nameof(Humans)} = {Humans} }}";

        return str;
    }

    #endregion
}
