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
            /// String propertylerde kullanılır. 
            /// Default olarak tanımlıdır.
            /// Hiç bir arama tipi atanmazsa equal olarak aramaktadır.
            /// </summary>
            Equal = 0,
            /// <summary>
            /// Sadece String alanlarda kullanılır.
            /// </summary>
            Contains = 1,
            /// <summary>
            /// Sadece Sayısal alanlarda kullanılır.
            /// </summary>
            GreaterThan = 2,
            /// <summary>
            /// Sadece Sayısal alanlarda kullanılır.
            /// </summary>
            GreaterThanOrEqual = 3,
            /// <summary>
            /// Sadece Sayısal alanlarda kullanılır.
            /// </summary>
            LessThan = 4,
            /// <summary>
            /// Sadece Sayısal alanlarda kullanılır.
            /// </summary>
            LessThanOrEqual = 5
        }

        #endregion
    }
}
