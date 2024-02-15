using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Performance.Benchmarks.ByteArrayToHexString;

[SuppressMessage("Maintainability", "CA1515:パブリック型を内部にすることを検討してください")]
public sealed class ByteArrayHelperXmlSerializationWriter : XmlSerializationWriter
{
    public static string ToHexString(byte[] value) => FromByteArrayHex(value);

    protected override void InitCallbacks() => throw new NotSupportedException();
}
