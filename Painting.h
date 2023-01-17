#pragma once

#include "Color/Color.h"
#include <opencv2/opencv.hpp>
#include <string>

using namespace std;
using namespace cv;

class Painting {
    private:
        Mat _image;

    public:
        Painting();
        Painting(Size size);
        void drawPolygon(Point** points, int nPoints, Color* color);
};