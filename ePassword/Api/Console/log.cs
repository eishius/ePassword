using Con = System.Console;
using System;

namespace ePassword.Api.Console
{
    internal class log
    {
        internal static void Write(string Msg, ConsoleColor TextColor = ConsoleColor.White, ConsoleColor NameColor = ConsoleColor.Green)
        {
            Con.ForegroundColor = ConsoleColor.White;
            Con.Write("(");
            Con.ForegroundColor = ConsoleColor.Cyan;
            Con.Write(DateTime.Now.ToString("yyyy-MM-dd @ hh.mm.ss"));
            Con.ForegroundColor = ConsoleColor.White;
            Con.Write(") [");
            Con.ForegroundColor = NameColor;
            Con.Write("ePassword");
            Con.ForegroundColor = ConsoleColor.White;
            Con.Write("]: ");
            Con.Write(Msg);
            Con.ResetColor();
            Con.WriteLine();
        }

        internal static void Info(string Msg, ConsoleColor TextColor = ConsoleColor.White, ConsoleColor NameColor = ConsoleColor.Cyan)
        {
            Con.ForegroundColor = ConsoleColor.White;
            Con.Write("(");
            Con.ForegroundColor = ConsoleColor.Cyan;
            Con.Write(DateTime.Now.ToString("yyyy-MM-dd @ hh.mm.ss"));
            Con.ForegroundColor = ConsoleColor.White;
            Con.Write(") [");
            Con.ForegroundColor = NameColor;
            Con.Write("Info");
            Con.ForegroundColor = ConsoleColor.White;
            Con.Write("]: ");
            Con.Write(Msg);
            Con.ResetColor();
            Con.WriteLine();
        }
    }
}
