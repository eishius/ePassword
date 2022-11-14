using System;
using Con = ePassword.Api.Console.log;
using System.Threading.Tasks;
using System.Threading;
using ePassword.Api;
using System.Text;
using System.Diagnostics;
using System.IO;
namespace ePassword.Core
{
    // Developed by Eishius.
    internal class Main
    {
        internal static void Startup()
        {
            Thread.Sleep(1000);
            Con.Write("User: " + Environment.UserName);
            Thread.Sleep(2000);
            BuildPasswords();
            Console.ReadLine();
        }

        async static void BuildPasswords()
        {
            Console.Clear();
            int PasswordAmount = 0;
            Con.Write(Environment.UserName + " How many should we make?");
            PasswordAmount = int.Parse(Console.ReadLine());
            Thread.Sleep(1000);
            Con.Write("Creating Passwords...");
            Console.Clear();
            string[] AllPasswords = new string[PasswordAmount];
            for (int i = 0; i < PasswordAmount; i++)
            {
                Console.Title = "Generating [" + PasswordAmount + "] Passwords";
                if (PasswordAmount == 1) { Console.Title = "Generating [" + PasswordAmount + "] Password"; }
                StringBuilder sb = new StringBuilder();
                string password = "";
                PasswordGenerator pwdGen1 = new PasswordGenerator(includeLowercase: true, includeUppercase: true, includeNumeric: true, includeSpecial: true, passwordLength: 35);
                password = pwdGen1.Next();
                PasswordGenerator pwdGen2 = new PasswordGenerator(includeLowercase: true, includeUppercase: true, includeNumeric: true, includeSpecial: true, passwordLength: 35);
                password = pwdGen2.Next();
                AllPasswords[i] = password.ToString();
            }
            foreach (string singlePassword in AllPasswords)
            {
                await Task.Delay(100);
                Con.Write("Password: " + singlePassword);
                try
                {
                    var fileName = Directory.GetCurrentDirectory() + "/ePassword.eishius";
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
                    File.WriteAllLines(fileName, AllPasswords);
                }
                catch (Exception e)
                {
                    Console.Write(e.ToString());
                }
            }
        }
    }
}