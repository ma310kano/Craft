namespace Craft;

/// <summary>
/// 人間
/// </summary>
public class Human : IEquatable<Human>
{
    #region Fields

    /// <summary>
    /// スキルのコレクション
    /// </summary>
    private readonly List<Skill> _skills;

    /// <summary>
    /// 装備
    /// </summary>
    private readonly Equipment _equipment;

	#endregion

	#region Constructors

	/// <summary>
	/// 人間を初期化します。
	/// </summary>
	/// <param name="humanId">人間ID</param>
	/// <param name="firstName">個人名</param>
	/// <param name="family">家族</param>
	/// <param name="skills">スキルのコレクション</param>
	/// <param name="itemRecipes">アイテムレシピのコレクション</param>
	/// <param name="equipment">装備</param>
	/// <param name="inventory">インベントリー</param>
	public Human(HumanId humanId, FirstName firstName, Family family, List<Skill> skills, ICollection<ItemRecipe> itemRecipes, Equipment equipment, IInventory inventory)
    {
        _skills = skills;
        _equipment = equipment;

        HumanId = humanId;
        FirstName = firstName;
        Family = family;
        ItemRecipes = itemRecipes;
        Inventory = inventory;

		_equipment.ItemMatterRemoved += Equipment_ItemMatterRemoved;
    }

	#endregion

	#region Properties

	/// <summary>
	/// 人間IDを取得します。
	/// </summary>
	public HumanId HumanId { get; }

    /// <summary>
    /// 個人名を取得します。
    /// </summary>
    public FirstName FirstName { get; }

    /// <summary>
    /// 家族を取得します。
    /// </summary>
    public Family Family { get; }

    /// <summary>
    /// エリアIDを取得します。
    /// </summary>
    public AreaId? AreaId { get; private set; }

    /// <summary>
    /// スキルのコレクションを取得します。
    /// </summary>
    public IReadOnlyCollection<Skill> Skills => _skills;

    /// <summary>
    /// アイテムレシピのコレクションを取得します。
    /// </summary>
    public ICollection<ItemRecipe> ItemRecipes { get; }

    /// <summary>
    /// 装備
    /// </summary>
    public IReadOnlyEquipment Equipment => _equipment;

    /// <summary>
    /// インベントリーを取得します。
    /// </summary>
    public IInventory Inventory { get; }

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
    /// アイテムを装備します。
    /// </summary>
    /// <param name="part">部位</param>
    /// <param name="itemId">アイテムID</param>
    public void EquipItem(EquipmentParts part, ItemId itemId)
    {
        ItemMatter itemMatter;
        {
			Quantity quantity = new(1);

			itemMatter = Inventory.RemoveItem(itemId, quantity);
		}

        _equipment.AddItemMatter(part, itemMatter);

        {
			// 装備は必ずスキルを内包している
			IReadOnlyCollection<Skill> skills = itemMatter.Item.Skills[ItemSkillCategory.Equipment];

            _skills.AddRange(skills);
        }
	}

	/// <summary>
	/// 装備がアイテム物質を除去した際に呼び出されます。
	/// </summary>
	/// <param name="sender">送信元</param>
	/// <param name="e">イベント引数</param>
	private void Equipment_ItemMatterRemoved(object? sender, ItemMatterRemovedEventArgs e)
	{
        Inventory.AddItemMatter(e.ItemMatter);
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

        foreach (Skill skill in itemRecipe.Skills)
        {
            bool haveSkill = Skills.Any(x => x.SkillId == skill.SkillId);

			if (!haveSkill) throw new InvalidOperationException("アイテムを作成するスキルを所持していません。");
        }

        foreach (RecipeIngredient ingredient in itemRecipe.Ingredients)
        {
            Inventory.RemoveItem(ingredient.Item.ItemId, ingredient.Quantity);
        }

        ItemMatter itemMatter;
        {
            ItemMatterId itemMatterId = ItemMatterId.Create();

            itemMatter = new ItemMatter(itemMatterId, itemRecipe.Item, itemRecipe.Quantity);
        }

        Inventory.AddItemMatter(itemMatter);
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
    /// エリアIDを設定します。
    /// </summary>
    /// <param name="areaId">エリアID</param>
    internal void SetAreaId(AreaId? areaId)
    {
        AreaId = areaId;
    }

    /// <summary>
    /// 現在のオブジェクトを表す文字列を返します。
    /// </summary>
    /// <returns>現在のオブジェクトを表す文字列。</returns>
    public override string ToString()
    {
        string str = $"{nameof(Human)} {{ {nameof(HumanId)} = {HumanId}, {nameof(FirstName)} = {FirstName}, {nameof(Family)} = {Family}, {nameof(AreaId)} = {AreaId}, {nameof(Skills)} = {Skills}, {nameof(ItemRecipes)} = {ItemRecipes}, {nameof(Equipment)} = {Equipment}, {nameof(Inventory)} = {Inventory} }}";

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
