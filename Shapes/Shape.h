#pragma once

#include "../Painting.h"
#include "../Color/Color.h"
#include "../Canvas.h"

#include <opencv2/opencv.hpp>

class Shape {
    public:
        virtual float area() = 0;
        virtual void draw(Painting* painting, Color* color) = 0;
};