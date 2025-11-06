compile:
	dotnet build -c Release
install:
	/usr/bin/install -d ./debian/tmp
	/usr/bin/install -d ./debian/tmp/usr/games/cbriscola
	/usr/bin/install -d ./debian/tmp/usr/share/applications
	/usr/bin/install ./bin/Release/net6.0/cbriscola ./debian/tmp/usr/games/cbriscola/cbriscola
	/usr/bin/install ./bin/Release/net6.0/cbriscola.dll ./debian/tmp/usr/games/cbriscola/cbriscola.dll
	/usr/bin/install ./bin/Release/net6.0/cbriscola.runtimeconfig.json ./debian/tmp/usr/games/cbriscola/cbriscola.runtimeconfig.json
	/usr/bin/install ../cbriscola.desktop ./debian/tmp/usr/share/applications/cbriscola.desktop
