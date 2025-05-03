using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCsvExporter
{
    /// <summary>
    /// 出力対象のフィールドに指定する属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class CsvColumn : Attribute
    {
        public string Name { get; init; }

        public CsvColumn(string name)
        {
            Name = name;
        }
    }
}
