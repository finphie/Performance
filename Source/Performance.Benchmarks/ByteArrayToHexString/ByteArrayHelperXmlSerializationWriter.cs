using System.Xml.Serialization;

namespace Benchmarks.CSharp.ByteArrayToHexString;

public sealed class ByteArrayHelperXmlSerializationWriter : XmlSerializationWriter
{
    public static string ToHexString(byte[] value) => FromByteArrayHex(value);

    protected override void InitCallbacks() => throw new NotSupportedException();
}
