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
        public class Setting
        {
            public string Separator { get; init; }

            private const string DEFAULT_SEPARATOR = ",";

            public Setting(string separator = DEFAULT_SEPARATOR)
            {
                Separator = separator;
            }
        }

        private Setting _setting;

        public  SimpleCsvExporter(Setting setting = null)
        {
            _setting = setting != null ? setting : new Setting();
        }

        public  void ExportCsv<T>(IEnumerable<T> dataList, string filePath)
        {
            var recordProcessor = new RecordProcessor<T>(dataList);

            // CSVの内容を作成する
            var csvStringBuilder = ZString.CreateStringBuilder();

            // ヘッダー
            csvStringBuilder.AppendLine(ZString.Join(_setting.Separator, recordProcessor.GetFieldNameList()));

            // レコード
            var recordValueList = recordProcessor.GetRecordValueList();
            foreach (var record in recordValueList)
            {
                csvStringBuilder.AppendLine(ZString.Join(_setting.Separator, record));
            }

            // CSVを出力する
            using (var writer = new StreamWriter(filePath))
            {
                writer.Write(csvStringBuilder.ToString());
            }
        }
    }
}
