using System.Text.RegularExpressions;

namespace Craft;

/// <summary>
/// アイテム物質ID
/// </summary>
public partial record class ItemMatterId
{
    #region Constructors

    /// <summary>
    /// アイテム物質IDを初期化します。
    /// </summary>
    /// <param name="value">値</param>
    public ItemMatterId(string value)
    {
        bool succeeded = Validate(value, out string message);
        if (!succeeded) throw new ArgumentException(message, nameof(value));

        Value = value;
    }

    #endregion

    #region Properties

    /// <summary>
    /// 値を取得します。
    /// </summary>
    public string Value { get; }

    #endregion

    #region Methods

    /// <summary>
    /// アイテム物質IDを作成します。
    /// </summary>
    /// <returns>作成したアイテム物質IDを返します。</returns>
    public static ItemMatterId Create()
    {
        ItemMatterId product;
        {
            string value = Guid.NewGuid().ToString().ToLower();

            product = new ItemMatterId(value);
        }

        return product;
    }

    /// <summary>
    /// 値を検証します。
    /// </summary>
    /// <param name="value">値</param>
    /// <param name="message">メッセージ</param>
    public static bool Validate(string value, out string message)
    {
        bool result = GetRegex().IsMatch(value);

        if (result)
        {
            message = string.Empty;
        }
        else
        {
            message = "アイテム物質IDは、UUID の形式で入力してください。";
        }

        return result;
    }

    /// <summary>
    /// 正規表現を取得します。
    /// </summary>
    /// <returns>正規表現を返します。</returns>
    [GeneratedRegex(@"^[0-9a-f]{8}\-[0-9a-f]{4}\-[0-9a-f]{4}\-[0-9a-f]{4}\-[0-9a-f]{12}$")]
    private static partial Regex GetRegex();

    #endregion
}
