Minimal CSV serializer based on attribute.

## Usage
```
public class SampleClass
{
    [CsvColumn(name: "Value1")]
    public int Value1;

    [CsvColumn(name: "Value2")]
    public string Value2;
}

var dataList = new List<SampleClass>();
// Add records to SampleClass

var exporter = new SimpleCsvExporter.SimpleCsvExporter();
exporter.ExportCsv(dataList, "output path");
```
