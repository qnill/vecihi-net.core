using System;

namespace vecihi.helper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class OrderAttribute : Attribute
    {
        public string DbName { get; }
        public bool Ignore { get; }

        public OrderAttribute(string dbName = null, bool ignore = false)
        {
            DbName = dbName;
            Ignore = ignore;
        }
    }
}