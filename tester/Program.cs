using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using de.fearvel.io;
using de.fearvel.io.File;
using de.fearvel.io.FnPak;

namespace tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new FnPakManager();
            //a.OpenFnPak(@"C:\Users\schreiner.andreas\Code\GitHub\TestPlugin.zip", "plugins");
            Encryption.EncryptFile(@"C:\Users\schreiner.andreas\Code\GitHub\TestPlugin.zip", @"C:\Users\schreiner.andreas\Code\GitHub\TestPlugin.fnPak", "adadwqeq");
            a.OpenFnPak(@"C:\Users\schreiner.andreas\Code\GitHub\TestPlugin.fnPak", "TEST", "adadwqeq");
        }
    }
}
