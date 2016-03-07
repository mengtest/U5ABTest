@IF NOT EXIST ProtoColProject\ (
@ECHO 未设置工程文件夹
@PAUSE
@EXIT
)

@SET CSC20=c:\Windows\Microsoft.NET\Framework\v2.0.50727\csc.exe

proto_conv\proto_conv.exe excels\ProtoTest.xlsx

@%CSC20% /out:m-client-proto.dll /target:library /reference:..\ProtoColProject\Assets\Libs\protobuf-net.dll /debug- /optimize+ code\*.cs
@xcopy m-client-proto.dll ..\ProtoColProject\Assets\Libs\ /Y /Q
@FOR %%P IN (protodata\*) DO xcopy %%P ..\ProtoColProject\Assets\Resources\ /Y /Q

@pause