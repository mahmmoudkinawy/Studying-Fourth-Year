import { LL1Parser } from "ll1-parser";

const p = new LL1Parser();

p.AddRule("E", "(D+E)");
p.AddRule("S", "+");
p.AddRule("E", "N");
p.AddRule("D", "0");
p.AddRule("D", "1");
p.AddRule("D", "2");
p.AddRule("N", "n");
p.AddRule("N", "DN");

p.Compile();

console.table(p.DumpTransitionTable());
