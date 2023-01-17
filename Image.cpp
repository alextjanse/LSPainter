#include "Image.h"

Image::Image() {
    _image = imread("./images/mona_lisa.png", IMREAD_COLOR);

    if (!_image.data)
    {
        printf("No image data \n");
    }

    Size s = _image.size();

    _width = s.width;
    _height = s.height;
}

Image::Image(string file_path) {
    _image = imread(file_path, IMREAD_COLOR);

    if (!_image.data)
    {
        printf("No image data \n");
    }

    Size s = _image.size();

    _width = s.width;
    _height = s.height;
}
