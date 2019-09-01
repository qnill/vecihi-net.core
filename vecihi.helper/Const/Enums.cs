namespace vecihi.helper.Const
{
    /// <summary>
    /// ToDo : Translate
    /// Enum değerleri asal sayılardan oluşturulmuştur.
    /// Multi seçim sırasında toplam değer, asal bölenlerine ayırılıp seçimler elde edilebilir.
    /// </summary>
    public class Enums
    {
        #region General
        public enum FooDEnum : byte
        {
            fooOne = 2,
            fooTwo = 3,
            fooThree = 5,
            fooFour = 7
        }

        #endregion

        #region SearchType

        public enum SearchType : byte
        {
            /// <summary>
            /// Eşitlik ataması için kullanılır.
            /// Default olarak tanımlıdır.
            /// Hiç bir arama tipi atanmazsa equal olarak aramaktadır.
            /// </summary>
            Equal = 0,
            /// <summary>
            /// Sadece metinsel alanlarda kullanılır. 
            /// Tüm değerin içinde arar.
            /// </summary>
            Contains = 1,
            /// <summary>
            /// Sadece sayısal alanlarda kullanılır.
            /// Değerin büyük olma şartını sağlar.
            /// </summary>
            GreaterThan = 2,
            /// <summary>
            /// Sayısal ve tarih alanlarında kullanılır.
            /// Değerin büyük veya eşit olma şartını sağlar.
            /// </summary>
            GreaterThanOrEqual = 3,
            /// <summary>
            /// Sadece sayısal alanlarda kullanılır.
            /// Değerin küçük olma şartını sağlar.
            /// </summary>
            LessThan = 4,
            /// <summary>
            /// Sayısal ve tarih alanlarında kullanılır.
            /// Değerin küçük veya eşit olma şartını sağlar.
            /// </summary>
            LessThanOrEqual = 5
        }

        #endregion
    }
}
