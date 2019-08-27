using System;

namespace vecihi.helper.Extensions
{
    public static class GuidExtension
    {
        public static bool IsNullOrEmpty(this Guid? guid)
        {
            return (guid == Guid.Empty || guid == null);
        }
        public static bool IsNullOrEmpty(this Guid guid)
        {
            return (guid == Guid.Empty);
        }
    }
}
