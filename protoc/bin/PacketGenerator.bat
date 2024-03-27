pushed %~dp0

protoc.exe --proto_path=./ --cpp_out=./ ./Protocol.proto
IF ERRORLEVEL 1 PAUSE

XCOPY /Y Protocol.pb.cc "../../Server"
XCOPY /Y Protocol.pb.h "../../Server"

XCOPY /Y Protocol.pb.cc "../../Client"
XCOPY /Y Protocol.pb.h "../../Client"