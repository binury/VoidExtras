The Void's Extras is a server plugin for the webfishing dedicated server software known as Cove, it offers a few easy to set up text display commands (!Rules and !Discord) as well as a few fun commands as of now.

<h1>Installation</h1>

after downloading both VoidExtras.dll and voidsettings.json, put VoidExtras into the plugins directory as-per-usual but place voidsettings.json into the same directory as your cove executable file (the same place that server.cfg and bans.txt are also present). An issue that MAY occur is when Cove uses the plugins directory but at the same time starts in a different directory entirely, causing a FileNotFoundException to occur for voidsettings.json or voidfilterlog.txt, you can fix this by simply placing those files in the directory it wants to get it from (E.G. Starting up a Cove server with the Task Scheduler from Microsoft Management Console causes it to start up in the system32 directory by default)
