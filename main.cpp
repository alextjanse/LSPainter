#include "LSPainter.h"
#include <opencv2/opencv.hpp>

#include <string>

using namespace std;
using namespace cv;

int main(int argc, char** argv) {
    LSPainter painter = LSPainter("Mona Lisa", "mona_lisa.png");

    painter.run();

    waitKey(0);

    return 0;
}
