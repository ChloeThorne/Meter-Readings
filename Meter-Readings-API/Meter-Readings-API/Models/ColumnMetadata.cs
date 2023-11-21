using System.Reflection;

namespace Meter_Readings_API.Models
{
    public class ColumnMetadata
    {
        public MethodInfo SetMethod { get; set; }
        public Type PropertyType { get; set; }
        public ColumnMetadata() { }
        public ColumnMetadata(MethodInfo methodInfo, Type propertyType)
        {
            SetMethod = methodInfo;
            PropertyType = propertyType;
        }
    }
}
