using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace SimpleCsvExporter
{
    /// <summary>
    /// CsvColumn属性の付いたフィールドを対象としてCSVを出力する
    /// </summary>
    public class SimpleCsvExporter
    {
        public  void ExportCsv<T>(IEnumerable<T> dataList, string filePath)
        {
            var targetType = typeof(T);
            // CsvColumn属性が付いたフィールドを取得する
            var targetFieldArray = targetType.GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetCustomAttribute<CsvColumn>() != null)
                .ToArray();

            // カラム名
            var columnNameArray = targetFieldArray.Select(x => x.GetCustomAttribute<CsvColumn>()?.Name)
                .ToArray();

            // CSVを出力する
            using (var writer = new StreamWriter(filePath))
            {
                // ヘッダー
                writer.WriteLine(string.Join(",", columnNameArray));

                // レコード
                foreach (var item in dataList)
                {
                    var valueArray = targetFieldArray.Select(field =>
                    {
                        var dataValueStr = field.GetValue(item)?.ToString() ?? string.Empty;
                        return ToCsvRecordValue(dataValueStr);
                    }).ToArray();

                    writer.WriteLine(string.Join(",", valueArray));
                }
            }
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
