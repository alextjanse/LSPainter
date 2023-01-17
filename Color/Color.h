#pragma once

#include <opencv2/opencv.hpp>

using namespace cv;

class Color {
    private:
        int _r, _g, _b, _a;

    public:
        Color();
        Color(int r, int g, int b);
        Color(int r, int g, int b, int a);

        Scalar getScalar() { return Scalar(_b, _g, _a); }
};