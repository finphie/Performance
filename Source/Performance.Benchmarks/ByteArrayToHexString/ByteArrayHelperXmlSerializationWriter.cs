using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Performance.Benchmarks.ByteArrayToHexString;

[SuppressMessage("Performance", "CA1812:インスタンス化されていない内部クラスを使用しません", Justification = "誤検知")]
sealed class ByteArrayHelperXmlSerializationWriter : XmlSerializationWriter
{
    public static string ToHexString(byte[] value) => FromByteArrayHex(value);

    protected override void InitCallbacks() => throw new NotSupportedException();
}
