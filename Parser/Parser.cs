// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, John Gough, QUT 2005-2014
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.5.2
// Machine:  FEDERICOMEN4F80
// DateTime: 29/05/2018 11:02:11
// UserName: federicomengozzi
// Input file <grammar.g - 29/05/2018 11:02:08>

// options: lines

using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Text;
using QUT.Gppg;

public enum Tokens {error=2,EOF=3,ID=4,BOOLEAN_VALUE=5,INTEGER_VALUE=6,
    REAL_VALUE=7,RATIONAL_VALUE=8,COMPLEX_VALUE=9,STRING_VALUE=10,DOT=11,COMMA=12,
    COLON=13,SEMICOLON=14,STAR=15,SLASH=16,PLUS=17,MINUS=18,
    INTEGER=19,COMPLEX=20,RATIONAL=21,REAL=22,STRING=23,BOOLEAN=24,
    ASSIGN=25,ARROW=26,LESS=27,LESSEQUAL=28,GREATER=29,GREATEREQUAL=30,
    EQUAL=31,NOTEQUAL=32,AND=33,OR=34,XOR=35,FUNC=36,
    DO=37,RETURN=38,PRINT=39,IS=40,IF=41,THEN=42,
    ELSE=43,END=44,WHILE=45,FOR=46,IN=47,LOOP=48,
    BREAK=49,CONTINUE=50,LROUND=51,RROUND=52,LSQUARE=53,RSQUARE=54,
    LCURLY=55,RCURLY=56,ELLIPSIS=57,NEG=58};

[GeneratedCodeAttribute( "Gardens Point Parser Generator", "1.5.2")]
public class Parser: ShiftReduceParser<int, LexLocation>
{
#pragma warning disable 649
  private static Dictionary<int, string> aliases;
#pragma warning restore 649
  private static Rule[] rules = new Rule[108];
  private static State[] states = new State[204];
  private static string[] nonTerms = new string[] {
      "starting", "$accept", "dec_list", "declaration", "opt_type", "expr", "type", 
      "func_type", "tuple_type", "array_type", "map_type", "secondary", "primary", 
      "func_call", "indexer", "value", "cond", "func_def", "array_def", "map_def", 
      "tuple_def", "opt_params", "func_body", "param_list", "param", "stm_list", 
      "statement", "assignment", "if_stm", "loop_stm", "return_stm", "break_stm", 
      "cont_stm", "print_stm", "opt_exprs", "expr_list", "loop_header", "pair_list", 
      "pair", "tuple_elist", "tuple_elem", "type_list", };

  static Parser() {
    states[0] = new State(new int[]{4,7},new int[]{-1,1,-3,3,-4,5});
    states[1] = new State(new int[]{3,2});
    states[2] = new State(-1);
    states[3] = new State(new int[]{3,4});
    states[4] = new State(-2);
    states[5] = new State(new int[]{4,7,3,-3},new int[]{-3,6,-4,5});
    states[6] = new State(-4);
    states[7] = new State(new int[]{13,91,40,-6},new int[]{-5,8});
    states[8] = new State(new int[]{40,9});
    states[9] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,10,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[10] = new State(new int[]{14,11});
    states[11] = new State(-5);
    states[12] = new State(new int[]{51,13,27,165,28,167,29,169,30,171,31,173,32,175,33,177,34,179,35,181,17,183,18,185,15,187,16,189,57,191,53,81,11,84,14,-18,12,-18,52,-18,54,-18,42,-18,43,-18,44,-18,13,-18,56,-18,48,-18},new int[]{-15,50});
    states[13] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78,52,-71},new int[]{-35,14,-36,16,-6,58,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[14] = new State(new int[]{52,15});
    states[15] = new State(-70);
    states[16] = new State(new int[]{12,17,52,-72,54,-72});
    states[17] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,18,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[18] = new State(-74);
    states[19] = new State(-34);
    states[20] = new State(-37);
    states[21] = new State(-43);
    states[22] = new State(-44);
    states[23] = new State(-45);
    states[24] = new State(-46);
    states[25] = new State(-47);
    states[26] = new State(-48);
    states[27] = new State(-49);
    states[28] = new State(-38);
    states[29] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,30,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[30] = new State(new int[]{42,31});
    states[31] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,32,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[32] = new State(new int[]{43,33});
    states[33] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,34,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[34] = new State(new int[]{44,35});
    states[35] = new State(-50);
    states[36] = new State(-39);
    states[37] = new State(new int[]{51,38});
    states[38] = new State(new int[]{4,200,52,-52},new int[]{-22,39,-24,197,-25,203});
    states[39] = new State(new int[]{52,40});
    states[40] = new State(new int[]{13,91,37,-6,26,-6},new int[]{-5,41});
    states[41] = new State(new int[]{37,43,26,193},new int[]{-23,42});
    states[42] = new State(-51);
    states[43] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,90,41,123,36,37,53,55,55,60,51,69,46,139,45,144,38,147,49,152,50,155,39,158,48,-79},new int[]{-26,44,-27,163,-14,47,-12,49,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-28,131,-4,132,-29,133,-30,134,-37,135,-31,146,-32,151,-33,154,-34,157});
    states[44] = new State(new int[]{44,45,5,21,6,22,7,23,8,24,9,25,10,26,4,90,41,123,36,37,53,55,55,60,51,69,46,139,45,144,38,147,49,152,50,155,39,158,48,-79},new int[]{-27,46,-14,47,-12,49,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-28,131,-4,132,-29,133,-30,134,-37,135,-31,146,-32,151,-33,154,-34,157});
    states[45] = new State(-57);
    states[46] = new State(-60);
    states[47] = new State(new int[]{14,48,51,-35,25,-35,53,-35,11,-35,27,-35,28,-35,29,-35,30,-35,31,-35,32,-35,33,-35,34,-35,35,-35,17,-35,18,-35,15,-35,16,-35,57,-35,43,-35});
    states[48] = new State(-61);
    states[49] = new State(new int[]{51,13,25,51,53,81,11,84},new int[]{-15,50});
    states[50] = new State(-36);
    states[51] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,52,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[52] = new State(new int[]{14,53});
    states[53] = new State(-75);
    states[54] = new State(-40);
    states[55] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78,54,-71},new int[]{-35,56,-36,16,-6,58,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[56] = new State(new int[]{54,57});
    states[57] = new State(-88);
    states[58] = new State(-73);
    states[59] = new State(-41);
    states[60] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78,56,-90,12,-90},new int[]{-38,61,-39,89,-6,65,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[61] = new State(new int[]{56,62,12,63});
    states[62] = new State(-89);
    states[63] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-39,64,-6,65,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[64] = new State(-92);
    states[65] = new State(new int[]{13,66});
    states[66] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,67,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[67] = new State(-93);
    states[68] = new State(-42);
    states[69] = new State(new int[]{4,74,5,21,6,22,7,23,8,24,9,25,10,26,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-40,70,-41,88,-6,87,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[70] = new State(new int[]{52,71,12,72});
    states[71] = new State(-94);
    states[72] = new State(new int[]{4,74,5,21,6,22,7,23,8,24,9,25,10,26,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-41,73,-6,87,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[73] = new State(-96);
    states[74] = new State(new int[]{40,75,51,-49,27,-49,28,-49,29,-49,30,-49,31,-49,32,-49,33,-49,34,-49,35,-49,17,-49,18,-49,15,-49,16,-49,57,-49,53,-49,11,-49,52,-49,12,-49});
    states[75] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,76,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[76] = new State(-97);
    states[77] = new State(-35);
    states[78] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69},new int[]{-12,79,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[79] = new State(new int[]{58,80,51,13,53,81,11,84},new int[]{-15,50});
    states[80] = new State(-32);
    states[81] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,82,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[82] = new State(new int[]{54,83});
    states[83] = new State(-99);
    states[84] = new State(new int[]{4,85,6,86});
    states[85] = new State(-100);
    states[86] = new State(-101);
    states[87] = new State(-98);
    states[88] = new State(-95);
    states[89] = new State(-91);
    states[90] = new State(new int[]{13,91,51,-49,25,-49,53,-49,11,-49,27,-49,28,-49,29,-49,30,-49,31,-49,32,-49,33,-49,34,-49,35,-49,17,-49,18,-49,15,-49,16,-49,57,-49,43,-49,40,-6},new int[]{-5,8});
    states[91] = new State(new int[]{19,93,20,94,21,95,22,96,23,97,24,98,36,100,51,107,53,113,55,117},new int[]{-7,92,-8,99,-9,106,-10,112,-11,116});
    states[92] = new State(-7);
    states[93] = new State(-8);
    states[94] = new State(-9);
    states[95] = new State(-10);
    states[96] = new State(-11);
    states[97] = new State(-12);
    states[98] = new State(-13);
    states[99] = new State(-14);
    states[100] = new State(new int[]{51,101});
    states[101] = new State(new int[]{19,93,20,94,21,95,22,96,23,97,24,98,36,100,51,107,53,113,55,117},new int[]{-42,102,-7,122,-8,99,-9,106,-10,112,-11,116});
    states[102] = new State(new int[]{52,103,12,110});
    states[103] = new State(new int[]{13,104});
    states[104] = new State(new int[]{19,93,20,94,21,95,22,96,23,97,24,98,36,100,51,107,53,113,55,117},new int[]{-7,105,-8,99,-9,106,-10,112,-11,116});
    states[105] = new State(-102);
    states[106] = new State(-15);
    states[107] = new State(new int[]{19,93,20,94,21,95,22,96,23,97,24,98,36,100,51,107,53,113,55,117},new int[]{-42,108,-7,122,-8,99,-9,106,-10,112,-11,116});
    states[108] = new State(new int[]{52,109,12,110});
    states[109] = new State(-106);
    states[110] = new State(new int[]{19,93,20,94,21,95,22,96,23,97,24,98,36,100,51,107,53,113,55,117},new int[]{-7,111,-8,99,-9,106,-10,112,-11,116});
    states[111] = new State(-104);
    states[112] = new State(-16);
    states[113] = new State(new int[]{19,93,20,94,21,95,22,96,23,97,24,98,36,100,51,107,53,113,55,117},new int[]{-7,114,-8,99,-9,106,-10,112,-11,116});
    states[114] = new State(new int[]{54,115});
    states[115] = new State(-105);
    states[116] = new State(-17);
    states[117] = new State(new int[]{19,93,20,94,21,95,22,96,23,97,24,98,36,100,51,107,53,113,55,117},new int[]{-7,118,-8,99,-9,106,-10,112,-11,116});
    states[118] = new State(new int[]{13,119});
    states[119] = new State(new int[]{19,93,20,94,21,95,22,96,23,97,24,98,36,100,51,107,53,113,55,117},new int[]{-7,120,-8,99,-9,106,-10,112,-11,116});
    states[120] = new State(new int[]{56,121});
    states[121] = new State(-107);
    states[122] = new State(-103);
    states[123] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,124,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[124] = new State(new int[]{42,125});
    states[125] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,90,41,123,36,37,53,55,55,60,51,69,18,78,46,139,45,144,38,147,49,152,50,155,39,158,48,-79},new int[]{-6,32,-26,126,-12,164,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,47,-27,163,-28,131,-4,132,-29,133,-30,134,-37,135,-31,146,-32,151,-33,154,-34,157});
    states[126] = new State(new int[]{44,127,43,128,5,21,6,22,7,23,8,24,9,25,10,26,4,90,41,123,36,37,53,55,55,60,51,69,46,139,45,144,38,147,49,152,50,155,39,158,48,-79},new int[]{-27,46,-14,47,-12,49,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-28,131,-4,132,-29,133,-30,134,-37,135,-31,146,-32,151,-33,154,-34,157});
    states[127] = new State(-76);
    states[128] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,90,41,123,36,37,53,55,55,60,51,69,46,139,45,144,38,147,49,152,50,155,39,158,48,-79},new int[]{-26,129,-27,163,-14,47,-12,49,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-28,131,-4,132,-29,133,-30,134,-37,135,-31,146,-32,151,-33,154,-34,157});
    states[129] = new State(new int[]{44,130,5,21,6,22,7,23,8,24,9,25,10,26,4,90,41,123,36,37,53,55,55,60,51,69,46,139,45,144,38,147,49,152,50,155,39,158,48,-79},new int[]{-27,46,-14,47,-12,49,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-28,131,-4,132,-29,133,-30,134,-37,135,-31,146,-32,151,-33,154,-34,157});
    states[130] = new State(-77);
    states[131] = new State(-62);
    states[132] = new State(-63);
    states[133] = new State(-64);
    states[134] = new State(-65);
    states[135] = new State(new int[]{48,136});
    states[136] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,90,41,123,36,37,53,55,55,60,51,69,46,139,45,144,38,147,49,152,50,155,39,158,48,-79},new int[]{-26,137,-27,163,-14,47,-12,49,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-28,131,-4,132,-29,133,-30,134,-37,135,-31,146,-32,151,-33,154,-34,157});
    states[137] = new State(new int[]{44,138,5,21,6,22,7,23,8,24,9,25,10,26,4,90,41,123,36,37,53,55,55,60,51,69,46,139,45,144,38,147,49,152,50,155,39,158,48,-79},new int[]{-27,46,-14,47,-12,49,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-28,131,-4,132,-29,133,-30,134,-37,135,-31,146,-32,151,-33,154,-34,157});
    states[138] = new State(-78);
    states[139] = new State(new int[]{4,140,5,21,6,22,7,23,8,24,9,25,10,26,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,143,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[140] = new State(new int[]{47,141,51,-49,27,-49,28,-49,29,-49,30,-49,31,-49,32,-49,33,-49,34,-49,35,-49,17,-49,18,-49,15,-49,16,-49,57,-49,53,-49,11,-49,48,-49});
    states[141] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,142,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[142] = new State(-80);
    states[143] = new State(-81);
    states[144] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,145,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[145] = new State(-82);
    states[146] = new State(-66);
    states[147] = new State(new int[]{14,148,5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,149,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[148] = new State(-83);
    states[149] = new State(new int[]{14,150});
    states[150] = new State(-84);
    states[151] = new State(-67);
    states[152] = new State(new int[]{14,153});
    states[153] = new State(-85);
    states[154] = new State(-68);
    states[155] = new State(new int[]{14,156});
    states[156] = new State(-86);
    states[157] = new State(-69);
    states[158] = new State(new int[]{51,159});
    states[159] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78,52,-71},new int[]{-35,160,-36,16,-6,58,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[160] = new State(new int[]{52,161});
    states[161] = new State(new int[]{14,162});
    states[162] = new State(-87);
    states[163] = new State(-59);
    states[164] = new State(new int[]{51,13,27,165,28,167,29,169,30,171,31,173,32,175,33,177,34,179,35,181,17,183,18,185,15,187,16,189,57,191,25,51,53,81,11,84,43,-18},new int[]{-15,50});
    states[165] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,166,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[166] = new State(-19);
    states[167] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,168,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[168] = new State(-20);
    states[169] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,170,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[170] = new State(-21);
    states[171] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,172,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[172] = new State(-22);
    states[173] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,174,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[174] = new State(-23);
    states[175] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,176,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[176] = new State(-24);
    states[177] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,178,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[178] = new State(-25);
    states[179] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,180,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[180] = new State(-26);
    states[181] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,182,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[182] = new State(-27);
    states[183] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,184,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[184] = new State(-28);
    states[185] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,186,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[186] = new State(-29);
    states[187] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,188,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[188] = new State(-30);
    states[189] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,190,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[190] = new State(-31);
    states[191] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69},new int[]{-12,192,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[192] = new State(new int[]{51,13,53,81,11,84,14,-33,12,-33,52,-33,54,-33,42,-33,43,-33,44,-33,13,-33,56,-33,48,-33},new int[]{-15,50});
    states[193] = new State(new int[]{51,194});
    states[194] = new State(new int[]{5,21,6,22,7,23,8,24,9,25,10,26,4,27,41,29,36,37,53,55,55,60,51,69,18,78},new int[]{-6,195,-12,12,-13,19,-16,20,-17,28,-18,36,-19,54,-20,59,-21,68,-14,77});
    states[195] = new State(new int[]{52,196});
    states[196] = new State(-58);
    states[197] = new State(new int[]{12,198,52,-53});
    states[198] = new State(new int[]{4,200},new int[]{-25,199});
    states[199] = new State(-55);
    states[200] = new State(new int[]{13,201});
    states[201] = new State(new int[]{19,93,20,94,21,95,22,96,23,97,24,98,36,100,51,107,53,113,55,117},new int[]{-7,202,-8,99,-9,106,-10,112,-11,116});
    states[202] = new State(-56);
    states[203] = new State(-54);

    for (int sNo = 0; sNo < states.Length; sNo++) states[sNo].number = sNo;

    rules[1] = new Rule(-2, new int[]{-1,3});
    rules[2] = new Rule(-1, new int[]{-3,3});
    rules[3] = new Rule(-3, new int[]{-4});
    rules[4] = new Rule(-3, new int[]{-4,-3});
    rules[5] = new Rule(-4, new int[]{4,-5,40,-6,14});
    rules[6] = new Rule(-5, new int[]{});
    rules[7] = new Rule(-5, new int[]{13,-7});
    rules[8] = new Rule(-7, new int[]{19});
    rules[9] = new Rule(-7, new int[]{20});
    rules[10] = new Rule(-7, new int[]{21});
    rules[11] = new Rule(-7, new int[]{22});
    rules[12] = new Rule(-7, new int[]{23});
    rules[13] = new Rule(-7, new int[]{24});
    rules[14] = new Rule(-7, new int[]{-8});
    rules[15] = new Rule(-7, new int[]{-9});
    rules[16] = new Rule(-7, new int[]{-10});
    rules[17] = new Rule(-7, new int[]{-11});
    rules[18] = new Rule(-6, new int[]{-12});
    rules[19] = new Rule(-6, new int[]{-12,27,-6});
    rules[20] = new Rule(-6, new int[]{-12,28,-6});
    rules[21] = new Rule(-6, new int[]{-12,29,-6});
    rules[22] = new Rule(-6, new int[]{-12,30,-6});
    rules[23] = new Rule(-6, new int[]{-12,31,-6});
    rules[24] = new Rule(-6, new int[]{-12,32,-6});
    rules[25] = new Rule(-6, new int[]{-12,33,-6});
    rules[26] = new Rule(-6, new int[]{-12,34,-6});
    rules[27] = new Rule(-6, new int[]{-12,35,-6});
    rules[28] = new Rule(-6, new int[]{-12,17,-6});
    rules[29] = new Rule(-6, new int[]{-12,18,-6});
    rules[30] = new Rule(-6, new int[]{-12,15,-6});
    rules[31] = new Rule(-6, new int[]{-12,16,-6});
    rules[32] = new Rule(-6, new int[]{18,-12,58});
    rules[33] = new Rule(-6, new int[]{-12,57,-12});
    rules[34] = new Rule(-12, new int[]{-13});
    rules[35] = new Rule(-12, new int[]{-14});
    rules[36] = new Rule(-12, new int[]{-12,-15});
    rules[37] = new Rule(-13, new int[]{-16});
    rules[38] = new Rule(-13, new int[]{-17});
    rules[39] = new Rule(-13, new int[]{-18});
    rules[40] = new Rule(-13, new int[]{-19});
    rules[41] = new Rule(-13, new int[]{-20});
    rules[42] = new Rule(-13, new int[]{-21});
    rules[43] = new Rule(-16, new int[]{5});
    rules[44] = new Rule(-16, new int[]{6});
    rules[45] = new Rule(-16, new int[]{7});
    rules[46] = new Rule(-16, new int[]{8});
    rules[47] = new Rule(-16, new int[]{9});
    rules[48] = new Rule(-16, new int[]{10});
    rules[49] = new Rule(-16, new int[]{4});
    rules[50] = new Rule(-17, new int[]{41,-6,42,-6,43,-6,44});
    rules[51] = new Rule(-18, new int[]{36,51,-22,52,-5,-23});
    rules[52] = new Rule(-22, new int[]{});
    rules[53] = new Rule(-22, new int[]{-24});
    rules[54] = new Rule(-24, new int[]{-25});
    rules[55] = new Rule(-24, new int[]{-24,12,-25});
    rules[56] = new Rule(-25, new int[]{4,13,-7});
    rules[57] = new Rule(-23, new int[]{37,-26,44});
    rules[58] = new Rule(-23, new int[]{26,51,-6,52});
    rules[59] = new Rule(-26, new int[]{-27});
    rules[60] = new Rule(-26, new int[]{-26,-27});
    rules[61] = new Rule(-27, new int[]{-14,14});
    rules[62] = new Rule(-27, new int[]{-28});
    rules[63] = new Rule(-27, new int[]{-4});
    rules[64] = new Rule(-27, new int[]{-29});
    rules[65] = new Rule(-27, new int[]{-30});
    rules[66] = new Rule(-27, new int[]{-31});
    rules[67] = new Rule(-27, new int[]{-32});
    rules[68] = new Rule(-27, new int[]{-33});
    rules[69] = new Rule(-27, new int[]{-34});
    rules[70] = new Rule(-14, new int[]{-12,51,-35,52});
    rules[71] = new Rule(-35, new int[]{});
    rules[72] = new Rule(-35, new int[]{-36});
    rules[73] = new Rule(-36, new int[]{-6});
    rules[74] = new Rule(-36, new int[]{-36,12,-6});
    rules[75] = new Rule(-28, new int[]{-12,25,-6,14});
    rules[76] = new Rule(-29, new int[]{41,-6,42,-26,44});
    rules[77] = new Rule(-29, new int[]{41,-6,42,-26,43,-26,44});
    rules[78] = new Rule(-30, new int[]{-37,48,-26,44});
    rules[79] = new Rule(-37, new int[]{});
    rules[80] = new Rule(-37, new int[]{46,4,47,-6});
    rules[81] = new Rule(-37, new int[]{46,-6});
    rules[82] = new Rule(-37, new int[]{45,-6});
    rules[83] = new Rule(-31, new int[]{38,14});
    rules[84] = new Rule(-31, new int[]{38,-6,14});
    rules[85] = new Rule(-32, new int[]{49,14});
    rules[86] = new Rule(-33, new int[]{50,14});
    rules[87] = new Rule(-34, new int[]{39,51,-35,52,14});
    rules[88] = new Rule(-19, new int[]{53,-35,54});
    rules[89] = new Rule(-20, new int[]{55,-38,56});
    rules[90] = new Rule(-38, new int[]{});
    rules[91] = new Rule(-38, new int[]{-39});
    rules[92] = new Rule(-38, new int[]{-38,12,-39});
    rules[93] = new Rule(-39, new int[]{-6,13,-6});
    rules[94] = new Rule(-21, new int[]{51,-40,52});
    rules[95] = new Rule(-40, new int[]{-41});
    rules[96] = new Rule(-40, new int[]{-40,12,-41});
    rules[97] = new Rule(-41, new int[]{4,40,-6});
    rules[98] = new Rule(-41, new int[]{-6});
    rules[99] = new Rule(-15, new int[]{53,-6,54});
    rules[100] = new Rule(-15, new int[]{11,4});
    rules[101] = new Rule(-15, new int[]{11,6});
    rules[102] = new Rule(-8, new int[]{36,51,-42,52,13,-7});
    rules[103] = new Rule(-42, new int[]{-7});
    rules[104] = new Rule(-42, new int[]{-42,12,-7});
    rules[105] = new Rule(-10, new int[]{53,-7,54});
    rules[106] = new Rule(-9, new int[]{51,-42,52});
    rules[107] = new Rule(-11, new int[]{55,-7,13,-7,56});
  }

  protected override void Initialize() {
    this.InitSpecialTokens((int)Tokens.error, (int)Tokens.EOF);
    this.InitStates(states);
    this.InitRules(rules);
    this.InitNonTerminals(nonTerms);
  }

  protected override void DoAction(int action)
  {
#pragma warning disable 162, 1522
    switch (action)
    {
      case 2: // starting -> dec_list, EOF
#line 30 "grammar.g"
                           { Console.WriteLine("Ciao"); }
#line default
        break;
    }
#pragma warning restore 162, 1522
  }

  protected override string TerminalToString(int terminal)
  {
    if (aliases != null && aliases.ContainsKey(terminal))
        return aliases[terminal];
    else if (((Tokens)terminal).ToString() != terminal.ToString(CultureInfo.InvariantCulture))
        return ((Tokens)terminal).ToString();
    else
        return CharToString((char)terminal);
  }

}
