using vecihi.helper.Const;
using System.Linq;

namespace vecihi.helper
{
    public static class ScreenCodeValidation
    {
        /// <summary>
        /// Controls whether the screen code is in the <see cref="ScreenCodes"/> class.
        /// </summary>
        /// <param name="screenCode"></param>
        /// <returns></returns>
        public static bool Verification(string screenCode)
        {
            return typeof(ScreenCodes).GetFields().Any(x => x.Name == screenCode);
        }
    }
}