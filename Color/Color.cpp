#include "Color.h"

Color::Color() {
    _r = 0;
    _g = 0;
    _b = 0;
    _a = 0;
}

Color::Color(int r, int g, int b) {
    _r = r;
    _g = g;
    _b = b;
    _a = 255;
}

Color::Color(int r, int g, int b, int a) {
    _r = r;
    _g = g;
    _b = b;
    _a = a;
}