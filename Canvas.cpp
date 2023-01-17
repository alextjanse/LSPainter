#include "Canvas.h"

Canvas::Canvas(string canvas_name) {
    _canvas_name = canvas_name;
    
    namedWindow(canvas_name, WINDOW_AUTOSIZE);
}


void Canvas::drawImage(Mat* image) {
    imshow(_canvas_name, *image);
}
