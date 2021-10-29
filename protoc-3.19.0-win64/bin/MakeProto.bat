protoc -I=./ --csharp_out=./ ./Protobuf.proto

XCOPY /Y Protobuf.cs "../../ServerCore"