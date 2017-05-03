
//  Microsoft (R) .NET Framework IL Disassembler.  Version 4.6.1055.0
//  Copyright (c) Microsoft Corporation.  All rights reserved.



// Metadata version: v4.0.30319
.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern System
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly Xeora.Web.RegularExpressions
{
  .custom instance void [mscorlib]System.Security.SecurityTransparentAttribute::.ctor() = ( 01 00 00 00 ) 
  .custom instance void [mscorlib]System.Security.SecurityRulesAttribute::.ctor(valuetype [mscorlib]System.Security.SecurityRuleSet) = ( 01 00 02 00 00 ) 
  .hash algorithm 0x00008004
  .ver 6:0:0:1002
}
.module Xeora.Web.RegularExpressions.dll
// MVID: {D4550B26-DBD3-408F-B7A9-C01DB43A1424}
.imagebase 0x00400000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000001    //  ILONLY
// Image base: 0x023C0000


// =============== CLASS MEMBERS DECLARATION ===================

.class private auto ansi Xeora.Web.RegularExpressions.MainCapturePatternRunner1
       extends [System]System.Text.RegularExpressions.RegexRunner
{
  .method public virtual instance void  Go() cil managed
  {
    // Code size       11354 (0x2c5a)
    .maxstack  337
    .locals init (int32 V_0,
             string V_1,
             int32 V_2,
             int32[] V_3,
             int32 V_4,
             int32[] V_5,
             int32 V_6,
             int32 V_7,
             int32 V_8,
             int32 V_9,
             int32 V_10,
             int32 V_11)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string [System]System.Text.RegularExpressions.RegexRunner::runtext
    IL_0006:  stloc.s    V_1
    IL_0008:  ldarg.0
    IL_0009:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextstart
    IL_000e:  stloc.s    V_11
    IL_0010:  ldarg.0
    IL_0011:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextbeg
    IL_0016:  stloc.s    V_9
    IL_0018:  ldarg.0
    IL_0019:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextend
    IL_001e:  stloc.s    V_10
    IL_0020:  ldarg.0
    IL_0021:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextpos
    IL_0026:  stloc.s    V_0
    IL_0028:  ldarg.0
    IL_0029:  ldfld      int32[] [System]System.Text.RegularExpressions.RegexRunner::runtrack
    IL_002e:  stloc.s    V_3
    IL_0030:  ldarg.0
    IL_0031:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtrackpos
    IL_0036:  stloc.s    V_2
    IL_0038:  ldarg.0
    IL_0039:  ldfld      int32[] [System]System.Text.RegularExpressions.RegexRunner::runstack
    IL_003e:  stloc.s    V_5
    IL_0040:  ldarg.0
    IL_0041:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runstackpos
    IL_0046:  stloc.s    V_4
    IL_0048:  ldarg.0
    IL_0049:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_004e:  ldloc.s    V_3
    IL_0050:  ldloc.s    V_2
    IL_0052:  ldc.i4.1
    IL_0053:  sub
    IL_0054:  dup
    IL_0055:  stloc.s    V_2
    IL_0057:  ldloc.s    V_0
    IL_0059:  stelem.i4
    IL_005a:  ldloc.s    V_3
    IL_005c:  ldloc.s    V_2
    IL_005e:  ldc.i4.1
    IL_005f:  sub
    IL_0060:  dup
    IL_0061:  stloc.s    V_2
    IL_0063:  ldc.i4.s   0
    IL_0065:  stelem.i4
    IL_0066:  ldarg.0
    IL_0067:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_006c:  ldloc.s    V_5
    IL_006e:  ldloc.s    V_4
    IL_0070:  ldc.i4.1
    IL_0071:  sub
    IL_0072:  dup
    IL_0073:  stloc.s    V_4
    IL_0075:  ldloc.s    V_0
    IL_0077:  stelem.i4
    IL_0078:  ldloc.s    V_3
    IL_007a:  ldloc.s    V_2
    IL_007c:  ldc.i4.1
    IL_007d:  sub
    IL_007e:  dup
    IL_007f:  stloc.s    V_2
    IL_0081:  ldc.i4.s   1
    IL_0083:  stelem.i4
    IL_0084:  ldarg.0
    IL_0085:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_008a:  ldloc.s    V_3
    IL_008c:  ldloc.s    V_2
    IL_008e:  ldc.i4.1
    IL_008f:  sub
    IL_0090:  dup
    IL_0091:  stloc.s    V_2
    IL_0093:  ldloc.s    V_0
    IL_0095:  stelem.i4
    IL_0096:  ldloc.s    V_3
    IL_0098:  ldloc.s    V_2
    IL_009a:  ldc.i4.1
    IL_009b:  sub
    IL_009c:  dup
    IL_009d:  stloc.s    V_2
    IL_009f:  ldc.i4.s   2
    IL_00a1:  stelem.i4
    IL_00a2:  ldarg.0
    IL_00a3:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_00a8:  ldloc.s    V_0
    IL_00aa:  ldloc.s    V_10
    IL_00ac:  bge        IL_200e

    IL_00b1:  ldloc.s    V_1
    IL_00b3:  ldloc.s    V_0
    IL_00b5:  dup
    IL_00b6:  ldc.i4.s   1
    IL_00b8:  add
    IL_00b9:  stloc.s    V_0
    IL_00bb:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_00c0:  ldc.i4.s   36
    IL_00c2:  bne.un     IL_200e

    IL_00c7:  ldarg.0
    IL_00c8:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_00cd:  ldloc.s    V_5
    IL_00cf:  ldloc.s    V_4
    IL_00d1:  ldc.i4.1
    IL_00d2:  sub
    IL_00d3:  dup
    IL_00d4:  stloc.s    V_4
    IL_00d6:  ldloc.s    V_0
    IL_00d8:  stelem.i4
    IL_00d9:  ldloc.s    V_3
    IL_00db:  ldloc.s    V_2
    IL_00dd:  ldc.i4.1
    IL_00de:  sub
    IL_00df:  dup
    IL_00e0:  stloc.s    V_2
    IL_00e2:  ldc.i4.s   1
    IL_00e4:  stelem.i4
    IL_00e5:  ldarg.0
    IL_00e6:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_00eb:  ldloc.s    V_3
    IL_00ed:  ldloc.s    V_2
    IL_00ef:  ldc.i4.1
    IL_00f0:  sub
    IL_00f1:  dup
    IL_00f2:  stloc.s    V_2
    IL_00f4:  ldloc.s    V_0
    IL_00f6:  stelem.i4
    IL_00f7:  ldloc.s    V_3
    IL_00f9:  ldloc.s    V_2
    IL_00fb:  ldc.i4.1
    IL_00fc:  sub
    IL_00fd:  dup
    IL_00fe:  stloc.s    V_2
    IL_0100:  ldc.i4.s   3
    IL_0102:  stelem.i4
    IL_0103:  ldarg.0
    IL_0104:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0109:  ldloc.s    V_5
    IL_010b:  ldloc.s    V_4
    IL_010d:  ldc.i4.1
    IL_010e:  sub
    IL_010f:  dup
    IL_0110:  stloc.s    V_4
    IL_0112:  ldloc.s    V_0
    IL_0114:  stelem.i4
    IL_0115:  ldloc.s    V_3
    IL_0117:  ldloc.s    V_2
    IL_0119:  ldc.i4.1
    IL_011a:  sub
    IL_011b:  dup
    IL_011c:  stloc.s    V_2
    IL_011e:  ldc.i4.s   1
    IL_0120:  stelem.i4
    IL_0121:  ldarg.0
    IL_0122:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0127:  ldloc.s    V_3
    IL_0129:  ldloc.s    V_2
    IL_012b:  ldc.i4.1
    IL_012c:  sub
    IL_012d:  dup
    IL_012e:  stloc.s    V_2
    IL_0130:  ldloc.s    V_0
    IL_0132:  stelem.i4
    IL_0133:  ldloc.s    V_3
    IL_0135:  ldloc.s    V_2
    IL_0137:  ldc.i4.1
    IL_0138:  sub
    IL_0139:  dup
    IL_013a:  stloc.s    V_2
    IL_013c:  ldc.i4.s   4
    IL_013e:  stelem.i4
    IL_013f:  ldarg.0
    IL_0140:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0145:  ldloc.s    V_5
    IL_0147:  ldloc.s    V_4
    IL_0149:  ldc.i4.1
    IL_014a:  sub
    IL_014b:  dup
    IL_014c:  stloc.s    V_4
    IL_014e:  ldc.i4.s   -1
    IL_0150:  stelem.i4
    IL_0151:  ldloc.s    V_5
    IL_0153:  ldloc.s    V_4
    IL_0155:  ldc.i4.1
    IL_0156:  sub
    IL_0157:  dup
    IL_0158:  stloc.s    V_4
    IL_015a:  ldc.i4.s   0
    IL_015c:  stelem.i4
    IL_015d:  ldloc.s    V_3
    IL_015f:  ldloc.s    V_2
    IL_0161:  ldc.i4.1
    IL_0162:  sub
    IL_0163:  dup
    IL_0164:  stloc.s    V_2
    IL_0166:  ldc.i4.s   5
    IL_0168:  stelem.i4
    IL_0169:  ldarg.0
    IL_016a:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_016f:  br         IL_02c6

    IL_0174:  ldarg.0
    IL_0175:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_017a:  ldloc.s    V_5
    IL_017c:  ldloc.s    V_4
    IL_017e:  ldc.i4.1
    IL_017f:  sub
    IL_0180:  dup
    IL_0181:  stloc.s    V_4
    IL_0183:  ldloc.s    V_0
    IL_0185:  stelem.i4
    IL_0186:  ldloc.s    V_3
    IL_0188:  ldloc.s    V_2
    IL_018a:  ldc.i4.1
    IL_018b:  sub
    IL_018c:  dup
    IL_018d:  stloc.s    V_2
    IL_018f:  ldc.i4.s   1
    IL_0191:  stelem.i4
    IL_0192:  ldarg.0
    IL_0193:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0198:  ldloc.s    V_3
    IL_019a:  ldloc.s    V_2
    IL_019c:  ldc.i4.1
    IL_019d:  sub
    IL_019e:  dup
    IL_019f:  stloc.s    V_2
    IL_01a1:  ldloc.s    V_0
    IL_01a3:  stelem.i4
    IL_01a4:  ldloc.s    V_3
    IL_01a6:  ldloc.s    V_2
    IL_01a8:  ldc.i4.1
    IL_01a9:  sub
    IL_01aa:  dup
    IL_01ab:  stloc.s    V_2
    IL_01ad:  ldc.i4.s   6
    IL_01af:  stelem.i4
    IL_01b0:  ldarg.0
    IL_01b1:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_01b6:  ldc.i4.s   1
    IL_01b8:  ldloc.s    V_10
    IL_01ba:  ldloc.s    V_0
    IL_01bc:  sub
    IL_01bd:  bgt        IL_200e

    IL_01c2:  ldloc.s    V_0
    IL_01c4:  ldc.i4.s   1
    IL_01c6:  add
    IL_01c7:  stloc.s    V_0
    IL_01c9:  ldc.i4.s   1
    IL_01cb:  stloc.s    V_6
    IL_01cd:  ldloc.s    V_1
    IL_01cf:  ldloc.s    V_0
    IL_01d1:  ldloc.s    V_6
    IL_01d3:  dup
    IL_01d4:  ldc.i4.s   1
    IL_01d6:  sub
    IL_01d7:  stloc.s    V_6
    IL_01d9:  sub
    IL_01da:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_01df:  ldc.i4.s   35
    IL_01e1:  bne.un     IL_200e

    IL_01e6:  ldloc.s    V_6
    IL_01e8:  ldc.i4.s   0
    IL_01ea:  bgt.s      IL_01cd

    IL_01ec:  ldarg.0
    IL_01ed:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_01f2:  ldloc.s    V_10
    IL_01f4:  ldloc.s    V_0
    IL_01f6:  sub
    IL_01f7:  dup
    IL_01f8:  stloc.s    V_7
    IL_01fa:  ldc.i4.s   1
    IL_01fc:  add
    IL_01fd:  stloc.s    V_6
    IL_01ff:  ldloc.s    V_6
    IL_0201:  ldc.i4.s   1
    IL_0203:  sub
    IL_0204:  dup
    IL_0205:  stloc.s    V_6
    IL_0207:  ldc.i4.s   0
    IL_0209:  ble.s      IL_0225

    IL_020b:  ldloc.s    V_1
    IL_020d:  ldloc.s    V_0
    IL_020f:  dup
    IL_0210:  ldc.i4.s   1
    IL_0212:  add
    IL_0213:  stloc.s    V_0
    IL_0215:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_021a:  ldc.i4.s   35
    IL_021c:  beq.s      IL_01ff

    IL_021e:  ldloc.s    V_0
    IL_0220:  ldc.i4.s   1
    IL_0222:  sub
    IL_0223:  stloc.s    V_0
    IL_0225:  ldloc.s    V_7
    IL_0227:  ldloc.s    V_6
    IL_0229:  ble.s      IL_0258

    IL_022b:  ldloc.s    V_3
    IL_022d:  ldloc.s    V_2
    IL_022f:  ldc.i4.1
    IL_0230:  sub
    IL_0231:  dup
    IL_0232:  stloc.s    V_2
    IL_0234:  ldloc.s    V_7
    IL_0236:  ldloc.s    V_6
    IL_0238:  sub
    IL_0239:  ldc.i4.s   1
    IL_023b:  sub
    IL_023c:  stelem.i4
    IL_023d:  ldloc.s    V_3
    IL_023f:  ldloc.s    V_2
    IL_0241:  ldc.i4.1
    IL_0242:  sub
    IL_0243:  dup
    IL_0244:  stloc.s    V_2
    IL_0246:  ldloc.s    V_0
    IL_0248:  ldc.i4.s   1
    IL_024a:  sub
    IL_024b:  stelem.i4
    IL_024c:  ldloc.s    V_3
    IL_024e:  ldloc.s    V_2
    IL_0250:  ldc.i4.1
    IL_0251:  sub
    IL_0252:  dup
    IL_0253:  stloc.s    V_2
    IL_0255:  ldc.i4.s   7
    IL_0257:  stelem.i4
    IL_0258:  ldarg.0
    IL_0259:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_025e:  br         IL_0290

    IL_0263:  ldarg.0
    IL_0264:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0269:  ldloc.s    V_0
    IL_026b:  ldloc.s    V_10
    IL_026d:  bge        IL_200e

    IL_0272:  ldloc.s    V_1
    IL_0274:  ldloc.s    V_0
    IL_0276:  dup
    IL_0277:  ldc.i4.s   1
    IL_0279:  add
    IL_027a:  stloc.s    V_0
    IL_027c:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0281:  ldstr      bytearray (00 00 08 00 00 00 2A 00 2C 00 2D 00 2E 00 5E 00   // ......*.,.-...^.
                                    5F 00 7E 00 7F 00 )                               // _.~...
    IL_0286:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_028b:  brfalse    IL_200e

    IL_0290:  ldarg.0
    IL_0291:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0296:  ldloc.s    V_5
    IL_0298:  ldloc.s    V_4
    IL_029a:  dup
    IL_029b:  ldc.i4.1
    IL_029c:  add
    IL_029d:  stloc.s    V_4
    IL_029f:  ldelem.i4
    IL_02a0:  stloc.s    V_6
    IL_02a2:  ldarg.0
    IL_02a3:  ldc.i4.s   3
    IL_02a5:  ldloc.s    V_6
    IL_02a7:  ldloc.s    V_0
    IL_02a9:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_02ae:  ldloc.s    V_3
    IL_02b0:  ldloc.s    V_2
    IL_02b2:  ldc.i4.1
    IL_02b3:  sub
    IL_02b4:  dup
    IL_02b5:  stloc.s    V_2
    IL_02b7:  ldloc.s    V_6
    IL_02b9:  stelem.i4
    IL_02ba:  ldloc.s    V_3
    IL_02bc:  ldloc.s    V_2
    IL_02be:  ldc.i4.1
    IL_02bf:  sub
    IL_02c0:  dup
    IL_02c1:  stloc.s    V_2
    IL_02c3:  ldc.i4.s   8
    IL_02c5:  stelem.i4
    IL_02c6:  ldarg.0
    IL_02c7:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_02cc:  ldloc.s    V_5
    IL_02ce:  ldloc.s    V_4
    IL_02d0:  dup
    IL_02d1:  ldc.i4.1
    IL_02d2:  add
    IL_02d3:  stloc.s    V_4
    IL_02d5:  ldelem.i4
    IL_02d6:  stloc.s    V_6
    IL_02d8:  ldloc.s    V_5
    IL_02da:  ldloc.s    V_4
    IL_02dc:  dup
    IL_02dd:  ldc.i4.1
    IL_02de:  add
    IL_02df:  stloc.s    V_4
    IL_02e1:  ldelem.i4
    IL_02e2:  dup
    IL_02e3:  stloc.s    V_7
    IL_02e5:  ldloc.s    V_3
    IL_02e7:  ldloc.s    V_2
    IL_02e9:  ldc.i4.1
    IL_02ea:  sub
    IL_02eb:  dup
    IL_02ec:  stloc.s    V_2
    IL_02ee:  ldloc.s    V_7
    IL_02f0:  stelem.i4
    IL_02f1:  ldloc.s    V_0
    IL_02f3:  bne.un.s   IL_02fb

    IL_02f5:  ldloc.s    V_6
    IL_02f7:  ldc.i4.s   0
    IL_02f9:  bge.s      IL_034e

    IL_02fb:  ldloc.s    V_6
    IL_02fd:  ldc.i4.s   1
    IL_02ff:  bge.s      IL_034e

    IL_0301:  ldloc.s    V_5
    IL_0303:  ldloc.s    V_4
    IL_0305:  ldc.i4.1
    IL_0306:  sub
    IL_0307:  dup
    IL_0308:  stloc.s    V_4
    IL_030a:  ldloc.s    V_0
    IL_030c:  stelem.i4
    IL_030d:  ldloc.s    V_5
    IL_030f:  ldloc.s    V_4
    IL_0311:  ldc.i4.1
    IL_0312:  sub
    IL_0313:  dup
    IL_0314:  stloc.s    V_4
    IL_0316:  ldloc.s    V_6
    IL_0318:  ldc.i4.s   1
    IL_031a:  add
    IL_031b:  stelem.i4
    IL_031c:  ldloc.s    V_3
    IL_031e:  ldloc.s    V_2
    IL_0320:  ldc.i4.1
    IL_0321:  sub
    IL_0322:  dup
    IL_0323:  stloc.s    V_2
    IL_0325:  ldc.i4.s   9
    IL_0327:  stelem.i4
    IL_0328:  ldloc.s    V_2
    IL_032a:  ldc.i4     0x188
    IL_032f:  ble.s      IL_033d

    IL_0331:  ldloc.s    V_4
    IL_0333:  ldc.i4     0x126
    IL_0338:  bgt        IL_0174

    IL_033d:  ldloc.s    V_3
    IL_033f:  ldloc.s    V_2
    IL_0341:  ldc.i4.1
    IL_0342:  sub
    IL_0343:  dup
    IL_0344:  stloc.s    V_2
    IL_0346:  ldc.i4.s   10
    IL_0348:  stelem.i4
    IL_0349:  br         IL_200e

    IL_034e:  ldloc.s    V_3
    IL_0350:  ldloc.s    V_2
    IL_0352:  ldc.i4.1
    IL_0353:  sub
    IL_0354:  dup
    IL_0355:  stloc.s    V_2
    IL_0357:  ldloc.s    V_6
    IL_0359:  stelem.i4
    IL_035a:  ldloc.s    V_3
    IL_035c:  ldloc.s    V_2
    IL_035e:  ldc.i4.1
    IL_035f:  sub
    IL_0360:  dup
    IL_0361:  stloc.s    V_2
    IL_0363:  ldc.i4.s   11
    IL_0365:  stelem.i4
    IL_0366:  ldarg.0
    IL_0367:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_036c:  ldloc.s    V_5
    IL_036e:  ldloc.s    V_4
    IL_0370:  ldc.i4.1
    IL_0371:  sub
    IL_0372:  dup
    IL_0373:  stloc.s    V_4
    IL_0375:  ldloc.s    V_0
    IL_0377:  stelem.i4
    IL_0378:  ldloc.s    V_3
    IL_037a:  ldloc.s    V_2
    IL_037c:  ldc.i4.1
    IL_037d:  sub
    IL_037e:  dup
    IL_037f:  stloc.s    V_2
    IL_0381:  ldc.i4.s   1
    IL_0383:  stelem.i4
    IL_0384:  ldarg.0
    IL_0385:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_038a:  ldc.i4.s   1
    IL_038c:  ldloc.s    V_10
    IL_038e:  ldloc.s    V_0
    IL_0390:  sub
    IL_0391:  bgt        IL_200e

    IL_0396:  ldloc.s    V_0
    IL_0398:  ldc.i4.s   1
    IL_039a:  add
    IL_039b:  stloc.s    V_0
    IL_039d:  ldc.i4.s   1
    IL_039f:  stloc.s    V_6
    IL_03a1:  ldloc.s    V_1
    IL_03a3:  ldloc.s    V_0
    IL_03a5:  ldloc.s    V_6
    IL_03a7:  dup
    IL_03a8:  ldc.i4.s   1
    IL_03aa:  sub
    IL_03ab:  stloc.s    V_6
    IL_03ad:  sub
    IL_03ae:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_03b3:  ldstr      bytearray (00 00 00 00 0A 00 00 00 02 00 04 00 05 00 03 00 
                                    01 00 06 00 09 00 13 00 00 00 ) 
    IL_03b8:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_03bd:  brfalse    IL_200e

    IL_03c2:  ldloc.s    V_6
    IL_03c4:  ldc.i4.s   0
    IL_03c6:  bgt        IL_03a1

    IL_03cb:  ldarg.0
    IL_03cc:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_03d1:  ldloc.s    V_10
    IL_03d3:  ldloc.s    V_0
    IL_03d5:  sub
    IL_03d6:  dup
    IL_03d7:  stloc.s    V_7
    IL_03d9:  ldc.i4.s   1
    IL_03db:  add
    IL_03dc:  stloc.s    V_6
    IL_03de:  ldloc.s    V_6
    IL_03e0:  ldc.i4.s   1
    IL_03e2:  sub
    IL_03e3:  dup
    IL_03e4:  stloc.s    V_6
    IL_03e6:  ldc.i4.s   0
    IL_03e8:  ble        IL_0412

    IL_03ed:  ldloc.s    V_1
    IL_03ef:  ldloc.s    V_0
    IL_03f1:  dup
    IL_03f2:  ldc.i4.s   1
    IL_03f4:  add
    IL_03f5:  stloc.s    V_0
    IL_03f7:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_03fc:  ldstr      bytearray (00 00 00 00 0A 00 00 00 02 00 04 00 05 00 03 00 
                                    01 00 06 00 09 00 13 00 00 00 ) 
    IL_0401:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0406:  brtrue     IL_03de

    IL_040b:  ldloc.s    V_0
    IL_040d:  ldc.i4.s   1
    IL_040f:  sub
    IL_0410:  stloc.s    V_0
    IL_0412:  ldloc.s    V_7
    IL_0414:  ldloc.s    V_6
    IL_0416:  ble.s      IL_0445

    IL_0418:  ldloc.s    V_3
    IL_041a:  ldloc.s    V_2
    IL_041c:  ldc.i4.1
    IL_041d:  sub
    IL_041e:  dup
    IL_041f:  stloc.s    V_2
    IL_0421:  ldloc.s    V_7
    IL_0423:  ldloc.s    V_6
    IL_0425:  sub
    IL_0426:  ldc.i4.s   1
    IL_0428:  sub
    IL_0429:  stelem.i4
    IL_042a:  ldloc.s    V_3
    IL_042c:  ldloc.s    V_2
    IL_042e:  ldc.i4.1
    IL_042f:  sub
    IL_0430:  dup
    IL_0431:  stloc.s    V_2
    IL_0433:  ldloc.s    V_0
    IL_0435:  ldc.i4.s   1
    IL_0437:  sub
    IL_0438:  stelem.i4
    IL_0439:  ldloc.s    V_3
    IL_043b:  ldloc.s    V_2
    IL_043d:  ldc.i4.1
    IL_043e:  sub
    IL_043f:  dup
    IL_0440:  stloc.s    V_2
    IL_0442:  ldc.i4.s   12
    IL_0444:  stelem.i4
    IL_0445:  ldarg.0
    IL_0446:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_044b:  ldloc.s    V_5
    IL_044d:  ldloc.s    V_4
    IL_044f:  dup
    IL_0450:  ldc.i4.1
    IL_0451:  add
    IL_0452:  stloc.s    V_4
    IL_0454:  ldelem.i4
    IL_0455:  stloc.s    V_6
    IL_0457:  ldarg.0
    IL_0458:  ldc.i4.s   4
    IL_045a:  ldloc.s    V_6
    IL_045c:  ldloc.s    V_0
    IL_045e:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_0463:  ldloc.s    V_3
    IL_0465:  ldloc.s    V_2
    IL_0467:  ldc.i4.1
    IL_0468:  sub
    IL_0469:  dup
    IL_046a:  stloc.s    V_2
    IL_046c:  ldloc.s    V_6
    IL_046e:  stelem.i4
    IL_046f:  ldloc.s    V_3
    IL_0471:  ldloc.s    V_2
    IL_0473:  ldc.i4.1
    IL_0474:  sub
    IL_0475:  dup
    IL_0476:  stloc.s    V_2
    IL_0478:  ldc.i4.s   8
    IL_047a:  stelem.i4
    IL_047b:  ldarg.0
    IL_047c:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0481:  br         IL_0720

    IL_0486:  ldarg.0
    IL_0487:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_048c:  ldloc.s    V_0
    IL_048e:  ldloc.s    V_10
    IL_0490:  bge        IL_200e

    IL_0495:  ldloc.s    V_1
    IL_0497:  ldloc.s    V_0
    IL_0499:  dup
    IL_049a:  ldc.i4.s   1
    IL_049c:  add
    IL_049d:  stloc.s    V_0
    IL_049f:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_04a4:  ldc.i4.s   64
    IL_04a6:  bne.un     IL_200e

    IL_04ab:  ldarg.0
    IL_04ac:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_04b1:  ldloc.s    V_10
    IL_04b3:  ldloc.s    V_0
    IL_04b5:  sub
    IL_04b6:  dup
    IL_04b7:  stloc.s    V_7
    IL_04b9:  ldc.i4.s   1
    IL_04bb:  add
    IL_04bc:  stloc.s    V_6
    IL_04be:  ldloc.s    V_6
    IL_04c0:  ldc.i4.s   1
    IL_04c2:  sub
    IL_04c3:  dup
    IL_04c4:  stloc.s    V_6
    IL_04c6:  ldc.i4.s   0
    IL_04c8:  ble        IL_04f2

    IL_04cd:  ldloc.s    V_1
    IL_04cf:  ldloc.s    V_0
    IL_04d1:  dup
    IL_04d2:  ldc.i4.s   1
    IL_04d4:  add
    IL_04d5:  stloc.s    V_0
    IL_04d7:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_04dc:  ldstr      bytearray (00 00 04 00 00 00 23 00 24 00 2D 00 2E 00 )       // ......#.$.-...
    IL_04e1:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_04e6:  brtrue     IL_04be

    IL_04eb:  ldloc.s    V_0
    IL_04ed:  ldc.i4.s   1
    IL_04ef:  sub
    IL_04f0:  stloc.s    V_0
    IL_04f2:  ldloc.s    V_7
    IL_04f4:  ldloc.s    V_6
    IL_04f6:  ble.s      IL_0525

    IL_04f8:  ldloc.s    V_3
    IL_04fa:  ldloc.s    V_2
    IL_04fc:  ldc.i4.1
    IL_04fd:  sub
    IL_04fe:  dup
    IL_04ff:  stloc.s    V_2
    IL_0501:  ldloc.s    V_7
    IL_0503:  ldloc.s    V_6
    IL_0505:  sub
    IL_0506:  ldc.i4.s   1
    IL_0508:  sub
    IL_0509:  stelem.i4
    IL_050a:  ldloc.s    V_3
    IL_050c:  ldloc.s    V_2
    IL_050e:  ldc.i4.1
    IL_050f:  sub
    IL_0510:  dup
    IL_0511:  stloc.s    V_2
    IL_0513:  ldloc.s    V_0
    IL_0515:  ldc.i4.s   1
    IL_0517:  sub
    IL_0518:  stelem.i4
    IL_0519:  ldloc.s    V_3
    IL_051b:  ldloc.s    V_2
    IL_051d:  ldc.i4.1
    IL_051e:  sub
    IL_051f:  dup
    IL_0520:  stloc.s    V_2
    IL_0522:  ldc.i4.s   13
    IL_0524:  stelem.i4
    IL_0525:  ldarg.0
    IL_0526:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_052b:  ldloc.s    V_5
    IL_052d:  ldloc.s    V_4
    IL_052f:  ldc.i4.1
    IL_0530:  sub
    IL_0531:  dup
    IL_0532:  stloc.s    V_4
    IL_0534:  ldloc.s    V_0
    IL_0536:  stelem.i4
    IL_0537:  ldloc.s    V_3
    IL_0539:  ldloc.s    V_2
    IL_053b:  ldc.i4.1
    IL_053c:  sub
    IL_053d:  dup
    IL_053e:  stloc.s    V_2
    IL_0540:  ldc.i4.s   1
    IL_0542:  stelem.i4
    IL_0543:  ldarg.0
    IL_0544:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0549:  ldc.i4.s   1
    IL_054b:  ldloc.s    V_10
    IL_054d:  ldloc.s    V_0
    IL_054f:  sub
    IL_0550:  bgt        IL_200e

    IL_0555:  ldloc.s    V_0
    IL_0557:  ldc.i4.s   1
    IL_0559:  add
    IL_055a:  stloc.s    V_0
    IL_055c:  ldc.i4.s   1
    IL_055e:  stloc.s    V_6
    IL_0560:  ldloc.s    V_1
    IL_0562:  ldloc.s    V_0
    IL_0564:  ldloc.s    V_6
    IL_0566:  dup
    IL_0567:  ldc.i4.s   1
    IL_0569:  sub
    IL_056a:  stloc.s    V_6
    IL_056c:  sub
    IL_056d:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0572:  ldstr      bytearray (00 00 00 00 0A 00 00 00 02 00 04 00 05 00 03 00 
                                    01 00 06 00 09 00 13 00 00 00 ) 
    IL_0577:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_057c:  brfalse    IL_200e

    IL_0581:  ldloc.s    V_6
    IL_0583:  ldc.i4.s   0
    IL_0585:  bgt        IL_0560

    IL_058a:  ldarg.0
    IL_058b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0590:  ldloc.s    V_10
    IL_0592:  ldloc.s    V_0
    IL_0594:  sub
    IL_0595:  dup
    IL_0596:  stloc.s    V_7
    IL_0598:  ldc.i4.s   1
    IL_059a:  add
    IL_059b:  stloc.s    V_6
    IL_059d:  ldloc.s    V_6
    IL_059f:  ldc.i4.s   1
    IL_05a1:  sub
    IL_05a2:  dup
    IL_05a3:  stloc.s    V_6
    IL_05a5:  ldc.i4.s   0
    IL_05a7:  ble        IL_05d1

    IL_05ac:  ldloc.s    V_1
    IL_05ae:  ldloc.s    V_0
    IL_05b0:  dup
    IL_05b1:  ldc.i4.s   1
    IL_05b3:  add
    IL_05b4:  stloc.s    V_0
    IL_05b6:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_05bb:  ldstr      bytearray (00 00 00 00 0A 00 00 00 02 00 04 00 05 00 03 00 
                                    01 00 06 00 09 00 13 00 00 00 ) 
    IL_05c0:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_05c5:  brtrue     IL_059d

    IL_05ca:  ldloc.s    V_0
    IL_05cc:  ldc.i4.s   1
    IL_05ce:  sub
    IL_05cf:  stloc.s    V_0
    IL_05d1:  ldloc.s    V_7
    IL_05d3:  ldloc.s    V_6
    IL_05d5:  ble.s      IL_0604

    IL_05d7:  ldloc.s    V_3
    IL_05d9:  ldloc.s    V_2
    IL_05db:  ldc.i4.1
    IL_05dc:  sub
    IL_05dd:  dup
    IL_05de:  stloc.s    V_2
    IL_05e0:  ldloc.s    V_7
    IL_05e2:  ldloc.s    V_6
    IL_05e4:  sub
    IL_05e5:  ldc.i4.s   1
    IL_05e7:  sub
    IL_05e8:  stelem.i4
    IL_05e9:  ldloc.s    V_3
    IL_05eb:  ldloc.s    V_2
    IL_05ed:  ldc.i4.1
    IL_05ee:  sub
    IL_05ef:  dup
    IL_05f0:  stloc.s    V_2
    IL_05f2:  ldloc.s    V_0
    IL_05f4:  ldc.i4.s   1
    IL_05f6:  sub
    IL_05f7:  stelem.i4
    IL_05f8:  ldloc.s    V_3
    IL_05fa:  ldloc.s    V_2
    IL_05fc:  ldc.i4.1
    IL_05fd:  sub
    IL_05fe:  dup
    IL_05ff:  stloc.s    V_2
    IL_0601:  ldc.i4.s   14
    IL_0603:  stelem.i4
    IL_0604:  ldarg.0
    IL_0605:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_060a:  ldloc.s    V_0
    IL_060c:  ldloc.s    V_10
    IL_060e:  bge        IL_200e

    IL_0613:  ldloc.s    V_1
    IL_0615:  ldloc.s    V_0
    IL_0617:  dup
    IL_0618:  ldc.i4.s   1
    IL_061a:  add
    IL_061b:  stloc.s    V_0
    IL_061d:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0622:  ldc.i4.s   46
    IL_0624:  bne.un     IL_200e

    IL_0629:  ldarg.0
    IL_062a:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_062f:  ldloc.s    V_5
    IL_0631:  ldloc.s    V_4
    IL_0633:  dup
    IL_0634:  ldc.i4.1
    IL_0635:  add
    IL_0636:  stloc.s    V_4
    IL_0638:  ldelem.i4
    IL_0639:  stloc.s    V_6
    IL_063b:  ldarg.0
    IL_063c:  ldc.i4.s   5
    IL_063e:  ldloc.s    V_6
    IL_0640:  ldloc.s    V_0
    IL_0642:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_0647:  ldloc.s    V_3
    IL_0649:  ldloc.s    V_2
    IL_064b:  ldc.i4.1
    IL_064c:  sub
    IL_064d:  dup
    IL_064e:  stloc.s    V_2
    IL_0650:  ldloc.s    V_6
    IL_0652:  stelem.i4
    IL_0653:  ldloc.s    V_3
    IL_0655:  ldloc.s    V_2
    IL_0657:  ldc.i4.1
    IL_0658:  sub
    IL_0659:  dup
    IL_065a:  stloc.s    V_2
    IL_065c:  ldc.i4.s   8
    IL_065e:  stelem.i4
    IL_065f:  ldarg.0
    IL_0660:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0665:  ldc.i4.s   1
    IL_0667:  ldloc.s    V_10
    IL_0669:  ldloc.s    V_0
    IL_066b:  sub
    IL_066c:  bgt        IL_200e

    IL_0671:  ldloc.s    V_0
    IL_0673:  ldc.i4.s   1
    IL_0675:  add
    IL_0676:  stloc.s    V_0
    IL_0678:  ldc.i4.s   1
    IL_067a:  stloc.s    V_6
    IL_067c:  ldloc.s    V_1
    IL_067e:  ldloc.s    V_0
    IL_0680:  ldloc.s    V_6
    IL_0682:  dup
    IL_0683:  ldc.i4.s   1
    IL_0685:  sub
    IL_0686:  stloc.s    V_6
    IL_0688:  sub
    IL_0689:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_068e:  ldstr      bytearray (00 00 04 00 0A 00 2B 00 2C 00 2E 00 2F 00 00 00   // ......+.,.../...
                                    02 00 04 00 05 00 03 00 01 00 06 00 09 00 13 00 
                                    00 00 ) 
    IL_0693:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0698:  brfalse    IL_200e

    IL_069d:  ldloc.s    V_6
    IL_069f:  ldc.i4.s   0
    IL_06a1:  bgt        IL_067c

    IL_06a6:  ldarg.0
    IL_06a7:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_06ac:  ldloc.s    V_10
    IL_06ae:  ldloc.s    V_0
    IL_06b0:  sub
    IL_06b1:  dup
    IL_06b2:  stloc.s    V_7
    IL_06b4:  ldc.i4.s   1
    IL_06b6:  add
    IL_06b7:  stloc.s    V_6
    IL_06b9:  ldloc.s    V_6
    IL_06bb:  ldc.i4.s   1
    IL_06bd:  sub
    IL_06be:  dup
    IL_06bf:  stloc.s    V_6
    IL_06c1:  ldc.i4.s   0
    IL_06c3:  ble        IL_06ed

    IL_06c8:  ldloc.s    V_1
    IL_06ca:  ldloc.s    V_0
    IL_06cc:  dup
    IL_06cd:  ldc.i4.s   1
    IL_06cf:  add
    IL_06d0:  stloc.s    V_0
    IL_06d2:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_06d7:  ldstr      bytearray (00 00 04 00 0A 00 2B 00 2C 00 2E 00 2F 00 00 00   // ......+.,.../...
                                    02 00 04 00 05 00 03 00 01 00 06 00 09 00 13 00 
                                    00 00 ) 
    IL_06dc:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_06e1:  brtrue     IL_06b9

    IL_06e6:  ldloc.s    V_0
    IL_06e8:  ldc.i4.s   1
    IL_06ea:  sub
    IL_06eb:  stloc.s    V_0
    IL_06ed:  ldloc.s    V_7
    IL_06ef:  ldloc.s    V_6
    IL_06f1:  ble.s      IL_0720

    IL_06f3:  ldloc.s    V_3
    IL_06f5:  ldloc.s    V_2
    IL_06f7:  ldc.i4.1
    IL_06f8:  sub
    IL_06f9:  dup
    IL_06fa:  stloc.s    V_2
    IL_06fc:  ldloc.s    V_7
    IL_06fe:  ldloc.s    V_6
    IL_0700:  sub
    IL_0701:  ldc.i4.s   1
    IL_0703:  sub
    IL_0704:  stelem.i4
    IL_0705:  ldloc.s    V_3
    IL_0707:  ldloc.s    V_2
    IL_0709:  ldc.i4.1
    IL_070a:  sub
    IL_070b:  dup
    IL_070c:  stloc.s    V_2
    IL_070e:  ldloc.s    V_0
    IL_0710:  ldc.i4.s   1
    IL_0712:  sub
    IL_0713:  stelem.i4
    IL_0714:  ldloc.s    V_3
    IL_0716:  ldloc.s    V_2
    IL_0718:  ldc.i4.1
    IL_0719:  sub
    IL_071a:  dup
    IL_071b:  stloc.s    V_2
    IL_071d:  ldc.i4.s   15
    IL_071f:  stelem.i4
    IL_0720:  ldarg.0
    IL_0721:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0726:  ldloc.s    V_5
    IL_0728:  ldloc.s    V_4
    IL_072a:  dup
    IL_072b:  ldc.i4.1
    IL_072c:  add
    IL_072d:  stloc.s    V_4
    IL_072f:  ldelem.i4
    IL_0730:  stloc.s    V_6
    IL_0732:  ldarg.0
    IL_0733:  ldc.i4.s   2
    IL_0735:  ldloc.s    V_6
    IL_0737:  ldloc.s    V_0
    IL_0739:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_073e:  ldloc.s    V_3
    IL_0740:  ldloc.s    V_2
    IL_0742:  ldc.i4.1
    IL_0743:  sub
    IL_0744:  dup
    IL_0745:  stloc.s    V_2
    IL_0747:  ldloc.s    V_6
    IL_0749:  stelem.i4
    IL_074a:  ldloc.s    V_3
    IL_074c:  ldloc.s    V_2
    IL_074e:  ldc.i4.1
    IL_074f:  sub
    IL_0750:  dup
    IL_0751:  stloc.s    V_2
    IL_0753:  ldc.i4.s   8
    IL_0755:  stelem.i4
    IL_0756:  ldarg.0
    IL_0757:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_075c:  ldloc.s    V_0
    IL_075e:  ldloc.s    V_10
    IL_0760:  bge        IL_200e

    IL_0765:  ldloc.s    V_1
    IL_0767:  ldloc.s    V_0
    IL_0769:  dup
    IL_076a:  ldc.i4.s   1
    IL_076c:  add
    IL_076d:  stloc.s    V_0
    IL_076f:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0774:  ldc.i4.s   36
    IL_0776:  bne.un     IL_200e

    IL_077b:  ldarg.0
    IL_077c:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0781:  br         IL_1d04

    IL_0786:  ldarg.0
    IL_0787:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_078c:  ldloc.s    V_3
    IL_078e:  ldloc.s    V_2
    IL_0790:  ldc.i4.1
    IL_0791:  sub
    IL_0792:  dup
    IL_0793:  stloc.s    V_2
    IL_0795:  ldloc.s    V_0
    IL_0797:  stelem.i4
    IL_0798:  ldloc.s    V_3
    IL_079a:  ldloc.s    V_2
    IL_079c:  ldc.i4.1
    IL_079d:  sub
    IL_079e:  dup
    IL_079f:  stloc.s    V_2
    IL_07a1:  ldc.i4.s   16
    IL_07a3:  stelem.i4
    IL_07a4:  ldarg.0
    IL_07a5:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_07aa:  ldc.i4.s   1
    IL_07ac:  ldloc.s    V_10
    IL_07ae:  ldloc.s    V_0
    IL_07b0:  sub
    IL_07b1:  bgt        IL_200e

    IL_07b6:  ldloc.s    V_0
    IL_07b8:  ldc.i4.s   1
    IL_07ba:  add
    IL_07bb:  stloc.s    V_0
    IL_07bd:  ldc.i4.s   1
    IL_07bf:  stloc.s    V_6
    IL_07c1:  ldloc.s    V_1
    IL_07c3:  ldloc.s    V_0
    IL_07c5:  ldloc.s    V_6
    IL_07c7:  dup
    IL_07c8:  ldc.i4.s   1
    IL_07ca:  sub
    IL_07cb:  stloc.s    V_6
    IL_07cd:  sub
    IL_07ce:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_07d3:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_07d8:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_07dd:  brfalse    IL_200e

    IL_07e2:  ldloc.s    V_6
    IL_07e4:  ldc.i4.s   0
    IL_07e6:  bgt        IL_07c1

    IL_07eb:  ldarg.0
    IL_07ec:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_07f1:  ldloc.s    V_10
    IL_07f3:  ldloc.s    V_0
    IL_07f5:  sub
    IL_07f6:  dup
    IL_07f7:  stloc.s    V_7
    IL_07f9:  ldc.i4.s   1
    IL_07fb:  add
    IL_07fc:  stloc.s    V_6
    IL_07fe:  ldloc.s    V_6
    IL_0800:  ldc.i4.s   1
    IL_0802:  sub
    IL_0803:  dup
    IL_0804:  stloc.s    V_6
    IL_0806:  ldc.i4.s   0
    IL_0808:  ble        IL_0832

    IL_080d:  ldloc.s    V_1
    IL_080f:  ldloc.s    V_0
    IL_0811:  dup
    IL_0812:  ldc.i4.s   1
    IL_0814:  add
    IL_0815:  stloc.s    V_0
    IL_0817:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_081c:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_0821:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0826:  brtrue     IL_07fe

    IL_082b:  ldloc.s    V_0
    IL_082d:  ldc.i4.s   1
    IL_082f:  sub
    IL_0830:  stloc.s    V_0
    IL_0832:  ldloc.s    V_7
    IL_0834:  ldloc.s    V_6
    IL_0836:  ble.s      IL_0865

    IL_0838:  ldloc.s    V_3
    IL_083a:  ldloc.s    V_2
    IL_083c:  ldc.i4.1
    IL_083d:  sub
    IL_083e:  dup
    IL_083f:  stloc.s    V_2
    IL_0841:  ldloc.s    V_7
    IL_0843:  ldloc.s    V_6
    IL_0845:  sub
    IL_0846:  ldc.i4.s   1
    IL_0848:  sub
    IL_0849:  stelem.i4
    IL_084a:  ldloc.s    V_3
    IL_084c:  ldloc.s    V_2
    IL_084e:  ldc.i4.1
    IL_084f:  sub
    IL_0850:  dup
    IL_0851:  stloc.s    V_2
    IL_0853:  ldloc.s    V_0
    IL_0855:  ldc.i4.s   1
    IL_0857:  sub
    IL_0858:  stelem.i4
    IL_0859:  ldloc.s    V_3
    IL_085b:  ldloc.s    V_2
    IL_085d:  ldc.i4.1
    IL_085e:  sub
    IL_085f:  dup
    IL_0860:  stloc.s    V_2
    IL_0862:  ldc.i4.s   17
    IL_0864:  stelem.i4
    IL_0865:  ldarg.0
    IL_0866:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_086b:  ldc.i4.s   2
    IL_086d:  ldloc.s    V_10
    IL_086f:  ldloc.s    V_0
    IL_0871:  sub
    IL_0872:  bgt        IL_200e

    IL_0877:  ldloc.s    V_1
    IL_0879:  ldloc.s    V_0
    IL_087b:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0880:  ldc.i4.s   58
    IL_0882:  bne.un     IL_200e

    IL_0887:  ldloc.s    V_1
    IL_0889:  ldloc.s    V_0
    IL_088b:  ldc.i4.s   1
    IL_088d:  add
    IL_088e:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0893:  ldc.i4.s   123
    IL_0895:  bne.un     IL_200e

    IL_089a:  ldloc.s    V_0
    IL_089c:  ldc.i4.s   2
    IL_089e:  add
    IL_089f:  stloc.s    V_0
    IL_08a1:  ldarg.0
    IL_08a2:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_08a7:  br         IL_1d04

    IL_08ac:  ldarg.0
    IL_08ad:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_08b2:  ldloc.s    V_0
    IL_08b4:  ldloc.s    V_10
    IL_08b6:  bge        IL_200e

    IL_08bb:  ldloc.s    V_1
    IL_08bd:  ldloc.s    V_0
    IL_08bf:  dup
    IL_08c0:  ldc.i4.s   1
    IL_08c2:  add
    IL_08c3:  stloc.s    V_0
    IL_08c5:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_08ca:  ldstr      bytearray (00 00 00 00 0A 00 00 00 02 00 04 00 05 00 03 00 
                                    01 00 06 00 09 00 13 00 00 00 ) 
    IL_08cf:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_08d4:  brfalse    IL_200e

    IL_08d9:  ldarg.0
    IL_08da:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_08df:  ldloc.s    V_5
    IL_08e1:  ldloc.s    V_4
    IL_08e3:  ldc.i4.1
    IL_08e4:  sub
    IL_08e5:  dup
    IL_08e6:  stloc.s    V_4
    IL_08e8:  ldc.i4.s   -1
    IL_08ea:  stelem.i4
    IL_08eb:  ldloc.s    V_5
    IL_08ed:  ldloc.s    V_4
    IL_08ef:  ldc.i4.1
    IL_08f0:  sub
    IL_08f1:  dup
    IL_08f2:  stloc.s    V_4
    IL_08f4:  ldc.i4.s   0
    IL_08f6:  stelem.i4
    IL_08f7:  ldloc.s    V_3
    IL_08f9:  ldloc.s    V_2
    IL_08fb:  ldc.i4.1
    IL_08fc:  sub
    IL_08fd:  dup
    IL_08fe:  stloc.s    V_2
    IL_0900:  ldc.i4.s   5
    IL_0902:  stelem.i4
    IL_0903:  ldarg.0
    IL_0904:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0909:  br         IL_0b96

    IL_090e:  ldarg.0
    IL_090f:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0914:  ldloc.s    V_5
    IL_0916:  ldloc.s    V_4
    IL_0918:  ldc.i4.1
    IL_0919:  sub
    IL_091a:  dup
    IL_091b:  stloc.s    V_4
    IL_091d:  ldloc.s    V_0
    IL_091f:  stelem.i4
    IL_0920:  ldloc.s    V_3
    IL_0922:  ldloc.s    V_2
    IL_0924:  ldc.i4.1
    IL_0925:  sub
    IL_0926:  dup
    IL_0927:  stloc.s    V_2
    IL_0929:  ldc.i4.s   1
    IL_092b:  stelem.i4
    IL_092c:  ldarg.0
    IL_092d:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0932:  ldloc.s    V_0
    IL_0934:  ldloc.s    V_10
    IL_0936:  bge        IL_200e

    IL_093b:  ldloc.s    V_1
    IL_093d:  ldloc.s    V_0
    IL_093f:  dup
    IL_0940:  ldc.i4.s   1
    IL_0942:  add
    IL_0943:  stloc.s    V_0
    IL_0945:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_094a:  ldc.i4.s   35
    IL_094c:  bne.un     IL_200e

    IL_0951:  ldarg.0
    IL_0952:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0957:  ldc.i4.s   1
    IL_0959:  ldloc.s    V_10
    IL_095b:  ldloc.s    V_0
    IL_095d:  sub
    IL_095e:  bgt        IL_200e

    IL_0963:  ldloc.s    V_0
    IL_0965:  ldc.i4.s   1
    IL_0967:  add
    IL_0968:  stloc.s    V_0
    IL_096a:  ldc.i4.s   1
    IL_096c:  stloc.s    V_6
    IL_096e:  ldloc.s    V_1
    IL_0970:  ldloc.s    V_0
    IL_0972:  ldloc.s    V_6
    IL_0974:  dup
    IL_0975:  ldc.i4.s   1
    IL_0977:  sub
    IL_0978:  stloc.s    V_6
    IL_097a:  sub
    IL_097b:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0980:  ldstr      bytearray (00 00 00 00 01 00 09 00 ) 
    IL_0985:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_098a:  brfalse    IL_200e

    IL_098f:  ldloc.s    V_6
    IL_0991:  ldc.i4.s   0
    IL_0993:  bgt        IL_096e

    IL_0998:  ldarg.0
    IL_0999:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_099e:  ldloc.s    V_10
    IL_09a0:  ldloc.s    V_0
    IL_09a2:  sub
    IL_09a3:  dup
    IL_09a4:  stloc.s    V_7
    IL_09a6:  ldc.i4.s   1
    IL_09a8:  add
    IL_09a9:  stloc.s    V_6
    IL_09ab:  ldloc.s    V_6
    IL_09ad:  ldc.i4.s   1
    IL_09af:  sub
    IL_09b0:  dup
    IL_09b1:  stloc.s    V_6
    IL_09b3:  ldc.i4.s   0
    IL_09b5:  ble        IL_09df

    IL_09ba:  ldloc.s    V_1
    IL_09bc:  ldloc.s    V_0
    IL_09be:  dup
    IL_09bf:  ldc.i4.s   1
    IL_09c1:  add
    IL_09c2:  stloc.s    V_0
    IL_09c4:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_09c9:  ldstr      bytearray (00 00 00 00 01 00 09 00 ) 
    IL_09ce:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_09d3:  brtrue     IL_09ab

    IL_09d8:  ldloc.s    V_0
    IL_09da:  ldc.i4.s   1
    IL_09dc:  sub
    IL_09dd:  stloc.s    V_0
    IL_09df:  ldloc.s    V_7
    IL_09e1:  ldloc.s    V_6
    IL_09e3:  ble.s      IL_0a12

    IL_09e5:  ldloc.s    V_3
    IL_09e7:  ldloc.s    V_2
    IL_09e9:  ldc.i4.1
    IL_09ea:  sub
    IL_09eb:  dup
    IL_09ec:  stloc.s    V_2
    IL_09ee:  ldloc.s    V_7
    IL_09f0:  ldloc.s    V_6
    IL_09f2:  sub
    IL_09f3:  ldc.i4.s   1
    IL_09f5:  sub
    IL_09f6:  stelem.i4
    IL_09f7:  ldloc.s    V_3
    IL_09f9:  ldloc.s    V_2
    IL_09fb:  ldc.i4.1
    IL_09fc:  sub
    IL_09fd:  dup
    IL_09fe:  stloc.s    V_2
    IL_0a00:  ldloc.s    V_0
    IL_0a02:  ldc.i4.s   1
    IL_0a04:  sub
    IL_0a05:  stelem.i4
    IL_0a06:  ldloc.s    V_3
    IL_0a08:  ldloc.s    V_2
    IL_0a0a:  ldc.i4.1
    IL_0a0b:  sub
    IL_0a0c:  dup
    IL_0a0d:  stloc.s    V_2
    IL_0a0f:  ldc.i4.s   18
    IL_0a11:  stelem.i4
    IL_0a12:  ldarg.0
    IL_0a13:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0a18:  ldloc.s    V_5
    IL_0a1a:  ldloc.s    V_4
    IL_0a1c:  ldc.i4.1
    IL_0a1d:  sub
    IL_0a1e:  dup
    IL_0a1f:  stloc.s    V_4
    IL_0a21:  ldc.i4.s   -1
    IL_0a23:  stelem.i4
    IL_0a24:  ldloc.s    V_5
    IL_0a26:  ldloc.s    V_4
    IL_0a28:  ldc.i4.1
    IL_0a29:  sub
    IL_0a2a:  dup
    IL_0a2b:  stloc.s    V_4
    IL_0a2d:  ldc.i4.s   0
    IL_0a2f:  stelem.i4
    IL_0a30:  ldloc.s    V_3
    IL_0a32:  ldloc.s    V_2
    IL_0a34:  ldc.i4.1
    IL_0a35:  sub
    IL_0a36:  dup
    IL_0a37:  stloc.s    V_2
    IL_0a39:  ldc.i4.s   5
    IL_0a3b:  stelem.i4
    IL_0a3c:  ldarg.0
    IL_0a3d:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0a42:  br         IL_0ac0

    IL_0a47:  ldarg.0
    IL_0a48:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0a4d:  ldloc.s    V_5
    IL_0a4f:  ldloc.s    V_4
    IL_0a51:  ldc.i4.1
    IL_0a52:  sub
    IL_0a53:  dup
    IL_0a54:  stloc.s    V_4
    IL_0a56:  ldloc.s    V_0
    IL_0a58:  stelem.i4
    IL_0a59:  ldloc.s    V_3
    IL_0a5b:  ldloc.s    V_2
    IL_0a5d:  ldc.i4.1
    IL_0a5e:  sub
    IL_0a5f:  dup
    IL_0a60:  stloc.s    V_2
    IL_0a62:  ldc.i4.s   1
    IL_0a64:  stelem.i4
    IL_0a65:  ldarg.0
    IL_0a66:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0a6b:  ldloc.s    V_0
    IL_0a6d:  ldloc.s    V_10
    IL_0a6f:  bge        IL_200e

    IL_0a74:  ldloc.s    V_1
    IL_0a76:  ldloc.s    V_0
    IL_0a78:  dup
    IL_0a79:  ldc.i4.s   1
    IL_0a7b:  add
    IL_0a7c:  stloc.s    V_0
    IL_0a7e:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0a83:  ldc.i4.s   43
    IL_0a85:  bne.un     IL_200e

    IL_0a8a:  ldarg.0
    IL_0a8b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0a90:  ldloc.s    V_5
    IL_0a92:  ldloc.s    V_4
    IL_0a94:  dup
    IL_0a95:  ldc.i4.1
    IL_0a96:  add
    IL_0a97:  stloc.s    V_4
    IL_0a99:  ldelem.i4
    IL_0a9a:  stloc.s    V_6
    IL_0a9c:  ldarg.0
    IL_0a9d:  ldc.i4.s   7
    IL_0a9f:  ldloc.s    V_6
    IL_0aa1:  ldloc.s    V_0
    IL_0aa3:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_0aa8:  ldloc.s    V_3
    IL_0aaa:  ldloc.s    V_2
    IL_0aac:  ldc.i4.1
    IL_0aad:  sub
    IL_0aae:  dup
    IL_0aaf:  stloc.s    V_2
    IL_0ab1:  ldloc.s    V_6
    IL_0ab3:  stelem.i4
    IL_0ab4:  ldloc.s    V_3
    IL_0ab6:  ldloc.s    V_2
    IL_0ab8:  ldc.i4.1
    IL_0ab9:  sub
    IL_0aba:  dup
    IL_0abb:  stloc.s    V_2
    IL_0abd:  ldc.i4.s   8
    IL_0abf:  stelem.i4
    IL_0ac0:  ldarg.0
    IL_0ac1:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0ac6:  ldloc.s    V_5
    IL_0ac8:  ldloc.s    V_4
    IL_0aca:  dup
    IL_0acb:  ldc.i4.1
    IL_0acc:  add
    IL_0acd:  stloc.s    V_4
    IL_0acf:  ldelem.i4
    IL_0ad0:  stloc.s    V_6
    IL_0ad2:  ldloc.s    V_5
    IL_0ad4:  ldloc.s    V_4
    IL_0ad6:  dup
    IL_0ad7:  ldc.i4.1
    IL_0ad8:  add
    IL_0ad9:  stloc.s    V_4
    IL_0adb:  ldelem.i4
    IL_0adc:  dup
    IL_0add:  stloc.s    V_7
    IL_0adf:  ldloc.s    V_3
    IL_0ae1:  ldloc.s    V_2
    IL_0ae3:  ldc.i4.1
    IL_0ae4:  sub
    IL_0ae5:  dup
    IL_0ae6:  stloc.s    V_2
    IL_0ae8:  ldloc.s    V_7
    IL_0aea:  stelem.i4
    IL_0aeb:  ldloc.s    V_0
    IL_0aed:  bne.un.s   IL_0af5

    IL_0aef:  ldloc.s    V_6
    IL_0af1:  ldc.i4.s   0
    IL_0af3:  bge.s      IL_0b48

    IL_0af5:  ldloc.s    V_6
    IL_0af7:  ldc.i4.s   1
    IL_0af9:  bge.s      IL_0b48

    IL_0afb:  ldloc.s    V_5
    IL_0afd:  ldloc.s    V_4
    IL_0aff:  ldc.i4.1
    IL_0b00:  sub
    IL_0b01:  dup
    IL_0b02:  stloc.s    V_4
    IL_0b04:  ldloc.s    V_0
    IL_0b06:  stelem.i4
    IL_0b07:  ldloc.s    V_5
    IL_0b09:  ldloc.s    V_4
    IL_0b0b:  ldc.i4.1
    IL_0b0c:  sub
    IL_0b0d:  dup
    IL_0b0e:  stloc.s    V_4
    IL_0b10:  ldloc.s    V_6
    IL_0b12:  ldc.i4.s   1
    IL_0b14:  add
    IL_0b15:  stelem.i4
    IL_0b16:  ldloc.s    V_3
    IL_0b18:  ldloc.s    V_2
    IL_0b1a:  ldc.i4.1
    IL_0b1b:  sub
    IL_0b1c:  dup
    IL_0b1d:  stloc.s    V_2
    IL_0b1f:  ldc.i4.s   19
    IL_0b21:  stelem.i4
    IL_0b22:  ldloc.s    V_2
    IL_0b24:  ldc.i4     0x188
    IL_0b29:  ble.s      IL_0b37

    IL_0b2b:  ldloc.s    V_4
    IL_0b2d:  ldc.i4     0x126
    IL_0b32:  bgt        IL_0a47

    IL_0b37:  ldloc.s    V_3
    IL_0b39:  ldloc.s    V_2
    IL_0b3b:  ldc.i4.1
    IL_0b3c:  sub
    IL_0b3d:  dup
    IL_0b3e:  stloc.s    V_2
    IL_0b40:  ldc.i4.s   20
    IL_0b42:  stelem.i4
    IL_0b43:  br         IL_200e

    IL_0b48:  ldloc.s    V_3
    IL_0b4a:  ldloc.s    V_2
    IL_0b4c:  ldc.i4.1
    IL_0b4d:  sub
    IL_0b4e:  dup
    IL_0b4f:  stloc.s    V_2
    IL_0b51:  ldloc.s    V_6
    IL_0b53:  stelem.i4
    IL_0b54:  ldloc.s    V_3
    IL_0b56:  ldloc.s    V_2
    IL_0b58:  ldc.i4.1
    IL_0b59:  sub
    IL_0b5a:  dup
    IL_0b5b:  stloc.s    V_2
    IL_0b5d:  ldc.i4.s   11
    IL_0b5f:  stelem.i4
    IL_0b60:  ldarg.0
    IL_0b61:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0b66:  ldloc.s    V_5
    IL_0b68:  ldloc.s    V_4
    IL_0b6a:  dup
    IL_0b6b:  ldc.i4.1
    IL_0b6c:  add
    IL_0b6d:  stloc.s    V_4
    IL_0b6f:  ldelem.i4
    IL_0b70:  stloc.s    V_6
    IL_0b72:  ldarg.0
    IL_0b73:  ldc.i4.s   6
    IL_0b75:  ldloc.s    V_6
    IL_0b77:  ldloc.s    V_0
    IL_0b79:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_0b7e:  ldloc.s    V_3
    IL_0b80:  ldloc.s    V_2
    IL_0b82:  ldc.i4.1
    IL_0b83:  sub
    IL_0b84:  dup
    IL_0b85:  stloc.s    V_2
    IL_0b87:  ldloc.s    V_6
    IL_0b89:  stelem.i4
    IL_0b8a:  ldloc.s    V_3
    IL_0b8c:  ldloc.s    V_2
    IL_0b8e:  ldc.i4.1
    IL_0b8f:  sub
    IL_0b90:  dup
    IL_0b91:  stloc.s    V_2
    IL_0b93:  ldc.i4.s   8
    IL_0b95:  stelem.i4
    IL_0b96:  ldarg.0
    IL_0b97:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0b9c:  ldloc.s    V_5
    IL_0b9e:  ldloc.s    V_4
    IL_0ba0:  dup
    IL_0ba1:  ldc.i4.1
    IL_0ba2:  add
    IL_0ba3:  stloc.s    V_4
    IL_0ba5:  ldelem.i4
    IL_0ba6:  stloc.s    V_6
    IL_0ba8:  ldloc.s    V_5
    IL_0baa:  ldloc.s    V_4
    IL_0bac:  dup
    IL_0bad:  ldc.i4.1
    IL_0bae:  add
    IL_0baf:  stloc.s    V_4
    IL_0bb1:  ldelem.i4
    IL_0bb2:  dup
    IL_0bb3:  stloc.s    V_7
    IL_0bb5:  ldloc.s    V_3
    IL_0bb7:  ldloc.s    V_2
    IL_0bb9:  ldc.i4.1
    IL_0bba:  sub
    IL_0bbb:  dup
    IL_0bbc:  stloc.s    V_2
    IL_0bbe:  ldloc.s    V_7
    IL_0bc0:  stelem.i4
    IL_0bc1:  ldloc.s    V_0
    IL_0bc3:  bne.un.s   IL_0bcb

    IL_0bc5:  ldloc.s    V_6
    IL_0bc7:  ldc.i4.s   0
    IL_0bc9:  bge.s      IL_0c1e

    IL_0bcb:  ldloc.s    V_6
    IL_0bcd:  ldc.i4.s   1
    IL_0bcf:  bge.s      IL_0c1e

    IL_0bd1:  ldloc.s    V_5
    IL_0bd3:  ldloc.s    V_4
    IL_0bd5:  ldc.i4.1
    IL_0bd6:  sub
    IL_0bd7:  dup
    IL_0bd8:  stloc.s    V_4
    IL_0bda:  ldloc.s    V_0
    IL_0bdc:  stelem.i4
    IL_0bdd:  ldloc.s    V_5
    IL_0bdf:  ldloc.s    V_4
    IL_0be1:  ldc.i4.1
    IL_0be2:  sub
    IL_0be3:  dup
    IL_0be4:  stloc.s    V_4
    IL_0be6:  ldloc.s    V_6
    IL_0be8:  ldc.i4.s   1
    IL_0bea:  add
    IL_0beb:  stelem.i4
    IL_0bec:  ldloc.s    V_3
    IL_0bee:  ldloc.s    V_2
    IL_0bf0:  ldc.i4.1
    IL_0bf1:  sub
    IL_0bf2:  dup
    IL_0bf3:  stloc.s    V_2
    IL_0bf5:  ldc.i4.s   21
    IL_0bf7:  stelem.i4
    IL_0bf8:  ldloc.s    V_2
    IL_0bfa:  ldc.i4     0x188
    IL_0bff:  ble.s      IL_0c0d

    IL_0c01:  ldloc.s    V_4
    IL_0c03:  ldc.i4     0x126
    IL_0c08:  bgt        IL_090e

    IL_0c0d:  ldloc.s    V_3
    IL_0c0f:  ldloc.s    V_2
    IL_0c11:  ldc.i4.1
    IL_0c12:  sub
    IL_0c13:  dup
    IL_0c14:  stloc.s    V_2
    IL_0c16:  ldc.i4.s   22
    IL_0c18:  stelem.i4
    IL_0c19:  br         IL_200e

    IL_0c1e:  ldloc.s    V_3
    IL_0c20:  ldloc.s    V_2
    IL_0c22:  ldc.i4.1
    IL_0c23:  sub
    IL_0c24:  dup
    IL_0c25:  stloc.s    V_2
    IL_0c27:  ldloc.s    V_6
    IL_0c29:  stelem.i4
    IL_0c2a:  ldloc.s    V_3
    IL_0c2c:  ldloc.s    V_2
    IL_0c2e:  ldc.i4.1
    IL_0c2f:  sub
    IL_0c30:  dup
    IL_0c31:  stloc.s    V_2
    IL_0c33:  ldc.i4.s   11
    IL_0c35:  stelem.i4
    IL_0c36:  ldarg.0
    IL_0c37:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0c3c:  ldloc.s    V_5
    IL_0c3e:  ldloc.s    V_4
    IL_0c40:  ldc.i4.1
    IL_0c41:  sub
    IL_0c42:  dup
    IL_0c43:  stloc.s    V_4
    IL_0c45:  ldc.i4.s   -1
    IL_0c47:  stelem.i4
    IL_0c48:  ldloc.s    V_5
    IL_0c4a:  ldloc.s    V_4
    IL_0c4c:  ldc.i4.1
    IL_0c4d:  sub
    IL_0c4e:  dup
    IL_0c4f:  stloc.s    V_4
    IL_0c51:  ldc.i4.s   0
    IL_0c53:  stelem.i4
    IL_0c54:  ldloc.s    V_3
    IL_0c56:  ldloc.s    V_2
    IL_0c58:  ldc.i4.1
    IL_0c59:  sub
    IL_0c5a:  dup
    IL_0c5b:  stloc.s    V_2
    IL_0c5d:  ldc.i4.s   5
    IL_0c5f:  stelem.i4
    IL_0c60:  ldarg.0
    IL_0c61:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0c66:  br         IL_0dca

    IL_0c6b:  ldarg.0
    IL_0c6c:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0c71:  ldloc.s    V_5
    IL_0c73:  ldloc.s    V_4
    IL_0c75:  ldc.i4.1
    IL_0c76:  sub
    IL_0c77:  dup
    IL_0c78:  stloc.s    V_4
    IL_0c7a:  ldloc.s    V_0
    IL_0c7c:  stelem.i4
    IL_0c7d:  ldloc.s    V_3
    IL_0c7f:  ldloc.s    V_2
    IL_0c81:  ldc.i4.1
    IL_0c82:  sub
    IL_0c83:  dup
    IL_0c84:  stloc.s    V_2
    IL_0c86:  ldc.i4.s   1
    IL_0c88:  stelem.i4
    IL_0c89:  ldarg.0
    IL_0c8a:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0c8f:  ldloc.s    V_0
    IL_0c91:  ldloc.s    V_10
    IL_0c93:  bge        IL_200e

    IL_0c98:  ldloc.s    V_1
    IL_0c9a:  ldloc.s    V_0
    IL_0c9c:  dup
    IL_0c9d:  ldc.i4.s   1
    IL_0c9f:  add
    IL_0ca0:  stloc.s    V_0
    IL_0ca2:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0ca7:  ldc.i4.s   91
    IL_0ca9:  bne.un     IL_200e

    IL_0cae:  ldarg.0
    IL_0caf:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0cb4:  ldc.i4.s   1
    IL_0cb6:  ldloc.s    V_10
    IL_0cb8:  ldloc.s    V_0
    IL_0cba:  sub
    IL_0cbb:  bgt        IL_200e

    IL_0cc0:  ldloc.s    V_0
    IL_0cc2:  ldc.i4.s   1
    IL_0cc4:  add
    IL_0cc5:  stloc.s    V_0
    IL_0cc7:  ldc.i4.s   1
    IL_0cc9:  stloc.s    V_6
    IL_0ccb:  ldloc.s    V_1
    IL_0ccd:  ldloc.s    V_0
    IL_0ccf:  ldloc.s    V_6
    IL_0cd1:  dup
    IL_0cd2:  ldc.i4.s   1
    IL_0cd4:  sub
    IL_0cd5:  stloc.s    V_6
    IL_0cd7:  sub
    IL_0cd8:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0cdd:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_0ce2:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0ce7:  brfalse    IL_200e

    IL_0cec:  ldloc.s    V_6
    IL_0cee:  ldc.i4.s   0
    IL_0cf0:  bgt        IL_0ccb

    IL_0cf5:  ldarg.0
    IL_0cf6:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0cfb:  ldloc.s    V_10
    IL_0cfd:  ldloc.s    V_0
    IL_0cff:  sub
    IL_0d00:  dup
    IL_0d01:  stloc.s    V_7
    IL_0d03:  ldc.i4.s   1
    IL_0d05:  add
    IL_0d06:  stloc.s    V_6
    IL_0d08:  ldloc.s    V_6
    IL_0d0a:  ldc.i4.s   1
    IL_0d0c:  sub
    IL_0d0d:  dup
    IL_0d0e:  stloc.s    V_6
    IL_0d10:  ldc.i4.s   0
    IL_0d12:  ble        IL_0d3c

    IL_0d17:  ldloc.s    V_1
    IL_0d19:  ldloc.s    V_0
    IL_0d1b:  dup
    IL_0d1c:  ldc.i4.s   1
    IL_0d1e:  add
    IL_0d1f:  stloc.s    V_0
    IL_0d21:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0d26:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_0d2b:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0d30:  brtrue     IL_0d08

    IL_0d35:  ldloc.s    V_0
    IL_0d37:  ldc.i4.s   1
    IL_0d39:  sub
    IL_0d3a:  stloc.s    V_0
    IL_0d3c:  ldloc.s    V_7
    IL_0d3e:  ldloc.s    V_6
    IL_0d40:  ble.s      IL_0d6f

    IL_0d42:  ldloc.s    V_3
    IL_0d44:  ldloc.s    V_2
    IL_0d46:  ldc.i4.1
    IL_0d47:  sub
    IL_0d48:  dup
    IL_0d49:  stloc.s    V_2
    IL_0d4b:  ldloc.s    V_7
    IL_0d4d:  ldloc.s    V_6
    IL_0d4f:  sub
    IL_0d50:  ldc.i4.s   1
    IL_0d52:  sub
    IL_0d53:  stelem.i4
    IL_0d54:  ldloc.s    V_3
    IL_0d56:  ldloc.s    V_2
    IL_0d58:  ldc.i4.1
    IL_0d59:  sub
    IL_0d5a:  dup
    IL_0d5b:  stloc.s    V_2
    IL_0d5d:  ldloc.s    V_0
    IL_0d5f:  ldc.i4.s   1
    IL_0d61:  sub
    IL_0d62:  stelem.i4
    IL_0d63:  ldloc.s    V_3
    IL_0d65:  ldloc.s    V_2
    IL_0d67:  ldc.i4.1
    IL_0d68:  sub
    IL_0d69:  dup
    IL_0d6a:  stloc.s    V_2
    IL_0d6c:  ldc.i4.s   23
    IL_0d6e:  stelem.i4
    IL_0d6f:  ldarg.0
    IL_0d70:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0d75:  ldloc.s    V_0
    IL_0d77:  ldloc.s    V_10
    IL_0d79:  bge        IL_200e

    IL_0d7e:  ldloc.s    V_1
    IL_0d80:  ldloc.s    V_0
    IL_0d82:  dup
    IL_0d83:  ldc.i4.s   1
    IL_0d85:  add
    IL_0d86:  stloc.s    V_0
    IL_0d88:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0d8d:  ldc.i4.s   93
    IL_0d8f:  bne.un     IL_200e

    IL_0d94:  ldarg.0
    IL_0d95:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0d9a:  ldloc.s    V_5
    IL_0d9c:  ldloc.s    V_4
    IL_0d9e:  dup
    IL_0d9f:  ldc.i4.1
    IL_0da0:  add
    IL_0da1:  stloc.s    V_4
    IL_0da3:  ldelem.i4
    IL_0da4:  stloc.s    V_6
    IL_0da6:  ldarg.0
    IL_0da7:  ldc.i4.s   8
    IL_0da9:  ldloc.s    V_6
    IL_0dab:  ldloc.s    V_0
    IL_0dad:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_0db2:  ldloc.s    V_3
    IL_0db4:  ldloc.s    V_2
    IL_0db6:  ldc.i4.1
    IL_0db7:  sub
    IL_0db8:  dup
    IL_0db9:  stloc.s    V_2
    IL_0dbb:  ldloc.s    V_6
    IL_0dbd:  stelem.i4
    IL_0dbe:  ldloc.s    V_3
    IL_0dc0:  ldloc.s    V_2
    IL_0dc2:  ldc.i4.1
    IL_0dc3:  sub
    IL_0dc4:  dup
    IL_0dc5:  stloc.s    V_2
    IL_0dc7:  ldc.i4.s   8
    IL_0dc9:  stelem.i4
    IL_0dca:  ldarg.0
    IL_0dcb:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0dd0:  ldloc.s    V_5
    IL_0dd2:  ldloc.s    V_4
    IL_0dd4:  dup
    IL_0dd5:  ldc.i4.1
    IL_0dd6:  add
    IL_0dd7:  stloc.s    V_4
    IL_0dd9:  ldelem.i4
    IL_0dda:  stloc.s    V_6
    IL_0ddc:  ldloc.s    V_5
    IL_0dde:  ldloc.s    V_4
    IL_0de0:  dup
    IL_0de1:  ldc.i4.1
    IL_0de2:  add
    IL_0de3:  stloc.s    V_4
    IL_0de5:  ldelem.i4
    IL_0de6:  dup
    IL_0de7:  stloc.s    V_7
    IL_0de9:  ldloc.s    V_3
    IL_0deb:  ldloc.s    V_2
    IL_0ded:  ldc.i4.1
    IL_0dee:  sub
    IL_0def:  dup
    IL_0df0:  stloc.s    V_2
    IL_0df2:  ldloc.s    V_7
    IL_0df4:  stelem.i4
    IL_0df5:  ldloc.s    V_0
    IL_0df7:  bne.un.s   IL_0dff

    IL_0df9:  ldloc.s    V_6
    IL_0dfb:  ldc.i4.s   0
    IL_0dfd:  bge.s      IL_0e52

    IL_0dff:  ldloc.s    V_6
    IL_0e01:  ldc.i4.s   1
    IL_0e03:  bge.s      IL_0e52

    IL_0e05:  ldloc.s    V_5
    IL_0e07:  ldloc.s    V_4
    IL_0e09:  ldc.i4.1
    IL_0e0a:  sub
    IL_0e0b:  dup
    IL_0e0c:  stloc.s    V_4
    IL_0e0e:  ldloc.s    V_0
    IL_0e10:  stelem.i4
    IL_0e11:  ldloc.s    V_5
    IL_0e13:  ldloc.s    V_4
    IL_0e15:  ldc.i4.1
    IL_0e16:  sub
    IL_0e17:  dup
    IL_0e18:  stloc.s    V_4
    IL_0e1a:  ldloc.s    V_6
    IL_0e1c:  ldc.i4.s   1
    IL_0e1e:  add
    IL_0e1f:  stelem.i4
    IL_0e20:  ldloc.s    V_3
    IL_0e22:  ldloc.s    V_2
    IL_0e24:  ldc.i4.1
    IL_0e25:  sub
    IL_0e26:  dup
    IL_0e27:  stloc.s    V_2
    IL_0e29:  ldc.i4.s   24
    IL_0e2b:  stelem.i4
    IL_0e2c:  ldloc.s    V_2
    IL_0e2e:  ldc.i4     0x188
    IL_0e33:  ble.s      IL_0e41

    IL_0e35:  ldloc.s    V_4
    IL_0e37:  ldc.i4     0x126
    IL_0e3c:  bgt        IL_0c6b

    IL_0e41:  ldloc.s    V_3
    IL_0e43:  ldloc.s    V_2
    IL_0e45:  ldc.i4.1
    IL_0e46:  sub
    IL_0e47:  dup
    IL_0e48:  stloc.s    V_2
    IL_0e4a:  ldc.i4.s   25
    IL_0e4c:  stelem.i4
    IL_0e4d:  br         IL_200e

    IL_0e52:  ldloc.s    V_3
    IL_0e54:  ldloc.s    V_2
    IL_0e56:  ldc.i4.1
    IL_0e57:  sub
    IL_0e58:  dup
    IL_0e59:  stloc.s    V_2
    IL_0e5b:  ldloc.s    V_6
    IL_0e5d:  stelem.i4
    IL_0e5e:  ldloc.s    V_3
    IL_0e60:  ldloc.s    V_2
    IL_0e62:  ldc.i4.1
    IL_0e63:  sub
    IL_0e64:  dup
    IL_0e65:  stloc.s    V_2
    IL_0e67:  ldc.i4.s   11
    IL_0e69:  stelem.i4
    IL_0e6a:  ldarg.0
    IL_0e6b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0e70:  ldloc.s    V_0
    IL_0e72:  ldloc.s    V_10
    IL_0e74:  bge        IL_200e

    IL_0e79:  ldloc.s    V_1
    IL_0e7b:  ldloc.s    V_0
    IL_0e7d:  dup
    IL_0e7e:  ldc.i4.s   1
    IL_0e80:  add
    IL_0e81:  stloc.s    V_0
    IL_0e83:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0e88:  ldc.i4.s   58
    IL_0e8a:  bne.un     IL_200e

    IL_0e8f:  ldarg.0
    IL_0e90:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0e95:  ldloc.s    V_5
    IL_0e97:  ldloc.s    V_4
    IL_0e99:  ldc.i4.1
    IL_0e9a:  sub
    IL_0e9b:  dup
    IL_0e9c:  stloc.s    V_4
    IL_0e9e:  ldloc.s    V_0
    IL_0ea0:  stelem.i4
    IL_0ea1:  ldloc.s    V_3
    IL_0ea3:  ldloc.s    V_2
    IL_0ea5:  ldc.i4.1
    IL_0ea6:  sub
    IL_0ea7:  dup
    IL_0ea8:  stloc.s    V_2
    IL_0eaa:  ldc.i4.s   1
    IL_0eac:  stelem.i4
    IL_0ead:  ldarg.0
    IL_0eae:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0eb3:  ldloc.s    V_3
    IL_0eb5:  ldloc.s    V_2
    IL_0eb7:  ldc.i4.1
    IL_0eb8:  sub
    IL_0eb9:  dup
    IL_0eba:  stloc.s    V_2
    IL_0ebc:  ldloc.s    V_0
    IL_0ebe:  stelem.i4
    IL_0ebf:  ldloc.s    V_3
    IL_0ec1:  ldloc.s    V_2
    IL_0ec3:  ldc.i4.1
    IL_0ec4:  sub
    IL_0ec5:  dup
    IL_0ec6:  stloc.s    V_2
    IL_0ec8:  ldc.i4.s   26
    IL_0eca:  stelem.i4
    IL_0ecb:  ldarg.0
    IL_0ecc:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0ed1:  ldc.i4.s   1
    IL_0ed3:  ldloc.s    V_10
    IL_0ed5:  ldloc.s    V_0
    IL_0ed7:  sub
    IL_0ed8:  bgt        IL_200e

    IL_0edd:  ldloc.s    V_0
    IL_0edf:  ldc.i4.s   1
    IL_0ee1:  add
    IL_0ee2:  stloc.s    V_0
    IL_0ee4:  ldc.i4.s   1
    IL_0ee6:  stloc.s    V_6
    IL_0ee8:  ldloc.s    V_1
    IL_0eea:  ldloc.s    V_0
    IL_0eec:  ldloc.s    V_6
    IL_0eee:  dup
    IL_0eef:  ldc.i4.s   1
    IL_0ef1:  sub
    IL_0ef2:  stloc.s    V_6
    IL_0ef4:  sub
    IL_0ef5:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0efa:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 30 00 00 00 02 00 04 00   // ......-.0.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_0eff:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0f04:  brfalse    IL_200e

    IL_0f09:  ldloc.s    V_6
    IL_0f0b:  ldc.i4.s   0
    IL_0f0d:  bgt        IL_0ee8

    IL_0f12:  ldarg.0
    IL_0f13:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0f18:  ldloc.s    V_10
    IL_0f1a:  ldloc.s    V_0
    IL_0f1c:  sub
    IL_0f1d:  dup
    IL_0f1e:  stloc.s    V_7
    IL_0f20:  ldc.i4.s   1
    IL_0f22:  add
    IL_0f23:  stloc.s    V_6
    IL_0f25:  ldloc.s    V_6
    IL_0f27:  ldc.i4.s   1
    IL_0f29:  sub
    IL_0f2a:  dup
    IL_0f2b:  stloc.s    V_6
    IL_0f2d:  ldc.i4.s   0
    IL_0f2f:  ble        IL_0f59

    IL_0f34:  ldloc.s    V_1
    IL_0f36:  ldloc.s    V_0
    IL_0f38:  dup
    IL_0f39:  ldc.i4.s   1
    IL_0f3b:  add
    IL_0f3c:  stloc.s    V_0
    IL_0f3e:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0f43:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 30 00 00 00 02 00 04 00   // ......-.0.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_0f48:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0f4d:  brtrue     IL_0f25

    IL_0f52:  ldloc.s    V_0
    IL_0f54:  ldc.i4.s   1
    IL_0f56:  sub
    IL_0f57:  stloc.s    V_0
    IL_0f59:  ldloc.s    V_7
    IL_0f5b:  ldloc.s    V_6
    IL_0f5d:  ble.s      IL_0f8c

    IL_0f5f:  ldloc.s    V_3
    IL_0f61:  ldloc.s    V_2
    IL_0f63:  ldc.i4.1
    IL_0f64:  sub
    IL_0f65:  dup
    IL_0f66:  stloc.s    V_2
    IL_0f68:  ldloc.s    V_7
    IL_0f6a:  ldloc.s    V_6
    IL_0f6c:  sub
    IL_0f6d:  ldc.i4.s   1
    IL_0f6f:  sub
    IL_0f70:  stelem.i4
    IL_0f71:  ldloc.s    V_3
    IL_0f73:  ldloc.s    V_2
    IL_0f75:  ldc.i4.1
    IL_0f76:  sub
    IL_0f77:  dup
    IL_0f78:  stloc.s    V_2
    IL_0f7a:  ldloc.s    V_0
    IL_0f7c:  ldc.i4.s   1
    IL_0f7e:  sub
    IL_0f7f:  stelem.i4
    IL_0f80:  ldloc.s    V_3
    IL_0f82:  ldloc.s    V_2
    IL_0f84:  ldc.i4.1
    IL_0f85:  sub
    IL_0f86:  dup
    IL_0f87:  stloc.s    V_2
    IL_0f89:  ldc.i4.s   27
    IL_0f8b:  stelem.i4
    IL_0f8c:  ldarg.0
    IL_0f8d:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0f92:  ldloc.s    V_0
    IL_0f94:  ldloc.s    V_10
    IL_0f96:  bge        IL_200e

    IL_0f9b:  ldloc.s    V_1
    IL_0f9d:  ldloc.s    V_0
    IL_0f9f:  dup
    IL_0fa0:  ldc.i4.s   1
    IL_0fa2:  add
    IL_0fa3:  stloc.s    V_0
    IL_0fa5:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0faa:  ldc.i4.s   36
    IL_0fac:  bne.un     IL_200e

    IL_0fb1:  ldarg.0
    IL_0fb2:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0fb7:  br         IL_1cce

    IL_0fbc:  ldarg.0
    IL_0fbd:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0fc2:  ldloc.s    V_3
    IL_0fc4:  ldloc.s    V_2
    IL_0fc6:  ldc.i4.1
    IL_0fc7:  sub
    IL_0fc8:  dup
    IL_0fc9:  stloc.s    V_2
    IL_0fcb:  ldloc.s    V_0
    IL_0fcd:  stelem.i4
    IL_0fce:  ldloc.s    V_3
    IL_0fd0:  ldloc.s    V_2
    IL_0fd2:  ldc.i4.1
    IL_0fd3:  sub
    IL_0fd4:  dup
    IL_0fd5:  stloc.s    V_2
    IL_0fd7:  ldc.i4.s   28
    IL_0fd9:  stelem.i4
    IL_0fda:  ldarg.0
    IL_0fdb:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0fe0:  ldc.i4.s   1
    IL_0fe2:  ldloc.s    V_10
    IL_0fe4:  ldloc.s    V_0
    IL_0fe6:  sub
    IL_0fe7:  bgt        IL_200e

    IL_0fec:  ldloc.s    V_0
    IL_0fee:  ldc.i4.s   1
    IL_0ff0:  add
    IL_0ff1:  stloc.s    V_0
    IL_0ff3:  ldc.i4.s   1
    IL_0ff5:  stloc.s    V_6
    IL_0ff7:  ldloc.s    V_1
    IL_0ff9:  ldloc.s    V_0
    IL_0ffb:  ldloc.s    V_6
    IL_0ffd:  dup
    IL_0ffe:  ldc.i4.s   1
    IL_1000:  sub
    IL_1001:  stloc.s    V_6
    IL_1003:  sub
    IL_1004:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1009:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_100e:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_1013:  brfalse    IL_200e

    IL_1018:  ldloc.s    V_6
    IL_101a:  ldc.i4.s   0
    IL_101c:  bgt        IL_0ff7

    IL_1021:  ldarg.0
    IL_1022:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1027:  ldloc.s    V_10
    IL_1029:  ldloc.s    V_0
    IL_102b:  sub
    IL_102c:  dup
    IL_102d:  stloc.s    V_7
    IL_102f:  ldc.i4.s   1
    IL_1031:  add
    IL_1032:  stloc.s    V_6
    IL_1034:  ldloc.s    V_6
    IL_1036:  ldc.i4.s   1
    IL_1038:  sub
    IL_1039:  dup
    IL_103a:  stloc.s    V_6
    IL_103c:  ldc.i4.s   0
    IL_103e:  ble        IL_1068

    IL_1043:  ldloc.s    V_1
    IL_1045:  ldloc.s    V_0
    IL_1047:  dup
    IL_1048:  ldc.i4.s   1
    IL_104a:  add
    IL_104b:  stloc.s    V_0
    IL_104d:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1052:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_1057:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_105c:  brtrue     IL_1034

    IL_1061:  ldloc.s    V_0
    IL_1063:  ldc.i4.s   1
    IL_1065:  sub
    IL_1066:  stloc.s    V_0
    IL_1068:  ldloc.s    V_7
    IL_106a:  ldloc.s    V_6
    IL_106c:  ble.s      IL_109b

    IL_106e:  ldloc.s    V_3
    IL_1070:  ldloc.s    V_2
    IL_1072:  ldc.i4.1
    IL_1073:  sub
    IL_1074:  dup
    IL_1075:  stloc.s    V_2
    IL_1077:  ldloc.s    V_7
    IL_1079:  ldloc.s    V_6
    IL_107b:  sub
    IL_107c:  ldc.i4.s   1
    IL_107e:  sub
    IL_107f:  stelem.i4
    IL_1080:  ldloc.s    V_3
    IL_1082:  ldloc.s    V_2
    IL_1084:  ldc.i4.1
    IL_1085:  sub
    IL_1086:  dup
    IL_1087:  stloc.s    V_2
    IL_1089:  ldloc.s    V_0
    IL_108b:  ldc.i4.s   1
    IL_108d:  sub
    IL_108e:  stelem.i4
    IL_108f:  ldloc.s    V_3
    IL_1091:  ldloc.s    V_2
    IL_1093:  ldc.i4.1
    IL_1094:  sub
    IL_1095:  dup
    IL_1096:  stloc.s    V_2
    IL_1098:  ldc.i4.s   29
    IL_109a:  stelem.i4
    IL_109b:  ldarg.0
    IL_109c:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_10a1:  ldc.i4.s   2
    IL_10a3:  ldloc.s    V_10
    IL_10a5:  ldloc.s    V_0
    IL_10a7:  sub
    IL_10a8:  bgt        IL_200e

    IL_10ad:  ldloc.s    V_1
    IL_10af:  ldloc.s    V_0
    IL_10b1:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_10b6:  ldc.i4.s   58
    IL_10b8:  bne.un     IL_200e

    IL_10bd:  ldloc.s    V_1
    IL_10bf:  ldloc.s    V_0
    IL_10c1:  ldc.i4.s   1
    IL_10c3:  add
    IL_10c4:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_10c9:  ldc.i4.s   123
    IL_10cb:  bne.un     IL_200e

    IL_10d0:  ldloc.s    V_0
    IL_10d2:  ldc.i4.s   2
    IL_10d4:  add
    IL_10d5:  stloc.s    V_0
    IL_10d7:  ldarg.0
    IL_10d8:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_10dd:  br         IL_1cce

    IL_10e2:  ldarg.0
    IL_10e3:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_10e8:  ldc.i4.s   1
    IL_10ea:  ldloc.s    V_10
    IL_10ec:  ldloc.s    V_0
    IL_10ee:  sub
    IL_10ef:  bgt        IL_200e

    IL_10f4:  ldloc.s    V_0
    IL_10f6:  ldc.i4.s   1
    IL_10f8:  add
    IL_10f9:  stloc.s    V_0
    IL_10fb:  ldc.i4.s   1
    IL_10fd:  stloc.s    V_6
    IL_10ff:  ldloc.s    V_1
    IL_1101:  ldloc.s    V_0
    IL_1103:  ldloc.s    V_6
    IL_1105:  dup
    IL_1106:  ldc.i4.s   1
    IL_1108:  sub
    IL_1109:  stloc.s    V_6
    IL_110b:  sub
    IL_110c:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1111:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_1116:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_111b:  brfalse    IL_200e

    IL_1120:  ldloc.s    V_6
    IL_1122:  ldc.i4.s   0
    IL_1124:  bgt        IL_10ff

    IL_1129:  ldarg.0
    IL_112a:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_112f:  ldloc.s    V_10
    IL_1131:  ldloc.s    V_0
    IL_1133:  sub
    IL_1134:  dup
    IL_1135:  stloc.s    V_7
    IL_1137:  ldc.i4.s   1
    IL_1139:  add
    IL_113a:  stloc.s    V_6
    IL_113c:  ldloc.s    V_6
    IL_113e:  ldc.i4.s   1
    IL_1140:  sub
    IL_1141:  dup
    IL_1142:  stloc.s    V_6
    IL_1144:  ldc.i4.s   0
    IL_1146:  ble        IL_1170

    IL_114b:  ldloc.s    V_1
    IL_114d:  ldloc.s    V_0
    IL_114f:  dup
    IL_1150:  ldc.i4.s   1
    IL_1152:  add
    IL_1153:  stloc.s    V_0
    IL_1155:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_115a:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_115f:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_1164:  brtrue     IL_113c

    IL_1169:  ldloc.s    V_0
    IL_116b:  ldc.i4.s   1
    IL_116d:  sub
    IL_116e:  stloc.s    V_0
    IL_1170:  ldloc.s    V_7
    IL_1172:  ldloc.s    V_6
    IL_1174:  ble.s      IL_11a3

    IL_1176:  ldloc.s    V_3
    IL_1178:  ldloc.s    V_2
    IL_117a:  ldc.i4.1
    IL_117b:  sub
    IL_117c:  dup
    IL_117d:  stloc.s    V_2
    IL_117f:  ldloc.s    V_7
    IL_1181:  ldloc.s    V_6
    IL_1183:  sub
    IL_1184:  ldc.i4.s   1
    IL_1186:  sub
    IL_1187:  stelem.i4
    IL_1188:  ldloc.s    V_3
    IL_118a:  ldloc.s    V_2
    IL_118c:  ldc.i4.1
    IL_118d:  sub
    IL_118e:  dup
    IL_118f:  stloc.s    V_2
    IL_1191:  ldloc.s    V_0
    IL_1193:  ldc.i4.s   1
    IL_1195:  sub
    IL_1196:  stelem.i4
    IL_1197:  ldloc.s    V_3
    IL_1199:  ldloc.s    V_2
    IL_119b:  ldc.i4.1
    IL_119c:  sub
    IL_119d:  dup
    IL_119e:  stloc.s    V_2
    IL_11a0:  ldc.i4.s   30
    IL_11a2:  stelem.i4
    IL_11a3:  ldarg.0
    IL_11a4:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_11a9:  ldloc.s    V_0
    IL_11ab:  ldloc.s    V_10
    IL_11ad:  bge        IL_200e

    IL_11b2:  ldloc.s    V_1
    IL_11b4:  ldloc.s    V_0
    IL_11b6:  dup
    IL_11b7:  ldc.i4.s   1
    IL_11b9:  add
    IL_11ba:  stloc.s    V_0
    IL_11bc:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_11c1:  ldc.i4.s   63
    IL_11c3:  bne.un     IL_200e

    IL_11c8:  ldarg.0
    IL_11c9:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_11ce:  ldc.i4.s   1
    IL_11d0:  ldloc.s    V_10
    IL_11d2:  ldloc.s    V_0
    IL_11d4:  sub
    IL_11d5:  bgt        IL_200e

    IL_11da:  ldloc.s    V_0
    IL_11dc:  ldc.i4.s   1
    IL_11de:  add
    IL_11df:  stloc.s    V_0
    IL_11e1:  ldc.i4.s   1
    IL_11e3:  stloc.s    V_6
    IL_11e5:  ldloc.s    V_1
    IL_11e7:  ldloc.s    V_0
    IL_11e9:  ldloc.s    V_6
    IL_11eb:  dup
    IL_11ec:  ldc.i4.s   1
    IL_11ee:  sub
    IL_11ef:  stloc.s    V_6
    IL_11f1:  sub
    IL_11f2:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_11f7:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_11fc:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_1201:  brfalse    IL_200e

    IL_1206:  ldloc.s    V_6
    IL_1208:  ldc.i4.s   0
    IL_120a:  bgt        IL_11e5

    IL_120f:  ldarg.0
    IL_1210:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1215:  ldloc.s    V_10
    IL_1217:  ldloc.s    V_0
    IL_1219:  sub
    IL_121a:  dup
    IL_121b:  stloc.s    V_7
    IL_121d:  ldc.i4.s   1
    IL_121f:  add
    IL_1220:  stloc.s    V_6
    IL_1222:  ldloc.s    V_6
    IL_1224:  ldc.i4.s   1
    IL_1226:  sub
    IL_1227:  dup
    IL_1228:  stloc.s    V_6
    IL_122a:  ldc.i4.s   0
    IL_122c:  ble        IL_1256

    IL_1231:  ldloc.s    V_1
    IL_1233:  ldloc.s    V_0
    IL_1235:  dup
    IL_1236:  ldc.i4.s   1
    IL_1238:  add
    IL_1239:  stloc.s    V_0
    IL_123b:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1240:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_1245:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_124a:  brtrue     IL_1222

    IL_124f:  ldloc.s    V_0
    IL_1251:  ldc.i4.s   1
    IL_1253:  sub
    IL_1254:  stloc.s    V_0
    IL_1256:  ldloc.s    V_7
    IL_1258:  ldloc.s    V_6
    IL_125a:  ble.s      IL_1289

    IL_125c:  ldloc.s    V_3
    IL_125e:  ldloc.s    V_2
    IL_1260:  ldc.i4.1
    IL_1261:  sub
    IL_1262:  dup
    IL_1263:  stloc.s    V_2
    IL_1265:  ldloc.s    V_7
    IL_1267:  ldloc.s    V_6
    IL_1269:  sub
    IL_126a:  ldc.i4.s   1
    IL_126c:  sub
    IL_126d:  stelem.i4
    IL_126e:  ldloc.s    V_3
    IL_1270:  ldloc.s    V_2
    IL_1272:  ldc.i4.1
    IL_1273:  sub
    IL_1274:  dup
    IL_1275:  stloc.s    V_2
    IL_1277:  ldloc.s    V_0
    IL_1279:  ldc.i4.s   1
    IL_127b:  sub
    IL_127c:  stelem.i4
    IL_127d:  ldloc.s    V_3
    IL_127f:  ldloc.s    V_2
    IL_1281:  ldc.i4.1
    IL_1282:  sub
    IL_1283:  dup
    IL_1284:  stloc.s    V_2
    IL_1286:  ldc.i4.s   31
    IL_1288:  stelem.i4
    IL_1289:  ldarg.0
    IL_128a:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_128f:  ldloc.s    V_5
    IL_1291:  ldloc.s    V_4
    IL_1293:  ldc.i4.1
    IL_1294:  sub
    IL_1295:  dup
    IL_1296:  stloc.s    V_4
    IL_1298:  ldc.i4.s   -1
    IL_129a:  stelem.i4
    IL_129b:  ldloc.s    V_5
    IL_129d:  ldloc.s    V_4
    IL_129f:  ldc.i4.1
    IL_12a0:  sub
    IL_12a1:  dup
    IL_12a2:  stloc.s    V_4
    IL_12a4:  ldc.i4.s   0
    IL_12a6:  stelem.i4
    IL_12a7:  ldloc.s    V_3
    IL_12a9:  ldloc.s    V_2
    IL_12ab:  ldc.i4.1
    IL_12ac:  sub
    IL_12ad:  dup
    IL_12ae:  stloc.s    V_2
    IL_12b0:  ldc.i4.s   5
    IL_12b2:  stelem.i4
    IL_12b3:  ldarg.0
    IL_12b4:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_12b9:  br         IL_1c09

    IL_12be:  ldarg.0
    IL_12bf:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_12c4:  ldloc.s    V_5
    IL_12c6:  ldloc.s    V_4
    IL_12c8:  ldc.i4.1
    IL_12c9:  sub
    IL_12ca:  dup
    IL_12cb:  stloc.s    V_4
    IL_12cd:  ldloc.s    V_0
    IL_12cf:  stelem.i4
    IL_12d0:  ldloc.s    V_3
    IL_12d2:  ldloc.s    V_2
    IL_12d4:  ldc.i4.1
    IL_12d5:  sub
    IL_12d6:  dup
    IL_12d7:  stloc.s    V_2
    IL_12d9:  ldc.i4.s   1
    IL_12db:  stelem.i4
    IL_12dc:  ldarg.0
    IL_12dd:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_12e2:  ldloc.s    V_0
    IL_12e4:  ldloc.s    V_10
    IL_12e6:  bge        IL_200e

    IL_12eb:  ldloc.s    V_1
    IL_12ed:  ldloc.s    V_0
    IL_12ef:  dup
    IL_12f0:  ldc.i4.s   1
    IL_12f2:  add
    IL_12f3:  stloc.s    V_0
    IL_12f5:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_12fa:  ldc.i4.s   44
    IL_12fc:  bne.un     IL_200e

    IL_1301:  ldarg.0
    IL_1302:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1307:  ldloc.s    V_5
    IL_1309:  ldloc.s    V_4
    IL_130b:  ldc.i4.1
    IL_130c:  sub
    IL_130d:  dup
    IL_130e:  stloc.s    V_4
    IL_1310:  ldc.i4.s   -1
    IL_1312:  stelem.i4
    IL_1313:  ldloc.s    V_3
    IL_1315:  ldloc.s    V_2
    IL_1317:  ldc.i4.1
    IL_1318:  sub
    IL_1319:  dup
    IL_131a:  stloc.s    V_2
    IL_131c:  ldc.i4.s   1
    IL_131e:  stelem.i4
    IL_131f:  ldarg.0
    IL_1320:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1325:  br         IL_1b5a

    IL_132a:  ldarg.0
    IL_132b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1330:  ldloc.s    V_5
    IL_1332:  ldloc.s    V_4
    IL_1334:  ldc.i4.1
    IL_1335:  sub
    IL_1336:  dup
    IL_1337:  stloc.s    V_4
    IL_1339:  ldloc.s    V_0
    IL_133b:  stelem.i4
    IL_133c:  ldloc.s    V_3
    IL_133e:  ldloc.s    V_2
    IL_1340:  ldc.i4.1
    IL_1341:  sub
    IL_1342:  dup
    IL_1343:  stloc.s    V_2
    IL_1345:  ldc.i4.s   1
    IL_1347:  stelem.i4
    IL_1348:  ldarg.0
    IL_1349:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_134e:  ldloc.s    V_5
    IL_1350:  ldloc.s    V_4
    IL_1352:  ldc.i4.1
    IL_1353:  sub
    IL_1354:  dup
    IL_1355:  stloc.s    V_4
    IL_1357:  ldc.i4.s   -1
    IL_1359:  stelem.i4
    IL_135a:  ldloc.s    V_5
    IL_135c:  ldloc.s    V_4
    IL_135e:  ldc.i4.1
    IL_135f:  sub
    IL_1360:  dup
    IL_1361:  stloc.s    V_4
    IL_1363:  ldc.i4.s   0
    IL_1365:  stelem.i4
    IL_1366:  ldloc.s    V_3
    IL_1368:  ldloc.s    V_2
    IL_136a:  ldc.i4.1
    IL_136b:  sub
    IL_136c:  dup
    IL_136d:  stloc.s    V_2
    IL_136f:  ldc.i4.s   5
    IL_1371:  stelem.i4
    IL_1372:  ldarg.0
    IL_1373:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1378:  br         IL_13f6

    IL_137d:  ldarg.0
    IL_137e:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1383:  ldloc.s    V_5
    IL_1385:  ldloc.s    V_4
    IL_1387:  ldc.i4.1
    IL_1388:  sub
    IL_1389:  dup
    IL_138a:  stloc.s    V_4
    IL_138c:  ldloc.s    V_0
    IL_138e:  stelem.i4
    IL_138f:  ldloc.s    V_3
    IL_1391:  ldloc.s    V_2
    IL_1393:  ldc.i4.1
    IL_1394:  sub
    IL_1395:  dup
    IL_1396:  stloc.s    V_2
    IL_1398:  ldc.i4.s   1
    IL_139a:  stelem.i4
    IL_139b:  ldarg.0
    IL_139c:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_13a1:  ldloc.s    V_0
    IL_13a3:  ldloc.s    V_10
    IL_13a5:  bge        IL_200e

    IL_13aa:  ldloc.s    V_1
    IL_13ac:  ldloc.s    V_0
    IL_13ae:  dup
    IL_13af:  ldc.i4.s   1
    IL_13b1:  add
    IL_13b2:  stloc.s    V_0
    IL_13b4:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_13b9:  ldc.i4.s   124
    IL_13bb:  bne.un     IL_200e

    IL_13c0:  ldarg.0
    IL_13c1:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_13c6:  ldloc.s    V_5
    IL_13c8:  ldloc.s    V_4
    IL_13ca:  dup
    IL_13cb:  ldc.i4.1
    IL_13cc:  add
    IL_13cd:  stloc.s    V_4
    IL_13cf:  ldelem.i4
    IL_13d0:  stloc.s    V_6
    IL_13d2:  ldarg.0
    IL_13d3:  ldc.i4.s   12
    IL_13d5:  ldloc.s    V_6
    IL_13d7:  ldloc.s    V_0
    IL_13d9:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_13de:  ldloc.s    V_3
    IL_13e0:  ldloc.s    V_2
    IL_13e2:  ldc.i4.1
    IL_13e3:  sub
    IL_13e4:  dup
    IL_13e5:  stloc.s    V_2
    IL_13e7:  ldloc.s    V_6
    IL_13e9:  stelem.i4
    IL_13ea:  ldloc.s    V_3
    IL_13ec:  ldloc.s    V_2
    IL_13ee:  ldc.i4.1
    IL_13ef:  sub
    IL_13f0:  dup
    IL_13f1:  stloc.s    V_2
    IL_13f3:  ldc.i4.s   8
    IL_13f5:  stelem.i4
    IL_13f6:  ldarg.0
    IL_13f7:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_13fc:  ldloc.s    V_5
    IL_13fe:  ldloc.s    V_4
    IL_1400:  dup
    IL_1401:  ldc.i4.1
    IL_1402:  add
    IL_1403:  stloc.s    V_4
    IL_1405:  ldelem.i4
    IL_1406:  stloc.s    V_6
    IL_1408:  ldloc.s    V_5
    IL_140a:  ldloc.s    V_4
    IL_140c:  dup
    IL_140d:  ldc.i4.1
    IL_140e:  add
    IL_140f:  stloc.s    V_4
    IL_1411:  ldelem.i4
    IL_1412:  dup
    IL_1413:  stloc.s    V_7
    IL_1415:  ldloc.s    V_3
    IL_1417:  ldloc.s    V_2
    IL_1419:  ldc.i4.1
    IL_141a:  sub
    IL_141b:  dup
    IL_141c:  stloc.s    V_2
    IL_141e:  ldloc.s    V_7
    IL_1420:  stelem.i4
    IL_1421:  ldloc.s    V_0
    IL_1423:  bne.un.s   IL_142b

    IL_1425:  ldloc.s    V_6
    IL_1427:  ldc.i4.s   0
    IL_1429:  bge.s      IL_147e

    IL_142b:  ldloc.s    V_6
    IL_142d:  ldc.i4.s   1
    IL_142f:  bge.s      IL_147e

    IL_1431:  ldloc.s    V_5
    IL_1433:  ldloc.s    V_4
    IL_1435:  ldc.i4.1
    IL_1436:  sub
    IL_1437:  dup
    IL_1438:  stloc.s    V_4
    IL_143a:  ldloc.s    V_0
    IL_143c:  stelem.i4
    IL_143d:  ldloc.s    V_5
    IL_143f:  ldloc.s    V_4
    IL_1441:  ldc.i4.1
    IL_1442:  sub
    IL_1443:  dup
    IL_1444:  stloc.s    V_4
    IL_1446:  ldloc.s    V_6
    IL_1448:  ldc.i4.s   1
    IL_144a:  add
    IL_144b:  stelem.i4
    IL_144c:  ldloc.s    V_3
    IL_144e:  ldloc.s    V_2
    IL_1450:  ldc.i4.1
    IL_1451:  sub
    IL_1452:  dup
    IL_1453:  stloc.s    V_2
    IL_1455:  ldc.i4.s   32
    IL_1457:  stelem.i4
    IL_1458:  ldloc.s    V_2
    IL_145a:  ldc.i4     0x188
    IL_145f:  ble.s      IL_146d

    IL_1461:  ldloc.s    V_4
    IL_1463:  ldc.i4     0x126
    IL_1468:  bgt        IL_137d

    IL_146d:  ldloc.s    V_3
    IL_146f:  ldloc.s    V_2
    IL_1471:  ldc.i4.1
    IL_1472:  sub
    IL_1473:  dup
    IL_1474:  stloc.s    V_2
    IL_1476:  ldc.i4.s   33
    IL_1478:  stelem.i4
    IL_1479:  br         IL_200e

    IL_147e:  ldloc.s    V_3
    IL_1480:  ldloc.s    V_2
    IL_1482:  ldc.i4.1
    IL_1483:  sub
    IL_1484:  dup
    IL_1485:  stloc.s    V_2
    IL_1487:  ldloc.s    V_6
    IL_1489:  stelem.i4
    IL_148a:  ldloc.s    V_3
    IL_148c:  ldloc.s    V_2
    IL_148e:  ldc.i4.1
    IL_148f:  sub
    IL_1490:  dup
    IL_1491:  stloc.s    V_2
    IL_1493:  ldc.i4.s   11
    IL_1495:  stelem.i4
    IL_1496:  ldarg.0
    IL_1497:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_149c:  ldloc.s    V_5
    IL_149e:  ldloc.s    V_4
    IL_14a0:  ldc.i4.1
    IL_14a1:  sub
    IL_14a2:  dup
    IL_14a3:  stloc.s    V_4
    IL_14a5:  ldc.i4.s   -1
    IL_14a7:  stelem.i4
    IL_14a8:  ldloc.s    V_5
    IL_14aa:  ldloc.s    V_4
    IL_14ac:  ldc.i4.1
    IL_14ad:  sub
    IL_14ae:  dup
    IL_14af:  stloc.s    V_4
    IL_14b1:  ldc.i4.s   0
    IL_14b3:  stelem.i4
    IL_14b4:  ldloc.s    V_3
    IL_14b6:  ldloc.s    V_2
    IL_14b8:  ldc.i4.1
    IL_14b9:  sub
    IL_14ba:  dup
    IL_14bb:  stloc.s    V_2
    IL_14bd:  ldc.i4.s   5
    IL_14bf:  stelem.i4
    IL_14c0:  ldarg.0
    IL_14c1:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_14c6:  br         IL_1a84

    IL_14cb:  ldarg.0
    IL_14cc:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_14d1:  ldloc.s    V_5
    IL_14d3:  ldloc.s    V_4
    IL_14d5:  ldc.i4.1
    IL_14d6:  sub
    IL_14d7:  dup
    IL_14d8:  stloc.s    V_4
    IL_14da:  ldloc.s    V_0
    IL_14dc:  stelem.i4
    IL_14dd:  ldloc.s    V_3
    IL_14df:  ldloc.s    V_2
    IL_14e1:  ldc.i4.1
    IL_14e2:  sub
    IL_14e3:  dup
    IL_14e4:  stloc.s    V_2
    IL_14e6:  ldc.i4.s   1
    IL_14e8:  stelem.i4
    IL_14e9:  ldarg.0
    IL_14ea:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_14ef:  ldloc.s    V_3
    IL_14f1:  ldloc.s    V_2
    IL_14f3:  ldc.i4.1
    IL_14f4:  sub
    IL_14f5:  dup
    IL_14f6:  stloc.s    V_2
    IL_14f8:  ldloc.s    V_0
    IL_14fa:  stelem.i4
    IL_14fb:  ldloc.s    V_3
    IL_14fd:  ldloc.s    V_2
    IL_14ff:  ldc.i4.1
    IL_1500:  sub
    IL_1501:  dup
    IL_1502:  stloc.s    V_2
    IL_1504:  ldc.i4.s   34
    IL_1506:  stelem.i4
    IL_1507:  ldarg.0
    IL_1508:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_150d:  ldloc.s    V_10
    IL_150f:  ldloc.s    V_0
    IL_1511:  sub
    IL_1512:  dup
    IL_1513:  stloc.s    V_7
    IL_1515:  ldc.i4.s   1
    IL_1517:  add
    IL_1518:  stloc.s    V_6
    IL_151a:  ldloc.s    V_6
    IL_151c:  ldc.i4.s   1
    IL_151e:  sub
    IL_151f:  dup
    IL_1520:  stloc.s    V_6
    IL_1522:  ldc.i4.s   0
    IL_1524:  ble        IL_154e

    IL_1529:  ldloc.s    V_1
    IL_152b:  ldloc.s    V_0
    IL_152d:  dup
    IL_152e:  ldc.i4.s   1
    IL_1530:  add
    IL_1531:  stloc.s    V_0
    IL_1533:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1538:  ldstr      bytearray (00 00 0A 00 00 00 23 00 24 00 2A 00 2C 00 2D 00   // ......#.$.*.,.-.
                                    2F 00 5E 00 5F 00 7E 00 7F 00 )                   // /.^._.~...
    IL_153d:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_1542:  brtrue     IL_151a

    IL_1547:  ldloc.s    V_0
    IL_1549:  ldc.i4.s   1
    IL_154b:  sub
    IL_154c:  stloc.s    V_0
    IL_154e:  ldloc.s    V_7
    IL_1550:  ldloc.s    V_6
    IL_1552:  ble.s      IL_1581

    IL_1554:  ldloc.s    V_3
    IL_1556:  ldloc.s    V_2
    IL_1558:  ldc.i4.1
    IL_1559:  sub
    IL_155a:  dup
    IL_155b:  stloc.s    V_2
    IL_155d:  ldloc.s    V_7
    IL_155f:  ldloc.s    V_6
    IL_1561:  sub
    IL_1562:  ldc.i4.s   1
    IL_1564:  sub
    IL_1565:  stelem.i4
    IL_1566:  ldloc.s    V_3
    IL_1568:  ldloc.s    V_2
    IL_156a:  ldc.i4.1
    IL_156b:  sub
    IL_156c:  dup
    IL_156d:  stloc.s    V_2
    IL_156f:  ldloc.s    V_0
    IL_1571:  ldc.i4.s   1
    IL_1573:  sub
    IL_1574:  stelem.i4
    IL_1575:  ldloc.s    V_3
    IL_1577:  ldloc.s    V_2
    IL_1579:  ldc.i4.1
    IL_157a:  sub
    IL_157b:  dup
    IL_157c:  stloc.s    V_2
    IL_157e:  ldc.i4.s   35
    IL_1580:  stelem.i4
    IL_1581:  ldarg.0
    IL_1582:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1587:  ldloc.s    V_5
    IL_1589:  ldloc.s    V_4
    IL_158b:  ldc.i4.1
    IL_158c:  sub
    IL_158d:  dup
    IL_158e:  stloc.s    V_4
    IL_1590:  ldloc.s    V_0
    IL_1592:  stelem.i4
    IL_1593:  ldloc.s    V_3
    IL_1595:  ldloc.s    V_2
    IL_1597:  ldc.i4.1
    IL_1598:  sub
    IL_1599:  dup
    IL_159a:  stloc.s    V_2
    IL_159c:  ldc.i4.s   1
    IL_159e:  stelem.i4
    IL_159f:  ldarg.0
    IL_15a0:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_15a5:  ldloc.s    V_0
    IL_15a7:  ldloc.s    V_10
    IL_15a9:  bge        IL_200e

    IL_15ae:  ldloc.s    V_1
    IL_15b0:  ldloc.s    V_0
    IL_15b2:  dup
    IL_15b3:  ldc.i4.s   1
    IL_15b5:  add
    IL_15b6:  stloc.s    V_0
    IL_15b8:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_15bd:  ldstr      bytearray (00 00 02 00 0A 00 2B 00 2C 00 00 00 02 00 04 00   // ......+.,.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_15c2:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_15c7:  brfalse    IL_200e

    IL_15cc:  ldarg.0
    IL_15cd:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_15d2:  ldloc.s    V_10
    IL_15d4:  ldloc.s    V_0
    IL_15d6:  sub
    IL_15d7:  dup
    IL_15d8:  stloc.s    V_7
    IL_15da:  ldc.i4.s   1
    IL_15dc:  add
    IL_15dd:  stloc.s    V_6
    IL_15df:  ldloc.s    V_6
    IL_15e1:  ldc.i4.s   1
    IL_15e3:  sub
    IL_15e4:  dup
    IL_15e5:  stloc.s    V_6
    IL_15e7:  ldc.i4.s   0
    IL_15e9:  ble.s      IL_1605

    IL_15eb:  ldloc.s    V_1
    IL_15ed:  ldloc.s    V_0
    IL_15ef:  dup
    IL_15f0:  ldc.i4.s   1
    IL_15f2:  add
    IL_15f3:  stloc.s    V_0
    IL_15f5:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_15fa:  ldc.i4.s   36
    IL_15fc:  bne.un.s   IL_15df

    IL_15fe:  ldloc.s    V_0
    IL_1600:  ldc.i4.s   1
    IL_1602:  sub
    IL_1603:  stloc.s    V_0
    IL_1605:  ldloc.s    V_7
    IL_1607:  ldloc.s    V_6
    IL_1609:  ble.s      IL_1638

    IL_160b:  ldloc.s    V_3
    IL_160d:  ldloc.s    V_2
    IL_160f:  ldc.i4.1
    IL_1610:  sub
    IL_1611:  dup
    IL_1612:  stloc.s    V_2
    IL_1614:  ldloc.s    V_7
    IL_1616:  ldloc.s    V_6
    IL_1618:  sub
    IL_1619:  ldc.i4.s   1
    IL_161b:  sub
    IL_161c:  stelem.i4
    IL_161d:  ldloc.s    V_3
    IL_161f:  ldloc.s    V_2
    IL_1621:  ldc.i4.1
    IL_1622:  sub
    IL_1623:  dup
    IL_1624:  stloc.s    V_2
    IL_1626:  ldloc.s    V_0
    IL_1628:  ldc.i4.s   1
    IL_162a:  sub
    IL_162b:  stelem.i4
    IL_162c:  ldloc.s    V_3
    IL_162e:  ldloc.s    V_2
    IL_1630:  ldc.i4.1
    IL_1631:  sub
    IL_1632:  dup
    IL_1633:  stloc.s    V_2
    IL_1635:  ldc.i4.s   36
    IL_1637:  stelem.i4
    IL_1638:  ldarg.0
    IL_1639:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_163e:  ldloc.s    V_5
    IL_1640:  ldloc.s    V_4
    IL_1642:  dup
    IL_1643:  ldc.i4.1
    IL_1644:  add
    IL_1645:  stloc.s    V_4
    IL_1647:  ldelem.i4
    IL_1648:  stloc.s    V_6
    IL_164a:  ldarg.0
    IL_164b:  ldc.i4.s   14
    IL_164d:  ldloc.s    V_6
    IL_164f:  ldloc.s    V_0
    IL_1651:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_1656:  ldloc.s    V_3
    IL_1658:  ldloc.s    V_2
    IL_165a:  ldc.i4.1
    IL_165b:  sub
    IL_165c:  dup
    IL_165d:  stloc.s    V_2
    IL_165f:  ldloc.s    V_6
    IL_1661:  stelem.i4
    IL_1662:  ldloc.s    V_3
    IL_1664:  ldloc.s    V_2
    IL_1666:  ldc.i4.1
    IL_1667:  sub
    IL_1668:  dup
    IL_1669:  stloc.s    V_2
    IL_166b:  ldc.i4.s   8
    IL_166d:  stelem.i4
    IL_166e:  ldarg.0
    IL_166f:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1674:  br         IL_1a4e

    IL_1679:  ldarg.0
    IL_167a:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_167f:  ldloc.s    V_3
    IL_1681:  ldloc.s    V_2
    IL_1683:  ldc.i4.1
    IL_1684:  sub
    IL_1685:  dup
    IL_1686:  stloc.s    V_2
    IL_1688:  ldloc.s    V_0
    IL_168a:  stelem.i4
    IL_168b:  ldloc.s    V_3
    IL_168d:  ldloc.s    V_2
    IL_168f:  ldc.i4.1
    IL_1690:  sub
    IL_1691:  dup
    IL_1692:  stloc.s    V_2
    IL_1694:  ldc.i4.s   37
    IL_1696:  stelem.i4
    IL_1697:  ldarg.0
    IL_1698:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_169d:  ldloc.s    V_0
    IL_169f:  ldloc.s    V_10
    IL_16a1:  bge        IL_200e

    IL_16a6:  ldloc.s    V_1
    IL_16a8:  ldloc.s    V_0
    IL_16aa:  dup
    IL_16ab:  ldc.i4.s   1
    IL_16ad:  add
    IL_16ae:  stloc.s    V_0
    IL_16b0:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_16b5:  ldc.i4.s   61
    IL_16b7:  bne.un     IL_200e

    IL_16bc:  ldarg.0
    IL_16bd:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_16c2:  ldloc.s    V_5
    IL_16c4:  ldloc.s    V_4
    IL_16c6:  ldc.i4.1
    IL_16c7:  sub
    IL_16c8:  dup
    IL_16c9:  stloc.s    V_4
    IL_16cb:  ldloc.s    V_0
    IL_16cd:  stelem.i4
    IL_16ce:  ldloc.s    V_3
    IL_16d0:  ldloc.s    V_2
    IL_16d2:  ldc.i4.1
    IL_16d3:  sub
    IL_16d4:  dup
    IL_16d5:  stloc.s    V_2
    IL_16d7:  ldc.i4.s   1
    IL_16d9:  stelem.i4
    IL_16da:  ldarg.0
    IL_16db:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_16e0:  ldloc.s    V_0
    IL_16e2:  ldloc.s    V_10
    IL_16e4:  bge        IL_200e

    IL_16e9:  ldloc.s    V_1
    IL_16eb:  ldloc.s    V_0
    IL_16ed:  dup
    IL_16ee:  ldc.i4.s   1
    IL_16f0:  add
    IL_16f1:  stloc.s    V_0
    IL_16f3:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_16f8:  ldstr      bytearray (00 00 02 00 01 00 2B 00 2C 00 9C FF )             // ......+.,...
    IL_16fd:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_1702:  brfalse    IL_200e

    IL_1707:  ldarg.0
    IL_1708:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_170d:  ldloc.s    V_10
    IL_170f:  ldloc.s    V_0
    IL_1711:  sub
    IL_1712:  dup
    IL_1713:  stloc.s    V_7
    IL_1715:  ldc.i4.s   1
    IL_1717:  add
    IL_1718:  stloc.s    V_6
    IL_171a:  ldloc.s    V_6
    IL_171c:  ldc.i4.s   1
    IL_171e:  sub
    IL_171f:  dup
    IL_1720:  stloc.s    V_6
    IL_1722:  ldc.i4.s   0
    IL_1724:  ble.s      IL_1740

    IL_1726:  ldloc.s    V_1
    IL_1728:  ldloc.s    V_0
    IL_172a:  dup
    IL_172b:  ldc.i4.s   1
    IL_172d:  add
    IL_172e:  stloc.s    V_0
    IL_1730:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1735:  ldc.i4.s   36
    IL_1737:  bne.un.s   IL_171a

    IL_1739:  ldloc.s    V_0
    IL_173b:  ldc.i4.s   1
    IL_173d:  sub
    IL_173e:  stloc.s    V_0
    IL_1740:  ldloc.s    V_7
    IL_1742:  ldloc.s    V_6
    IL_1744:  ble.s      IL_1773

    IL_1746:  ldloc.s    V_3
    IL_1748:  ldloc.s    V_2
    IL_174a:  ldc.i4.1
    IL_174b:  sub
    IL_174c:  dup
    IL_174d:  stloc.s    V_2
    IL_174f:  ldloc.s    V_7
    IL_1751:  ldloc.s    V_6
    IL_1753:  sub
    IL_1754:  ldc.i4.s   1
    IL_1756:  sub
    IL_1757:  stelem.i4
    IL_1758:  ldloc.s    V_3
    IL_175a:  ldloc.s    V_2
    IL_175c:  ldc.i4.1
    IL_175d:  sub
    IL_175e:  dup
    IL_175f:  stloc.s    V_2
    IL_1761:  ldloc.s    V_0
    IL_1763:  ldc.i4.s   1
    IL_1765:  sub
    IL_1766:  stelem.i4
    IL_1767:  ldloc.s    V_3
    IL_1769:  ldloc.s    V_2
    IL_176b:  ldc.i4.1
    IL_176c:  sub
    IL_176d:  dup
    IL_176e:  stloc.s    V_2
    IL_1770:  ldc.i4.s   38
    IL_1772:  stelem.i4
    IL_1773:  ldarg.0
    IL_1774:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1779:  ldloc.s    V_5
    IL_177b:  ldloc.s    V_4
    IL_177d:  dup
    IL_177e:  ldc.i4.1
    IL_177f:  add
    IL_1780:  stloc.s    V_4
    IL_1782:  ldelem.i4
    IL_1783:  stloc.s    V_6
    IL_1785:  ldarg.0
    IL_1786:  ldc.i4.s   15
    IL_1788:  ldloc.s    V_6
    IL_178a:  ldloc.s    V_0
    IL_178c:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_1791:  ldloc.s    V_3
    IL_1793:  ldloc.s    V_2
    IL_1795:  ldc.i4.1
    IL_1796:  sub
    IL_1797:  dup
    IL_1798:  stloc.s    V_2
    IL_179a:  ldloc.s    V_6
    IL_179c:  stelem.i4
    IL_179d:  ldloc.s    V_3
    IL_179f:  ldloc.s    V_2
    IL_17a1:  ldc.i4.1
    IL_17a2:  sub
    IL_17a3:  dup
    IL_17a4:  stloc.s    V_2
    IL_17a6:  ldc.i4.s   8
    IL_17a8:  stelem.i4
    IL_17a9:  ldarg.0
    IL_17aa:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_17af:  br         IL_1a4e

    IL_17b4:  ldarg.0
    IL_17b5:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_17ba:  ldloc.s    V_0
    IL_17bc:  ldloc.s    V_10
    IL_17be:  bge        IL_200e

    IL_17c3:  ldloc.s    V_1
    IL_17c5:  ldloc.s    V_0
    IL_17c7:  dup
    IL_17c8:  ldc.i4.s   1
    IL_17ca:  add
    IL_17cb:  stloc.s    V_0
    IL_17cd:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_17d2:  ldc.i4.s   64
    IL_17d4:  bne.un     IL_200e

    IL_17d9:  ldarg.0
    IL_17da:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_17df:  ldloc.s    V_10
    IL_17e1:  ldloc.s    V_0
    IL_17e3:  sub
    IL_17e4:  dup
    IL_17e5:  stloc.s    V_7
    IL_17e7:  ldc.i4.s   1
    IL_17e9:  add
    IL_17ea:  stloc.s    V_6
    IL_17ec:  ldloc.s    V_6
    IL_17ee:  ldc.i4.s   1
    IL_17f0:  sub
    IL_17f1:  dup
    IL_17f2:  stloc.s    V_6
    IL_17f4:  ldc.i4.s   0
    IL_17f6:  ble        IL_1820

    IL_17fb:  ldloc.s    V_1
    IL_17fd:  ldloc.s    V_0
    IL_17ff:  dup
    IL_1800:  ldc.i4.s   1
    IL_1802:  add
    IL_1803:  stloc.s    V_0
    IL_1805:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_180a:  ldstr      bytearray (00 00 04 00 00 00 23 00 24 00 2D 00 2E 00 )       // ......#.$.-...
    IL_180f:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_1814:  brtrue     IL_17ec

    IL_1819:  ldloc.s    V_0
    IL_181b:  ldc.i4.s   1
    IL_181d:  sub
    IL_181e:  stloc.s    V_0
    IL_1820:  ldloc.s    V_7
    IL_1822:  ldloc.s    V_6
    IL_1824:  ble.s      IL_1853

    IL_1826:  ldloc.s    V_3
    IL_1828:  ldloc.s    V_2
    IL_182a:  ldc.i4.1
    IL_182b:  sub
    IL_182c:  dup
    IL_182d:  stloc.s    V_2
    IL_182f:  ldloc.s    V_7
    IL_1831:  ldloc.s    V_6
    IL_1833:  sub
    IL_1834:  ldc.i4.s   1
    IL_1836:  sub
    IL_1837:  stelem.i4
    IL_1838:  ldloc.s    V_3
    IL_183a:  ldloc.s    V_2
    IL_183c:  ldc.i4.1
    IL_183d:  sub
    IL_183e:  dup
    IL_183f:  stloc.s    V_2
    IL_1841:  ldloc.s    V_0
    IL_1843:  ldc.i4.s   1
    IL_1845:  sub
    IL_1846:  stelem.i4
    IL_1847:  ldloc.s    V_3
    IL_1849:  ldloc.s    V_2
    IL_184b:  ldc.i4.1
    IL_184c:  sub
    IL_184d:  dup
    IL_184e:  stloc.s    V_2
    IL_1850:  ldc.i4.s   39
    IL_1852:  stelem.i4
    IL_1853:  ldarg.0
    IL_1854:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1859:  ldloc.s    V_5
    IL_185b:  ldloc.s    V_4
    IL_185d:  ldc.i4.1
    IL_185e:  sub
    IL_185f:  dup
    IL_1860:  stloc.s    V_4
    IL_1862:  ldloc.s    V_0
    IL_1864:  stelem.i4
    IL_1865:  ldloc.s    V_3
    IL_1867:  ldloc.s    V_2
    IL_1869:  ldc.i4.1
    IL_186a:  sub
    IL_186b:  dup
    IL_186c:  stloc.s    V_2
    IL_186e:  ldc.i4.s   1
    IL_1870:  stelem.i4
    IL_1871:  ldarg.0
    IL_1872:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1877:  ldc.i4.s   1
    IL_1879:  ldloc.s    V_10
    IL_187b:  ldloc.s    V_0
    IL_187d:  sub
    IL_187e:  bgt        IL_200e

    IL_1883:  ldloc.s    V_0
    IL_1885:  ldc.i4.s   1
    IL_1887:  add
    IL_1888:  stloc.s    V_0
    IL_188a:  ldc.i4.s   1
    IL_188c:  stloc.s    V_6
    IL_188e:  ldloc.s    V_1
    IL_1890:  ldloc.s    V_0
    IL_1892:  ldloc.s    V_6
    IL_1894:  dup
    IL_1895:  ldc.i4.s   1
    IL_1897:  sub
    IL_1898:  stloc.s    V_6
    IL_189a:  sub
    IL_189b:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_18a0:  ldstr      bytearray (00 00 00 00 0A 00 00 00 02 00 04 00 05 00 03 00 
                                    01 00 06 00 09 00 13 00 00 00 ) 
    IL_18a5:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_18aa:  brfalse    IL_200e

    IL_18af:  ldloc.s    V_6
    IL_18b1:  ldc.i4.s   0
    IL_18b3:  bgt        IL_188e

    IL_18b8:  ldarg.0
    IL_18b9:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_18be:  ldloc.s    V_10
    IL_18c0:  ldloc.s    V_0
    IL_18c2:  sub
    IL_18c3:  dup
    IL_18c4:  stloc.s    V_7
    IL_18c6:  ldc.i4.s   1
    IL_18c8:  add
    IL_18c9:  stloc.s    V_6
    IL_18cb:  ldloc.s    V_6
    IL_18cd:  ldc.i4.s   1
    IL_18cf:  sub
    IL_18d0:  dup
    IL_18d1:  stloc.s    V_6
    IL_18d3:  ldc.i4.s   0
    IL_18d5:  ble        IL_18ff

    IL_18da:  ldloc.s    V_1
    IL_18dc:  ldloc.s    V_0
    IL_18de:  dup
    IL_18df:  ldc.i4.s   1
    IL_18e1:  add
    IL_18e2:  stloc.s    V_0
    IL_18e4:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_18e9:  ldstr      bytearray (00 00 00 00 0A 00 00 00 02 00 04 00 05 00 03 00 
                                    01 00 06 00 09 00 13 00 00 00 ) 
    IL_18ee:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_18f3:  brtrue     IL_18cb

    IL_18f8:  ldloc.s    V_0
    IL_18fa:  ldc.i4.s   1
    IL_18fc:  sub
    IL_18fd:  stloc.s    V_0
    IL_18ff:  ldloc.s    V_7
    IL_1901:  ldloc.s    V_6
    IL_1903:  ble.s      IL_1932

    IL_1905:  ldloc.s    V_3
    IL_1907:  ldloc.s    V_2
    IL_1909:  ldc.i4.1
    IL_190a:  sub
    IL_190b:  dup
    IL_190c:  stloc.s    V_2
    IL_190e:  ldloc.s    V_7
    IL_1910:  ldloc.s    V_6
    IL_1912:  sub
    IL_1913:  ldc.i4.s   1
    IL_1915:  sub
    IL_1916:  stelem.i4
    IL_1917:  ldloc.s    V_3
    IL_1919:  ldloc.s    V_2
    IL_191b:  ldc.i4.1
    IL_191c:  sub
    IL_191d:  dup
    IL_191e:  stloc.s    V_2
    IL_1920:  ldloc.s    V_0
    IL_1922:  ldc.i4.s   1
    IL_1924:  sub
    IL_1925:  stelem.i4
    IL_1926:  ldloc.s    V_3
    IL_1928:  ldloc.s    V_2
    IL_192a:  ldc.i4.1
    IL_192b:  sub
    IL_192c:  dup
    IL_192d:  stloc.s    V_2
    IL_192f:  ldc.i4.s   40
    IL_1931:  stelem.i4
    IL_1932:  ldarg.0
    IL_1933:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1938:  ldloc.s    V_0
    IL_193a:  ldloc.s    V_10
    IL_193c:  bge        IL_200e

    IL_1941:  ldloc.s    V_1
    IL_1943:  ldloc.s    V_0
    IL_1945:  dup
    IL_1946:  ldc.i4.s   1
    IL_1948:  add
    IL_1949:  stloc.s    V_0
    IL_194b:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1950:  ldc.i4.s   46
    IL_1952:  bne.un     IL_200e

    IL_1957:  ldarg.0
    IL_1958:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_195d:  ldloc.s    V_5
    IL_195f:  ldloc.s    V_4
    IL_1961:  dup
    IL_1962:  ldc.i4.1
    IL_1963:  add
    IL_1964:  stloc.s    V_4
    IL_1966:  ldelem.i4
    IL_1967:  stloc.s    V_6
    IL_1969:  ldarg.0
    IL_196a:  ldc.i4.s   16
    IL_196c:  ldloc.s    V_6
    IL_196e:  ldloc.s    V_0
    IL_1970:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_1975:  ldloc.s    V_3
    IL_1977:  ldloc.s    V_2
    IL_1979:  ldc.i4.1
    IL_197a:  sub
    IL_197b:  dup
    IL_197c:  stloc.s    V_2
    IL_197e:  ldloc.s    V_6
    IL_1980:  stelem.i4
    IL_1981:  ldloc.s    V_3
    IL_1983:  ldloc.s    V_2
    IL_1985:  ldc.i4.1
    IL_1986:  sub
    IL_1987:  dup
    IL_1988:  stloc.s    V_2
    IL_198a:  ldc.i4.s   8
    IL_198c:  stelem.i4
    IL_198d:  ldarg.0
    IL_198e:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1993:  ldc.i4.s   1
    IL_1995:  ldloc.s    V_10
    IL_1997:  ldloc.s    V_0
    IL_1999:  sub
    IL_199a:  bgt        IL_200e

    IL_199f:  ldloc.s    V_0
    IL_19a1:  ldc.i4.s   1
    IL_19a3:  add
    IL_19a4:  stloc.s    V_0
    IL_19a6:  ldc.i4.s   1
    IL_19a8:  stloc.s    V_6
    IL_19aa:  ldloc.s    V_1
    IL_19ac:  ldloc.s    V_0
    IL_19ae:  ldloc.s    V_6
    IL_19b0:  dup
    IL_19b1:  ldc.i4.s   1
    IL_19b3:  sub
    IL_19b4:  stloc.s    V_6
    IL_19b6:  sub
    IL_19b7:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_19bc:  ldstr      bytearray (00 00 04 00 0A 00 2B 00 2C 00 2E 00 2F 00 00 00   // ......+.,.../...
                                    02 00 04 00 05 00 03 00 01 00 06 00 09 00 13 00 
                                    00 00 ) 
    IL_19c1:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_19c6:  brfalse    IL_200e

    IL_19cb:  ldloc.s    V_6
    IL_19cd:  ldc.i4.s   0
    IL_19cf:  bgt        IL_19aa

    IL_19d4:  ldarg.0
    IL_19d5:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_19da:  ldloc.s    V_10
    IL_19dc:  ldloc.s    V_0
    IL_19de:  sub
    IL_19df:  dup
    IL_19e0:  stloc.s    V_7
    IL_19e2:  ldc.i4.s   1
    IL_19e4:  add
    IL_19e5:  stloc.s    V_6
    IL_19e7:  ldloc.s    V_6
    IL_19e9:  ldc.i4.s   1
    IL_19eb:  sub
    IL_19ec:  dup
    IL_19ed:  stloc.s    V_6
    IL_19ef:  ldc.i4.s   0
    IL_19f1:  ble        IL_1a1b

    IL_19f6:  ldloc.s    V_1
    IL_19f8:  ldloc.s    V_0
    IL_19fa:  dup
    IL_19fb:  ldc.i4.s   1
    IL_19fd:  add
    IL_19fe:  stloc.s    V_0
    IL_1a00:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1a05:  ldstr      bytearray (00 00 04 00 0A 00 2B 00 2C 00 2E 00 2F 00 00 00   // ......+.,.../...
                                    02 00 04 00 05 00 03 00 01 00 06 00 09 00 13 00 
                                    00 00 ) 
    IL_1a0a:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_1a0f:  brtrue     IL_19e7

    IL_1a14:  ldloc.s    V_0
    IL_1a16:  ldc.i4.s   1
    IL_1a18:  sub
    IL_1a19:  stloc.s    V_0
    IL_1a1b:  ldloc.s    V_7
    IL_1a1d:  ldloc.s    V_6
    IL_1a1f:  ble.s      IL_1a4e

    IL_1a21:  ldloc.s    V_3
    IL_1a23:  ldloc.s    V_2
    IL_1a25:  ldc.i4.1
    IL_1a26:  sub
    IL_1a27:  dup
    IL_1a28:  stloc.s    V_2
    IL_1a2a:  ldloc.s    V_7
    IL_1a2c:  ldloc.s    V_6
    IL_1a2e:  sub
    IL_1a2f:  ldc.i4.s   1
    IL_1a31:  sub
    IL_1a32:  stelem.i4
    IL_1a33:  ldloc.s    V_3
    IL_1a35:  ldloc.s    V_2
    IL_1a37:  ldc.i4.1
    IL_1a38:  sub
    IL_1a39:  dup
    IL_1a3a:  stloc.s    V_2
    IL_1a3c:  ldloc.s    V_0
    IL_1a3e:  ldc.i4.s   1
    IL_1a40:  sub
    IL_1a41:  stelem.i4
    IL_1a42:  ldloc.s    V_3
    IL_1a44:  ldloc.s    V_2
    IL_1a46:  ldc.i4.1
    IL_1a47:  sub
    IL_1a48:  dup
    IL_1a49:  stloc.s    V_2
    IL_1a4b:  ldc.i4.s   41
    IL_1a4d:  stelem.i4
    IL_1a4e:  ldarg.0
    IL_1a4f:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1a54:  ldloc.s    V_5
    IL_1a56:  ldloc.s    V_4
    IL_1a58:  dup
    IL_1a59:  ldc.i4.1
    IL_1a5a:  add
    IL_1a5b:  stloc.s    V_4
    IL_1a5d:  ldelem.i4
    IL_1a5e:  stloc.s    V_6
    IL_1a60:  ldarg.0
    IL_1a61:  ldc.i4.s   13
    IL_1a63:  ldloc.s    V_6
    IL_1a65:  ldloc.s    V_0
    IL_1a67:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_1a6c:  ldloc.s    V_3
    IL_1a6e:  ldloc.s    V_2
    IL_1a70:  ldc.i4.1
    IL_1a71:  sub
    IL_1a72:  dup
    IL_1a73:  stloc.s    V_2
    IL_1a75:  ldloc.s    V_6
    IL_1a77:  stelem.i4
    IL_1a78:  ldloc.s    V_3
    IL_1a7a:  ldloc.s    V_2
    IL_1a7c:  ldc.i4.1
    IL_1a7d:  sub
    IL_1a7e:  dup
    IL_1a7f:  stloc.s    V_2
    IL_1a81:  ldc.i4.s   8
    IL_1a83:  stelem.i4
    IL_1a84:  ldarg.0
    IL_1a85:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1a8a:  ldloc.s    V_5
    IL_1a8c:  ldloc.s    V_4
    IL_1a8e:  dup
    IL_1a8f:  ldc.i4.1
    IL_1a90:  add
    IL_1a91:  stloc.s    V_4
    IL_1a93:  ldelem.i4
    IL_1a94:  stloc.s    V_6
    IL_1a96:  ldloc.s    V_5
    IL_1a98:  ldloc.s    V_4
    IL_1a9a:  dup
    IL_1a9b:  ldc.i4.1
    IL_1a9c:  add
    IL_1a9d:  stloc.s    V_4
    IL_1a9f:  ldelem.i4
    IL_1aa0:  dup
    IL_1aa1:  stloc.s    V_7
    IL_1aa3:  ldloc.s    V_3
    IL_1aa5:  ldloc.s    V_2
    IL_1aa7:  ldc.i4.1
    IL_1aa8:  sub
    IL_1aa9:  dup
    IL_1aaa:  stloc.s    V_2
    IL_1aac:  ldloc.s    V_7
    IL_1aae:  stelem.i4
    IL_1aaf:  ldloc.s    V_0
    IL_1ab1:  bne.un.s   IL_1ab9

    IL_1ab3:  ldloc.s    V_6
    IL_1ab5:  ldc.i4.s   0
    IL_1ab7:  bge.s      IL_1b0c

    IL_1ab9:  ldloc.s    V_6
    IL_1abb:  ldc.i4.s   1
    IL_1abd:  bge.s      IL_1b0c

    IL_1abf:  ldloc.s    V_5
    IL_1ac1:  ldloc.s    V_4
    IL_1ac3:  ldc.i4.1
    IL_1ac4:  sub
    IL_1ac5:  dup
    IL_1ac6:  stloc.s    V_4
    IL_1ac8:  ldloc.s    V_0
    IL_1aca:  stelem.i4
    IL_1acb:  ldloc.s    V_5
    IL_1acd:  ldloc.s    V_4
    IL_1acf:  ldc.i4.1
    IL_1ad0:  sub
    IL_1ad1:  dup
    IL_1ad2:  stloc.s    V_4
    IL_1ad4:  ldloc.s    V_6
    IL_1ad6:  ldc.i4.s   1
    IL_1ad8:  add
    IL_1ad9:  stelem.i4
    IL_1ada:  ldloc.s    V_3
    IL_1adc:  ldloc.s    V_2
    IL_1ade:  ldc.i4.1
    IL_1adf:  sub
    IL_1ae0:  dup
    IL_1ae1:  stloc.s    V_2
    IL_1ae3:  ldc.i4.s   42
    IL_1ae5:  stelem.i4
    IL_1ae6:  ldloc.s    V_2
    IL_1ae8:  ldc.i4     0x188
    IL_1aed:  ble.s      IL_1afb

    IL_1aef:  ldloc.s    V_4
    IL_1af1:  ldc.i4     0x126
    IL_1af6:  bgt        IL_14cb

    IL_1afb:  ldloc.s    V_3
    IL_1afd:  ldloc.s    V_2
    IL_1aff:  ldc.i4.1
    IL_1b00:  sub
    IL_1b01:  dup
    IL_1b02:  stloc.s    V_2
    IL_1b04:  ldc.i4.s   43
    IL_1b06:  stelem.i4
    IL_1b07:  br         IL_200e

    IL_1b0c:  ldloc.s    V_3
    IL_1b0e:  ldloc.s    V_2
    IL_1b10:  ldc.i4.1
    IL_1b11:  sub
    IL_1b12:  dup
    IL_1b13:  stloc.s    V_2
    IL_1b15:  ldloc.s    V_6
    IL_1b17:  stelem.i4
    IL_1b18:  ldloc.s    V_3
    IL_1b1a:  ldloc.s    V_2
    IL_1b1c:  ldc.i4.1
    IL_1b1d:  sub
    IL_1b1e:  dup
    IL_1b1f:  stloc.s    V_2
    IL_1b21:  ldc.i4.s   11
    IL_1b23:  stelem.i4
    IL_1b24:  ldarg.0
    IL_1b25:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1b2a:  ldloc.s    V_5
    IL_1b2c:  ldloc.s    V_4
    IL_1b2e:  dup
    IL_1b2f:  ldc.i4.1
    IL_1b30:  add
    IL_1b31:  stloc.s    V_4
    IL_1b33:  ldelem.i4
    IL_1b34:  stloc.s    V_6
    IL_1b36:  ldarg.0
    IL_1b37:  ldc.i4.s   11
    IL_1b39:  ldloc.s    V_6
    IL_1b3b:  ldloc.s    V_0
    IL_1b3d:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_1b42:  ldloc.s    V_3
    IL_1b44:  ldloc.s    V_2
    IL_1b46:  ldc.i4.1
    IL_1b47:  sub
    IL_1b48:  dup
    IL_1b49:  stloc.s    V_2
    IL_1b4b:  ldloc.s    V_6
    IL_1b4d:  stelem.i4
    IL_1b4e:  ldloc.s    V_3
    IL_1b50:  ldloc.s    V_2
    IL_1b52:  ldc.i4.1
    IL_1b53:  sub
    IL_1b54:  dup
    IL_1b55:  stloc.s    V_2
    IL_1b57:  ldc.i4.s   8
    IL_1b59:  stelem.i4
    IL_1b5a:  ldarg.0
    IL_1b5b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1b60:  ldloc.s    V_5
    IL_1b62:  ldloc.s    V_4
    IL_1b64:  dup
    IL_1b65:  ldc.i4.1
    IL_1b66:  add
    IL_1b67:  stloc.s    V_4
    IL_1b69:  ldelem.i4
    IL_1b6a:  dup
    IL_1b6b:  stloc.s    V_6
    IL_1b6d:  ldloc.s    V_3
    IL_1b6f:  ldloc.s    V_2
    IL_1b71:  ldc.i4.1
    IL_1b72:  sub
    IL_1b73:  dup
    IL_1b74:  stloc.s    V_2
    IL_1b76:  ldloc.s    V_6
    IL_1b78:  stelem.i4
    IL_1b79:  ldloc.s    V_0
    IL_1b7b:  beq.s      IL_1bc7

    IL_1b7d:  ldloc.s    V_3
    IL_1b7f:  ldloc.s    V_2
    IL_1b81:  ldc.i4.1
    IL_1b82:  sub
    IL_1b83:  dup
    IL_1b84:  stloc.s    V_2
    IL_1b86:  ldloc.s    V_0
    IL_1b88:  stelem.i4
    IL_1b89:  ldloc.s    V_5
    IL_1b8b:  ldloc.s    V_4
    IL_1b8d:  ldc.i4.1
    IL_1b8e:  sub
    IL_1b8f:  dup
    IL_1b90:  stloc.s    V_4
    IL_1b92:  ldloc.s    V_0
    IL_1b94:  stelem.i4
    IL_1b95:  ldloc.s    V_3
    IL_1b97:  ldloc.s    V_2
    IL_1b99:  ldc.i4.1
    IL_1b9a:  sub
    IL_1b9b:  dup
    IL_1b9c:  stloc.s    V_2
    IL_1b9e:  ldc.i4.s   44
    IL_1ba0:  stelem.i4
    IL_1ba1:  ldloc.s    V_2
    IL_1ba3:  ldc.i4     0x188
    IL_1ba8:  ble.s      IL_1bb6

    IL_1baa:  ldloc.s    V_4
    IL_1bac:  ldc.i4     0x126
    IL_1bb1:  bgt        IL_132a

    IL_1bb6:  ldloc.s    V_3
    IL_1bb8:  ldloc.s    V_2
    IL_1bba:  ldc.i4.1
    IL_1bbb:  sub
    IL_1bbc:  dup
    IL_1bbd:  stloc.s    V_2
    IL_1bbf:  ldc.i4.s   45
    IL_1bc1:  stelem.i4
    IL_1bc2:  br         IL_200e

    IL_1bc7:  ldloc.s    V_3
    IL_1bc9:  ldloc.s    V_2
    IL_1bcb:  ldc.i4.1
    IL_1bcc:  sub
    IL_1bcd:  dup
    IL_1bce:  stloc.s    V_2
    IL_1bd0:  ldc.i4.s   46
    IL_1bd2:  stelem.i4
    IL_1bd3:  ldarg.0
    IL_1bd4:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1bd9:  ldloc.s    V_5
    IL_1bdb:  ldloc.s    V_4
    IL_1bdd:  dup
    IL_1bde:  ldc.i4.1
    IL_1bdf:  add
    IL_1be0:  stloc.s    V_4
    IL_1be2:  ldelem.i4
    IL_1be3:  stloc.s    V_6
    IL_1be5:  ldarg.0
    IL_1be6:  ldc.i4.s   10
    IL_1be8:  ldloc.s    V_6
    IL_1bea:  ldloc.s    V_0
    IL_1bec:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_1bf1:  ldloc.s    V_3
    IL_1bf3:  ldloc.s    V_2
    IL_1bf5:  ldc.i4.1
    IL_1bf6:  sub
    IL_1bf7:  dup
    IL_1bf8:  stloc.s    V_2
    IL_1bfa:  ldloc.s    V_6
    IL_1bfc:  stelem.i4
    IL_1bfd:  ldloc.s    V_3
    IL_1bff:  ldloc.s    V_2
    IL_1c01:  ldc.i4.1
    IL_1c02:  sub
    IL_1c03:  dup
    IL_1c04:  stloc.s    V_2
    IL_1c06:  ldc.i4.s   8
    IL_1c08:  stelem.i4
    IL_1c09:  ldarg.0
    IL_1c0a:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1c0f:  ldloc.s    V_5
    IL_1c11:  ldloc.s    V_4
    IL_1c13:  dup
    IL_1c14:  ldc.i4.1
    IL_1c15:  add
    IL_1c16:  stloc.s    V_4
    IL_1c18:  ldelem.i4
    IL_1c19:  stloc.s    V_6
    IL_1c1b:  ldloc.s    V_5
    IL_1c1d:  ldloc.s    V_4
    IL_1c1f:  dup
    IL_1c20:  ldc.i4.1
    IL_1c21:  add
    IL_1c22:  stloc.s    V_4
    IL_1c24:  ldelem.i4
    IL_1c25:  dup
    IL_1c26:  stloc.s    V_7
    IL_1c28:  ldloc.s    V_3
    IL_1c2a:  ldloc.s    V_2
    IL_1c2c:  ldc.i4.1
    IL_1c2d:  sub
    IL_1c2e:  dup
    IL_1c2f:  stloc.s    V_2
    IL_1c31:  ldloc.s    V_7
    IL_1c33:  stelem.i4
    IL_1c34:  ldloc.s    V_0
    IL_1c36:  bne.un.s   IL_1c3e

    IL_1c38:  ldloc.s    V_6
    IL_1c3a:  ldc.i4.s   0
    IL_1c3c:  bge.s      IL_1c91

    IL_1c3e:  ldloc.s    V_6
    IL_1c40:  ldc.i4.s   1
    IL_1c42:  bge.s      IL_1c91

    IL_1c44:  ldloc.s    V_5
    IL_1c46:  ldloc.s    V_4
    IL_1c48:  ldc.i4.1
    IL_1c49:  sub
    IL_1c4a:  dup
    IL_1c4b:  stloc.s    V_4
    IL_1c4d:  ldloc.s    V_0
    IL_1c4f:  stelem.i4
    IL_1c50:  ldloc.s    V_5
    IL_1c52:  ldloc.s    V_4
    IL_1c54:  ldc.i4.1
    IL_1c55:  sub
    IL_1c56:  dup
    IL_1c57:  stloc.s    V_4
    IL_1c59:  ldloc.s    V_6
    IL_1c5b:  ldc.i4.s   1
    IL_1c5d:  add
    IL_1c5e:  stelem.i4
    IL_1c5f:  ldloc.s    V_3
    IL_1c61:  ldloc.s    V_2
    IL_1c63:  ldc.i4.1
    IL_1c64:  sub
    IL_1c65:  dup
    IL_1c66:  stloc.s    V_2
    IL_1c68:  ldc.i4.s   47
    IL_1c6a:  stelem.i4
    IL_1c6b:  ldloc.s    V_2
    IL_1c6d:  ldc.i4     0x188
    IL_1c72:  ble.s      IL_1c80

    IL_1c74:  ldloc.s    V_4
    IL_1c76:  ldc.i4     0x126
    IL_1c7b:  bgt        IL_12be

    IL_1c80:  ldloc.s    V_3
    IL_1c82:  ldloc.s    V_2
    IL_1c84:  ldc.i4.1
    IL_1c85:  sub
    IL_1c86:  dup
    IL_1c87:  stloc.s    V_2
    IL_1c89:  ldc.i4.s   48
    IL_1c8b:  stelem.i4
    IL_1c8c:  br         IL_200e

    IL_1c91:  ldloc.s    V_3
    IL_1c93:  ldloc.s    V_2
    IL_1c95:  ldc.i4.1
    IL_1c96:  sub
    IL_1c97:  dup
    IL_1c98:  stloc.s    V_2
    IL_1c9a:  ldloc.s    V_6
    IL_1c9c:  stelem.i4
    IL_1c9d:  ldloc.s    V_3
    IL_1c9f:  ldloc.s    V_2
    IL_1ca1:  ldc.i4.1
    IL_1ca2:  sub
    IL_1ca3:  dup
    IL_1ca4:  stloc.s    V_2
    IL_1ca6:  ldc.i4.s   11
    IL_1ca8:  stelem.i4
    IL_1ca9:  ldarg.0
    IL_1caa:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1caf:  ldloc.s    V_0
    IL_1cb1:  ldloc.s    V_10
    IL_1cb3:  bge        IL_200e

    IL_1cb8:  ldloc.s    V_1
    IL_1cba:  ldloc.s    V_0
    IL_1cbc:  dup
    IL_1cbd:  ldc.i4.s   1
    IL_1cbf:  add
    IL_1cc0:  stloc.s    V_0
    IL_1cc2:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1cc7:  ldc.i4.s   36
    IL_1cc9:  bne.un     IL_200e

    IL_1cce:  ldarg.0
    IL_1ccf:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1cd4:  ldloc.s    V_5
    IL_1cd6:  ldloc.s    V_4
    IL_1cd8:  dup
    IL_1cd9:  ldc.i4.1
    IL_1cda:  add
    IL_1cdb:  stloc.s    V_4
    IL_1cdd:  ldelem.i4
    IL_1cde:  stloc.s    V_6
    IL_1ce0:  ldarg.0
    IL_1ce1:  ldc.i4.s   9
    IL_1ce3:  ldloc.s    V_6
    IL_1ce5:  ldloc.s    V_0
    IL_1ce7:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_1cec:  ldloc.s    V_3
    IL_1cee:  ldloc.s    V_2
    IL_1cf0:  ldc.i4.1
    IL_1cf1:  sub
    IL_1cf2:  dup
    IL_1cf3:  stloc.s    V_2
    IL_1cf5:  ldloc.s    V_6
    IL_1cf7:  stelem.i4
    IL_1cf8:  ldloc.s    V_3
    IL_1cfa:  ldloc.s    V_2
    IL_1cfc:  ldc.i4.1
    IL_1cfd:  sub
    IL_1cfe:  dup
    IL_1cff:  stloc.s    V_2
    IL_1d01:  ldc.i4.s   8
    IL_1d03:  stelem.i4
    IL_1d04:  ldarg.0
    IL_1d05:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1d0a:  ldloc.s    V_5
    IL_1d0c:  ldloc.s    V_4
    IL_1d0e:  dup
    IL_1d0f:  ldc.i4.1
    IL_1d10:  add
    IL_1d11:  stloc.s    V_4
    IL_1d13:  ldelem.i4
    IL_1d14:  stloc.s    V_6
    IL_1d16:  ldarg.0
    IL_1d17:  ldc.i4.s   1
    IL_1d19:  ldloc.s    V_6
    IL_1d1b:  ldloc.s    V_0
    IL_1d1d:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_1d22:  ldloc.s    V_3
    IL_1d24:  ldloc.s    V_2
    IL_1d26:  ldc.i4.1
    IL_1d27:  sub
    IL_1d28:  dup
    IL_1d29:  stloc.s    V_2
    IL_1d2b:  ldloc.s    V_6
    IL_1d2d:  stelem.i4
    IL_1d2e:  ldloc.s    V_3
    IL_1d30:  ldloc.s    V_2
    IL_1d32:  ldc.i4.1
    IL_1d33:  sub
    IL_1d34:  dup
    IL_1d35:  stloc.s    V_2
    IL_1d37:  ldc.i4.s   8
    IL_1d39:  stelem.i4
    IL_1d3a:  ldarg.0
    IL_1d3b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1d40:  br         IL_1fc9

    IL_1d45:  ldarg.0
    IL_1d46:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1d4b:  ldloc.s    V_3
    IL_1d4d:  ldloc.s    V_2
    IL_1d4f:  ldc.i4.1
    IL_1d50:  sub
    IL_1d51:  dup
    IL_1d52:  stloc.s    V_2
    IL_1d54:  ldloc.s    V_0
    IL_1d56:  stelem.i4
    IL_1d57:  ldloc.s    V_3
    IL_1d59:  ldloc.s    V_2
    IL_1d5b:  ldc.i4.1
    IL_1d5c:  sub
    IL_1d5d:  dup
    IL_1d5e:  stloc.s    V_2
    IL_1d60:  ldc.i4.s   49
    IL_1d62:  stelem.i4
    IL_1d63:  ldarg.0
    IL_1d64:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1d69:  ldc.i4.s   2
    IL_1d6b:  ldloc.s    V_10
    IL_1d6d:  ldloc.s    V_0
    IL_1d6f:  sub
    IL_1d70:  bgt        IL_200e

    IL_1d75:  ldloc.s    V_1
    IL_1d77:  ldloc.s    V_0
    IL_1d79:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1d7e:  ldc.i4.s   125
    IL_1d80:  bne.un     IL_200e

    IL_1d85:  ldloc.s    V_1
    IL_1d87:  ldloc.s    V_0
    IL_1d89:  ldc.i4.s   1
    IL_1d8b:  add
    IL_1d8c:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1d91:  ldc.i4.s   58
    IL_1d93:  bne.un     IL_200e

    IL_1d98:  ldloc.s    V_0
    IL_1d9a:  ldc.i4.s   2
    IL_1d9c:  add
    IL_1d9d:  stloc.s    V_0
    IL_1d9f:  ldarg.0
    IL_1da0:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1da5:  ldc.i4.s   1
    IL_1da7:  ldloc.s    V_10
    IL_1da9:  ldloc.s    V_0
    IL_1dab:  sub
    IL_1dac:  bgt        IL_200e

    IL_1db1:  ldloc.s    V_0
    IL_1db3:  ldc.i4.s   1
    IL_1db5:  add
    IL_1db6:  stloc.s    V_0
    IL_1db8:  ldc.i4.s   1
    IL_1dba:  stloc.s    V_6
    IL_1dbc:  ldloc.s    V_1
    IL_1dbe:  ldloc.s    V_0
    IL_1dc0:  ldloc.s    V_6
    IL_1dc2:  dup
    IL_1dc3:  ldc.i4.s   1
    IL_1dc5:  sub
    IL_1dc6:  stloc.s    V_6
    IL_1dc8:  sub
    IL_1dc9:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1dce:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_1dd3:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_1dd8:  brfalse    IL_200e

    IL_1ddd:  ldloc.s    V_6
    IL_1ddf:  ldc.i4.s   0
    IL_1de1:  bgt        IL_1dbc

    IL_1de6:  ldarg.0
    IL_1de7:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1dec:  ldloc.s    V_10
    IL_1dee:  ldloc.s    V_0
    IL_1df0:  sub
    IL_1df1:  dup
    IL_1df2:  stloc.s    V_7
    IL_1df4:  ldc.i4.s   1
    IL_1df6:  add
    IL_1df7:  stloc.s    V_6
    IL_1df9:  ldloc.s    V_6
    IL_1dfb:  ldc.i4.s   1
    IL_1dfd:  sub
    IL_1dfe:  dup
    IL_1dff:  stloc.s    V_6
    IL_1e01:  ldc.i4.s   0
    IL_1e03:  ble        IL_1e2d

    IL_1e08:  ldloc.s    V_1
    IL_1e0a:  ldloc.s    V_0
    IL_1e0c:  dup
    IL_1e0d:  ldc.i4.s   1
    IL_1e0f:  add
    IL_1e10:  stloc.s    V_0
    IL_1e12:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1e17:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_1e1c:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_1e21:  brtrue     IL_1df9

    IL_1e26:  ldloc.s    V_0
    IL_1e28:  ldc.i4.s   1
    IL_1e2a:  sub
    IL_1e2b:  stloc.s    V_0
    IL_1e2d:  ldloc.s    V_7
    IL_1e2f:  ldloc.s    V_6
    IL_1e31:  ble.s      IL_1e60

    IL_1e33:  ldloc.s    V_3
    IL_1e35:  ldloc.s    V_2
    IL_1e37:  ldc.i4.1
    IL_1e38:  sub
    IL_1e39:  dup
    IL_1e3a:  stloc.s    V_2
    IL_1e3c:  ldloc.s    V_7
    IL_1e3e:  ldloc.s    V_6
    IL_1e40:  sub
    IL_1e41:  ldc.i4.s   1
    IL_1e43:  sub
    IL_1e44:  stelem.i4
    IL_1e45:  ldloc.s    V_3
    IL_1e47:  ldloc.s    V_2
    IL_1e49:  ldc.i4.1
    IL_1e4a:  sub
    IL_1e4b:  dup
    IL_1e4c:  stloc.s    V_2
    IL_1e4e:  ldloc.s    V_0
    IL_1e50:  ldc.i4.s   1
    IL_1e52:  sub
    IL_1e53:  stelem.i4
    IL_1e54:  ldloc.s    V_3
    IL_1e56:  ldloc.s    V_2
    IL_1e58:  ldc.i4.1
    IL_1e59:  sub
    IL_1e5a:  dup
    IL_1e5b:  stloc.s    V_2
    IL_1e5d:  ldc.i4.s   50
    IL_1e5f:  stelem.i4
    IL_1e60:  ldarg.0
    IL_1e61:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1e66:  ldc.i4.s   2
    IL_1e68:  ldloc.s    V_10
    IL_1e6a:  ldloc.s    V_0
    IL_1e6c:  sub
    IL_1e6d:  bgt        IL_200e

    IL_1e72:  ldloc.s    V_1
    IL_1e74:  ldloc.s    V_0
    IL_1e76:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1e7b:  ldc.i4.s   58
    IL_1e7d:  bne.un     IL_200e

    IL_1e82:  ldloc.s    V_1
    IL_1e84:  ldloc.s    V_0
    IL_1e86:  ldc.i4.s   1
    IL_1e88:  add
    IL_1e89:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1e8e:  ldc.i4.s   123
    IL_1e90:  bne.un     IL_200e

    IL_1e95:  ldloc.s    V_0
    IL_1e97:  ldc.i4.s   2
    IL_1e99:  add
    IL_1e9a:  stloc.s    V_0
    IL_1e9c:  ldarg.0
    IL_1e9d:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1ea2:  br         IL_1fc9

    IL_1ea7:  ldarg.0
    IL_1ea8:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1ead:  ldc.i4.s   2
    IL_1eaf:  ldloc.s    V_10
    IL_1eb1:  ldloc.s    V_0
    IL_1eb3:  sub
    IL_1eb4:  bgt        IL_200e

    IL_1eb9:  ldloc.s    V_1
    IL_1ebb:  ldloc.s    V_0
    IL_1ebd:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1ec2:  ldc.i4.s   125
    IL_1ec4:  bne.un     IL_200e

    IL_1ec9:  ldloc.s    V_1
    IL_1ecb:  ldloc.s    V_0
    IL_1ecd:  ldc.i4.s   1
    IL_1ecf:  add
    IL_1ed0:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1ed5:  ldc.i4.s   58
    IL_1ed7:  bne.un     IL_200e

    IL_1edc:  ldloc.s    V_0
    IL_1ede:  ldc.i4.s   2
    IL_1ee0:  add
    IL_1ee1:  stloc.s    V_0
    IL_1ee3:  ldarg.0
    IL_1ee4:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1ee9:  ldc.i4.s   1
    IL_1eeb:  ldloc.s    V_10
    IL_1eed:  ldloc.s    V_0
    IL_1eef:  sub
    IL_1ef0:  bgt        IL_200e

    IL_1ef5:  ldloc.s    V_0
    IL_1ef7:  ldc.i4.s   1
    IL_1ef9:  add
    IL_1efa:  stloc.s    V_0
    IL_1efc:  ldc.i4.s   1
    IL_1efe:  stloc.s    V_6
    IL_1f00:  ldloc.s    V_1
    IL_1f02:  ldloc.s    V_0
    IL_1f04:  ldloc.s    V_6
    IL_1f06:  dup
    IL_1f07:  ldc.i4.s   1
    IL_1f09:  sub
    IL_1f0a:  stloc.s    V_6
    IL_1f0c:  sub
    IL_1f0d:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1f12:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_1f17:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_1f1c:  brfalse    IL_200e

    IL_1f21:  ldloc.s    V_6
    IL_1f23:  ldc.i4.s   0
    IL_1f25:  bgt        IL_1f00

    IL_1f2a:  ldarg.0
    IL_1f2b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1f30:  ldloc.s    V_10
    IL_1f32:  ldloc.s    V_0
    IL_1f34:  sub
    IL_1f35:  dup
    IL_1f36:  stloc.s    V_7
    IL_1f38:  ldc.i4.s   1
    IL_1f3a:  add
    IL_1f3b:  stloc.s    V_6
    IL_1f3d:  ldloc.s    V_6
    IL_1f3f:  ldc.i4.s   1
    IL_1f41:  sub
    IL_1f42:  dup
    IL_1f43:  stloc.s    V_6
    IL_1f45:  ldc.i4.s   0
    IL_1f47:  ble        IL_1f71

    IL_1f4c:  ldloc.s    V_1
    IL_1f4e:  ldloc.s    V_0
    IL_1f50:  dup
    IL_1f51:  ldc.i4.s   1
    IL_1f53:  add
    IL_1f54:  stloc.s    V_0
    IL_1f56:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1f5b:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_1f60:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_1f65:  brtrue     IL_1f3d

    IL_1f6a:  ldloc.s    V_0
    IL_1f6c:  ldc.i4.s   1
    IL_1f6e:  sub
    IL_1f6f:  stloc.s    V_0
    IL_1f71:  ldloc.s    V_7
    IL_1f73:  ldloc.s    V_6
    IL_1f75:  ble.s      IL_1fa4

    IL_1f77:  ldloc.s    V_3
    IL_1f79:  ldloc.s    V_2
    IL_1f7b:  ldc.i4.1
    IL_1f7c:  sub
    IL_1f7d:  dup
    IL_1f7e:  stloc.s    V_2
    IL_1f80:  ldloc.s    V_7
    IL_1f82:  ldloc.s    V_6
    IL_1f84:  sub
    IL_1f85:  ldc.i4.s   1
    IL_1f87:  sub
    IL_1f88:  stelem.i4
    IL_1f89:  ldloc.s    V_3
    IL_1f8b:  ldloc.s    V_2
    IL_1f8d:  ldc.i4.1
    IL_1f8e:  sub
    IL_1f8f:  dup
    IL_1f90:  stloc.s    V_2
    IL_1f92:  ldloc.s    V_0
    IL_1f94:  ldc.i4.s   1
    IL_1f96:  sub
    IL_1f97:  stelem.i4
    IL_1f98:  ldloc.s    V_3
    IL_1f9a:  ldloc.s    V_2
    IL_1f9c:  ldc.i4.1
    IL_1f9d:  sub
    IL_1f9e:  dup
    IL_1f9f:  stloc.s    V_2
    IL_1fa1:  ldc.i4.s   51
    IL_1fa3:  stelem.i4
    IL_1fa4:  ldarg.0
    IL_1fa5:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1faa:  ldloc.s    V_0
    IL_1fac:  ldloc.s    V_10
    IL_1fae:  bge        IL_200e

    IL_1fb3:  ldloc.s    V_1
    IL_1fb5:  ldloc.s    V_0
    IL_1fb7:  dup
    IL_1fb8:  ldc.i4.s   1
    IL_1fba:  add
    IL_1fbb:  stloc.s    V_0
    IL_1fbd:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1fc2:  ldc.i4.s   36
    IL_1fc4:  bne.un     IL_200e

    IL_1fc9:  ldarg.0
    IL_1fca:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1fcf:  ldloc.s    V_5
    IL_1fd1:  ldloc.s    V_4
    IL_1fd3:  dup
    IL_1fd4:  ldc.i4.1
    IL_1fd5:  add
    IL_1fd6:  stloc.s    V_4
    IL_1fd8:  ldelem.i4
    IL_1fd9:  stloc.s    V_6
    IL_1fdb:  ldarg.0
    IL_1fdc:  ldc.i4.s   0
    IL_1fde:  ldloc.s    V_6
    IL_1fe0:  ldloc.s    V_0
    IL_1fe2:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_1fe7:  ldloc.s    V_3
    IL_1fe9:  ldloc.s    V_2
    IL_1feb:  ldc.i4.1
    IL_1fec:  sub
    IL_1fed:  dup
    IL_1fee:  stloc.s    V_2
    IL_1ff0:  ldloc.s    V_6
    IL_1ff2:  stelem.i4
    IL_1ff3:  ldloc.s    V_3
    IL_1ff5:  ldloc.s    V_2
    IL_1ff7:  ldc.i4.1
    IL_1ff8:  sub
    IL_1ff9:  dup
    IL_1ffa:  stloc.s    V_2
    IL_1ffc:  ldc.i4.s   8
    IL_1ffe:  stelem.i4
    IL_1fff:  ldarg.0
    IL_2000:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2005:  ldarg.0
    IL_2006:  ldloc.s    V_0
    IL_2008:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextpos
    IL_200d:  ret

    IL_200e:  ldarg.0
    IL_200f:  ldloc.s    V_2
    IL_2011:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtrackpos
    IL_2016:  ldarg.0
    IL_2017:  ldloc.s    V_4
    IL_2019:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runstackpos
    IL_201e:  ldarg.0
    IL_201f:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::EnsureStorage()
    IL_2024:  ldarg.0
    IL_2025:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtrackpos
    IL_202a:  stloc.s    V_2
    IL_202c:  ldarg.0
    IL_202d:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runstackpos
    IL_2032:  stloc.s    V_4
    IL_2034:  ldarg.0
    IL_2035:  ldfld      int32[] [System]System.Text.RegularExpressions.RegexRunner::runtrack
    IL_203a:  stloc.s    V_3
    IL_203c:  ldarg.0
    IL_203d:  ldfld      int32[] [System]System.Text.RegularExpressions.RegexRunner::runstack
    IL_2042:  stloc.s    V_5
    IL_2044:  ldloc.s    V_3
    IL_2046:  ldloc.s    V_2
    IL_2048:  dup
    IL_2049:  ldc.i4.1
    IL_204a:  add
    IL_204b:  stloc.s    V_2
    IL_204d:  ldelem.i4
    IL_204e:  switch     ( 
                          IL_2123,
                          IL_213a,
                          IL_214c,
                          IL_2163,
                          IL_217a,
                          IL_2191,
                          IL_21a3,
                          IL_21ba,
                          IL_2210,
                          IL_2235,
                          IL_0174,
                          IL_2298,
                          IL_22cf,
                          IL_2325,
                          IL_237b,
                          IL_23d1,
                          IL_2427,
                          IL_243e,
                          IL_2494,
                          IL_24ea,
                          IL_0a47,
                          IL_254d,
                          IL_090e,
                          IL_25b0,
                          IL_2606,
                          IL_0c6b,
                          IL_2669,
                          IL_2680,
                          IL_26d6,
                          IL_26ed,
                          IL_2743,
                          IL_2799,
                          IL_27ef,
                          IL_137d,
                          IL_2852,
                          IL_2869,
                          IL_28bf,
                          IL_2915,
                          IL_292c,
                          IL_2982,
                          IL_29d8,
                          IL_2a2e,
                          IL_2a84,
                          IL_14cb,
                          IL_2ae7,
                          IL_132a,
                          IL_2b15,
                          IL_2b34,
                          IL_12be,
                          IL_2b97,
                          IL_2bae,
                          IL_2c04)
    IL_2123:  ldarg.0
    IL_2124:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2129:  ldloc.s    V_3
    IL_212b:  ldloc.s    V_2
    IL_212d:  dup
    IL_212e:  ldc.i4.1
    IL_212f:  add
    IL_2130:  stloc.s    V_2
    IL_2132:  ldelem.i4
    IL_2133:  stloc.s    V_0
    IL_2135:  br         IL_1fff

    IL_213a:  ldarg.0
    IL_213b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2140:  ldloc.s    V_4
    IL_2142:  ldc.i4.s   1
    IL_2144:  add
    IL_2145:  stloc.s    V_4
    IL_2147:  br         IL_200e

    IL_214c:  ldarg.0
    IL_214d:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2152:  ldloc.s    V_3
    IL_2154:  ldloc.s    V_2
    IL_2156:  dup
    IL_2157:  ldc.i4.1
    IL_2158:  add
    IL_2159:  stloc.s    V_2
    IL_215b:  ldelem.i4
    IL_215c:  stloc.s    V_0
    IL_215e:  br         IL_1d45

    IL_2163:  ldarg.0
    IL_2164:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2169:  ldloc.s    V_3
    IL_216b:  ldloc.s    V_2
    IL_216d:  dup
    IL_216e:  ldc.i4.1
    IL_216f:  add
    IL_2170:  stloc.s    V_2
    IL_2172:  ldelem.i4
    IL_2173:  stloc.s    V_0
    IL_2175:  br         IL_0786

    IL_217a:  ldarg.0
    IL_217b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2180:  ldloc.s    V_3
    IL_2182:  ldloc.s    V_2
    IL_2184:  dup
    IL_2185:  ldc.i4.1
    IL_2186:  add
    IL_2187:  stloc.s    V_2
    IL_2189:  ldelem.i4
    IL_218a:  stloc.s    V_0
    IL_218c:  br         IL_0486

    IL_2191:  ldarg.0
    IL_2192:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2197:  ldloc.s    V_4
    IL_2199:  ldc.i4.s   2
    IL_219b:  add
    IL_219c:  stloc.s    V_4
    IL_219e:  br         IL_200e

    IL_21a3:  ldarg.0
    IL_21a4:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_21a9:  ldloc.s    V_3
    IL_21ab:  ldloc.s    V_2
    IL_21ad:  dup
    IL_21ae:  ldc.i4.1
    IL_21af:  add
    IL_21b0:  stloc.s    V_2
    IL_21b2:  ldelem.i4
    IL_21b3:  stloc.s    V_0
    IL_21b5:  br         IL_0263

    IL_21ba:  ldarg.0
    IL_21bb:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_21c0:  ldloc.s    V_3
    IL_21c2:  ldloc.s    V_2
    IL_21c4:  dup
    IL_21c5:  ldc.i4.1
    IL_21c6:  add
    IL_21c7:  stloc.s    V_2
    IL_21c9:  ldelem.i4
    IL_21ca:  stloc.s    V_0
    IL_21cc:  ldloc.s    V_3
    IL_21ce:  ldloc.s    V_2
    IL_21d0:  dup
    IL_21d1:  ldc.i4.1
    IL_21d2:  add
    IL_21d3:  stloc.s    V_2
    IL_21d5:  ldelem.i4
    IL_21d6:  stloc.s    V_6
    IL_21d8:  ldloc.s    V_6
    IL_21da:  ldc.i4.s   0
    IL_21dc:  ble        IL_0258

    IL_21e1:  ldloc.s    V_3
    IL_21e3:  ldloc.s    V_2
    IL_21e5:  ldc.i4.1
    IL_21e6:  sub
    IL_21e7:  dup
    IL_21e8:  stloc.s    V_2
    IL_21ea:  ldloc.s    V_6
    IL_21ec:  ldc.i4.s   1
    IL_21ee:  sub
    IL_21ef:  stelem.i4
    IL_21f0:  ldloc.s    V_3
    IL_21f2:  ldloc.s    V_2
    IL_21f4:  ldc.i4.1
    IL_21f5:  sub
    IL_21f6:  dup
    IL_21f7:  stloc.s    V_2
    IL_21f9:  ldloc.s    V_0
    IL_21fb:  ldc.i4.s   1
    IL_21fd:  sub
    IL_21fe:  stelem.i4
    IL_21ff:  ldloc.s    V_3
    IL_2201:  ldloc.s    V_2
    IL_2203:  ldc.i4.1
    IL_2204:  sub
    IL_2205:  dup
    IL_2206:  stloc.s    V_2
    IL_2208:  ldc.i4.s   7
    IL_220a:  stelem.i4
    IL_220b:  br         IL_0258

    IL_2210:  ldarg.0
    IL_2211:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2216:  ldloc.s    V_5
    IL_2218:  ldloc.s    V_4
    IL_221a:  ldc.i4.1
    IL_221b:  sub
    IL_221c:  dup
    IL_221d:  stloc.s    V_4
    IL_221f:  ldloc.s    V_3
    IL_2221:  ldloc.s    V_2
    IL_2223:  dup
    IL_2224:  ldc.i4.1
    IL_2225:  add
    IL_2226:  stloc.s    V_2
    IL_2228:  ldelem.i4
    IL_2229:  stelem.i4
    IL_222a:  ldarg.0
    IL_222b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Uncapture()
    IL_2230:  br         IL_200e

    IL_2235:  ldarg.0
    IL_2236:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_223b:  ldloc.s    V_5
    IL_223d:  ldloc.s    V_4
    IL_223f:  dup
    IL_2240:  ldc.i4.1
    IL_2241:  add
    IL_2242:  stloc.s    V_4
    IL_2244:  ldelem.i4
    IL_2245:  ldc.i4.s   1
    IL_2247:  sub
    IL_2248:  dup
    IL_2249:  stloc.s    V_6
    IL_224b:  ldc.i4.s   0
    IL_224d:  blt.s      IL_2278

    IL_224f:  ldloc.s    V_5
    IL_2251:  ldloc.s    V_4
    IL_2253:  dup
    IL_2254:  ldc.i4.1
    IL_2255:  add
    IL_2256:  stloc.s    V_4
    IL_2258:  ldelem.i4
    IL_2259:  stloc.s    V_0
    IL_225b:  ldloc.s    V_3
    IL_225d:  ldloc.s    V_2
    IL_225f:  ldc.i4.1
    IL_2260:  sub
    IL_2261:  dup
    IL_2262:  stloc.s    V_2
    IL_2264:  ldloc.s    V_6
    IL_2266:  stelem.i4
    IL_2267:  ldloc.s    V_3
    IL_2269:  ldloc.s    V_2
    IL_226b:  ldc.i4.1
    IL_226c:  sub
    IL_226d:  dup
    IL_226e:  stloc.s    V_2
    IL_2270:  ldc.i4.s   11
    IL_2272:  stelem.i4
    IL_2273:  br         IL_0366

    IL_2278:  ldloc.s    V_5
    IL_227a:  ldloc.s    V_4
    IL_227c:  ldloc.s    V_3
    IL_227e:  ldloc.s    V_2
    IL_2280:  dup
    IL_2281:  ldc.i4.1
    IL_2282:  add
    IL_2283:  stloc.s    V_2
    IL_2285:  ldelem.i4
    IL_2286:  stelem.i4
    IL_2287:  ldloc.s    V_5
    IL_2289:  ldloc.s    V_4
    IL_228b:  ldc.i4.1
    IL_228c:  sub
    IL_228d:  dup
    IL_228e:  stloc.s    V_4
    IL_2290:  ldloc.s    V_6
    IL_2292:  stelem.i4
    IL_2293:  br         IL_200e

    IL_2298:  ldarg.0
    IL_2299:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_229e:  ldloc.s    V_3
    IL_22a0:  ldloc.s    V_2
    IL_22a2:  dup
    IL_22a3:  ldc.i4.1
    IL_22a4:  add
    IL_22a5:  stloc.s    V_2
    IL_22a7:  ldelem.i4
    IL_22a8:  stloc.s    V_6
    IL_22aa:  ldloc.s    V_5
    IL_22ac:  ldloc.s    V_4
    IL_22ae:  ldc.i4.1
    IL_22af:  sub
    IL_22b0:  dup
    IL_22b1:  stloc.s    V_4
    IL_22b3:  ldloc.s    V_3
    IL_22b5:  ldloc.s    V_2
    IL_22b7:  dup
    IL_22b8:  ldc.i4.1
    IL_22b9:  add
    IL_22ba:  stloc.s    V_2
    IL_22bc:  ldelem.i4
    IL_22bd:  stelem.i4
    IL_22be:  ldloc.s    V_5
    IL_22c0:  ldloc.s    V_4
    IL_22c2:  ldc.i4.1
    IL_22c3:  sub
    IL_22c4:  dup
    IL_22c5:  stloc.s    V_4
    IL_22c7:  ldloc.s    V_6
    IL_22c9:  stelem.i4
    IL_22ca:  br         IL_200e

    IL_22cf:  ldarg.0
    IL_22d0:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_22d5:  ldloc.s    V_3
    IL_22d7:  ldloc.s    V_2
    IL_22d9:  dup
    IL_22da:  ldc.i4.1
    IL_22db:  add
    IL_22dc:  stloc.s    V_2
    IL_22de:  ldelem.i4
    IL_22df:  stloc.s    V_0
    IL_22e1:  ldloc.s    V_3
    IL_22e3:  ldloc.s    V_2
    IL_22e5:  dup
    IL_22e6:  ldc.i4.1
    IL_22e7:  add
    IL_22e8:  stloc.s    V_2
    IL_22ea:  ldelem.i4
    IL_22eb:  stloc.s    V_6
    IL_22ed:  ldloc.s    V_6
    IL_22ef:  ldc.i4.s   0
    IL_22f1:  ble        IL_0445

    IL_22f6:  ldloc.s    V_3
    IL_22f8:  ldloc.s    V_2
    IL_22fa:  ldc.i4.1
    IL_22fb:  sub
    IL_22fc:  dup
    IL_22fd:  stloc.s    V_2
    IL_22ff:  ldloc.s    V_6
    IL_2301:  ldc.i4.s   1
    IL_2303:  sub
    IL_2304:  stelem.i4
    IL_2305:  ldloc.s    V_3
    IL_2307:  ldloc.s    V_2
    IL_2309:  ldc.i4.1
    IL_230a:  sub
    IL_230b:  dup
    IL_230c:  stloc.s    V_2
    IL_230e:  ldloc.s    V_0
    IL_2310:  ldc.i4.s   1
    IL_2312:  sub
    IL_2313:  stelem.i4
    IL_2314:  ldloc.s    V_3
    IL_2316:  ldloc.s    V_2
    IL_2318:  ldc.i4.1
    IL_2319:  sub
    IL_231a:  dup
    IL_231b:  stloc.s    V_2
    IL_231d:  ldc.i4.s   12
    IL_231f:  stelem.i4
    IL_2320:  br         IL_0445

    IL_2325:  ldarg.0
    IL_2326:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_232b:  ldloc.s    V_3
    IL_232d:  ldloc.s    V_2
    IL_232f:  dup
    IL_2330:  ldc.i4.1
    IL_2331:  add
    IL_2332:  stloc.s    V_2
    IL_2334:  ldelem.i4
    IL_2335:  stloc.s    V_0
    IL_2337:  ldloc.s    V_3
    IL_2339:  ldloc.s    V_2
    IL_233b:  dup
    IL_233c:  ldc.i4.1
    IL_233d:  add
    IL_233e:  stloc.s    V_2
    IL_2340:  ldelem.i4
    IL_2341:  stloc.s    V_6
    IL_2343:  ldloc.s    V_6
    IL_2345:  ldc.i4.s   0
    IL_2347:  ble        IL_0525

    IL_234c:  ldloc.s    V_3
    IL_234e:  ldloc.s    V_2
    IL_2350:  ldc.i4.1
    IL_2351:  sub
    IL_2352:  dup
    IL_2353:  stloc.s    V_2
    IL_2355:  ldloc.s    V_6
    IL_2357:  ldc.i4.s   1
    IL_2359:  sub
    IL_235a:  stelem.i4
    IL_235b:  ldloc.s    V_3
    IL_235d:  ldloc.s    V_2
    IL_235f:  ldc.i4.1
    IL_2360:  sub
    IL_2361:  dup
    IL_2362:  stloc.s    V_2
    IL_2364:  ldloc.s    V_0
    IL_2366:  ldc.i4.s   1
    IL_2368:  sub
    IL_2369:  stelem.i4
    IL_236a:  ldloc.s    V_3
    IL_236c:  ldloc.s    V_2
    IL_236e:  ldc.i4.1
    IL_236f:  sub
    IL_2370:  dup
    IL_2371:  stloc.s    V_2
    IL_2373:  ldc.i4.s   13
    IL_2375:  stelem.i4
    IL_2376:  br         IL_0525

    IL_237b:  ldarg.0
    IL_237c:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2381:  ldloc.s    V_3
    IL_2383:  ldloc.s    V_2
    IL_2385:  dup
    IL_2386:  ldc.i4.1
    IL_2387:  add
    IL_2388:  stloc.s    V_2
    IL_238a:  ldelem.i4
    IL_238b:  stloc.s    V_0
    IL_238d:  ldloc.s    V_3
    IL_238f:  ldloc.s    V_2
    IL_2391:  dup
    IL_2392:  ldc.i4.1
    IL_2393:  add
    IL_2394:  stloc.s    V_2
    IL_2396:  ldelem.i4
    IL_2397:  stloc.s    V_6
    IL_2399:  ldloc.s    V_6
    IL_239b:  ldc.i4.s   0
    IL_239d:  ble        IL_0604

    IL_23a2:  ldloc.s    V_3
    IL_23a4:  ldloc.s    V_2
    IL_23a6:  ldc.i4.1
    IL_23a7:  sub
    IL_23a8:  dup
    IL_23a9:  stloc.s    V_2
    IL_23ab:  ldloc.s    V_6
    IL_23ad:  ldc.i4.s   1
    IL_23af:  sub
    IL_23b0:  stelem.i4
    IL_23b1:  ldloc.s    V_3
    IL_23b3:  ldloc.s    V_2
    IL_23b5:  ldc.i4.1
    IL_23b6:  sub
    IL_23b7:  dup
    IL_23b8:  stloc.s    V_2
    IL_23ba:  ldloc.s    V_0
    IL_23bc:  ldc.i4.s   1
    IL_23be:  sub
    IL_23bf:  stelem.i4
    IL_23c0:  ldloc.s    V_3
    IL_23c2:  ldloc.s    V_2
    IL_23c4:  ldc.i4.1
    IL_23c5:  sub
    IL_23c6:  dup
    IL_23c7:  stloc.s    V_2
    IL_23c9:  ldc.i4.s   14
    IL_23cb:  stelem.i4
    IL_23cc:  br         IL_0604

    IL_23d1:  ldarg.0
    IL_23d2:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_23d7:  ldloc.s    V_3
    IL_23d9:  ldloc.s    V_2
    IL_23db:  dup
    IL_23dc:  ldc.i4.1
    IL_23dd:  add
    IL_23de:  stloc.s    V_2
    IL_23e0:  ldelem.i4
    IL_23e1:  stloc.s    V_0
    IL_23e3:  ldloc.s    V_3
    IL_23e5:  ldloc.s    V_2
    IL_23e7:  dup
    IL_23e8:  ldc.i4.1
    IL_23e9:  add
    IL_23ea:  stloc.s    V_2
    IL_23ec:  ldelem.i4
    IL_23ed:  stloc.s    V_6
    IL_23ef:  ldloc.s    V_6
    IL_23f1:  ldc.i4.s   0
    IL_23f3:  ble        IL_0720

    IL_23f8:  ldloc.s    V_3
    IL_23fa:  ldloc.s    V_2
    IL_23fc:  ldc.i4.1
    IL_23fd:  sub
    IL_23fe:  dup
    IL_23ff:  stloc.s    V_2
    IL_2401:  ldloc.s    V_6
    IL_2403:  ldc.i4.s   1
    IL_2405:  sub
    IL_2406:  stelem.i4
    IL_2407:  ldloc.s    V_3
    IL_2409:  ldloc.s    V_2
    IL_240b:  ldc.i4.1
    IL_240c:  sub
    IL_240d:  dup
    IL_240e:  stloc.s    V_2
    IL_2410:  ldloc.s    V_0
    IL_2412:  ldc.i4.s   1
    IL_2414:  sub
    IL_2415:  stelem.i4
    IL_2416:  ldloc.s    V_3
    IL_2418:  ldloc.s    V_2
    IL_241a:  ldc.i4.1
    IL_241b:  sub
    IL_241c:  dup
    IL_241d:  stloc.s    V_2
    IL_241f:  ldc.i4.s   15
    IL_2421:  stelem.i4
    IL_2422:  br         IL_0720

    IL_2427:  ldarg.0
    IL_2428:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_242d:  ldloc.s    V_3
    IL_242f:  ldloc.s    V_2
    IL_2431:  dup
    IL_2432:  ldc.i4.1
    IL_2433:  add
    IL_2434:  stloc.s    V_2
    IL_2436:  ldelem.i4
    IL_2437:  stloc.s    V_0
    IL_2439:  br         IL_08ac

    IL_243e:  ldarg.0
    IL_243f:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2444:  ldloc.s    V_3
    IL_2446:  ldloc.s    V_2
    IL_2448:  dup
    IL_2449:  ldc.i4.1
    IL_244a:  add
    IL_244b:  stloc.s    V_2
    IL_244d:  ldelem.i4
    IL_244e:  stloc.s    V_0
    IL_2450:  ldloc.s    V_3
    IL_2452:  ldloc.s    V_2
    IL_2454:  dup
    IL_2455:  ldc.i4.1
    IL_2456:  add
    IL_2457:  stloc.s    V_2
    IL_2459:  ldelem.i4
    IL_245a:  stloc.s    V_6
    IL_245c:  ldloc.s    V_6
    IL_245e:  ldc.i4.s   0
    IL_2460:  ble        IL_0865

    IL_2465:  ldloc.s    V_3
    IL_2467:  ldloc.s    V_2
    IL_2469:  ldc.i4.1
    IL_246a:  sub
    IL_246b:  dup
    IL_246c:  stloc.s    V_2
    IL_246e:  ldloc.s    V_6
    IL_2470:  ldc.i4.s   1
    IL_2472:  sub
    IL_2473:  stelem.i4
    IL_2474:  ldloc.s    V_3
    IL_2476:  ldloc.s    V_2
    IL_2478:  ldc.i4.1
    IL_2479:  sub
    IL_247a:  dup
    IL_247b:  stloc.s    V_2
    IL_247d:  ldloc.s    V_0
    IL_247f:  ldc.i4.s   1
    IL_2481:  sub
    IL_2482:  stelem.i4
    IL_2483:  ldloc.s    V_3
    IL_2485:  ldloc.s    V_2
    IL_2487:  ldc.i4.1
    IL_2488:  sub
    IL_2489:  dup
    IL_248a:  stloc.s    V_2
    IL_248c:  ldc.i4.s   17
    IL_248e:  stelem.i4
    IL_248f:  br         IL_0865

    IL_2494:  ldarg.0
    IL_2495:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_249a:  ldloc.s    V_3
    IL_249c:  ldloc.s    V_2
    IL_249e:  dup
    IL_249f:  ldc.i4.1
    IL_24a0:  add
    IL_24a1:  stloc.s    V_2
    IL_24a3:  ldelem.i4
    IL_24a4:  stloc.s    V_0
    IL_24a6:  ldloc.s    V_3
    IL_24a8:  ldloc.s    V_2
    IL_24aa:  dup
    IL_24ab:  ldc.i4.1
    IL_24ac:  add
    IL_24ad:  stloc.s    V_2
    IL_24af:  ldelem.i4
    IL_24b0:  stloc.s    V_6
    IL_24b2:  ldloc.s    V_6
    IL_24b4:  ldc.i4.s   0
    IL_24b6:  ble        IL_0a12

    IL_24bb:  ldloc.s    V_3
    IL_24bd:  ldloc.s    V_2
    IL_24bf:  ldc.i4.1
    IL_24c0:  sub
    IL_24c1:  dup
    IL_24c2:  stloc.s    V_2
    IL_24c4:  ldloc.s    V_6
    IL_24c6:  ldc.i4.s   1
    IL_24c8:  sub
    IL_24c9:  stelem.i4
    IL_24ca:  ldloc.s    V_3
    IL_24cc:  ldloc.s    V_2
    IL_24ce:  ldc.i4.1
    IL_24cf:  sub
    IL_24d0:  dup
    IL_24d1:  stloc.s    V_2
    IL_24d3:  ldloc.s    V_0
    IL_24d5:  ldc.i4.s   1
    IL_24d7:  sub
    IL_24d8:  stelem.i4
    IL_24d9:  ldloc.s    V_3
    IL_24db:  ldloc.s    V_2
    IL_24dd:  ldc.i4.1
    IL_24de:  sub
    IL_24df:  dup
    IL_24e0:  stloc.s    V_2
    IL_24e2:  ldc.i4.s   18
    IL_24e4:  stelem.i4
    IL_24e5:  br         IL_0a12

    IL_24ea:  ldarg.0
    IL_24eb:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_24f0:  ldloc.s    V_5
    IL_24f2:  ldloc.s    V_4
    IL_24f4:  dup
    IL_24f5:  ldc.i4.1
    IL_24f6:  add
    IL_24f7:  stloc.s    V_4
    IL_24f9:  ldelem.i4
    IL_24fa:  ldc.i4.s   1
    IL_24fc:  sub
    IL_24fd:  dup
    IL_24fe:  stloc.s    V_6
    IL_2500:  ldc.i4.s   0
    IL_2502:  blt.s      IL_252d

    IL_2504:  ldloc.s    V_5
    IL_2506:  ldloc.s    V_4
    IL_2508:  dup
    IL_2509:  ldc.i4.1
    IL_250a:  add
    IL_250b:  stloc.s    V_4
    IL_250d:  ldelem.i4
    IL_250e:  stloc.s    V_0
    IL_2510:  ldloc.s    V_3
    IL_2512:  ldloc.s    V_2
    IL_2514:  ldc.i4.1
    IL_2515:  sub
    IL_2516:  dup
    IL_2517:  stloc.s    V_2
    IL_2519:  ldloc.s    V_6
    IL_251b:  stelem.i4
    IL_251c:  ldloc.s    V_3
    IL_251e:  ldloc.s    V_2
    IL_2520:  ldc.i4.1
    IL_2521:  sub
    IL_2522:  dup
    IL_2523:  stloc.s    V_2
    IL_2525:  ldc.i4.s   11
    IL_2527:  stelem.i4
    IL_2528:  br         IL_0b60

    IL_252d:  ldloc.s    V_5
    IL_252f:  ldloc.s    V_4
    IL_2531:  ldloc.s    V_3
    IL_2533:  ldloc.s    V_2
    IL_2535:  dup
    IL_2536:  ldc.i4.1
    IL_2537:  add
    IL_2538:  stloc.s    V_2
    IL_253a:  ldelem.i4
    IL_253b:  stelem.i4
    IL_253c:  ldloc.s    V_5
    IL_253e:  ldloc.s    V_4
    IL_2540:  ldc.i4.1
    IL_2541:  sub
    IL_2542:  dup
    IL_2543:  stloc.s    V_4
    IL_2545:  ldloc.s    V_6
    IL_2547:  stelem.i4
    IL_2548:  br         IL_200e

    IL_254d:  ldarg.0
    IL_254e:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2553:  ldloc.s    V_5
    IL_2555:  ldloc.s    V_4
    IL_2557:  dup
    IL_2558:  ldc.i4.1
    IL_2559:  add
    IL_255a:  stloc.s    V_4
    IL_255c:  ldelem.i4
    IL_255d:  ldc.i4.s   1
    IL_255f:  sub
    IL_2560:  dup
    IL_2561:  stloc.s    V_6
    IL_2563:  ldc.i4.s   0
    IL_2565:  blt.s      IL_2590

    IL_2567:  ldloc.s    V_5
    IL_2569:  ldloc.s    V_4
    IL_256b:  dup
    IL_256c:  ldc.i4.1
    IL_256d:  add
    IL_256e:  stloc.s    V_4
    IL_2570:  ldelem.i4
    IL_2571:  stloc.s    V_0
    IL_2573:  ldloc.s    V_3
    IL_2575:  ldloc.s    V_2
    IL_2577:  ldc.i4.1
    IL_2578:  sub
    IL_2579:  dup
    IL_257a:  stloc.s    V_2
    IL_257c:  ldloc.s    V_6
    IL_257e:  stelem.i4
    IL_257f:  ldloc.s    V_3
    IL_2581:  ldloc.s    V_2
    IL_2583:  ldc.i4.1
    IL_2584:  sub
    IL_2585:  dup
    IL_2586:  stloc.s    V_2
    IL_2588:  ldc.i4.s   11
    IL_258a:  stelem.i4
    IL_258b:  br         IL_0c36

    IL_2590:  ldloc.s    V_5
    IL_2592:  ldloc.s    V_4
    IL_2594:  ldloc.s    V_3
    IL_2596:  ldloc.s    V_2
    IL_2598:  dup
    IL_2599:  ldc.i4.1
    IL_259a:  add
    IL_259b:  stloc.s    V_2
    IL_259d:  ldelem.i4
    IL_259e:  stelem.i4
    IL_259f:  ldloc.s    V_5
    IL_25a1:  ldloc.s    V_4
    IL_25a3:  ldc.i4.1
    IL_25a4:  sub
    IL_25a5:  dup
    IL_25a6:  stloc.s    V_4
    IL_25a8:  ldloc.s    V_6
    IL_25aa:  stelem.i4
    IL_25ab:  br         IL_200e

    IL_25b0:  ldarg.0
    IL_25b1:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_25b6:  ldloc.s    V_3
    IL_25b8:  ldloc.s    V_2
    IL_25ba:  dup
    IL_25bb:  ldc.i4.1
    IL_25bc:  add
    IL_25bd:  stloc.s    V_2
    IL_25bf:  ldelem.i4
    IL_25c0:  stloc.s    V_0
    IL_25c2:  ldloc.s    V_3
    IL_25c4:  ldloc.s    V_2
    IL_25c6:  dup
    IL_25c7:  ldc.i4.1
    IL_25c8:  add
    IL_25c9:  stloc.s    V_2
    IL_25cb:  ldelem.i4
    IL_25cc:  stloc.s    V_6
    IL_25ce:  ldloc.s    V_6
    IL_25d0:  ldc.i4.s   0
    IL_25d2:  ble        IL_0d6f

    IL_25d7:  ldloc.s    V_3
    IL_25d9:  ldloc.s    V_2
    IL_25db:  ldc.i4.1
    IL_25dc:  sub
    IL_25dd:  dup
    IL_25de:  stloc.s    V_2
    IL_25e0:  ldloc.s    V_6
    IL_25e2:  ldc.i4.s   1
    IL_25e4:  sub
    IL_25e5:  stelem.i4
    IL_25e6:  ldloc.s    V_3
    IL_25e8:  ldloc.s    V_2
    IL_25ea:  ldc.i4.1
    IL_25eb:  sub
    IL_25ec:  dup
    IL_25ed:  stloc.s    V_2
    IL_25ef:  ldloc.s    V_0
    IL_25f1:  ldc.i4.s   1
    IL_25f3:  sub
    IL_25f4:  stelem.i4
    IL_25f5:  ldloc.s    V_3
    IL_25f7:  ldloc.s    V_2
    IL_25f9:  ldc.i4.1
    IL_25fa:  sub
    IL_25fb:  dup
    IL_25fc:  stloc.s    V_2
    IL_25fe:  ldc.i4.s   23
    IL_2600:  stelem.i4
    IL_2601:  br         IL_0d6f

    IL_2606:  ldarg.0
    IL_2607:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_260c:  ldloc.s    V_5
    IL_260e:  ldloc.s    V_4
    IL_2610:  dup
    IL_2611:  ldc.i4.1
    IL_2612:  add
    IL_2613:  stloc.s    V_4
    IL_2615:  ldelem.i4
    IL_2616:  ldc.i4.s   1
    IL_2618:  sub
    IL_2619:  dup
    IL_261a:  stloc.s    V_6
    IL_261c:  ldc.i4.s   0
    IL_261e:  blt.s      IL_2649

    IL_2620:  ldloc.s    V_5
    IL_2622:  ldloc.s    V_4
    IL_2624:  dup
    IL_2625:  ldc.i4.1
    IL_2626:  add
    IL_2627:  stloc.s    V_4
    IL_2629:  ldelem.i4
    IL_262a:  stloc.s    V_0
    IL_262c:  ldloc.s    V_3
    IL_262e:  ldloc.s    V_2
    IL_2630:  ldc.i4.1
    IL_2631:  sub
    IL_2632:  dup
    IL_2633:  stloc.s    V_2
    IL_2635:  ldloc.s    V_6
    IL_2637:  stelem.i4
    IL_2638:  ldloc.s    V_3
    IL_263a:  ldloc.s    V_2
    IL_263c:  ldc.i4.1
    IL_263d:  sub
    IL_263e:  dup
    IL_263f:  stloc.s    V_2
    IL_2641:  ldc.i4.s   11
    IL_2643:  stelem.i4
    IL_2644:  br         IL_0e6a

    IL_2649:  ldloc.s    V_5
    IL_264b:  ldloc.s    V_4
    IL_264d:  ldloc.s    V_3
    IL_264f:  ldloc.s    V_2
    IL_2651:  dup
    IL_2652:  ldc.i4.1
    IL_2653:  add
    IL_2654:  stloc.s    V_2
    IL_2656:  ldelem.i4
    IL_2657:  stelem.i4
    IL_2658:  ldloc.s    V_5
    IL_265a:  ldloc.s    V_4
    IL_265c:  ldc.i4.1
    IL_265d:  sub
    IL_265e:  dup
    IL_265f:  stloc.s    V_4
    IL_2661:  ldloc.s    V_6
    IL_2663:  stelem.i4
    IL_2664:  br         IL_200e

    IL_2669:  ldarg.0
    IL_266a:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_266f:  ldloc.s    V_3
    IL_2671:  ldloc.s    V_2
    IL_2673:  dup
    IL_2674:  ldc.i4.1
    IL_2675:  add
    IL_2676:  stloc.s    V_2
    IL_2678:  ldelem.i4
    IL_2679:  stloc.s    V_0
    IL_267b:  br         IL_0fbc

    IL_2680:  ldarg.0
    IL_2681:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2686:  ldloc.s    V_3
    IL_2688:  ldloc.s    V_2
    IL_268a:  dup
    IL_268b:  ldc.i4.1
    IL_268c:  add
    IL_268d:  stloc.s    V_2
    IL_268f:  ldelem.i4
    IL_2690:  stloc.s    V_0
    IL_2692:  ldloc.s    V_3
    IL_2694:  ldloc.s    V_2
    IL_2696:  dup
    IL_2697:  ldc.i4.1
    IL_2698:  add
    IL_2699:  stloc.s    V_2
    IL_269b:  ldelem.i4
    IL_269c:  stloc.s    V_6
    IL_269e:  ldloc.s    V_6
    IL_26a0:  ldc.i4.s   0
    IL_26a2:  ble        IL_0f8c

    IL_26a7:  ldloc.s    V_3
    IL_26a9:  ldloc.s    V_2
    IL_26ab:  ldc.i4.1
    IL_26ac:  sub
    IL_26ad:  dup
    IL_26ae:  stloc.s    V_2
    IL_26b0:  ldloc.s    V_6
    IL_26b2:  ldc.i4.s   1
    IL_26b4:  sub
    IL_26b5:  stelem.i4
    IL_26b6:  ldloc.s    V_3
    IL_26b8:  ldloc.s    V_2
    IL_26ba:  ldc.i4.1
    IL_26bb:  sub
    IL_26bc:  dup
    IL_26bd:  stloc.s    V_2
    IL_26bf:  ldloc.s    V_0
    IL_26c1:  ldc.i4.s   1
    IL_26c3:  sub
    IL_26c4:  stelem.i4
    IL_26c5:  ldloc.s    V_3
    IL_26c7:  ldloc.s    V_2
    IL_26c9:  ldc.i4.1
    IL_26ca:  sub
    IL_26cb:  dup
    IL_26cc:  stloc.s    V_2
    IL_26ce:  ldc.i4.s   27
    IL_26d0:  stelem.i4
    IL_26d1:  br         IL_0f8c

    IL_26d6:  ldarg.0
    IL_26d7:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_26dc:  ldloc.s    V_3
    IL_26de:  ldloc.s    V_2
    IL_26e0:  dup
    IL_26e1:  ldc.i4.1
    IL_26e2:  add
    IL_26e3:  stloc.s    V_2
    IL_26e5:  ldelem.i4
    IL_26e6:  stloc.s    V_0
    IL_26e8:  br         IL_10e2

    IL_26ed:  ldarg.0
    IL_26ee:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_26f3:  ldloc.s    V_3
    IL_26f5:  ldloc.s    V_2
    IL_26f7:  dup
    IL_26f8:  ldc.i4.1
    IL_26f9:  add
    IL_26fa:  stloc.s    V_2
    IL_26fc:  ldelem.i4
    IL_26fd:  stloc.s    V_0
    IL_26ff:  ldloc.s    V_3
    IL_2701:  ldloc.s    V_2
    IL_2703:  dup
    IL_2704:  ldc.i4.1
    IL_2705:  add
    IL_2706:  stloc.s    V_2
    IL_2708:  ldelem.i4
    IL_2709:  stloc.s    V_6
    IL_270b:  ldloc.s    V_6
    IL_270d:  ldc.i4.s   0
    IL_270f:  ble        IL_109b

    IL_2714:  ldloc.s    V_3
    IL_2716:  ldloc.s    V_2
    IL_2718:  ldc.i4.1
    IL_2719:  sub
    IL_271a:  dup
    IL_271b:  stloc.s    V_2
    IL_271d:  ldloc.s    V_6
    IL_271f:  ldc.i4.s   1
    IL_2721:  sub
    IL_2722:  stelem.i4
    IL_2723:  ldloc.s    V_3
    IL_2725:  ldloc.s    V_2
    IL_2727:  ldc.i4.1
    IL_2728:  sub
    IL_2729:  dup
    IL_272a:  stloc.s    V_2
    IL_272c:  ldloc.s    V_0
    IL_272e:  ldc.i4.s   1
    IL_2730:  sub
    IL_2731:  stelem.i4
    IL_2732:  ldloc.s    V_3
    IL_2734:  ldloc.s    V_2
    IL_2736:  ldc.i4.1
    IL_2737:  sub
    IL_2738:  dup
    IL_2739:  stloc.s    V_2
    IL_273b:  ldc.i4.s   29
    IL_273d:  stelem.i4
    IL_273e:  br         IL_109b

    IL_2743:  ldarg.0
    IL_2744:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2749:  ldloc.s    V_3
    IL_274b:  ldloc.s    V_2
    IL_274d:  dup
    IL_274e:  ldc.i4.1
    IL_274f:  add
    IL_2750:  stloc.s    V_2
    IL_2752:  ldelem.i4
    IL_2753:  stloc.s    V_0
    IL_2755:  ldloc.s    V_3
    IL_2757:  ldloc.s    V_2
    IL_2759:  dup
    IL_275a:  ldc.i4.1
    IL_275b:  add
    IL_275c:  stloc.s    V_2
    IL_275e:  ldelem.i4
    IL_275f:  stloc.s    V_6
    IL_2761:  ldloc.s    V_6
    IL_2763:  ldc.i4.s   0
    IL_2765:  ble        IL_11a3

    IL_276a:  ldloc.s    V_3
    IL_276c:  ldloc.s    V_2
    IL_276e:  ldc.i4.1
    IL_276f:  sub
    IL_2770:  dup
    IL_2771:  stloc.s    V_2
    IL_2773:  ldloc.s    V_6
    IL_2775:  ldc.i4.s   1
    IL_2777:  sub
    IL_2778:  stelem.i4
    IL_2779:  ldloc.s    V_3
    IL_277b:  ldloc.s    V_2
    IL_277d:  ldc.i4.1
    IL_277e:  sub
    IL_277f:  dup
    IL_2780:  stloc.s    V_2
    IL_2782:  ldloc.s    V_0
    IL_2784:  ldc.i4.s   1
    IL_2786:  sub
    IL_2787:  stelem.i4
    IL_2788:  ldloc.s    V_3
    IL_278a:  ldloc.s    V_2
    IL_278c:  ldc.i4.1
    IL_278d:  sub
    IL_278e:  dup
    IL_278f:  stloc.s    V_2
    IL_2791:  ldc.i4.s   30
    IL_2793:  stelem.i4
    IL_2794:  br         IL_11a3

    IL_2799:  ldarg.0
    IL_279a:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_279f:  ldloc.s    V_3
    IL_27a1:  ldloc.s    V_2
    IL_27a3:  dup
    IL_27a4:  ldc.i4.1
    IL_27a5:  add
    IL_27a6:  stloc.s    V_2
    IL_27a8:  ldelem.i4
    IL_27a9:  stloc.s    V_0
    IL_27ab:  ldloc.s    V_3
    IL_27ad:  ldloc.s    V_2
    IL_27af:  dup
    IL_27b0:  ldc.i4.1
    IL_27b1:  add
    IL_27b2:  stloc.s    V_2
    IL_27b4:  ldelem.i4
    IL_27b5:  stloc.s    V_6
    IL_27b7:  ldloc.s    V_6
    IL_27b9:  ldc.i4.s   0
    IL_27bb:  ble        IL_1289

    IL_27c0:  ldloc.s    V_3
    IL_27c2:  ldloc.s    V_2
    IL_27c4:  ldc.i4.1
    IL_27c5:  sub
    IL_27c6:  dup
    IL_27c7:  stloc.s    V_2
    IL_27c9:  ldloc.s    V_6
    IL_27cb:  ldc.i4.s   1
    IL_27cd:  sub
    IL_27ce:  stelem.i4
    IL_27cf:  ldloc.s    V_3
    IL_27d1:  ldloc.s    V_2
    IL_27d3:  ldc.i4.1
    IL_27d4:  sub
    IL_27d5:  dup
    IL_27d6:  stloc.s    V_2
    IL_27d8:  ldloc.s    V_0
    IL_27da:  ldc.i4.s   1
    IL_27dc:  sub
    IL_27dd:  stelem.i4
    IL_27de:  ldloc.s    V_3
    IL_27e0:  ldloc.s    V_2
    IL_27e2:  ldc.i4.1
    IL_27e3:  sub
    IL_27e4:  dup
    IL_27e5:  stloc.s    V_2
    IL_27e7:  ldc.i4.s   31
    IL_27e9:  stelem.i4
    IL_27ea:  br         IL_1289

    IL_27ef:  ldarg.0
    IL_27f0:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_27f5:  ldloc.s    V_5
    IL_27f7:  ldloc.s    V_4
    IL_27f9:  dup
    IL_27fa:  ldc.i4.1
    IL_27fb:  add
    IL_27fc:  stloc.s    V_4
    IL_27fe:  ldelem.i4
    IL_27ff:  ldc.i4.s   1
    IL_2801:  sub
    IL_2802:  dup
    IL_2803:  stloc.s    V_6
    IL_2805:  ldc.i4.s   0
    IL_2807:  blt.s      IL_2832

    IL_2809:  ldloc.s    V_5
    IL_280b:  ldloc.s    V_4
    IL_280d:  dup
    IL_280e:  ldc.i4.1
    IL_280f:  add
    IL_2810:  stloc.s    V_4
    IL_2812:  ldelem.i4
    IL_2813:  stloc.s    V_0
    IL_2815:  ldloc.s    V_3
    IL_2817:  ldloc.s    V_2
    IL_2819:  ldc.i4.1
    IL_281a:  sub
    IL_281b:  dup
    IL_281c:  stloc.s    V_2
    IL_281e:  ldloc.s    V_6
    IL_2820:  stelem.i4
    IL_2821:  ldloc.s    V_3
    IL_2823:  ldloc.s    V_2
    IL_2825:  ldc.i4.1
    IL_2826:  sub
    IL_2827:  dup
    IL_2828:  stloc.s    V_2
    IL_282a:  ldc.i4.s   11
    IL_282c:  stelem.i4
    IL_282d:  br         IL_1496

    IL_2832:  ldloc.s    V_5
    IL_2834:  ldloc.s    V_4
    IL_2836:  ldloc.s    V_3
    IL_2838:  ldloc.s    V_2
    IL_283a:  dup
    IL_283b:  ldc.i4.1
    IL_283c:  add
    IL_283d:  stloc.s    V_2
    IL_283f:  ldelem.i4
    IL_2840:  stelem.i4
    IL_2841:  ldloc.s    V_5
    IL_2843:  ldloc.s    V_4
    IL_2845:  ldc.i4.1
    IL_2846:  sub
    IL_2847:  dup
    IL_2848:  stloc.s    V_4
    IL_284a:  ldloc.s    V_6
    IL_284c:  stelem.i4
    IL_284d:  br         IL_200e

    IL_2852:  ldarg.0
    IL_2853:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2858:  ldloc.s    V_3
    IL_285a:  ldloc.s    V_2
    IL_285c:  dup
    IL_285d:  ldc.i4.1
    IL_285e:  add
    IL_285f:  stloc.s    V_2
    IL_2861:  ldelem.i4
    IL_2862:  stloc.s    V_0
    IL_2864:  br         IL_1679

    IL_2869:  ldarg.0
    IL_286a:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_286f:  ldloc.s    V_3
    IL_2871:  ldloc.s    V_2
    IL_2873:  dup
    IL_2874:  ldc.i4.1
    IL_2875:  add
    IL_2876:  stloc.s    V_2
    IL_2878:  ldelem.i4
    IL_2879:  stloc.s    V_0
    IL_287b:  ldloc.s    V_3
    IL_287d:  ldloc.s    V_2
    IL_287f:  dup
    IL_2880:  ldc.i4.1
    IL_2881:  add
    IL_2882:  stloc.s    V_2
    IL_2884:  ldelem.i4
    IL_2885:  stloc.s    V_6
    IL_2887:  ldloc.s    V_6
    IL_2889:  ldc.i4.s   0
    IL_288b:  ble        IL_1581

    IL_2890:  ldloc.s    V_3
    IL_2892:  ldloc.s    V_2
    IL_2894:  ldc.i4.1
    IL_2895:  sub
    IL_2896:  dup
    IL_2897:  stloc.s    V_2
    IL_2899:  ldloc.s    V_6
    IL_289b:  ldc.i4.s   1
    IL_289d:  sub
    IL_289e:  stelem.i4
    IL_289f:  ldloc.s    V_3
    IL_28a1:  ldloc.s    V_2
    IL_28a3:  ldc.i4.1
    IL_28a4:  sub
    IL_28a5:  dup
    IL_28a6:  stloc.s    V_2
    IL_28a8:  ldloc.s    V_0
    IL_28aa:  ldc.i4.s   1
    IL_28ac:  sub
    IL_28ad:  stelem.i4
    IL_28ae:  ldloc.s    V_3
    IL_28b0:  ldloc.s    V_2
    IL_28b2:  ldc.i4.1
    IL_28b3:  sub
    IL_28b4:  dup
    IL_28b5:  stloc.s    V_2
    IL_28b7:  ldc.i4.s   35
    IL_28b9:  stelem.i4
    IL_28ba:  br         IL_1581

    IL_28bf:  ldarg.0
    IL_28c0:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_28c5:  ldloc.s    V_3
    IL_28c7:  ldloc.s    V_2
    IL_28c9:  dup
    IL_28ca:  ldc.i4.1
    IL_28cb:  add
    IL_28cc:  stloc.s    V_2
    IL_28ce:  ldelem.i4
    IL_28cf:  stloc.s    V_0
    IL_28d1:  ldloc.s    V_3
    IL_28d3:  ldloc.s    V_2
    IL_28d5:  dup
    IL_28d6:  ldc.i4.1
    IL_28d7:  add
    IL_28d8:  stloc.s    V_2
    IL_28da:  ldelem.i4
    IL_28db:  stloc.s    V_6
    IL_28dd:  ldloc.s    V_6
    IL_28df:  ldc.i4.s   0
    IL_28e1:  ble        IL_1638

    IL_28e6:  ldloc.s    V_3
    IL_28e8:  ldloc.s    V_2
    IL_28ea:  ldc.i4.1
    IL_28eb:  sub
    IL_28ec:  dup
    IL_28ed:  stloc.s    V_2
    IL_28ef:  ldloc.s    V_6
    IL_28f1:  ldc.i4.s   1
    IL_28f3:  sub
    IL_28f4:  stelem.i4
    IL_28f5:  ldloc.s    V_3
    IL_28f7:  ldloc.s    V_2
    IL_28f9:  ldc.i4.1
    IL_28fa:  sub
    IL_28fb:  dup
    IL_28fc:  stloc.s    V_2
    IL_28fe:  ldloc.s    V_0
    IL_2900:  ldc.i4.s   1
    IL_2902:  sub
    IL_2903:  stelem.i4
    IL_2904:  ldloc.s    V_3
    IL_2906:  ldloc.s    V_2
    IL_2908:  ldc.i4.1
    IL_2909:  sub
    IL_290a:  dup
    IL_290b:  stloc.s    V_2
    IL_290d:  ldc.i4.s   36
    IL_290f:  stelem.i4
    IL_2910:  br         IL_1638

    IL_2915:  ldarg.0
    IL_2916:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_291b:  ldloc.s    V_3
    IL_291d:  ldloc.s    V_2
    IL_291f:  dup
    IL_2920:  ldc.i4.1
    IL_2921:  add
    IL_2922:  stloc.s    V_2
    IL_2924:  ldelem.i4
    IL_2925:  stloc.s    V_0
    IL_2927:  br         IL_17b4

    IL_292c:  ldarg.0
    IL_292d:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2932:  ldloc.s    V_3
    IL_2934:  ldloc.s    V_2
    IL_2936:  dup
    IL_2937:  ldc.i4.1
    IL_2938:  add
    IL_2939:  stloc.s    V_2
    IL_293b:  ldelem.i4
    IL_293c:  stloc.s    V_0
    IL_293e:  ldloc.s    V_3
    IL_2940:  ldloc.s    V_2
    IL_2942:  dup
    IL_2943:  ldc.i4.1
    IL_2944:  add
    IL_2945:  stloc.s    V_2
    IL_2947:  ldelem.i4
    IL_2948:  stloc.s    V_6
    IL_294a:  ldloc.s    V_6
    IL_294c:  ldc.i4.s   0
    IL_294e:  ble        IL_1773

    IL_2953:  ldloc.s    V_3
    IL_2955:  ldloc.s    V_2
    IL_2957:  ldc.i4.1
    IL_2958:  sub
    IL_2959:  dup
    IL_295a:  stloc.s    V_2
    IL_295c:  ldloc.s    V_6
    IL_295e:  ldc.i4.s   1
    IL_2960:  sub
    IL_2961:  stelem.i4
    IL_2962:  ldloc.s    V_3
    IL_2964:  ldloc.s    V_2
    IL_2966:  ldc.i4.1
    IL_2967:  sub
    IL_2968:  dup
    IL_2969:  stloc.s    V_2
    IL_296b:  ldloc.s    V_0
    IL_296d:  ldc.i4.s   1
    IL_296f:  sub
    IL_2970:  stelem.i4
    IL_2971:  ldloc.s    V_3
    IL_2973:  ldloc.s    V_2
    IL_2975:  ldc.i4.1
    IL_2976:  sub
    IL_2977:  dup
    IL_2978:  stloc.s    V_2
    IL_297a:  ldc.i4.s   38
    IL_297c:  stelem.i4
    IL_297d:  br         IL_1773

    IL_2982:  ldarg.0
    IL_2983:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2988:  ldloc.s    V_3
    IL_298a:  ldloc.s    V_2
    IL_298c:  dup
    IL_298d:  ldc.i4.1
    IL_298e:  add
    IL_298f:  stloc.s    V_2
    IL_2991:  ldelem.i4
    IL_2992:  stloc.s    V_0
    IL_2994:  ldloc.s    V_3
    IL_2996:  ldloc.s    V_2
    IL_2998:  dup
    IL_2999:  ldc.i4.1
    IL_299a:  add
    IL_299b:  stloc.s    V_2
    IL_299d:  ldelem.i4
    IL_299e:  stloc.s    V_6
    IL_29a0:  ldloc.s    V_6
    IL_29a2:  ldc.i4.s   0
    IL_29a4:  ble        IL_1853

    IL_29a9:  ldloc.s    V_3
    IL_29ab:  ldloc.s    V_2
    IL_29ad:  ldc.i4.1
    IL_29ae:  sub
    IL_29af:  dup
    IL_29b0:  stloc.s    V_2
    IL_29b2:  ldloc.s    V_6
    IL_29b4:  ldc.i4.s   1
    IL_29b6:  sub
    IL_29b7:  stelem.i4
    IL_29b8:  ldloc.s    V_3
    IL_29ba:  ldloc.s    V_2
    IL_29bc:  ldc.i4.1
    IL_29bd:  sub
    IL_29be:  dup
    IL_29bf:  stloc.s    V_2
    IL_29c1:  ldloc.s    V_0
    IL_29c3:  ldc.i4.s   1
    IL_29c5:  sub
    IL_29c6:  stelem.i4
    IL_29c7:  ldloc.s    V_3
    IL_29c9:  ldloc.s    V_2
    IL_29cb:  ldc.i4.1
    IL_29cc:  sub
    IL_29cd:  dup
    IL_29ce:  stloc.s    V_2
    IL_29d0:  ldc.i4.s   39
    IL_29d2:  stelem.i4
    IL_29d3:  br         IL_1853

    IL_29d8:  ldarg.0
    IL_29d9:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_29de:  ldloc.s    V_3
    IL_29e0:  ldloc.s    V_2
    IL_29e2:  dup
    IL_29e3:  ldc.i4.1
    IL_29e4:  add
    IL_29e5:  stloc.s    V_2
    IL_29e7:  ldelem.i4
    IL_29e8:  stloc.s    V_0
    IL_29ea:  ldloc.s    V_3
    IL_29ec:  ldloc.s    V_2
    IL_29ee:  dup
    IL_29ef:  ldc.i4.1
    IL_29f0:  add
    IL_29f1:  stloc.s    V_2
    IL_29f3:  ldelem.i4
    IL_29f4:  stloc.s    V_6
    IL_29f6:  ldloc.s    V_6
    IL_29f8:  ldc.i4.s   0
    IL_29fa:  ble        IL_1932

    IL_29ff:  ldloc.s    V_3
    IL_2a01:  ldloc.s    V_2
    IL_2a03:  ldc.i4.1
    IL_2a04:  sub
    IL_2a05:  dup
    IL_2a06:  stloc.s    V_2
    IL_2a08:  ldloc.s    V_6
    IL_2a0a:  ldc.i4.s   1
    IL_2a0c:  sub
    IL_2a0d:  stelem.i4
    IL_2a0e:  ldloc.s    V_3
    IL_2a10:  ldloc.s    V_2
    IL_2a12:  ldc.i4.1
    IL_2a13:  sub
    IL_2a14:  dup
    IL_2a15:  stloc.s    V_2
    IL_2a17:  ldloc.s    V_0
    IL_2a19:  ldc.i4.s   1
    IL_2a1b:  sub
    IL_2a1c:  stelem.i4
    IL_2a1d:  ldloc.s    V_3
    IL_2a1f:  ldloc.s    V_2
    IL_2a21:  ldc.i4.1
    IL_2a22:  sub
    IL_2a23:  dup
    IL_2a24:  stloc.s    V_2
    IL_2a26:  ldc.i4.s   40
    IL_2a28:  stelem.i4
    IL_2a29:  br         IL_1932

    IL_2a2e:  ldarg.0
    IL_2a2f:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2a34:  ldloc.s    V_3
    IL_2a36:  ldloc.s    V_2
    IL_2a38:  dup
    IL_2a39:  ldc.i4.1
    IL_2a3a:  add
    IL_2a3b:  stloc.s    V_2
    IL_2a3d:  ldelem.i4
    IL_2a3e:  stloc.s    V_0
    IL_2a40:  ldloc.s    V_3
    IL_2a42:  ldloc.s    V_2
    IL_2a44:  dup
    IL_2a45:  ldc.i4.1
    IL_2a46:  add
    IL_2a47:  stloc.s    V_2
    IL_2a49:  ldelem.i4
    IL_2a4a:  stloc.s    V_6
    IL_2a4c:  ldloc.s    V_6
    IL_2a4e:  ldc.i4.s   0
    IL_2a50:  ble        IL_1a4e

    IL_2a55:  ldloc.s    V_3
    IL_2a57:  ldloc.s    V_2
    IL_2a59:  ldc.i4.1
    IL_2a5a:  sub
    IL_2a5b:  dup
    IL_2a5c:  stloc.s    V_2
    IL_2a5e:  ldloc.s    V_6
    IL_2a60:  ldc.i4.s   1
    IL_2a62:  sub
    IL_2a63:  stelem.i4
    IL_2a64:  ldloc.s    V_3
    IL_2a66:  ldloc.s    V_2
    IL_2a68:  ldc.i4.1
    IL_2a69:  sub
    IL_2a6a:  dup
    IL_2a6b:  stloc.s    V_2
    IL_2a6d:  ldloc.s    V_0
    IL_2a6f:  ldc.i4.s   1
    IL_2a71:  sub
    IL_2a72:  stelem.i4
    IL_2a73:  ldloc.s    V_3
    IL_2a75:  ldloc.s    V_2
    IL_2a77:  ldc.i4.1
    IL_2a78:  sub
    IL_2a79:  dup
    IL_2a7a:  stloc.s    V_2
    IL_2a7c:  ldc.i4.s   41
    IL_2a7e:  stelem.i4
    IL_2a7f:  br         IL_1a4e

    IL_2a84:  ldarg.0
    IL_2a85:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2a8a:  ldloc.s    V_5
    IL_2a8c:  ldloc.s    V_4
    IL_2a8e:  dup
    IL_2a8f:  ldc.i4.1
    IL_2a90:  add
    IL_2a91:  stloc.s    V_4
    IL_2a93:  ldelem.i4
    IL_2a94:  ldc.i4.s   1
    IL_2a96:  sub
    IL_2a97:  dup
    IL_2a98:  stloc.s    V_6
    IL_2a9a:  ldc.i4.s   0
    IL_2a9c:  blt.s      IL_2ac7

    IL_2a9e:  ldloc.s    V_5
    IL_2aa0:  ldloc.s    V_4
    IL_2aa2:  dup
    IL_2aa3:  ldc.i4.1
    IL_2aa4:  add
    IL_2aa5:  stloc.s    V_4
    IL_2aa7:  ldelem.i4
    IL_2aa8:  stloc.s    V_0
    IL_2aaa:  ldloc.s    V_3
    IL_2aac:  ldloc.s    V_2
    IL_2aae:  ldc.i4.1
    IL_2aaf:  sub
    IL_2ab0:  dup
    IL_2ab1:  stloc.s    V_2
    IL_2ab3:  ldloc.s    V_6
    IL_2ab5:  stelem.i4
    IL_2ab6:  ldloc.s    V_3
    IL_2ab8:  ldloc.s    V_2
    IL_2aba:  ldc.i4.1
    IL_2abb:  sub
    IL_2abc:  dup
    IL_2abd:  stloc.s    V_2
    IL_2abf:  ldc.i4.s   11
    IL_2ac1:  stelem.i4
    IL_2ac2:  br         IL_1b24

    IL_2ac7:  ldloc.s    V_5
    IL_2ac9:  ldloc.s    V_4
    IL_2acb:  ldloc.s    V_3
    IL_2acd:  ldloc.s    V_2
    IL_2acf:  dup
    IL_2ad0:  ldc.i4.1
    IL_2ad1:  add
    IL_2ad2:  stloc.s    V_2
    IL_2ad4:  ldelem.i4
    IL_2ad5:  stelem.i4
    IL_2ad6:  ldloc.s    V_5
    IL_2ad8:  ldloc.s    V_4
    IL_2ada:  ldc.i4.1
    IL_2adb:  sub
    IL_2adc:  dup
    IL_2add:  stloc.s    V_4
    IL_2adf:  ldloc.s    V_6
    IL_2ae1:  stelem.i4
    IL_2ae2:  br         IL_200e

    IL_2ae7:  ldarg.0
    IL_2ae8:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2aed:  ldloc.s    V_3
    IL_2aef:  ldloc.s    V_2
    IL_2af1:  dup
    IL_2af2:  ldc.i4.1
    IL_2af3:  add
    IL_2af4:  stloc.s    V_2
    IL_2af6:  ldelem.i4
    IL_2af7:  stloc.s    V_0
    IL_2af9:  ldloc.s    V_5
    IL_2afb:  ldloc.s    V_4
    IL_2afd:  dup
    IL_2afe:  ldc.i4.1
    IL_2aff:  add
    IL_2b00:  stloc.s    V_4
    IL_2b02:  ldelem.i4
    IL_2b03:  pop
    IL_2b04:  ldloc.s    V_3
    IL_2b06:  ldloc.s    V_2
    IL_2b08:  ldc.i4.1
    IL_2b09:  sub
    IL_2b0a:  dup
    IL_2b0b:  stloc.s    V_2
    IL_2b0d:  ldc.i4.s   46
    IL_2b0f:  stelem.i4
    IL_2b10:  br         IL_1bd3

    IL_2b15:  ldarg.0
    IL_2b16:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2b1b:  ldloc.s    V_5
    IL_2b1d:  ldloc.s    V_4
    IL_2b1f:  ldc.i4.1
    IL_2b20:  sub
    IL_2b21:  dup
    IL_2b22:  stloc.s    V_4
    IL_2b24:  ldloc.s    V_3
    IL_2b26:  ldloc.s    V_2
    IL_2b28:  dup
    IL_2b29:  ldc.i4.1
    IL_2b2a:  add
    IL_2b2b:  stloc.s    V_2
    IL_2b2d:  ldelem.i4
    IL_2b2e:  stelem.i4
    IL_2b2f:  br         IL_200e

    IL_2b34:  ldarg.0
    IL_2b35:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2b3a:  ldloc.s    V_5
    IL_2b3c:  ldloc.s    V_4
    IL_2b3e:  dup
    IL_2b3f:  ldc.i4.1
    IL_2b40:  add
    IL_2b41:  stloc.s    V_4
    IL_2b43:  ldelem.i4
    IL_2b44:  ldc.i4.s   1
    IL_2b46:  sub
    IL_2b47:  dup
    IL_2b48:  stloc.s    V_6
    IL_2b4a:  ldc.i4.s   0
    IL_2b4c:  blt.s      IL_2b77

    IL_2b4e:  ldloc.s    V_5
    IL_2b50:  ldloc.s    V_4
    IL_2b52:  dup
    IL_2b53:  ldc.i4.1
    IL_2b54:  add
    IL_2b55:  stloc.s    V_4
    IL_2b57:  ldelem.i4
    IL_2b58:  stloc.s    V_0
    IL_2b5a:  ldloc.s    V_3
    IL_2b5c:  ldloc.s    V_2
    IL_2b5e:  ldc.i4.1
    IL_2b5f:  sub
    IL_2b60:  dup
    IL_2b61:  stloc.s    V_2
    IL_2b63:  ldloc.s    V_6
    IL_2b65:  stelem.i4
    IL_2b66:  ldloc.s    V_3
    IL_2b68:  ldloc.s    V_2
    IL_2b6a:  ldc.i4.1
    IL_2b6b:  sub
    IL_2b6c:  dup
    IL_2b6d:  stloc.s    V_2
    IL_2b6f:  ldc.i4.s   11
    IL_2b71:  stelem.i4
    IL_2b72:  br         IL_1ca9

    IL_2b77:  ldloc.s    V_5
    IL_2b79:  ldloc.s    V_4
    IL_2b7b:  ldloc.s    V_3
    IL_2b7d:  ldloc.s    V_2
    IL_2b7f:  dup
    IL_2b80:  ldc.i4.1
    IL_2b81:  add
    IL_2b82:  stloc.s    V_2
    IL_2b84:  ldelem.i4
    IL_2b85:  stelem.i4
    IL_2b86:  ldloc.s    V_5
    IL_2b88:  ldloc.s    V_4
    IL_2b8a:  ldc.i4.1
    IL_2b8b:  sub
    IL_2b8c:  dup
    IL_2b8d:  stloc.s    V_4
    IL_2b8f:  ldloc.s    V_6
    IL_2b91:  stelem.i4
    IL_2b92:  br         IL_200e

    IL_2b97:  ldarg.0
    IL_2b98:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2b9d:  ldloc.s    V_3
    IL_2b9f:  ldloc.s    V_2
    IL_2ba1:  dup
    IL_2ba2:  ldc.i4.1
    IL_2ba3:  add
    IL_2ba4:  stloc.s    V_2
    IL_2ba6:  ldelem.i4
    IL_2ba7:  stloc.s    V_0
    IL_2ba9:  br         IL_1ea7

    IL_2bae:  ldarg.0
    IL_2baf:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2bb4:  ldloc.s    V_3
    IL_2bb6:  ldloc.s    V_2
    IL_2bb8:  dup
    IL_2bb9:  ldc.i4.1
    IL_2bba:  add
    IL_2bbb:  stloc.s    V_2
    IL_2bbd:  ldelem.i4
    IL_2bbe:  stloc.s    V_0
    IL_2bc0:  ldloc.s    V_3
    IL_2bc2:  ldloc.s    V_2
    IL_2bc4:  dup
    IL_2bc5:  ldc.i4.1
    IL_2bc6:  add
    IL_2bc7:  stloc.s    V_2
    IL_2bc9:  ldelem.i4
    IL_2bca:  stloc.s    V_6
    IL_2bcc:  ldloc.s    V_6
    IL_2bce:  ldc.i4.s   0
    IL_2bd0:  ble        IL_1e60

    IL_2bd5:  ldloc.s    V_3
    IL_2bd7:  ldloc.s    V_2
    IL_2bd9:  ldc.i4.1
    IL_2bda:  sub
    IL_2bdb:  dup
    IL_2bdc:  stloc.s    V_2
    IL_2bde:  ldloc.s    V_6
    IL_2be0:  ldc.i4.s   1
    IL_2be2:  sub
    IL_2be3:  stelem.i4
    IL_2be4:  ldloc.s    V_3
    IL_2be6:  ldloc.s    V_2
    IL_2be8:  ldc.i4.1
    IL_2be9:  sub
    IL_2bea:  dup
    IL_2beb:  stloc.s    V_2
    IL_2bed:  ldloc.s    V_0
    IL_2bef:  ldc.i4.s   1
    IL_2bf1:  sub
    IL_2bf2:  stelem.i4
    IL_2bf3:  ldloc.s    V_3
    IL_2bf5:  ldloc.s    V_2
    IL_2bf7:  ldc.i4.1
    IL_2bf8:  sub
    IL_2bf9:  dup
    IL_2bfa:  stloc.s    V_2
    IL_2bfc:  ldc.i4.s   50
    IL_2bfe:  stelem.i4
    IL_2bff:  br         IL_1e60

    IL_2c04:  ldarg.0
    IL_2c05:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2c0a:  ldloc.s    V_3
    IL_2c0c:  ldloc.s    V_2
    IL_2c0e:  dup
    IL_2c0f:  ldc.i4.1
    IL_2c10:  add
    IL_2c11:  stloc.s    V_2
    IL_2c13:  ldelem.i4
    IL_2c14:  stloc.s    V_0
    IL_2c16:  ldloc.s    V_3
    IL_2c18:  ldloc.s    V_2
    IL_2c1a:  dup
    IL_2c1b:  ldc.i4.1
    IL_2c1c:  add
    IL_2c1d:  stloc.s    V_2
    IL_2c1f:  ldelem.i4
    IL_2c20:  stloc.s    V_6
    IL_2c22:  ldloc.s    V_6
    IL_2c24:  ldc.i4.s   0
    IL_2c26:  ble        IL_1fa4

    IL_2c2b:  ldloc.s    V_3
    IL_2c2d:  ldloc.s    V_2
    IL_2c2f:  ldc.i4.1
    IL_2c30:  sub
    IL_2c31:  dup
    IL_2c32:  stloc.s    V_2
    IL_2c34:  ldloc.s    V_6
    IL_2c36:  ldc.i4.s   1
    IL_2c38:  sub
    IL_2c39:  stelem.i4
    IL_2c3a:  ldloc.s    V_3
    IL_2c3c:  ldloc.s    V_2
    IL_2c3e:  ldc.i4.1
    IL_2c3f:  sub
    IL_2c40:  dup
    IL_2c41:  stloc.s    V_2
    IL_2c43:  ldloc.s    V_0
    IL_2c45:  ldc.i4.s   1
    IL_2c47:  sub
    IL_2c48:  stelem.i4
    IL_2c49:  ldloc.s    V_3
    IL_2c4b:  ldloc.s    V_2
    IL_2c4d:  ldc.i4.1
    IL_2c4e:  sub
    IL_2c4f:  dup
    IL_2c50:  stloc.s    V_2
    IL_2c52:  ldc.i4.s   51
    IL_2c54:  stelem.i4
    IL_2c55:  br         IL_1fa4
  } // end of method MainCapturePatternRunner1::Go

  .method public virtual instance bool  FindFirstChar() cil managed
  {
    // Code size       110 (0x6e)
    .maxstack  8
    .locals init (int32 V_0,
             string V_1,
             int32 V_2,
             int32 V_3)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextpos
    IL_0006:  stloc.s    V_0
    IL_0008:  ldarg.0
    IL_0009:  ldfld      string [System]System.Text.RegularExpressions.RegexRunner::runtext
    IL_000e:  stloc.s    V_1
    IL_0010:  ldarg.0
    IL_0011:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextend
    IL_0016:  ldloc.s    V_0
    IL_0018:  sub
    IL_0019:  stloc.s    V_3
    IL_001b:  ldloc.s    V_3
    IL_001d:  ldc.i4.s   0
    IL_001f:  ble        IL_006b

    IL_0024:  ldloc.s    V_3
    IL_0026:  ldc.i4.s   1
    IL_0028:  sub
    IL_0029:  stloc.s    V_3
    IL_002b:  ldloc.s    V_1
    IL_002d:  ldloc.s    V_0
    IL_002f:  dup
    IL_0030:  ldc.i4.s   1
    IL_0032:  add
    IL_0033:  stloc.s    V_0
    IL_0035:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_003a:  ldstr      bytearray (00 00 04 00 00 00 24 00 25 00 7D 00 7E 00 )       // ......$.%.}.~.
    IL_003f:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0044:  brtrue     IL_0059

    IL_0049:  ldloc.s    V_3
    IL_004b:  ldc.i4.s   0
    IL_004d:  bgt        IL_0024

    IL_0052:  ldc.i4.s   0
    IL_0054:  br         IL_0062

    IL_0059:  ldloc.s    V_0
    IL_005b:  ldc.i4.s   1
    IL_005d:  sub
    IL_005e:  stloc.s    V_0
    IL_0060:  ldc.i4.s   1
    IL_0062:  ldarg.0
    IL_0063:  ldloc.s    V_0
    IL_0065:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextpos
    IL_006a:  ret

    IL_006b:  ldc.i4.s   0
    IL_006d:  ret
  } // end of method MainCapturePatternRunner1::FindFirstChar

  .method public virtual instance void  InitTrackCount() cil managed
  {
    // Code size       9 (0x9)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.s   98
    IL_0003:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtrackcount
    IL_0008:  ret
  } // end of method MainCapturePatternRunner1::InitTrackCount

  .method public specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [System]System.Text.RegularExpressions.RegexRunner::.ctor()
    IL_0006:  ret
  } // end of method MainCapturePatternRunner1::.ctor

} // end of class Xeora.Web.RegularExpressions.MainCapturePatternRunner1

.class private auto ansi Xeora.Web.RegularExpressions.MainCapturePatternFactory1
       extends [System]System.Text.RegularExpressions.RegexRunnerFactory
{
  .method public virtual instance class [System]System.Text.RegularExpressions.RegexRunner 
          CreateInstance() cil managed
  {
    // Code size       6 (0x6)
    .maxstack  1
    IL_0000:  newobj     instance void Xeora.Web.RegularExpressions.MainCapturePatternRunner1::.ctor()
    IL_0005:  ret
  } // end of method MainCapturePatternFactory1::CreateInstance

  .method public specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [System]System.Text.RegularExpressions.RegexRunnerFactory::.ctor()
    IL_0006:  ret
  } // end of method MainCapturePatternFactory1::.ctor

} // end of class Xeora.Web.RegularExpressions.MainCapturePatternFactory1

.class public auto ansi Xeora.Web.RegularExpressions.MainCapturePattern
       extends [System]System.Text.RegularExpressions.Regex
{
  .method public specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       68 (0x44)
    .maxstack  4
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [System]System.Text.RegularExpressions.Regex::.ctor()
    IL_0006:  ldarg.0
    IL_0007:  ldstr      "\\$((([#]+|[\\^\\-\\+\\*\\~])\?(\\w+)|@[#\\-]*(\\w+\\.)[\\w+\\."
    + "]+)\\$|[\\.\\w\\-]+\\:\\{|\\w(\\#\\d+(\\+)\?)\?(\\[[\\.\\w\\-]+\\])\?\\:([\\/\\.\\w\\-]+\\$|[\\."
    + "\\w\\-]+\\:\\{|[\\.\\w\\-]+\\\?[\\.\\w\\-]+(\\,((\\|)\?([#\\.\\^\\-\\+\\*\\~]*([\\w+][^\\$]*)|"
    + "\\=([\\S+][^\\$]*)|@[#\\-]*(\\w+\\.)[\\w+\\.]+)\?)*)\?\\$))|\\}\\:[\\.\\w\\-]+\\:\\{|\\}\\:"
    + "[\\.\\w\\-]+\\$"
    IL_000c:  stfld      string [System]System.Text.RegularExpressions.Regex::pattern
    IL_0011:  ldarg.0
    IL_0012:  ldc.i4.s   2
    IL_0014:  stfld      valuetype [System]System.Text.RegularExpressions.RegexOptions [System]System.Text.RegularExpressions.Regex::roptions
    IL_0019:  ldarg.0
    IL_001a:  ldc.i4     0xffffd8f0
    IL_001f:  conv.i8
    IL_0020:  call       valuetype [mscorlib]System.TimeSpan [mscorlib]System.TimeSpan::FromTicks(int64)
    IL_0025:  stfld      valuetype [mscorlib]System.TimeSpan [System]System.Text.RegularExpressions.Regex::internalMatchTimeout
    IL_002a:  ldarg.0
    IL_002b:  newobj     instance void Xeora.Web.RegularExpressions.MainCapturePatternFactory1::.ctor()
    IL_0030:  stfld      class [System]System.Text.RegularExpressions.RegexRunnerFactory [System]System.Text.RegularExpressions.Regex::factory
    IL_0035:  ldarg.0
    IL_0036:  ldc.i4.s   17
    IL_0038:  stfld      int32 [System]System.Text.RegularExpressions.Regex::capsize
    IL_003d:  ldarg.0
    IL_003e:  call       instance void [System]System.Text.RegularExpressions.Regex::InitializeReferences()
    IL_0043:  ret
  } // end of method MainCapturePattern::.ctor

  .method public specialname rtspecialname 
          instance void  .ctor(valuetype [mscorlib]System.TimeSpan A_1) cil managed
  {
    // Code size       20 (0x14)
    .maxstack  4
    IL_0000:  ldarg.0
    IL_0001:  call       instance void Xeora.Web.RegularExpressions.MainCapturePattern::.ctor()
    IL_0006:  ldarg.1
    IL_0007:  call       void [System]System.Text.RegularExpressions.Regex::ValidateMatchTimeout(valuetype [mscorlib]System.TimeSpan)
    IL_000c:  ldarg.0
    IL_000d:  ldarg.1
    IL_000e:  stfld      valuetype [mscorlib]System.TimeSpan [System]System.Text.RegularExpressions.Regex::internalMatchTimeout
    IL_0013:  ret
  } // end of method MainCapturePattern::.ctor

} // end of class Xeora.Web.RegularExpressions.MainCapturePattern

.class private auto ansi Xeora.Web.RegularExpressions.BracketedControllerOpenPatternRunner2
       extends [System]System.Text.RegularExpressions.RegexRunner
{
  .method public virtual instance void  Go() cil managed
  {
    // Code size       3507 (0xdb3)
    .maxstack  105
    .locals init (int32 V_0,
             string V_1,
             int32 V_2,
             int32[] V_3,
             int32 V_4,
             int32[] V_5,
             int32 V_6,
             int32 V_7,
             int32 V_8,
             int32 V_9,
             int32 V_10,
             int32 V_11)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string [System]System.Text.RegularExpressions.RegexRunner::runtext
    IL_0006:  stloc.s    V_1
    IL_0008:  ldarg.0
    IL_0009:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextstart
    IL_000e:  stloc.s    V_11
    IL_0010:  ldarg.0
    IL_0011:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextbeg
    IL_0016:  stloc.s    V_9
    IL_0018:  ldarg.0
    IL_0019:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextend
    IL_001e:  stloc.s    V_10
    IL_0020:  ldarg.0
    IL_0021:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextpos
    IL_0026:  stloc.s    V_0
    IL_0028:  ldarg.0
    IL_0029:  ldfld      int32[] [System]System.Text.RegularExpressions.RegexRunner::runtrack
    IL_002e:  stloc.s    V_3
    IL_0030:  ldarg.0
    IL_0031:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtrackpos
    IL_0036:  stloc.s    V_2
    IL_0038:  ldarg.0
    IL_0039:  ldfld      int32[] [System]System.Text.RegularExpressions.RegexRunner::runstack
    IL_003e:  stloc.s    V_5
    IL_0040:  ldarg.0
    IL_0041:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runstackpos
    IL_0046:  stloc.s    V_4
    IL_0048:  ldarg.0
    IL_0049:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_004e:  ldloc.s    V_3
    IL_0050:  ldloc.s    V_2
    IL_0052:  ldc.i4.1
    IL_0053:  sub
    IL_0054:  dup
    IL_0055:  stloc.s    V_2
    IL_0057:  ldloc.s    V_0
    IL_0059:  stelem.i4
    IL_005a:  ldloc.s    V_3
    IL_005c:  ldloc.s    V_2
    IL_005e:  ldc.i4.1
    IL_005f:  sub
    IL_0060:  dup
    IL_0061:  stloc.s    V_2
    IL_0063:  ldc.i4.s   0
    IL_0065:  stelem.i4
    IL_0066:  ldarg.0
    IL_0067:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_006c:  ldloc.s    V_5
    IL_006e:  ldloc.s    V_4
    IL_0070:  ldc.i4.1
    IL_0071:  sub
    IL_0072:  dup
    IL_0073:  stloc.s    V_4
    IL_0075:  ldloc.s    V_0
    IL_0077:  stelem.i4
    IL_0078:  ldloc.s    V_3
    IL_007a:  ldloc.s    V_2
    IL_007c:  ldc.i4.1
    IL_007d:  sub
    IL_007e:  dup
    IL_007f:  stloc.s    V_2
    IL_0081:  ldc.i4.s   1
    IL_0083:  stelem.i4
    IL_0084:  ldarg.0
    IL_0085:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_008a:  ldloc.s    V_0
    IL_008c:  ldloc.s    V_10
    IL_008e:  bge        IL_09ff

    IL_0093:  ldloc.s    V_1
    IL_0095:  ldloc.s    V_0
    IL_0097:  dup
    IL_0098:  ldc.i4.s   1
    IL_009a:  add
    IL_009b:  stloc.s    V_0
    IL_009d:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_00a2:  ldc.i4.s   36
    IL_00a4:  bne.un     IL_09ff

    IL_00a9:  ldarg.0
    IL_00aa:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_00af:  ldloc.s    V_5
    IL_00b1:  ldloc.s    V_4
    IL_00b3:  ldc.i4.1
    IL_00b4:  sub
    IL_00b5:  dup
    IL_00b6:  stloc.s    V_4
    IL_00b8:  ldloc.s    V_0
    IL_00ba:  stelem.i4
    IL_00bb:  ldloc.s    V_3
    IL_00bd:  ldloc.s    V_2
    IL_00bf:  ldc.i4.1
    IL_00c0:  sub
    IL_00c1:  dup
    IL_00c2:  stloc.s    V_2
    IL_00c4:  ldc.i4.s   1
    IL_00c6:  stelem.i4
    IL_00c7:  ldarg.0
    IL_00c8:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_00cd:  ldloc.s    V_3
    IL_00cf:  ldloc.s    V_2
    IL_00d1:  ldc.i4.1
    IL_00d2:  sub
    IL_00d3:  dup
    IL_00d4:  stloc.s    V_2
    IL_00d6:  ldloc.s    V_0
    IL_00d8:  stelem.i4
    IL_00d9:  ldloc.s    V_3
    IL_00db:  ldloc.s    V_2
    IL_00dd:  ldc.i4.1
    IL_00de:  sub
    IL_00df:  dup
    IL_00e0:  stloc.s    V_2
    IL_00e2:  ldc.i4.s   2
    IL_00e4:  stelem.i4
    IL_00e5:  ldarg.0
    IL_00e6:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_00eb:  ldloc.s    V_5
    IL_00ed:  ldloc.s    V_4
    IL_00ef:  ldc.i4.1
    IL_00f0:  sub
    IL_00f1:  dup
    IL_00f2:  stloc.s    V_4
    IL_00f4:  ldloc.s    V_0
    IL_00f6:  stelem.i4
    IL_00f7:  ldloc.s    V_3
    IL_00f9:  ldloc.s    V_2
    IL_00fb:  ldc.i4.1
    IL_00fc:  sub
    IL_00fd:  dup
    IL_00fe:  stloc.s    V_2
    IL_0100:  ldc.i4.s   1
    IL_0102:  stelem.i4
    IL_0103:  ldarg.0
    IL_0104:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0109:  ldc.i4.s   1
    IL_010b:  ldloc.s    V_10
    IL_010d:  ldloc.s    V_0
    IL_010f:  sub
    IL_0110:  bgt        IL_09ff

    IL_0115:  ldloc.s    V_0
    IL_0117:  ldc.i4.s   1
    IL_0119:  add
    IL_011a:  stloc.s    V_0
    IL_011c:  ldc.i4.s   1
    IL_011e:  stloc.s    V_6
    IL_0120:  ldloc.s    V_1
    IL_0122:  ldloc.s    V_0
    IL_0124:  ldloc.s    V_6
    IL_0126:  dup
    IL_0127:  ldc.i4.s   1
    IL_0129:  sub
    IL_012a:  stloc.s    V_6
    IL_012c:  sub
    IL_012d:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0132:  ldstr      bytearray (00 00 00 00 0A 00 00 00 02 00 04 00 05 00 03 00 
                                    01 00 06 00 09 00 13 00 00 00 ) 
    IL_0137:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_013c:  brfalse    IL_09ff

    IL_0141:  ldloc.s    V_6
    IL_0143:  ldc.i4.s   0
    IL_0145:  bgt        IL_0120

    IL_014a:  ldarg.0
    IL_014b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0150:  ldloc.s    V_10
    IL_0152:  ldloc.s    V_0
    IL_0154:  sub
    IL_0155:  dup
    IL_0156:  stloc.s    V_7
    IL_0158:  ldc.i4.s   1
    IL_015a:  add
    IL_015b:  stloc.s    V_6
    IL_015d:  ldloc.s    V_6
    IL_015f:  ldc.i4.s   1
    IL_0161:  sub
    IL_0162:  dup
    IL_0163:  stloc.s    V_6
    IL_0165:  ldc.i4.s   0
    IL_0167:  ble        IL_0191

    IL_016c:  ldloc.s    V_1
    IL_016e:  ldloc.s    V_0
    IL_0170:  dup
    IL_0171:  ldc.i4.s   1
    IL_0173:  add
    IL_0174:  stloc.s    V_0
    IL_0176:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_017b:  ldstr      bytearray (00 00 00 00 0A 00 00 00 02 00 04 00 05 00 03 00 
                                    01 00 06 00 09 00 13 00 00 00 ) 
    IL_0180:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0185:  brtrue     IL_015d

    IL_018a:  ldloc.s    V_0
    IL_018c:  ldc.i4.s   1
    IL_018e:  sub
    IL_018f:  stloc.s    V_0
    IL_0191:  ldloc.s    V_7
    IL_0193:  ldloc.s    V_6
    IL_0195:  ble.s      IL_01c4

    IL_0197:  ldloc.s    V_3
    IL_0199:  ldloc.s    V_2
    IL_019b:  ldc.i4.1
    IL_019c:  sub
    IL_019d:  dup
    IL_019e:  stloc.s    V_2
    IL_01a0:  ldloc.s    V_7
    IL_01a2:  ldloc.s    V_6
    IL_01a4:  sub
    IL_01a5:  ldc.i4.s   1
    IL_01a7:  sub
    IL_01a8:  stelem.i4
    IL_01a9:  ldloc.s    V_3
    IL_01ab:  ldloc.s    V_2
    IL_01ad:  ldc.i4.1
    IL_01ae:  sub
    IL_01af:  dup
    IL_01b0:  stloc.s    V_2
    IL_01b2:  ldloc.s    V_0
    IL_01b4:  ldc.i4.s   1
    IL_01b6:  sub
    IL_01b7:  stelem.i4
    IL_01b8:  ldloc.s    V_3
    IL_01ba:  ldloc.s    V_2
    IL_01bc:  ldc.i4.1
    IL_01bd:  sub
    IL_01be:  dup
    IL_01bf:  stloc.s    V_2
    IL_01c1:  ldc.i4.s   3
    IL_01c3:  stelem.i4
    IL_01c4:  ldarg.0
    IL_01c5:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_01ca:  ldloc.s    V_5
    IL_01cc:  ldloc.s    V_4
    IL_01ce:  dup
    IL_01cf:  ldc.i4.1
    IL_01d0:  add
    IL_01d1:  stloc.s    V_4
    IL_01d3:  ldelem.i4
    IL_01d4:  stloc.s    V_6
    IL_01d6:  ldarg.0
    IL_01d7:  ldc.i4.s   5
    IL_01d9:  ldloc.s    V_6
    IL_01db:  ldloc.s    V_0
    IL_01dd:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_01e2:  ldloc.s    V_3
    IL_01e4:  ldloc.s    V_2
    IL_01e6:  ldc.i4.1
    IL_01e7:  sub
    IL_01e8:  dup
    IL_01e9:  stloc.s    V_2
    IL_01eb:  ldloc.s    V_6
    IL_01ed:  stelem.i4
    IL_01ee:  ldloc.s    V_3
    IL_01f0:  ldloc.s    V_2
    IL_01f2:  ldc.i4.1
    IL_01f3:  sub
    IL_01f4:  dup
    IL_01f5:  stloc.s    V_2
    IL_01f7:  ldc.i4.s   4
    IL_01f9:  stelem.i4
    IL_01fa:  ldarg.0
    IL_01fb:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0200:  br         IL_0948

    IL_0205:  ldarg.0
    IL_0206:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_020b:  ldloc.s    V_5
    IL_020d:  ldloc.s    V_4
    IL_020f:  ldc.i4.1
    IL_0210:  sub
    IL_0211:  dup
    IL_0212:  stloc.s    V_4
    IL_0214:  ldloc.s    V_0
    IL_0216:  stelem.i4
    IL_0217:  ldloc.s    V_3
    IL_0219:  ldloc.s    V_2
    IL_021b:  ldc.i4.1
    IL_021c:  sub
    IL_021d:  dup
    IL_021e:  stloc.s    V_2
    IL_0220:  ldc.i4.s   1
    IL_0222:  stelem.i4
    IL_0223:  ldarg.0
    IL_0224:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0229:  ldloc.s    V_0
    IL_022b:  ldloc.s    V_10
    IL_022d:  bge        IL_09ff

    IL_0232:  ldloc.s    V_1
    IL_0234:  ldloc.s    V_0
    IL_0236:  dup
    IL_0237:  ldc.i4.s   1
    IL_0239:  add
    IL_023a:  stloc.s    V_0
    IL_023c:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0241:  ldstr      bytearray (00 00 00 00 0A 00 00 00 02 00 04 00 05 00 03 00 
                                    01 00 06 00 09 00 13 00 00 00 ) 
    IL_0246:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_024b:  brfalse    IL_09ff

    IL_0250:  ldarg.0
    IL_0251:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0256:  ldloc.s    V_5
    IL_0258:  ldloc.s    V_4
    IL_025a:  dup
    IL_025b:  ldc.i4.1
    IL_025c:  add
    IL_025d:  stloc.s    V_4
    IL_025f:  ldelem.i4
    IL_0260:  stloc.s    V_6
    IL_0262:  ldarg.0
    IL_0263:  ldc.i4.s   6
    IL_0265:  ldloc.s    V_6
    IL_0267:  ldloc.s    V_0
    IL_0269:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_026e:  ldloc.s    V_3
    IL_0270:  ldloc.s    V_2
    IL_0272:  ldc.i4.1
    IL_0273:  sub
    IL_0274:  dup
    IL_0275:  stloc.s    V_2
    IL_0277:  ldloc.s    V_6
    IL_0279:  stelem.i4
    IL_027a:  ldloc.s    V_3
    IL_027c:  ldloc.s    V_2
    IL_027e:  ldc.i4.1
    IL_027f:  sub
    IL_0280:  dup
    IL_0281:  stloc.s    V_2
    IL_0283:  ldc.i4.s   4
    IL_0285:  stelem.i4
    IL_0286:  ldarg.0
    IL_0287:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_028c:  ldloc.s    V_5
    IL_028e:  ldloc.s    V_4
    IL_0290:  ldc.i4.1
    IL_0291:  sub
    IL_0292:  dup
    IL_0293:  stloc.s    V_4
    IL_0295:  ldc.i4.s   -1
    IL_0297:  stelem.i4
    IL_0298:  ldloc.s    V_5
    IL_029a:  ldloc.s    V_4
    IL_029c:  ldc.i4.1
    IL_029d:  sub
    IL_029e:  dup
    IL_029f:  stloc.s    V_4
    IL_02a1:  ldc.i4.s   0
    IL_02a3:  stelem.i4
    IL_02a4:  ldloc.s    V_3
    IL_02a6:  ldloc.s    V_2
    IL_02a8:  ldc.i4.1
    IL_02a9:  sub
    IL_02aa:  dup
    IL_02ab:  stloc.s    V_2
    IL_02ad:  ldc.i4.s   5
    IL_02af:  stelem.i4
    IL_02b0:  ldarg.0
    IL_02b1:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_02b6:  br         IL_0540

    IL_02bb:  ldarg.0
    IL_02bc:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_02c1:  ldloc.s    V_5
    IL_02c3:  ldloc.s    V_4
    IL_02c5:  ldc.i4.1
    IL_02c6:  sub
    IL_02c7:  dup
    IL_02c8:  stloc.s    V_4
    IL_02ca:  ldloc.s    V_0
    IL_02cc:  stelem.i4
    IL_02cd:  ldloc.s    V_3
    IL_02cf:  ldloc.s    V_2
    IL_02d1:  ldc.i4.1
    IL_02d2:  sub
    IL_02d3:  dup
    IL_02d4:  stloc.s    V_2
    IL_02d6:  ldc.i4.s   1
    IL_02d8:  stelem.i4
    IL_02d9:  ldarg.0
    IL_02da:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_02df:  ldloc.s    V_0
    IL_02e1:  ldloc.s    V_10
    IL_02e3:  bge        IL_09ff

    IL_02e8:  ldloc.s    V_1
    IL_02ea:  ldloc.s    V_0
    IL_02ec:  dup
    IL_02ed:  ldc.i4.s   1
    IL_02ef:  add
    IL_02f0:  stloc.s    V_0
    IL_02f2:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_02f7:  ldc.i4.s   35
    IL_02f9:  bne.un     IL_09ff

    IL_02fe:  ldarg.0
    IL_02ff:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0304:  ldc.i4.s   1
    IL_0306:  ldloc.s    V_10
    IL_0308:  ldloc.s    V_0
    IL_030a:  sub
    IL_030b:  bgt        IL_09ff

    IL_0310:  ldloc.s    V_0
    IL_0312:  ldc.i4.s   1
    IL_0314:  add
    IL_0315:  stloc.s    V_0
    IL_0317:  ldc.i4.s   1
    IL_0319:  stloc.s    V_6
    IL_031b:  ldloc.s    V_1
    IL_031d:  ldloc.s    V_0
    IL_031f:  ldloc.s    V_6
    IL_0321:  dup
    IL_0322:  ldc.i4.s   1
    IL_0324:  sub
    IL_0325:  stloc.s    V_6
    IL_0327:  sub
    IL_0328:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_032d:  ldstr      bytearray (00 00 00 00 01 00 09 00 ) 
    IL_0332:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0337:  brfalse    IL_09ff

    IL_033c:  ldloc.s    V_6
    IL_033e:  ldc.i4.s   0
    IL_0340:  bgt        IL_031b

    IL_0345:  ldarg.0
    IL_0346:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_034b:  ldloc.s    V_10
    IL_034d:  ldloc.s    V_0
    IL_034f:  sub
    IL_0350:  dup
    IL_0351:  stloc.s    V_7
    IL_0353:  ldc.i4.s   1
    IL_0355:  add
    IL_0356:  stloc.s    V_6
    IL_0358:  ldloc.s    V_6
    IL_035a:  ldc.i4.s   1
    IL_035c:  sub
    IL_035d:  dup
    IL_035e:  stloc.s    V_6
    IL_0360:  ldc.i4.s   0
    IL_0362:  ble        IL_038c

    IL_0367:  ldloc.s    V_1
    IL_0369:  ldloc.s    V_0
    IL_036b:  dup
    IL_036c:  ldc.i4.s   1
    IL_036e:  add
    IL_036f:  stloc.s    V_0
    IL_0371:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0376:  ldstr      bytearray (00 00 00 00 01 00 09 00 ) 
    IL_037b:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0380:  brtrue     IL_0358

    IL_0385:  ldloc.s    V_0
    IL_0387:  ldc.i4.s   1
    IL_0389:  sub
    IL_038a:  stloc.s    V_0
    IL_038c:  ldloc.s    V_7
    IL_038e:  ldloc.s    V_6
    IL_0390:  ble.s      IL_03bf

    IL_0392:  ldloc.s    V_3
    IL_0394:  ldloc.s    V_2
    IL_0396:  ldc.i4.1
    IL_0397:  sub
    IL_0398:  dup
    IL_0399:  stloc.s    V_2
    IL_039b:  ldloc.s    V_7
    IL_039d:  ldloc.s    V_6
    IL_039f:  sub
    IL_03a0:  ldc.i4.s   1
    IL_03a2:  sub
    IL_03a3:  stelem.i4
    IL_03a4:  ldloc.s    V_3
    IL_03a6:  ldloc.s    V_2
    IL_03a8:  ldc.i4.1
    IL_03a9:  sub
    IL_03aa:  dup
    IL_03ab:  stloc.s    V_2
    IL_03ad:  ldloc.s    V_0
    IL_03af:  ldc.i4.s   1
    IL_03b1:  sub
    IL_03b2:  stelem.i4
    IL_03b3:  ldloc.s    V_3
    IL_03b5:  ldloc.s    V_2
    IL_03b7:  ldc.i4.1
    IL_03b8:  sub
    IL_03b9:  dup
    IL_03ba:  stloc.s    V_2
    IL_03bc:  ldc.i4.s   6
    IL_03be:  stelem.i4
    IL_03bf:  ldarg.0
    IL_03c0:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_03c5:  ldloc.s    V_5
    IL_03c7:  ldloc.s    V_4
    IL_03c9:  ldc.i4.1
    IL_03ca:  sub
    IL_03cb:  dup
    IL_03cc:  stloc.s    V_4
    IL_03ce:  ldc.i4.s   -1
    IL_03d0:  stelem.i4
    IL_03d1:  ldloc.s    V_5
    IL_03d3:  ldloc.s    V_4
    IL_03d5:  ldc.i4.1
    IL_03d6:  sub
    IL_03d7:  dup
    IL_03d8:  stloc.s    V_4
    IL_03da:  ldc.i4.s   0
    IL_03dc:  stelem.i4
    IL_03dd:  ldloc.s    V_3
    IL_03df:  ldloc.s    V_2
    IL_03e1:  ldc.i4.1
    IL_03e2:  sub
    IL_03e3:  dup
    IL_03e4:  stloc.s    V_2
    IL_03e6:  ldc.i4.s   5
    IL_03e8:  stelem.i4
    IL_03e9:  ldarg.0
    IL_03ea:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_03ef:  br         IL_046d

    IL_03f4:  ldarg.0
    IL_03f5:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_03fa:  ldloc.s    V_5
    IL_03fc:  ldloc.s    V_4
    IL_03fe:  ldc.i4.1
    IL_03ff:  sub
    IL_0400:  dup
    IL_0401:  stloc.s    V_4
    IL_0403:  ldloc.s    V_0
    IL_0405:  stelem.i4
    IL_0406:  ldloc.s    V_3
    IL_0408:  ldloc.s    V_2
    IL_040a:  ldc.i4.1
    IL_040b:  sub
    IL_040c:  dup
    IL_040d:  stloc.s    V_2
    IL_040f:  ldc.i4.s   1
    IL_0411:  stelem.i4
    IL_0412:  ldarg.0
    IL_0413:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0418:  ldloc.s    V_0
    IL_041a:  ldloc.s    V_10
    IL_041c:  bge        IL_09ff

    IL_0421:  ldloc.s    V_1
    IL_0423:  ldloc.s    V_0
    IL_0425:  dup
    IL_0426:  ldc.i4.s   1
    IL_0428:  add
    IL_0429:  stloc.s    V_0
    IL_042b:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0430:  ldc.i4.s   43
    IL_0432:  bne.un     IL_09ff

    IL_0437:  ldarg.0
    IL_0438:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_043d:  ldloc.s    V_5
    IL_043f:  ldloc.s    V_4
    IL_0441:  dup
    IL_0442:  ldc.i4.1
    IL_0443:  add
    IL_0444:  stloc.s    V_4
    IL_0446:  ldelem.i4
    IL_0447:  stloc.s    V_6
    IL_0449:  ldarg.0
    IL_044a:  ldc.i4.s   3
    IL_044c:  ldloc.s    V_6
    IL_044e:  ldloc.s    V_0
    IL_0450:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_0455:  ldloc.s    V_3
    IL_0457:  ldloc.s    V_2
    IL_0459:  ldc.i4.1
    IL_045a:  sub
    IL_045b:  dup
    IL_045c:  stloc.s    V_2
    IL_045e:  ldloc.s    V_6
    IL_0460:  stelem.i4
    IL_0461:  ldloc.s    V_3
    IL_0463:  ldloc.s    V_2
    IL_0465:  ldc.i4.1
    IL_0466:  sub
    IL_0467:  dup
    IL_0468:  stloc.s    V_2
    IL_046a:  ldc.i4.s   4
    IL_046c:  stelem.i4
    IL_046d:  ldarg.0
    IL_046e:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0473:  ldloc.s    V_5
    IL_0475:  ldloc.s    V_4
    IL_0477:  dup
    IL_0478:  ldc.i4.1
    IL_0479:  add
    IL_047a:  stloc.s    V_4
    IL_047c:  ldelem.i4
    IL_047d:  stloc.s    V_6
    IL_047f:  ldloc.s    V_5
    IL_0481:  ldloc.s    V_4
    IL_0483:  dup
    IL_0484:  ldc.i4.1
    IL_0485:  add
    IL_0486:  stloc.s    V_4
    IL_0488:  ldelem.i4
    IL_0489:  dup
    IL_048a:  stloc.s    V_7
    IL_048c:  ldloc.s    V_3
    IL_048e:  ldloc.s    V_2
    IL_0490:  ldc.i4.1
    IL_0491:  sub
    IL_0492:  dup
    IL_0493:  stloc.s    V_2
    IL_0495:  ldloc.s    V_7
    IL_0497:  stelem.i4
    IL_0498:  ldloc.s    V_0
    IL_049a:  bne.un.s   IL_04a2

    IL_049c:  ldloc.s    V_6
    IL_049e:  ldc.i4.s   0
    IL_04a0:  bge.s      IL_04f2

    IL_04a2:  ldloc.s    V_6
    IL_04a4:  ldc.i4.s   1
    IL_04a6:  bge.s      IL_04f2

    IL_04a8:  ldloc.s    V_5
    IL_04aa:  ldloc.s    V_4
    IL_04ac:  ldc.i4.1
    IL_04ad:  sub
    IL_04ae:  dup
    IL_04af:  stloc.s    V_4
    IL_04b1:  ldloc.s    V_0
    IL_04b3:  stelem.i4
    IL_04b4:  ldloc.s    V_5
    IL_04b6:  ldloc.s    V_4
    IL_04b8:  ldc.i4.1
    IL_04b9:  sub
    IL_04ba:  dup
    IL_04bb:  stloc.s    V_4
    IL_04bd:  ldloc.s    V_6
    IL_04bf:  ldc.i4.s   1
    IL_04c1:  add
    IL_04c2:  stelem.i4
    IL_04c3:  ldloc.s    V_3
    IL_04c5:  ldloc.s    V_2
    IL_04c7:  ldc.i4.1
    IL_04c8:  sub
    IL_04c9:  dup
    IL_04ca:  stloc.s    V_2
    IL_04cc:  ldc.i4.s   7
    IL_04ce:  stelem.i4
    IL_04cf:  ldloc.s    V_2
    IL_04d1:  ldc.i4     0x80
    IL_04d6:  ble.s      IL_04e1

    IL_04d8:  ldloc.s    V_4
    IL_04da:  ldc.i4.s   96
    IL_04dc:  bgt        IL_03f4

    IL_04e1:  ldloc.s    V_3
    IL_04e3:  ldloc.s    V_2
    IL_04e5:  ldc.i4.1
    IL_04e6:  sub
    IL_04e7:  dup
    IL_04e8:  stloc.s    V_2
    IL_04ea:  ldc.i4.s   8
    IL_04ec:  stelem.i4
    IL_04ed:  br         IL_09ff

    IL_04f2:  ldloc.s    V_3
    IL_04f4:  ldloc.s    V_2
    IL_04f6:  ldc.i4.1
    IL_04f7:  sub
    IL_04f8:  dup
    IL_04f9:  stloc.s    V_2
    IL_04fb:  ldloc.s    V_6
    IL_04fd:  stelem.i4
    IL_04fe:  ldloc.s    V_3
    IL_0500:  ldloc.s    V_2
    IL_0502:  ldc.i4.1
    IL_0503:  sub
    IL_0504:  dup
    IL_0505:  stloc.s    V_2
    IL_0507:  ldc.i4.s   9
    IL_0509:  stelem.i4
    IL_050a:  ldarg.0
    IL_050b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0510:  ldloc.s    V_5
    IL_0512:  ldloc.s    V_4
    IL_0514:  dup
    IL_0515:  ldc.i4.1
    IL_0516:  add
    IL_0517:  stloc.s    V_4
    IL_0519:  ldelem.i4
    IL_051a:  stloc.s    V_6
    IL_051c:  ldarg.0
    IL_051d:  ldc.i4.s   2
    IL_051f:  ldloc.s    V_6
    IL_0521:  ldloc.s    V_0
    IL_0523:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_0528:  ldloc.s    V_3
    IL_052a:  ldloc.s    V_2
    IL_052c:  ldc.i4.1
    IL_052d:  sub
    IL_052e:  dup
    IL_052f:  stloc.s    V_2
    IL_0531:  ldloc.s    V_6
    IL_0533:  stelem.i4
    IL_0534:  ldloc.s    V_3
    IL_0536:  ldloc.s    V_2
    IL_0538:  ldc.i4.1
    IL_0539:  sub
    IL_053a:  dup
    IL_053b:  stloc.s    V_2
    IL_053d:  ldc.i4.s   4
    IL_053f:  stelem.i4
    IL_0540:  ldarg.0
    IL_0541:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0546:  ldloc.s    V_5
    IL_0548:  ldloc.s    V_4
    IL_054a:  dup
    IL_054b:  ldc.i4.1
    IL_054c:  add
    IL_054d:  stloc.s    V_4
    IL_054f:  ldelem.i4
    IL_0550:  stloc.s    V_6
    IL_0552:  ldloc.s    V_5
    IL_0554:  ldloc.s    V_4
    IL_0556:  dup
    IL_0557:  ldc.i4.1
    IL_0558:  add
    IL_0559:  stloc.s    V_4
    IL_055b:  ldelem.i4
    IL_055c:  dup
    IL_055d:  stloc.s    V_7
    IL_055f:  ldloc.s    V_3
    IL_0561:  ldloc.s    V_2
    IL_0563:  ldc.i4.1
    IL_0564:  sub
    IL_0565:  dup
    IL_0566:  stloc.s    V_2
    IL_0568:  ldloc.s    V_7
    IL_056a:  stelem.i4
    IL_056b:  ldloc.s    V_0
    IL_056d:  bne.un.s   IL_0575

    IL_056f:  ldloc.s    V_6
    IL_0571:  ldc.i4.s   0
    IL_0573:  bge.s      IL_05c5

    IL_0575:  ldloc.s    V_6
    IL_0577:  ldc.i4.s   1
    IL_0579:  bge.s      IL_05c5

    IL_057b:  ldloc.s    V_5
    IL_057d:  ldloc.s    V_4
    IL_057f:  ldc.i4.1
    IL_0580:  sub
    IL_0581:  dup
    IL_0582:  stloc.s    V_4
    IL_0584:  ldloc.s    V_0
    IL_0586:  stelem.i4
    IL_0587:  ldloc.s    V_5
    IL_0589:  ldloc.s    V_4
    IL_058b:  ldc.i4.1
    IL_058c:  sub
    IL_058d:  dup
    IL_058e:  stloc.s    V_4
    IL_0590:  ldloc.s    V_6
    IL_0592:  ldc.i4.s   1
    IL_0594:  add
    IL_0595:  stelem.i4
    IL_0596:  ldloc.s    V_3
    IL_0598:  ldloc.s    V_2
    IL_059a:  ldc.i4.1
    IL_059b:  sub
    IL_059c:  dup
    IL_059d:  stloc.s    V_2
    IL_059f:  ldc.i4.s   10
    IL_05a1:  stelem.i4
    IL_05a2:  ldloc.s    V_2
    IL_05a4:  ldc.i4     0x80
    IL_05a9:  ble.s      IL_05b4

    IL_05ab:  ldloc.s    V_4
    IL_05ad:  ldc.i4.s   96
    IL_05af:  bgt        IL_02bb

    IL_05b4:  ldloc.s    V_3
    IL_05b6:  ldloc.s    V_2
    IL_05b8:  ldc.i4.1
    IL_05b9:  sub
    IL_05ba:  dup
    IL_05bb:  stloc.s    V_2
    IL_05bd:  ldc.i4.s   11
    IL_05bf:  stelem.i4
    IL_05c0:  br         IL_09ff

    IL_05c5:  ldloc.s    V_3
    IL_05c7:  ldloc.s    V_2
    IL_05c9:  ldc.i4.1
    IL_05ca:  sub
    IL_05cb:  dup
    IL_05cc:  stloc.s    V_2
    IL_05ce:  ldloc.s    V_6
    IL_05d0:  stelem.i4
    IL_05d1:  ldloc.s    V_3
    IL_05d3:  ldloc.s    V_2
    IL_05d5:  ldc.i4.1
    IL_05d6:  sub
    IL_05d7:  dup
    IL_05d8:  stloc.s    V_2
    IL_05da:  ldc.i4.s   9
    IL_05dc:  stelem.i4
    IL_05dd:  ldarg.0
    IL_05de:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_05e3:  ldloc.s    V_5
    IL_05e5:  ldloc.s    V_4
    IL_05e7:  ldc.i4.1
    IL_05e8:  sub
    IL_05e9:  dup
    IL_05ea:  stloc.s    V_4
    IL_05ec:  ldc.i4.s   -1
    IL_05ee:  stelem.i4
    IL_05ef:  ldloc.s    V_5
    IL_05f1:  ldloc.s    V_4
    IL_05f3:  ldc.i4.1
    IL_05f4:  sub
    IL_05f5:  dup
    IL_05f6:  stloc.s    V_4
    IL_05f8:  ldc.i4.s   0
    IL_05fa:  stelem.i4
    IL_05fb:  ldloc.s    V_3
    IL_05fd:  ldloc.s    V_2
    IL_05ff:  ldc.i4.1
    IL_0600:  sub
    IL_0601:  dup
    IL_0602:  stloc.s    V_2
    IL_0604:  ldc.i4.s   5
    IL_0606:  stelem.i4
    IL_0607:  ldarg.0
    IL_0608:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_060d:  br         IL_0771

    IL_0612:  ldarg.0
    IL_0613:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0618:  ldloc.s    V_5
    IL_061a:  ldloc.s    V_4
    IL_061c:  ldc.i4.1
    IL_061d:  sub
    IL_061e:  dup
    IL_061f:  stloc.s    V_4
    IL_0621:  ldloc.s    V_0
    IL_0623:  stelem.i4
    IL_0624:  ldloc.s    V_3
    IL_0626:  ldloc.s    V_2
    IL_0628:  ldc.i4.1
    IL_0629:  sub
    IL_062a:  dup
    IL_062b:  stloc.s    V_2
    IL_062d:  ldc.i4.s   1
    IL_062f:  stelem.i4
    IL_0630:  ldarg.0
    IL_0631:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0636:  ldloc.s    V_0
    IL_0638:  ldloc.s    V_10
    IL_063a:  bge        IL_09ff

    IL_063f:  ldloc.s    V_1
    IL_0641:  ldloc.s    V_0
    IL_0643:  dup
    IL_0644:  ldc.i4.s   1
    IL_0646:  add
    IL_0647:  stloc.s    V_0
    IL_0649:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_064e:  ldc.i4.s   91
    IL_0650:  bne.un     IL_09ff

    IL_0655:  ldarg.0
    IL_0656:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_065b:  ldc.i4.s   1
    IL_065d:  ldloc.s    V_10
    IL_065f:  ldloc.s    V_0
    IL_0661:  sub
    IL_0662:  bgt        IL_09ff

    IL_0667:  ldloc.s    V_0
    IL_0669:  ldc.i4.s   1
    IL_066b:  add
    IL_066c:  stloc.s    V_0
    IL_066e:  ldc.i4.s   1
    IL_0670:  stloc.s    V_6
    IL_0672:  ldloc.s    V_1
    IL_0674:  ldloc.s    V_0
    IL_0676:  ldloc.s    V_6
    IL_0678:  dup
    IL_0679:  ldc.i4.s   1
    IL_067b:  sub
    IL_067c:  stloc.s    V_6
    IL_067e:  sub
    IL_067f:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0684:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_0689:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_068e:  brfalse    IL_09ff

    IL_0693:  ldloc.s    V_6
    IL_0695:  ldc.i4.s   0
    IL_0697:  bgt        IL_0672

    IL_069c:  ldarg.0
    IL_069d:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_06a2:  ldloc.s    V_10
    IL_06a4:  ldloc.s    V_0
    IL_06a6:  sub
    IL_06a7:  dup
    IL_06a8:  stloc.s    V_7
    IL_06aa:  ldc.i4.s   1
    IL_06ac:  add
    IL_06ad:  stloc.s    V_6
    IL_06af:  ldloc.s    V_6
    IL_06b1:  ldc.i4.s   1
    IL_06b3:  sub
    IL_06b4:  dup
    IL_06b5:  stloc.s    V_6
    IL_06b7:  ldc.i4.s   0
    IL_06b9:  ble        IL_06e3

    IL_06be:  ldloc.s    V_1
    IL_06c0:  ldloc.s    V_0
    IL_06c2:  dup
    IL_06c3:  ldc.i4.s   1
    IL_06c5:  add
    IL_06c6:  stloc.s    V_0
    IL_06c8:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_06cd:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_06d2:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_06d7:  brtrue     IL_06af

    IL_06dc:  ldloc.s    V_0
    IL_06de:  ldc.i4.s   1
    IL_06e0:  sub
    IL_06e1:  stloc.s    V_0
    IL_06e3:  ldloc.s    V_7
    IL_06e5:  ldloc.s    V_6
    IL_06e7:  ble.s      IL_0716

    IL_06e9:  ldloc.s    V_3
    IL_06eb:  ldloc.s    V_2
    IL_06ed:  ldc.i4.1
    IL_06ee:  sub
    IL_06ef:  dup
    IL_06f0:  stloc.s    V_2
    IL_06f2:  ldloc.s    V_7
    IL_06f4:  ldloc.s    V_6
    IL_06f6:  sub
    IL_06f7:  ldc.i4.s   1
    IL_06f9:  sub
    IL_06fa:  stelem.i4
    IL_06fb:  ldloc.s    V_3
    IL_06fd:  ldloc.s    V_2
    IL_06ff:  ldc.i4.1
    IL_0700:  sub
    IL_0701:  dup
    IL_0702:  stloc.s    V_2
    IL_0704:  ldloc.s    V_0
    IL_0706:  ldc.i4.s   1
    IL_0708:  sub
    IL_0709:  stelem.i4
    IL_070a:  ldloc.s    V_3
    IL_070c:  ldloc.s    V_2
    IL_070e:  ldc.i4.1
    IL_070f:  sub
    IL_0710:  dup
    IL_0711:  stloc.s    V_2
    IL_0713:  ldc.i4.s   12
    IL_0715:  stelem.i4
    IL_0716:  ldarg.0
    IL_0717:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_071c:  ldloc.s    V_0
    IL_071e:  ldloc.s    V_10
    IL_0720:  bge        IL_09ff

    IL_0725:  ldloc.s    V_1
    IL_0727:  ldloc.s    V_0
    IL_0729:  dup
    IL_072a:  ldc.i4.s   1
    IL_072c:  add
    IL_072d:  stloc.s    V_0
    IL_072f:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0734:  ldc.i4.s   93
    IL_0736:  bne.un     IL_09ff

    IL_073b:  ldarg.0
    IL_073c:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0741:  ldloc.s    V_5
    IL_0743:  ldloc.s    V_4
    IL_0745:  dup
    IL_0746:  ldc.i4.1
    IL_0747:  add
    IL_0748:  stloc.s    V_4
    IL_074a:  ldelem.i4
    IL_074b:  stloc.s    V_6
    IL_074d:  ldarg.0
    IL_074e:  ldc.i4.s   4
    IL_0750:  ldloc.s    V_6
    IL_0752:  ldloc.s    V_0
    IL_0754:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_0759:  ldloc.s    V_3
    IL_075b:  ldloc.s    V_2
    IL_075d:  ldc.i4.1
    IL_075e:  sub
    IL_075f:  dup
    IL_0760:  stloc.s    V_2
    IL_0762:  ldloc.s    V_6
    IL_0764:  stelem.i4
    IL_0765:  ldloc.s    V_3
    IL_0767:  ldloc.s    V_2
    IL_0769:  ldc.i4.1
    IL_076a:  sub
    IL_076b:  dup
    IL_076c:  stloc.s    V_2
    IL_076e:  ldc.i4.s   4
    IL_0770:  stelem.i4
    IL_0771:  ldarg.0
    IL_0772:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0777:  ldloc.s    V_5
    IL_0779:  ldloc.s    V_4
    IL_077b:  dup
    IL_077c:  ldc.i4.1
    IL_077d:  add
    IL_077e:  stloc.s    V_4
    IL_0780:  ldelem.i4
    IL_0781:  stloc.s    V_6
    IL_0783:  ldloc.s    V_5
    IL_0785:  ldloc.s    V_4
    IL_0787:  dup
    IL_0788:  ldc.i4.1
    IL_0789:  add
    IL_078a:  stloc.s    V_4
    IL_078c:  ldelem.i4
    IL_078d:  dup
    IL_078e:  stloc.s    V_7
    IL_0790:  ldloc.s    V_3
    IL_0792:  ldloc.s    V_2
    IL_0794:  ldc.i4.1
    IL_0795:  sub
    IL_0796:  dup
    IL_0797:  stloc.s    V_2
    IL_0799:  ldloc.s    V_7
    IL_079b:  stelem.i4
    IL_079c:  ldloc.s    V_0
    IL_079e:  bne.un.s   IL_07a6

    IL_07a0:  ldloc.s    V_6
    IL_07a2:  ldc.i4.s   0
    IL_07a4:  bge.s      IL_07f6

    IL_07a6:  ldloc.s    V_6
    IL_07a8:  ldc.i4.s   1
    IL_07aa:  bge.s      IL_07f6

    IL_07ac:  ldloc.s    V_5
    IL_07ae:  ldloc.s    V_4
    IL_07b0:  ldc.i4.1
    IL_07b1:  sub
    IL_07b2:  dup
    IL_07b3:  stloc.s    V_4
    IL_07b5:  ldloc.s    V_0
    IL_07b7:  stelem.i4
    IL_07b8:  ldloc.s    V_5
    IL_07ba:  ldloc.s    V_4
    IL_07bc:  ldc.i4.1
    IL_07bd:  sub
    IL_07be:  dup
    IL_07bf:  stloc.s    V_4
    IL_07c1:  ldloc.s    V_6
    IL_07c3:  ldc.i4.s   1
    IL_07c5:  add
    IL_07c6:  stelem.i4
    IL_07c7:  ldloc.s    V_3
    IL_07c9:  ldloc.s    V_2
    IL_07cb:  ldc.i4.1
    IL_07cc:  sub
    IL_07cd:  dup
    IL_07ce:  stloc.s    V_2
    IL_07d0:  ldc.i4.s   13
    IL_07d2:  stelem.i4
    IL_07d3:  ldloc.s    V_2
    IL_07d5:  ldc.i4     0x80
    IL_07da:  ble.s      IL_07e5

    IL_07dc:  ldloc.s    V_4
    IL_07de:  ldc.i4.s   96
    IL_07e0:  bgt        IL_0612

    IL_07e5:  ldloc.s    V_3
    IL_07e7:  ldloc.s    V_2
    IL_07e9:  ldc.i4.1
    IL_07ea:  sub
    IL_07eb:  dup
    IL_07ec:  stloc.s    V_2
    IL_07ee:  ldc.i4.s   14
    IL_07f0:  stelem.i4
    IL_07f1:  br         IL_09ff

    IL_07f6:  ldloc.s    V_3
    IL_07f8:  ldloc.s    V_2
    IL_07fa:  ldc.i4.1
    IL_07fb:  sub
    IL_07fc:  dup
    IL_07fd:  stloc.s    V_2
    IL_07ff:  ldloc.s    V_6
    IL_0801:  stelem.i4
    IL_0802:  ldloc.s    V_3
    IL_0804:  ldloc.s    V_2
    IL_0806:  ldc.i4.1
    IL_0807:  sub
    IL_0808:  dup
    IL_0809:  stloc.s    V_2
    IL_080b:  ldc.i4.s   9
    IL_080d:  stelem.i4
    IL_080e:  ldarg.0
    IL_080f:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0814:  ldloc.s    V_0
    IL_0816:  ldloc.s    V_10
    IL_0818:  bge        IL_09ff

    IL_081d:  ldloc.s    V_1
    IL_081f:  ldloc.s    V_0
    IL_0821:  dup
    IL_0822:  ldc.i4.s   1
    IL_0824:  add
    IL_0825:  stloc.s    V_0
    IL_0827:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_082c:  ldc.i4.s   58
    IL_082e:  bne.un     IL_09ff

    IL_0833:  ldarg.0
    IL_0834:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0839:  ldloc.s    V_5
    IL_083b:  ldloc.s    V_4
    IL_083d:  ldc.i4.1
    IL_083e:  sub
    IL_083f:  dup
    IL_0840:  stloc.s    V_4
    IL_0842:  ldloc.s    V_0
    IL_0844:  stelem.i4
    IL_0845:  ldloc.s    V_3
    IL_0847:  ldloc.s    V_2
    IL_0849:  ldc.i4.1
    IL_084a:  sub
    IL_084b:  dup
    IL_084c:  stloc.s    V_2
    IL_084e:  ldc.i4.s   1
    IL_0850:  stelem.i4
    IL_0851:  ldarg.0
    IL_0852:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0857:  ldc.i4.s   1
    IL_0859:  ldloc.s    V_10
    IL_085b:  ldloc.s    V_0
    IL_085d:  sub
    IL_085e:  bgt        IL_09ff

    IL_0863:  ldloc.s    V_0
    IL_0865:  ldc.i4.s   1
    IL_0867:  add
    IL_0868:  stloc.s    V_0
    IL_086a:  ldc.i4.s   1
    IL_086c:  stloc.s    V_6
    IL_086e:  ldloc.s    V_1
    IL_0870:  ldloc.s    V_0
    IL_0872:  ldloc.s    V_6
    IL_0874:  dup
    IL_0875:  ldc.i4.s   1
    IL_0877:  sub
    IL_0878:  stloc.s    V_6
    IL_087a:  sub
    IL_087b:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0880:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_0885:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_088a:  brfalse    IL_09ff

    IL_088f:  ldloc.s    V_6
    IL_0891:  ldc.i4.s   0
    IL_0893:  bgt        IL_086e

    IL_0898:  ldarg.0
    IL_0899:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_089e:  ldloc.s    V_10
    IL_08a0:  ldloc.s    V_0
    IL_08a2:  sub
    IL_08a3:  dup
    IL_08a4:  stloc.s    V_7
    IL_08a6:  ldc.i4.s   1
    IL_08a8:  add
    IL_08a9:  stloc.s    V_6
    IL_08ab:  ldloc.s    V_6
    IL_08ad:  ldc.i4.s   1
    IL_08af:  sub
    IL_08b0:  dup
    IL_08b1:  stloc.s    V_6
    IL_08b3:  ldc.i4.s   0
    IL_08b5:  ble        IL_08df

    IL_08ba:  ldloc.s    V_1
    IL_08bc:  ldloc.s    V_0
    IL_08be:  dup
    IL_08bf:  ldc.i4.s   1
    IL_08c1:  add
    IL_08c2:  stloc.s    V_0
    IL_08c4:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_08c9:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_08ce:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_08d3:  brtrue     IL_08ab

    IL_08d8:  ldloc.s    V_0
    IL_08da:  ldc.i4.s   1
    IL_08dc:  sub
    IL_08dd:  stloc.s    V_0
    IL_08df:  ldloc.s    V_7
    IL_08e1:  ldloc.s    V_6
    IL_08e3:  ble.s      IL_0912

    IL_08e5:  ldloc.s    V_3
    IL_08e7:  ldloc.s    V_2
    IL_08e9:  ldc.i4.1
    IL_08ea:  sub
    IL_08eb:  dup
    IL_08ec:  stloc.s    V_2
    IL_08ee:  ldloc.s    V_7
    IL_08f0:  ldloc.s    V_6
    IL_08f2:  sub
    IL_08f3:  ldc.i4.s   1
    IL_08f5:  sub
    IL_08f6:  stelem.i4
    IL_08f7:  ldloc.s    V_3
    IL_08f9:  ldloc.s    V_2
    IL_08fb:  ldc.i4.1
    IL_08fc:  sub
    IL_08fd:  dup
    IL_08fe:  stloc.s    V_2
    IL_0900:  ldloc.s    V_0
    IL_0902:  ldc.i4.s   1
    IL_0904:  sub
    IL_0905:  stelem.i4
    IL_0906:  ldloc.s    V_3
    IL_0908:  ldloc.s    V_2
    IL_090a:  ldc.i4.1
    IL_090b:  sub
    IL_090c:  dup
    IL_090d:  stloc.s    V_2
    IL_090f:  ldc.i4.s   15
    IL_0911:  stelem.i4
    IL_0912:  ldarg.0
    IL_0913:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0918:  ldloc.s    V_5
    IL_091a:  ldloc.s    V_4
    IL_091c:  dup
    IL_091d:  ldc.i4.1
    IL_091e:  add
    IL_091f:  stloc.s    V_4
    IL_0921:  ldelem.i4
    IL_0922:  stloc.s    V_6
    IL_0924:  ldarg.0
    IL_0925:  ldc.i4.s   5
    IL_0927:  ldloc.s    V_6
    IL_0929:  ldloc.s    V_0
    IL_092b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_0930:  ldloc.s    V_3
    IL_0932:  ldloc.s    V_2
    IL_0934:  ldc.i4.1
    IL_0935:  sub
    IL_0936:  dup
    IL_0937:  stloc.s    V_2
    IL_0939:  ldloc.s    V_6
    IL_093b:  stelem.i4
    IL_093c:  ldloc.s    V_3
    IL_093e:  ldloc.s    V_2
    IL_0940:  ldc.i4.1
    IL_0941:  sub
    IL_0942:  dup
    IL_0943:  stloc.s    V_2
    IL_0945:  ldc.i4.s   4
    IL_0947:  stelem.i4
    IL_0948:  ldarg.0
    IL_0949:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_094e:  ldloc.s    V_5
    IL_0950:  ldloc.s    V_4
    IL_0952:  dup
    IL_0953:  ldc.i4.1
    IL_0954:  add
    IL_0955:  stloc.s    V_4
    IL_0957:  ldelem.i4
    IL_0958:  stloc.s    V_6
    IL_095a:  ldarg.0
    IL_095b:  ldc.i4.s   1
    IL_095d:  ldloc.s    V_6
    IL_095f:  ldloc.s    V_0
    IL_0961:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_0966:  ldloc.s    V_3
    IL_0968:  ldloc.s    V_2
    IL_096a:  ldc.i4.1
    IL_096b:  sub
    IL_096c:  dup
    IL_096d:  stloc.s    V_2
    IL_096f:  ldloc.s    V_6
    IL_0971:  stelem.i4
    IL_0972:  ldloc.s    V_3
    IL_0974:  ldloc.s    V_2
    IL_0976:  ldc.i4.1
    IL_0977:  sub
    IL_0978:  dup
    IL_0979:  stloc.s    V_2
    IL_097b:  ldc.i4.s   4
    IL_097d:  stelem.i4
    IL_097e:  ldarg.0
    IL_097f:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0984:  ldc.i4.s   2
    IL_0986:  ldloc.s    V_10
    IL_0988:  ldloc.s    V_0
    IL_098a:  sub
    IL_098b:  bgt        IL_09ff

    IL_0990:  ldloc.s    V_1
    IL_0992:  ldloc.s    V_0
    IL_0994:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0999:  ldc.i4.s   58
    IL_099b:  bne.un     IL_09ff

    IL_09a0:  ldloc.s    V_1
    IL_09a2:  ldloc.s    V_0
    IL_09a4:  ldc.i4.s   1
    IL_09a6:  add
    IL_09a7:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_09ac:  ldc.i4.s   123
    IL_09ae:  bne.un     IL_09ff

    IL_09b3:  ldloc.s    V_0
    IL_09b5:  ldc.i4.s   2
    IL_09b7:  add
    IL_09b8:  stloc.s    V_0
    IL_09ba:  ldarg.0
    IL_09bb:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_09c0:  ldloc.s    V_5
    IL_09c2:  ldloc.s    V_4
    IL_09c4:  dup
    IL_09c5:  ldc.i4.1
    IL_09c6:  add
    IL_09c7:  stloc.s    V_4
    IL_09c9:  ldelem.i4
    IL_09ca:  stloc.s    V_6
    IL_09cc:  ldarg.0
    IL_09cd:  ldc.i4.s   0
    IL_09cf:  ldloc.s    V_6
    IL_09d1:  ldloc.s    V_0
    IL_09d3:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_09d8:  ldloc.s    V_3
    IL_09da:  ldloc.s    V_2
    IL_09dc:  ldc.i4.1
    IL_09dd:  sub
    IL_09de:  dup
    IL_09df:  stloc.s    V_2
    IL_09e1:  ldloc.s    V_6
    IL_09e3:  stelem.i4
    IL_09e4:  ldloc.s    V_3
    IL_09e6:  ldloc.s    V_2
    IL_09e8:  ldc.i4.1
    IL_09e9:  sub
    IL_09ea:  dup
    IL_09eb:  stloc.s    V_2
    IL_09ed:  ldc.i4.s   4
    IL_09ef:  stelem.i4
    IL_09f0:  ldarg.0
    IL_09f1:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_09f6:  ldarg.0
    IL_09f7:  ldloc.s    V_0
    IL_09f9:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextpos
    IL_09fe:  ret

    IL_09ff:  ldarg.0
    IL_0a00:  ldloc.s    V_2
    IL_0a02:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtrackpos
    IL_0a07:  ldarg.0
    IL_0a08:  ldloc.s    V_4
    IL_0a0a:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runstackpos
    IL_0a0f:  ldarg.0
    IL_0a10:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::EnsureStorage()
    IL_0a15:  ldarg.0
    IL_0a16:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtrackpos
    IL_0a1b:  stloc.s    V_2
    IL_0a1d:  ldarg.0
    IL_0a1e:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runstackpos
    IL_0a23:  stloc.s    V_4
    IL_0a25:  ldarg.0
    IL_0a26:  ldfld      int32[] [System]System.Text.RegularExpressions.RegexRunner::runtrack
    IL_0a2b:  stloc.s    V_3
    IL_0a2d:  ldarg.0
    IL_0a2e:  ldfld      int32[] [System]System.Text.RegularExpressions.RegexRunner::runstack
    IL_0a33:  stloc.s    V_5
    IL_0a35:  ldloc.s    V_3
    IL_0a37:  ldloc.s    V_2
    IL_0a39:  dup
    IL_0a3a:  ldc.i4.1
    IL_0a3b:  add
    IL_0a3c:  stloc.s    V_2
    IL_0a3e:  ldelem.i4
    IL_0a3f:  switch     ( 
                          IL_0a84,
                          IL_0a9b,
                          IL_0aad,
                          IL_0ac4,
                          IL_0b1a,
                          IL_0b3f,
                          IL_0b51,
                          IL_0ba7,
                          IL_03f4,
                          IL_0c0a,
                          IL_0c41,
                          IL_02bb,
                          IL_0ca4,
                          IL_0cfa,
                          IL_0612,
                          IL_0d5d)
    IL_0a84:  ldarg.0
    IL_0a85:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0a8a:  ldloc.s    V_3
    IL_0a8c:  ldloc.s    V_2
    IL_0a8e:  dup
    IL_0a8f:  ldc.i4.1
    IL_0a90:  add
    IL_0a91:  stloc.s    V_2
    IL_0a93:  ldelem.i4
    IL_0a94:  stloc.s    V_0
    IL_0a96:  br         IL_09f0

    IL_0a9b:  ldarg.0
    IL_0a9c:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0aa1:  ldloc.s    V_4
    IL_0aa3:  ldc.i4.s   1
    IL_0aa5:  add
    IL_0aa6:  stloc.s    V_4
    IL_0aa8:  br         IL_09ff

    IL_0aad:  ldarg.0
    IL_0aae:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0ab3:  ldloc.s    V_3
    IL_0ab5:  ldloc.s    V_2
    IL_0ab7:  dup
    IL_0ab8:  ldc.i4.1
    IL_0ab9:  add
    IL_0aba:  stloc.s    V_2
    IL_0abc:  ldelem.i4
    IL_0abd:  stloc.s    V_0
    IL_0abf:  br         IL_0205

    IL_0ac4:  ldarg.0
    IL_0ac5:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0aca:  ldloc.s    V_3
    IL_0acc:  ldloc.s    V_2
    IL_0ace:  dup
    IL_0acf:  ldc.i4.1
    IL_0ad0:  add
    IL_0ad1:  stloc.s    V_2
    IL_0ad3:  ldelem.i4
    IL_0ad4:  stloc.s    V_0
    IL_0ad6:  ldloc.s    V_3
    IL_0ad8:  ldloc.s    V_2
    IL_0ada:  dup
    IL_0adb:  ldc.i4.1
    IL_0adc:  add
    IL_0add:  stloc.s    V_2
    IL_0adf:  ldelem.i4
    IL_0ae0:  stloc.s    V_6
    IL_0ae2:  ldloc.s    V_6
    IL_0ae4:  ldc.i4.s   0
    IL_0ae6:  ble        IL_01c4

    IL_0aeb:  ldloc.s    V_3
    IL_0aed:  ldloc.s    V_2
    IL_0aef:  ldc.i4.1
    IL_0af0:  sub
    IL_0af1:  dup
    IL_0af2:  stloc.s    V_2
    IL_0af4:  ldloc.s    V_6
    IL_0af6:  ldc.i4.s   1
    IL_0af8:  sub
    IL_0af9:  stelem.i4
    IL_0afa:  ldloc.s    V_3
    IL_0afc:  ldloc.s    V_2
    IL_0afe:  ldc.i4.1
    IL_0aff:  sub
    IL_0b00:  dup
    IL_0b01:  stloc.s    V_2
    IL_0b03:  ldloc.s    V_0
    IL_0b05:  ldc.i4.s   1
    IL_0b07:  sub
    IL_0b08:  stelem.i4
    IL_0b09:  ldloc.s    V_3
    IL_0b0b:  ldloc.s    V_2
    IL_0b0d:  ldc.i4.1
    IL_0b0e:  sub
    IL_0b0f:  dup
    IL_0b10:  stloc.s    V_2
    IL_0b12:  ldc.i4.s   3
    IL_0b14:  stelem.i4
    IL_0b15:  br         IL_01c4

    IL_0b1a:  ldarg.0
    IL_0b1b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0b20:  ldloc.s    V_5
    IL_0b22:  ldloc.s    V_4
    IL_0b24:  ldc.i4.1
    IL_0b25:  sub
    IL_0b26:  dup
    IL_0b27:  stloc.s    V_4
    IL_0b29:  ldloc.s    V_3
    IL_0b2b:  ldloc.s    V_2
    IL_0b2d:  dup
    IL_0b2e:  ldc.i4.1
    IL_0b2f:  add
    IL_0b30:  stloc.s    V_2
    IL_0b32:  ldelem.i4
    IL_0b33:  stelem.i4
    IL_0b34:  ldarg.0
    IL_0b35:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Uncapture()
    IL_0b3a:  br         IL_09ff

    IL_0b3f:  ldarg.0
    IL_0b40:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0b45:  ldloc.s    V_4
    IL_0b47:  ldc.i4.s   2
    IL_0b49:  add
    IL_0b4a:  stloc.s    V_4
    IL_0b4c:  br         IL_09ff

    IL_0b51:  ldarg.0
    IL_0b52:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0b57:  ldloc.s    V_3
    IL_0b59:  ldloc.s    V_2
    IL_0b5b:  dup
    IL_0b5c:  ldc.i4.1
    IL_0b5d:  add
    IL_0b5e:  stloc.s    V_2
    IL_0b60:  ldelem.i4
    IL_0b61:  stloc.s    V_0
    IL_0b63:  ldloc.s    V_3
    IL_0b65:  ldloc.s    V_2
    IL_0b67:  dup
    IL_0b68:  ldc.i4.1
    IL_0b69:  add
    IL_0b6a:  stloc.s    V_2
    IL_0b6c:  ldelem.i4
    IL_0b6d:  stloc.s    V_6
    IL_0b6f:  ldloc.s    V_6
    IL_0b71:  ldc.i4.s   0
    IL_0b73:  ble        IL_03bf

    IL_0b78:  ldloc.s    V_3
    IL_0b7a:  ldloc.s    V_2
    IL_0b7c:  ldc.i4.1
    IL_0b7d:  sub
    IL_0b7e:  dup
    IL_0b7f:  stloc.s    V_2
    IL_0b81:  ldloc.s    V_6
    IL_0b83:  ldc.i4.s   1
    IL_0b85:  sub
    IL_0b86:  stelem.i4
    IL_0b87:  ldloc.s    V_3
    IL_0b89:  ldloc.s    V_2
    IL_0b8b:  ldc.i4.1
    IL_0b8c:  sub
    IL_0b8d:  dup
    IL_0b8e:  stloc.s    V_2
    IL_0b90:  ldloc.s    V_0
    IL_0b92:  ldc.i4.s   1
    IL_0b94:  sub
    IL_0b95:  stelem.i4
    IL_0b96:  ldloc.s    V_3
    IL_0b98:  ldloc.s    V_2
    IL_0b9a:  ldc.i4.1
    IL_0b9b:  sub
    IL_0b9c:  dup
    IL_0b9d:  stloc.s    V_2
    IL_0b9f:  ldc.i4.s   6
    IL_0ba1:  stelem.i4
    IL_0ba2:  br         IL_03bf

    IL_0ba7:  ldarg.0
    IL_0ba8:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0bad:  ldloc.s    V_5
    IL_0baf:  ldloc.s    V_4
    IL_0bb1:  dup
    IL_0bb2:  ldc.i4.1
    IL_0bb3:  add
    IL_0bb4:  stloc.s    V_4
    IL_0bb6:  ldelem.i4
    IL_0bb7:  ldc.i4.s   1
    IL_0bb9:  sub
    IL_0bba:  dup
    IL_0bbb:  stloc.s    V_6
    IL_0bbd:  ldc.i4.s   0
    IL_0bbf:  blt.s      IL_0bea

    IL_0bc1:  ldloc.s    V_5
    IL_0bc3:  ldloc.s    V_4
    IL_0bc5:  dup
    IL_0bc6:  ldc.i4.1
    IL_0bc7:  add
    IL_0bc8:  stloc.s    V_4
    IL_0bca:  ldelem.i4
    IL_0bcb:  stloc.s    V_0
    IL_0bcd:  ldloc.s    V_3
    IL_0bcf:  ldloc.s    V_2
    IL_0bd1:  ldc.i4.1
    IL_0bd2:  sub
    IL_0bd3:  dup
    IL_0bd4:  stloc.s    V_2
    IL_0bd6:  ldloc.s    V_6
    IL_0bd8:  stelem.i4
    IL_0bd9:  ldloc.s    V_3
    IL_0bdb:  ldloc.s    V_2
    IL_0bdd:  ldc.i4.1
    IL_0bde:  sub
    IL_0bdf:  dup
    IL_0be0:  stloc.s    V_2
    IL_0be2:  ldc.i4.s   9
    IL_0be4:  stelem.i4
    IL_0be5:  br         IL_050a

    IL_0bea:  ldloc.s    V_5
    IL_0bec:  ldloc.s    V_4
    IL_0bee:  ldloc.s    V_3
    IL_0bf0:  ldloc.s    V_2
    IL_0bf2:  dup
    IL_0bf3:  ldc.i4.1
    IL_0bf4:  add
    IL_0bf5:  stloc.s    V_2
    IL_0bf7:  ldelem.i4
    IL_0bf8:  stelem.i4
    IL_0bf9:  ldloc.s    V_5
    IL_0bfb:  ldloc.s    V_4
    IL_0bfd:  ldc.i4.1
    IL_0bfe:  sub
    IL_0bff:  dup
    IL_0c00:  stloc.s    V_4
    IL_0c02:  ldloc.s    V_6
    IL_0c04:  stelem.i4
    IL_0c05:  br         IL_09ff

    IL_0c0a:  ldarg.0
    IL_0c0b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0c10:  ldloc.s    V_3
    IL_0c12:  ldloc.s    V_2
    IL_0c14:  dup
    IL_0c15:  ldc.i4.1
    IL_0c16:  add
    IL_0c17:  stloc.s    V_2
    IL_0c19:  ldelem.i4
    IL_0c1a:  stloc.s    V_6
    IL_0c1c:  ldloc.s    V_5
    IL_0c1e:  ldloc.s    V_4
    IL_0c20:  ldc.i4.1
    IL_0c21:  sub
    IL_0c22:  dup
    IL_0c23:  stloc.s    V_4
    IL_0c25:  ldloc.s    V_3
    IL_0c27:  ldloc.s    V_2
    IL_0c29:  dup
    IL_0c2a:  ldc.i4.1
    IL_0c2b:  add
    IL_0c2c:  stloc.s    V_2
    IL_0c2e:  ldelem.i4
    IL_0c2f:  stelem.i4
    IL_0c30:  ldloc.s    V_5
    IL_0c32:  ldloc.s    V_4
    IL_0c34:  ldc.i4.1
    IL_0c35:  sub
    IL_0c36:  dup
    IL_0c37:  stloc.s    V_4
    IL_0c39:  ldloc.s    V_6
    IL_0c3b:  stelem.i4
    IL_0c3c:  br         IL_09ff

    IL_0c41:  ldarg.0
    IL_0c42:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0c47:  ldloc.s    V_5
    IL_0c49:  ldloc.s    V_4
    IL_0c4b:  dup
    IL_0c4c:  ldc.i4.1
    IL_0c4d:  add
    IL_0c4e:  stloc.s    V_4
    IL_0c50:  ldelem.i4
    IL_0c51:  ldc.i4.s   1
    IL_0c53:  sub
    IL_0c54:  dup
    IL_0c55:  stloc.s    V_6
    IL_0c57:  ldc.i4.s   0
    IL_0c59:  blt.s      IL_0c84

    IL_0c5b:  ldloc.s    V_5
    IL_0c5d:  ldloc.s    V_4
    IL_0c5f:  dup
    IL_0c60:  ldc.i4.1
    IL_0c61:  add
    IL_0c62:  stloc.s    V_4
    IL_0c64:  ldelem.i4
    IL_0c65:  stloc.s    V_0
    IL_0c67:  ldloc.s    V_3
    IL_0c69:  ldloc.s    V_2
    IL_0c6b:  ldc.i4.1
    IL_0c6c:  sub
    IL_0c6d:  dup
    IL_0c6e:  stloc.s    V_2
    IL_0c70:  ldloc.s    V_6
    IL_0c72:  stelem.i4
    IL_0c73:  ldloc.s    V_3
    IL_0c75:  ldloc.s    V_2
    IL_0c77:  ldc.i4.1
    IL_0c78:  sub
    IL_0c79:  dup
    IL_0c7a:  stloc.s    V_2
    IL_0c7c:  ldc.i4.s   9
    IL_0c7e:  stelem.i4
    IL_0c7f:  br         IL_05dd

    IL_0c84:  ldloc.s    V_5
    IL_0c86:  ldloc.s    V_4
    IL_0c88:  ldloc.s    V_3
    IL_0c8a:  ldloc.s    V_2
    IL_0c8c:  dup
    IL_0c8d:  ldc.i4.1
    IL_0c8e:  add
    IL_0c8f:  stloc.s    V_2
    IL_0c91:  ldelem.i4
    IL_0c92:  stelem.i4
    IL_0c93:  ldloc.s    V_5
    IL_0c95:  ldloc.s    V_4
    IL_0c97:  ldc.i4.1
    IL_0c98:  sub
    IL_0c99:  dup
    IL_0c9a:  stloc.s    V_4
    IL_0c9c:  ldloc.s    V_6
    IL_0c9e:  stelem.i4
    IL_0c9f:  br         IL_09ff

    IL_0ca4:  ldarg.0
    IL_0ca5:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0caa:  ldloc.s    V_3
    IL_0cac:  ldloc.s    V_2
    IL_0cae:  dup
    IL_0caf:  ldc.i4.1
    IL_0cb0:  add
    IL_0cb1:  stloc.s    V_2
    IL_0cb3:  ldelem.i4
    IL_0cb4:  stloc.s    V_0
    IL_0cb6:  ldloc.s    V_3
    IL_0cb8:  ldloc.s    V_2
    IL_0cba:  dup
    IL_0cbb:  ldc.i4.1
    IL_0cbc:  add
    IL_0cbd:  stloc.s    V_2
    IL_0cbf:  ldelem.i4
    IL_0cc0:  stloc.s    V_6
    IL_0cc2:  ldloc.s    V_6
    IL_0cc4:  ldc.i4.s   0
    IL_0cc6:  ble        IL_0716

    IL_0ccb:  ldloc.s    V_3
    IL_0ccd:  ldloc.s    V_2
    IL_0ccf:  ldc.i4.1
    IL_0cd0:  sub
    IL_0cd1:  dup
    IL_0cd2:  stloc.s    V_2
    IL_0cd4:  ldloc.s    V_6
    IL_0cd6:  ldc.i4.s   1
    IL_0cd8:  sub
    IL_0cd9:  stelem.i4
    IL_0cda:  ldloc.s    V_3
    IL_0cdc:  ldloc.s    V_2
    IL_0cde:  ldc.i4.1
    IL_0cdf:  sub
    IL_0ce0:  dup
    IL_0ce1:  stloc.s    V_2
    IL_0ce3:  ldloc.s    V_0
    IL_0ce5:  ldc.i4.s   1
    IL_0ce7:  sub
    IL_0ce8:  stelem.i4
    IL_0ce9:  ldloc.s    V_3
    IL_0ceb:  ldloc.s    V_2
    IL_0ced:  ldc.i4.1
    IL_0cee:  sub
    IL_0cef:  dup
    IL_0cf0:  stloc.s    V_2
    IL_0cf2:  ldc.i4.s   12
    IL_0cf4:  stelem.i4
    IL_0cf5:  br         IL_0716

    IL_0cfa:  ldarg.0
    IL_0cfb:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0d00:  ldloc.s    V_5
    IL_0d02:  ldloc.s    V_4
    IL_0d04:  dup
    IL_0d05:  ldc.i4.1
    IL_0d06:  add
    IL_0d07:  stloc.s    V_4
    IL_0d09:  ldelem.i4
    IL_0d0a:  ldc.i4.s   1
    IL_0d0c:  sub
    IL_0d0d:  dup
    IL_0d0e:  stloc.s    V_6
    IL_0d10:  ldc.i4.s   0
    IL_0d12:  blt.s      IL_0d3d

    IL_0d14:  ldloc.s    V_5
    IL_0d16:  ldloc.s    V_4
    IL_0d18:  dup
    IL_0d19:  ldc.i4.1
    IL_0d1a:  add
    IL_0d1b:  stloc.s    V_4
    IL_0d1d:  ldelem.i4
    IL_0d1e:  stloc.s    V_0
    IL_0d20:  ldloc.s    V_3
    IL_0d22:  ldloc.s    V_2
    IL_0d24:  ldc.i4.1
    IL_0d25:  sub
    IL_0d26:  dup
    IL_0d27:  stloc.s    V_2
    IL_0d29:  ldloc.s    V_6
    IL_0d2b:  stelem.i4
    IL_0d2c:  ldloc.s    V_3
    IL_0d2e:  ldloc.s    V_2
    IL_0d30:  ldc.i4.1
    IL_0d31:  sub
    IL_0d32:  dup
    IL_0d33:  stloc.s    V_2
    IL_0d35:  ldc.i4.s   9
    IL_0d37:  stelem.i4
    IL_0d38:  br         IL_080e

    IL_0d3d:  ldloc.s    V_5
    IL_0d3f:  ldloc.s    V_4
    IL_0d41:  ldloc.s    V_3
    IL_0d43:  ldloc.s    V_2
    IL_0d45:  dup
    IL_0d46:  ldc.i4.1
    IL_0d47:  add
    IL_0d48:  stloc.s    V_2
    IL_0d4a:  ldelem.i4
    IL_0d4b:  stelem.i4
    IL_0d4c:  ldloc.s    V_5
    IL_0d4e:  ldloc.s    V_4
    IL_0d50:  ldc.i4.1
    IL_0d51:  sub
    IL_0d52:  dup
    IL_0d53:  stloc.s    V_4
    IL_0d55:  ldloc.s    V_6
    IL_0d57:  stelem.i4
    IL_0d58:  br         IL_09ff

    IL_0d5d:  ldarg.0
    IL_0d5e:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0d63:  ldloc.s    V_3
    IL_0d65:  ldloc.s    V_2
    IL_0d67:  dup
    IL_0d68:  ldc.i4.1
    IL_0d69:  add
    IL_0d6a:  stloc.s    V_2
    IL_0d6c:  ldelem.i4
    IL_0d6d:  stloc.s    V_0
    IL_0d6f:  ldloc.s    V_3
    IL_0d71:  ldloc.s    V_2
    IL_0d73:  dup
    IL_0d74:  ldc.i4.1
    IL_0d75:  add
    IL_0d76:  stloc.s    V_2
    IL_0d78:  ldelem.i4
    IL_0d79:  stloc.s    V_6
    IL_0d7b:  ldloc.s    V_6
    IL_0d7d:  ldc.i4.s   0
    IL_0d7f:  ble        IL_0912

    IL_0d84:  ldloc.s    V_3
    IL_0d86:  ldloc.s    V_2
    IL_0d88:  ldc.i4.1
    IL_0d89:  sub
    IL_0d8a:  dup
    IL_0d8b:  stloc.s    V_2
    IL_0d8d:  ldloc.s    V_6
    IL_0d8f:  ldc.i4.s   1
    IL_0d91:  sub
    IL_0d92:  stelem.i4
    IL_0d93:  ldloc.s    V_3
    IL_0d95:  ldloc.s    V_2
    IL_0d97:  ldc.i4.1
    IL_0d98:  sub
    IL_0d99:  dup
    IL_0d9a:  stloc.s    V_2
    IL_0d9c:  ldloc.s    V_0
    IL_0d9e:  ldc.i4.s   1
    IL_0da0:  sub
    IL_0da1:  stelem.i4
    IL_0da2:  ldloc.s    V_3
    IL_0da4:  ldloc.s    V_2
    IL_0da6:  ldc.i4.1
    IL_0da7:  sub
    IL_0da8:  dup
    IL_0da9:  stloc.s    V_2
    IL_0dab:  ldc.i4.s   15
    IL_0dad:  stelem.i4
    IL_0dae:  br         IL_0912
  } // end of method BracketedControllerOpenPatternRunner2::Go

  .method public virtual instance bool  FindFirstChar() cil managed
  {
    // Code size       124 (0x7c)
    .maxstack  8
    .locals init (int32 V_0,
             string V_1,
             int32 V_2,
             int32 V_3)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string [System]System.Text.RegularExpressions.RegexRunner::runtext
    IL_0006:  stloc.s    V_1
    IL_0008:  ldarg.0
    IL_0009:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextend
    IL_000e:  stloc.s    V_3
    IL_0010:  ldarg.0
    IL_0011:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextpos
    IL_0016:  ldc.i4.s   0
    IL_0018:  add
    IL_0019:  stloc.s    V_0
    IL_001b:  br.s       IL_0024

    IL_001d:  ldc.i4.s   1
    IL_001f:  ldloc.s    V_0
    IL_0021:  add
    IL_0022:  stloc.s    V_0
    IL_0024:  ldloc.s    V_0
    IL_0026:  ldloc.s    V_3
    IL_0028:  bge        IL_006d

    IL_002d:  ldloc.s    V_1
    IL_002f:  ldloc.s    V_0
    IL_0031:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0036:  dup
    IL_0037:  stloc.s    V_2
    IL_0039:  ldc.i4.s   36
    IL_003b:  beq        IL_005e

    IL_0040:  ldloc.s    V_2
    IL_0042:  ldc.i4.s   36
    IL_0044:  sub
    IL_0045:  dup
    IL_0046:  stloc.s    V_2
    IL_0048:  ldc.i4.s   0
    IL_004a:  bgt.un.s   IL_001d

    IL_004c:  ldloc.s    V_2
    IL_004e:  switch     ( 
                          IL_0057)
    IL_0057:  ldc.i4.s   0
    IL_0059:  br         IL_001f

    IL_005e:  ldloc.s    V_0
    IL_0060:  stloc.s    V_2
    IL_0062:  ldarg.0
    IL_0063:  ldloc.s    V_2
    IL_0065:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextpos
    IL_006a:  ldc.i4.s   1
    IL_006c:  ret

    IL_006d:  ldarg.0
    IL_006e:  ldarg.0
    IL_006f:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextend
    IL_0074:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextpos
    IL_0079:  ldc.i4.s   0
    IL_007b:  ret
  } // end of method BracketedControllerOpenPatternRunner2::FindFirstChar

  .method public virtual instance void  InitTrackCount() cil managed
  {
    // Code size       9 (0x9)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.s   32
    IL_0003:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtrackcount
    IL_0008:  ret
  } // end of method BracketedControllerOpenPatternRunner2::InitTrackCount

  .method public specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [System]System.Text.RegularExpressions.RegexRunner::.ctor()
    IL_0006:  ret
  } // end of method BracketedControllerOpenPatternRunner2::.ctor

} // end of class Xeora.Web.RegularExpressions.BracketedControllerOpenPatternRunner2

.class private auto ansi Xeora.Web.RegularExpressions.BracketedControllerOpenPatternFactory2
       extends [System]System.Text.RegularExpressions.RegexRunnerFactory
{
  .method public virtual instance class [System]System.Text.RegularExpressions.RegexRunner 
          CreateInstance() cil managed
  {
    // Code size       6 (0x6)
    .maxstack  1
    IL_0000:  newobj     instance void Xeora.Web.RegularExpressions.BracketedControllerOpenPatternRunner2::.ctor()
    IL_0005:  ret
  } // end of method BracketedControllerOpenPatternFactory2::CreateInstance

  .method public specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [System]System.Text.RegularExpressions.RegexRunnerFactory::.ctor()
    IL_0006:  ret
  } // end of method BracketedControllerOpenPatternFactory2::.ctor

} // end of class Xeora.Web.RegularExpressions.BracketedControllerOpenPatternFactory2

.class public auto ansi Xeora.Web.RegularExpressions.BracketedControllerOpenPattern
       extends [System]System.Text.RegularExpressions.Regex
{
  .method public specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       351 (0x15f)
    .maxstack  5
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [System]System.Text.RegularExpressions.Regex::.ctor()
    IL_0006:  ldarg.0
    IL_0007:  ldstr      "\\$((\?<ItemID>\\w+)|(\?<DirectiveType>\\w)(\\#\\d+(\\+)\?)"
    + "\?(\\[[\\.\\w\\-]+\\])\?\\:(\?<ItemID>[\\.\\w\\-]+))\\:\\{"
    IL_000c:  stfld      string [System]System.Text.RegularExpressions.Regex::pattern
    IL_0011:  ldarg.0
    IL_0012:  ldc.i4.s   2
    IL_0014:  stfld      valuetype [System]System.Text.RegularExpressions.RegexOptions [System]System.Text.RegularExpressions.Regex::roptions
    IL_0019:  ldarg.0
    IL_001a:  ldc.i4     0xffffd8f0
    IL_001f:  conv.i8
    IL_0020:  call       valuetype [mscorlib]System.TimeSpan [mscorlib]System.TimeSpan::FromTicks(int64)
    IL_0025:  stfld      valuetype [mscorlib]System.TimeSpan [System]System.Text.RegularExpressions.Regex::internalMatchTimeout
    IL_002a:  ldarg.0
    IL_002b:  newobj     instance void Xeora.Web.RegularExpressions.BracketedControllerOpenPatternFactory2::.ctor()
    IL_0030:  stfld      class [System]System.Text.RegularExpressions.RegexRunnerFactory [System]System.Text.RegularExpressions.Regex::factory
    IL_0035:  ldarg.0
    IL_0036:  newobj     instance void [mscorlib]System.Collections.Hashtable::.ctor()
    IL_003b:  stfld      class [mscorlib]System.Collections.Hashtable [System]System.Text.RegularExpressions.Regex::capnames
    IL_0040:  ldarg.0
    IL_0041:  ldfld      class [mscorlib]System.Collections.Hashtable [System]System.Text.RegularExpressions.Regex::capnames
    IL_0046:  ldstr      "ItemID"
    IL_004b:  ldc.i4.s   5
    IL_004d:  box        [mscorlib]System.Int32
    IL_0052:  callvirt   instance void [mscorlib]System.Collections.Hashtable::Add(object,
                                                                                   object)
    IL_0057:  ldarg.0
    IL_0058:  ldfld      class [mscorlib]System.Collections.Hashtable [System]System.Text.RegularExpressions.Regex::capnames
    IL_005d:  ldstr      "0"
    IL_0062:  ldc.i4.s   0
    IL_0064:  box        [mscorlib]System.Int32
    IL_0069:  callvirt   instance void [mscorlib]System.Collections.Hashtable::Add(object,
                                                                                   object)
    IL_006e:  ldarg.0
    IL_006f:  ldfld      class [mscorlib]System.Collections.Hashtable [System]System.Text.RegularExpressions.Regex::capnames
    IL_0074:  ldstr      "1"
    IL_0079:  ldc.i4.s   1
    IL_007b:  box        [mscorlib]System.Int32
    IL_0080:  callvirt   instance void [mscorlib]System.Collections.Hashtable::Add(object,
                                                                                   object)
    IL_0085:  ldarg.0
    IL_0086:  ldfld      class [mscorlib]System.Collections.Hashtable [System]System.Text.RegularExpressions.Regex::capnames
    IL_008b:  ldstr      "2"
    IL_0090:  ldc.i4.s   2
    IL_0092:  box        [mscorlib]System.Int32
    IL_0097:  callvirt   instance void [mscorlib]System.Collections.Hashtable::Add(object,
                                                                                   object)
    IL_009c:  ldarg.0
    IL_009d:  ldfld      class [mscorlib]System.Collections.Hashtable [System]System.Text.RegularExpressions.Regex::capnames
    IL_00a2:  ldstr      "3"
    IL_00a7:  ldc.i4.s   3
    IL_00a9:  box        [mscorlib]System.Int32
    IL_00ae:  callvirt   instance void [mscorlib]System.Collections.Hashtable::Add(object,
                                                                                   object)
    IL_00b3:  ldarg.0
    IL_00b4:  ldfld      class [mscorlib]System.Collections.Hashtable [System]System.Text.RegularExpressions.Regex::capnames
    IL_00b9:  ldstr      "4"
    IL_00be:  ldc.i4.s   4
    IL_00c0:  box        [mscorlib]System.Int32
    IL_00c5:  callvirt   instance void [mscorlib]System.Collections.Hashtable::Add(object,
                                                                                   object)
    IL_00ca:  ldarg.0
    IL_00cb:  ldfld      class [mscorlib]System.Collections.Hashtable [System]System.Text.RegularExpressions.Regex::capnames
    IL_00d0:  ldstr      "DirectiveType"
    IL_00d5:  ldc.i4.s   6
    IL_00d7:  box        [mscorlib]System.Int32
    IL_00dc:  callvirt   instance void [mscorlib]System.Collections.Hashtable::Add(object,
                                                                                   object)
    IL_00e1:  ldarg.0
    IL_00e2:  ldc.i4.s   7
    IL_00e4:  newarr     [mscorlib]System.String
    IL_00e9:  stfld      string[] [System]System.Text.RegularExpressions.Regex::capslist
    IL_00ee:  ldarg.0
    IL_00ef:  ldfld      string[] [System]System.Text.RegularExpressions.Regex::capslist
    IL_00f4:  ldc.i4.s   0
    IL_00f6:  ldstr      "0"
    IL_00fb:  stelem.ref
    IL_00fc:  ldarg.0
    IL_00fd:  ldfld      string[] [System]System.Text.RegularExpressions.Regex::capslist
    IL_0102:  ldc.i4.s   1
    IL_0104:  ldstr      "1"
    IL_0109:  stelem.ref
    IL_010a:  ldarg.0
    IL_010b:  ldfld      string[] [System]System.Text.RegularExpressions.Regex::capslist
    IL_0110:  ldc.i4.s   2
    IL_0112:  ldstr      "2"
    IL_0117:  stelem.ref
    IL_0118:  ldarg.0
    IL_0119:  ldfld      string[] [System]System.Text.RegularExpressions.Regex::capslist
    IL_011e:  ldc.i4.s   3
    IL_0120:  ldstr      "3"
    IL_0125:  stelem.ref
    IL_0126:  ldarg.0
    IL_0127:  ldfld      string[] [System]System.Text.RegularExpressions.Regex::capslist
    IL_012c:  ldc.i4.s   4
    IL_012e:  ldstr      "4"
    IL_0133:  stelem.ref
    IL_0134:  ldarg.0
    IL_0135:  ldfld      string[] [System]System.Text.RegularExpressions.Regex::capslist
    IL_013a:  ldc.i4.s   5
    IL_013c:  ldstr      "ItemID"
    IL_0141:  stelem.ref
    IL_0142:  ldarg.0
    IL_0143:  ldfld      string[] [System]System.Text.RegularExpressions.Regex::capslist
    IL_0148:  ldc.i4.s   6
    IL_014a:  ldstr      "DirectiveType"
    IL_014f:  stelem.ref
    IL_0150:  ldarg.0
    IL_0151:  ldc.i4.s   7
    IL_0153:  stfld      int32 [System]System.Text.RegularExpressions.Regex::capsize
    IL_0158:  ldarg.0
    IL_0159:  call       instance void [System]System.Text.RegularExpressions.Regex::InitializeReferences()
    IL_015e:  ret
  } // end of method BracketedControllerOpenPattern::.ctor

  .method public specialname rtspecialname 
          instance void  .ctor(valuetype [mscorlib]System.TimeSpan A_1) cil managed
  {
    // Code size       20 (0x14)
    .maxstack  4
    IL_0000:  ldarg.0
    IL_0001:  call       instance void Xeora.Web.RegularExpressions.BracketedControllerOpenPattern::.ctor()
    IL_0006:  ldarg.1
    IL_0007:  call       void [System]System.Text.RegularExpressions.Regex::ValidateMatchTimeout(valuetype [mscorlib]System.TimeSpan)
    IL_000c:  ldarg.0
    IL_000d:  ldarg.1
    IL_000e:  stfld      valuetype [mscorlib]System.TimeSpan [System]System.Text.RegularExpressions.Regex::internalMatchTimeout
    IL_0013:  ret
  } // end of method BracketedControllerOpenPattern::.ctor

} // end of class Xeora.Web.RegularExpressions.BracketedControllerOpenPattern

.class private auto ansi Xeora.Web.RegularExpressions.BracketedControllerSeparatorPatternRunner3
       extends [System]System.Text.RegularExpressions.RegexRunner
{
  .method public virtual instance void  Go() cil managed
  {
    // Code size       847 (0x34f)
    .maxstack  21
    .locals init (int32 V_0,
             string V_1,
             int32 V_2,
             int32[] V_3,
             int32 V_4,
             int32[] V_5,
             int32 V_6,
             int32 V_7,
             int32 V_8,
             int32 V_9,
             int32 V_10,
             int32 V_11)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string [System]System.Text.RegularExpressions.RegexRunner::runtext
    IL_0006:  stloc.s    V_1
    IL_0008:  ldarg.0
    IL_0009:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextstart
    IL_000e:  stloc.s    V_11
    IL_0010:  ldarg.0
    IL_0011:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextbeg
    IL_0016:  stloc.s    V_9
    IL_0018:  ldarg.0
    IL_0019:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextend
    IL_001e:  stloc.s    V_10
    IL_0020:  ldarg.0
    IL_0021:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextpos
    IL_0026:  stloc.s    V_0
    IL_0028:  ldarg.0
    IL_0029:  ldfld      int32[] [System]System.Text.RegularExpressions.RegexRunner::runtrack
    IL_002e:  stloc.s    V_3
    IL_0030:  ldarg.0
    IL_0031:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtrackpos
    IL_0036:  stloc.s    V_2
    IL_0038:  ldarg.0
    IL_0039:  ldfld      int32[] [System]System.Text.RegularExpressions.RegexRunner::runstack
    IL_003e:  stloc.s    V_5
    IL_0040:  ldarg.0
    IL_0041:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runstackpos
    IL_0046:  stloc.s    V_4
    IL_0048:  ldarg.0
    IL_0049:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_004e:  ldloc.s    V_3
    IL_0050:  ldloc.s    V_2
    IL_0052:  ldc.i4.1
    IL_0053:  sub
    IL_0054:  dup
    IL_0055:  stloc.s    V_2
    IL_0057:  ldloc.s    V_0
    IL_0059:  stelem.i4
    IL_005a:  ldloc.s    V_3
    IL_005c:  ldloc.s    V_2
    IL_005e:  ldc.i4.1
    IL_005f:  sub
    IL_0060:  dup
    IL_0061:  stloc.s    V_2
    IL_0063:  ldc.i4.s   0
    IL_0065:  stelem.i4
    IL_0066:  ldarg.0
    IL_0067:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_006c:  ldloc.s    V_5
    IL_006e:  ldloc.s    V_4
    IL_0070:  ldc.i4.1
    IL_0071:  sub
    IL_0072:  dup
    IL_0073:  stloc.s    V_4
    IL_0075:  ldloc.s    V_0
    IL_0077:  stelem.i4
    IL_0078:  ldloc.s    V_3
    IL_007a:  ldloc.s    V_2
    IL_007c:  ldc.i4.1
    IL_007d:  sub
    IL_007e:  dup
    IL_007f:  stloc.s    V_2
    IL_0081:  ldc.i4.s   1
    IL_0083:  stelem.i4
    IL_0084:  ldarg.0
    IL_0085:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_008a:  ldc.i4.s   2
    IL_008c:  ldloc.s    V_10
    IL_008e:  ldloc.s    V_0
    IL_0090:  sub
    IL_0091:  bgt        IL_0256

    IL_0096:  ldloc.s    V_1
    IL_0098:  ldloc.s    V_0
    IL_009a:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_009f:  ldc.i4.s   125
    IL_00a1:  bne.un     IL_0256

    IL_00a6:  ldloc.s    V_1
    IL_00a8:  ldloc.s    V_0
    IL_00aa:  ldc.i4.s   1
    IL_00ac:  add
    IL_00ad:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_00b2:  ldc.i4.s   58
    IL_00b4:  bne.un     IL_0256

    IL_00b9:  ldloc.s    V_0
    IL_00bb:  ldc.i4.s   2
    IL_00bd:  add
    IL_00be:  stloc.s    V_0
    IL_00c0:  ldarg.0
    IL_00c1:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_00c6:  ldloc.s    V_5
    IL_00c8:  ldloc.s    V_4
    IL_00ca:  ldc.i4.1
    IL_00cb:  sub
    IL_00cc:  dup
    IL_00cd:  stloc.s    V_4
    IL_00cf:  ldloc.s    V_0
    IL_00d1:  stelem.i4
    IL_00d2:  ldloc.s    V_3
    IL_00d4:  ldloc.s    V_2
    IL_00d6:  ldc.i4.1
    IL_00d7:  sub
    IL_00d8:  dup
    IL_00d9:  stloc.s    V_2
    IL_00db:  ldc.i4.s   1
    IL_00dd:  stelem.i4
    IL_00de:  ldarg.0
    IL_00df:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_00e4:  ldc.i4.s   1
    IL_00e6:  ldloc.s    V_10
    IL_00e8:  ldloc.s    V_0
    IL_00ea:  sub
    IL_00eb:  bgt        IL_0256

    IL_00f0:  ldloc.s    V_0
    IL_00f2:  ldc.i4.s   1
    IL_00f4:  add
    IL_00f5:  stloc.s    V_0
    IL_00f7:  ldc.i4.s   1
    IL_00f9:  stloc.s    V_6
    IL_00fb:  ldloc.s    V_1
    IL_00fd:  ldloc.s    V_0
    IL_00ff:  ldloc.s    V_6
    IL_0101:  dup
    IL_0102:  ldc.i4.s   1
    IL_0104:  sub
    IL_0105:  stloc.s    V_6
    IL_0107:  sub
    IL_0108:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_010d:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_0112:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0117:  brfalse    IL_0256

    IL_011c:  ldloc.s    V_6
    IL_011e:  ldc.i4.s   0
    IL_0120:  bgt        IL_00fb

    IL_0125:  ldarg.0
    IL_0126:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_012b:  ldloc.s    V_10
    IL_012d:  ldloc.s    V_0
    IL_012f:  sub
    IL_0130:  dup
    IL_0131:  stloc.s    V_7
    IL_0133:  ldc.i4.s   1
    IL_0135:  add
    IL_0136:  stloc.s    V_6
    IL_0138:  ldloc.s    V_6
    IL_013a:  ldc.i4.s   1
    IL_013c:  sub
    IL_013d:  dup
    IL_013e:  stloc.s    V_6
    IL_0140:  ldc.i4.s   0
    IL_0142:  ble        IL_016c

    IL_0147:  ldloc.s    V_1
    IL_0149:  ldloc.s    V_0
    IL_014b:  dup
    IL_014c:  ldc.i4.s   1
    IL_014e:  add
    IL_014f:  stloc.s    V_0
    IL_0151:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0156:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_015b:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0160:  brtrue     IL_0138

    IL_0165:  ldloc.s    V_0
    IL_0167:  ldc.i4.s   1
    IL_0169:  sub
    IL_016a:  stloc.s    V_0
    IL_016c:  ldloc.s    V_7
    IL_016e:  ldloc.s    V_6
    IL_0170:  ble.s      IL_019f

    IL_0172:  ldloc.s    V_3
    IL_0174:  ldloc.s    V_2
    IL_0176:  ldc.i4.1
    IL_0177:  sub
    IL_0178:  dup
    IL_0179:  stloc.s    V_2
    IL_017b:  ldloc.s    V_7
    IL_017d:  ldloc.s    V_6
    IL_017f:  sub
    IL_0180:  ldc.i4.s   1
    IL_0182:  sub
    IL_0183:  stelem.i4
    IL_0184:  ldloc.s    V_3
    IL_0186:  ldloc.s    V_2
    IL_0188:  ldc.i4.1
    IL_0189:  sub
    IL_018a:  dup
    IL_018b:  stloc.s    V_2
    IL_018d:  ldloc.s    V_0
    IL_018f:  ldc.i4.s   1
    IL_0191:  sub
    IL_0192:  stelem.i4
    IL_0193:  ldloc.s    V_3
    IL_0195:  ldloc.s    V_2
    IL_0197:  ldc.i4.1
    IL_0198:  sub
    IL_0199:  dup
    IL_019a:  stloc.s    V_2
    IL_019c:  ldc.i4.s   2
    IL_019e:  stelem.i4
    IL_019f:  ldarg.0
    IL_01a0:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_01a5:  ldloc.s    V_5
    IL_01a7:  ldloc.s    V_4
    IL_01a9:  dup
    IL_01aa:  ldc.i4.1
    IL_01ab:  add
    IL_01ac:  stloc.s    V_4
    IL_01ae:  ldelem.i4
    IL_01af:  stloc.s    V_6
    IL_01b1:  ldarg.0
    IL_01b2:  ldc.i4.s   1
    IL_01b4:  ldloc.s    V_6
    IL_01b6:  ldloc.s    V_0
    IL_01b8:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_01bd:  ldloc.s    V_3
    IL_01bf:  ldloc.s    V_2
    IL_01c1:  ldc.i4.1
    IL_01c2:  sub
    IL_01c3:  dup
    IL_01c4:  stloc.s    V_2
    IL_01c6:  ldloc.s    V_6
    IL_01c8:  stelem.i4
    IL_01c9:  ldloc.s    V_3
    IL_01cb:  ldloc.s    V_2
    IL_01cd:  ldc.i4.1
    IL_01ce:  sub
    IL_01cf:  dup
    IL_01d0:  stloc.s    V_2
    IL_01d2:  ldc.i4.s   3
    IL_01d4:  stelem.i4
    IL_01d5:  ldarg.0
    IL_01d6:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_01db:  ldc.i4.s   2
    IL_01dd:  ldloc.s    V_10
    IL_01df:  ldloc.s    V_0
    IL_01e1:  sub
    IL_01e2:  bgt        IL_0256

    IL_01e7:  ldloc.s    V_1
    IL_01e9:  ldloc.s    V_0
    IL_01eb:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_01f0:  ldc.i4.s   58
    IL_01f2:  bne.un     IL_0256

    IL_01f7:  ldloc.s    V_1
    IL_01f9:  ldloc.s    V_0
    IL_01fb:  ldc.i4.s   1
    IL_01fd:  add
    IL_01fe:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0203:  ldc.i4.s   123
    IL_0205:  bne.un     IL_0256

    IL_020a:  ldloc.s    V_0
    IL_020c:  ldc.i4.s   2
    IL_020e:  add
    IL_020f:  stloc.s    V_0
    IL_0211:  ldarg.0
    IL_0212:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0217:  ldloc.s    V_5
    IL_0219:  ldloc.s    V_4
    IL_021b:  dup
    IL_021c:  ldc.i4.1
    IL_021d:  add
    IL_021e:  stloc.s    V_4
    IL_0220:  ldelem.i4
    IL_0221:  stloc.s    V_6
    IL_0223:  ldarg.0
    IL_0224:  ldc.i4.s   0
    IL_0226:  ldloc.s    V_6
    IL_0228:  ldloc.s    V_0
    IL_022a:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_022f:  ldloc.s    V_3
    IL_0231:  ldloc.s    V_2
    IL_0233:  ldc.i4.1
    IL_0234:  sub
    IL_0235:  dup
    IL_0236:  stloc.s    V_2
    IL_0238:  ldloc.s    V_6
    IL_023a:  stelem.i4
    IL_023b:  ldloc.s    V_3
    IL_023d:  ldloc.s    V_2
    IL_023f:  ldc.i4.1
    IL_0240:  sub
    IL_0241:  dup
    IL_0242:  stloc.s    V_2
    IL_0244:  ldc.i4.s   3
    IL_0246:  stelem.i4
    IL_0247:  ldarg.0
    IL_0248:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_024d:  ldarg.0
    IL_024e:  ldloc.s    V_0
    IL_0250:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextpos
    IL_0255:  ret

    IL_0256:  ldarg.0
    IL_0257:  ldloc.s    V_2
    IL_0259:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtrackpos
    IL_025e:  ldarg.0
    IL_025f:  ldloc.s    V_4
    IL_0261:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runstackpos
    IL_0266:  ldarg.0
    IL_0267:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::EnsureStorage()
    IL_026c:  ldarg.0
    IL_026d:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtrackpos
    IL_0272:  stloc.s    V_2
    IL_0274:  ldarg.0
    IL_0275:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runstackpos
    IL_027a:  stloc.s    V_4
    IL_027c:  ldarg.0
    IL_027d:  ldfld      int32[] [System]System.Text.RegularExpressions.RegexRunner::runtrack
    IL_0282:  stloc.s    V_3
    IL_0284:  ldarg.0
    IL_0285:  ldfld      int32[] [System]System.Text.RegularExpressions.RegexRunner::runstack
    IL_028a:  stloc.s    V_5
    IL_028c:  ldloc.s    V_3
    IL_028e:  ldloc.s    V_2
    IL_0290:  dup
    IL_0291:  ldc.i4.1
    IL_0292:  add
    IL_0293:  stloc.s    V_2
    IL_0295:  ldelem.i4
    IL_0296:  switch     ( 
                          IL_02ab,
                          IL_02c2,
                          IL_02d4,
                          IL_032a)
    IL_02ab:  ldarg.0
    IL_02ac:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_02b1:  ldloc.s    V_3
    IL_02b3:  ldloc.s    V_2
    IL_02b5:  dup
    IL_02b6:  ldc.i4.1
    IL_02b7:  add
    IL_02b8:  stloc.s    V_2
    IL_02ba:  ldelem.i4
    IL_02bb:  stloc.s    V_0
    IL_02bd:  br         IL_0247

    IL_02c2:  ldarg.0
    IL_02c3:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_02c8:  ldloc.s    V_4
    IL_02ca:  ldc.i4.s   1
    IL_02cc:  add
    IL_02cd:  stloc.s    V_4
    IL_02cf:  br         IL_0256

    IL_02d4:  ldarg.0
    IL_02d5:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_02da:  ldloc.s    V_3
    IL_02dc:  ldloc.s    V_2
    IL_02de:  dup
    IL_02df:  ldc.i4.1
    IL_02e0:  add
    IL_02e1:  stloc.s    V_2
    IL_02e3:  ldelem.i4
    IL_02e4:  stloc.s    V_0
    IL_02e6:  ldloc.s    V_3
    IL_02e8:  ldloc.s    V_2
    IL_02ea:  dup
    IL_02eb:  ldc.i4.1
    IL_02ec:  add
    IL_02ed:  stloc.s    V_2
    IL_02ef:  ldelem.i4
    IL_02f0:  stloc.s    V_6
    IL_02f2:  ldloc.s    V_6
    IL_02f4:  ldc.i4.s   0
    IL_02f6:  ble        IL_019f

    IL_02fb:  ldloc.s    V_3
    IL_02fd:  ldloc.s    V_2
    IL_02ff:  ldc.i4.1
    IL_0300:  sub
    IL_0301:  dup
    IL_0302:  stloc.s    V_2
    IL_0304:  ldloc.s    V_6
    IL_0306:  ldc.i4.s   1
    IL_0308:  sub
    IL_0309:  stelem.i4
    IL_030a:  ldloc.s    V_3
    IL_030c:  ldloc.s    V_2
    IL_030e:  ldc.i4.1
    IL_030f:  sub
    IL_0310:  dup
    IL_0311:  stloc.s    V_2
    IL_0313:  ldloc.s    V_0
    IL_0315:  ldc.i4.s   1
    IL_0317:  sub
    IL_0318:  stelem.i4
    IL_0319:  ldloc.s    V_3
    IL_031b:  ldloc.s    V_2
    IL_031d:  ldc.i4.1
    IL_031e:  sub
    IL_031f:  dup
    IL_0320:  stloc.s    V_2
    IL_0322:  ldc.i4.s   2
    IL_0324:  stelem.i4
    IL_0325:  br         IL_019f

    IL_032a:  ldarg.0
    IL_032b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0330:  ldloc.s    V_5
    IL_0332:  ldloc.s    V_4
    IL_0334:  ldc.i4.1
    IL_0335:  sub
    IL_0336:  dup
    IL_0337:  stloc.s    V_4
    IL_0339:  ldloc.s    V_3
    IL_033b:  ldloc.s    V_2
    IL_033d:  dup
    IL_033e:  ldc.i4.1
    IL_033f:  add
    IL_0340:  stloc.s    V_2
    IL_0342:  ldelem.i4
    IL_0343:  stelem.i4
    IL_0344:  ldarg.0
    IL_0345:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Uncapture()
    IL_034a:  br         IL_0256
  } // end of method BracketedControllerSeparatorPatternRunner3::Go

  .method public virtual instance bool  FindFirstChar() cil managed
  {
    // Code size       887 (0x377)
    .maxstack  78
    .locals init (int32 V_0,
             string V_1,
             int32 V_2,
             int32 V_3)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string [System]System.Text.RegularExpressions.RegexRunner::runtext
    IL_0006:  stloc.s    V_1
    IL_0008:  ldarg.0
    IL_0009:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextend
    IL_000e:  stloc.s    V_3
    IL_0010:  ldarg.0
    IL_0011:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextpos
    IL_0016:  ldc.i4.s   1
    IL_0018:  add
    IL_0019:  stloc.s    V_0
    IL_001b:  br.s       IL_0024

    IL_001d:  ldc.i4.s   2
    IL_001f:  ldloc.s    V_0
    IL_0021:  add
    IL_0022:  stloc.s    V_0
    IL_0024:  ldloc.s    V_0
    IL_0026:  ldloc.s    V_3
    IL_0028:  bge        IL_0368

    IL_002d:  ldloc.s    V_1
    IL_002f:  ldloc.s    V_0
    IL_0031:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0036:  dup
    IL_0037:  stloc.s    V_2
    IL_0039:  ldc.i4.s   58
    IL_003b:  beq        IL_033f

    IL_0040:  ldloc.s    V_2
    IL_0042:  ldc.i4.s   58
    IL_0044:  sub
    IL_0045:  dup
    IL_0046:  stloc.s    V_2
    IL_0048:  ldc.i4.s   67
    IL_004a:  bgt.un.s   IL_001d

    IL_004c:  ldloc.s    V_2
    IL_004e:  switch     ( 
                          IL_0163,
                          IL_016a,
                          IL_0171,
                          IL_0178,
                          IL_017f,
                          IL_0186,
                          IL_018d,
                          IL_0194,
                          IL_019b,
                          IL_01a2,
                          IL_01a9,
                          IL_01b0,
                          IL_01b7,
                          IL_01be,
                          IL_01c5,
                          IL_01cc,
                          IL_01d3,
                          IL_01da,
                          IL_01e1,
                          IL_01e8,
                          IL_01ef,
                          IL_01f6,
                          IL_01fd,
                          IL_0204,
                          IL_020b,
                          IL_0212,
                          IL_0219,
                          IL_0220,
                          IL_0227,
                          IL_022e,
                          IL_0235,
                          IL_023c,
                          IL_0243,
                          IL_024a,
                          IL_0251,
                          IL_0258,
                          IL_025f,
                          IL_0266,
                          IL_026d,
                          IL_0274,
                          IL_027b,
                          IL_0282,
                          IL_0289,
                          IL_0290,
                          IL_0297,
                          IL_029e,
                          IL_02a5,
                          IL_02ac,
                          IL_02b3,
                          IL_02ba,
                          IL_02c1,
                          IL_02c8,
                          IL_02cf,
                          IL_02d6,
                          IL_02dd,
                          IL_02e4,
                          IL_02eb,
                          IL_02f2,
                          IL_02f9,
                          IL_0300,
                          IL_0307,
                          IL_030e,
                          IL_0315,
                          IL_031c,
                          IL_0323,
                          IL_032a,
                          IL_0331,
                          IL_0338)
    IL_0163:  ldc.i4.s   0
    IL_0165:  br         IL_001f

    IL_016a:  ldc.i4.s   2
    IL_016c:  br         IL_001f

    IL_0171:  ldc.i4.s   2
    IL_0173:  br         IL_001f

    IL_0178:  ldc.i4.s   2
    IL_017a:  br         IL_001f

    IL_017f:  ldc.i4.s   2
    IL_0181:  br         IL_001f

    IL_0186:  ldc.i4.s   2
    IL_0188:  br         IL_001f

    IL_018d:  ldc.i4.s   2
    IL_018f:  br         IL_001f

    IL_0194:  ldc.i4.s   2
    IL_0196:  br         IL_001f

    IL_019b:  ldc.i4.s   2
    IL_019d:  br         IL_001f

    IL_01a2:  ldc.i4.s   2
    IL_01a4:  br         IL_001f

    IL_01a9:  ldc.i4.s   2
    IL_01ab:  br         IL_001f

    IL_01b0:  ldc.i4.s   2
    IL_01b2:  br         IL_001f

    IL_01b7:  ldc.i4.s   2
    IL_01b9:  br         IL_001f

    IL_01be:  ldc.i4.s   2
    IL_01c0:  br         IL_001f

    IL_01c5:  ldc.i4.s   2
    IL_01c7:  br         IL_001f

    IL_01cc:  ldc.i4.s   2
    IL_01ce:  br         IL_001f

    IL_01d3:  ldc.i4.s   2
    IL_01d5:  br         IL_001f

    IL_01da:  ldc.i4.s   2
    IL_01dc:  br         IL_001f

    IL_01e1:  ldc.i4.s   2
    IL_01e3:  br         IL_001f

    IL_01e8:  ldc.i4.s   2
    IL_01ea:  br         IL_001f

    IL_01ef:  ldc.i4.s   2
    IL_01f1:  br         IL_001f

    IL_01f6:  ldc.i4.s   2
    IL_01f8:  br         IL_001f

    IL_01fd:  ldc.i4.s   2
    IL_01ff:  br         IL_001f

    IL_0204:  ldc.i4.s   2
    IL_0206:  br         IL_001f

    IL_020b:  ldc.i4.s   2
    IL_020d:  br         IL_001f

    IL_0212:  ldc.i4.s   2
    IL_0214:  br         IL_001f

    IL_0219:  ldc.i4.s   2
    IL_021b:  br         IL_001f

    IL_0220:  ldc.i4.s   2
    IL_0222:  br         IL_001f

    IL_0227:  ldc.i4.s   2
    IL_0229:  br         IL_001f

    IL_022e:  ldc.i4.s   2
    IL_0230:  br         IL_001f

    IL_0235:  ldc.i4.s   2
    IL_0237:  br         IL_001f

    IL_023c:  ldc.i4.s   2
    IL_023e:  br         IL_001f

    IL_0243:  ldc.i4.s   2
    IL_0245:  br         IL_001f

    IL_024a:  ldc.i4.s   2
    IL_024c:  br         IL_001f

    IL_0251:  ldc.i4.s   2
    IL_0253:  br         IL_001f

    IL_0258:  ldc.i4.s   2
    IL_025a:  br         IL_001f

    IL_025f:  ldc.i4.s   2
    IL_0261:  br         IL_001f

    IL_0266:  ldc.i4.s   2
    IL_0268:  br         IL_001f

    IL_026d:  ldc.i4.s   2
    IL_026f:  br         IL_001f

    IL_0274:  ldc.i4.s   2
    IL_0276:  br         IL_001f

    IL_027b:  ldc.i4.s   2
    IL_027d:  br         IL_001f

    IL_0282:  ldc.i4.s   2
    IL_0284:  br         IL_001f

    IL_0289:  ldc.i4.s   2
    IL_028b:  br         IL_001f

    IL_0290:  ldc.i4.s   2
    IL_0292:  br         IL_001f

    IL_0297:  ldc.i4.s   2
    IL_0299:  br         IL_001f

    IL_029e:  ldc.i4.s   2
    IL_02a0:  br         IL_001f

    IL_02a5:  ldc.i4.s   2
    IL_02a7:  br         IL_001f

    IL_02ac:  ldc.i4.s   2
    IL_02ae:  br         IL_001f

    IL_02b3:  ldc.i4.s   2
    IL_02b5:  br         IL_001f

    IL_02ba:  ldc.i4.s   2
    IL_02bc:  br         IL_001f

    IL_02c1:  ldc.i4.s   2
    IL_02c3:  br         IL_001f

    IL_02c8:  ldc.i4.s   2
    IL_02ca:  br         IL_001f

    IL_02cf:  ldc.i4.s   2
    IL_02d1:  br         IL_001f

    IL_02d6:  ldc.i4.s   2
    IL_02d8:  br         IL_001f

    IL_02dd:  ldc.i4.s   2
    IL_02df:  br         IL_001f

    IL_02e4:  ldc.i4.s   2
    IL_02e6:  br         IL_001f

    IL_02eb:  ldc.i4.s   2
    IL_02ed:  br         IL_001f

    IL_02f2:  ldc.i4.s   2
    IL_02f4:  br         IL_001f

    IL_02f9:  ldc.i4.s   2
    IL_02fb:  br         IL_001f

    IL_0300:  ldc.i4.s   2
    IL_0302:  br         IL_001f

    IL_0307:  ldc.i4.s   2
    IL_0309:  br         IL_001f

    IL_030e:  ldc.i4.s   2
    IL_0310:  br         IL_001f

    IL_0315:  ldc.i4.s   2
    IL_0317:  br         IL_001f

    IL_031c:  ldc.i4.s   2
    IL_031e:  br         IL_001f

    IL_0323:  ldc.i4.s   2
    IL_0325:  br         IL_001f

    IL_032a:  ldc.i4.s   2
    IL_032c:  br         IL_001f

    IL_0331:  ldc.i4.s   2
    IL_0333:  br         IL_001f

    IL_0338:  ldc.i4.s   1
    IL_033a:  br         IL_001f

    IL_033f:  ldloc.s    V_0
    IL_0341:  stloc.s    V_2
    IL_0343:  ldloc.s    V_1
    IL_0345:  ldloc.s    V_2
    IL_0347:  ldc.i4.s   1
    IL_0349:  sub
    IL_034a:  dup
    IL_034b:  stloc.s    V_2
    IL_034d:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0352:  ldc.i4.s   125
    IL_0354:  beq.s      IL_035d

    IL_0356:  ldc.i4.s   1
    IL_0358:  br         IL_001f

    IL_035d:  ldarg.0
    IL_035e:  ldloc.s    V_2
    IL_0360:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextpos
    IL_0365:  ldc.i4.s   1
    IL_0367:  ret

    IL_0368:  ldarg.0
    IL_0369:  ldarg.0
    IL_036a:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextend
    IL_036f:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextpos
    IL_0374:  ldc.i4.s   0
    IL_0376:  ret
  } // end of method BracketedControllerSeparatorPatternRunner3::FindFirstChar

  .method public virtual instance void  InitTrackCount() cil managed
  {
    // Code size       9 (0x9)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.s   6
    IL_0003:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtrackcount
    IL_0008:  ret
  } // end of method BracketedControllerSeparatorPatternRunner3::InitTrackCount

  .method public specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [System]System.Text.RegularExpressions.RegexRunner::.ctor()
    IL_0006:  ret
  } // end of method BracketedControllerSeparatorPatternRunner3::.ctor

} // end of class Xeora.Web.RegularExpressions.BracketedControllerSeparatorPatternRunner3

.class private auto ansi Xeora.Web.RegularExpressions.BracketedControllerSeparatorPatternFactory3
       extends [System]System.Text.RegularExpressions.RegexRunnerFactory
{
  .method public virtual instance class [System]System.Text.RegularExpressions.RegexRunner 
          CreateInstance() cil managed
  {
    // Code size       6 (0x6)
    .maxstack  1
    IL_0000:  newobj     instance void Xeora.Web.RegularExpressions.BracketedControllerSeparatorPatternRunner3::.ctor()
    IL_0005:  ret
  } // end of method BracketedControllerSeparatorPatternFactory3::CreateInstance

  .method public specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [System]System.Text.RegularExpressions.RegexRunnerFactory::.ctor()
    IL_0006:  ret
  } // end of method BracketedControllerSeparatorPatternFactory3::.ctor

} // end of class Xeora.Web.RegularExpressions.BracketedControllerSeparatorPatternFactory3

.class public auto ansi Xeora.Web.RegularExpressions.BracketedControllerSeparatorPattern
       extends [System]System.Text.RegularExpressions.Regex
{
  .method public specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       166 (0xa6)
    .maxstack  5
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [System]System.Text.RegularExpressions.Regex::.ctor()
    IL_0006:  ldarg.0
    IL_0007:  ldstr      "\\}:(\?<ItemID>[\\.\\w\\-]+)\\:\\{"
    IL_000c:  stfld      string [System]System.Text.RegularExpressions.Regex::pattern
    IL_0011:  ldarg.0
    IL_0012:  ldc.i4.s   2
    IL_0014:  stfld      valuetype [System]System.Text.RegularExpressions.RegexOptions [System]System.Text.RegularExpressions.Regex::roptions
    IL_0019:  ldarg.0
    IL_001a:  ldc.i4     0xffffd8f0
    IL_001f:  conv.i8
    IL_0020:  call       valuetype [mscorlib]System.TimeSpan [mscorlib]System.TimeSpan::FromTicks(int64)
    IL_0025:  stfld      valuetype [mscorlib]System.TimeSpan [System]System.Text.RegularExpressions.Regex::internalMatchTimeout
    IL_002a:  ldarg.0
    IL_002b:  newobj     instance void Xeora.Web.RegularExpressions.BracketedControllerSeparatorPatternFactory3::.ctor()
    IL_0030:  stfld      class [System]System.Text.RegularExpressions.RegexRunnerFactory [System]System.Text.RegularExpressions.Regex::factory
    IL_0035:  ldarg.0
    IL_0036:  newobj     instance void [mscorlib]System.Collections.Hashtable::.ctor()
    IL_003b:  stfld      class [mscorlib]System.Collections.Hashtable [System]System.Text.RegularExpressions.Regex::capnames
    IL_0040:  ldarg.0
    IL_0041:  ldfld      class [mscorlib]System.Collections.Hashtable [System]System.Text.RegularExpressions.Regex::capnames
    IL_0046:  ldstr      "0"
    IL_004b:  ldc.i4.s   0
    IL_004d:  box        [mscorlib]System.Int32
    IL_0052:  callvirt   instance void [mscorlib]System.Collections.Hashtable::Add(object,
                                                                                   object)
    IL_0057:  ldarg.0
    IL_0058:  ldfld      class [mscorlib]System.Collections.Hashtable [System]System.Text.RegularExpressions.Regex::capnames
    IL_005d:  ldstr      "ItemID"
    IL_0062:  ldc.i4.s   1
    IL_0064:  box        [mscorlib]System.Int32
    IL_0069:  callvirt   instance void [mscorlib]System.Collections.Hashtable::Add(object,
                                                                                   object)
    IL_006e:  ldarg.0
    IL_006f:  ldc.i4.s   2
    IL_0071:  newarr     [mscorlib]System.String
    IL_0076:  stfld      string[] [System]System.Text.RegularExpressions.Regex::capslist
    IL_007b:  ldarg.0
    IL_007c:  ldfld      string[] [System]System.Text.RegularExpressions.Regex::capslist
    IL_0081:  ldc.i4.s   0
    IL_0083:  ldstr      "0"
    IL_0088:  stelem.ref
    IL_0089:  ldarg.0
    IL_008a:  ldfld      string[] [System]System.Text.RegularExpressions.Regex::capslist
    IL_008f:  ldc.i4.s   1
    IL_0091:  ldstr      "ItemID"
    IL_0096:  stelem.ref
    IL_0097:  ldarg.0
    IL_0098:  ldc.i4.s   2
    IL_009a:  stfld      int32 [System]System.Text.RegularExpressions.Regex::capsize
    IL_009f:  ldarg.0
    IL_00a0:  call       instance void [System]System.Text.RegularExpressions.Regex::InitializeReferences()
    IL_00a5:  ret
  } // end of method BracketedControllerSeparatorPattern::.ctor

  .method public specialname rtspecialname 
          instance void  .ctor(valuetype [mscorlib]System.TimeSpan A_1) cil managed
  {
    // Code size       20 (0x14)
    .maxstack  4
    IL_0000:  ldarg.0
    IL_0001:  call       instance void Xeora.Web.RegularExpressions.BracketedControllerSeparatorPattern::.ctor()
    IL_0006:  ldarg.1
    IL_0007:  call       void [System]System.Text.RegularExpressions.Regex::ValidateMatchTimeout(valuetype [mscorlib]System.TimeSpan)
    IL_000c:  ldarg.0
    IL_000d:  ldarg.1
    IL_000e:  stfld      valuetype [mscorlib]System.TimeSpan [System]System.Text.RegularExpressions.Regex::internalMatchTimeout
    IL_0013:  ret
  } // end of method BracketedControllerSeparatorPattern::.ctor

} // end of class Xeora.Web.RegularExpressions.BracketedControllerSeparatorPattern

.class private auto ansi Xeora.Web.RegularExpressions.BracketedControllerClosePatternRunner4
       extends [System]System.Text.RegularExpressions.RegexRunner
{
  .method public virtual instance void  Go() cil managed
  {
    // Code size       824 (0x338)
    .maxstack  21
    .locals init (int32 V_0,
             string V_1,
             int32 V_2,
             int32[] V_3,
             int32 V_4,
             int32[] V_5,
             int32 V_6,
             int32 V_7,
             int32 V_8,
             int32 V_9,
             int32 V_10,
             int32 V_11)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string [System]System.Text.RegularExpressions.RegexRunner::runtext
    IL_0006:  stloc.s    V_1
    IL_0008:  ldarg.0
    IL_0009:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextstart
    IL_000e:  stloc.s    V_11
    IL_0010:  ldarg.0
    IL_0011:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextbeg
    IL_0016:  stloc.s    V_9
    IL_0018:  ldarg.0
    IL_0019:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextend
    IL_001e:  stloc.s    V_10
    IL_0020:  ldarg.0
    IL_0021:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextpos
    IL_0026:  stloc.s    V_0
    IL_0028:  ldarg.0
    IL_0029:  ldfld      int32[] [System]System.Text.RegularExpressions.RegexRunner::runtrack
    IL_002e:  stloc.s    V_3
    IL_0030:  ldarg.0
    IL_0031:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtrackpos
    IL_0036:  stloc.s    V_2
    IL_0038:  ldarg.0
    IL_0039:  ldfld      int32[] [System]System.Text.RegularExpressions.RegexRunner::runstack
    IL_003e:  stloc.s    V_5
    IL_0040:  ldarg.0
    IL_0041:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runstackpos
    IL_0046:  stloc.s    V_4
    IL_0048:  ldarg.0
    IL_0049:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_004e:  ldloc.s    V_3
    IL_0050:  ldloc.s    V_2
    IL_0052:  ldc.i4.1
    IL_0053:  sub
    IL_0054:  dup
    IL_0055:  stloc.s    V_2
    IL_0057:  ldloc.s    V_0
    IL_0059:  stelem.i4
    IL_005a:  ldloc.s    V_3
    IL_005c:  ldloc.s    V_2
    IL_005e:  ldc.i4.1
    IL_005f:  sub
    IL_0060:  dup
    IL_0061:  stloc.s    V_2
    IL_0063:  ldc.i4.s   0
    IL_0065:  stelem.i4
    IL_0066:  ldarg.0
    IL_0067:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_006c:  ldloc.s    V_5
    IL_006e:  ldloc.s    V_4
    IL_0070:  ldc.i4.1
    IL_0071:  sub
    IL_0072:  dup
    IL_0073:  stloc.s    V_4
    IL_0075:  ldloc.s    V_0
    IL_0077:  stelem.i4
    IL_0078:  ldloc.s    V_3
    IL_007a:  ldloc.s    V_2
    IL_007c:  ldc.i4.1
    IL_007d:  sub
    IL_007e:  dup
    IL_007f:  stloc.s    V_2
    IL_0081:  ldc.i4.s   1
    IL_0083:  stelem.i4
    IL_0084:  ldarg.0
    IL_0085:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_008a:  ldc.i4.s   2
    IL_008c:  ldloc.s    V_10
    IL_008e:  ldloc.s    V_0
    IL_0090:  sub
    IL_0091:  bgt        IL_023f

    IL_0096:  ldloc.s    V_1
    IL_0098:  ldloc.s    V_0
    IL_009a:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_009f:  ldc.i4.s   125
    IL_00a1:  bne.un     IL_023f

    IL_00a6:  ldloc.s    V_1
    IL_00a8:  ldloc.s    V_0
    IL_00aa:  ldc.i4.s   1
    IL_00ac:  add
    IL_00ad:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_00b2:  ldc.i4.s   58
    IL_00b4:  bne.un     IL_023f

    IL_00b9:  ldloc.s    V_0
    IL_00bb:  ldc.i4.s   2
    IL_00bd:  add
    IL_00be:  stloc.s    V_0
    IL_00c0:  ldarg.0
    IL_00c1:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_00c6:  ldloc.s    V_5
    IL_00c8:  ldloc.s    V_4
    IL_00ca:  ldc.i4.1
    IL_00cb:  sub
    IL_00cc:  dup
    IL_00cd:  stloc.s    V_4
    IL_00cf:  ldloc.s    V_0
    IL_00d1:  stelem.i4
    IL_00d2:  ldloc.s    V_3
    IL_00d4:  ldloc.s    V_2
    IL_00d6:  ldc.i4.1
    IL_00d7:  sub
    IL_00d8:  dup
    IL_00d9:  stloc.s    V_2
    IL_00db:  ldc.i4.s   1
    IL_00dd:  stelem.i4
    IL_00de:  ldarg.0
    IL_00df:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_00e4:  ldc.i4.s   1
    IL_00e6:  ldloc.s    V_10
    IL_00e8:  ldloc.s    V_0
    IL_00ea:  sub
    IL_00eb:  bgt        IL_023f

    IL_00f0:  ldloc.s    V_0
    IL_00f2:  ldc.i4.s   1
    IL_00f4:  add
    IL_00f5:  stloc.s    V_0
    IL_00f7:  ldc.i4.s   1
    IL_00f9:  stloc.s    V_6
    IL_00fb:  ldloc.s    V_1
    IL_00fd:  ldloc.s    V_0
    IL_00ff:  ldloc.s    V_6
    IL_0101:  dup
    IL_0102:  ldc.i4.s   1
    IL_0104:  sub
    IL_0105:  stloc.s    V_6
    IL_0107:  sub
    IL_0108:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_010d:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_0112:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0117:  brfalse    IL_023f

    IL_011c:  ldloc.s    V_6
    IL_011e:  ldc.i4.s   0
    IL_0120:  bgt        IL_00fb

    IL_0125:  ldarg.0
    IL_0126:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_012b:  ldloc.s    V_10
    IL_012d:  ldloc.s    V_0
    IL_012f:  sub
    IL_0130:  dup
    IL_0131:  stloc.s    V_7
    IL_0133:  ldc.i4.s   1
    IL_0135:  add
    IL_0136:  stloc.s    V_6
    IL_0138:  ldloc.s    V_6
    IL_013a:  ldc.i4.s   1
    IL_013c:  sub
    IL_013d:  dup
    IL_013e:  stloc.s    V_6
    IL_0140:  ldc.i4.s   0
    IL_0142:  ble        IL_016c

    IL_0147:  ldloc.s    V_1
    IL_0149:  ldloc.s    V_0
    IL_014b:  dup
    IL_014c:  ldc.i4.s   1
    IL_014e:  add
    IL_014f:  stloc.s    V_0
    IL_0151:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0156:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_015b:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0160:  brtrue     IL_0138

    IL_0165:  ldloc.s    V_0
    IL_0167:  ldc.i4.s   1
    IL_0169:  sub
    IL_016a:  stloc.s    V_0
    IL_016c:  ldloc.s    V_7
    IL_016e:  ldloc.s    V_6
    IL_0170:  ble.s      IL_019f

    IL_0172:  ldloc.s    V_3
    IL_0174:  ldloc.s    V_2
    IL_0176:  ldc.i4.1
    IL_0177:  sub
    IL_0178:  dup
    IL_0179:  stloc.s    V_2
    IL_017b:  ldloc.s    V_7
    IL_017d:  ldloc.s    V_6
    IL_017f:  sub
    IL_0180:  ldc.i4.s   1
    IL_0182:  sub
    IL_0183:  stelem.i4
    IL_0184:  ldloc.s    V_3
    IL_0186:  ldloc.s    V_2
    IL_0188:  ldc.i4.1
    IL_0189:  sub
    IL_018a:  dup
    IL_018b:  stloc.s    V_2
    IL_018d:  ldloc.s    V_0
    IL_018f:  ldc.i4.s   1
    IL_0191:  sub
    IL_0192:  stelem.i4
    IL_0193:  ldloc.s    V_3
    IL_0195:  ldloc.s    V_2
    IL_0197:  ldc.i4.1
    IL_0198:  sub
    IL_0199:  dup
    IL_019a:  stloc.s    V_2
    IL_019c:  ldc.i4.s   2
    IL_019e:  stelem.i4
    IL_019f:  ldarg.0
    IL_01a0:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_01a5:  ldloc.s    V_5
    IL_01a7:  ldloc.s    V_4
    IL_01a9:  dup
    IL_01aa:  ldc.i4.1
    IL_01ab:  add
    IL_01ac:  stloc.s    V_4
    IL_01ae:  ldelem.i4
    IL_01af:  stloc.s    V_6
    IL_01b1:  ldarg.0
    IL_01b2:  ldc.i4.s   1
    IL_01b4:  ldloc.s    V_6
    IL_01b6:  ldloc.s    V_0
    IL_01b8:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_01bd:  ldloc.s    V_3
    IL_01bf:  ldloc.s    V_2
    IL_01c1:  ldc.i4.1
    IL_01c2:  sub
    IL_01c3:  dup
    IL_01c4:  stloc.s    V_2
    IL_01c6:  ldloc.s    V_6
    IL_01c8:  stelem.i4
    IL_01c9:  ldloc.s    V_3
    IL_01cb:  ldloc.s    V_2
    IL_01cd:  ldc.i4.1
    IL_01ce:  sub
    IL_01cf:  dup
    IL_01d0:  stloc.s    V_2
    IL_01d2:  ldc.i4.s   3
    IL_01d4:  stelem.i4
    IL_01d5:  ldarg.0
    IL_01d6:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_01db:  ldloc.s    V_0
    IL_01dd:  ldloc.s    V_10
    IL_01df:  bge        IL_023f

    IL_01e4:  ldloc.s    V_1
    IL_01e6:  ldloc.s    V_0
    IL_01e8:  dup
    IL_01e9:  ldc.i4.s   1
    IL_01eb:  add
    IL_01ec:  stloc.s    V_0
    IL_01ee:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_01f3:  ldc.i4.s   36
    IL_01f5:  bne.un     IL_023f

    IL_01fa:  ldarg.0
    IL_01fb:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0200:  ldloc.s    V_5
    IL_0202:  ldloc.s    V_4
    IL_0204:  dup
    IL_0205:  ldc.i4.1
    IL_0206:  add
    IL_0207:  stloc.s    V_4
    IL_0209:  ldelem.i4
    IL_020a:  stloc.s    V_6
    IL_020c:  ldarg.0
    IL_020d:  ldc.i4.s   0
    IL_020f:  ldloc.s    V_6
    IL_0211:  ldloc.s    V_0
    IL_0213:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_0218:  ldloc.s    V_3
    IL_021a:  ldloc.s    V_2
    IL_021c:  ldc.i4.1
    IL_021d:  sub
    IL_021e:  dup
    IL_021f:  stloc.s    V_2
    IL_0221:  ldloc.s    V_6
    IL_0223:  stelem.i4
    IL_0224:  ldloc.s    V_3
    IL_0226:  ldloc.s    V_2
    IL_0228:  ldc.i4.1
    IL_0229:  sub
    IL_022a:  dup
    IL_022b:  stloc.s    V_2
    IL_022d:  ldc.i4.s   3
    IL_022f:  stelem.i4
    IL_0230:  ldarg.0
    IL_0231:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0236:  ldarg.0
    IL_0237:  ldloc.s    V_0
    IL_0239:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextpos
    IL_023e:  ret

    IL_023f:  ldarg.0
    IL_0240:  ldloc.s    V_2
    IL_0242:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtrackpos
    IL_0247:  ldarg.0
    IL_0248:  ldloc.s    V_4
    IL_024a:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runstackpos
    IL_024f:  ldarg.0
    IL_0250:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::EnsureStorage()
    IL_0255:  ldarg.0
    IL_0256:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtrackpos
    IL_025b:  stloc.s    V_2
    IL_025d:  ldarg.0
    IL_025e:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runstackpos
    IL_0263:  stloc.s    V_4
    IL_0265:  ldarg.0
    IL_0266:  ldfld      int32[] [System]System.Text.RegularExpressions.RegexRunner::runtrack
    IL_026b:  stloc.s    V_3
    IL_026d:  ldarg.0
    IL_026e:  ldfld      int32[] [System]System.Text.RegularExpressions.RegexRunner::runstack
    IL_0273:  stloc.s    V_5
    IL_0275:  ldloc.s    V_3
    IL_0277:  ldloc.s    V_2
    IL_0279:  dup
    IL_027a:  ldc.i4.1
    IL_027b:  add
    IL_027c:  stloc.s    V_2
    IL_027e:  ldelem.i4
    IL_027f:  switch     ( 
                          IL_0294,
                          IL_02ab,
                          IL_02bd,
                          IL_0313)
    IL_0294:  ldarg.0
    IL_0295:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_029a:  ldloc.s    V_3
    IL_029c:  ldloc.s    V_2
    IL_029e:  dup
    IL_029f:  ldc.i4.1
    IL_02a0:  add
    IL_02a1:  stloc.s    V_2
    IL_02a3:  ldelem.i4
    IL_02a4:  stloc.s    V_0
    IL_02a6:  br         IL_0230

    IL_02ab:  ldarg.0
    IL_02ac:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_02b1:  ldloc.s    V_4
    IL_02b3:  ldc.i4.s   1
    IL_02b5:  add
    IL_02b6:  stloc.s    V_4
    IL_02b8:  br         IL_023f

    IL_02bd:  ldarg.0
    IL_02be:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_02c3:  ldloc.s    V_3
    IL_02c5:  ldloc.s    V_2
    IL_02c7:  dup
    IL_02c8:  ldc.i4.1
    IL_02c9:  add
    IL_02ca:  stloc.s    V_2
    IL_02cc:  ldelem.i4
    IL_02cd:  stloc.s    V_0
    IL_02cf:  ldloc.s    V_3
    IL_02d1:  ldloc.s    V_2
    IL_02d3:  dup
    IL_02d4:  ldc.i4.1
    IL_02d5:  add
    IL_02d6:  stloc.s    V_2
    IL_02d8:  ldelem.i4
    IL_02d9:  stloc.s    V_6
    IL_02db:  ldloc.s    V_6
    IL_02dd:  ldc.i4.s   0
    IL_02df:  ble        IL_019f

    IL_02e4:  ldloc.s    V_3
    IL_02e6:  ldloc.s    V_2
    IL_02e8:  ldc.i4.1
    IL_02e9:  sub
    IL_02ea:  dup
    IL_02eb:  stloc.s    V_2
    IL_02ed:  ldloc.s    V_6
    IL_02ef:  ldc.i4.s   1
    IL_02f1:  sub
    IL_02f2:  stelem.i4
    IL_02f3:  ldloc.s    V_3
    IL_02f5:  ldloc.s    V_2
    IL_02f7:  ldc.i4.1
    IL_02f8:  sub
    IL_02f9:  dup
    IL_02fa:  stloc.s    V_2
    IL_02fc:  ldloc.s    V_0
    IL_02fe:  ldc.i4.s   1
    IL_0300:  sub
    IL_0301:  stelem.i4
    IL_0302:  ldloc.s    V_3
    IL_0304:  ldloc.s    V_2
    IL_0306:  ldc.i4.1
    IL_0307:  sub
    IL_0308:  dup
    IL_0309:  stloc.s    V_2
    IL_030b:  ldc.i4.s   2
    IL_030d:  stelem.i4
    IL_030e:  br         IL_019f

    IL_0313:  ldarg.0
    IL_0314:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0319:  ldloc.s    V_5
    IL_031b:  ldloc.s    V_4
    IL_031d:  ldc.i4.1
    IL_031e:  sub
    IL_031f:  dup
    IL_0320:  stloc.s    V_4
    IL_0322:  ldloc.s    V_3
    IL_0324:  ldloc.s    V_2
    IL_0326:  dup
    IL_0327:  ldc.i4.1
    IL_0328:  add
    IL_0329:  stloc.s    V_2
    IL_032b:  ldelem.i4
    IL_032c:  stelem.i4
    IL_032d:  ldarg.0
    IL_032e:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Uncapture()
    IL_0333:  br         IL_023f
  } // end of method BracketedControllerClosePatternRunner4::Go

  .method public virtual instance bool  FindFirstChar() cil managed
  {
    // Code size       887 (0x377)
    .maxstack  78
    .locals init (int32 V_0,
             string V_1,
             int32 V_2,
             int32 V_3)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      string [System]System.Text.RegularExpressions.RegexRunner::runtext
    IL_0006:  stloc.s    V_1
    IL_0008:  ldarg.0
    IL_0009:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextend
    IL_000e:  stloc.s    V_3
    IL_0010:  ldarg.0
    IL_0011:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextpos
    IL_0016:  ldc.i4.s   1
    IL_0018:  add
    IL_0019:  stloc.s    V_0
    IL_001b:  br.s       IL_0024

    IL_001d:  ldc.i4.s   2
    IL_001f:  ldloc.s    V_0
    IL_0021:  add
    IL_0022:  stloc.s    V_0
    IL_0024:  ldloc.s    V_0
    IL_0026:  ldloc.s    V_3
    IL_0028:  bge        IL_0368

    IL_002d:  ldloc.s    V_1
    IL_002f:  ldloc.s    V_0
    IL_0031:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0036:  dup
    IL_0037:  stloc.s    V_2
    IL_0039:  ldc.i4.s   58
    IL_003b:  beq        IL_033f

    IL_0040:  ldloc.s    V_2
    IL_0042:  ldc.i4.s   58
    IL_0044:  sub
    IL_0045:  dup
    IL_0046:  stloc.s    V_2
    IL_0048:  ldc.i4.s   67
    IL_004a:  bgt.un.s   IL_001d

    IL_004c:  ldloc.s    V_2
    IL_004e:  switch     ( 
                          IL_0163,
                          IL_016a,
                          IL_0171,
                          IL_0178,
                          IL_017f,
                          IL_0186,
                          IL_018d,
                          IL_0194,
                          IL_019b,
                          IL_01a2,
                          IL_01a9,
                          IL_01b0,
                          IL_01b7,
                          IL_01be,
                          IL_01c5,
                          IL_01cc,
                          IL_01d3,
                          IL_01da,
                          IL_01e1,
                          IL_01e8,
                          IL_01ef,
                          IL_01f6,
                          IL_01fd,
                          IL_0204,
                          IL_020b,
                          IL_0212,
                          IL_0219,
                          IL_0220,
                          IL_0227,
                          IL_022e,
                          IL_0235,
                          IL_023c,
                          IL_0243,
                          IL_024a,
                          IL_0251,
                          IL_0258,
                          IL_025f,
                          IL_0266,
                          IL_026d,
                          IL_0274,
                          IL_027b,
                          IL_0282,
                          IL_0289,
                          IL_0290,
                          IL_0297,
                          IL_029e,
                          IL_02a5,
                          IL_02ac,
                          IL_02b3,
                          IL_02ba,
                          IL_02c1,
                          IL_02c8,
                          IL_02cf,
                          IL_02d6,
                          IL_02dd,
                          IL_02e4,
                          IL_02eb,
                          IL_02f2,
                          IL_02f9,
                          IL_0300,
                          IL_0307,
                          IL_030e,
                          IL_0315,
                          IL_031c,
                          IL_0323,
                          IL_032a,
                          IL_0331,
                          IL_0338)
    IL_0163:  ldc.i4.s   0
    IL_0165:  br         IL_001f

    IL_016a:  ldc.i4.s   2
    IL_016c:  br         IL_001f

    IL_0171:  ldc.i4.s   2
    IL_0173:  br         IL_001f

    IL_0178:  ldc.i4.s   2
    IL_017a:  br         IL_001f

    IL_017f:  ldc.i4.s   2
    IL_0181:  br         IL_001f

    IL_0186:  ldc.i4.s   2
    IL_0188:  br         IL_001f

    IL_018d:  ldc.i4.s   2
    IL_018f:  br         IL_001f

    IL_0194:  ldc.i4.s   2
    IL_0196:  br         IL_001f

    IL_019b:  ldc.i4.s   2
    IL_019d:  br         IL_001f

    IL_01a2:  ldc.i4.s   2
    IL_01a4:  br         IL_001f

    IL_01a9:  ldc.i4.s   2
    IL_01ab:  br         IL_001f

    IL_01b0:  ldc.i4.s   2
    IL_01b2:  br         IL_001f

    IL_01b7:  ldc.i4.s   2
    IL_01b9:  br         IL_001f

    IL_01be:  ldc.i4.s   2
    IL_01c0:  br         IL_001f

    IL_01c5:  ldc.i4.s   2
    IL_01c7:  br         IL_001f

    IL_01cc:  ldc.i4.s   2
    IL_01ce:  br         IL_001f

    IL_01d3:  ldc.i4.s   2
    IL_01d5:  br         IL_001f

    IL_01da:  ldc.i4.s   2
    IL_01dc:  br         IL_001f

    IL_01e1:  ldc.i4.s   2
    IL_01e3:  br         IL_001f

    IL_01e8:  ldc.i4.s   2
    IL_01ea:  br         IL_001f

    IL_01ef:  ldc.i4.s   2
    IL_01f1:  br         IL_001f

    IL_01f6:  ldc.i4.s   2
    IL_01f8:  br         IL_001f

    IL_01fd:  ldc.i4.s   2
    IL_01ff:  br         IL_001f

    IL_0204:  ldc.i4.s   2
    IL_0206:  br         IL_001f

    IL_020b:  ldc.i4.s   2
    IL_020d:  br         IL_001f

    IL_0212:  ldc.i4.s   2
    IL_0214:  br         IL_001f

    IL_0219:  ldc.i4.s   2
    IL_021b:  br         IL_001f

    IL_0220:  ldc.i4.s   2
    IL_0222:  br         IL_001f

    IL_0227:  ldc.i4.s   2
    IL_0229:  br         IL_001f

    IL_022e:  ldc.i4.s   2
    IL_0230:  br         IL_001f

    IL_0235:  ldc.i4.s   2
    IL_0237:  br         IL_001f

    IL_023c:  ldc.i4.s   2
    IL_023e:  br         IL_001f

    IL_0243:  ldc.i4.s   2
    IL_0245:  br         IL_001f

    IL_024a:  ldc.i4.s   2
    IL_024c:  br         IL_001f

    IL_0251:  ldc.i4.s   2
    IL_0253:  br         IL_001f

    IL_0258:  ldc.i4.s   2
    IL_025a:  br         IL_001f

    IL_025f:  ldc.i4.s   2
    IL_0261:  br         IL_001f

    IL_0266:  ldc.i4.s   2
    IL_0268:  br         IL_001f

    IL_026d:  ldc.i4.s   2
    IL_026f:  br         IL_001f

    IL_0274:  ldc.i4.s   2
    IL_0276:  br         IL_001f

    IL_027b:  ldc.i4.s   2
    IL_027d:  br         IL_001f

    IL_0282:  ldc.i4.s   2
    IL_0284:  br         IL_001f

    IL_0289:  ldc.i4.s   2
    IL_028b:  br         IL_001f

    IL_0290:  ldc.i4.s   2
    IL_0292:  br         IL_001f

    IL_0297:  ldc.i4.s   2
    IL_0299:  br         IL_001f

    IL_029e:  ldc.i4.s   2
    IL_02a0:  br         IL_001f

    IL_02a5:  ldc.i4.s   2
    IL_02a7:  br         IL_001f

    IL_02ac:  ldc.i4.s   2
    IL_02ae:  br         IL_001f

    IL_02b3:  ldc.i4.s   2
    IL_02b5:  br         IL_001f

    IL_02ba:  ldc.i4.s   2
    IL_02bc:  br         IL_001f

    IL_02c1:  ldc.i4.s   2
    IL_02c3:  br         IL_001f

    IL_02c8:  ldc.i4.s   2
    IL_02ca:  br         IL_001f

    IL_02cf:  ldc.i4.s   2
    IL_02d1:  br         IL_001f

    IL_02d6:  ldc.i4.s   2
    IL_02d8:  br         IL_001f

    IL_02dd:  ldc.i4.s   2
    IL_02df:  br         IL_001f

    IL_02e4:  ldc.i4.s   2
    IL_02e6:  br         IL_001f

    IL_02eb:  ldc.i4.s   2
    IL_02ed:  br         IL_001f

    IL_02f2:  ldc.i4.s   2
    IL_02f4:  br         IL_001f

    IL_02f9:  ldc.i4.s   2
    IL_02fb:  br         IL_001f

    IL_0300:  ldc.i4.s   2
    IL_0302:  br         IL_001f

    IL_0307:  ldc.i4.s   2
    IL_0309:  br         IL_001f

    IL_030e:  ldc.i4.s   2
    IL_0310:  br         IL_001f

    IL_0315:  ldc.i4.s   2
    IL_0317:  br         IL_001f

    IL_031c:  ldc.i4.s   2
    IL_031e:  br         IL_001f

    IL_0323:  ldc.i4.s   2
    IL_0325:  br         IL_001f

    IL_032a:  ldc.i4.s   2
    IL_032c:  br         IL_001f

    IL_0331:  ldc.i4.s   2
    IL_0333:  br         IL_001f

    IL_0338:  ldc.i4.s   1
    IL_033a:  br         IL_001f

    IL_033f:  ldloc.s    V_0
    IL_0341:  stloc.s    V_2
    IL_0343:  ldloc.s    V_1
    IL_0345:  ldloc.s    V_2
    IL_0347:  ldc.i4.s   1
    IL_0349:  sub
    IL_034a:  dup
    IL_034b:  stloc.s    V_2
    IL_034d:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0352:  ldc.i4.s   125
    IL_0354:  beq.s      IL_035d

    IL_0356:  ldc.i4.s   1
    IL_0358:  br         IL_001f

    IL_035d:  ldarg.0
    IL_035e:  ldloc.s    V_2
    IL_0360:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextpos
    IL_0365:  ldc.i4.s   1
    IL_0367:  ret

    IL_0368:  ldarg.0
    IL_0369:  ldarg.0
    IL_036a:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextend
    IL_036f:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextpos
    IL_0374:  ldc.i4.s   0
    IL_0376:  ret
  } // end of method BracketedControllerClosePatternRunner4::FindFirstChar

  .method public virtual instance void  InitTrackCount() cil managed
  {
    // Code size       9 (0x9)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.s   6
    IL_0003:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtrackcount
    IL_0008:  ret
  } // end of method BracketedControllerClosePatternRunner4::InitTrackCount

  .method public specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [System]System.Text.RegularExpressions.RegexRunner::.ctor()
    IL_0006:  ret
  } // end of method BracketedControllerClosePatternRunner4::.ctor

} // end of class Xeora.Web.RegularExpressions.BracketedControllerClosePatternRunner4

.class private auto ansi Xeora.Web.RegularExpressions.BracketedControllerClosePatternFactory4
       extends [System]System.Text.RegularExpressions.RegexRunnerFactory
{
  .method public virtual instance class [System]System.Text.RegularExpressions.RegexRunner 
          CreateInstance() cil managed
  {
    // Code size       6 (0x6)
    .maxstack  1
    IL_0000:  newobj     instance void Xeora.Web.RegularExpressions.BracketedControllerClosePatternRunner4::.ctor()
    IL_0005:  ret
  } // end of method BracketedControllerClosePatternFactory4::CreateInstance

  .method public specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [System]System.Text.RegularExpressions.RegexRunnerFactory::.ctor()
    IL_0006:  ret
  } // end of method BracketedControllerClosePatternFactory4::.ctor

} // end of class Xeora.Web.RegularExpressions.BracketedControllerClosePatternFactory4

.class public auto ansi Xeora.Web.RegularExpressions.BracketedControllerClosePattern
       extends [System]System.Text.RegularExpressions.Regex
{
  .method public specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       166 (0xa6)
    .maxstack  5
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [System]System.Text.RegularExpressions.Regex::.ctor()
    IL_0006:  ldarg.0
    IL_0007:  ldstr      "\\}:(\?<ItemID>[\\.\\w\\-]+)\\$"
    IL_000c:  stfld      string [System]System.Text.RegularExpressions.Regex::pattern
    IL_0011:  ldarg.0
    IL_0012:  ldc.i4.s   2
    IL_0014:  stfld      valuetype [System]System.Text.RegularExpressions.RegexOptions [System]System.Text.RegularExpressions.Regex::roptions
    IL_0019:  ldarg.0
    IL_001a:  ldc.i4     0xffffd8f0
    IL_001f:  conv.i8
    IL_0020:  call       valuetype [mscorlib]System.TimeSpan [mscorlib]System.TimeSpan::FromTicks(int64)
    IL_0025:  stfld      valuetype [mscorlib]System.TimeSpan [System]System.Text.RegularExpressions.Regex::internalMatchTimeout
    IL_002a:  ldarg.0
    IL_002b:  newobj     instance void Xeora.Web.RegularExpressions.BracketedControllerClosePatternFactory4::.ctor()
    IL_0030:  stfld      class [System]System.Text.RegularExpressions.RegexRunnerFactory [System]System.Text.RegularExpressions.Regex::factory
    IL_0035:  ldarg.0
    IL_0036:  newobj     instance void [mscorlib]System.Collections.Hashtable::.ctor()
    IL_003b:  stfld      class [mscorlib]System.Collections.Hashtable [System]System.Text.RegularExpressions.Regex::capnames
    IL_0040:  ldarg.0
    IL_0041:  ldfld      class [mscorlib]System.Collections.Hashtable [System]System.Text.RegularExpressions.Regex::capnames
    IL_0046:  ldstr      "0"
    IL_004b:  ldc.i4.s   0
    IL_004d:  box        [mscorlib]System.Int32
    IL_0052:  callvirt   instance void [mscorlib]System.Collections.Hashtable::Add(object,
                                                                                   object)
    IL_0057:  ldarg.0
    IL_0058:  ldfld      class [mscorlib]System.Collections.Hashtable [System]System.Text.RegularExpressions.Regex::capnames
    IL_005d:  ldstr      "ItemID"
    IL_0062:  ldc.i4.s   1
    IL_0064:  box        [mscorlib]System.Int32
    IL_0069:  callvirt   instance void [mscorlib]System.Collections.Hashtable::Add(object,
                                                                                   object)
    IL_006e:  ldarg.0
    IL_006f:  ldc.i4.s   2
    IL_0071:  newarr     [mscorlib]System.String
    IL_0076:  stfld      string[] [System]System.Text.RegularExpressions.Regex::capslist
    IL_007b:  ldarg.0
    IL_007c:  ldfld      string[] [System]System.Text.RegularExpressions.Regex::capslist
    IL_0081:  ldc.i4.s   0
    IL_0083:  ldstr      "0"
    IL_0088:  stelem.ref
    IL_0089:  ldarg.0
    IL_008a:  ldfld      string[] [System]System.Text.RegularExpressions.Regex::capslist
    IL_008f:  ldc.i4.s   1
    IL_0091:  ldstr      "ItemID"
    IL_0096:  stelem.ref
    IL_0097:  ldarg.0
    IL_0098:  ldc.i4.s   2
    IL_009a:  stfld      int32 [System]System.Text.RegularExpressions.Regex::capsize
    IL_009f:  ldarg.0
    IL_00a0:  call       instance void [System]System.Text.RegularExpressions.Regex::InitializeReferences()
    IL_00a5:  ret
  } // end of method BracketedControllerClosePattern::.ctor

  .method public specialname rtspecialname 
          instance void  .ctor(valuetype [mscorlib]System.TimeSpan A_1) cil managed
  {
    // Code size       20 (0x14)
    .maxstack  4
    IL_0000:  ldarg.0
    IL_0001:  call       instance void Xeora.Web.RegularExpressions.BracketedControllerClosePattern::.ctor()
    IL_0006:  ldarg.1
    IL_0007:  call       void [System]System.Text.RegularExpressions.Regex::ValidateMatchTimeout(valuetype [mscorlib]System.TimeSpan)
    IL_000c:  ldarg.0
    IL_000d:  ldarg.1
    IL_000e:  stfld      valuetype [mscorlib]System.TimeSpan [System]System.Text.RegularExpressions.Regex::internalMatchTimeout
    IL_0013:  ret
  } // end of method BracketedControllerClosePattern::.ctor

} // end of class Xeora.Web.RegularExpressions.BracketedControllerClosePattern


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
