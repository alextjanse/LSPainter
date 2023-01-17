#pragma once

#include "Canvas.h"
#include "Image.h"
#include "Solution.h"
#include "Shapes/Triangle.h"
#include "Shapes/Shape.h"

#include <opencv2/opencv.hpp>
#include <string>

using namespace std;

class LSPainter {
    private:
        string _painting_name;
        Image _original_image;
        Solution _current_solution;

        Canvas _original_canvas, _solution_canvas;

    public:
        LSPainter(string painting_name, string file_name);

        void run();
        Triangle generateShape();
        Color generateColor();
        void draw(Shape* shape, Color* color);
        void printSolution();
};