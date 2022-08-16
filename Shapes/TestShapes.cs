
using static System.Console;

namespace Shapes
{
    public static class TestShapes
    {
        public static void Test()
        {

            var circle = new Circle(3);
            ShowShape(circle);
            var rectangle = new Rectangle(20, 25);
            ShowShape(rectangle);
            var square = new Square(18);
            ShowShape(square);
            square.Length = 55;
            ShowShape(square);
            rectangle = new Rectangle();
            ShowShape(rectangle);

        }

        static void ShowShape(Shape shape)
        {
            WriteLine($"{shape.GetType().Name}\tArea = {shape.Area}\t\tPerimeter = {shape.Perimeter}\t\tUom = {shape.UoM}");
        }

    }
}