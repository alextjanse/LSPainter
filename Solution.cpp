#include "Solution.h"

Solution::Solution() {
    _score = 0;
    _painting = Painting();
}

Solution::Solution(Size size) {
    _score = 0;
    _painting = Painting(size);
}

int Solution::tryShape(Shape* shape, Color* color) {
    return 0;
}

void Solution::applyShape(Shape* shape, Color* color) {
    shape->draw(&_painting, color);
}