#include "LSPainter.h"

LSPainter::LSPainter(string painting_name, string file_name) {
    _painting_name = painting_name;

    string file_path = "../images/" + file_name;

    _original_image = Image(file_path);

    Size size = _original_image.getSize();

    _current_solution = Solution(size);

    _original_canvas = Canvas("Original " + _painting_name);
    _solution_canvas = Canvas("My " + _painting_name);
}

void LSPainter::run() {
    Triangle triangle = generateShape();
    Color color = generateColor();
    draw(&triangle, &color);
}

Triangle LSPainter::generateShape() {
    Point p1 = Point(10, 10);
    Point p2 = Point(10, 100);
    Point p3 = Point(100, 10);

    return Triangle(p1, p2, p3);
}

Color LSPainter::generateColor() {
    return Color(255, 0, 0);
}

void LSPainter::draw(Shape* shape, Color* color) {
    _current_solution.applyShape(shape, color);
}

void printSolution() {
    // TODO
}
