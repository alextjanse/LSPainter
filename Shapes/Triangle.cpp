#include "Triangle.h"
#include <cstdlib>

Triangle::Triangle(Point p1, Point p2, Point p3) {
    _p1 = p1;
    _p2 = p2;
    _p3 = p3;
}

float Triangle::area() {
    return 0.5f * float(abs(_p1.x * (_p2.y - _p3.y) + _p2.x * (_p3.y - _p1.y) + _p3.x * (_p1.y - _p2.y)));
}

void Triangle::draw(Painting* painting, Color* color) {
    int npts = 3;

    Point* points[] = { &_p1, &_p2, &_p3 };
    
    painting->drawPolygon(&points, npts, color);
}