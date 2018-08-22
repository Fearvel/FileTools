using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using de.fearvel.io;
namespace tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = "testdir";
            var a = new FnPakManager();
            //a.OpenFnPak(@"C:\Users\schreiner.andreas\Code\GitHub\TestPlugin.zip", "plugins");
            //FileTools.EncryptFile(@"C:\Users\schreiner.andreas\Code\GitHub\TestPlugin.zip", @"C:\Users\schreiner.andreas\Code\GitHub\TestPlugin.fnPAK", "adadwqeq");
            a.OpenFnPak(@"C:\Users\schreiner.andreas\Code\GitHub\TestPlugin.fnPAK", "TEST", "adadwqeq");
        }
    }
}
