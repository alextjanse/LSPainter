project(LSPainter)

file(GLOB SOURCE_FILES
    ${PROJECT_SOURCE_DIR}/*.cpp
    ${PROJECT_SOURCE_DIR}/Color/*.cpp
    ${PROJECT_SOURCE_DIR}/Shapes/*.cpp
)

add_executable(LSPainter ${SOURCE_FILES})

find_package( OpenCV REQUIRED )

include_directories( ${OpenCV_INCLUDE_DIRS} )
target_link_libraries( LSPainter ${OpenCV_LIBS} )