using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collection
{
    #region Student
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public float GPA { get; set; }
        public static List<Student> Students
        {
            get
            {
                var list = new List<Student>();
                //--< Add code here >--//
                return list;
            }
        }
        public static Dictionary<int, Student> StudentDictionary
        {
            get
            {
                var dictionary = new Dictionary<int, Student>();
                //--< Add code here >--//

                return dictionary;
            }
        }
        public static Stack<Student> StudentStack
        {
            get
            {
                var stack = new Stack<Student>();
                //--< Add code here >--//
                return stack;
            }
        }
        public static Queue<Student> StudentQueue
        {
            get
            {
                var queue = new Queue<Student>();
                //--< Add code here >--//
                return queue;
            }
        }
        private static Student[] students =
        {
            new Student(101, "Skip Wythe", 3.43f),
            new Student(108, "James River", 2.89f),
            new Student(107, "Maggie Walker", 3.49f),
            new Student(105, "Tre deGar", 2.77f),
            new Student(109, "Chip N. Ham", 3.08f)
        };
        private static void Playground()
        {
            var student = students[0];
            student = students[4];
        }
        public Student(int studentId, string name, float gpa)
        {
            StudentId = studentId;
            Name = name;
            GPA = gpa;
        }
    }
    #endregion
}
