using System;
using System.Collections.Generic;
using System.Text;

namespace SiMay.Core
{
    public class ApplicationNameAttribute : Attribute
    {
        public string Name { get; set; }
        public ApplicationNameAttribute(string name) => Name = name;
    }
}
