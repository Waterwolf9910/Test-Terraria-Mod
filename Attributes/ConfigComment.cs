using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMod.Attributes {

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    internal class ConfigComment : Attribute {

        public string Comment { get; set; }

        public ConfigComment(string comment) {
            this.Comment = comment;
        }
    }
}
