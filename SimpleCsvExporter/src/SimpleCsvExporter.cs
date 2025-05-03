using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Cysharp.Text;

namespace SimpleCsvExporter
{
    /// <summary>
    /// CsvColumn属性の付いたフィールドを対象としてCSVを出力する
    /// </summary>
    public class SimpleCsvExporter
    {
        private const string SEPARATOR = ",";

        public  void ExportCsv<T>(IEnumerable<T> dataList, string filePath)
        {
            // CsvColumn属性が付いたフィールドを取得する
            var targetFieldArray = GetTargeFields<T>();

            // CSVの内容を作成する
            var csvStringBuilder = ZString.CreateStringBuilder();

            // ヘッダー
            csvStringBuilder.AppendLine(CreateHeader(targetFieldArray));
            // レコード
            csvStringBuilder.AppendLine(CreateRecord<T>(targetFieldArray, dataList));

            // CSVを出力する
            using (var writer = new StreamWriter(filePath))
            {
                writer.Write(csvStringBuilder.ToString());
            }
        }

        /// <summary>
        /// 出力対象のフィールドのFieldInfoを取得
        /// </summary>
        private FieldInfo[] GetTargeFields<T>()
        {
            var targetType = typeof(T);
            // CsvColumn属性が付いたフィールドを取得する
            var targetFieldArray = targetType.GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetCustomAttribute<CsvColumn>() != null)
                .ToArray();

            return targetFieldArray;
        }

        /// <summary>
        /// ヘッダー行を作成
        /// </summary>
        private string CreateHeader(FieldInfo[] fieldArray)
        {
            var columnNameArray = fieldArray.Select(x => x.GetCustomAttribute<CsvColumn>()?.Name).ToArray();
            return ZString.Join(SEPARATOR, columnNameArray);
        }

        /// <summary>
        /// レコード行を作成
        /// </summary>
        private string CreateRecord<T>(FieldInfo[] fieldArray, IEnumerable<T> dataList)
        {
            var recordStringBuilder = ZString.CreateStringBuilder();

            foreach (var item in dataList)
            {
                var valueArray = fieldArray.Select(field =>
                {
                    var dataValueStr = field.GetValue(item)?.ToString() ?? string.Empty;
                    return ToCsvRecordValue(dataValueStr);
                }).ToArray();

                recordStringBuilder.AppendLine(ZString.Join(SEPARATOR, valueArray));
            }

            return recordStringBuilder.ToString();
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
