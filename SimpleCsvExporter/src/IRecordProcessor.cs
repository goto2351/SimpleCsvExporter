using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCsvExporter
{
    /// <summary>
    /// 出力対象の型とListから出力内容を生成するインターフェース
    /// </summary>
    internal interface IRecordProcessor
    {
        /// <summary>
        /// 出力対象のフィールド名のリストを取得する
        /// </summary>
        public List<string> GetFieldNameArray();

        /// <summary>
        /// 出力対象フィールドの内容のリストを取得する
        /// </summary>
        /// <returns></returns>
        public List<List<string>> GetRecordValueList();
    }
}
