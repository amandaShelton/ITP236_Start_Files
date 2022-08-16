using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI
{
    public static class Extension
    {
        public static string Left(this string name, int length)
        {
            if (name == null)
                return null;
            int stringLength = name.Length;
            if (length >= stringLength)
                return name;
            return name.Substring(0, length);
        }
        public static string Right(this string name, int length)
        {
            if (name == null)
                return null;
            int stringLength = name.Length;
            if (length >= stringLength)
                return name;
            return name.Substring(stringLength - length, length);
        }
    }
    public class ExtensionTutorial
    {
        public ExtensionTutorial()
        {
            var name = "Bob Dust";
            var firstName = name.Left(3);
            var lastName = name.Right(4);
            Console.WriteLine($"Name: {name}\t\tFirstName: {firstName}");
            Console.WriteLine($"LastName: {lastName}");
        }
    }
}
