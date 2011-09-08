using System;

namespace ORTS.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BindViewAttribute : Attribute
    {
        private readonly Type _keyDataType;
        public BindViewAttribute(Type type)
        {
            _keyDataType = type;
        }

        public Type GameObjectType
        {
            get { return _keyDataType; }
        }
    }
}
