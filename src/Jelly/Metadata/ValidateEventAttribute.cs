using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jelly.Metadata
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ValidateEventAttribute : Attribute
    {
        public ValidateEventAttribute(string field)
        {
            Field = field;
        }

        public string Field { get; set; }
    }
}
