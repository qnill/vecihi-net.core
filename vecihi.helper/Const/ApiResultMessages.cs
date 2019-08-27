namespace vecihi.helper.Const
{
    /// <summary>
    /// ToDo : Translate
    /// Sistem mesajları 7 karakter olarak tasarlanmıştır:
    /// Ilk 2 karakter Modül Kodu
    /// 3. Karakter Mesaj Kodu (E:Error, S:Success, I:Information, W:Warning)
    /// Son 4 karakter hata kodu
    /// </summary>
    public static class ApiResultMessages
    {
        ///Modul kodu : 'GN'
        #region General

        #region Success

        /// <summary>
        /// İşlem başarılı
        /// - Status Code: Ok
        /// </summary>
        public const string Ok = "Ok";

        #endregion Success

        #region Error

        /// <summary>
        /// Kayıt bulunamadı 
        /// - Status Code: NotFound
        /// </summary>
        public const string GNE0001 = "GNE0001";

        /// <summary>
        ///  Kullanıcıya mail gönderilemedi
        /// - Status Code: InternalServerError
        /// </summary>
        public const string GNE0002 = "GNE0002";

        /// <summary>
        ///  Bu email ile daha önce bir kayıt oluşturulmuş.
        /// - Status Code: BadRequest
        /// </summary>
        public const string GNE0003 = "GNE0003";

        /// <summary>
        ///  Kayıt sırasında hata oluştu.
        /// - Status Code: BadRequest
        /// </summary>
        public const string GNE0004 = "GNE0004";

        #endregion Error

        #region Warning

        /// <summary>
        /// Bu işlemi yapmak için yetki sahibi değilsiniz 
        /// - Status Code: Unauthorized
        /// </summary>
        public const string GNW0001 = "GNW0001";

        /// <summary>
        /// İlgili ekran kodu bulunamadı.
        /// - Status Code: NotFound
        /// </summary>
        public const string GNW0002 = "GNW0002";

        /// <summary>
        /// Sıralanmak istenilen alan bulunamadı.
        /// - Status Code: NotFound
        /// </summary>
        public const string GNW0003 = "GNW0003";

        #endregion Warning

        #endregion General

        ///Modul kodu : 'EM'
        #region Email

        #region Warning
        /// <summary>
        /// Mail gönderilmek üzere en az bir mail adresi seçilmek zorundadır.
        /// - Status Code: Unauthorized
        /// </summary>
        public const string EMW0001 = "EMW0001";
        #endregion Warning

        #endregion Email

        ///Modul kodu : 'AC'
        #region AutoCode

        #region Warning

        /// <summary>
        /// Kod formatının içine, otomatik artan kod numarasının eklenebilmesi için {0} yazılmalıdır.
        /// - Status Code: NotAcceptable
        /// </summary>
        public const string ACW0001 = "ACW0001";
        #endregion Warning

        #endregion AutoCode

        ///Modul kodu : 'FU'
        #region FileUpload

        #region Error
        /// <summary>
        /// Dosya bulunamadı.
        /// - Status Code: NotFound
        /// </summary>
        public const string FUE0001 = "FUE0001";

        /// <summary>
        /// screenCode ve refId alanı boş geçilemez.
        /// - Status Code: BadRequest
        /// </summary>
        public const string FUE0002 = "FUE0002";

        #endregion Error

        #region Warning
        /// <summary>
        ///  Desteklenmeyen dosya türü.
        /// - Status Code: UnsupportedMediaType
        /// </summary>
        public const string FUW0001 = "FUW0001";

        #endregion Warning

        #endregion
    }
}
