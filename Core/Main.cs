using System;
using Con = ePassword.Api.Console.log;
using System.Threading.Tasks;
using System.Threading;
using ePassword.Api;
using System.Text;

namespace ePassword.Core
{
 
    internal class Main
    {
        internal static void Startup()
        {
            Thread.Sleep(4000);
            Build();
            Console.ReadLine();
        }

        internal static void Build()
        {
            Console.Clear();
            Thread.Yield();
            Con.Write("User: " + Environment.UserName);
            Thread.Sleep(2000);
            BuildPasswords();
        }
        static void OpenFile(string[] args)
        {
            if (args.Length == 0) { Console.ReadLine(); }
            string str = Console.ReadLine(); ;
            string path = "documents/ePasswords/passwords.txt";
            Con.Write("Would you like to save these passwords?");
            if (str == "false" || str == "") { Con.Write("Press Enter!"); Console.ReadLine(); }
            Con.Write("Saving Passwords to" + path + ".");
        }

        async static void BuildPasswords()
        {
            int PasswordAmount = 0;
            bool v = false;
            Con.Write(Environment.UserName + " How many should we make?");
            PasswordAmount = int.Parse(Console.ReadLine());
            Con.Write("Creating Passwords...");
            string[] AllPasswords = new string[PasswordAmount];
            for (int i = 0; i < PasswordAmount; i++)
            {
                Console.Title = "Generating [" + PasswordAmount + "] Passwords";
                if (PasswordAmount == 1) { Console.Title = "Generating [" + PasswordAmount + "] Password"; }
                StringBuilder sb = new StringBuilder();
                string password = "";
                PasswordGenerator pwdGen1 = new PasswordGenerator(includeLowercase: true, includeUppercase: true, includeNumeric: true, includeSpecial: true, passwordLength: 35);
                password = pwdGen1.Next();
                PasswordGenerator pwdGen2 = new PasswordGenerator(includeLowercase: false, includeUppercase: true, includeNumeric: true, includeSpecial: true, passwordLength: 35);
                password = pwdGen2.Next();
                PasswordGenerator pwdGen3 = new PasswordGenerator(includeLowercase: true, includeUppercase: true, includeNumeric: true, includeSpecial: true, passwordLength: 35);
                password = pwdGen3.Next();
                PasswordGenerator pwdGen4 = new PasswordGenerator(includeLowercase: true, includeUppercase: true, includeNumeric: true, includeSpecial: true, passwordLength: 35);
                password = pwdGen4.Next();
                PasswordGenerator pwdGen5 = new PasswordGenerator(includeLowercase: true, includeUppercase: true, includeNumeric: true, includeSpecial: true, passwordLength: 35);
                password = pwdGen5.Next();
                PasswordGenerator pwdGen6 = new PasswordGenerator(includeLowercase: true, includeUppercase: true, includeNumeric: true, includeSpecial: true, passwordLength: 35);
                password = pwdGen6.Next();
                AllPasswords[i] = password.ToString();
            }

            foreach (string singlePassword in AllPasswords)
            {
                await Task.Delay(PasswordAmount + 100);
                Con.Write("Password: " + singlePassword);
            }
        }
    }
}