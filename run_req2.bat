:run.bat
cd CommPrototype1029/Server/bin/Debug/
START Server.exe
cd../../../../

cd CommPrototype1029/Client_DemoDB/bin/Debug/
START Client.exe /R http://localhost:8080/CommService /L http://localhost:8070/CommService
cd../../../../











