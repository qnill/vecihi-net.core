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

        #endregion

        #region Error

        /// <summary>
        /// No Records Found
        /// - Status Code: NotFound
        /// </summary>
        public const string GNE0001 = "GNE0001";

        /// <summary>
        /// An error occurred while recording
        /// - Status Code: BadRequest
        /// </summary>
        public const string GNE0002 = "GNE0002";

        #endregion

        #region Warning
        
        /// <summary>
        /// You are not authorized to perform this operation
        /// - Status Code: Unauthorized
        /// </summary>
        public const string GNW0001 = "GNW0001";

        #endregion

        #endregion

        // Module:EM
        #region Email

        #region Error

        /// <summary>
        /// There was an error sending the email.
        /// - Status Code: Unauthorized
        /// </summary>
        public const string EME0001 = "EME0001";

        #endregion

        #region Warning

        /// <summary>
        /// At least one e-mail address must be selected in order to send e-mail
        /// - Status Code: Unauthorized
        /// </summary>
        public const string EMW0001 = "EMW0001";

        #endregion

        #endregion

        // Module:AC
        #region AutoCode

        #region Warning

        /// <summary>
        /// In the code format, {0} must be written so that the automatic incremental code number can be added
        /// - Status Code: NotAcceptable
        /// </summary>
        public const string ACW0001 = "ACW0001";

        /// <summary>
        /// The corresponding screen code is not found
        /// - Status Code: NotFound
        /// </summary>
        public const string ACW0002 = "ACW0002";

        #endregion

        #endregion

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

        #endregion

        #region Warning

        /// <summary>
        /// Unsupported file type
        /// - Status Code: UnsupportedMediaType
        /// </summary>
        public const string FLW0001 = "FLW0001";

        #endregion

        #endregion

        // Module:AU
        #region Auth

        #region Error

        /// <summary>
        /// The username or password is incorrect.
        /// - Status Code: BadRequest
        /// </summary>
        public const string AUE0001 = "AUE0001";

        /// <summary>
        /// Invalid Token.
        /// - Status Code: BadRequest
        /// </summary>
        public const string AUE0002 = "AUE0002";

        /// <summary>
        /// This refresh token does not exist.
        /// - Status Code: BadRequest
        /// </summary>
        public const string AUE0003 = "AUE0003";

        /// <summary>
        /// This refresh token has expired.
        /// - Status Code: BadRequest
        /// </summary>
        public const string AUE0004 = "AUE0004";

        /// <summary>
        /// This refresh token has been used.
        /// - Status Code: BadRequest
        /// </summary>
        public const string AUE0005 = "AUE0005";

        /// <summary>
        /// This refresh token does not match this JWT.
        /// - Status Code: BadRequest
        /// </summary>
        public const string AUE0006 = "AUE0006";

        /// <summary>
        /// Email not confirmed, please check your email address.
        /// - Status Code: BadRequest
        /// </summary>
        public const string AUE0007 = "AUE0007";

        /// <summary>
        /// This mail has already been activated.
        /// - Status Code: BadRequest
        /// </summary>
        public const string AUE0008 = "AUE0008";

        #endregion

        #endregion
    }
}
