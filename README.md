# openprocurement-agent
> OpenProcurementAgent - service for filtering tenders in the  open database via openprocurement.api

[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-brightgreen.svg)](COPYING)
[![Build Status](https://dev.azure.com/oleksandr-nazaruk/openprocurement-agent/_apis/build/status/openprocurement-agent-CI)](https://dev.azure.com/oleksandr-nazaruk/openprocurement-agent/_apis/build/status/openprocurement-agent-CI)


## Compile and install
Once you have installed all the dependencies, get the code:

	git clone https://github.com/freehand-dev/openprocurement-agent.git
	cd openprocurement-agent

Then just use:

	sudo mkdir /opt/openprocurement-agent/bin
	dotnet restore
	dotnet build
	sudo dotnet publish --runtime linux-x64 --output /opt/openprocurement-agent/bin -p:PublishSingleFile=true -p:PublishTrimmed=true ./openprocurement-agent

Install as daemon
   
	sudo nano /etc/systemd/system/openprocurement-agent.service

The content of the file will be the following one

	[Unit]
	Description=OpenProcurement Agent 

	[Service]
	Type=notify
	WorkingDirectory=/opt/openprocurement-agent/etc/openprocurement-agent
	Restart=always
	RestartSec=10
	KillSignal=SIGINT
	ExecStart=/opt/openprocurement-agent/bin/openprocurement-agent
	Environment=ASPNETCORE_ENVIRONMENT=Production 

	[Install]
	WantedBy=multi-user.target

Add daemon to startup

	sudo systemctl daemon-reload
	sudo systemctl start openprocurement-agent
	sudo systemctl status openprocurement-agent
	sudo systemctl enable openprocurement-agent


## Configure and start
To start the server, you can use the `openprocurement-agent` executable as the application or `sudo systemctl start openprocurement-agent` as a daemon. For configuration you can edit a configuration file:

	sudo nano /opt/openprocurement-agent/etc/openprocurement-agent/openprocurement-agent.conf

The content of the file will be the following one

	[Global]
	# subtract startup offset from now in Hours for get tenders 
	Subtract=1 

	#
	[Action:SendMail]
	Username=sender@corp-mail.com
	Password=password
	Server=smtp.server.com
	Port=25
	EnableSsl=false
	MailTo:0=user@corp-mail.com
	MailTo:1=user1@corp-mail.com

	#
	[Action:TendersHistory]

	#
	[Transform:TendersHistory]

	#
	[Transform:Status]
	Allow:0=active.enquiries
	Allow:1=active.tendering

	#
	[Transform:Identifier]

	#
	[Logging:LogLevel]
	Default=Debug
	Microsoft=Warning