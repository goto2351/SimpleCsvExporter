using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Text;

namespace SimpleCsvExporter
{
    /// <summary>
    /// 出力対象の型とListから出力内容を生成する
    /// </summary>
    internal class RecordProcessor<T> : IRecordProcessor
    {
        private FieldInfo[] _targetFieldArray;
        private IEnumerable<T> _dataList;

        public RecordProcessor(IEnumerable<T> dataList)
        {
            _dataList = dataList;

            // 対象フィールドの情報を取得
            _targetFieldArray = GetTargetFieldInfo<T>();
        }

        /// <summary>
        /// 出力対象フィールドのFieldInfoを取得する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private FieldInfo[] GetTargetFieldInfo<T>()
        {
            var targetType = typeof(T);
            // CsvColumn属性が付いたフィールドを取得する
            var targetFieldArray = targetType.GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetCustomAttribute<CsvColumn>() != null)
                .ToArray();

            return targetFieldArray;
        }

        /// <summary>
        /// 出力対象のフィールド名のリストを取得する
        /// </summary>
        public List<string> GetFieldNameArray()
        {
            // CsvColumn属性から出力するフィールド名を取得
            return _targetFieldArray.Select(x => x.GetCustomAttribute<CsvColumn>()?.Name ?? string.Empty).ToList();
        }

        /// <summary>
        /// 出力対象フィールドの内容のリストを取得する
        /// </summary>
        /// <returns></returns>
        public List<List<string>> GetRecordValueList()
        {
            var recordsValueList = new List<List<string>>();
            foreach (var record in _dataList)
            {
                // 対象の各フィールドの値を取得して文字列にする
                var valueList = _targetFieldArray.Select(field =>
                {
                    var dataValueStr = field.GetValue(record)?.ToString() ?? string.Empty;
                    return ToCsvRecordValue(dataValueStr);
                }).ToList();

                recordsValueList.Add(valueList);
            }

            return recordsValueList;
        }

        /// <summary>
        /// CSVのレコードに書き込む文字列に変換
        /// </summary>
        private string ToCsvRecordValue(string value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var recordValue = value.Replace(",", "\\,");
            return recordValue;
        }
    }   
}
