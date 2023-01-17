#pragma once

#include <opencv2/opencv.hpp>

#include <string>

using namespace std;
using namespace cv;

class Image {
    private:
        Mat _image;
        int _width, _height;

    public:
        Image();
        Image(string file_path);

        int getWidth() { return _width; }
        int getHeight() { return _height; }
        Size getSize() { return _image.size(); }
        Mat* getImage() { return &_image; }
};
