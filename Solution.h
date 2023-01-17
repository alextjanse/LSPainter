#pragma once

#include "Shapes/Shape.h"
#include "Painting.h"

#include <opencv2/opencv.hpp>
#include <string>

using namespace std;
using namespace cv;

class Solution {
    private:
        int _score;
        Painting _painting;

    public:
        Solution();
        Solution(Size size);

        int tryShape(Shape* shape, Color* color);
        void applyShape(Shape* shape, Color* color);
};