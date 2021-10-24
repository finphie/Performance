﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Benchmarks.CSharp.ByteArrayToHexString;

static class ByteArrayHelperLookup
{
    static readonly int[] Table =
    {
        0x300030, 0x310030, 0x320030, 0x330030, 0x340030, 0x350030, 0x360030, 0x370030, 0x380030, 0x390030,
        0x610030, 0x620030, 0x630030, 0x640030, 0x650030, 0x660030, 0x300031, 0x310031, 0x320031, 0x330031,
        0x340031, 0x350031, 0x360031, 0x370031, 0x380031, 0x390031, 0x610031, 0x620031, 0x630031, 0x640031,
        0x650031, 0x660031, 0x300032, 0x310032, 0x320032, 0x330032, 0x340032, 0x350032, 0x360032, 0x370032,
        0x380032, 0x390032, 0x610032, 0x620032, 0x630032, 0x640032, 0x650032, 0x660032, 0x300033, 0x310033,
        0x320033, 0x330033, 0x340033, 0x350033, 0x360033, 0x370033, 0x380033, 0x390033, 0x610033, 0x620033,
        0x630033, 0x640033, 0x650033, 0x660033, 0x300034, 0x310034, 0x320034, 0x330034, 0x340034, 0x350034,
        0x360034, 0x370034, 0x380034, 0x390034, 0x610034, 0x620034, 0x630034, 0x640034, 0x650034, 0x660034,
        0x300035, 0x310035, 0x320035, 0x330035, 0x340035, 0x350035, 0x360035, 0x370035, 0x380035, 0x390035,
        0x610035, 0x620035, 0x630035, 0x640035, 0x650035, 0x660035, 0x300036, 0x310036, 0x320036, 0x330036,
        0x340036, 0x350036, 0x360036, 0x370036, 0x380036, 0x390036, 0x610036, 0x620036, 0x630036, 0x640036,
        0x650036, 0x660036, 0x300037, 0x310037, 0x320037, 0x330037, 0x340037, 0x350037, 0x360037, 0x370037,
        0x380037, 0x390037, 0x610037, 0x620037, 0x630037, 0x640037, 0x650037, 0x660037, 0x300038, 0x310038,
        0x320038, 0x330038, 0x340038, 0x350038, 0x360038, 0x370038, 0x380038, 0x390038, 0x610038, 0x620038,
        0x630038, 0x640038, 0x650038, 0x660038, 0x300039, 0x310039, 0x320039, 0x330039, 0x340039, 0x350039,
        0x360039, 0x370039, 0x380039, 0x390039, 0x610039, 0x620039, 0x630039, 0x640039, 0x650039, 0x660039,
        0x300061, 0x310061, 0x320061, 0x330061, 0x340061, 0x350061, 0x360061, 0x370061, 0x380061, 0x390061,
        0x610061, 0x620061, 0x630061, 0x640061, 0x650061, 0x660061, 0x300062, 0x310062, 0x320062, 0x330062,
        0x340062, 0x350062, 0x360062, 0x370062, 0x380062, 0x390062, 0x610062, 0x620062, 0x630062, 0x640062,
        0x650062, 0x660062, 0x300063, 0x310063, 0x320063, 0x330063, 0x340063, 0x350063, 0x360063, 0x370063,
        0x380063, 0x390063, 0x610063, 0x620063, 0x630063, 0x640063, 0x650063, 0x660063, 0x300064, 0x310064,
        0x320064, 0x330064, 0x340064, 0x350064, 0x360064, 0x370064, 0x380064, 0x390064, 0x610064, 0x620064,
        0x630064, 0x640064, 0x650064, 0x660064, 0x300065, 0x310065, 0x320065, 0x330065, 0x340065, 0x350065,
        0x360065, 0x370065, 0x380065, 0x390065, 0x610065, 0x620065, 0x630065, 0x640065, 0x650065, 0x660065,
        0x300066, 0x310066, 0x320066, 0x330066, 0x340066, 0x350066, 0x360066, 0x370066, 0x380066, 0x390066,
        0x610066, 0x620066, 0x630066, 0x640066, 0x650066, 0x660066
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToHexString(byte[] source)
    {
        var result = new string(default, source.Length * 2);
        ref var resultStart = ref Unsafe.As<char, int>(ref MemoryMarshal.GetReference(result.AsSpan()));
        ref var tableStart = ref MemoryMarshal.GetArrayDataReference(Table);
        for (var i = 0; i < source.Length; i++)
        {
            Unsafe.Add(ref resultStart, i) = Unsafe.Add(ref tableStart, source[i]);
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe string ToHexStringUnsafe(byte[] source)
    {
        var result = new string(default, source.Length * 2);
        fixed (char* resultPointer = result)
        {
            fixed (int* tablePointer = &Table[0])
            {
                var pointer = (int*)resultPointer;
                for (var i = 0; i < source.Length; i++)
                {
                    pointer[i] = tablePointer[source[i]];
                }
            }
        }

        return result;
    }
}
