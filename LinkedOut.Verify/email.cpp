
#include <regex>
using namespace std;

extern "C" __declspec(dllexport)bool email(char* s)
{
    const regex pattern(R"(([0-9A-Za-z\-_\.]+)@([0-9a-z]+\.[a-z]{2,3}(\.[a-z]{2})?))");
    return regex_match( s,pattern);
}
