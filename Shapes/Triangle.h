#pragma once

#include "../Painting.h"
#include "Shape.h"
#include "../Solution.h"
#include <opencv2/opencv.hpp>

using namespace cv;

class Triangle : public Shape {
    private:
        Point _p1, _p2, _p3;

    public:
        Triangle(Point p1, Point p2, Point p3);

        float area() override;
        void draw(Painting* painting, Color* color) override;
};