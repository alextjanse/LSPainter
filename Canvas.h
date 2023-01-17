#pragma once

#include "Image.h"
#include "Color/Color.h"

#include <opencv2/opencv.hpp>

#include <string>

using namespace std;
using namespace cv;

class Canvas {
    private:
        string _canvas_name;

    public:
        Canvas(string canvas_name = "Canvas");

        void drawImage(Mat* image);
};