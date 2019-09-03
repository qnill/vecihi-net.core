namespace vecihi.helper.Const
{
    /// <summary>
    /// System messages are designed as 7 characters:
    /// First 2 characters Module Code
    /// Third Character Message Code (E: Error, S: Success, I: Information, W: Warning)
    /// Last 4 characters error code
    /// </summary>
    public static class ApiResultMessages
    {
        // Module:GN
        #region General

        #region Success

        /// <summary>
        /// Successful
        /// - Status Code: Ok
        /// </summary>
        public const string Ok = "Ok";

        #endregion Success

        #region Error

        /// <summary>
        /// No Records Found
        /// - Status Code: NotFound
        /// </summary>
        public const string GNE0001 = "GNE0001";

        /// <summary>
        /// Failed send mail to user
        /// - Status Code: InternalServerError
        /// </summary>
        public const string GNE0002 = "GNE0002";

        /// <summary>
        /// A registration has already been created with this email
        /// - Status Code: BadRequest
        /// </summary>
        public const string GNE0003 = "GNE0003";

        /// <summary>
        /// An error occurred while recording
        /// - Status Code: BadRequest
        /// </summary>
        public const string GNE0004 = "GNE0004";

        #endregion Error

        #region Warning

        /// <summary>
        /// You are not authorized to perform this operation
        /// - Status Code: Unauthorized
        /// </summary>
        public const string GNW0001 = "GNW0001";

        /// <summary>
        /// The corresponding screen code is not found
        /// - Status Code: NotFound
        /// </summary>
        public const string GNW0002 = "GNW0002";

        /// <summary>
        /// No fields were found to be sorted.
        /// - Status Code: NotFound
        /// </summary>
        public const string GNW0003 = "GNW0003";

        #endregion Warning

        #endregion General

        // Module:EM
        #region Email

        #region Warning
        /// <summary>
        /// At least one e-mail address must be selected in order to send e-mail
        /// - Status Code: Unauthorized
        /// </summary>
        public const string EMW0001 = "EMW0001";
        #endregion Warning

        #endregion Email

        // Module:AC
        #region AutoCode

        #region Warning

        /// <summary>
        /// In the code format, {0} must be written so that the automatic incremental code number can be added
        /// - Status Code: NotAcceptable
        /// </summary>
        public const string ACW0001 = "ACW0001";
        #endregion Warning

        #endregion AutoCode

        // Module:FL
        #region File

        #region Error
        /// <summary>
        /// File is not found
        /// - Status Code: NotFound
        /// </summary>
        public const string FLE0001 = "FLE0001";

        /// <summary>
        /// The screenCode and refId field cannot be blank
        /// - Status Code: BadRequest
        /// </summary>
        public const string FLE0002 = "FLE0002";

        #endregion Error

        #region Warning
        /// <summary>
        /// Unsupported file type
        /// - Status Code: UnsupportedMediaType
        /// </summary>
        public const string FLW0001 = "FLW0001";

        #endregion Warning

        #endregion

        // Module:AU
        #region Auth

        #region Error
        /// <summary>
        /// The username or password is incorrect.
        /// - Status Code: BadRequest
        /// </summary>
        public const string AUE0001 = "AUE0001";

        #endregion Error

        #endregion
    }
}
