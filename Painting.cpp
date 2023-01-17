#include "Painting.h"
#include "Color/Color.h"

Painting::Painting() {
    _image = Mat::zeros(Size(255, 255), CV_8UC4);
}
Painting::Painting(Size size) {
    _image = Mat::zeros(size, CV_8UC4);
}

void Painting::drawPolygon(const Point** points, int nPoints, Color* color) {
    int lineType = LINE_8;

    int npt[] = { nPoints };

    fillPoly(
        _image,
        points,
        npt,
        1,
        color->getScalar(),
        lineType
    );
}