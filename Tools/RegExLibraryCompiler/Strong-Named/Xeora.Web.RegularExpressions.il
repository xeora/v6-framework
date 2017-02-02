
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
// MVID: {6910F0F6-73F8-4CB3-AA32-1F785C3E699E}
.imagebase 0x00400000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000001    //  ILONLY
// Image base: 0x01070000


// =============== CLASS MEMBERS DECLARATION ===================

.class private auto ansi Xeora.Web.RegularExpressions.MainCapturePatternRunner1
       extends [System]System.Text.RegularExpressions.RegexRunner
{
  .method public virtual instance void  Go() cil managed
  {
    // Code size       12352 (0x3040)
    .maxstack  369
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
    IL_00ac:  bge        IL_22d4

    IL_00b1:  ldloc.s    V_1
    IL_00b3:  ldloc.s    V_0
    IL_00b5:  dup
    IL_00b6:  ldc.i4.s   1
    IL_00b8:  add
    IL_00b9:  stloc.s    V_0
    IL_00bb:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_00c0:  ldc.i4.s   36
    IL_00c2:  bne.un     IL_22d4

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
    IL_01bd:  bgt        IL_22d4

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
    IL_01e1:  bne.un     IL_22d4

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
    IL_026d:  bge        IL_22d4

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
    IL_028b:  brfalse    IL_22d4

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
    IL_032a:  ldc.i4     0x1a0
    IL_032f:  ble.s      IL_033d

    IL_0331:  ldloc.s    V_4
    IL_0333:  ldc.i4     0x138
    IL_0338:  bgt        IL_0174

    IL_033d:  ldloc.s    V_3
    IL_033f:  ldloc.s    V_2
    IL_0341:  ldc.i4.1
    IL_0342:  sub
    IL_0343:  dup
    IL_0344:  stloc.s    V_2
    IL_0346:  ldc.i4.s   10
    IL_0348:  stelem.i4
    IL_0349:  br         IL_22d4

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
    IL_0391:  bgt        IL_22d4

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
    IL_03bd:  brfalse    IL_22d4

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
    IL_0481:  br         IL_0883

    IL_0486:  ldarg.0
    IL_0487:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_048c:  ldloc.s    V_5
    IL_048e:  ldloc.s    V_4
    IL_0490:  ldc.i4.1
    IL_0491:  sub
    IL_0492:  dup
    IL_0493:  stloc.s    V_4
    IL_0495:  ldc.i4.s   -1
    IL_0497:  stelem.i4
    IL_0498:  ldloc.s    V_3
    IL_049a:  ldloc.s    V_2
    IL_049c:  ldc.i4.1
    IL_049d:  sub
    IL_049e:  dup
    IL_049f:  stloc.s    V_2
    IL_04a1:  ldc.i4.s   1
    IL_04a3:  stelem.i4
    IL_04a4:  ldarg.0
    IL_04a5:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_04aa:  br         IL_05e9

    IL_04af:  ldarg.0
    IL_04b0:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_04b5:  ldloc.s    V_5
    IL_04b7:  ldloc.s    V_4
    IL_04b9:  ldc.i4.1
    IL_04ba:  sub
    IL_04bb:  dup
    IL_04bc:  stloc.s    V_4
    IL_04be:  ldloc.s    V_0
    IL_04c0:  stelem.i4
    IL_04c1:  ldloc.s    V_3
    IL_04c3:  ldloc.s    V_2
    IL_04c5:  ldc.i4.1
    IL_04c6:  sub
    IL_04c7:  dup
    IL_04c8:  stloc.s    V_2
    IL_04ca:  ldc.i4.s   1
    IL_04cc:  stelem.i4
    IL_04cd:  ldarg.0
    IL_04ce:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_04d3:  ldc.i4.s   1
    IL_04d5:  ldloc.s    V_10
    IL_04d7:  ldloc.s    V_0
    IL_04d9:  sub
    IL_04da:  bgt        IL_22d4

    IL_04df:  ldloc.s    V_0
    IL_04e1:  ldc.i4.s   1
    IL_04e3:  add
    IL_04e4:  stloc.s    V_0
    IL_04e6:  ldc.i4.s   1
    IL_04e8:  stloc.s    V_6
    IL_04ea:  ldloc.s    V_1
    IL_04ec:  ldloc.s    V_0
    IL_04ee:  ldloc.s    V_6
    IL_04f0:  dup
    IL_04f1:  ldc.i4.s   1
    IL_04f3:  sub
    IL_04f4:  stloc.s    V_6
    IL_04f6:  sub
    IL_04f7:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_04fc:  ldstr      bytearray (00 00 00 00 0A 00 00 00 02 00 04 00 05 00 03 00 
                                    01 00 06 00 09 00 13 00 00 00 ) 
    IL_0501:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0506:  brfalse    IL_22d4

    IL_050b:  ldloc.s    V_6
    IL_050d:  ldc.i4.s   0
    IL_050f:  bgt        IL_04ea

    IL_0514:  ldarg.0
    IL_0515:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_051a:  ldloc.s    V_10
    IL_051c:  ldloc.s    V_0
    IL_051e:  sub
    IL_051f:  dup
    IL_0520:  stloc.s    V_7
    IL_0522:  ldc.i4.s   1
    IL_0524:  add
    IL_0525:  stloc.s    V_6
    IL_0527:  ldloc.s    V_6
    IL_0529:  ldc.i4.s   1
    IL_052b:  sub
    IL_052c:  dup
    IL_052d:  stloc.s    V_6
    IL_052f:  ldc.i4.s   0
    IL_0531:  ble        IL_055b

    IL_0536:  ldloc.s    V_1
    IL_0538:  ldloc.s    V_0
    IL_053a:  dup
    IL_053b:  ldc.i4.s   1
    IL_053d:  add
    IL_053e:  stloc.s    V_0
    IL_0540:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0545:  ldstr      bytearray (00 00 00 00 0A 00 00 00 02 00 04 00 05 00 03 00 
                                    01 00 06 00 09 00 13 00 00 00 ) 
    IL_054a:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_054f:  brtrue     IL_0527

    IL_0554:  ldloc.s    V_0
    IL_0556:  ldc.i4.s   1
    IL_0558:  sub
    IL_0559:  stloc.s    V_0
    IL_055b:  ldloc.s    V_7
    IL_055d:  ldloc.s    V_6
    IL_055f:  ble.s      IL_058e

    IL_0561:  ldloc.s    V_3
    IL_0563:  ldloc.s    V_2
    IL_0565:  ldc.i4.1
    IL_0566:  sub
    IL_0567:  dup
    IL_0568:  stloc.s    V_2
    IL_056a:  ldloc.s    V_7
    IL_056c:  ldloc.s    V_6
    IL_056e:  sub
    IL_056f:  ldc.i4.s   1
    IL_0571:  sub
    IL_0572:  stelem.i4
    IL_0573:  ldloc.s    V_3
    IL_0575:  ldloc.s    V_2
    IL_0577:  ldc.i4.1
    IL_0578:  sub
    IL_0579:  dup
    IL_057a:  stloc.s    V_2
    IL_057c:  ldloc.s    V_0
    IL_057e:  ldc.i4.s   1
    IL_0580:  sub
    IL_0581:  stelem.i4
    IL_0582:  ldloc.s    V_3
    IL_0584:  ldloc.s    V_2
    IL_0586:  ldc.i4.1
    IL_0587:  sub
    IL_0588:  dup
    IL_0589:  stloc.s    V_2
    IL_058b:  ldc.i4.s   13
    IL_058d:  stelem.i4
    IL_058e:  ldarg.0
    IL_058f:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0594:  ldloc.s    V_0
    IL_0596:  ldloc.s    V_10
    IL_0598:  bge        IL_22d4

    IL_059d:  ldloc.s    V_1
    IL_059f:  ldloc.s    V_0
    IL_05a1:  dup
    IL_05a2:  ldc.i4.s   1
    IL_05a4:  add
    IL_05a5:  stloc.s    V_0
    IL_05a7:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_05ac:  ldc.i4.s   46
    IL_05ae:  bne.un     IL_22d4

    IL_05b3:  ldarg.0
    IL_05b4:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_05b9:  ldloc.s    V_5
    IL_05bb:  ldloc.s    V_4
    IL_05bd:  dup
    IL_05be:  ldc.i4.1
    IL_05bf:  add
    IL_05c0:  stloc.s    V_4
    IL_05c2:  ldelem.i4
    IL_05c3:  stloc.s    V_6
    IL_05c5:  ldarg.0
    IL_05c6:  ldc.i4.s   5
    IL_05c8:  ldloc.s    V_6
    IL_05ca:  ldloc.s    V_0
    IL_05cc:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_05d1:  ldloc.s    V_3
    IL_05d3:  ldloc.s    V_2
    IL_05d5:  ldc.i4.1
    IL_05d6:  sub
    IL_05d7:  dup
    IL_05d8:  stloc.s    V_2
    IL_05da:  ldloc.s    V_6
    IL_05dc:  stelem.i4
    IL_05dd:  ldloc.s    V_3
    IL_05df:  ldloc.s    V_2
    IL_05e1:  ldc.i4.1
    IL_05e2:  sub
    IL_05e3:  dup
    IL_05e4:  stloc.s    V_2
    IL_05e6:  ldc.i4.s   8
    IL_05e8:  stelem.i4
    IL_05e9:  ldarg.0
    IL_05ea:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_05ef:  ldloc.s    V_5
    IL_05f1:  ldloc.s    V_4
    IL_05f3:  dup
    IL_05f4:  ldc.i4.1
    IL_05f5:  add
    IL_05f6:  stloc.s    V_4
    IL_05f8:  ldelem.i4
    IL_05f9:  dup
    IL_05fa:  stloc.s    V_6
    IL_05fc:  ldloc.s    V_3
    IL_05fe:  ldloc.s    V_2
    IL_0600:  ldc.i4.1
    IL_0601:  sub
    IL_0602:  dup
    IL_0603:  stloc.s    V_2
    IL_0605:  ldloc.s    V_6
    IL_0607:  stelem.i4
    IL_0608:  ldloc.s    V_0
    IL_060a:  beq.s      IL_0656

    IL_060c:  ldloc.s    V_3
    IL_060e:  ldloc.s    V_2
    IL_0610:  ldc.i4.1
    IL_0611:  sub
    IL_0612:  dup
    IL_0613:  stloc.s    V_2
    IL_0615:  ldloc.s    V_0
    IL_0617:  stelem.i4
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
    IL_062d:  ldc.i4.s   14
    IL_062f:  stelem.i4
    IL_0630:  ldloc.s    V_2
    IL_0632:  ldc.i4     0x1a0
    IL_0637:  ble.s      IL_0645

    IL_0639:  ldloc.s    V_4
    IL_063b:  ldc.i4     0x138
    IL_0640:  bgt        IL_04af

    IL_0645:  ldloc.s    V_3
    IL_0647:  ldloc.s    V_2
    IL_0649:  ldc.i4.1
    IL_064a:  sub
    IL_064b:  dup
    IL_064c:  stloc.s    V_2
    IL_064e:  ldc.i4.s   15
    IL_0650:  stelem.i4
    IL_0651:  br         IL_22d4

    IL_0656:  ldloc.s    V_3
    IL_0658:  ldloc.s    V_2
    IL_065a:  ldc.i4.1
    IL_065b:  sub
    IL_065c:  dup
    IL_065d:  stloc.s    V_2
    IL_065f:  ldc.i4.s   16
    IL_0661:  stelem.i4
    IL_0662:  ldarg.0
    IL_0663:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0668:  ldc.i4.s   1
    IL_066a:  ldloc.s    V_10
    IL_066c:  ldloc.s    V_0
    IL_066e:  sub
    IL_066f:  bgt        IL_22d4

    IL_0674:  ldloc.s    V_0
    IL_0676:  ldc.i4.s   1
    IL_0678:  add
    IL_0679:  stloc.s    V_0
    IL_067b:  ldc.i4.s   1
    IL_067d:  stloc.s    V_6
    IL_067f:  ldloc.s    V_1
    IL_0681:  ldloc.s    V_0
    IL_0683:  ldloc.s    V_6
    IL_0685:  dup
    IL_0686:  ldc.i4.s   1
    IL_0688:  sub
    IL_0689:  stloc.s    V_6
    IL_068b:  sub
    IL_068c:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0691:  ldstr      bytearray (00 00 00 00 0A 00 00 00 02 00 04 00 05 00 03 00 
                                    01 00 06 00 09 00 13 00 00 00 ) 
    IL_0696:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_069b:  brfalse    IL_22d4

    IL_06a0:  ldloc.s    V_6
    IL_06a2:  ldc.i4.s   0
    IL_06a4:  bgt        IL_067f

    IL_06a9:  ldarg.0
    IL_06aa:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_06af:  ldloc.s    V_10
    IL_06b1:  ldloc.s    V_0
    IL_06b3:  sub
    IL_06b4:  dup
    IL_06b5:  stloc.s    V_7
    IL_06b7:  ldc.i4.s   1
    IL_06b9:  add
    IL_06ba:  stloc.s    V_6
    IL_06bc:  ldloc.s    V_6
    IL_06be:  ldc.i4.s   1
    IL_06c0:  sub
    IL_06c1:  dup
    IL_06c2:  stloc.s    V_6
    IL_06c4:  ldc.i4.s   0
    IL_06c6:  ble        IL_06f0

    IL_06cb:  ldloc.s    V_1
    IL_06cd:  ldloc.s    V_0
    IL_06cf:  dup
    IL_06d0:  ldc.i4.s   1
    IL_06d2:  add
    IL_06d3:  stloc.s    V_0
    IL_06d5:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_06da:  ldstr      bytearray (00 00 00 00 0A 00 00 00 02 00 04 00 05 00 03 00 
                                    01 00 06 00 09 00 13 00 00 00 ) 
    IL_06df:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_06e4:  brtrue     IL_06bc

    IL_06e9:  ldloc.s    V_0
    IL_06eb:  ldc.i4.s   1
    IL_06ed:  sub
    IL_06ee:  stloc.s    V_0
    IL_06f0:  ldloc.s    V_7
    IL_06f2:  ldloc.s    V_6
    IL_06f4:  ble.s      IL_0723

    IL_06f6:  ldloc.s    V_3
    IL_06f8:  ldloc.s    V_2
    IL_06fa:  ldc.i4.1
    IL_06fb:  sub
    IL_06fc:  dup
    IL_06fd:  stloc.s    V_2
    IL_06ff:  ldloc.s    V_7
    IL_0701:  ldloc.s    V_6
    IL_0703:  sub
    IL_0704:  ldc.i4.s   1
    IL_0706:  sub
    IL_0707:  stelem.i4
    IL_0708:  ldloc.s    V_3
    IL_070a:  ldloc.s    V_2
    IL_070c:  ldc.i4.1
    IL_070d:  sub
    IL_070e:  dup
    IL_070f:  stloc.s    V_2
    IL_0711:  ldloc.s    V_0
    IL_0713:  ldc.i4.s   1
    IL_0715:  sub
    IL_0716:  stelem.i4
    IL_0717:  ldloc.s    V_3
    IL_0719:  ldloc.s    V_2
    IL_071b:  ldc.i4.1
    IL_071c:  sub
    IL_071d:  dup
    IL_071e:  stloc.s    V_2
    IL_0720:  ldc.i4.s   17
    IL_0722:  stelem.i4
    IL_0723:  ldarg.0
    IL_0724:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0729:  ldloc.s    V_0
    IL_072b:  ldloc.s    V_10
    IL_072d:  bge        IL_22d4

    IL_0732:  ldloc.s    V_1
    IL_0734:  ldloc.s    V_0
    IL_0736:  dup
    IL_0737:  ldc.i4.s   1
    IL_0739:  add
    IL_073a:  stloc.s    V_0
    IL_073c:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0741:  ldc.i4.s   64
    IL_0743:  bne.un     IL_22d4

    IL_0748:  ldarg.0
    IL_0749:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_074e:  ldloc.s    V_10
    IL_0750:  ldloc.s    V_0
    IL_0752:  sub
    IL_0753:  dup
    IL_0754:  stloc.s    V_7
    IL_0756:  ldc.i4.s   1
    IL_0758:  add
    IL_0759:  stloc.s    V_6
    IL_075b:  ldloc.s    V_6
    IL_075d:  ldc.i4.s   1
    IL_075f:  sub
    IL_0760:  dup
    IL_0761:  stloc.s    V_6
    IL_0763:  ldc.i4.s   0
    IL_0765:  ble        IL_078f

    IL_076a:  ldloc.s    V_1
    IL_076c:  ldloc.s    V_0
    IL_076e:  dup
    IL_076f:  ldc.i4.s   1
    IL_0771:  add
    IL_0772:  stloc.s    V_0
    IL_0774:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0779:  ldstr      bytearray (00 00 04 00 00 00 23 00 24 00 2D 00 2E 00 )       // ......#.$.-...
    IL_077e:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0783:  brtrue     IL_075b

    IL_0788:  ldloc.s    V_0
    IL_078a:  ldc.i4.s   1
    IL_078c:  sub
    IL_078d:  stloc.s    V_0
    IL_078f:  ldloc.s    V_7
    IL_0791:  ldloc.s    V_6
    IL_0793:  ble.s      IL_07c2

    IL_0795:  ldloc.s    V_3
    IL_0797:  ldloc.s    V_2
    IL_0799:  ldc.i4.1
    IL_079a:  sub
    IL_079b:  dup
    IL_079c:  stloc.s    V_2
    IL_079e:  ldloc.s    V_7
    IL_07a0:  ldloc.s    V_6
    IL_07a2:  sub
    IL_07a3:  ldc.i4.s   1
    IL_07a5:  sub
    IL_07a6:  stelem.i4
    IL_07a7:  ldloc.s    V_3
    IL_07a9:  ldloc.s    V_2
    IL_07ab:  ldc.i4.1
    IL_07ac:  sub
    IL_07ad:  dup
    IL_07ae:  stloc.s    V_2
    IL_07b0:  ldloc.s    V_0
    IL_07b2:  ldc.i4.s   1
    IL_07b4:  sub
    IL_07b5:  stelem.i4
    IL_07b6:  ldloc.s    V_3
    IL_07b8:  ldloc.s    V_2
    IL_07ba:  ldc.i4.1
    IL_07bb:  sub
    IL_07bc:  dup
    IL_07bd:  stloc.s    V_2
    IL_07bf:  ldc.i4.s   18
    IL_07c1:  stelem.i4
    IL_07c2:  ldarg.0
    IL_07c3:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_07c8:  ldc.i4.s   1
    IL_07ca:  ldloc.s    V_10
    IL_07cc:  ldloc.s    V_0
    IL_07ce:  sub
    IL_07cf:  bgt        IL_22d4

    IL_07d4:  ldloc.s    V_0
    IL_07d6:  ldc.i4.s   1
    IL_07d8:  add
    IL_07d9:  stloc.s    V_0
    IL_07db:  ldc.i4.s   1
    IL_07dd:  stloc.s    V_6
    IL_07df:  ldloc.s    V_1
    IL_07e1:  ldloc.s    V_0
    IL_07e3:  ldloc.s    V_6
    IL_07e5:  dup
    IL_07e6:  ldc.i4.s   1
    IL_07e8:  sub
    IL_07e9:  stloc.s    V_6
    IL_07eb:  sub
    IL_07ec:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_07f1:  ldstr      bytearray (00 00 04 00 0A 00 2B 00 2C 00 2E 00 2F 00 00 00   // ......+.,.../...
                                    02 00 04 00 05 00 03 00 01 00 06 00 09 00 13 00 
                                    00 00 ) 
    IL_07f6:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_07fb:  brfalse    IL_22d4

    IL_0800:  ldloc.s    V_6
    IL_0802:  ldc.i4.s   0
    IL_0804:  bgt        IL_07df

    IL_0809:  ldarg.0
    IL_080a:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_080f:  ldloc.s    V_10
    IL_0811:  ldloc.s    V_0
    IL_0813:  sub
    IL_0814:  dup
    IL_0815:  stloc.s    V_7
    IL_0817:  ldc.i4.s   1
    IL_0819:  add
    IL_081a:  stloc.s    V_6
    IL_081c:  ldloc.s    V_6
    IL_081e:  ldc.i4.s   1
    IL_0820:  sub
    IL_0821:  dup
    IL_0822:  stloc.s    V_6
    IL_0824:  ldc.i4.s   0
    IL_0826:  ble        IL_0850

    IL_082b:  ldloc.s    V_1
    IL_082d:  ldloc.s    V_0
    IL_082f:  dup
    IL_0830:  ldc.i4.s   1
    IL_0832:  add
    IL_0833:  stloc.s    V_0
    IL_0835:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_083a:  ldstr      bytearray (00 00 04 00 0A 00 2B 00 2C 00 2E 00 2F 00 00 00   // ......+.,.../...
                                    02 00 04 00 05 00 03 00 01 00 06 00 09 00 13 00 
                                    00 00 ) 
    IL_083f:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0844:  brtrue     IL_081c

    IL_0849:  ldloc.s    V_0
    IL_084b:  ldc.i4.s   1
    IL_084d:  sub
    IL_084e:  stloc.s    V_0
    IL_0850:  ldloc.s    V_7
    IL_0852:  ldloc.s    V_6
    IL_0854:  ble.s      IL_0883

    IL_0856:  ldloc.s    V_3
    IL_0858:  ldloc.s    V_2
    IL_085a:  ldc.i4.1
    IL_085b:  sub
    IL_085c:  dup
    IL_085d:  stloc.s    V_2
    IL_085f:  ldloc.s    V_7
    IL_0861:  ldloc.s    V_6
    IL_0863:  sub
    IL_0864:  ldc.i4.s   1
    IL_0866:  sub
    IL_0867:  stelem.i4
    IL_0868:  ldloc.s    V_3
    IL_086a:  ldloc.s    V_2
    IL_086c:  ldc.i4.1
    IL_086d:  sub
    IL_086e:  dup
    IL_086f:  stloc.s    V_2
    IL_0871:  ldloc.s    V_0
    IL_0873:  ldc.i4.s   1
    IL_0875:  sub
    IL_0876:  stelem.i4
    IL_0877:  ldloc.s    V_3
    IL_0879:  ldloc.s    V_2
    IL_087b:  ldc.i4.1
    IL_087c:  sub
    IL_087d:  dup
    IL_087e:  stloc.s    V_2
    IL_0880:  ldc.i4.s   19
    IL_0882:  stelem.i4
    IL_0883:  ldarg.0
    IL_0884:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0889:  ldloc.s    V_5
    IL_088b:  ldloc.s    V_4
    IL_088d:  dup
    IL_088e:  ldc.i4.1
    IL_088f:  add
    IL_0890:  stloc.s    V_4
    IL_0892:  ldelem.i4
    IL_0893:  stloc.s    V_6
    IL_0895:  ldarg.0
    IL_0896:  ldc.i4.s   2
    IL_0898:  ldloc.s    V_6
    IL_089a:  ldloc.s    V_0
    IL_089c:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_08a1:  ldloc.s    V_3
    IL_08a3:  ldloc.s    V_2
    IL_08a5:  ldc.i4.1
    IL_08a6:  sub
    IL_08a7:  dup
    IL_08a8:  stloc.s    V_2
    IL_08aa:  ldloc.s    V_6
    IL_08ac:  stelem.i4
    IL_08ad:  ldloc.s    V_3
    IL_08af:  ldloc.s    V_2
    IL_08b1:  ldc.i4.1
    IL_08b2:  sub
    IL_08b3:  dup
    IL_08b4:  stloc.s    V_2
    IL_08b6:  ldc.i4.s   8
    IL_08b8:  stelem.i4
    IL_08b9:  ldarg.0
    IL_08ba:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_08bf:  ldloc.s    V_0
    IL_08c1:  ldloc.s    V_10
    IL_08c3:  bge        IL_22d4

    IL_08c8:  ldloc.s    V_1
    IL_08ca:  ldloc.s    V_0
    IL_08cc:  dup
    IL_08cd:  ldc.i4.s   1
    IL_08cf:  add
    IL_08d0:  stloc.s    V_0
    IL_08d2:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_08d7:  ldc.i4.s   36
    IL_08d9:  bne.un     IL_22d4

    IL_08de:  ldarg.0
    IL_08df:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_08e4:  br         IL_1fca

    IL_08e9:  ldarg.0
    IL_08ea:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_08ef:  ldloc.s    V_3
    IL_08f1:  ldloc.s    V_2
    IL_08f3:  ldc.i4.1
    IL_08f4:  sub
    IL_08f5:  dup
    IL_08f6:  stloc.s    V_2
    IL_08f8:  ldloc.s    V_0
    IL_08fa:  stelem.i4
    IL_08fb:  ldloc.s    V_3
    IL_08fd:  ldloc.s    V_2
    IL_08ff:  ldc.i4.1
    IL_0900:  sub
    IL_0901:  dup
    IL_0902:  stloc.s    V_2
    IL_0904:  ldc.i4.s   20
    IL_0906:  stelem.i4
    IL_0907:  ldarg.0
    IL_0908:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_090d:  ldc.i4.s   1
    IL_090f:  ldloc.s    V_10
    IL_0911:  ldloc.s    V_0
    IL_0913:  sub
    IL_0914:  bgt        IL_22d4

    IL_0919:  ldloc.s    V_0
    IL_091b:  ldc.i4.s   1
    IL_091d:  add
    IL_091e:  stloc.s    V_0
    IL_0920:  ldc.i4.s   1
    IL_0922:  stloc.s    V_6
    IL_0924:  ldloc.s    V_1
    IL_0926:  ldloc.s    V_0
    IL_0928:  ldloc.s    V_6
    IL_092a:  dup
    IL_092b:  ldc.i4.s   1
    IL_092d:  sub
    IL_092e:  stloc.s    V_6
    IL_0930:  sub
    IL_0931:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0936:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_093b:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0940:  brfalse    IL_22d4

    IL_0945:  ldloc.s    V_6
    IL_0947:  ldc.i4.s   0
    IL_0949:  bgt        IL_0924

    IL_094e:  ldarg.0
    IL_094f:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0954:  ldloc.s    V_10
    IL_0956:  ldloc.s    V_0
    IL_0958:  sub
    IL_0959:  dup
    IL_095a:  stloc.s    V_7
    IL_095c:  ldc.i4.s   1
    IL_095e:  add
    IL_095f:  stloc.s    V_6
    IL_0961:  ldloc.s    V_6
    IL_0963:  ldc.i4.s   1
    IL_0965:  sub
    IL_0966:  dup
    IL_0967:  stloc.s    V_6
    IL_0969:  ldc.i4.s   0
    IL_096b:  ble        IL_0995

    IL_0970:  ldloc.s    V_1
    IL_0972:  ldloc.s    V_0
    IL_0974:  dup
    IL_0975:  ldc.i4.s   1
    IL_0977:  add
    IL_0978:  stloc.s    V_0
    IL_097a:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_097f:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_0984:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0989:  brtrue     IL_0961

    IL_098e:  ldloc.s    V_0
    IL_0990:  ldc.i4.s   1
    IL_0992:  sub
    IL_0993:  stloc.s    V_0
    IL_0995:  ldloc.s    V_7
    IL_0997:  ldloc.s    V_6
    IL_0999:  ble.s      IL_09c8

    IL_099b:  ldloc.s    V_3
    IL_099d:  ldloc.s    V_2
    IL_099f:  ldc.i4.1
    IL_09a0:  sub
    IL_09a1:  dup
    IL_09a2:  stloc.s    V_2
    IL_09a4:  ldloc.s    V_7
    IL_09a6:  ldloc.s    V_6
    IL_09a8:  sub
    IL_09a9:  ldc.i4.s   1
    IL_09ab:  sub
    IL_09ac:  stelem.i4
    IL_09ad:  ldloc.s    V_3
    IL_09af:  ldloc.s    V_2
    IL_09b1:  ldc.i4.1
    IL_09b2:  sub
    IL_09b3:  dup
    IL_09b4:  stloc.s    V_2
    IL_09b6:  ldloc.s    V_0
    IL_09b8:  ldc.i4.s   1
    IL_09ba:  sub
    IL_09bb:  stelem.i4
    IL_09bc:  ldloc.s    V_3
    IL_09be:  ldloc.s    V_2
    IL_09c0:  ldc.i4.1
    IL_09c1:  sub
    IL_09c2:  dup
    IL_09c3:  stloc.s    V_2
    IL_09c5:  ldc.i4.s   21
    IL_09c7:  stelem.i4
    IL_09c8:  ldarg.0
    IL_09c9:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_09ce:  ldc.i4.s   2
    IL_09d0:  ldloc.s    V_10
    IL_09d2:  ldloc.s    V_0
    IL_09d4:  sub
    IL_09d5:  bgt        IL_22d4

    IL_09da:  ldloc.s    V_1
    IL_09dc:  ldloc.s    V_0
    IL_09de:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_09e3:  ldc.i4.s   58
    IL_09e5:  bne.un     IL_22d4

    IL_09ea:  ldloc.s    V_1
    IL_09ec:  ldloc.s    V_0
    IL_09ee:  ldc.i4.s   1
    IL_09f0:  add
    IL_09f1:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_09f6:  ldc.i4.s   123
    IL_09f8:  bne.un     IL_22d4

    IL_09fd:  ldloc.s    V_0
    IL_09ff:  ldc.i4.s   2
    IL_0a01:  add
    IL_0a02:  stloc.s    V_0
    IL_0a04:  ldarg.0
    IL_0a05:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0a0a:  br         IL_1fca

    IL_0a0f:  ldarg.0
    IL_0a10:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0a15:  ldloc.s    V_0
    IL_0a17:  ldloc.s    V_10
    IL_0a19:  bge        IL_22d4

    IL_0a1e:  ldloc.s    V_1
    IL_0a20:  ldloc.s    V_0
    IL_0a22:  dup
    IL_0a23:  ldc.i4.s   1
    IL_0a25:  add
    IL_0a26:  stloc.s    V_0
    IL_0a28:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0a2d:  ldstr      bytearray (00 00 00 00 0A 00 00 00 02 00 04 00 05 00 03 00 
                                    01 00 06 00 09 00 13 00 00 00 ) 
    IL_0a32:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0a37:  brfalse    IL_22d4

    IL_0a3c:  ldarg.0
    IL_0a3d:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0a42:  ldloc.s    V_5
    IL_0a44:  ldloc.s    V_4
    IL_0a46:  ldc.i4.1
    IL_0a47:  sub
    IL_0a48:  dup
    IL_0a49:  stloc.s    V_4
    IL_0a4b:  ldc.i4.s   -1
    IL_0a4d:  stelem.i4
    IL_0a4e:  ldloc.s    V_5
    IL_0a50:  ldloc.s    V_4
    IL_0a52:  ldc.i4.1
    IL_0a53:  sub
    IL_0a54:  dup
    IL_0a55:  stloc.s    V_4
    IL_0a57:  ldc.i4.s   0
    IL_0a59:  stelem.i4
    IL_0a5a:  ldloc.s    V_3
    IL_0a5c:  ldloc.s    V_2
    IL_0a5e:  ldc.i4.1
    IL_0a5f:  sub
    IL_0a60:  dup
    IL_0a61:  stloc.s    V_2
    IL_0a63:  ldc.i4.s   5
    IL_0a65:  stelem.i4
    IL_0a66:  ldarg.0
    IL_0a67:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0a6c:  br         IL_0cf9

    IL_0a71:  ldarg.0
    IL_0a72:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0a77:  ldloc.s    V_5
    IL_0a79:  ldloc.s    V_4
    IL_0a7b:  ldc.i4.1
    IL_0a7c:  sub
    IL_0a7d:  dup
    IL_0a7e:  stloc.s    V_4
    IL_0a80:  ldloc.s    V_0
    IL_0a82:  stelem.i4
    IL_0a83:  ldloc.s    V_3
    IL_0a85:  ldloc.s    V_2
    IL_0a87:  ldc.i4.1
    IL_0a88:  sub
    IL_0a89:  dup
    IL_0a8a:  stloc.s    V_2
    IL_0a8c:  ldc.i4.s   1
    IL_0a8e:  stelem.i4
    IL_0a8f:  ldarg.0
    IL_0a90:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0a95:  ldloc.s    V_0
    IL_0a97:  ldloc.s    V_10
    IL_0a99:  bge        IL_22d4

    IL_0a9e:  ldloc.s    V_1
    IL_0aa0:  ldloc.s    V_0
    IL_0aa2:  dup
    IL_0aa3:  ldc.i4.s   1
    IL_0aa5:  add
    IL_0aa6:  stloc.s    V_0
    IL_0aa8:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0aad:  ldc.i4.s   35
    IL_0aaf:  bne.un     IL_22d4

    IL_0ab4:  ldarg.0
    IL_0ab5:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0aba:  ldc.i4.s   1
    IL_0abc:  ldloc.s    V_10
    IL_0abe:  ldloc.s    V_0
    IL_0ac0:  sub
    IL_0ac1:  bgt        IL_22d4

    IL_0ac6:  ldloc.s    V_0
    IL_0ac8:  ldc.i4.s   1
    IL_0aca:  add
    IL_0acb:  stloc.s    V_0
    IL_0acd:  ldc.i4.s   1
    IL_0acf:  stloc.s    V_6
    IL_0ad1:  ldloc.s    V_1
    IL_0ad3:  ldloc.s    V_0
    IL_0ad5:  ldloc.s    V_6
    IL_0ad7:  dup
    IL_0ad8:  ldc.i4.s   1
    IL_0ada:  sub
    IL_0adb:  stloc.s    V_6
    IL_0add:  sub
    IL_0ade:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0ae3:  ldstr      bytearray (00 00 00 00 01 00 09 00 ) 
    IL_0ae8:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0aed:  brfalse    IL_22d4

    IL_0af2:  ldloc.s    V_6
    IL_0af4:  ldc.i4.s   0
    IL_0af6:  bgt        IL_0ad1

    IL_0afb:  ldarg.0
    IL_0afc:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0b01:  ldloc.s    V_10
    IL_0b03:  ldloc.s    V_0
    IL_0b05:  sub
    IL_0b06:  dup
    IL_0b07:  stloc.s    V_7
    IL_0b09:  ldc.i4.s   1
    IL_0b0b:  add
    IL_0b0c:  stloc.s    V_6
    IL_0b0e:  ldloc.s    V_6
    IL_0b10:  ldc.i4.s   1
    IL_0b12:  sub
    IL_0b13:  dup
    IL_0b14:  stloc.s    V_6
    IL_0b16:  ldc.i4.s   0
    IL_0b18:  ble        IL_0b42

    IL_0b1d:  ldloc.s    V_1
    IL_0b1f:  ldloc.s    V_0
    IL_0b21:  dup
    IL_0b22:  ldc.i4.s   1
    IL_0b24:  add
    IL_0b25:  stloc.s    V_0
    IL_0b27:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0b2c:  ldstr      bytearray (00 00 00 00 01 00 09 00 ) 
    IL_0b31:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0b36:  brtrue     IL_0b0e

    IL_0b3b:  ldloc.s    V_0
    IL_0b3d:  ldc.i4.s   1
    IL_0b3f:  sub
    IL_0b40:  stloc.s    V_0
    IL_0b42:  ldloc.s    V_7
    IL_0b44:  ldloc.s    V_6
    IL_0b46:  ble.s      IL_0b75

    IL_0b48:  ldloc.s    V_3
    IL_0b4a:  ldloc.s    V_2
    IL_0b4c:  ldc.i4.1
    IL_0b4d:  sub
    IL_0b4e:  dup
    IL_0b4f:  stloc.s    V_2
    IL_0b51:  ldloc.s    V_7
    IL_0b53:  ldloc.s    V_6
    IL_0b55:  sub
    IL_0b56:  ldc.i4.s   1
    IL_0b58:  sub
    IL_0b59:  stelem.i4
    IL_0b5a:  ldloc.s    V_3
    IL_0b5c:  ldloc.s    V_2
    IL_0b5e:  ldc.i4.1
    IL_0b5f:  sub
    IL_0b60:  dup
    IL_0b61:  stloc.s    V_2
    IL_0b63:  ldloc.s    V_0
    IL_0b65:  ldc.i4.s   1
    IL_0b67:  sub
    IL_0b68:  stelem.i4
    IL_0b69:  ldloc.s    V_3
    IL_0b6b:  ldloc.s    V_2
    IL_0b6d:  ldc.i4.1
    IL_0b6e:  sub
    IL_0b6f:  dup
    IL_0b70:  stloc.s    V_2
    IL_0b72:  ldc.i4.s   22
    IL_0b74:  stelem.i4
    IL_0b75:  ldarg.0
    IL_0b76:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0b7b:  ldloc.s    V_5
    IL_0b7d:  ldloc.s    V_4
    IL_0b7f:  ldc.i4.1
    IL_0b80:  sub
    IL_0b81:  dup
    IL_0b82:  stloc.s    V_4
    IL_0b84:  ldc.i4.s   -1
    IL_0b86:  stelem.i4
    IL_0b87:  ldloc.s    V_5
    IL_0b89:  ldloc.s    V_4
    IL_0b8b:  ldc.i4.1
    IL_0b8c:  sub
    IL_0b8d:  dup
    IL_0b8e:  stloc.s    V_4
    IL_0b90:  ldc.i4.s   0
    IL_0b92:  stelem.i4
    IL_0b93:  ldloc.s    V_3
    IL_0b95:  ldloc.s    V_2
    IL_0b97:  ldc.i4.1
    IL_0b98:  sub
    IL_0b99:  dup
    IL_0b9a:  stloc.s    V_2
    IL_0b9c:  ldc.i4.s   5
    IL_0b9e:  stelem.i4
    IL_0b9f:  ldarg.0
    IL_0ba0:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0ba5:  br         IL_0c23

    IL_0baa:  ldarg.0
    IL_0bab:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0bb0:  ldloc.s    V_5
    IL_0bb2:  ldloc.s    V_4
    IL_0bb4:  ldc.i4.1
    IL_0bb5:  sub
    IL_0bb6:  dup
    IL_0bb7:  stloc.s    V_4
    IL_0bb9:  ldloc.s    V_0
    IL_0bbb:  stelem.i4
    IL_0bbc:  ldloc.s    V_3
    IL_0bbe:  ldloc.s    V_2
    IL_0bc0:  ldc.i4.1
    IL_0bc1:  sub
    IL_0bc2:  dup
    IL_0bc3:  stloc.s    V_2
    IL_0bc5:  ldc.i4.s   1
    IL_0bc7:  stelem.i4
    IL_0bc8:  ldarg.0
    IL_0bc9:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0bce:  ldloc.s    V_0
    IL_0bd0:  ldloc.s    V_10
    IL_0bd2:  bge        IL_22d4

    IL_0bd7:  ldloc.s    V_1
    IL_0bd9:  ldloc.s    V_0
    IL_0bdb:  dup
    IL_0bdc:  ldc.i4.s   1
    IL_0bde:  add
    IL_0bdf:  stloc.s    V_0
    IL_0be1:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0be6:  ldc.i4.s   43
    IL_0be8:  bne.un     IL_22d4

    IL_0bed:  ldarg.0
    IL_0bee:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0bf3:  ldloc.s    V_5
    IL_0bf5:  ldloc.s    V_4
    IL_0bf7:  dup
    IL_0bf8:  ldc.i4.1
    IL_0bf9:  add
    IL_0bfa:  stloc.s    V_4
    IL_0bfc:  ldelem.i4
    IL_0bfd:  stloc.s    V_6
    IL_0bff:  ldarg.0
    IL_0c00:  ldc.i4.s   7
    IL_0c02:  ldloc.s    V_6
    IL_0c04:  ldloc.s    V_0
    IL_0c06:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_0c0b:  ldloc.s    V_3
    IL_0c0d:  ldloc.s    V_2
    IL_0c0f:  ldc.i4.1
    IL_0c10:  sub
    IL_0c11:  dup
    IL_0c12:  stloc.s    V_2
    IL_0c14:  ldloc.s    V_6
    IL_0c16:  stelem.i4
    IL_0c17:  ldloc.s    V_3
    IL_0c19:  ldloc.s    V_2
    IL_0c1b:  ldc.i4.1
    IL_0c1c:  sub
    IL_0c1d:  dup
    IL_0c1e:  stloc.s    V_2
    IL_0c20:  ldc.i4.s   8
    IL_0c22:  stelem.i4
    IL_0c23:  ldarg.0
    IL_0c24:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0c29:  ldloc.s    V_5
    IL_0c2b:  ldloc.s    V_4
    IL_0c2d:  dup
    IL_0c2e:  ldc.i4.1
    IL_0c2f:  add
    IL_0c30:  stloc.s    V_4
    IL_0c32:  ldelem.i4
    IL_0c33:  stloc.s    V_6
    IL_0c35:  ldloc.s    V_5
    IL_0c37:  ldloc.s    V_4
    IL_0c39:  dup
    IL_0c3a:  ldc.i4.1
    IL_0c3b:  add
    IL_0c3c:  stloc.s    V_4
    IL_0c3e:  ldelem.i4
    IL_0c3f:  dup
    IL_0c40:  stloc.s    V_7
    IL_0c42:  ldloc.s    V_3
    IL_0c44:  ldloc.s    V_2
    IL_0c46:  ldc.i4.1
    IL_0c47:  sub
    IL_0c48:  dup
    IL_0c49:  stloc.s    V_2
    IL_0c4b:  ldloc.s    V_7
    IL_0c4d:  stelem.i4
    IL_0c4e:  ldloc.s    V_0
    IL_0c50:  bne.un.s   IL_0c58

    IL_0c52:  ldloc.s    V_6
    IL_0c54:  ldc.i4.s   0
    IL_0c56:  bge.s      IL_0cab

    IL_0c58:  ldloc.s    V_6
    IL_0c5a:  ldc.i4.s   1
    IL_0c5c:  bge.s      IL_0cab

    IL_0c5e:  ldloc.s    V_5
    IL_0c60:  ldloc.s    V_4
    IL_0c62:  ldc.i4.1
    IL_0c63:  sub
    IL_0c64:  dup
    IL_0c65:  stloc.s    V_4
    IL_0c67:  ldloc.s    V_0
    IL_0c69:  stelem.i4
    IL_0c6a:  ldloc.s    V_5
    IL_0c6c:  ldloc.s    V_4
    IL_0c6e:  ldc.i4.1
    IL_0c6f:  sub
    IL_0c70:  dup
    IL_0c71:  stloc.s    V_4
    IL_0c73:  ldloc.s    V_6
    IL_0c75:  ldc.i4.s   1
    IL_0c77:  add
    IL_0c78:  stelem.i4
    IL_0c79:  ldloc.s    V_3
    IL_0c7b:  ldloc.s    V_2
    IL_0c7d:  ldc.i4.1
    IL_0c7e:  sub
    IL_0c7f:  dup
    IL_0c80:  stloc.s    V_2
    IL_0c82:  ldc.i4.s   23
    IL_0c84:  stelem.i4
    IL_0c85:  ldloc.s    V_2
    IL_0c87:  ldc.i4     0x1a0
    IL_0c8c:  ble.s      IL_0c9a

    IL_0c8e:  ldloc.s    V_4
    IL_0c90:  ldc.i4     0x138
    IL_0c95:  bgt        IL_0baa

    IL_0c9a:  ldloc.s    V_3
    IL_0c9c:  ldloc.s    V_2
    IL_0c9e:  ldc.i4.1
    IL_0c9f:  sub
    IL_0ca0:  dup
    IL_0ca1:  stloc.s    V_2
    IL_0ca3:  ldc.i4.s   24
    IL_0ca5:  stelem.i4
    IL_0ca6:  br         IL_22d4

    IL_0cab:  ldloc.s    V_3
    IL_0cad:  ldloc.s    V_2
    IL_0caf:  ldc.i4.1
    IL_0cb0:  sub
    IL_0cb1:  dup
    IL_0cb2:  stloc.s    V_2
    IL_0cb4:  ldloc.s    V_6
    IL_0cb6:  stelem.i4
    IL_0cb7:  ldloc.s    V_3
    IL_0cb9:  ldloc.s    V_2
    IL_0cbb:  ldc.i4.1
    IL_0cbc:  sub
    IL_0cbd:  dup
    IL_0cbe:  stloc.s    V_2
    IL_0cc0:  ldc.i4.s   11
    IL_0cc2:  stelem.i4
    IL_0cc3:  ldarg.0
    IL_0cc4:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0cc9:  ldloc.s    V_5
    IL_0ccb:  ldloc.s    V_4
    IL_0ccd:  dup
    IL_0cce:  ldc.i4.1
    IL_0ccf:  add
    IL_0cd0:  stloc.s    V_4
    IL_0cd2:  ldelem.i4
    IL_0cd3:  stloc.s    V_6
    IL_0cd5:  ldarg.0
    IL_0cd6:  ldc.i4.s   6
    IL_0cd8:  ldloc.s    V_6
    IL_0cda:  ldloc.s    V_0
    IL_0cdc:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_0ce1:  ldloc.s    V_3
    IL_0ce3:  ldloc.s    V_2
    IL_0ce5:  ldc.i4.1
    IL_0ce6:  sub
    IL_0ce7:  dup
    IL_0ce8:  stloc.s    V_2
    IL_0cea:  ldloc.s    V_6
    IL_0cec:  stelem.i4
    IL_0ced:  ldloc.s    V_3
    IL_0cef:  ldloc.s    V_2
    IL_0cf1:  ldc.i4.1
    IL_0cf2:  sub
    IL_0cf3:  dup
    IL_0cf4:  stloc.s    V_2
    IL_0cf6:  ldc.i4.s   8
    IL_0cf8:  stelem.i4
    IL_0cf9:  ldarg.0
    IL_0cfa:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0cff:  ldloc.s    V_5
    IL_0d01:  ldloc.s    V_4
    IL_0d03:  dup
    IL_0d04:  ldc.i4.1
    IL_0d05:  add
    IL_0d06:  stloc.s    V_4
    IL_0d08:  ldelem.i4
    IL_0d09:  stloc.s    V_6
    IL_0d0b:  ldloc.s    V_5
    IL_0d0d:  ldloc.s    V_4
    IL_0d0f:  dup
    IL_0d10:  ldc.i4.1
    IL_0d11:  add
    IL_0d12:  stloc.s    V_4
    IL_0d14:  ldelem.i4
    IL_0d15:  dup
    IL_0d16:  stloc.s    V_7
    IL_0d18:  ldloc.s    V_3
    IL_0d1a:  ldloc.s    V_2
    IL_0d1c:  ldc.i4.1
    IL_0d1d:  sub
    IL_0d1e:  dup
    IL_0d1f:  stloc.s    V_2
    IL_0d21:  ldloc.s    V_7
    IL_0d23:  stelem.i4
    IL_0d24:  ldloc.s    V_0
    IL_0d26:  bne.un.s   IL_0d2e

    IL_0d28:  ldloc.s    V_6
    IL_0d2a:  ldc.i4.s   0
    IL_0d2c:  bge.s      IL_0d81

    IL_0d2e:  ldloc.s    V_6
    IL_0d30:  ldc.i4.s   1
    IL_0d32:  bge.s      IL_0d81

    IL_0d34:  ldloc.s    V_5
    IL_0d36:  ldloc.s    V_4
    IL_0d38:  ldc.i4.1
    IL_0d39:  sub
    IL_0d3a:  dup
    IL_0d3b:  stloc.s    V_4
    IL_0d3d:  ldloc.s    V_0
    IL_0d3f:  stelem.i4
    IL_0d40:  ldloc.s    V_5
    IL_0d42:  ldloc.s    V_4
    IL_0d44:  ldc.i4.1
    IL_0d45:  sub
    IL_0d46:  dup
    IL_0d47:  stloc.s    V_4
    IL_0d49:  ldloc.s    V_6
    IL_0d4b:  ldc.i4.s   1
    IL_0d4d:  add
    IL_0d4e:  stelem.i4
    IL_0d4f:  ldloc.s    V_3
    IL_0d51:  ldloc.s    V_2
    IL_0d53:  ldc.i4.1
    IL_0d54:  sub
    IL_0d55:  dup
    IL_0d56:  stloc.s    V_2
    IL_0d58:  ldc.i4.s   25
    IL_0d5a:  stelem.i4
    IL_0d5b:  ldloc.s    V_2
    IL_0d5d:  ldc.i4     0x1a0
    IL_0d62:  ble.s      IL_0d70

    IL_0d64:  ldloc.s    V_4
    IL_0d66:  ldc.i4     0x138
    IL_0d6b:  bgt        IL_0a71

    IL_0d70:  ldloc.s    V_3
    IL_0d72:  ldloc.s    V_2
    IL_0d74:  ldc.i4.1
    IL_0d75:  sub
    IL_0d76:  dup
    IL_0d77:  stloc.s    V_2
    IL_0d79:  ldc.i4.s   26
    IL_0d7b:  stelem.i4
    IL_0d7c:  br         IL_22d4

    IL_0d81:  ldloc.s    V_3
    IL_0d83:  ldloc.s    V_2
    IL_0d85:  ldc.i4.1
    IL_0d86:  sub
    IL_0d87:  dup
    IL_0d88:  stloc.s    V_2
    IL_0d8a:  ldloc.s    V_6
    IL_0d8c:  stelem.i4
    IL_0d8d:  ldloc.s    V_3
    IL_0d8f:  ldloc.s    V_2
    IL_0d91:  ldc.i4.1
    IL_0d92:  sub
    IL_0d93:  dup
    IL_0d94:  stloc.s    V_2
    IL_0d96:  ldc.i4.s   11
    IL_0d98:  stelem.i4
    IL_0d99:  ldarg.0
    IL_0d9a:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0d9f:  ldloc.s    V_5
    IL_0da1:  ldloc.s    V_4
    IL_0da3:  ldc.i4.1
    IL_0da4:  sub
    IL_0da5:  dup
    IL_0da6:  stloc.s    V_4
    IL_0da8:  ldc.i4.s   -1
    IL_0daa:  stelem.i4
    IL_0dab:  ldloc.s    V_5
    IL_0dad:  ldloc.s    V_4
    IL_0daf:  ldc.i4.1
    IL_0db0:  sub
    IL_0db1:  dup
    IL_0db2:  stloc.s    V_4
    IL_0db4:  ldc.i4.s   0
    IL_0db6:  stelem.i4
    IL_0db7:  ldloc.s    V_3
    IL_0db9:  ldloc.s    V_2
    IL_0dbb:  ldc.i4.1
    IL_0dbc:  sub
    IL_0dbd:  dup
    IL_0dbe:  stloc.s    V_2
    IL_0dc0:  ldc.i4.s   5
    IL_0dc2:  stelem.i4
    IL_0dc3:  ldarg.0
    IL_0dc4:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0dc9:  br         IL_0f2d

    IL_0dce:  ldarg.0
    IL_0dcf:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0dd4:  ldloc.s    V_5
    IL_0dd6:  ldloc.s    V_4
    IL_0dd8:  ldc.i4.1
    IL_0dd9:  sub
    IL_0dda:  dup
    IL_0ddb:  stloc.s    V_4
    IL_0ddd:  ldloc.s    V_0
    IL_0ddf:  stelem.i4
    IL_0de0:  ldloc.s    V_3
    IL_0de2:  ldloc.s    V_2
    IL_0de4:  ldc.i4.1
    IL_0de5:  sub
    IL_0de6:  dup
    IL_0de7:  stloc.s    V_2
    IL_0de9:  ldc.i4.s   1
    IL_0deb:  stelem.i4
    IL_0dec:  ldarg.0
    IL_0ded:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0df2:  ldloc.s    V_0
    IL_0df4:  ldloc.s    V_10
    IL_0df6:  bge        IL_22d4

    IL_0dfb:  ldloc.s    V_1
    IL_0dfd:  ldloc.s    V_0
    IL_0dff:  dup
    IL_0e00:  ldc.i4.s   1
    IL_0e02:  add
    IL_0e03:  stloc.s    V_0
    IL_0e05:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0e0a:  ldc.i4.s   91
    IL_0e0c:  bne.un     IL_22d4

    IL_0e11:  ldarg.0
    IL_0e12:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0e17:  ldc.i4.s   1
    IL_0e19:  ldloc.s    V_10
    IL_0e1b:  ldloc.s    V_0
    IL_0e1d:  sub
    IL_0e1e:  bgt        IL_22d4

    IL_0e23:  ldloc.s    V_0
    IL_0e25:  ldc.i4.s   1
    IL_0e27:  add
    IL_0e28:  stloc.s    V_0
    IL_0e2a:  ldc.i4.s   1
    IL_0e2c:  stloc.s    V_6
    IL_0e2e:  ldloc.s    V_1
    IL_0e30:  ldloc.s    V_0
    IL_0e32:  ldloc.s    V_6
    IL_0e34:  dup
    IL_0e35:  ldc.i4.s   1
    IL_0e37:  sub
    IL_0e38:  stloc.s    V_6
    IL_0e3a:  sub
    IL_0e3b:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0e40:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_0e45:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0e4a:  brfalse    IL_22d4

    IL_0e4f:  ldloc.s    V_6
    IL_0e51:  ldc.i4.s   0
    IL_0e53:  bgt        IL_0e2e

    IL_0e58:  ldarg.0
    IL_0e59:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0e5e:  ldloc.s    V_10
    IL_0e60:  ldloc.s    V_0
    IL_0e62:  sub
    IL_0e63:  dup
    IL_0e64:  stloc.s    V_7
    IL_0e66:  ldc.i4.s   1
    IL_0e68:  add
    IL_0e69:  stloc.s    V_6
    IL_0e6b:  ldloc.s    V_6
    IL_0e6d:  ldc.i4.s   1
    IL_0e6f:  sub
    IL_0e70:  dup
    IL_0e71:  stloc.s    V_6
    IL_0e73:  ldc.i4.s   0
    IL_0e75:  ble        IL_0e9f

    IL_0e7a:  ldloc.s    V_1
    IL_0e7c:  ldloc.s    V_0
    IL_0e7e:  dup
    IL_0e7f:  ldc.i4.s   1
    IL_0e81:  add
    IL_0e82:  stloc.s    V_0
    IL_0e84:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0e89:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_0e8e:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_0e93:  brtrue     IL_0e6b

    IL_0e98:  ldloc.s    V_0
    IL_0e9a:  ldc.i4.s   1
    IL_0e9c:  sub
    IL_0e9d:  stloc.s    V_0
    IL_0e9f:  ldloc.s    V_7
    IL_0ea1:  ldloc.s    V_6
    IL_0ea3:  ble.s      IL_0ed2

    IL_0ea5:  ldloc.s    V_3
    IL_0ea7:  ldloc.s    V_2
    IL_0ea9:  ldc.i4.1
    IL_0eaa:  sub
    IL_0eab:  dup
    IL_0eac:  stloc.s    V_2
    IL_0eae:  ldloc.s    V_7
    IL_0eb0:  ldloc.s    V_6
    IL_0eb2:  sub
    IL_0eb3:  ldc.i4.s   1
    IL_0eb5:  sub
    IL_0eb6:  stelem.i4
    IL_0eb7:  ldloc.s    V_3
    IL_0eb9:  ldloc.s    V_2
    IL_0ebb:  ldc.i4.1
    IL_0ebc:  sub
    IL_0ebd:  dup
    IL_0ebe:  stloc.s    V_2
    IL_0ec0:  ldloc.s    V_0
    IL_0ec2:  ldc.i4.s   1
    IL_0ec4:  sub
    IL_0ec5:  stelem.i4
    IL_0ec6:  ldloc.s    V_3
    IL_0ec8:  ldloc.s    V_2
    IL_0eca:  ldc.i4.1
    IL_0ecb:  sub
    IL_0ecc:  dup
    IL_0ecd:  stloc.s    V_2
    IL_0ecf:  ldc.i4.s   27
    IL_0ed1:  stelem.i4
    IL_0ed2:  ldarg.0
    IL_0ed3:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0ed8:  ldloc.s    V_0
    IL_0eda:  ldloc.s    V_10
    IL_0edc:  bge        IL_22d4

    IL_0ee1:  ldloc.s    V_1
    IL_0ee3:  ldloc.s    V_0
    IL_0ee5:  dup
    IL_0ee6:  ldc.i4.s   1
    IL_0ee8:  add
    IL_0ee9:  stloc.s    V_0
    IL_0eeb:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0ef0:  ldc.i4.s   93
    IL_0ef2:  bne.un     IL_22d4

    IL_0ef7:  ldarg.0
    IL_0ef8:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0efd:  ldloc.s    V_5
    IL_0eff:  ldloc.s    V_4
    IL_0f01:  dup
    IL_0f02:  ldc.i4.1
    IL_0f03:  add
    IL_0f04:  stloc.s    V_4
    IL_0f06:  ldelem.i4
    IL_0f07:  stloc.s    V_6
    IL_0f09:  ldarg.0
    IL_0f0a:  ldc.i4.s   8
    IL_0f0c:  ldloc.s    V_6
    IL_0f0e:  ldloc.s    V_0
    IL_0f10:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_0f15:  ldloc.s    V_3
    IL_0f17:  ldloc.s    V_2
    IL_0f19:  ldc.i4.1
    IL_0f1a:  sub
    IL_0f1b:  dup
    IL_0f1c:  stloc.s    V_2
    IL_0f1e:  ldloc.s    V_6
    IL_0f20:  stelem.i4
    IL_0f21:  ldloc.s    V_3
    IL_0f23:  ldloc.s    V_2
    IL_0f25:  ldc.i4.1
    IL_0f26:  sub
    IL_0f27:  dup
    IL_0f28:  stloc.s    V_2
    IL_0f2a:  ldc.i4.s   8
    IL_0f2c:  stelem.i4
    IL_0f2d:  ldarg.0
    IL_0f2e:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0f33:  ldloc.s    V_5
    IL_0f35:  ldloc.s    V_4
    IL_0f37:  dup
    IL_0f38:  ldc.i4.1
    IL_0f39:  add
    IL_0f3a:  stloc.s    V_4
    IL_0f3c:  ldelem.i4
    IL_0f3d:  stloc.s    V_6
    IL_0f3f:  ldloc.s    V_5
    IL_0f41:  ldloc.s    V_4
    IL_0f43:  dup
    IL_0f44:  ldc.i4.1
    IL_0f45:  add
    IL_0f46:  stloc.s    V_4
    IL_0f48:  ldelem.i4
    IL_0f49:  dup
    IL_0f4a:  stloc.s    V_7
    IL_0f4c:  ldloc.s    V_3
    IL_0f4e:  ldloc.s    V_2
    IL_0f50:  ldc.i4.1
    IL_0f51:  sub
    IL_0f52:  dup
    IL_0f53:  stloc.s    V_2
    IL_0f55:  ldloc.s    V_7
    IL_0f57:  stelem.i4
    IL_0f58:  ldloc.s    V_0
    IL_0f5a:  bne.un.s   IL_0f62

    IL_0f5c:  ldloc.s    V_6
    IL_0f5e:  ldc.i4.s   0
    IL_0f60:  bge.s      IL_0fb5

    IL_0f62:  ldloc.s    V_6
    IL_0f64:  ldc.i4.s   1
    IL_0f66:  bge.s      IL_0fb5

    IL_0f68:  ldloc.s    V_5
    IL_0f6a:  ldloc.s    V_4
    IL_0f6c:  ldc.i4.1
    IL_0f6d:  sub
    IL_0f6e:  dup
    IL_0f6f:  stloc.s    V_4
    IL_0f71:  ldloc.s    V_0
    IL_0f73:  stelem.i4
    IL_0f74:  ldloc.s    V_5
    IL_0f76:  ldloc.s    V_4
    IL_0f78:  ldc.i4.1
    IL_0f79:  sub
    IL_0f7a:  dup
    IL_0f7b:  stloc.s    V_4
    IL_0f7d:  ldloc.s    V_6
    IL_0f7f:  ldc.i4.s   1
    IL_0f81:  add
    IL_0f82:  stelem.i4
    IL_0f83:  ldloc.s    V_3
    IL_0f85:  ldloc.s    V_2
    IL_0f87:  ldc.i4.1
    IL_0f88:  sub
    IL_0f89:  dup
    IL_0f8a:  stloc.s    V_2
    IL_0f8c:  ldc.i4.s   28
    IL_0f8e:  stelem.i4
    IL_0f8f:  ldloc.s    V_2
    IL_0f91:  ldc.i4     0x1a0
    IL_0f96:  ble.s      IL_0fa4

    IL_0f98:  ldloc.s    V_4
    IL_0f9a:  ldc.i4     0x138
    IL_0f9f:  bgt        IL_0dce

    IL_0fa4:  ldloc.s    V_3
    IL_0fa6:  ldloc.s    V_2
    IL_0fa8:  ldc.i4.1
    IL_0fa9:  sub
    IL_0faa:  dup
    IL_0fab:  stloc.s    V_2
    IL_0fad:  ldc.i4.s   29
    IL_0faf:  stelem.i4
    IL_0fb0:  br         IL_22d4

    IL_0fb5:  ldloc.s    V_3
    IL_0fb7:  ldloc.s    V_2
    IL_0fb9:  ldc.i4.1
    IL_0fba:  sub
    IL_0fbb:  dup
    IL_0fbc:  stloc.s    V_2
    IL_0fbe:  ldloc.s    V_6
    IL_0fc0:  stelem.i4
    IL_0fc1:  ldloc.s    V_3
    IL_0fc3:  ldloc.s    V_2
    IL_0fc5:  ldc.i4.1
    IL_0fc6:  sub
    IL_0fc7:  dup
    IL_0fc8:  stloc.s    V_2
    IL_0fca:  ldc.i4.s   11
    IL_0fcc:  stelem.i4
    IL_0fcd:  ldarg.0
    IL_0fce:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0fd3:  ldloc.s    V_0
    IL_0fd5:  ldloc.s    V_10
    IL_0fd7:  bge        IL_22d4

    IL_0fdc:  ldloc.s    V_1
    IL_0fde:  ldloc.s    V_0
    IL_0fe0:  dup
    IL_0fe1:  ldc.i4.s   1
    IL_0fe3:  add
    IL_0fe4:  stloc.s    V_0
    IL_0fe6:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_0feb:  ldc.i4.s   58
    IL_0fed:  bne.un     IL_22d4

    IL_0ff2:  ldarg.0
    IL_0ff3:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_0ff8:  ldloc.s    V_5
    IL_0ffa:  ldloc.s    V_4
    IL_0ffc:  ldc.i4.1
    IL_0ffd:  sub
    IL_0ffe:  dup
    IL_0fff:  stloc.s    V_4
    IL_1001:  ldloc.s    V_0
    IL_1003:  stelem.i4
    IL_1004:  ldloc.s    V_3
    IL_1006:  ldloc.s    V_2
    IL_1008:  ldc.i4.1
    IL_1009:  sub
    IL_100a:  dup
    IL_100b:  stloc.s    V_2
    IL_100d:  ldc.i4.s   1
    IL_100f:  stelem.i4
    IL_1010:  ldarg.0
    IL_1011:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1016:  ldloc.s    V_3
    IL_1018:  ldloc.s    V_2
    IL_101a:  ldc.i4.1
    IL_101b:  sub
    IL_101c:  dup
    IL_101d:  stloc.s    V_2
    IL_101f:  ldloc.s    V_0
    IL_1021:  stelem.i4
    IL_1022:  ldloc.s    V_3
    IL_1024:  ldloc.s    V_2
    IL_1026:  ldc.i4.1
    IL_1027:  sub
    IL_1028:  dup
    IL_1029:  stloc.s    V_2
    IL_102b:  ldc.i4.s   30
    IL_102d:  stelem.i4
    IL_102e:  ldarg.0
    IL_102f:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1034:  ldc.i4.s   1
    IL_1036:  ldloc.s    V_10
    IL_1038:  ldloc.s    V_0
    IL_103a:  sub
    IL_103b:  bgt        IL_22d4

    IL_1040:  ldloc.s    V_0
    IL_1042:  ldc.i4.s   1
    IL_1044:  add
    IL_1045:  stloc.s    V_0
    IL_1047:  ldc.i4.s   1
    IL_1049:  stloc.s    V_6
    IL_104b:  ldloc.s    V_1
    IL_104d:  ldloc.s    V_0
    IL_104f:  ldloc.s    V_6
    IL_1051:  dup
    IL_1052:  ldc.i4.s   1
    IL_1054:  sub
    IL_1055:  stloc.s    V_6
    IL_1057:  sub
    IL_1058:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_105d:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 30 00 00 00 02 00 04 00   // ......-.0.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_1062:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_1067:  brfalse    IL_22d4

    IL_106c:  ldloc.s    V_6
    IL_106e:  ldc.i4.s   0
    IL_1070:  bgt        IL_104b

    IL_1075:  ldarg.0
    IL_1076:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_107b:  ldloc.s    V_10
    IL_107d:  ldloc.s    V_0
    IL_107f:  sub
    IL_1080:  dup
    IL_1081:  stloc.s    V_7
    IL_1083:  ldc.i4.s   1
    IL_1085:  add
    IL_1086:  stloc.s    V_6
    IL_1088:  ldloc.s    V_6
    IL_108a:  ldc.i4.s   1
    IL_108c:  sub
    IL_108d:  dup
    IL_108e:  stloc.s    V_6
    IL_1090:  ldc.i4.s   0
    IL_1092:  ble        IL_10bc

    IL_1097:  ldloc.s    V_1
    IL_1099:  ldloc.s    V_0
    IL_109b:  dup
    IL_109c:  ldc.i4.s   1
    IL_109e:  add
    IL_109f:  stloc.s    V_0
    IL_10a1:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_10a6:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 30 00 00 00 02 00 04 00   // ......-.0.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_10ab:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_10b0:  brtrue     IL_1088

    IL_10b5:  ldloc.s    V_0
    IL_10b7:  ldc.i4.s   1
    IL_10b9:  sub
    IL_10ba:  stloc.s    V_0
    IL_10bc:  ldloc.s    V_7
    IL_10be:  ldloc.s    V_6
    IL_10c0:  ble.s      IL_10ef

    IL_10c2:  ldloc.s    V_3
    IL_10c4:  ldloc.s    V_2
    IL_10c6:  ldc.i4.1
    IL_10c7:  sub
    IL_10c8:  dup
    IL_10c9:  stloc.s    V_2
    IL_10cb:  ldloc.s    V_7
    IL_10cd:  ldloc.s    V_6
    IL_10cf:  sub
    IL_10d0:  ldc.i4.s   1
    IL_10d2:  sub
    IL_10d3:  stelem.i4
    IL_10d4:  ldloc.s    V_3
    IL_10d6:  ldloc.s    V_2
    IL_10d8:  ldc.i4.1
    IL_10d9:  sub
    IL_10da:  dup
    IL_10db:  stloc.s    V_2
    IL_10dd:  ldloc.s    V_0
    IL_10df:  ldc.i4.s   1
    IL_10e1:  sub
    IL_10e2:  stelem.i4
    IL_10e3:  ldloc.s    V_3
    IL_10e5:  ldloc.s    V_2
    IL_10e7:  ldc.i4.1
    IL_10e8:  sub
    IL_10e9:  dup
    IL_10ea:  stloc.s    V_2
    IL_10ec:  ldc.i4.s   31
    IL_10ee:  stelem.i4
    IL_10ef:  ldarg.0
    IL_10f0:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_10f5:  ldloc.s    V_0
    IL_10f7:  ldloc.s    V_10
    IL_10f9:  bge        IL_22d4

    IL_10fe:  ldloc.s    V_1
    IL_1100:  ldloc.s    V_0
    IL_1102:  dup
    IL_1103:  ldc.i4.s   1
    IL_1105:  add
    IL_1106:  stloc.s    V_0
    IL_1108:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_110d:  ldc.i4.s   36
    IL_110f:  bne.un     IL_22d4

    IL_1114:  ldarg.0
    IL_1115:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_111a:  br         IL_1f94

    IL_111f:  ldarg.0
    IL_1120:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1125:  ldloc.s    V_3
    IL_1127:  ldloc.s    V_2
    IL_1129:  ldc.i4.1
    IL_112a:  sub
    IL_112b:  dup
    IL_112c:  stloc.s    V_2
    IL_112e:  ldloc.s    V_0
    IL_1130:  stelem.i4
    IL_1131:  ldloc.s    V_3
    IL_1133:  ldloc.s    V_2
    IL_1135:  ldc.i4.1
    IL_1136:  sub
    IL_1137:  dup
    IL_1138:  stloc.s    V_2
    IL_113a:  ldc.i4.s   32
    IL_113c:  stelem.i4
    IL_113d:  ldarg.0
    IL_113e:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1143:  ldc.i4.s   1
    IL_1145:  ldloc.s    V_10
    IL_1147:  ldloc.s    V_0
    IL_1149:  sub
    IL_114a:  bgt        IL_22d4

    IL_114f:  ldloc.s    V_0
    IL_1151:  ldc.i4.s   1
    IL_1153:  add
    IL_1154:  stloc.s    V_0
    IL_1156:  ldc.i4.s   1
    IL_1158:  stloc.s    V_6
    IL_115a:  ldloc.s    V_1
    IL_115c:  ldloc.s    V_0
    IL_115e:  ldloc.s    V_6
    IL_1160:  dup
    IL_1161:  ldc.i4.s   1
    IL_1163:  sub
    IL_1164:  stloc.s    V_6
    IL_1166:  sub
    IL_1167:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_116c:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_1171:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_1176:  brfalse    IL_22d4

    IL_117b:  ldloc.s    V_6
    IL_117d:  ldc.i4.s   0
    IL_117f:  bgt        IL_115a

    IL_1184:  ldarg.0
    IL_1185:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_118a:  ldloc.s    V_10
    IL_118c:  ldloc.s    V_0
    IL_118e:  sub
    IL_118f:  dup
    IL_1190:  stloc.s    V_7
    IL_1192:  ldc.i4.s   1
    IL_1194:  add
    IL_1195:  stloc.s    V_6
    IL_1197:  ldloc.s    V_6
    IL_1199:  ldc.i4.s   1
    IL_119b:  sub
    IL_119c:  dup
    IL_119d:  stloc.s    V_6
    IL_119f:  ldc.i4.s   0
    IL_11a1:  ble        IL_11cb

    IL_11a6:  ldloc.s    V_1
    IL_11a8:  ldloc.s    V_0
    IL_11aa:  dup
    IL_11ab:  ldc.i4.s   1
    IL_11ad:  add
    IL_11ae:  stloc.s    V_0
    IL_11b0:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_11b5:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_11ba:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_11bf:  brtrue     IL_1197

    IL_11c4:  ldloc.s    V_0
    IL_11c6:  ldc.i4.s   1
    IL_11c8:  sub
    IL_11c9:  stloc.s    V_0
    IL_11cb:  ldloc.s    V_7
    IL_11cd:  ldloc.s    V_6
    IL_11cf:  ble.s      IL_11fe

    IL_11d1:  ldloc.s    V_3
    IL_11d3:  ldloc.s    V_2
    IL_11d5:  ldc.i4.1
    IL_11d6:  sub
    IL_11d7:  dup
    IL_11d8:  stloc.s    V_2
    IL_11da:  ldloc.s    V_7
    IL_11dc:  ldloc.s    V_6
    IL_11de:  sub
    IL_11df:  ldc.i4.s   1
    IL_11e1:  sub
    IL_11e2:  stelem.i4
    IL_11e3:  ldloc.s    V_3
    IL_11e5:  ldloc.s    V_2
    IL_11e7:  ldc.i4.1
    IL_11e8:  sub
    IL_11e9:  dup
    IL_11ea:  stloc.s    V_2
    IL_11ec:  ldloc.s    V_0
    IL_11ee:  ldc.i4.s   1
    IL_11f0:  sub
    IL_11f1:  stelem.i4
    IL_11f2:  ldloc.s    V_3
    IL_11f4:  ldloc.s    V_2
    IL_11f6:  ldc.i4.1
    IL_11f7:  sub
    IL_11f8:  dup
    IL_11f9:  stloc.s    V_2
    IL_11fb:  ldc.i4.s   33
    IL_11fd:  stelem.i4
    IL_11fe:  ldarg.0
    IL_11ff:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1204:  ldc.i4.s   2
    IL_1206:  ldloc.s    V_10
    IL_1208:  ldloc.s    V_0
    IL_120a:  sub
    IL_120b:  bgt        IL_22d4

    IL_1210:  ldloc.s    V_1
    IL_1212:  ldloc.s    V_0
    IL_1214:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1219:  ldc.i4.s   58
    IL_121b:  bne.un     IL_22d4

    IL_1220:  ldloc.s    V_1
    IL_1222:  ldloc.s    V_0
    IL_1224:  ldc.i4.s   1
    IL_1226:  add
    IL_1227:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_122c:  ldc.i4.s   123
    IL_122e:  bne.un     IL_22d4

    IL_1233:  ldloc.s    V_0
    IL_1235:  ldc.i4.s   2
    IL_1237:  add
    IL_1238:  stloc.s    V_0
    IL_123a:  ldarg.0
    IL_123b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1240:  br         IL_1f94

    IL_1245:  ldarg.0
    IL_1246:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_124b:  ldc.i4.s   1
    IL_124d:  ldloc.s    V_10
    IL_124f:  ldloc.s    V_0
    IL_1251:  sub
    IL_1252:  bgt        IL_22d4

    IL_1257:  ldloc.s    V_0
    IL_1259:  ldc.i4.s   1
    IL_125b:  add
    IL_125c:  stloc.s    V_0
    IL_125e:  ldc.i4.s   1
    IL_1260:  stloc.s    V_6
    IL_1262:  ldloc.s    V_1
    IL_1264:  ldloc.s    V_0
    IL_1266:  ldloc.s    V_6
    IL_1268:  dup
    IL_1269:  ldc.i4.s   1
    IL_126b:  sub
    IL_126c:  stloc.s    V_6
    IL_126e:  sub
    IL_126f:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1274:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_1279:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_127e:  brfalse    IL_22d4

    IL_1283:  ldloc.s    V_6
    IL_1285:  ldc.i4.s   0
    IL_1287:  bgt        IL_1262

    IL_128c:  ldarg.0
    IL_128d:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1292:  ldloc.s    V_10
    IL_1294:  ldloc.s    V_0
    IL_1296:  sub
    IL_1297:  dup
    IL_1298:  stloc.s    V_7
    IL_129a:  ldc.i4.s   1
    IL_129c:  add
    IL_129d:  stloc.s    V_6
    IL_129f:  ldloc.s    V_6
    IL_12a1:  ldc.i4.s   1
    IL_12a3:  sub
    IL_12a4:  dup
    IL_12a5:  stloc.s    V_6
    IL_12a7:  ldc.i4.s   0
    IL_12a9:  ble        IL_12d3

    IL_12ae:  ldloc.s    V_1
    IL_12b0:  ldloc.s    V_0
    IL_12b2:  dup
    IL_12b3:  ldc.i4.s   1
    IL_12b5:  add
    IL_12b6:  stloc.s    V_0
    IL_12b8:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_12bd:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_12c2:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_12c7:  brtrue     IL_129f

    IL_12cc:  ldloc.s    V_0
    IL_12ce:  ldc.i4.s   1
    IL_12d0:  sub
    IL_12d1:  stloc.s    V_0
    IL_12d3:  ldloc.s    V_7
    IL_12d5:  ldloc.s    V_6
    IL_12d7:  ble.s      IL_1306

    IL_12d9:  ldloc.s    V_3
    IL_12db:  ldloc.s    V_2
    IL_12dd:  ldc.i4.1
    IL_12de:  sub
    IL_12df:  dup
    IL_12e0:  stloc.s    V_2
    IL_12e2:  ldloc.s    V_7
    IL_12e4:  ldloc.s    V_6
    IL_12e6:  sub
    IL_12e7:  ldc.i4.s   1
    IL_12e9:  sub
    IL_12ea:  stelem.i4
    IL_12eb:  ldloc.s    V_3
    IL_12ed:  ldloc.s    V_2
    IL_12ef:  ldc.i4.1
    IL_12f0:  sub
    IL_12f1:  dup
    IL_12f2:  stloc.s    V_2
    IL_12f4:  ldloc.s    V_0
    IL_12f6:  ldc.i4.s   1
    IL_12f8:  sub
    IL_12f9:  stelem.i4
    IL_12fa:  ldloc.s    V_3
    IL_12fc:  ldloc.s    V_2
    IL_12fe:  ldc.i4.1
    IL_12ff:  sub
    IL_1300:  dup
    IL_1301:  stloc.s    V_2
    IL_1303:  ldc.i4.s   34
    IL_1305:  stelem.i4
    IL_1306:  ldarg.0
    IL_1307:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_130c:  ldloc.s    V_0
    IL_130e:  ldloc.s    V_10
    IL_1310:  bge        IL_22d4

    IL_1315:  ldloc.s    V_1
    IL_1317:  ldloc.s    V_0
    IL_1319:  dup
    IL_131a:  ldc.i4.s   1
    IL_131c:  add
    IL_131d:  stloc.s    V_0
    IL_131f:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1324:  ldc.i4.s   63
    IL_1326:  bne.un     IL_22d4

    IL_132b:  ldarg.0
    IL_132c:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1331:  ldc.i4.s   1
    IL_1333:  ldloc.s    V_10
    IL_1335:  ldloc.s    V_0
    IL_1337:  sub
    IL_1338:  bgt        IL_22d4

    IL_133d:  ldloc.s    V_0
    IL_133f:  ldc.i4.s   1
    IL_1341:  add
    IL_1342:  stloc.s    V_0
    IL_1344:  ldc.i4.s   1
    IL_1346:  stloc.s    V_6
    IL_1348:  ldloc.s    V_1
    IL_134a:  ldloc.s    V_0
    IL_134c:  ldloc.s    V_6
    IL_134e:  dup
    IL_134f:  ldc.i4.s   1
    IL_1351:  sub
    IL_1352:  stloc.s    V_6
    IL_1354:  sub
    IL_1355:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_135a:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_135f:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_1364:  brfalse    IL_22d4

    IL_1369:  ldloc.s    V_6
    IL_136b:  ldc.i4.s   0
    IL_136d:  bgt        IL_1348

    IL_1372:  ldarg.0
    IL_1373:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1378:  ldloc.s    V_10
    IL_137a:  ldloc.s    V_0
    IL_137c:  sub
    IL_137d:  dup
    IL_137e:  stloc.s    V_7
    IL_1380:  ldc.i4.s   1
    IL_1382:  add
    IL_1383:  stloc.s    V_6
    IL_1385:  ldloc.s    V_6
    IL_1387:  ldc.i4.s   1
    IL_1389:  sub
    IL_138a:  dup
    IL_138b:  stloc.s    V_6
    IL_138d:  ldc.i4.s   0
    IL_138f:  ble        IL_13b9

    IL_1394:  ldloc.s    V_1
    IL_1396:  ldloc.s    V_0
    IL_1398:  dup
    IL_1399:  ldc.i4.s   1
    IL_139b:  add
    IL_139c:  stloc.s    V_0
    IL_139e:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_13a3:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_13a8:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_13ad:  brtrue     IL_1385

    IL_13b2:  ldloc.s    V_0
    IL_13b4:  ldc.i4.s   1
    IL_13b6:  sub
    IL_13b7:  stloc.s    V_0
    IL_13b9:  ldloc.s    V_7
    IL_13bb:  ldloc.s    V_6
    IL_13bd:  ble.s      IL_13ec

    IL_13bf:  ldloc.s    V_3
    IL_13c1:  ldloc.s    V_2
    IL_13c3:  ldc.i4.1
    IL_13c4:  sub
    IL_13c5:  dup
    IL_13c6:  stloc.s    V_2
    IL_13c8:  ldloc.s    V_7
    IL_13ca:  ldloc.s    V_6
    IL_13cc:  sub
    IL_13cd:  ldc.i4.s   1
    IL_13cf:  sub
    IL_13d0:  stelem.i4
    IL_13d1:  ldloc.s    V_3
    IL_13d3:  ldloc.s    V_2
    IL_13d5:  ldc.i4.1
    IL_13d6:  sub
    IL_13d7:  dup
    IL_13d8:  stloc.s    V_2
    IL_13da:  ldloc.s    V_0
    IL_13dc:  ldc.i4.s   1
    IL_13de:  sub
    IL_13df:  stelem.i4
    IL_13e0:  ldloc.s    V_3
    IL_13e2:  ldloc.s    V_2
    IL_13e4:  ldc.i4.1
    IL_13e5:  sub
    IL_13e6:  dup
    IL_13e7:  stloc.s    V_2
    IL_13e9:  ldc.i4.s   35
    IL_13eb:  stelem.i4
    IL_13ec:  ldarg.0
    IL_13ed:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_13f2:  ldloc.s    V_5
    IL_13f4:  ldloc.s    V_4
    IL_13f6:  ldc.i4.1
    IL_13f7:  sub
    IL_13f8:  dup
    IL_13f9:  stloc.s    V_4
    IL_13fb:  ldc.i4.s   -1
    IL_13fd:  stelem.i4
    IL_13fe:  ldloc.s    V_5
    IL_1400:  ldloc.s    V_4
    IL_1402:  ldc.i4.1
    IL_1403:  sub
    IL_1404:  dup
    IL_1405:  stloc.s    V_4
    IL_1407:  ldc.i4.s   0
    IL_1409:  stelem.i4
    IL_140a:  ldloc.s    V_3
    IL_140c:  ldloc.s    V_2
    IL_140e:  ldc.i4.1
    IL_140f:  sub
    IL_1410:  dup
    IL_1411:  stloc.s    V_2
    IL_1413:  ldc.i4.s   5
    IL_1415:  stelem.i4
    IL_1416:  ldarg.0
    IL_1417:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_141c:  br         IL_1ecf

    IL_1421:  ldarg.0
    IL_1422:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1427:  ldloc.s    V_5
    IL_1429:  ldloc.s    V_4
    IL_142b:  ldc.i4.1
    IL_142c:  sub
    IL_142d:  dup
    IL_142e:  stloc.s    V_4
    IL_1430:  ldloc.s    V_0
    IL_1432:  stelem.i4
    IL_1433:  ldloc.s    V_3
    IL_1435:  ldloc.s    V_2
    IL_1437:  ldc.i4.1
    IL_1438:  sub
    IL_1439:  dup
    IL_143a:  stloc.s    V_2
    IL_143c:  ldc.i4.s   1
    IL_143e:  stelem.i4
    IL_143f:  ldarg.0
    IL_1440:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1445:  ldloc.s    V_0
    IL_1447:  ldloc.s    V_10
    IL_1449:  bge        IL_22d4

    IL_144e:  ldloc.s    V_1
    IL_1450:  ldloc.s    V_0
    IL_1452:  dup
    IL_1453:  ldc.i4.s   1
    IL_1455:  add
    IL_1456:  stloc.s    V_0
    IL_1458:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_145d:  ldc.i4.s   44
    IL_145f:  bne.un     IL_22d4

    IL_1464:  ldarg.0
    IL_1465:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_146a:  ldloc.s    V_5
    IL_146c:  ldloc.s    V_4
    IL_146e:  ldc.i4.1
    IL_146f:  sub
    IL_1470:  dup
    IL_1471:  stloc.s    V_4
    IL_1473:  ldc.i4.s   -1
    IL_1475:  stelem.i4
    IL_1476:  ldloc.s    V_3
    IL_1478:  ldloc.s    V_2
    IL_147a:  ldc.i4.1
    IL_147b:  sub
    IL_147c:  dup
    IL_147d:  stloc.s    V_2
    IL_147f:  ldc.i4.s   1
    IL_1481:  stelem.i4
    IL_1482:  ldarg.0
    IL_1483:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1488:  br         IL_1e20

    IL_148d:  ldarg.0
    IL_148e:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1493:  ldloc.s    V_5
    IL_1495:  ldloc.s    V_4
    IL_1497:  ldc.i4.1
    IL_1498:  sub
    IL_1499:  dup
    IL_149a:  stloc.s    V_4
    IL_149c:  ldloc.s    V_0
    IL_149e:  stelem.i4
    IL_149f:  ldloc.s    V_3
    IL_14a1:  ldloc.s    V_2
    IL_14a3:  ldc.i4.1
    IL_14a4:  sub
    IL_14a5:  dup
    IL_14a6:  stloc.s    V_2
    IL_14a8:  ldc.i4.s   1
    IL_14aa:  stelem.i4
    IL_14ab:  ldarg.0
    IL_14ac:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_14b1:  ldloc.s    V_5
    IL_14b3:  ldloc.s    V_4
    IL_14b5:  ldc.i4.1
    IL_14b6:  sub
    IL_14b7:  dup
    IL_14b8:  stloc.s    V_4
    IL_14ba:  ldc.i4.s   -1
    IL_14bc:  stelem.i4
    IL_14bd:  ldloc.s    V_5
    IL_14bf:  ldloc.s    V_4
    IL_14c1:  ldc.i4.1
    IL_14c2:  sub
    IL_14c3:  dup
    IL_14c4:  stloc.s    V_4
    IL_14c6:  ldc.i4.s   0
    IL_14c8:  stelem.i4
    IL_14c9:  ldloc.s    V_3
    IL_14cb:  ldloc.s    V_2
    IL_14cd:  ldc.i4.1
    IL_14ce:  sub
    IL_14cf:  dup
    IL_14d0:  stloc.s    V_2
    IL_14d2:  ldc.i4.s   5
    IL_14d4:  stelem.i4
    IL_14d5:  ldarg.0
    IL_14d6:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_14db:  br         IL_1559

    IL_14e0:  ldarg.0
    IL_14e1:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_14e6:  ldloc.s    V_5
    IL_14e8:  ldloc.s    V_4
    IL_14ea:  ldc.i4.1
    IL_14eb:  sub
    IL_14ec:  dup
    IL_14ed:  stloc.s    V_4
    IL_14ef:  ldloc.s    V_0
    IL_14f1:  stelem.i4
    IL_14f2:  ldloc.s    V_3
    IL_14f4:  ldloc.s    V_2
    IL_14f6:  ldc.i4.1
    IL_14f7:  sub
    IL_14f8:  dup
    IL_14f9:  stloc.s    V_2
    IL_14fb:  ldc.i4.s   1
    IL_14fd:  stelem.i4
    IL_14fe:  ldarg.0
    IL_14ff:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1504:  ldloc.s    V_0
    IL_1506:  ldloc.s    V_10
    IL_1508:  bge        IL_22d4

    IL_150d:  ldloc.s    V_1
    IL_150f:  ldloc.s    V_0
    IL_1511:  dup
    IL_1512:  ldc.i4.s   1
    IL_1514:  add
    IL_1515:  stloc.s    V_0
    IL_1517:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_151c:  ldc.i4.s   124
    IL_151e:  bne.un     IL_22d4

    IL_1523:  ldarg.0
    IL_1524:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1529:  ldloc.s    V_5
    IL_152b:  ldloc.s    V_4
    IL_152d:  dup
    IL_152e:  ldc.i4.1
    IL_152f:  add
    IL_1530:  stloc.s    V_4
    IL_1532:  ldelem.i4
    IL_1533:  stloc.s    V_6
    IL_1535:  ldarg.0
    IL_1536:  ldc.i4.s   12
    IL_1538:  ldloc.s    V_6
    IL_153a:  ldloc.s    V_0
    IL_153c:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_1541:  ldloc.s    V_3
    IL_1543:  ldloc.s    V_2
    IL_1545:  ldc.i4.1
    IL_1546:  sub
    IL_1547:  dup
    IL_1548:  stloc.s    V_2
    IL_154a:  ldloc.s    V_6
    IL_154c:  stelem.i4
    IL_154d:  ldloc.s    V_3
    IL_154f:  ldloc.s    V_2
    IL_1551:  ldc.i4.1
    IL_1552:  sub
    IL_1553:  dup
    IL_1554:  stloc.s    V_2
    IL_1556:  ldc.i4.s   8
    IL_1558:  stelem.i4
    IL_1559:  ldarg.0
    IL_155a:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_155f:  ldloc.s    V_5
    IL_1561:  ldloc.s    V_4
    IL_1563:  dup
    IL_1564:  ldc.i4.1
    IL_1565:  add
    IL_1566:  stloc.s    V_4
    IL_1568:  ldelem.i4
    IL_1569:  stloc.s    V_6
    IL_156b:  ldloc.s    V_5
    IL_156d:  ldloc.s    V_4
    IL_156f:  dup
    IL_1570:  ldc.i4.1
    IL_1571:  add
    IL_1572:  stloc.s    V_4
    IL_1574:  ldelem.i4
    IL_1575:  dup
    IL_1576:  stloc.s    V_7
    IL_1578:  ldloc.s    V_3
    IL_157a:  ldloc.s    V_2
    IL_157c:  ldc.i4.1
    IL_157d:  sub
    IL_157e:  dup
    IL_157f:  stloc.s    V_2
    IL_1581:  ldloc.s    V_7
    IL_1583:  stelem.i4
    IL_1584:  ldloc.s    V_0
    IL_1586:  bne.un.s   IL_158e

    IL_1588:  ldloc.s    V_6
    IL_158a:  ldc.i4.s   0
    IL_158c:  bge.s      IL_15e1

    IL_158e:  ldloc.s    V_6
    IL_1590:  ldc.i4.s   1
    IL_1592:  bge.s      IL_15e1

    IL_1594:  ldloc.s    V_5
    IL_1596:  ldloc.s    V_4
    IL_1598:  ldc.i4.1
    IL_1599:  sub
    IL_159a:  dup
    IL_159b:  stloc.s    V_4
    IL_159d:  ldloc.s    V_0
    IL_159f:  stelem.i4
    IL_15a0:  ldloc.s    V_5
    IL_15a2:  ldloc.s    V_4
    IL_15a4:  ldc.i4.1
    IL_15a5:  sub
    IL_15a6:  dup
    IL_15a7:  stloc.s    V_4
    IL_15a9:  ldloc.s    V_6
    IL_15ab:  ldc.i4.s   1
    IL_15ad:  add
    IL_15ae:  stelem.i4
    IL_15af:  ldloc.s    V_3
    IL_15b1:  ldloc.s    V_2
    IL_15b3:  ldc.i4.1
    IL_15b4:  sub
    IL_15b5:  dup
    IL_15b6:  stloc.s    V_2
    IL_15b8:  ldc.i4.s   36
    IL_15ba:  stelem.i4
    IL_15bb:  ldloc.s    V_2
    IL_15bd:  ldc.i4     0x1a0
    IL_15c2:  ble.s      IL_15d0

    IL_15c4:  ldloc.s    V_4
    IL_15c6:  ldc.i4     0x138
    IL_15cb:  bgt        IL_14e0

    IL_15d0:  ldloc.s    V_3
    IL_15d2:  ldloc.s    V_2
    IL_15d4:  ldc.i4.1
    IL_15d5:  sub
    IL_15d6:  dup
    IL_15d7:  stloc.s    V_2
    IL_15d9:  ldc.i4.s   37
    IL_15db:  stelem.i4
    IL_15dc:  br         IL_22d4

    IL_15e1:  ldloc.s    V_3
    IL_15e3:  ldloc.s    V_2
    IL_15e5:  ldc.i4.1
    IL_15e6:  sub
    IL_15e7:  dup
    IL_15e8:  stloc.s    V_2
    IL_15ea:  ldloc.s    V_6
    IL_15ec:  stelem.i4
    IL_15ed:  ldloc.s    V_3
    IL_15ef:  ldloc.s    V_2
    IL_15f1:  ldc.i4.1
    IL_15f2:  sub
    IL_15f3:  dup
    IL_15f4:  stloc.s    V_2
    IL_15f6:  ldc.i4.s   11
    IL_15f8:  stelem.i4
    IL_15f9:  ldarg.0
    IL_15fa:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_15ff:  ldloc.s    V_5
    IL_1601:  ldloc.s    V_4
    IL_1603:  ldc.i4.1
    IL_1604:  sub
    IL_1605:  dup
    IL_1606:  stloc.s    V_4
    IL_1608:  ldc.i4.s   -1
    IL_160a:  stelem.i4
    IL_160b:  ldloc.s    V_5
    IL_160d:  ldloc.s    V_4
    IL_160f:  ldc.i4.1
    IL_1610:  sub
    IL_1611:  dup
    IL_1612:  stloc.s    V_4
    IL_1614:  ldc.i4.s   0
    IL_1616:  stelem.i4
    IL_1617:  ldloc.s    V_3
    IL_1619:  ldloc.s    V_2
    IL_161b:  ldc.i4.1
    IL_161c:  sub
    IL_161d:  dup
    IL_161e:  stloc.s    V_2
    IL_1620:  ldc.i4.s   5
    IL_1622:  stelem.i4
    IL_1623:  ldarg.0
    IL_1624:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1629:  br         IL_1d4a

    IL_162e:  ldarg.0
    IL_162f:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1634:  ldloc.s    V_5
    IL_1636:  ldloc.s    V_4
    IL_1638:  ldc.i4.1
    IL_1639:  sub
    IL_163a:  dup
    IL_163b:  stloc.s    V_4
    IL_163d:  ldloc.s    V_0
    IL_163f:  stelem.i4
    IL_1640:  ldloc.s    V_3
    IL_1642:  ldloc.s    V_2
    IL_1644:  ldc.i4.1
    IL_1645:  sub
    IL_1646:  dup
    IL_1647:  stloc.s    V_2
    IL_1649:  ldc.i4.s   1
    IL_164b:  stelem.i4
    IL_164c:  ldarg.0
    IL_164d:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1652:  ldloc.s    V_3
    IL_1654:  ldloc.s    V_2
    IL_1656:  ldc.i4.1
    IL_1657:  sub
    IL_1658:  dup
    IL_1659:  stloc.s    V_2
    IL_165b:  ldloc.s    V_0
    IL_165d:  stelem.i4
    IL_165e:  ldloc.s    V_3
    IL_1660:  ldloc.s    V_2
    IL_1662:  ldc.i4.1
    IL_1663:  sub
    IL_1664:  dup
    IL_1665:  stloc.s    V_2
    IL_1667:  ldc.i4.s   38
    IL_1669:  stelem.i4
    IL_166a:  ldarg.0
    IL_166b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1670:  ldloc.s    V_10
    IL_1672:  ldloc.s    V_0
    IL_1674:  sub
    IL_1675:  dup
    IL_1676:  stloc.s    V_7
    IL_1678:  ldc.i4.s   1
    IL_167a:  add
    IL_167b:  stloc.s    V_6
    IL_167d:  ldloc.s    V_6
    IL_167f:  ldc.i4.s   1
    IL_1681:  sub
    IL_1682:  dup
    IL_1683:  stloc.s    V_6
    IL_1685:  ldc.i4.s   0
    IL_1687:  ble        IL_16b1

    IL_168c:  ldloc.s    V_1
    IL_168e:  ldloc.s    V_0
    IL_1690:  dup
    IL_1691:  ldc.i4.s   1
    IL_1693:  add
    IL_1694:  stloc.s    V_0
    IL_1696:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_169b:  ldstr      bytearray (00 00 0A 00 00 00 23 00 24 00 2A 00 2C 00 2D 00   // ......#.$.*.,.-.
                                    2F 00 5E 00 5F 00 7E 00 7F 00 )                   // /.^._.~...
    IL_16a0:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_16a5:  brtrue     IL_167d

    IL_16aa:  ldloc.s    V_0
    IL_16ac:  ldc.i4.s   1
    IL_16ae:  sub
    IL_16af:  stloc.s    V_0
    IL_16b1:  ldloc.s    V_7
    IL_16b3:  ldloc.s    V_6
    IL_16b5:  ble.s      IL_16e4

    IL_16b7:  ldloc.s    V_3
    IL_16b9:  ldloc.s    V_2
    IL_16bb:  ldc.i4.1
    IL_16bc:  sub
    IL_16bd:  dup
    IL_16be:  stloc.s    V_2
    IL_16c0:  ldloc.s    V_7
    IL_16c2:  ldloc.s    V_6
    IL_16c4:  sub
    IL_16c5:  ldc.i4.s   1
    IL_16c7:  sub
    IL_16c8:  stelem.i4
    IL_16c9:  ldloc.s    V_3
    IL_16cb:  ldloc.s    V_2
    IL_16cd:  ldc.i4.1
    IL_16ce:  sub
    IL_16cf:  dup
    IL_16d0:  stloc.s    V_2
    IL_16d2:  ldloc.s    V_0
    IL_16d4:  ldc.i4.s   1
    IL_16d6:  sub
    IL_16d7:  stelem.i4
    IL_16d8:  ldloc.s    V_3
    IL_16da:  ldloc.s    V_2
    IL_16dc:  ldc.i4.1
    IL_16dd:  sub
    IL_16de:  dup
    IL_16df:  stloc.s    V_2
    IL_16e1:  ldc.i4.s   39
    IL_16e3:  stelem.i4
    IL_16e4:  ldarg.0
    IL_16e5:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_16ea:  ldloc.s    V_5
    IL_16ec:  ldloc.s    V_4
    IL_16ee:  ldc.i4.1
    IL_16ef:  sub
    IL_16f0:  dup
    IL_16f1:  stloc.s    V_4
    IL_16f3:  ldloc.s    V_0
    IL_16f5:  stelem.i4
    IL_16f6:  ldloc.s    V_3
    IL_16f8:  ldloc.s    V_2
    IL_16fa:  ldc.i4.1
    IL_16fb:  sub
    IL_16fc:  dup
    IL_16fd:  stloc.s    V_2
    IL_16ff:  ldc.i4.s   1
    IL_1701:  stelem.i4
    IL_1702:  ldarg.0
    IL_1703:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1708:  ldloc.s    V_0
    IL_170a:  ldloc.s    V_10
    IL_170c:  bge        IL_22d4

    IL_1711:  ldloc.s    V_1
    IL_1713:  ldloc.s    V_0
    IL_1715:  dup
    IL_1716:  ldc.i4.s   1
    IL_1718:  add
    IL_1719:  stloc.s    V_0
    IL_171b:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1720:  ldstr      bytearray (00 00 02 00 0A 00 2B 00 2C 00 00 00 02 00 04 00   // ......+.,.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_1725:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_172a:  brfalse    IL_22d4

    IL_172f:  ldarg.0
    IL_1730:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1735:  ldloc.s    V_10
    IL_1737:  ldloc.s    V_0
    IL_1739:  sub
    IL_173a:  dup
    IL_173b:  stloc.s    V_7
    IL_173d:  ldc.i4.s   1
    IL_173f:  add
    IL_1740:  stloc.s    V_6
    IL_1742:  ldloc.s    V_6
    IL_1744:  ldc.i4.s   1
    IL_1746:  sub
    IL_1747:  dup
    IL_1748:  stloc.s    V_6
    IL_174a:  ldc.i4.s   0
    IL_174c:  ble.s      IL_1768

    IL_174e:  ldloc.s    V_1
    IL_1750:  ldloc.s    V_0
    IL_1752:  dup
    IL_1753:  ldc.i4.s   1
    IL_1755:  add
    IL_1756:  stloc.s    V_0
    IL_1758:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_175d:  ldc.i4.s   36
    IL_175f:  bne.un.s   IL_1742

    IL_1761:  ldloc.s    V_0
    IL_1763:  ldc.i4.s   1
    IL_1765:  sub
    IL_1766:  stloc.s    V_0
    IL_1768:  ldloc.s    V_7
    IL_176a:  ldloc.s    V_6
    IL_176c:  ble.s      IL_179b

    IL_176e:  ldloc.s    V_3
    IL_1770:  ldloc.s    V_2
    IL_1772:  ldc.i4.1
    IL_1773:  sub
    IL_1774:  dup
    IL_1775:  stloc.s    V_2
    IL_1777:  ldloc.s    V_7
    IL_1779:  ldloc.s    V_6
    IL_177b:  sub
    IL_177c:  ldc.i4.s   1
    IL_177e:  sub
    IL_177f:  stelem.i4
    IL_1780:  ldloc.s    V_3
    IL_1782:  ldloc.s    V_2
    IL_1784:  ldc.i4.1
    IL_1785:  sub
    IL_1786:  dup
    IL_1787:  stloc.s    V_2
    IL_1789:  ldloc.s    V_0
    IL_178b:  ldc.i4.s   1
    IL_178d:  sub
    IL_178e:  stelem.i4
    IL_178f:  ldloc.s    V_3
    IL_1791:  ldloc.s    V_2
    IL_1793:  ldc.i4.1
    IL_1794:  sub
    IL_1795:  dup
    IL_1796:  stloc.s    V_2
    IL_1798:  ldc.i4.s   40
    IL_179a:  stelem.i4
    IL_179b:  ldarg.0
    IL_179c:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_17a1:  ldloc.s    V_5
    IL_17a3:  ldloc.s    V_4
    IL_17a5:  dup
    IL_17a6:  ldc.i4.1
    IL_17a7:  add
    IL_17a8:  stloc.s    V_4
    IL_17aa:  ldelem.i4
    IL_17ab:  stloc.s    V_6
    IL_17ad:  ldarg.0
    IL_17ae:  ldc.i4.s   14
    IL_17b0:  ldloc.s    V_6
    IL_17b2:  ldloc.s    V_0
    IL_17b4:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_17b9:  ldloc.s    V_3
    IL_17bb:  ldloc.s    V_2
    IL_17bd:  ldc.i4.1
    IL_17be:  sub
    IL_17bf:  dup
    IL_17c0:  stloc.s    V_2
    IL_17c2:  ldloc.s    V_6
    IL_17c4:  stelem.i4
    IL_17c5:  ldloc.s    V_3
    IL_17c7:  ldloc.s    V_2
    IL_17c9:  ldc.i4.1
    IL_17ca:  sub
    IL_17cb:  dup
    IL_17cc:  stloc.s    V_2
    IL_17ce:  ldc.i4.s   8
    IL_17d0:  stelem.i4
    IL_17d1:  ldarg.0
    IL_17d2:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_17d7:  br         IL_1d14

    IL_17dc:  ldarg.0
    IL_17dd:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_17e2:  ldloc.s    V_3
    IL_17e4:  ldloc.s    V_2
    IL_17e6:  ldc.i4.1
    IL_17e7:  sub
    IL_17e8:  dup
    IL_17e9:  stloc.s    V_2
    IL_17eb:  ldloc.s    V_0
    IL_17ed:  stelem.i4
    IL_17ee:  ldloc.s    V_3
    IL_17f0:  ldloc.s    V_2
    IL_17f2:  ldc.i4.1
    IL_17f3:  sub
    IL_17f4:  dup
    IL_17f5:  stloc.s    V_2
    IL_17f7:  ldc.i4.s   41
    IL_17f9:  stelem.i4
    IL_17fa:  ldarg.0
    IL_17fb:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1800:  ldloc.s    V_0
    IL_1802:  ldloc.s    V_10
    IL_1804:  bge        IL_22d4

    IL_1809:  ldloc.s    V_1
    IL_180b:  ldloc.s    V_0
    IL_180d:  dup
    IL_180e:  ldc.i4.s   1
    IL_1810:  add
    IL_1811:  stloc.s    V_0
    IL_1813:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1818:  ldc.i4.s   61
    IL_181a:  bne.un     IL_22d4

    IL_181f:  ldarg.0
    IL_1820:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1825:  ldloc.s    V_5
    IL_1827:  ldloc.s    V_4
    IL_1829:  ldc.i4.1
    IL_182a:  sub
    IL_182b:  dup
    IL_182c:  stloc.s    V_4
    IL_182e:  ldloc.s    V_0
    IL_1830:  stelem.i4
    IL_1831:  ldloc.s    V_3
    IL_1833:  ldloc.s    V_2
    IL_1835:  ldc.i4.1
    IL_1836:  sub
    IL_1837:  dup
    IL_1838:  stloc.s    V_2
    IL_183a:  ldc.i4.s   1
    IL_183c:  stelem.i4
    IL_183d:  ldarg.0
    IL_183e:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1843:  ldloc.s    V_0
    IL_1845:  ldloc.s    V_10
    IL_1847:  bge        IL_22d4

    IL_184c:  ldloc.s    V_1
    IL_184e:  ldloc.s    V_0
    IL_1850:  dup
    IL_1851:  ldc.i4.s   1
    IL_1853:  add
    IL_1854:  stloc.s    V_0
    IL_1856:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_185b:  ldstr      bytearray (00 00 02 00 01 00 2B 00 2C 00 9C FF )             // ......+.,...
    IL_1860:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_1865:  brfalse    IL_22d4

    IL_186a:  ldarg.0
    IL_186b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1870:  ldloc.s    V_10
    IL_1872:  ldloc.s    V_0
    IL_1874:  sub
    IL_1875:  dup
    IL_1876:  stloc.s    V_7
    IL_1878:  ldc.i4.s   1
    IL_187a:  add
    IL_187b:  stloc.s    V_6
    IL_187d:  ldloc.s    V_6
    IL_187f:  ldc.i4.s   1
    IL_1881:  sub
    IL_1882:  dup
    IL_1883:  stloc.s    V_6
    IL_1885:  ldc.i4.s   0
    IL_1887:  ble.s      IL_18a3

    IL_1889:  ldloc.s    V_1
    IL_188b:  ldloc.s    V_0
    IL_188d:  dup
    IL_188e:  ldc.i4.s   1
    IL_1890:  add
    IL_1891:  stloc.s    V_0
    IL_1893:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1898:  ldc.i4.s   36
    IL_189a:  bne.un.s   IL_187d

    IL_189c:  ldloc.s    V_0
    IL_189e:  ldc.i4.s   1
    IL_18a0:  sub
    IL_18a1:  stloc.s    V_0
    IL_18a3:  ldloc.s    V_7
    IL_18a5:  ldloc.s    V_6
    IL_18a7:  ble.s      IL_18d6

    IL_18a9:  ldloc.s    V_3
    IL_18ab:  ldloc.s    V_2
    IL_18ad:  ldc.i4.1
    IL_18ae:  sub
    IL_18af:  dup
    IL_18b0:  stloc.s    V_2
    IL_18b2:  ldloc.s    V_7
    IL_18b4:  ldloc.s    V_6
    IL_18b6:  sub
    IL_18b7:  ldc.i4.s   1
    IL_18b9:  sub
    IL_18ba:  stelem.i4
    IL_18bb:  ldloc.s    V_3
    IL_18bd:  ldloc.s    V_2
    IL_18bf:  ldc.i4.1
    IL_18c0:  sub
    IL_18c1:  dup
    IL_18c2:  stloc.s    V_2
    IL_18c4:  ldloc.s    V_0
    IL_18c6:  ldc.i4.s   1
    IL_18c8:  sub
    IL_18c9:  stelem.i4
    IL_18ca:  ldloc.s    V_3
    IL_18cc:  ldloc.s    V_2
    IL_18ce:  ldc.i4.1
    IL_18cf:  sub
    IL_18d0:  dup
    IL_18d1:  stloc.s    V_2
    IL_18d3:  ldc.i4.s   42
    IL_18d5:  stelem.i4
    IL_18d6:  ldarg.0
    IL_18d7:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_18dc:  ldloc.s    V_5
    IL_18de:  ldloc.s    V_4
    IL_18e0:  dup
    IL_18e1:  ldc.i4.1
    IL_18e2:  add
    IL_18e3:  stloc.s    V_4
    IL_18e5:  ldelem.i4
    IL_18e6:  stloc.s    V_6
    IL_18e8:  ldarg.0
    IL_18e9:  ldc.i4.s   15
    IL_18eb:  ldloc.s    V_6
    IL_18ed:  ldloc.s    V_0
    IL_18ef:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_18f4:  ldloc.s    V_3
    IL_18f6:  ldloc.s    V_2
    IL_18f8:  ldc.i4.1
    IL_18f9:  sub
    IL_18fa:  dup
    IL_18fb:  stloc.s    V_2
    IL_18fd:  ldloc.s    V_6
    IL_18ff:  stelem.i4
    IL_1900:  ldloc.s    V_3
    IL_1902:  ldloc.s    V_2
    IL_1904:  ldc.i4.1
    IL_1905:  sub
    IL_1906:  dup
    IL_1907:  stloc.s    V_2
    IL_1909:  ldc.i4.s   8
    IL_190b:  stelem.i4
    IL_190c:  ldarg.0
    IL_190d:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1912:  br         IL_1d14

    IL_1917:  ldarg.0
    IL_1918:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_191d:  ldloc.s    V_5
    IL_191f:  ldloc.s    V_4
    IL_1921:  ldc.i4.1
    IL_1922:  sub
    IL_1923:  dup
    IL_1924:  stloc.s    V_4
    IL_1926:  ldc.i4.s   -1
    IL_1928:  stelem.i4
    IL_1929:  ldloc.s    V_3
    IL_192b:  ldloc.s    V_2
    IL_192d:  ldc.i4.1
    IL_192e:  sub
    IL_192f:  dup
    IL_1930:  stloc.s    V_2
    IL_1932:  ldc.i4.s   1
    IL_1934:  stelem.i4
    IL_1935:  ldarg.0
    IL_1936:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_193b:  br         IL_1a7a

    IL_1940:  ldarg.0
    IL_1941:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1946:  ldloc.s    V_5
    IL_1948:  ldloc.s    V_4
    IL_194a:  ldc.i4.1
    IL_194b:  sub
    IL_194c:  dup
    IL_194d:  stloc.s    V_4
    IL_194f:  ldloc.s    V_0
    IL_1951:  stelem.i4
    IL_1952:  ldloc.s    V_3
    IL_1954:  ldloc.s    V_2
    IL_1956:  ldc.i4.1
    IL_1957:  sub
    IL_1958:  dup
    IL_1959:  stloc.s    V_2
    IL_195b:  ldc.i4.s   1
    IL_195d:  stelem.i4
    IL_195e:  ldarg.0
    IL_195f:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1964:  ldc.i4.s   1
    IL_1966:  ldloc.s    V_10
    IL_1968:  ldloc.s    V_0
    IL_196a:  sub
    IL_196b:  bgt        IL_22d4

    IL_1970:  ldloc.s    V_0
    IL_1972:  ldc.i4.s   1
    IL_1974:  add
    IL_1975:  stloc.s    V_0
    IL_1977:  ldc.i4.s   1
    IL_1979:  stloc.s    V_6
    IL_197b:  ldloc.s    V_1
    IL_197d:  ldloc.s    V_0
    IL_197f:  ldloc.s    V_6
    IL_1981:  dup
    IL_1982:  ldc.i4.s   1
    IL_1984:  sub
    IL_1985:  stloc.s    V_6
    IL_1987:  sub
    IL_1988:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_198d:  ldstr      bytearray (00 00 00 00 0A 00 00 00 02 00 04 00 05 00 03 00 
                                    01 00 06 00 09 00 13 00 00 00 ) 
    IL_1992:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_1997:  brfalse    IL_22d4

    IL_199c:  ldloc.s    V_6
    IL_199e:  ldc.i4.s   0
    IL_19a0:  bgt        IL_197b

    IL_19a5:  ldarg.0
    IL_19a6:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_19ab:  ldloc.s    V_10
    IL_19ad:  ldloc.s    V_0
    IL_19af:  sub
    IL_19b0:  dup
    IL_19b1:  stloc.s    V_7
    IL_19b3:  ldc.i4.s   1
    IL_19b5:  add
    IL_19b6:  stloc.s    V_6
    IL_19b8:  ldloc.s    V_6
    IL_19ba:  ldc.i4.s   1
    IL_19bc:  sub
    IL_19bd:  dup
    IL_19be:  stloc.s    V_6
    IL_19c0:  ldc.i4.s   0
    IL_19c2:  ble        IL_19ec

    IL_19c7:  ldloc.s    V_1
    IL_19c9:  ldloc.s    V_0
    IL_19cb:  dup
    IL_19cc:  ldc.i4.s   1
    IL_19ce:  add
    IL_19cf:  stloc.s    V_0
    IL_19d1:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_19d6:  ldstr      bytearray (00 00 00 00 0A 00 00 00 02 00 04 00 05 00 03 00 
                                    01 00 06 00 09 00 13 00 00 00 ) 
    IL_19db:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_19e0:  brtrue     IL_19b8

    IL_19e5:  ldloc.s    V_0
    IL_19e7:  ldc.i4.s   1
    IL_19e9:  sub
    IL_19ea:  stloc.s    V_0
    IL_19ec:  ldloc.s    V_7
    IL_19ee:  ldloc.s    V_6
    IL_19f0:  ble.s      IL_1a1f

    IL_19f2:  ldloc.s    V_3
    IL_19f4:  ldloc.s    V_2
    IL_19f6:  ldc.i4.1
    IL_19f7:  sub
    IL_19f8:  dup
    IL_19f9:  stloc.s    V_2
    IL_19fb:  ldloc.s    V_7
    IL_19fd:  ldloc.s    V_6
    IL_19ff:  sub
    IL_1a00:  ldc.i4.s   1
    IL_1a02:  sub
    IL_1a03:  stelem.i4
    IL_1a04:  ldloc.s    V_3
    IL_1a06:  ldloc.s    V_2
    IL_1a08:  ldc.i4.1
    IL_1a09:  sub
    IL_1a0a:  dup
    IL_1a0b:  stloc.s    V_2
    IL_1a0d:  ldloc.s    V_0
    IL_1a0f:  ldc.i4.s   1
    IL_1a11:  sub
    IL_1a12:  stelem.i4
    IL_1a13:  ldloc.s    V_3
    IL_1a15:  ldloc.s    V_2
    IL_1a17:  ldc.i4.1
    IL_1a18:  sub
    IL_1a19:  dup
    IL_1a1a:  stloc.s    V_2
    IL_1a1c:  ldc.i4.s   43
    IL_1a1e:  stelem.i4
    IL_1a1f:  ldarg.0
    IL_1a20:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1a25:  ldloc.s    V_0
    IL_1a27:  ldloc.s    V_10
    IL_1a29:  bge        IL_22d4

    IL_1a2e:  ldloc.s    V_1
    IL_1a30:  ldloc.s    V_0
    IL_1a32:  dup
    IL_1a33:  ldc.i4.s   1
    IL_1a35:  add
    IL_1a36:  stloc.s    V_0
    IL_1a38:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1a3d:  ldc.i4.s   46
    IL_1a3f:  bne.un     IL_22d4

    IL_1a44:  ldarg.0
    IL_1a45:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1a4a:  ldloc.s    V_5
    IL_1a4c:  ldloc.s    V_4
    IL_1a4e:  dup
    IL_1a4f:  ldc.i4.1
    IL_1a50:  add
    IL_1a51:  stloc.s    V_4
    IL_1a53:  ldelem.i4
    IL_1a54:  stloc.s    V_6
    IL_1a56:  ldarg.0
    IL_1a57:  ldc.i4.s   16
    IL_1a59:  ldloc.s    V_6
    IL_1a5b:  ldloc.s    V_0
    IL_1a5d:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_1a62:  ldloc.s    V_3
    IL_1a64:  ldloc.s    V_2
    IL_1a66:  ldc.i4.1
    IL_1a67:  sub
    IL_1a68:  dup
    IL_1a69:  stloc.s    V_2
    IL_1a6b:  ldloc.s    V_6
    IL_1a6d:  stelem.i4
    IL_1a6e:  ldloc.s    V_3
    IL_1a70:  ldloc.s    V_2
    IL_1a72:  ldc.i4.1
    IL_1a73:  sub
    IL_1a74:  dup
    IL_1a75:  stloc.s    V_2
    IL_1a77:  ldc.i4.s   8
    IL_1a79:  stelem.i4
    IL_1a7a:  ldarg.0
    IL_1a7b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1a80:  ldloc.s    V_5
    IL_1a82:  ldloc.s    V_4
    IL_1a84:  dup
    IL_1a85:  ldc.i4.1
    IL_1a86:  add
    IL_1a87:  stloc.s    V_4
    IL_1a89:  ldelem.i4
    IL_1a8a:  dup
    IL_1a8b:  stloc.s    V_6
    IL_1a8d:  ldloc.s    V_3
    IL_1a8f:  ldloc.s    V_2
    IL_1a91:  ldc.i4.1
    IL_1a92:  sub
    IL_1a93:  dup
    IL_1a94:  stloc.s    V_2
    IL_1a96:  ldloc.s    V_6
    IL_1a98:  stelem.i4
    IL_1a99:  ldloc.s    V_0
    IL_1a9b:  beq.s      IL_1ae7

    IL_1a9d:  ldloc.s    V_3
    IL_1a9f:  ldloc.s    V_2
    IL_1aa1:  ldc.i4.1
    IL_1aa2:  sub
    IL_1aa3:  dup
    IL_1aa4:  stloc.s    V_2
    IL_1aa6:  ldloc.s    V_0
    IL_1aa8:  stelem.i4
    IL_1aa9:  ldloc.s    V_5
    IL_1aab:  ldloc.s    V_4
    IL_1aad:  ldc.i4.1
    IL_1aae:  sub
    IL_1aaf:  dup
    IL_1ab0:  stloc.s    V_4
    IL_1ab2:  ldloc.s    V_0
    IL_1ab4:  stelem.i4
    IL_1ab5:  ldloc.s    V_3
    IL_1ab7:  ldloc.s    V_2
    IL_1ab9:  ldc.i4.1
    IL_1aba:  sub
    IL_1abb:  dup
    IL_1abc:  stloc.s    V_2
    IL_1abe:  ldc.i4.s   44
    IL_1ac0:  stelem.i4
    IL_1ac1:  ldloc.s    V_2
    IL_1ac3:  ldc.i4     0x1a0
    IL_1ac8:  ble.s      IL_1ad6

    IL_1aca:  ldloc.s    V_4
    IL_1acc:  ldc.i4     0x138
    IL_1ad1:  bgt        IL_1940

    IL_1ad6:  ldloc.s    V_3
    IL_1ad8:  ldloc.s    V_2
    IL_1ada:  ldc.i4.1
    IL_1adb:  sub
    IL_1adc:  dup
    IL_1add:  stloc.s    V_2
    IL_1adf:  ldc.i4.s   45
    IL_1ae1:  stelem.i4
    IL_1ae2:  br         IL_22d4

    IL_1ae7:  ldloc.s    V_3
    IL_1ae9:  ldloc.s    V_2
    IL_1aeb:  ldc.i4.1
    IL_1aec:  sub
    IL_1aed:  dup
    IL_1aee:  stloc.s    V_2
    IL_1af0:  ldc.i4.s   16
    IL_1af2:  stelem.i4
    IL_1af3:  ldarg.0
    IL_1af4:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1af9:  ldc.i4.s   1
    IL_1afb:  ldloc.s    V_10
    IL_1afd:  ldloc.s    V_0
    IL_1aff:  sub
    IL_1b00:  bgt        IL_22d4

    IL_1b05:  ldloc.s    V_0
    IL_1b07:  ldc.i4.s   1
    IL_1b09:  add
    IL_1b0a:  stloc.s    V_0
    IL_1b0c:  ldc.i4.s   1
    IL_1b0e:  stloc.s    V_6
    IL_1b10:  ldloc.s    V_1
    IL_1b12:  ldloc.s    V_0
    IL_1b14:  ldloc.s    V_6
    IL_1b16:  dup
    IL_1b17:  ldc.i4.s   1
    IL_1b19:  sub
    IL_1b1a:  stloc.s    V_6
    IL_1b1c:  sub
    IL_1b1d:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1b22:  ldstr      bytearray (00 00 00 00 0A 00 00 00 02 00 04 00 05 00 03 00 
                                    01 00 06 00 09 00 13 00 00 00 ) 
    IL_1b27:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_1b2c:  brfalse    IL_22d4

    IL_1b31:  ldloc.s    V_6
    IL_1b33:  ldc.i4.s   0
    IL_1b35:  bgt        IL_1b10

    IL_1b3a:  ldarg.0
    IL_1b3b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1b40:  ldloc.s    V_10
    IL_1b42:  ldloc.s    V_0
    IL_1b44:  sub
    IL_1b45:  dup
    IL_1b46:  stloc.s    V_7
    IL_1b48:  ldc.i4.s   1
    IL_1b4a:  add
    IL_1b4b:  stloc.s    V_6
    IL_1b4d:  ldloc.s    V_6
    IL_1b4f:  ldc.i4.s   1
    IL_1b51:  sub
    IL_1b52:  dup
    IL_1b53:  stloc.s    V_6
    IL_1b55:  ldc.i4.s   0
    IL_1b57:  ble        IL_1b81

    IL_1b5c:  ldloc.s    V_1
    IL_1b5e:  ldloc.s    V_0
    IL_1b60:  dup
    IL_1b61:  ldc.i4.s   1
    IL_1b63:  add
    IL_1b64:  stloc.s    V_0
    IL_1b66:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1b6b:  ldstr      bytearray (00 00 00 00 0A 00 00 00 02 00 04 00 05 00 03 00 
                                    01 00 06 00 09 00 13 00 00 00 ) 
    IL_1b70:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_1b75:  brtrue     IL_1b4d

    IL_1b7a:  ldloc.s    V_0
    IL_1b7c:  ldc.i4.s   1
    IL_1b7e:  sub
    IL_1b7f:  stloc.s    V_0
    IL_1b81:  ldloc.s    V_7
    IL_1b83:  ldloc.s    V_6
    IL_1b85:  ble.s      IL_1bb4

    IL_1b87:  ldloc.s    V_3
    IL_1b89:  ldloc.s    V_2
    IL_1b8b:  ldc.i4.1
    IL_1b8c:  sub
    IL_1b8d:  dup
    IL_1b8e:  stloc.s    V_2
    IL_1b90:  ldloc.s    V_7
    IL_1b92:  ldloc.s    V_6
    IL_1b94:  sub
    IL_1b95:  ldc.i4.s   1
    IL_1b97:  sub
    IL_1b98:  stelem.i4
    IL_1b99:  ldloc.s    V_3
    IL_1b9b:  ldloc.s    V_2
    IL_1b9d:  ldc.i4.1
    IL_1b9e:  sub
    IL_1b9f:  dup
    IL_1ba0:  stloc.s    V_2
    IL_1ba2:  ldloc.s    V_0
    IL_1ba4:  ldc.i4.s   1
    IL_1ba6:  sub
    IL_1ba7:  stelem.i4
    IL_1ba8:  ldloc.s    V_3
    IL_1baa:  ldloc.s    V_2
    IL_1bac:  ldc.i4.1
    IL_1bad:  sub
    IL_1bae:  dup
    IL_1baf:  stloc.s    V_2
    IL_1bb1:  ldc.i4.s   46
    IL_1bb3:  stelem.i4
    IL_1bb4:  ldarg.0
    IL_1bb5:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1bba:  ldloc.s    V_0
    IL_1bbc:  ldloc.s    V_10
    IL_1bbe:  bge        IL_22d4

    IL_1bc3:  ldloc.s    V_1
    IL_1bc5:  ldloc.s    V_0
    IL_1bc7:  dup
    IL_1bc8:  ldc.i4.s   1
    IL_1bca:  add
    IL_1bcb:  stloc.s    V_0
    IL_1bcd:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1bd2:  ldc.i4.s   64
    IL_1bd4:  bne.un     IL_22d4

    IL_1bd9:  ldarg.0
    IL_1bda:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1bdf:  ldloc.s    V_10
    IL_1be1:  ldloc.s    V_0
    IL_1be3:  sub
    IL_1be4:  dup
    IL_1be5:  stloc.s    V_7
    IL_1be7:  ldc.i4.s   1
    IL_1be9:  add
    IL_1bea:  stloc.s    V_6
    IL_1bec:  ldloc.s    V_6
    IL_1bee:  ldc.i4.s   1
    IL_1bf0:  sub
    IL_1bf1:  dup
    IL_1bf2:  stloc.s    V_6
    IL_1bf4:  ldc.i4.s   0
    IL_1bf6:  ble        IL_1c20

    IL_1bfb:  ldloc.s    V_1
    IL_1bfd:  ldloc.s    V_0
    IL_1bff:  dup
    IL_1c00:  ldc.i4.s   1
    IL_1c02:  add
    IL_1c03:  stloc.s    V_0
    IL_1c05:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1c0a:  ldstr      bytearray (00 00 04 00 00 00 23 00 24 00 2D 00 2E 00 )       // ......#.$.-...
    IL_1c0f:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_1c14:  brtrue     IL_1bec

    IL_1c19:  ldloc.s    V_0
    IL_1c1b:  ldc.i4.s   1
    IL_1c1d:  sub
    IL_1c1e:  stloc.s    V_0
    IL_1c20:  ldloc.s    V_7
    IL_1c22:  ldloc.s    V_6
    IL_1c24:  ble.s      IL_1c53

    IL_1c26:  ldloc.s    V_3
    IL_1c28:  ldloc.s    V_2
    IL_1c2a:  ldc.i4.1
    IL_1c2b:  sub
    IL_1c2c:  dup
    IL_1c2d:  stloc.s    V_2
    IL_1c2f:  ldloc.s    V_7
    IL_1c31:  ldloc.s    V_6
    IL_1c33:  sub
    IL_1c34:  ldc.i4.s   1
    IL_1c36:  sub
    IL_1c37:  stelem.i4
    IL_1c38:  ldloc.s    V_3
    IL_1c3a:  ldloc.s    V_2
    IL_1c3c:  ldc.i4.1
    IL_1c3d:  sub
    IL_1c3e:  dup
    IL_1c3f:  stloc.s    V_2
    IL_1c41:  ldloc.s    V_0
    IL_1c43:  ldc.i4.s   1
    IL_1c45:  sub
    IL_1c46:  stelem.i4
    IL_1c47:  ldloc.s    V_3
    IL_1c49:  ldloc.s    V_2
    IL_1c4b:  ldc.i4.1
    IL_1c4c:  sub
    IL_1c4d:  dup
    IL_1c4e:  stloc.s    V_2
    IL_1c50:  ldc.i4.s   47
    IL_1c52:  stelem.i4
    IL_1c53:  ldarg.0
    IL_1c54:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1c59:  ldc.i4.s   1
    IL_1c5b:  ldloc.s    V_10
    IL_1c5d:  ldloc.s    V_0
    IL_1c5f:  sub
    IL_1c60:  bgt        IL_22d4

    IL_1c65:  ldloc.s    V_0
    IL_1c67:  ldc.i4.s   1
    IL_1c69:  add
    IL_1c6a:  stloc.s    V_0
    IL_1c6c:  ldc.i4.s   1
    IL_1c6e:  stloc.s    V_6
    IL_1c70:  ldloc.s    V_1
    IL_1c72:  ldloc.s    V_0
    IL_1c74:  ldloc.s    V_6
    IL_1c76:  dup
    IL_1c77:  ldc.i4.s   1
    IL_1c79:  sub
    IL_1c7a:  stloc.s    V_6
    IL_1c7c:  sub
    IL_1c7d:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1c82:  ldstr      bytearray (00 00 04 00 0A 00 2B 00 2C 00 2E 00 2F 00 00 00   // ......+.,.../...
                                    02 00 04 00 05 00 03 00 01 00 06 00 09 00 13 00 
                                    00 00 ) 
    IL_1c87:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_1c8c:  brfalse    IL_22d4

    IL_1c91:  ldloc.s    V_6
    IL_1c93:  ldc.i4.s   0
    IL_1c95:  bgt        IL_1c70

    IL_1c9a:  ldarg.0
    IL_1c9b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1ca0:  ldloc.s    V_10
    IL_1ca2:  ldloc.s    V_0
    IL_1ca4:  sub
    IL_1ca5:  dup
    IL_1ca6:  stloc.s    V_7
    IL_1ca8:  ldc.i4.s   1
    IL_1caa:  add
    IL_1cab:  stloc.s    V_6
    IL_1cad:  ldloc.s    V_6
    IL_1caf:  ldc.i4.s   1
    IL_1cb1:  sub
    IL_1cb2:  dup
    IL_1cb3:  stloc.s    V_6
    IL_1cb5:  ldc.i4.s   0
    IL_1cb7:  ble        IL_1ce1

    IL_1cbc:  ldloc.s    V_1
    IL_1cbe:  ldloc.s    V_0
    IL_1cc0:  dup
    IL_1cc1:  ldc.i4.s   1
    IL_1cc3:  add
    IL_1cc4:  stloc.s    V_0
    IL_1cc6:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1ccb:  ldstr      bytearray (00 00 04 00 0A 00 2B 00 2C 00 2E 00 2F 00 00 00   // ......+.,.../...
                                    02 00 04 00 05 00 03 00 01 00 06 00 09 00 13 00 
                                    00 00 ) 
    IL_1cd0:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_1cd5:  brtrue     IL_1cad

    IL_1cda:  ldloc.s    V_0
    IL_1cdc:  ldc.i4.s   1
    IL_1cde:  sub
    IL_1cdf:  stloc.s    V_0
    IL_1ce1:  ldloc.s    V_7
    IL_1ce3:  ldloc.s    V_6
    IL_1ce5:  ble.s      IL_1d14

    IL_1ce7:  ldloc.s    V_3
    IL_1ce9:  ldloc.s    V_2
    IL_1ceb:  ldc.i4.1
    IL_1cec:  sub
    IL_1ced:  dup
    IL_1cee:  stloc.s    V_2
    IL_1cf0:  ldloc.s    V_7
    IL_1cf2:  ldloc.s    V_6
    IL_1cf4:  sub
    IL_1cf5:  ldc.i4.s   1
    IL_1cf7:  sub
    IL_1cf8:  stelem.i4
    IL_1cf9:  ldloc.s    V_3
    IL_1cfb:  ldloc.s    V_2
    IL_1cfd:  ldc.i4.1
    IL_1cfe:  sub
    IL_1cff:  dup
    IL_1d00:  stloc.s    V_2
    IL_1d02:  ldloc.s    V_0
    IL_1d04:  ldc.i4.s   1
    IL_1d06:  sub
    IL_1d07:  stelem.i4
    IL_1d08:  ldloc.s    V_3
    IL_1d0a:  ldloc.s    V_2
    IL_1d0c:  ldc.i4.1
    IL_1d0d:  sub
    IL_1d0e:  dup
    IL_1d0f:  stloc.s    V_2
    IL_1d11:  ldc.i4.s   48
    IL_1d13:  stelem.i4
    IL_1d14:  ldarg.0
    IL_1d15:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1d1a:  ldloc.s    V_5
    IL_1d1c:  ldloc.s    V_4
    IL_1d1e:  dup
    IL_1d1f:  ldc.i4.1
    IL_1d20:  add
    IL_1d21:  stloc.s    V_4
    IL_1d23:  ldelem.i4
    IL_1d24:  stloc.s    V_6
    IL_1d26:  ldarg.0
    IL_1d27:  ldc.i4.s   13
    IL_1d29:  ldloc.s    V_6
    IL_1d2b:  ldloc.s    V_0
    IL_1d2d:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_1d32:  ldloc.s    V_3
    IL_1d34:  ldloc.s    V_2
    IL_1d36:  ldc.i4.1
    IL_1d37:  sub
    IL_1d38:  dup
    IL_1d39:  stloc.s    V_2
    IL_1d3b:  ldloc.s    V_6
    IL_1d3d:  stelem.i4
    IL_1d3e:  ldloc.s    V_3
    IL_1d40:  ldloc.s    V_2
    IL_1d42:  ldc.i4.1
    IL_1d43:  sub
    IL_1d44:  dup
    IL_1d45:  stloc.s    V_2
    IL_1d47:  ldc.i4.s   8
    IL_1d49:  stelem.i4
    IL_1d4a:  ldarg.0
    IL_1d4b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1d50:  ldloc.s    V_5
    IL_1d52:  ldloc.s    V_4
    IL_1d54:  dup
    IL_1d55:  ldc.i4.1
    IL_1d56:  add
    IL_1d57:  stloc.s    V_4
    IL_1d59:  ldelem.i4
    IL_1d5a:  stloc.s    V_6
    IL_1d5c:  ldloc.s    V_5
    IL_1d5e:  ldloc.s    V_4
    IL_1d60:  dup
    IL_1d61:  ldc.i4.1
    IL_1d62:  add
    IL_1d63:  stloc.s    V_4
    IL_1d65:  ldelem.i4
    IL_1d66:  dup
    IL_1d67:  stloc.s    V_7
    IL_1d69:  ldloc.s    V_3
    IL_1d6b:  ldloc.s    V_2
    IL_1d6d:  ldc.i4.1
    IL_1d6e:  sub
    IL_1d6f:  dup
    IL_1d70:  stloc.s    V_2
    IL_1d72:  ldloc.s    V_7
    IL_1d74:  stelem.i4
    IL_1d75:  ldloc.s    V_0
    IL_1d77:  bne.un.s   IL_1d7f

    IL_1d79:  ldloc.s    V_6
    IL_1d7b:  ldc.i4.s   0
    IL_1d7d:  bge.s      IL_1dd2

    IL_1d7f:  ldloc.s    V_6
    IL_1d81:  ldc.i4.s   1
    IL_1d83:  bge.s      IL_1dd2

    IL_1d85:  ldloc.s    V_5
    IL_1d87:  ldloc.s    V_4
    IL_1d89:  ldc.i4.1
    IL_1d8a:  sub
    IL_1d8b:  dup
    IL_1d8c:  stloc.s    V_4
    IL_1d8e:  ldloc.s    V_0
    IL_1d90:  stelem.i4
    IL_1d91:  ldloc.s    V_5
    IL_1d93:  ldloc.s    V_4
    IL_1d95:  ldc.i4.1
    IL_1d96:  sub
    IL_1d97:  dup
    IL_1d98:  stloc.s    V_4
    IL_1d9a:  ldloc.s    V_6
    IL_1d9c:  ldc.i4.s   1
    IL_1d9e:  add
    IL_1d9f:  stelem.i4
    IL_1da0:  ldloc.s    V_3
    IL_1da2:  ldloc.s    V_2
    IL_1da4:  ldc.i4.1
    IL_1da5:  sub
    IL_1da6:  dup
    IL_1da7:  stloc.s    V_2
    IL_1da9:  ldc.i4.s   49
    IL_1dab:  stelem.i4
    IL_1dac:  ldloc.s    V_2
    IL_1dae:  ldc.i4     0x1a0
    IL_1db3:  ble.s      IL_1dc1

    IL_1db5:  ldloc.s    V_4
    IL_1db7:  ldc.i4     0x138
    IL_1dbc:  bgt        IL_162e

    IL_1dc1:  ldloc.s    V_3
    IL_1dc3:  ldloc.s    V_2
    IL_1dc5:  ldc.i4.1
    IL_1dc6:  sub
    IL_1dc7:  dup
    IL_1dc8:  stloc.s    V_2
    IL_1dca:  ldc.i4.s   50
    IL_1dcc:  stelem.i4
    IL_1dcd:  br         IL_22d4

    IL_1dd2:  ldloc.s    V_3
    IL_1dd4:  ldloc.s    V_2
    IL_1dd6:  ldc.i4.1
    IL_1dd7:  sub
    IL_1dd8:  dup
    IL_1dd9:  stloc.s    V_2
    IL_1ddb:  ldloc.s    V_6
    IL_1ddd:  stelem.i4
    IL_1dde:  ldloc.s    V_3
    IL_1de0:  ldloc.s    V_2
    IL_1de2:  ldc.i4.1
    IL_1de3:  sub
    IL_1de4:  dup
    IL_1de5:  stloc.s    V_2
    IL_1de7:  ldc.i4.s   11
    IL_1de9:  stelem.i4
    IL_1dea:  ldarg.0
    IL_1deb:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1df0:  ldloc.s    V_5
    IL_1df2:  ldloc.s    V_4
    IL_1df4:  dup
    IL_1df5:  ldc.i4.1
    IL_1df6:  add
    IL_1df7:  stloc.s    V_4
    IL_1df9:  ldelem.i4
    IL_1dfa:  stloc.s    V_6
    IL_1dfc:  ldarg.0
    IL_1dfd:  ldc.i4.s   11
    IL_1dff:  ldloc.s    V_6
    IL_1e01:  ldloc.s    V_0
    IL_1e03:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_1e08:  ldloc.s    V_3
    IL_1e0a:  ldloc.s    V_2
    IL_1e0c:  ldc.i4.1
    IL_1e0d:  sub
    IL_1e0e:  dup
    IL_1e0f:  stloc.s    V_2
    IL_1e11:  ldloc.s    V_6
    IL_1e13:  stelem.i4
    IL_1e14:  ldloc.s    V_3
    IL_1e16:  ldloc.s    V_2
    IL_1e18:  ldc.i4.1
    IL_1e19:  sub
    IL_1e1a:  dup
    IL_1e1b:  stloc.s    V_2
    IL_1e1d:  ldc.i4.s   8
    IL_1e1f:  stelem.i4
    IL_1e20:  ldarg.0
    IL_1e21:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1e26:  ldloc.s    V_5
    IL_1e28:  ldloc.s    V_4
    IL_1e2a:  dup
    IL_1e2b:  ldc.i4.1
    IL_1e2c:  add
    IL_1e2d:  stloc.s    V_4
    IL_1e2f:  ldelem.i4
    IL_1e30:  dup
    IL_1e31:  stloc.s    V_6
    IL_1e33:  ldloc.s    V_3
    IL_1e35:  ldloc.s    V_2
    IL_1e37:  ldc.i4.1
    IL_1e38:  sub
    IL_1e39:  dup
    IL_1e3a:  stloc.s    V_2
    IL_1e3c:  ldloc.s    V_6
    IL_1e3e:  stelem.i4
    IL_1e3f:  ldloc.s    V_0
    IL_1e41:  beq.s      IL_1e8d

    IL_1e43:  ldloc.s    V_3
    IL_1e45:  ldloc.s    V_2
    IL_1e47:  ldc.i4.1
    IL_1e48:  sub
    IL_1e49:  dup
    IL_1e4a:  stloc.s    V_2
    IL_1e4c:  ldloc.s    V_0
    IL_1e4e:  stelem.i4
    IL_1e4f:  ldloc.s    V_5
    IL_1e51:  ldloc.s    V_4
    IL_1e53:  ldc.i4.1
    IL_1e54:  sub
    IL_1e55:  dup
    IL_1e56:  stloc.s    V_4
    IL_1e58:  ldloc.s    V_0
    IL_1e5a:  stelem.i4
    IL_1e5b:  ldloc.s    V_3
    IL_1e5d:  ldloc.s    V_2
    IL_1e5f:  ldc.i4.1
    IL_1e60:  sub
    IL_1e61:  dup
    IL_1e62:  stloc.s    V_2
    IL_1e64:  ldc.i4.s   51
    IL_1e66:  stelem.i4
    IL_1e67:  ldloc.s    V_2
    IL_1e69:  ldc.i4     0x1a0
    IL_1e6e:  ble.s      IL_1e7c

    IL_1e70:  ldloc.s    V_4
    IL_1e72:  ldc.i4     0x138
    IL_1e77:  bgt        IL_148d

    IL_1e7c:  ldloc.s    V_3
    IL_1e7e:  ldloc.s    V_2
    IL_1e80:  ldc.i4.1
    IL_1e81:  sub
    IL_1e82:  dup
    IL_1e83:  stloc.s    V_2
    IL_1e85:  ldc.i4.s   52
    IL_1e87:  stelem.i4
    IL_1e88:  br         IL_22d4

    IL_1e8d:  ldloc.s    V_3
    IL_1e8f:  ldloc.s    V_2
    IL_1e91:  ldc.i4.1
    IL_1e92:  sub
    IL_1e93:  dup
    IL_1e94:  stloc.s    V_2
    IL_1e96:  ldc.i4.s   16
    IL_1e98:  stelem.i4
    IL_1e99:  ldarg.0
    IL_1e9a:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1e9f:  ldloc.s    V_5
    IL_1ea1:  ldloc.s    V_4
    IL_1ea3:  dup
    IL_1ea4:  ldc.i4.1
    IL_1ea5:  add
    IL_1ea6:  stloc.s    V_4
    IL_1ea8:  ldelem.i4
    IL_1ea9:  stloc.s    V_6
    IL_1eab:  ldarg.0
    IL_1eac:  ldc.i4.s   10
    IL_1eae:  ldloc.s    V_6
    IL_1eb0:  ldloc.s    V_0
    IL_1eb2:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_1eb7:  ldloc.s    V_3
    IL_1eb9:  ldloc.s    V_2
    IL_1ebb:  ldc.i4.1
    IL_1ebc:  sub
    IL_1ebd:  dup
    IL_1ebe:  stloc.s    V_2
    IL_1ec0:  ldloc.s    V_6
    IL_1ec2:  stelem.i4
    IL_1ec3:  ldloc.s    V_3
    IL_1ec5:  ldloc.s    V_2
    IL_1ec7:  ldc.i4.1
    IL_1ec8:  sub
    IL_1ec9:  dup
    IL_1eca:  stloc.s    V_2
    IL_1ecc:  ldc.i4.s   8
    IL_1ece:  stelem.i4
    IL_1ecf:  ldarg.0
    IL_1ed0:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1ed5:  ldloc.s    V_5
    IL_1ed7:  ldloc.s    V_4
    IL_1ed9:  dup
    IL_1eda:  ldc.i4.1
    IL_1edb:  add
    IL_1edc:  stloc.s    V_4
    IL_1ede:  ldelem.i4
    IL_1edf:  stloc.s    V_6
    IL_1ee1:  ldloc.s    V_5
    IL_1ee3:  ldloc.s    V_4
    IL_1ee5:  dup
    IL_1ee6:  ldc.i4.1
    IL_1ee7:  add
    IL_1ee8:  stloc.s    V_4
    IL_1eea:  ldelem.i4
    IL_1eeb:  dup
    IL_1eec:  stloc.s    V_7
    IL_1eee:  ldloc.s    V_3
    IL_1ef0:  ldloc.s    V_2
    IL_1ef2:  ldc.i4.1
    IL_1ef3:  sub
    IL_1ef4:  dup
    IL_1ef5:  stloc.s    V_2
    IL_1ef7:  ldloc.s    V_7
    IL_1ef9:  stelem.i4
    IL_1efa:  ldloc.s    V_0
    IL_1efc:  bne.un.s   IL_1f04

    IL_1efe:  ldloc.s    V_6
    IL_1f00:  ldc.i4.s   0
    IL_1f02:  bge.s      IL_1f57

    IL_1f04:  ldloc.s    V_6
    IL_1f06:  ldc.i4.s   1
    IL_1f08:  bge.s      IL_1f57

    IL_1f0a:  ldloc.s    V_5
    IL_1f0c:  ldloc.s    V_4
    IL_1f0e:  ldc.i4.1
    IL_1f0f:  sub
    IL_1f10:  dup
    IL_1f11:  stloc.s    V_4
    IL_1f13:  ldloc.s    V_0
    IL_1f15:  stelem.i4
    IL_1f16:  ldloc.s    V_5
    IL_1f18:  ldloc.s    V_4
    IL_1f1a:  ldc.i4.1
    IL_1f1b:  sub
    IL_1f1c:  dup
    IL_1f1d:  stloc.s    V_4
    IL_1f1f:  ldloc.s    V_6
    IL_1f21:  ldc.i4.s   1
    IL_1f23:  add
    IL_1f24:  stelem.i4
    IL_1f25:  ldloc.s    V_3
    IL_1f27:  ldloc.s    V_2
    IL_1f29:  ldc.i4.1
    IL_1f2a:  sub
    IL_1f2b:  dup
    IL_1f2c:  stloc.s    V_2
    IL_1f2e:  ldc.i4.s   53
    IL_1f30:  stelem.i4
    IL_1f31:  ldloc.s    V_2
    IL_1f33:  ldc.i4     0x1a0
    IL_1f38:  ble.s      IL_1f46

    IL_1f3a:  ldloc.s    V_4
    IL_1f3c:  ldc.i4     0x138
    IL_1f41:  bgt        IL_1421

    IL_1f46:  ldloc.s    V_3
    IL_1f48:  ldloc.s    V_2
    IL_1f4a:  ldc.i4.1
    IL_1f4b:  sub
    IL_1f4c:  dup
    IL_1f4d:  stloc.s    V_2
    IL_1f4f:  ldc.i4.s   54
    IL_1f51:  stelem.i4
    IL_1f52:  br         IL_22d4

    IL_1f57:  ldloc.s    V_3
    IL_1f59:  ldloc.s    V_2
    IL_1f5b:  ldc.i4.1
    IL_1f5c:  sub
    IL_1f5d:  dup
    IL_1f5e:  stloc.s    V_2
    IL_1f60:  ldloc.s    V_6
    IL_1f62:  stelem.i4
    IL_1f63:  ldloc.s    V_3
    IL_1f65:  ldloc.s    V_2
    IL_1f67:  ldc.i4.1
    IL_1f68:  sub
    IL_1f69:  dup
    IL_1f6a:  stloc.s    V_2
    IL_1f6c:  ldc.i4.s   11
    IL_1f6e:  stelem.i4
    IL_1f6f:  ldarg.0
    IL_1f70:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1f75:  ldloc.s    V_0
    IL_1f77:  ldloc.s    V_10
    IL_1f79:  bge        IL_22d4

    IL_1f7e:  ldloc.s    V_1
    IL_1f80:  ldloc.s    V_0
    IL_1f82:  dup
    IL_1f83:  ldc.i4.s   1
    IL_1f85:  add
    IL_1f86:  stloc.s    V_0
    IL_1f88:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_1f8d:  ldc.i4.s   36
    IL_1f8f:  bne.un     IL_22d4

    IL_1f94:  ldarg.0
    IL_1f95:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1f9a:  ldloc.s    V_5
    IL_1f9c:  ldloc.s    V_4
    IL_1f9e:  dup
    IL_1f9f:  ldc.i4.1
    IL_1fa0:  add
    IL_1fa1:  stloc.s    V_4
    IL_1fa3:  ldelem.i4
    IL_1fa4:  stloc.s    V_6
    IL_1fa6:  ldarg.0
    IL_1fa7:  ldc.i4.s   9
    IL_1fa9:  ldloc.s    V_6
    IL_1fab:  ldloc.s    V_0
    IL_1fad:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_1fb2:  ldloc.s    V_3
    IL_1fb4:  ldloc.s    V_2
    IL_1fb6:  ldc.i4.1
    IL_1fb7:  sub
    IL_1fb8:  dup
    IL_1fb9:  stloc.s    V_2
    IL_1fbb:  ldloc.s    V_6
    IL_1fbd:  stelem.i4
    IL_1fbe:  ldloc.s    V_3
    IL_1fc0:  ldloc.s    V_2
    IL_1fc2:  ldc.i4.1
    IL_1fc3:  sub
    IL_1fc4:  dup
    IL_1fc5:  stloc.s    V_2
    IL_1fc7:  ldc.i4.s   8
    IL_1fc9:  stelem.i4
    IL_1fca:  ldarg.0
    IL_1fcb:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_1fd0:  ldloc.s    V_5
    IL_1fd2:  ldloc.s    V_4
    IL_1fd4:  dup
    IL_1fd5:  ldc.i4.1
    IL_1fd6:  add
    IL_1fd7:  stloc.s    V_4
    IL_1fd9:  ldelem.i4
    IL_1fda:  stloc.s    V_6
    IL_1fdc:  ldarg.0
    IL_1fdd:  ldc.i4.s   1
    IL_1fdf:  ldloc.s    V_6
    IL_1fe1:  ldloc.s    V_0
    IL_1fe3:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_1fe8:  ldloc.s    V_3
    IL_1fea:  ldloc.s    V_2
    IL_1fec:  ldc.i4.1
    IL_1fed:  sub
    IL_1fee:  dup
    IL_1fef:  stloc.s    V_2
    IL_1ff1:  ldloc.s    V_6
    IL_1ff3:  stelem.i4
    IL_1ff4:  ldloc.s    V_3
    IL_1ff6:  ldloc.s    V_2
    IL_1ff8:  ldc.i4.1
    IL_1ff9:  sub
    IL_1ffa:  dup
    IL_1ffb:  stloc.s    V_2
    IL_1ffd:  ldc.i4.s   8
    IL_1fff:  stelem.i4
    IL_2000:  ldarg.0
    IL_2001:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2006:  br         IL_228f

    IL_200b:  ldarg.0
    IL_200c:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2011:  ldloc.s    V_3
    IL_2013:  ldloc.s    V_2
    IL_2015:  ldc.i4.1
    IL_2016:  sub
    IL_2017:  dup
    IL_2018:  stloc.s    V_2
    IL_201a:  ldloc.s    V_0
    IL_201c:  stelem.i4
    IL_201d:  ldloc.s    V_3
    IL_201f:  ldloc.s    V_2
    IL_2021:  ldc.i4.1
    IL_2022:  sub
    IL_2023:  dup
    IL_2024:  stloc.s    V_2
    IL_2026:  ldc.i4.s   55
    IL_2028:  stelem.i4
    IL_2029:  ldarg.0
    IL_202a:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_202f:  ldc.i4.s   2
    IL_2031:  ldloc.s    V_10
    IL_2033:  ldloc.s    V_0
    IL_2035:  sub
    IL_2036:  bgt        IL_22d4

    IL_203b:  ldloc.s    V_1
    IL_203d:  ldloc.s    V_0
    IL_203f:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_2044:  ldc.i4.s   125
    IL_2046:  bne.un     IL_22d4

    IL_204b:  ldloc.s    V_1
    IL_204d:  ldloc.s    V_0
    IL_204f:  ldc.i4.s   1
    IL_2051:  add
    IL_2052:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_2057:  ldc.i4.s   58
    IL_2059:  bne.un     IL_22d4

    IL_205e:  ldloc.s    V_0
    IL_2060:  ldc.i4.s   2
    IL_2062:  add
    IL_2063:  stloc.s    V_0
    IL_2065:  ldarg.0
    IL_2066:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_206b:  ldc.i4.s   1
    IL_206d:  ldloc.s    V_10
    IL_206f:  ldloc.s    V_0
    IL_2071:  sub
    IL_2072:  bgt        IL_22d4

    IL_2077:  ldloc.s    V_0
    IL_2079:  ldc.i4.s   1
    IL_207b:  add
    IL_207c:  stloc.s    V_0
    IL_207e:  ldc.i4.s   1
    IL_2080:  stloc.s    V_6
    IL_2082:  ldloc.s    V_1
    IL_2084:  ldloc.s    V_0
    IL_2086:  ldloc.s    V_6
    IL_2088:  dup
    IL_2089:  ldc.i4.s   1
    IL_208b:  sub
    IL_208c:  stloc.s    V_6
    IL_208e:  sub
    IL_208f:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_2094:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_2099:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_209e:  brfalse    IL_22d4

    IL_20a3:  ldloc.s    V_6
    IL_20a5:  ldc.i4.s   0
    IL_20a7:  bgt        IL_2082

    IL_20ac:  ldarg.0
    IL_20ad:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_20b2:  ldloc.s    V_10
    IL_20b4:  ldloc.s    V_0
    IL_20b6:  sub
    IL_20b7:  dup
    IL_20b8:  stloc.s    V_7
    IL_20ba:  ldc.i4.s   1
    IL_20bc:  add
    IL_20bd:  stloc.s    V_6
    IL_20bf:  ldloc.s    V_6
    IL_20c1:  ldc.i4.s   1
    IL_20c3:  sub
    IL_20c4:  dup
    IL_20c5:  stloc.s    V_6
    IL_20c7:  ldc.i4.s   0
    IL_20c9:  ble        IL_20f3

    IL_20ce:  ldloc.s    V_1
    IL_20d0:  ldloc.s    V_0
    IL_20d2:  dup
    IL_20d3:  ldc.i4.s   1
    IL_20d5:  add
    IL_20d6:  stloc.s    V_0
    IL_20d8:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_20dd:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_20e2:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_20e7:  brtrue     IL_20bf

    IL_20ec:  ldloc.s    V_0
    IL_20ee:  ldc.i4.s   1
    IL_20f0:  sub
    IL_20f1:  stloc.s    V_0
    IL_20f3:  ldloc.s    V_7
    IL_20f5:  ldloc.s    V_6
    IL_20f7:  ble.s      IL_2126

    IL_20f9:  ldloc.s    V_3
    IL_20fb:  ldloc.s    V_2
    IL_20fd:  ldc.i4.1
    IL_20fe:  sub
    IL_20ff:  dup
    IL_2100:  stloc.s    V_2
    IL_2102:  ldloc.s    V_7
    IL_2104:  ldloc.s    V_6
    IL_2106:  sub
    IL_2107:  ldc.i4.s   1
    IL_2109:  sub
    IL_210a:  stelem.i4
    IL_210b:  ldloc.s    V_3
    IL_210d:  ldloc.s    V_2
    IL_210f:  ldc.i4.1
    IL_2110:  sub
    IL_2111:  dup
    IL_2112:  stloc.s    V_2
    IL_2114:  ldloc.s    V_0
    IL_2116:  ldc.i4.s   1
    IL_2118:  sub
    IL_2119:  stelem.i4
    IL_211a:  ldloc.s    V_3
    IL_211c:  ldloc.s    V_2
    IL_211e:  ldc.i4.1
    IL_211f:  sub
    IL_2120:  dup
    IL_2121:  stloc.s    V_2
    IL_2123:  ldc.i4.s   56
    IL_2125:  stelem.i4
    IL_2126:  ldarg.0
    IL_2127:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_212c:  ldc.i4.s   2
    IL_212e:  ldloc.s    V_10
    IL_2130:  ldloc.s    V_0
    IL_2132:  sub
    IL_2133:  bgt        IL_22d4

    IL_2138:  ldloc.s    V_1
    IL_213a:  ldloc.s    V_0
    IL_213c:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_2141:  ldc.i4.s   58
    IL_2143:  bne.un     IL_22d4

    IL_2148:  ldloc.s    V_1
    IL_214a:  ldloc.s    V_0
    IL_214c:  ldc.i4.s   1
    IL_214e:  add
    IL_214f:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_2154:  ldc.i4.s   123
    IL_2156:  bne.un     IL_22d4

    IL_215b:  ldloc.s    V_0
    IL_215d:  ldc.i4.s   2
    IL_215f:  add
    IL_2160:  stloc.s    V_0
    IL_2162:  ldarg.0
    IL_2163:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2168:  br         IL_228f

    IL_216d:  ldarg.0
    IL_216e:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2173:  ldc.i4.s   2
    IL_2175:  ldloc.s    V_10
    IL_2177:  ldloc.s    V_0
    IL_2179:  sub
    IL_217a:  bgt        IL_22d4

    IL_217f:  ldloc.s    V_1
    IL_2181:  ldloc.s    V_0
    IL_2183:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_2188:  ldc.i4.s   125
    IL_218a:  bne.un     IL_22d4

    IL_218f:  ldloc.s    V_1
    IL_2191:  ldloc.s    V_0
    IL_2193:  ldc.i4.s   1
    IL_2195:  add
    IL_2196:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_219b:  ldc.i4.s   58
    IL_219d:  bne.un     IL_22d4

    IL_21a2:  ldloc.s    V_0
    IL_21a4:  ldc.i4.s   2
    IL_21a6:  add
    IL_21a7:  stloc.s    V_0
    IL_21a9:  ldarg.0
    IL_21aa:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_21af:  ldc.i4.s   1
    IL_21b1:  ldloc.s    V_10
    IL_21b3:  ldloc.s    V_0
    IL_21b5:  sub
    IL_21b6:  bgt        IL_22d4

    IL_21bb:  ldloc.s    V_0
    IL_21bd:  ldc.i4.s   1
    IL_21bf:  add
    IL_21c0:  stloc.s    V_0
    IL_21c2:  ldc.i4.s   1
    IL_21c4:  stloc.s    V_6
    IL_21c6:  ldloc.s    V_1
    IL_21c8:  ldloc.s    V_0
    IL_21ca:  ldloc.s    V_6
    IL_21cc:  dup
    IL_21cd:  ldc.i4.s   1
    IL_21cf:  sub
    IL_21d0:  stloc.s    V_6
    IL_21d2:  sub
    IL_21d3:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_21d8:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_21dd:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_21e2:  brfalse    IL_22d4

    IL_21e7:  ldloc.s    V_6
    IL_21e9:  ldc.i4.s   0
    IL_21eb:  bgt        IL_21c6

    IL_21f0:  ldarg.0
    IL_21f1:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_21f6:  ldloc.s    V_10
    IL_21f8:  ldloc.s    V_0
    IL_21fa:  sub
    IL_21fb:  dup
    IL_21fc:  stloc.s    V_7
    IL_21fe:  ldc.i4.s   1
    IL_2200:  add
    IL_2201:  stloc.s    V_6
    IL_2203:  ldloc.s    V_6
    IL_2205:  ldc.i4.s   1
    IL_2207:  sub
    IL_2208:  dup
    IL_2209:  stloc.s    V_6
    IL_220b:  ldc.i4.s   0
    IL_220d:  ble        IL_2237

    IL_2212:  ldloc.s    V_1
    IL_2214:  ldloc.s    V_0
    IL_2216:  dup
    IL_2217:  ldc.i4.s   1
    IL_2219:  add
    IL_221a:  stloc.s    V_0
    IL_221c:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_2221:  ldstr      bytearray (00 00 02 00 0A 00 2D 00 2F 00 00 00 02 00 04 00   // ......-./.......
                                    05 00 03 00 01 00 06 00 09 00 13 00 00 00 ) 
    IL_2226:  call       bool [System]System.Text.RegularExpressions.RegexRunner::CharInClass(char,
                                                                                              string)
    IL_222b:  brtrue     IL_2203

    IL_2230:  ldloc.s    V_0
    IL_2232:  ldc.i4.s   1
    IL_2234:  sub
    IL_2235:  stloc.s    V_0
    IL_2237:  ldloc.s    V_7
    IL_2239:  ldloc.s    V_6
    IL_223b:  ble.s      IL_226a

    IL_223d:  ldloc.s    V_3
    IL_223f:  ldloc.s    V_2
    IL_2241:  ldc.i4.1
    IL_2242:  sub
    IL_2243:  dup
    IL_2244:  stloc.s    V_2
    IL_2246:  ldloc.s    V_7
    IL_2248:  ldloc.s    V_6
    IL_224a:  sub
    IL_224b:  ldc.i4.s   1
    IL_224d:  sub
    IL_224e:  stelem.i4
    IL_224f:  ldloc.s    V_3
    IL_2251:  ldloc.s    V_2
    IL_2253:  ldc.i4.1
    IL_2254:  sub
    IL_2255:  dup
    IL_2256:  stloc.s    V_2
    IL_2258:  ldloc.s    V_0
    IL_225a:  ldc.i4.s   1
    IL_225c:  sub
    IL_225d:  stelem.i4
    IL_225e:  ldloc.s    V_3
    IL_2260:  ldloc.s    V_2
    IL_2262:  ldc.i4.1
    IL_2263:  sub
    IL_2264:  dup
    IL_2265:  stloc.s    V_2
    IL_2267:  ldc.i4.s   57
    IL_2269:  stelem.i4
    IL_226a:  ldarg.0
    IL_226b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2270:  ldloc.s    V_0
    IL_2272:  ldloc.s    V_10
    IL_2274:  bge        IL_22d4

    IL_2279:  ldloc.s    V_1
    IL_227b:  ldloc.s    V_0
    IL_227d:  dup
    IL_227e:  ldc.i4.s   1
    IL_2280:  add
    IL_2281:  stloc.s    V_0
    IL_2283:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
    IL_2288:  ldc.i4.s   36
    IL_228a:  bne.un     IL_22d4

    IL_228f:  ldarg.0
    IL_2290:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2295:  ldloc.s    V_5
    IL_2297:  ldloc.s    V_4
    IL_2299:  dup
    IL_229a:  ldc.i4.1
    IL_229b:  add
    IL_229c:  stloc.s    V_4
    IL_229e:  ldelem.i4
    IL_229f:  stloc.s    V_6
    IL_22a1:  ldarg.0
    IL_22a2:  ldc.i4.s   0
    IL_22a4:  ldloc.s    V_6
    IL_22a6:  ldloc.s    V_0
    IL_22a8:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Capture(int32,
                                                                                                   int32,
                                                                                                   int32)
    IL_22ad:  ldloc.s    V_3
    IL_22af:  ldloc.s    V_2
    IL_22b1:  ldc.i4.1
    IL_22b2:  sub
    IL_22b3:  dup
    IL_22b4:  stloc.s    V_2
    IL_22b6:  ldloc.s    V_6
    IL_22b8:  stelem.i4
    IL_22b9:  ldloc.s    V_3
    IL_22bb:  ldloc.s    V_2
    IL_22bd:  ldc.i4.1
    IL_22be:  sub
    IL_22bf:  dup
    IL_22c0:  stloc.s    V_2
    IL_22c2:  ldc.i4.s   8
    IL_22c4:  stelem.i4
    IL_22c5:  ldarg.0
    IL_22c6:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_22cb:  ldarg.0
    IL_22cc:  ldloc.s    V_0
    IL_22ce:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtextpos
    IL_22d3:  ret

    IL_22d4:  ldarg.0
    IL_22d5:  ldloc.s    V_2
    IL_22d7:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtrackpos
    IL_22dc:  ldarg.0
    IL_22dd:  ldloc.s    V_4
    IL_22df:  stfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runstackpos
    IL_22e4:  ldarg.0
    IL_22e5:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::EnsureStorage()
    IL_22ea:  ldarg.0
    IL_22eb:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runtrackpos
    IL_22f0:  stloc.s    V_2
    IL_22f2:  ldarg.0
    IL_22f3:  ldfld      int32 [System]System.Text.RegularExpressions.RegexRunner::runstackpos
    IL_22f8:  stloc.s    V_4
    IL_22fa:  ldarg.0
    IL_22fb:  ldfld      int32[] [System]System.Text.RegularExpressions.RegexRunner::runtrack
    IL_2300:  stloc.s    V_3
    IL_2302:  ldarg.0
    IL_2303:  ldfld      int32[] [System]System.Text.RegularExpressions.RegexRunner::runstack
    IL_2308:  stloc.s    V_5
    IL_230a:  ldloc.s    V_3
    IL_230c:  ldloc.s    V_2
    IL_230e:  dup
    IL_230f:  ldc.i4.1
    IL_2310:  add
    IL_2311:  stloc.s    V_2
    IL_2313:  ldelem.i4
    IL_2314:  switch     ( 
                          IL_2401,
                          IL_2418,
                          IL_242a,
                          IL_2441,
                          IL_2458,
                          IL_246f,
                          IL_2481,
                          IL_2498,
                          IL_24ee,
                          IL_2513,
                          IL_0174,
                          IL_2576,
                          IL_25ad,
                          IL_2603,
                          IL_2659,
                          IL_04af,
                          IL_2687,
                          IL_26a6,
                          IL_26fc,
                          IL_2752,
                          IL_27a8,
                          IL_27bf,
                          IL_2815,
                          IL_286b,
                          IL_0baa,
                          IL_28ce,
                          IL_0a71,
                          IL_2931,
                          IL_2987,
                          IL_0dce,
                          IL_29ea,
                          IL_2a01,
                          IL_2a57,
                          IL_2a6e,
                          IL_2ac4,
                          IL_2b1a,
                          IL_2b70,
                          IL_14e0,
                          IL_2bd3,
                          IL_2bea,
                          IL_2c40,
                          IL_2c96,
                          IL_2cad,
                          IL_2d03,
                          IL_2d59,
                          IL_1940,
                          IL_2d87,
                          IL_2ddd,
                          IL_2e33,
                          IL_2e89,
                          IL_162e,
                          IL_2eec,
                          IL_148d,
                          IL_2f1a,
                          IL_1421,
                          IL_2f7d,
                          IL_2f94,
                          IL_2fea)
    IL_2401:  ldarg.0
    IL_2402:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2407:  ldloc.s    V_3
    IL_2409:  ldloc.s    V_2
    IL_240b:  dup
    IL_240c:  ldc.i4.1
    IL_240d:  add
    IL_240e:  stloc.s    V_2
    IL_2410:  ldelem.i4
    IL_2411:  stloc.s    V_0
    IL_2413:  br         IL_22c5

    IL_2418:  ldarg.0
    IL_2419:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_241e:  ldloc.s    V_4
    IL_2420:  ldc.i4.s   1
    IL_2422:  add
    IL_2423:  stloc.s    V_4
    IL_2425:  br         IL_22d4

    IL_242a:  ldarg.0
    IL_242b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2430:  ldloc.s    V_3
    IL_2432:  ldloc.s    V_2
    IL_2434:  dup
    IL_2435:  ldc.i4.1
    IL_2436:  add
    IL_2437:  stloc.s    V_2
    IL_2439:  ldelem.i4
    IL_243a:  stloc.s    V_0
    IL_243c:  br         IL_200b

    IL_2441:  ldarg.0
    IL_2442:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2447:  ldloc.s    V_3
    IL_2449:  ldloc.s    V_2
    IL_244b:  dup
    IL_244c:  ldc.i4.1
    IL_244d:  add
    IL_244e:  stloc.s    V_2
    IL_2450:  ldelem.i4
    IL_2451:  stloc.s    V_0
    IL_2453:  br         IL_08e9

    IL_2458:  ldarg.0
    IL_2459:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_245e:  ldloc.s    V_3
    IL_2460:  ldloc.s    V_2
    IL_2462:  dup
    IL_2463:  ldc.i4.1
    IL_2464:  add
    IL_2465:  stloc.s    V_2
    IL_2467:  ldelem.i4
    IL_2468:  stloc.s    V_0
    IL_246a:  br         IL_0486

    IL_246f:  ldarg.0
    IL_2470:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2475:  ldloc.s    V_4
    IL_2477:  ldc.i4.s   2
    IL_2479:  add
    IL_247a:  stloc.s    V_4
    IL_247c:  br         IL_22d4

    IL_2481:  ldarg.0
    IL_2482:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2487:  ldloc.s    V_3
    IL_2489:  ldloc.s    V_2
    IL_248b:  dup
    IL_248c:  ldc.i4.1
    IL_248d:  add
    IL_248e:  stloc.s    V_2
    IL_2490:  ldelem.i4
    IL_2491:  stloc.s    V_0
    IL_2493:  br         IL_0263

    IL_2498:  ldarg.0
    IL_2499:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_249e:  ldloc.s    V_3
    IL_24a0:  ldloc.s    V_2
    IL_24a2:  dup
    IL_24a3:  ldc.i4.1
    IL_24a4:  add
    IL_24a5:  stloc.s    V_2
    IL_24a7:  ldelem.i4
    IL_24a8:  stloc.s    V_0
    IL_24aa:  ldloc.s    V_3
    IL_24ac:  ldloc.s    V_2
    IL_24ae:  dup
    IL_24af:  ldc.i4.1
    IL_24b0:  add
    IL_24b1:  stloc.s    V_2
    IL_24b3:  ldelem.i4
    IL_24b4:  stloc.s    V_6
    IL_24b6:  ldloc.s    V_6
    IL_24b8:  ldc.i4.s   0
    IL_24ba:  ble        IL_0258

    IL_24bf:  ldloc.s    V_3
    IL_24c1:  ldloc.s    V_2
    IL_24c3:  ldc.i4.1
    IL_24c4:  sub
    IL_24c5:  dup
    IL_24c6:  stloc.s    V_2
    IL_24c8:  ldloc.s    V_6
    IL_24ca:  ldc.i4.s   1
    IL_24cc:  sub
    IL_24cd:  stelem.i4
    IL_24ce:  ldloc.s    V_3
    IL_24d0:  ldloc.s    V_2
    IL_24d2:  ldc.i4.1
    IL_24d3:  sub
    IL_24d4:  dup
    IL_24d5:  stloc.s    V_2
    IL_24d7:  ldloc.s    V_0
    IL_24d9:  ldc.i4.s   1
    IL_24db:  sub
    IL_24dc:  stelem.i4
    IL_24dd:  ldloc.s    V_3
    IL_24df:  ldloc.s    V_2
    IL_24e1:  ldc.i4.1
    IL_24e2:  sub
    IL_24e3:  dup
    IL_24e4:  stloc.s    V_2
    IL_24e6:  ldc.i4.s   7
    IL_24e8:  stelem.i4
    IL_24e9:  br         IL_0258

    IL_24ee:  ldarg.0
    IL_24ef:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_24f4:  ldloc.s    V_5
    IL_24f6:  ldloc.s    V_4
    IL_24f8:  ldc.i4.1
    IL_24f9:  sub
    IL_24fa:  dup
    IL_24fb:  stloc.s    V_4
    IL_24fd:  ldloc.s    V_3
    IL_24ff:  ldloc.s    V_2
    IL_2501:  dup
    IL_2502:  ldc.i4.1
    IL_2503:  add
    IL_2504:  stloc.s    V_2
    IL_2506:  ldelem.i4
    IL_2507:  stelem.i4
    IL_2508:  ldarg.0
    IL_2509:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::Uncapture()
    IL_250e:  br         IL_22d4

    IL_2513:  ldarg.0
    IL_2514:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2519:  ldloc.s    V_5
    IL_251b:  ldloc.s    V_4
    IL_251d:  dup
    IL_251e:  ldc.i4.1
    IL_251f:  add
    IL_2520:  stloc.s    V_4
    IL_2522:  ldelem.i4
    IL_2523:  ldc.i4.s   1
    IL_2525:  sub
    IL_2526:  dup
    IL_2527:  stloc.s    V_6
    IL_2529:  ldc.i4.s   0
    IL_252b:  blt.s      IL_2556

    IL_252d:  ldloc.s    V_5
    IL_252f:  ldloc.s    V_4
    IL_2531:  dup
    IL_2532:  ldc.i4.1
    IL_2533:  add
    IL_2534:  stloc.s    V_4
    IL_2536:  ldelem.i4
    IL_2537:  stloc.s    V_0
    IL_2539:  ldloc.s    V_3
    IL_253b:  ldloc.s    V_2
    IL_253d:  ldc.i4.1
    IL_253e:  sub
    IL_253f:  dup
    IL_2540:  stloc.s    V_2
    IL_2542:  ldloc.s    V_6
    IL_2544:  stelem.i4
    IL_2545:  ldloc.s    V_3
    IL_2547:  ldloc.s    V_2
    IL_2549:  ldc.i4.1
    IL_254a:  sub
    IL_254b:  dup
    IL_254c:  stloc.s    V_2
    IL_254e:  ldc.i4.s   11
    IL_2550:  stelem.i4
    IL_2551:  br         IL_0366

    IL_2556:  ldloc.s    V_5
    IL_2558:  ldloc.s    V_4
    IL_255a:  ldloc.s    V_3
    IL_255c:  ldloc.s    V_2
    IL_255e:  dup
    IL_255f:  ldc.i4.1
    IL_2560:  add
    IL_2561:  stloc.s    V_2
    IL_2563:  ldelem.i4
    IL_2564:  stelem.i4
    IL_2565:  ldloc.s    V_5
    IL_2567:  ldloc.s    V_4
    IL_2569:  ldc.i4.1
    IL_256a:  sub
    IL_256b:  dup
    IL_256c:  stloc.s    V_4
    IL_256e:  ldloc.s    V_6
    IL_2570:  stelem.i4
    IL_2571:  br         IL_22d4

    IL_2576:  ldarg.0
    IL_2577:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_257c:  ldloc.s    V_3
    IL_257e:  ldloc.s    V_2
    IL_2580:  dup
    IL_2581:  ldc.i4.1
    IL_2582:  add
    IL_2583:  stloc.s    V_2
    IL_2585:  ldelem.i4
    IL_2586:  stloc.s    V_6
    IL_2588:  ldloc.s    V_5
    IL_258a:  ldloc.s    V_4
    IL_258c:  ldc.i4.1
    IL_258d:  sub
    IL_258e:  dup
    IL_258f:  stloc.s    V_4
    IL_2591:  ldloc.s    V_3
    IL_2593:  ldloc.s    V_2
    IL_2595:  dup
    IL_2596:  ldc.i4.1
    IL_2597:  add
    IL_2598:  stloc.s    V_2
    IL_259a:  ldelem.i4
    IL_259b:  stelem.i4
    IL_259c:  ldloc.s    V_5
    IL_259e:  ldloc.s    V_4
    IL_25a0:  ldc.i4.1
    IL_25a1:  sub
    IL_25a2:  dup
    IL_25a3:  stloc.s    V_4
    IL_25a5:  ldloc.s    V_6
    IL_25a7:  stelem.i4
    IL_25a8:  br         IL_22d4

    IL_25ad:  ldarg.0
    IL_25ae:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_25b3:  ldloc.s    V_3
    IL_25b5:  ldloc.s    V_2
    IL_25b7:  dup
    IL_25b8:  ldc.i4.1
    IL_25b9:  add
    IL_25ba:  stloc.s    V_2
    IL_25bc:  ldelem.i4
    IL_25bd:  stloc.s    V_0
    IL_25bf:  ldloc.s    V_3
    IL_25c1:  ldloc.s    V_2
    IL_25c3:  dup
    IL_25c4:  ldc.i4.1
    IL_25c5:  add
    IL_25c6:  stloc.s    V_2
    IL_25c8:  ldelem.i4
    IL_25c9:  stloc.s    V_6
    IL_25cb:  ldloc.s    V_6
    IL_25cd:  ldc.i4.s   0
    IL_25cf:  ble        IL_0445

    IL_25d4:  ldloc.s    V_3
    IL_25d6:  ldloc.s    V_2
    IL_25d8:  ldc.i4.1
    IL_25d9:  sub
    IL_25da:  dup
    IL_25db:  stloc.s    V_2
    IL_25dd:  ldloc.s    V_6
    IL_25df:  ldc.i4.s   1
    IL_25e1:  sub
    IL_25e2:  stelem.i4
    IL_25e3:  ldloc.s    V_3
    IL_25e5:  ldloc.s    V_2
    IL_25e7:  ldc.i4.1
    IL_25e8:  sub
    IL_25e9:  dup
    IL_25ea:  stloc.s    V_2
    IL_25ec:  ldloc.s    V_0
    IL_25ee:  ldc.i4.s   1
    IL_25f0:  sub
    IL_25f1:  stelem.i4
    IL_25f2:  ldloc.s    V_3
    IL_25f4:  ldloc.s    V_2
    IL_25f6:  ldc.i4.1
    IL_25f7:  sub
    IL_25f8:  dup
    IL_25f9:  stloc.s    V_2
    IL_25fb:  ldc.i4.s   12
    IL_25fd:  stelem.i4
    IL_25fe:  br         IL_0445

    IL_2603:  ldarg.0
    IL_2604:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2609:  ldloc.s    V_3
    IL_260b:  ldloc.s    V_2
    IL_260d:  dup
    IL_260e:  ldc.i4.1
    IL_260f:  add
    IL_2610:  stloc.s    V_2
    IL_2612:  ldelem.i4
    IL_2613:  stloc.s    V_0
    IL_2615:  ldloc.s    V_3
    IL_2617:  ldloc.s    V_2
    IL_2619:  dup
    IL_261a:  ldc.i4.1
    IL_261b:  add
    IL_261c:  stloc.s    V_2
    IL_261e:  ldelem.i4
    IL_261f:  stloc.s    V_6
    IL_2621:  ldloc.s    V_6
    IL_2623:  ldc.i4.s   0
    IL_2625:  ble        IL_058e

    IL_262a:  ldloc.s    V_3
    IL_262c:  ldloc.s    V_2
    IL_262e:  ldc.i4.1
    IL_262f:  sub
    IL_2630:  dup
    IL_2631:  stloc.s    V_2
    IL_2633:  ldloc.s    V_6
    IL_2635:  ldc.i4.s   1
    IL_2637:  sub
    IL_2638:  stelem.i4
    IL_2639:  ldloc.s    V_3
    IL_263b:  ldloc.s    V_2
    IL_263d:  ldc.i4.1
    IL_263e:  sub
    IL_263f:  dup
    IL_2640:  stloc.s    V_2
    IL_2642:  ldloc.s    V_0
    IL_2644:  ldc.i4.s   1
    IL_2646:  sub
    IL_2647:  stelem.i4
    IL_2648:  ldloc.s    V_3
    IL_264a:  ldloc.s    V_2
    IL_264c:  ldc.i4.1
    IL_264d:  sub
    IL_264e:  dup
    IL_264f:  stloc.s    V_2
    IL_2651:  ldc.i4.s   13
    IL_2653:  stelem.i4
    IL_2654:  br         IL_058e

    IL_2659:  ldarg.0
    IL_265a:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_265f:  ldloc.s    V_3
    IL_2661:  ldloc.s    V_2
    IL_2663:  dup
    IL_2664:  ldc.i4.1
    IL_2665:  add
    IL_2666:  stloc.s    V_2
    IL_2668:  ldelem.i4
    IL_2669:  stloc.s    V_0
    IL_266b:  ldloc.s    V_5
    IL_266d:  ldloc.s    V_4
    IL_266f:  dup
    IL_2670:  ldc.i4.1
    IL_2671:  add
    IL_2672:  stloc.s    V_4
    IL_2674:  ldelem.i4
    IL_2675:  pop
    IL_2676:  ldloc.s    V_3
    IL_2678:  ldloc.s    V_2
    IL_267a:  ldc.i4.1
    IL_267b:  sub
    IL_267c:  dup
    IL_267d:  stloc.s    V_2
    IL_267f:  ldc.i4.s   16
    IL_2681:  stelem.i4
    IL_2682:  br         IL_0662

    IL_2687:  ldarg.0
    IL_2688:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_268d:  ldloc.s    V_5
    IL_268f:  ldloc.s    V_4
    IL_2691:  ldc.i4.1
    IL_2692:  sub
    IL_2693:  dup
    IL_2694:  stloc.s    V_4
    IL_2696:  ldloc.s    V_3
    IL_2698:  ldloc.s    V_2
    IL_269a:  dup
    IL_269b:  ldc.i4.1
    IL_269c:  add
    IL_269d:  stloc.s    V_2
    IL_269f:  ldelem.i4
    IL_26a0:  stelem.i4
    IL_26a1:  br         IL_22d4

    IL_26a6:  ldarg.0
    IL_26a7:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_26ac:  ldloc.s    V_3
    IL_26ae:  ldloc.s    V_2
    IL_26b0:  dup
    IL_26b1:  ldc.i4.1
    IL_26b2:  add
    IL_26b3:  stloc.s    V_2
    IL_26b5:  ldelem.i4
    IL_26b6:  stloc.s    V_0
    IL_26b8:  ldloc.s    V_3
    IL_26ba:  ldloc.s    V_2
    IL_26bc:  dup
    IL_26bd:  ldc.i4.1
    IL_26be:  add
    IL_26bf:  stloc.s    V_2
    IL_26c1:  ldelem.i4
    IL_26c2:  stloc.s    V_6
    IL_26c4:  ldloc.s    V_6
    IL_26c6:  ldc.i4.s   0
    IL_26c8:  ble        IL_0723

    IL_26cd:  ldloc.s    V_3
    IL_26cf:  ldloc.s    V_2
    IL_26d1:  ldc.i4.1
    IL_26d2:  sub
    IL_26d3:  dup
    IL_26d4:  stloc.s    V_2
    IL_26d6:  ldloc.s    V_6
    IL_26d8:  ldc.i4.s   1
    IL_26da:  sub
    IL_26db:  stelem.i4
    IL_26dc:  ldloc.s    V_3
    IL_26de:  ldloc.s    V_2
    IL_26e0:  ldc.i4.1
    IL_26e1:  sub
    IL_26e2:  dup
    IL_26e3:  stloc.s    V_2
    IL_26e5:  ldloc.s    V_0
    IL_26e7:  ldc.i4.s   1
    IL_26e9:  sub
    IL_26ea:  stelem.i4
    IL_26eb:  ldloc.s    V_3
    IL_26ed:  ldloc.s    V_2
    IL_26ef:  ldc.i4.1
    IL_26f0:  sub
    IL_26f1:  dup
    IL_26f2:  stloc.s    V_2
    IL_26f4:  ldc.i4.s   17
    IL_26f6:  stelem.i4
    IL_26f7:  br         IL_0723

    IL_26fc:  ldarg.0
    IL_26fd:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2702:  ldloc.s    V_3
    IL_2704:  ldloc.s    V_2
    IL_2706:  dup
    IL_2707:  ldc.i4.1
    IL_2708:  add
    IL_2709:  stloc.s    V_2
    IL_270b:  ldelem.i4
    IL_270c:  stloc.s    V_0
    IL_270e:  ldloc.s    V_3
    IL_2710:  ldloc.s    V_2
    IL_2712:  dup
    IL_2713:  ldc.i4.1
    IL_2714:  add
    IL_2715:  stloc.s    V_2
    IL_2717:  ldelem.i4
    IL_2718:  stloc.s    V_6
    IL_271a:  ldloc.s    V_6
    IL_271c:  ldc.i4.s   0
    IL_271e:  ble        IL_07c2

    IL_2723:  ldloc.s    V_3
    IL_2725:  ldloc.s    V_2
    IL_2727:  ldc.i4.1
    IL_2728:  sub
    IL_2729:  dup
    IL_272a:  stloc.s    V_2
    IL_272c:  ldloc.s    V_6
    IL_272e:  ldc.i4.s   1
    IL_2730:  sub
    IL_2731:  stelem.i4
    IL_2732:  ldloc.s    V_3
    IL_2734:  ldloc.s    V_2
    IL_2736:  ldc.i4.1
    IL_2737:  sub
    IL_2738:  dup
    IL_2739:  stloc.s    V_2
    IL_273b:  ldloc.s    V_0
    IL_273d:  ldc.i4.s   1
    IL_273f:  sub
    IL_2740:  stelem.i4
    IL_2741:  ldloc.s    V_3
    IL_2743:  ldloc.s    V_2
    IL_2745:  ldc.i4.1
    IL_2746:  sub
    IL_2747:  dup
    IL_2748:  stloc.s    V_2
    IL_274a:  ldc.i4.s   18
    IL_274c:  stelem.i4
    IL_274d:  br         IL_07c2

    IL_2752:  ldarg.0
    IL_2753:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2758:  ldloc.s    V_3
    IL_275a:  ldloc.s    V_2
    IL_275c:  dup
    IL_275d:  ldc.i4.1
    IL_275e:  add
    IL_275f:  stloc.s    V_2
    IL_2761:  ldelem.i4
    IL_2762:  stloc.s    V_0
    IL_2764:  ldloc.s    V_3
    IL_2766:  ldloc.s    V_2
    IL_2768:  dup
    IL_2769:  ldc.i4.1
    IL_276a:  add
    IL_276b:  stloc.s    V_2
    IL_276d:  ldelem.i4
    IL_276e:  stloc.s    V_6
    IL_2770:  ldloc.s    V_6
    IL_2772:  ldc.i4.s   0
    IL_2774:  ble        IL_0883

    IL_2779:  ldloc.s    V_3
    IL_277b:  ldloc.s    V_2
    IL_277d:  ldc.i4.1
    IL_277e:  sub
    IL_277f:  dup
    IL_2780:  stloc.s    V_2
    IL_2782:  ldloc.s    V_6
    IL_2784:  ldc.i4.s   1
    IL_2786:  sub
    IL_2787:  stelem.i4
    IL_2788:  ldloc.s    V_3
    IL_278a:  ldloc.s    V_2
    IL_278c:  ldc.i4.1
    IL_278d:  sub
    IL_278e:  dup
    IL_278f:  stloc.s    V_2
    IL_2791:  ldloc.s    V_0
    IL_2793:  ldc.i4.s   1
    IL_2795:  sub
    IL_2796:  stelem.i4
    IL_2797:  ldloc.s    V_3
    IL_2799:  ldloc.s    V_2
    IL_279b:  ldc.i4.1
    IL_279c:  sub
    IL_279d:  dup
    IL_279e:  stloc.s    V_2
    IL_27a0:  ldc.i4.s   19
    IL_27a2:  stelem.i4
    IL_27a3:  br         IL_0883

    IL_27a8:  ldarg.0
    IL_27a9:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_27ae:  ldloc.s    V_3
    IL_27b0:  ldloc.s    V_2
    IL_27b2:  dup
    IL_27b3:  ldc.i4.1
    IL_27b4:  add
    IL_27b5:  stloc.s    V_2
    IL_27b7:  ldelem.i4
    IL_27b8:  stloc.s    V_0
    IL_27ba:  br         IL_0a0f

    IL_27bf:  ldarg.0
    IL_27c0:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_27c5:  ldloc.s    V_3
    IL_27c7:  ldloc.s    V_2
    IL_27c9:  dup
    IL_27ca:  ldc.i4.1
    IL_27cb:  add
    IL_27cc:  stloc.s    V_2
    IL_27ce:  ldelem.i4
    IL_27cf:  stloc.s    V_0
    IL_27d1:  ldloc.s    V_3
    IL_27d3:  ldloc.s    V_2
    IL_27d5:  dup
    IL_27d6:  ldc.i4.1
    IL_27d7:  add
    IL_27d8:  stloc.s    V_2
    IL_27da:  ldelem.i4
    IL_27db:  stloc.s    V_6
    IL_27dd:  ldloc.s    V_6
    IL_27df:  ldc.i4.s   0
    IL_27e1:  ble        IL_09c8

    IL_27e6:  ldloc.s    V_3
    IL_27e8:  ldloc.s    V_2
    IL_27ea:  ldc.i4.1
    IL_27eb:  sub
    IL_27ec:  dup
    IL_27ed:  stloc.s    V_2
    IL_27ef:  ldloc.s    V_6
    IL_27f1:  ldc.i4.s   1
    IL_27f3:  sub
    IL_27f4:  stelem.i4
    IL_27f5:  ldloc.s    V_3
    IL_27f7:  ldloc.s    V_2
    IL_27f9:  ldc.i4.1
    IL_27fa:  sub
    IL_27fb:  dup
    IL_27fc:  stloc.s    V_2
    IL_27fe:  ldloc.s    V_0
    IL_2800:  ldc.i4.s   1
    IL_2802:  sub
    IL_2803:  stelem.i4
    IL_2804:  ldloc.s    V_3
    IL_2806:  ldloc.s    V_2
    IL_2808:  ldc.i4.1
    IL_2809:  sub
    IL_280a:  dup
    IL_280b:  stloc.s    V_2
    IL_280d:  ldc.i4.s   21
    IL_280f:  stelem.i4
    IL_2810:  br         IL_09c8

    IL_2815:  ldarg.0
    IL_2816:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_281b:  ldloc.s    V_3
    IL_281d:  ldloc.s    V_2
    IL_281f:  dup
    IL_2820:  ldc.i4.1
    IL_2821:  add
    IL_2822:  stloc.s    V_2
    IL_2824:  ldelem.i4
    IL_2825:  stloc.s    V_0
    IL_2827:  ldloc.s    V_3
    IL_2829:  ldloc.s    V_2
    IL_282b:  dup
    IL_282c:  ldc.i4.1
    IL_282d:  add
    IL_282e:  stloc.s    V_2
    IL_2830:  ldelem.i4
    IL_2831:  stloc.s    V_6
    IL_2833:  ldloc.s    V_6
    IL_2835:  ldc.i4.s   0
    IL_2837:  ble        IL_0b75

    IL_283c:  ldloc.s    V_3
    IL_283e:  ldloc.s    V_2
    IL_2840:  ldc.i4.1
    IL_2841:  sub
    IL_2842:  dup
    IL_2843:  stloc.s    V_2
    IL_2845:  ldloc.s    V_6
    IL_2847:  ldc.i4.s   1
    IL_2849:  sub
    IL_284a:  stelem.i4
    IL_284b:  ldloc.s    V_3
    IL_284d:  ldloc.s    V_2
    IL_284f:  ldc.i4.1
    IL_2850:  sub
    IL_2851:  dup
    IL_2852:  stloc.s    V_2
    IL_2854:  ldloc.s    V_0
    IL_2856:  ldc.i4.s   1
    IL_2858:  sub
    IL_2859:  stelem.i4
    IL_285a:  ldloc.s    V_3
    IL_285c:  ldloc.s    V_2
    IL_285e:  ldc.i4.1
    IL_285f:  sub
    IL_2860:  dup
    IL_2861:  stloc.s    V_2
    IL_2863:  ldc.i4.s   22
    IL_2865:  stelem.i4
    IL_2866:  br         IL_0b75

    IL_286b:  ldarg.0
    IL_286c:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2871:  ldloc.s    V_5
    IL_2873:  ldloc.s    V_4
    IL_2875:  dup
    IL_2876:  ldc.i4.1
    IL_2877:  add
    IL_2878:  stloc.s    V_4
    IL_287a:  ldelem.i4
    IL_287b:  ldc.i4.s   1
    IL_287d:  sub
    IL_287e:  dup
    IL_287f:  stloc.s    V_6
    IL_2881:  ldc.i4.s   0
    IL_2883:  blt.s      IL_28ae

    IL_2885:  ldloc.s    V_5
    IL_2887:  ldloc.s    V_4
    IL_2889:  dup
    IL_288a:  ldc.i4.1
    IL_288b:  add
    IL_288c:  stloc.s    V_4
    IL_288e:  ldelem.i4
    IL_288f:  stloc.s    V_0
    IL_2891:  ldloc.s    V_3
    IL_2893:  ldloc.s    V_2
    IL_2895:  ldc.i4.1
    IL_2896:  sub
    IL_2897:  dup
    IL_2898:  stloc.s    V_2
    IL_289a:  ldloc.s    V_6
    IL_289c:  stelem.i4
    IL_289d:  ldloc.s    V_3
    IL_289f:  ldloc.s    V_2
    IL_28a1:  ldc.i4.1
    IL_28a2:  sub
    IL_28a3:  dup
    IL_28a4:  stloc.s    V_2
    IL_28a6:  ldc.i4.s   11
    IL_28a8:  stelem.i4
    IL_28a9:  br         IL_0cc3

    IL_28ae:  ldloc.s    V_5
    IL_28b0:  ldloc.s    V_4
    IL_28b2:  ldloc.s    V_3
    IL_28b4:  ldloc.s    V_2
    IL_28b6:  dup
    IL_28b7:  ldc.i4.1
    IL_28b8:  add
    IL_28b9:  stloc.s    V_2
    IL_28bb:  ldelem.i4
    IL_28bc:  stelem.i4
    IL_28bd:  ldloc.s    V_5
    IL_28bf:  ldloc.s    V_4
    IL_28c1:  ldc.i4.1
    IL_28c2:  sub
    IL_28c3:  dup
    IL_28c4:  stloc.s    V_4
    IL_28c6:  ldloc.s    V_6
    IL_28c8:  stelem.i4
    IL_28c9:  br         IL_22d4

    IL_28ce:  ldarg.0
    IL_28cf:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_28d4:  ldloc.s    V_5
    IL_28d6:  ldloc.s    V_4
    IL_28d8:  dup
    IL_28d9:  ldc.i4.1
    IL_28da:  add
    IL_28db:  stloc.s    V_4
    IL_28dd:  ldelem.i4
    IL_28de:  ldc.i4.s   1
    IL_28e0:  sub
    IL_28e1:  dup
    IL_28e2:  stloc.s    V_6
    IL_28e4:  ldc.i4.s   0
    IL_28e6:  blt.s      IL_2911

    IL_28e8:  ldloc.s    V_5
    IL_28ea:  ldloc.s    V_4
    IL_28ec:  dup
    IL_28ed:  ldc.i4.1
    IL_28ee:  add
    IL_28ef:  stloc.s    V_4
    IL_28f1:  ldelem.i4
    IL_28f2:  stloc.s    V_0
    IL_28f4:  ldloc.s    V_3
    IL_28f6:  ldloc.s    V_2
    IL_28f8:  ldc.i4.1
    IL_28f9:  sub
    IL_28fa:  dup
    IL_28fb:  stloc.s    V_2
    IL_28fd:  ldloc.s    V_6
    IL_28ff:  stelem.i4
    IL_2900:  ldloc.s    V_3
    IL_2902:  ldloc.s    V_2
    IL_2904:  ldc.i4.1
    IL_2905:  sub
    IL_2906:  dup
    IL_2907:  stloc.s    V_2
    IL_2909:  ldc.i4.s   11
    IL_290b:  stelem.i4
    IL_290c:  br         IL_0d99

    IL_2911:  ldloc.s    V_5
    IL_2913:  ldloc.s    V_4
    IL_2915:  ldloc.s    V_3
    IL_2917:  ldloc.s    V_2
    IL_2919:  dup
    IL_291a:  ldc.i4.1
    IL_291b:  add
    IL_291c:  stloc.s    V_2
    IL_291e:  ldelem.i4
    IL_291f:  stelem.i4
    IL_2920:  ldloc.s    V_5
    IL_2922:  ldloc.s    V_4
    IL_2924:  ldc.i4.1
    IL_2925:  sub
    IL_2926:  dup
    IL_2927:  stloc.s    V_4
    IL_2929:  ldloc.s    V_6
    IL_292b:  stelem.i4
    IL_292c:  br         IL_22d4

    IL_2931:  ldarg.0
    IL_2932:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2937:  ldloc.s    V_3
    IL_2939:  ldloc.s    V_2
    IL_293b:  dup
    IL_293c:  ldc.i4.1
    IL_293d:  add
    IL_293e:  stloc.s    V_2
    IL_2940:  ldelem.i4
    IL_2941:  stloc.s    V_0
    IL_2943:  ldloc.s    V_3
    IL_2945:  ldloc.s    V_2
    IL_2947:  dup
    IL_2948:  ldc.i4.1
    IL_2949:  add
    IL_294a:  stloc.s    V_2
    IL_294c:  ldelem.i4
    IL_294d:  stloc.s    V_6
    IL_294f:  ldloc.s    V_6
    IL_2951:  ldc.i4.s   0
    IL_2953:  ble        IL_0ed2

    IL_2958:  ldloc.s    V_3
    IL_295a:  ldloc.s    V_2
    IL_295c:  ldc.i4.1
    IL_295d:  sub
    IL_295e:  dup
    IL_295f:  stloc.s    V_2
    IL_2961:  ldloc.s    V_6
    IL_2963:  ldc.i4.s   1
    IL_2965:  sub
    IL_2966:  stelem.i4
    IL_2967:  ldloc.s    V_3
    IL_2969:  ldloc.s    V_2
    IL_296b:  ldc.i4.1
    IL_296c:  sub
    IL_296d:  dup
    IL_296e:  stloc.s    V_2
    IL_2970:  ldloc.s    V_0
    IL_2972:  ldc.i4.s   1
    IL_2974:  sub
    IL_2975:  stelem.i4
    IL_2976:  ldloc.s    V_3
    IL_2978:  ldloc.s    V_2
    IL_297a:  ldc.i4.1
    IL_297b:  sub
    IL_297c:  dup
    IL_297d:  stloc.s    V_2
    IL_297f:  ldc.i4.s   27
    IL_2981:  stelem.i4
    IL_2982:  br         IL_0ed2

    IL_2987:  ldarg.0
    IL_2988:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_298d:  ldloc.s    V_5
    IL_298f:  ldloc.s    V_4
    IL_2991:  dup
    IL_2992:  ldc.i4.1
    IL_2993:  add
    IL_2994:  stloc.s    V_4
    IL_2996:  ldelem.i4
    IL_2997:  ldc.i4.s   1
    IL_2999:  sub
    IL_299a:  dup
    IL_299b:  stloc.s    V_6
    IL_299d:  ldc.i4.s   0
    IL_299f:  blt.s      IL_29ca

    IL_29a1:  ldloc.s    V_5
    IL_29a3:  ldloc.s    V_4
    IL_29a5:  dup
    IL_29a6:  ldc.i4.1
    IL_29a7:  add
    IL_29a8:  stloc.s    V_4
    IL_29aa:  ldelem.i4
    IL_29ab:  stloc.s    V_0
    IL_29ad:  ldloc.s    V_3
    IL_29af:  ldloc.s    V_2
    IL_29b1:  ldc.i4.1
    IL_29b2:  sub
    IL_29b3:  dup
    IL_29b4:  stloc.s    V_2
    IL_29b6:  ldloc.s    V_6
    IL_29b8:  stelem.i4
    IL_29b9:  ldloc.s    V_3
    IL_29bb:  ldloc.s    V_2
    IL_29bd:  ldc.i4.1
    IL_29be:  sub
    IL_29bf:  dup
    IL_29c0:  stloc.s    V_2
    IL_29c2:  ldc.i4.s   11
    IL_29c4:  stelem.i4
    IL_29c5:  br         IL_0fcd

    IL_29ca:  ldloc.s    V_5
    IL_29cc:  ldloc.s    V_4
    IL_29ce:  ldloc.s    V_3
    IL_29d0:  ldloc.s    V_2
    IL_29d2:  dup
    IL_29d3:  ldc.i4.1
    IL_29d4:  add
    IL_29d5:  stloc.s    V_2
    IL_29d7:  ldelem.i4
    IL_29d8:  stelem.i4
    IL_29d9:  ldloc.s    V_5
    IL_29db:  ldloc.s    V_4
    IL_29dd:  ldc.i4.1
    IL_29de:  sub
    IL_29df:  dup
    IL_29e0:  stloc.s    V_4
    IL_29e2:  ldloc.s    V_6
    IL_29e4:  stelem.i4
    IL_29e5:  br         IL_22d4

    IL_29ea:  ldarg.0
    IL_29eb:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_29f0:  ldloc.s    V_3
    IL_29f2:  ldloc.s    V_2
    IL_29f4:  dup
    IL_29f5:  ldc.i4.1
    IL_29f6:  add
    IL_29f7:  stloc.s    V_2
    IL_29f9:  ldelem.i4
    IL_29fa:  stloc.s    V_0
    IL_29fc:  br         IL_111f

    IL_2a01:  ldarg.0
    IL_2a02:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2a07:  ldloc.s    V_3
    IL_2a09:  ldloc.s    V_2
    IL_2a0b:  dup
    IL_2a0c:  ldc.i4.1
    IL_2a0d:  add
    IL_2a0e:  stloc.s    V_2
    IL_2a10:  ldelem.i4
    IL_2a11:  stloc.s    V_0
    IL_2a13:  ldloc.s    V_3
    IL_2a15:  ldloc.s    V_2
    IL_2a17:  dup
    IL_2a18:  ldc.i4.1
    IL_2a19:  add
    IL_2a1a:  stloc.s    V_2
    IL_2a1c:  ldelem.i4
    IL_2a1d:  stloc.s    V_6
    IL_2a1f:  ldloc.s    V_6
    IL_2a21:  ldc.i4.s   0
    IL_2a23:  ble        IL_10ef

    IL_2a28:  ldloc.s    V_3
    IL_2a2a:  ldloc.s    V_2
    IL_2a2c:  ldc.i4.1
    IL_2a2d:  sub
    IL_2a2e:  dup
    IL_2a2f:  stloc.s    V_2
    IL_2a31:  ldloc.s    V_6
    IL_2a33:  ldc.i4.s   1
    IL_2a35:  sub
    IL_2a36:  stelem.i4
    IL_2a37:  ldloc.s    V_3
    IL_2a39:  ldloc.s    V_2
    IL_2a3b:  ldc.i4.1
    IL_2a3c:  sub
    IL_2a3d:  dup
    IL_2a3e:  stloc.s    V_2
    IL_2a40:  ldloc.s    V_0
    IL_2a42:  ldc.i4.s   1
    IL_2a44:  sub
    IL_2a45:  stelem.i4
    IL_2a46:  ldloc.s    V_3
    IL_2a48:  ldloc.s    V_2
    IL_2a4a:  ldc.i4.1
    IL_2a4b:  sub
    IL_2a4c:  dup
    IL_2a4d:  stloc.s    V_2
    IL_2a4f:  ldc.i4.s   31
    IL_2a51:  stelem.i4
    IL_2a52:  br         IL_10ef

    IL_2a57:  ldarg.0
    IL_2a58:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2a5d:  ldloc.s    V_3
    IL_2a5f:  ldloc.s    V_2
    IL_2a61:  dup
    IL_2a62:  ldc.i4.1
    IL_2a63:  add
    IL_2a64:  stloc.s    V_2
    IL_2a66:  ldelem.i4
    IL_2a67:  stloc.s    V_0
    IL_2a69:  br         IL_1245

    IL_2a6e:  ldarg.0
    IL_2a6f:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2a74:  ldloc.s    V_3
    IL_2a76:  ldloc.s    V_2
    IL_2a78:  dup
    IL_2a79:  ldc.i4.1
    IL_2a7a:  add
    IL_2a7b:  stloc.s    V_2
    IL_2a7d:  ldelem.i4
    IL_2a7e:  stloc.s    V_0
    IL_2a80:  ldloc.s    V_3
    IL_2a82:  ldloc.s    V_2
    IL_2a84:  dup
    IL_2a85:  ldc.i4.1
    IL_2a86:  add
    IL_2a87:  stloc.s    V_2
    IL_2a89:  ldelem.i4
    IL_2a8a:  stloc.s    V_6
    IL_2a8c:  ldloc.s    V_6
    IL_2a8e:  ldc.i4.s   0
    IL_2a90:  ble        IL_11fe

    IL_2a95:  ldloc.s    V_3
    IL_2a97:  ldloc.s    V_2
    IL_2a99:  ldc.i4.1
    IL_2a9a:  sub
    IL_2a9b:  dup
    IL_2a9c:  stloc.s    V_2
    IL_2a9e:  ldloc.s    V_6
    IL_2aa0:  ldc.i4.s   1
    IL_2aa2:  sub
    IL_2aa3:  stelem.i4
    IL_2aa4:  ldloc.s    V_3
    IL_2aa6:  ldloc.s    V_2
    IL_2aa8:  ldc.i4.1
    IL_2aa9:  sub
    IL_2aaa:  dup
    IL_2aab:  stloc.s    V_2
    IL_2aad:  ldloc.s    V_0
    IL_2aaf:  ldc.i4.s   1
    IL_2ab1:  sub
    IL_2ab2:  stelem.i4
    IL_2ab3:  ldloc.s    V_3
    IL_2ab5:  ldloc.s    V_2
    IL_2ab7:  ldc.i4.1
    IL_2ab8:  sub
    IL_2ab9:  dup
    IL_2aba:  stloc.s    V_2
    IL_2abc:  ldc.i4.s   33
    IL_2abe:  stelem.i4
    IL_2abf:  br         IL_11fe

    IL_2ac4:  ldarg.0
    IL_2ac5:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2aca:  ldloc.s    V_3
    IL_2acc:  ldloc.s    V_2
    IL_2ace:  dup
    IL_2acf:  ldc.i4.1
    IL_2ad0:  add
    IL_2ad1:  stloc.s    V_2
    IL_2ad3:  ldelem.i4
    IL_2ad4:  stloc.s    V_0
    IL_2ad6:  ldloc.s    V_3
    IL_2ad8:  ldloc.s    V_2
    IL_2ada:  dup
    IL_2adb:  ldc.i4.1
    IL_2adc:  add
    IL_2add:  stloc.s    V_2
    IL_2adf:  ldelem.i4
    IL_2ae0:  stloc.s    V_6
    IL_2ae2:  ldloc.s    V_6
    IL_2ae4:  ldc.i4.s   0
    IL_2ae6:  ble        IL_1306

    IL_2aeb:  ldloc.s    V_3
    IL_2aed:  ldloc.s    V_2
    IL_2aef:  ldc.i4.1
    IL_2af0:  sub
    IL_2af1:  dup
    IL_2af2:  stloc.s    V_2
    IL_2af4:  ldloc.s    V_6
    IL_2af6:  ldc.i4.s   1
    IL_2af8:  sub
    IL_2af9:  stelem.i4
    IL_2afa:  ldloc.s    V_3
    IL_2afc:  ldloc.s    V_2
    IL_2afe:  ldc.i4.1
    IL_2aff:  sub
    IL_2b00:  dup
    IL_2b01:  stloc.s    V_2
    IL_2b03:  ldloc.s    V_0
    IL_2b05:  ldc.i4.s   1
    IL_2b07:  sub
    IL_2b08:  stelem.i4
    IL_2b09:  ldloc.s    V_3
    IL_2b0b:  ldloc.s    V_2
    IL_2b0d:  ldc.i4.1
    IL_2b0e:  sub
    IL_2b0f:  dup
    IL_2b10:  stloc.s    V_2
    IL_2b12:  ldc.i4.s   34
    IL_2b14:  stelem.i4
    IL_2b15:  br         IL_1306

    IL_2b1a:  ldarg.0
    IL_2b1b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2b20:  ldloc.s    V_3
    IL_2b22:  ldloc.s    V_2
    IL_2b24:  dup
    IL_2b25:  ldc.i4.1
    IL_2b26:  add
    IL_2b27:  stloc.s    V_2
    IL_2b29:  ldelem.i4
    IL_2b2a:  stloc.s    V_0
    IL_2b2c:  ldloc.s    V_3
    IL_2b2e:  ldloc.s    V_2
    IL_2b30:  dup
    IL_2b31:  ldc.i4.1
    IL_2b32:  add
    IL_2b33:  stloc.s    V_2
    IL_2b35:  ldelem.i4
    IL_2b36:  stloc.s    V_6
    IL_2b38:  ldloc.s    V_6
    IL_2b3a:  ldc.i4.s   0
    IL_2b3c:  ble        IL_13ec

    IL_2b41:  ldloc.s    V_3
    IL_2b43:  ldloc.s    V_2
    IL_2b45:  ldc.i4.1
    IL_2b46:  sub
    IL_2b47:  dup
    IL_2b48:  stloc.s    V_2
    IL_2b4a:  ldloc.s    V_6
    IL_2b4c:  ldc.i4.s   1
    IL_2b4e:  sub
    IL_2b4f:  stelem.i4
    IL_2b50:  ldloc.s    V_3
    IL_2b52:  ldloc.s    V_2
    IL_2b54:  ldc.i4.1
    IL_2b55:  sub
    IL_2b56:  dup
    IL_2b57:  stloc.s    V_2
    IL_2b59:  ldloc.s    V_0
    IL_2b5b:  ldc.i4.s   1
    IL_2b5d:  sub
    IL_2b5e:  stelem.i4
    IL_2b5f:  ldloc.s    V_3
    IL_2b61:  ldloc.s    V_2
    IL_2b63:  ldc.i4.1
    IL_2b64:  sub
    IL_2b65:  dup
    IL_2b66:  stloc.s    V_2
    IL_2b68:  ldc.i4.s   35
    IL_2b6a:  stelem.i4
    IL_2b6b:  br         IL_13ec

    IL_2b70:  ldarg.0
    IL_2b71:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2b76:  ldloc.s    V_5
    IL_2b78:  ldloc.s    V_4
    IL_2b7a:  dup
    IL_2b7b:  ldc.i4.1
    IL_2b7c:  add
    IL_2b7d:  stloc.s    V_4
    IL_2b7f:  ldelem.i4
    IL_2b80:  ldc.i4.s   1
    IL_2b82:  sub
    IL_2b83:  dup
    IL_2b84:  stloc.s    V_6
    IL_2b86:  ldc.i4.s   0
    IL_2b88:  blt.s      IL_2bb3

    IL_2b8a:  ldloc.s    V_5
    IL_2b8c:  ldloc.s    V_4
    IL_2b8e:  dup
    IL_2b8f:  ldc.i4.1
    IL_2b90:  add
    IL_2b91:  stloc.s    V_4
    IL_2b93:  ldelem.i4
    IL_2b94:  stloc.s    V_0
    IL_2b96:  ldloc.s    V_3
    IL_2b98:  ldloc.s    V_2
    IL_2b9a:  ldc.i4.1
    IL_2b9b:  sub
    IL_2b9c:  dup
    IL_2b9d:  stloc.s    V_2
    IL_2b9f:  ldloc.s    V_6
    IL_2ba1:  stelem.i4
    IL_2ba2:  ldloc.s    V_3
    IL_2ba4:  ldloc.s    V_2
    IL_2ba6:  ldc.i4.1
    IL_2ba7:  sub
    IL_2ba8:  dup
    IL_2ba9:  stloc.s    V_2
    IL_2bab:  ldc.i4.s   11
    IL_2bad:  stelem.i4
    IL_2bae:  br         IL_15f9

    IL_2bb3:  ldloc.s    V_5
    IL_2bb5:  ldloc.s    V_4
    IL_2bb7:  ldloc.s    V_3
    IL_2bb9:  ldloc.s    V_2
    IL_2bbb:  dup
    IL_2bbc:  ldc.i4.1
    IL_2bbd:  add
    IL_2bbe:  stloc.s    V_2
    IL_2bc0:  ldelem.i4
    IL_2bc1:  stelem.i4
    IL_2bc2:  ldloc.s    V_5
    IL_2bc4:  ldloc.s    V_4
    IL_2bc6:  ldc.i4.1
    IL_2bc7:  sub
    IL_2bc8:  dup
    IL_2bc9:  stloc.s    V_4
    IL_2bcb:  ldloc.s    V_6
    IL_2bcd:  stelem.i4
    IL_2bce:  br         IL_22d4

    IL_2bd3:  ldarg.0
    IL_2bd4:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2bd9:  ldloc.s    V_3
    IL_2bdb:  ldloc.s    V_2
    IL_2bdd:  dup
    IL_2bde:  ldc.i4.1
    IL_2bdf:  add
    IL_2be0:  stloc.s    V_2
    IL_2be2:  ldelem.i4
    IL_2be3:  stloc.s    V_0
    IL_2be5:  br         IL_17dc

    IL_2bea:  ldarg.0
    IL_2beb:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2bf0:  ldloc.s    V_3
    IL_2bf2:  ldloc.s    V_2
    IL_2bf4:  dup
    IL_2bf5:  ldc.i4.1
    IL_2bf6:  add
    IL_2bf7:  stloc.s    V_2
    IL_2bf9:  ldelem.i4
    IL_2bfa:  stloc.s    V_0
    IL_2bfc:  ldloc.s    V_3
    IL_2bfe:  ldloc.s    V_2
    IL_2c00:  dup
    IL_2c01:  ldc.i4.1
    IL_2c02:  add
    IL_2c03:  stloc.s    V_2
    IL_2c05:  ldelem.i4
    IL_2c06:  stloc.s    V_6
    IL_2c08:  ldloc.s    V_6
    IL_2c0a:  ldc.i4.s   0
    IL_2c0c:  ble        IL_16e4

    IL_2c11:  ldloc.s    V_3
    IL_2c13:  ldloc.s    V_2
    IL_2c15:  ldc.i4.1
    IL_2c16:  sub
    IL_2c17:  dup
    IL_2c18:  stloc.s    V_2
    IL_2c1a:  ldloc.s    V_6
    IL_2c1c:  ldc.i4.s   1
    IL_2c1e:  sub
    IL_2c1f:  stelem.i4
    IL_2c20:  ldloc.s    V_3
    IL_2c22:  ldloc.s    V_2
    IL_2c24:  ldc.i4.1
    IL_2c25:  sub
    IL_2c26:  dup
    IL_2c27:  stloc.s    V_2
    IL_2c29:  ldloc.s    V_0
    IL_2c2b:  ldc.i4.s   1
    IL_2c2d:  sub
    IL_2c2e:  stelem.i4
    IL_2c2f:  ldloc.s    V_3
    IL_2c31:  ldloc.s    V_2
    IL_2c33:  ldc.i4.1
    IL_2c34:  sub
    IL_2c35:  dup
    IL_2c36:  stloc.s    V_2
    IL_2c38:  ldc.i4.s   39
    IL_2c3a:  stelem.i4
    IL_2c3b:  br         IL_16e4

    IL_2c40:  ldarg.0
    IL_2c41:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2c46:  ldloc.s    V_3
    IL_2c48:  ldloc.s    V_2
    IL_2c4a:  dup
    IL_2c4b:  ldc.i4.1
    IL_2c4c:  add
    IL_2c4d:  stloc.s    V_2
    IL_2c4f:  ldelem.i4
    IL_2c50:  stloc.s    V_0
    IL_2c52:  ldloc.s    V_3
    IL_2c54:  ldloc.s    V_2
    IL_2c56:  dup
    IL_2c57:  ldc.i4.1
    IL_2c58:  add
    IL_2c59:  stloc.s    V_2
    IL_2c5b:  ldelem.i4
    IL_2c5c:  stloc.s    V_6
    IL_2c5e:  ldloc.s    V_6
    IL_2c60:  ldc.i4.s   0
    IL_2c62:  ble        IL_179b

    IL_2c67:  ldloc.s    V_3
    IL_2c69:  ldloc.s    V_2
    IL_2c6b:  ldc.i4.1
    IL_2c6c:  sub
    IL_2c6d:  dup
    IL_2c6e:  stloc.s    V_2
    IL_2c70:  ldloc.s    V_6
    IL_2c72:  ldc.i4.s   1
    IL_2c74:  sub
    IL_2c75:  stelem.i4
    IL_2c76:  ldloc.s    V_3
    IL_2c78:  ldloc.s    V_2
    IL_2c7a:  ldc.i4.1
    IL_2c7b:  sub
    IL_2c7c:  dup
    IL_2c7d:  stloc.s    V_2
    IL_2c7f:  ldloc.s    V_0
    IL_2c81:  ldc.i4.s   1
    IL_2c83:  sub
    IL_2c84:  stelem.i4
    IL_2c85:  ldloc.s    V_3
    IL_2c87:  ldloc.s    V_2
    IL_2c89:  ldc.i4.1
    IL_2c8a:  sub
    IL_2c8b:  dup
    IL_2c8c:  stloc.s    V_2
    IL_2c8e:  ldc.i4.s   40
    IL_2c90:  stelem.i4
    IL_2c91:  br         IL_179b

    IL_2c96:  ldarg.0
    IL_2c97:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2c9c:  ldloc.s    V_3
    IL_2c9e:  ldloc.s    V_2
    IL_2ca0:  dup
    IL_2ca1:  ldc.i4.1
    IL_2ca2:  add
    IL_2ca3:  stloc.s    V_2
    IL_2ca5:  ldelem.i4
    IL_2ca6:  stloc.s    V_0
    IL_2ca8:  br         IL_1917

    IL_2cad:  ldarg.0
    IL_2cae:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2cb3:  ldloc.s    V_3
    IL_2cb5:  ldloc.s    V_2
    IL_2cb7:  dup
    IL_2cb8:  ldc.i4.1
    IL_2cb9:  add
    IL_2cba:  stloc.s    V_2
    IL_2cbc:  ldelem.i4
    IL_2cbd:  stloc.s    V_0
    IL_2cbf:  ldloc.s    V_3
    IL_2cc1:  ldloc.s    V_2
    IL_2cc3:  dup
    IL_2cc4:  ldc.i4.1
    IL_2cc5:  add
    IL_2cc6:  stloc.s    V_2
    IL_2cc8:  ldelem.i4
    IL_2cc9:  stloc.s    V_6
    IL_2ccb:  ldloc.s    V_6
    IL_2ccd:  ldc.i4.s   0
    IL_2ccf:  ble        IL_18d6

    IL_2cd4:  ldloc.s    V_3
    IL_2cd6:  ldloc.s    V_2
    IL_2cd8:  ldc.i4.1
    IL_2cd9:  sub
    IL_2cda:  dup
    IL_2cdb:  stloc.s    V_2
    IL_2cdd:  ldloc.s    V_6
    IL_2cdf:  ldc.i4.s   1
    IL_2ce1:  sub
    IL_2ce2:  stelem.i4
    IL_2ce3:  ldloc.s    V_3
    IL_2ce5:  ldloc.s    V_2
    IL_2ce7:  ldc.i4.1
    IL_2ce8:  sub
    IL_2ce9:  dup
    IL_2cea:  stloc.s    V_2
    IL_2cec:  ldloc.s    V_0
    IL_2cee:  ldc.i4.s   1
    IL_2cf0:  sub
    IL_2cf1:  stelem.i4
    IL_2cf2:  ldloc.s    V_3
    IL_2cf4:  ldloc.s    V_2
    IL_2cf6:  ldc.i4.1
    IL_2cf7:  sub
    IL_2cf8:  dup
    IL_2cf9:  stloc.s    V_2
    IL_2cfb:  ldc.i4.s   42
    IL_2cfd:  stelem.i4
    IL_2cfe:  br         IL_18d6

    IL_2d03:  ldarg.0
    IL_2d04:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2d09:  ldloc.s    V_3
    IL_2d0b:  ldloc.s    V_2
    IL_2d0d:  dup
    IL_2d0e:  ldc.i4.1
    IL_2d0f:  add
    IL_2d10:  stloc.s    V_2
    IL_2d12:  ldelem.i4
    IL_2d13:  stloc.s    V_0
    IL_2d15:  ldloc.s    V_3
    IL_2d17:  ldloc.s    V_2
    IL_2d19:  dup
    IL_2d1a:  ldc.i4.1
    IL_2d1b:  add
    IL_2d1c:  stloc.s    V_2
    IL_2d1e:  ldelem.i4
    IL_2d1f:  stloc.s    V_6
    IL_2d21:  ldloc.s    V_6
    IL_2d23:  ldc.i4.s   0
    IL_2d25:  ble        IL_1a1f

    IL_2d2a:  ldloc.s    V_3
    IL_2d2c:  ldloc.s    V_2
    IL_2d2e:  ldc.i4.1
    IL_2d2f:  sub
    IL_2d30:  dup
    IL_2d31:  stloc.s    V_2
    IL_2d33:  ldloc.s    V_6
    IL_2d35:  ldc.i4.s   1
    IL_2d37:  sub
    IL_2d38:  stelem.i4
    IL_2d39:  ldloc.s    V_3
    IL_2d3b:  ldloc.s    V_2
    IL_2d3d:  ldc.i4.1
    IL_2d3e:  sub
    IL_2d3f:  dup
    IL_2d40:  stloc.s    V_2
    IL_2d42:  ldloc.s    V_0
    IL_2d44:  ldc.i4.s   1
    IL_2d46:  sub
    IL_2d47:  stelem.i4
    IL_2d48:  ldloc.s    V_3
    IL_2d4a:  ldloc.s    V_2
    IL_2d4c:  ldc.i4.1
    IL_2d4d:  sub
    IL_2d4e:  dup
    IL_2d4f:  stloc.s    V_2
    IL_2d51:  ldc.i4.s   43
    IL_2d53:  stelem.i4
    IL_2d54:  br         IL_1a1f

    IL_2d59:  ldarg.0
    IL_2d5a:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2d5f:  ldloc.s    V_3
    IL_2d61:  ldloc.s    V_2
    IL_2d63:  dup
    IL_2d64:  ldc.i4.1
    IL_2d65:  add
    IL_2d66:  stloc.s    V_2
    IL_2d68:  ldelem.i4
    IL_2d69:  stloc.s    V_0
    IL_2d6b:  ldloc.s    V_5
    IL_2d6d:  ldloc.s    V_4
    IL_2d6f:  dup
    IL_2d70:  ldc.i4.1
    IL_2d71:  add
    IL_2d72:  stloc.s    V_4
    IL_2d74:  ldelem.i4
    IL_2d75:  pop
    IL_2d76:  ldloc.s    V_3
    IL_2d78:  ldloc.s    V_2
    IL_2d7a:  ldc.i4.1
    IL_2d7b:  sub
    IL_2d7c:  dup
    IL_2d7d:  stloc.s    V_2
    IL_2d7f:  ldc.i4.s   16
    IL_2d81:  stelem.i4
    IL_2d82:  br         IL_1af3

    IL_2d87:  ldarg.0
    IL_2d88:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2d8d:  ldloc.s    V_3
    IL_2d8f:  ldloc.s    V_2
    IL_2d91:  dup
    IL_2d92:  ldc.i4.1
    IL_2d93:  add
    IL_2d94:  stloc.s    V_2
    IL_2d96:  ldelem.i4
    IL_2d97:  stloc.s    V_0
    IL_2d99:  ldloc.s    V_3
    IL_2d9b:  ldloc.s    V_2
    IL_2d9d:  dup
    IL_2d9e:  ldc.i4.1
    IL_2d9f:  add
    IL_2da0:  stloc.s    V_2
    IL_2da2:  ldelem.i4
    IL_2da3:  stloc.s    V_6
    IL_2da5:  ldloc.s    V_6
    IL_2da7:  ldc.i4.s   0
    IL_2da9:  ble        IL_1bb4

    IL_2dae:  ldloc.s    V_3
    IL_2db0:  ldloc.s    V_2
    IL_2db2:  ldc.i4.1
    IL_2db3:  sub
    IL_2db4:  dup
    IL_2db5:  stloc.s    V_2
    IL_2db7:  ldloc.s    V_6
    IL_2db9:  ldc.i4.s   1
    IL_2dbb:  sub
    IL_2dbc:  stelem.i4
    IL_2dbd:  ldloc.s    V_3
    IL_2dbf:  ldloc.s    V_2
    IL_2dc1:  ldc.i4.1
    IL_2dc2:  sub
    IL_2dc3:  dup
    IL_2dc4:  stloc.s    V_2
    IL_2dc6:  ldloc.s    V_0
    IL_2dc8:  ldc.i4.s   1
    IL_2dca:  sub
    IL_2dcb:  stelem.i4
    IL_2dcc:  ldloc.s    V_3
    IL_2dce:  ldloc.s    V_2
    IL_2dd0:  ldc.i4.1
    IL_2dd1:  sub
    IL_2dd2:  dup
    IL_2dd3:  stloc.s    V_2
    IL_2dd5:  ldc.i4.s   46
    IL_2dd7:  stelem.i4
    IL_2dd8:  br         IL_1bb4

    IL_2ddd:  ldarg.0
    IL_2dde:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2de3:  ldloc.s    V_3
    IL_2de5:  ldloc.s    V_2
    IL_2de7:  dup
    IL_2de8:  ldc.i4.1
    IL_2de9:  add
    IL_2dea:  stloc.s    V_2
    IL_2dec:  ldelem.i4
    IL_2ded:  stloc.s    V_0
    IL_2def:  ldloc.s    V_3
    IL_2df1:  ldloc.s    V_2
    IL_2df3:  dup
    IL_2df4:  ldc.i4.1
    IL_2df5:  add
    IL_2df6:  stloc.s    V_2
    IL_2df8:  ldelem.i4
    IL_2df9:  stloc.s    V_6
    IL_2dfb:  ldloc.s    V_6
    IL_2dfd:  ldc.i4.s   0
    IL_2dff:  ble        IL_1c53

    IL_2e04:  ldloc.s    V_3
    IL_2e06:  ldloc.s    V_2
    IL_2e08:  ldc.i4.1
    IL_2e09:  sub
    IL_2e0a:  dup
    IL_2e0b:  stloc.s    V_2
    IL_2e0d:  ldloc.s    V_6
    IL_2e0f:  ldc.i4.s   1
    IL_2e11:  sub
    IL_2e12:  stelem.i4
    IL_2e13:  ldloc.s    V_3
    IL_2e15:  ldloc.s    V_2
    IL_2e17:  ldc.i4.1
    IL_2e18:  sub
    IL_2e19:  dup
    IL_2e1a:  stloc.s    V_2
    IL_2e1c:  ldloc.s    V_0
    IL_2e1e:  ldc.i4.s   1
    IL_2e20:  sub
    IL_2e21:  stelem.i4
    IL_2e22:  ldloc.s    V_3
    IL_2e24:  ldloc.s    V_2
    IL_2e26:  ldc.i4.1
    IL_2e27:  sub
    IL_2e28:  dup
    IL_2e29:  stloc.s    V_2
    IL_2e2b:  ldc.i4.s   47
    IL_2e2d:  stelem.i4
    IL_2e2e:  br         IL_1c53

    IL_2e33:  ldarg.0
    IL_2e34:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2e39:  ldloc.s    V_3
    IL_2e3b:  ldloc.s    V_2
    IL_2e3d:  dup
    IL_2e3e:  ldc.i4.1
    IL_2e3f:  add
    IL_2e40:  stloc.s    V_2
    IL_2e42:  ldelem.i4
    IL_2e43:  stloc.s    V_0
    IL_2e45:  ldloc.s    V_3
    IL_2e47:  ldloc.s    V_2
    IL_2e49:  dup
    IL_2e4a:  ldc.i4.1
    IL_2e4b:  add
    IL_2e4c:  stloc.s    V_2
    IL_2e4e:  ldelem.i4
    IL_2e4f:  stloc.s    V_6
    IL_2e51:  ldloc.s    V_6
    IL_2e53:  ldc.i4.s   0
    IL_2e55:  ble        IL_1d14

    IL_2e5a:  ldloc.s    V_3
    IL_2e5c:  ldloc.s    V_2
    IL_2e5e:  ldc.i4.1
    IL_2e5f:  sub
    IL_2e60:  dup
    IL_2e61:  stloc.s    V_2
    IL_2e63:  ldloc.s    V_6
    IL_2e65:  ldc.i4.s   1
    IL_2e67:  sub
    IL_2e68:  stelem.i4
    IL_2e69:  ldloc.s    V_3
    IL_2e6b:  ldloc.s    V_2
    IL_2e6d:  ldc.i4.1
    IL_2e6e:  sub
    IL_2e6f:  dup
    IL_2e70:  stloc.s    V_2
    IL_2e72:  ldloc.s    V_0
    IL_2e74:  ldc.i4.s   1
    IL_2e76:  sub
    IL_2e77:  stelem.i4
    IL_2e78:  ldloc.s    V_3
    IL_2e7a:  ldloc.s    V_2
    IL_2e7c:  ldc.i4.1
    IL_2e7d:  sub
    IL_2e7e:  dup
    IL_2e7f:  stloc.s    V_2
    IL_2e81:  ldc.i4.s   48
    IL_2e83:  stelem.i4
    IL_2e84:  br         IL_1d14

    IL_2e89:  ldarg.0
    IL_2e8a:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2e8f:  ldloc.s    V_5
    IL_2e91:  ldloc.s    V_4
    IL_2e93:  dup
    IL_2e94:  ldc.i4.1
    IL_2e95:  add
    IL_2e96:  stloc.s    V_4
    IL_2e98:  ldelem.i4
    IL_2e99:  ldc.i4.s   1
    IL_2e9b:  sub
    IL_2e9c:  dup
    IL_2e9d:  stloc.s    V_6
    IL_2e9f:  ldc.i4.s   0
    IL_2ea1:  blt.s      IL_2ecc

    IL_2ea3:  ldloc.s    V_5
    IL_2ea5:  ldloc.s    V_4
    IL_2ea7:  dup
    IL_2ea8:  ldc.i4.1
    IL_2ea9:  add
    IL_2eaa:  stloc.s    V_4
    IL_2eac:  ldelem.i4
    IL_2ead:  stloc.s    V_0
    IL_2eaf:  ldloc.s    V_3
    IL_2eb1:  ldloc.s    V_2
    IL_2eb3:  ldc.i4.1
    IL_2eb4:  sub
    IL_2eb5:  dup
    IL_2eb6:  stloc.s    V_2
    IL_2eb8:  ldloc.s    V_6
    IL_2eba:  stelem.i4
    IL_2ebb:  ldloc.s    V_3
    IL_2ebd:  ldloc.s    V_2
    IL_2ebf:  ldc.i4.1
    IL_2ec0:  sub
    IL_2ec1:  dup
    IL_2ec2:  stloc.s    V_2
    IL_2ec4:  ldc.i4.s   11
    IL_2ec6:  stelem.i4
    IL_2ec7:  br         IL_1dea

    IL_2ecc:  ldloc.s    V_5
    IL_2ece:  ldloc.s    V_4
    IL_2ed0:  ldloc.s    V_3
    IL_2ed2:  ldloc.s    V_2
    IL_2ed4:  dup
    IL_2ed5:  ldc.i4.1
    IL_2ed6:  add
    IL_2ed7:  stloc.s    V_2
    IL_2ed9:  ldelem.i4
    IL_2eda:  stelem.i4
    IL_2edb:  ldloc.s    V_5
    IL_2edd:  ldloc.s    V_4
    IL_2edf:  ldc.i4.1
    IL_2ee0:  sub
    IL_2ee1:  dup
    IL_2ee2:  stloc.s    V_4
    IL_2ee4:  ldloc.s    V_6
    IL_2ee6:  stelem.i4
    IL_2ee7:  br         IL_22d4

    IL_2eec:  ldarg.0
    IL_2eed:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2ef2:  ldloc.s    V_3
    IL_2ef4:  ldloc.s    V_2
    IL_2ef6:  dup
    IL_2ef7:  ldc.i4.1
    IL_2ef8:  add
    IL_2ef9:  stloc.s    V_2
    IL_2efb:  ldelem.i4
    IL_2efc:  stloc.s    V_0
    IL_2efe:  ldloc.s    V_5
    IL_2f00:  ldloc.s    V_4
    IL_2f02:  dup
    IL_2f03:  ldc.i4.1
    IL_2f04:  add
    IL_2f05:  stloc.s    V_4
    IL_2f07:  ldelem.i4
    IL_2f08:  pop
    IL_2f09:  ldloc.s    V_3
    IL_2f0b:  ldloc.s    V_2
    IL_2f0d:  ldc.i4.1
    IL_2f0e:  sub
    IL_2f0f:  dup
    IL_2f10:  stloc.s    V_2
    IL_2f12:  ldc.i4.s   16
    IL_2f14:  stelem.i4
    IL_2f15:  br         IL_1e99

    IL_2f1a:  ldarg.0
    IL_2f1b:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2f20:  ldloc.s    V_5
    IL_2f22:  ldloc.s    V_4
    IL_2f24:  dup
    IL_2f25:  ldc.i4.1
    IL_2f26:  add
    IL_2f27:  stloc.s    V_4
    IL_2f29:  ldelem.i4
    IL_2f2a:  ldc.i4.s   1
    IL_2f2c:  sub
    IL_2f2d:  dup
    IL_2f2e:  stloc.s    V_6
    IL_2f30:  ldc.i4.s   0
    IL_2f32:  blt.s      IL_2f5d

    IL_2f34:  ldloc.s    V_5
    IL_2f36:  ldloc.s    V_4
    IL_2f38:  dup
    IL_2f39:  ldc.i4.1
    IL_2f3a:  add
    IL_2f3b:  stloc.s    V_4
    IL_2f3d:  ldelem.i4
    IL_2f3e:  stloc.s    V_0
    IL_2f40:  ldloc.s    V_3
    IL_2f42:  ldloc.s    V_2
    IL_2f44:  ldc.i4.1
    IL_2f45:  sub
    IL_2f46:  dup
    IL_2f47:  stloc.s    V_2
    IL_2f49:  ldloc.s    V_6
    IL_2f4b:  stelem.i4
    IL_2f4c:  ldloc.s    V_3
    IL_2f4e:  ldloc.s    V_2
    IL_2f50:  ldc.i4.1
    IL_2f51:  sub
    IL_2f52:  dup
    IL_2f53:  stloc.s    V_2
    IL_2f55:  ldc.i4.s   11
    IL_2f57:  stelem.i4
    IL_2f58:  br         IL_1f6f

    IL_2f5d:  ldloc.s    V_5
    IL_2f5f:  ldloc.s    V_4
    IL_2f61:  ldloc.s    V_3
    IL_2f63:  ldloc.s    V_2
    IL_2f65:  dup
    IL_2f66:  ldc.i4.1
    IL_2f67:  add
    IL_2f68:  stloc.s    V_2
    IL_2f6a:  ldelem.i4
    IL_2f6b:  stelem.i4
    IL_2f6c:  ldloc.s    V_5
    IL_2f6e:  ldloc.s    V_4
    IL_2f70:  ldc.i4.1
    IL_2f71:  sub
    IL_2f72:  dup
    IL_2f73:  stloc.s    V_4
    IL_2f75:  ldloc.s    V_6
    IL_2f77:  stelem.i4
    IL_2f78:  br         IL_22d4

    IL_2f7d:  ldarg.0
    IL_2f7e:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2f83:  ldloc.s    V_3
    IL_2f85:  ldloc.s    V_2
    IL_2f87:  dup
    IL_2f88:  ldc.i4.1
    IL_2f89:  add
    IL_2f8a:  stloc.s    V_2
    IL_2f8c:  ldelem.i4
    IL_2f8d:  stloc.s    V_0
    IL_2f8f:  br         IL_216d

    IL_2f94:  ldarg.0
    IL_2f95:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2f9a:  ldloc.s    V_3
    IL_2f9c:  ldloc.s    V_2
    IL_2f9e:  dup
    IL_2f9f:  ldc.i4.1
    IL_2fa0:  add
    IL_2fa1:  stloc.s    V_2
    IL_2fa3:  ldelem.i4
    IL_2fa4:  stloc.s    V_0
    IL_2fa6:  ldloc.s    V_3
    IL_2fa8:  ldloc.s    V_2
    IL_2faa:  dup
    IL_2fab:  ldc.i4.1
    IL_2fac:  add
    IL_2fad:  stloc.s    V_2
    IL_2faf:  ldelem.i4
    IL_2fb0:  stloc.s    V_6
    IL_2fb2:  ldloc.s    V_6
    IL_2fb4:  ldc.i4.s   0
    IL_2fb6:  ble        IL_2126

    IL_2fbb:  ldloc.s    V_3
    IL_2fbd:  ldloc.s    V_2
    IL_2fbf:  ldc.i4.1
    IL_2fc0:  sub
    IL_2fc1:  dup
    IL_2fc2:  stloc.s    V_2
    IL_2fc4:  ldloc.s    V_6
    IL_2fc6:  ldc.i4.s   1
    IL_2fc8:  sub
    IL_2fc9:  stelem.i4
    IL_2fca:  ldloc.s    V_3
    IL_2fcc:  ldloc.s    V_2
    IL_2fce:  ldc.i4.1
    IL_2fcf:  sub
    IL_2fd0:  dup
    IL_2fd1:  stloc.s    V_2
    IL_2fd3:  ldloc.s    V_0
    IL_2fd5:  ldc.i4.s   1
    IL_2fd7:  sub
    IL_2fd8:  stelem.i4
    IL_2fd9:  ldloc.s    V_3
    IL_2fdb:  ldloc.s    V_2
    IL_2fdd:  ldc.i4.1
    IL_2fde:  sub
    IL_2fdf:  dup
    IL_2fe0:  stloc.s    V_2
    IL_2fe2:  ldc.i4.s   56
    IL_2fe4:  stelem.i4
    IL_2fe5:  br         IL_2126

    IL_2fea:  ldarg.0
    IL_2feb:  callvirt   instance void [System]System.Text.RegularExpressions.RegexRunner::CheckTimeout()
    IL_2ff0:  ldloc.s    V_3
    IL_2ff2:  ldloc.s    V_2
    IL_2ff4:  dup
    IL_2ff5:  ldc.i4.1
    IL_2ff6:  add
    IL_2ff7:  stloc.s    V_2
    IL_2ff9:  ldelem.i4
    IL_2ffa:  stloc.s    V_0
    IL_2ffc:  ldloc.s    V_3
    IL_2ffe:  ldloc.s    V_2
    IL_3000:  dup
    IL_3001:  ldc.i4.1
    IL_3002:  add
    IL_3003:  stloc.s    V_2
    IL_3005:  ldelem.i4
    IL_3006:  stloc.s    V_6
    IL_3008:  ldloc.s    V_6
    IL_300a:  ldc.i4.s   0
    IL_300c:  ble        IL_226a

    IL_3011:  ldloc.s    V_3
    IL_3013:  ldloc.s    V_2
    IL_3015:  ldc.i4.1
    IL_3016:  sub
    IL_3017:  dup
    IL_3018:  stloc.s    V_2
    IL_301a:  ldloc.s    V_6
    IL_301c:  ldc.i4.s   1
    IL_301e:  sub
    IL_301f:  stelem.i4
    IL_3020:  ldloc.s    V_3
    IL_3022:  ldloc.s    V_2
    IL_3024:  ldc.i4.1
    IL_3025:  sub
    IL_3026:  dup
    IL_3027:  stloc.s    V_2
    IL_3029:  ldloc.s    V_0
    IL_302b:  ldc.i4.s   1
    IL_302d:  sub
    IL_302e:  stelem.i4
    IL_302f:  ldloc.s    V_3
    IL_3031:  ldloc.s    V_2
    IL_3033:  ldc.i4.1
    IL_3034:  sub
    IL_3035:  dup
    IL_3036:  stloc.s    V_2
    IL_3038:  ldc.i4.s   57
    IL_303a:  stelem.i4
    IL_303b:  br         IL_226a
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
    IL_0001:  ldc.i4.s   104
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
    IL_0007:  ldstr      "\\$((([#]+|[\\^\\-\\+\\*\\~])\?(\\w+)|(\\w+\\.)*\\w+\\@[#\\-]*["
    + "\\w+\\.]+)\\$|[\\.\\w\\-]+\\:\\{|\\w(\\#\\d+(\\+)\?)\?(\\[[\\.\\w\\-]+\\])\?\\:([\\/\\.\\w\\-]+"
    + "\\$|[\\.\\w\\-]+\\:\\{|[\\.\\w\\-]+\\\?[\\.\\w\\-]+(\\,((\\|)\?([#\\.\\^\\-\\+\\*\\~]*([\\w+][^"
    + "\\$]*)|\\=([\\S+][^\\$]*)|(\\w+\\.)*\\w+\\@[#\\-]*[\\w+\\.]+)\?)*)\?\\$))|\\}\\:[\\.\\w\\-"
    + "]+\\:\\{|\\}\\:[\\.\\w\\-]+\\$"
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
