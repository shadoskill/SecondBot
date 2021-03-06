
```
  _____                          _ _           _
 /  ___|                        | | |         | |
 \ `--.  ___  ___ ___  _ __   __| | |__   ___ | |_
  `--. \/ _ \/ __/ _ \| '_ \ / _` | '_ \ / _ \| __|
 /\__/ /  __/ (_| (_) | | | | (_| | |_) | (_) | |_
 \____/ \___|\___\___/|_| |_|\__,_|_.__/ \___/ \__|
```
# Secondbot
Secondbot is a CommandLine based bot for SecondLife based on libremetaverse retargeted for .net core.

[madpeter/secondbot on dockerhub](https://hub.docker.com/repository/docker/madpeter/secondbot)


### Status
---

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/1945bad2070d4421adc9c6266dadb237)](https://www.codacy.com/manual/madpeter/SecondBot?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Madpeterz/SecondBot&amp;utm_campaign=Badge_Grade)

[![CII Best Practices](https://bestpractices.coreinfrastructure.org/projects/3765/badge)](https://bestpractices.coreinfrastructure.org/projects/3765)


#### Helpfull stuff
---
[Commands Wiki](https://wiki.magicmadpeter.xyz/)

[Web UI](https://webui.magicmadpeter.xyz/)
- note: if you running in windows and have HTTP enabled you will want to add HTTP support
via a command promt with the following command where 8185 is the port you wish to use.
"netsh http add urlacl url=http://*:8185/ user=Everyone"

## Releases
These are pre compiled versions for the most upto date please use docker or compile it yourself.

[View releases](https://github.com/Madpeterz/SecondBot/releases)
 
 
### Starting multiple bots from the same executable in windows
---
Create a shortcut (rename it for the bot) and 'madpeterbot' after the end of the target

so the target would look something like this: BetterSecondBot.exe" madpeterbot

save changes to the shortcut and your ready to go.


### Custom commands
---
Custom commands allow you to create a chain of commands triggered by one command

Format

    <Custom command name>[!!!]<corelib command 1>[{-}]<corelib command 2>[{-}]...

example command

    sayexample!!!say|||Hello{-}delay|||2500{-}say|||Bye


if your using docker to create a custom command
you would create a new env value that starts with "cmd_" followed by the custom command name
example: `cmd_sayexample`

and the value would be the actions
example: `say|||Hello{-}say|||Bye`

If you want to pass custom args
set `[C_ARG_1]` (this goes up to 5)

and the command would look something like this

    sayexample!!!say|||Hello [C_ARG_1]{-}delay|||2500{-}say|||Bye [C_ARG_2]

### HTTP web interface

by setting

> Http_Enable to true
> Http_Host to http://*:8080
> Security_WebUIKey to a vaild code (12 letters+numbers long or more)

you will be able to connect to the bot via HTTP and use the webUI
to control the bot!

[Dev server UI](http://webui.magicmadpeter.xyz/)
or 
[Host it yourself](https://github.com/Madpeterz/secondbot_web_folders)

###  HTTP scoped tokens
---
Scoped tokens allow you to hardcode access to the HTTP interface (if enabled)
and set more detailed control over what areas can be accessed
plus no need to give the full http 

> Security_WebUIKey 

You get set these tokens up by file or Environment Variables.
##### File

> {   
> "ScopedTokens": [
>     "t:[10charcode],ws:core,ws:group",
>     "t:[10charcode],cm:chat/localchathistory",   
>     ]
>   }


##### Environment Variables
|Name  |  Value|
|--|--|
| ScopedToken1 |  "t:[10charcode],ws:core,ws:group" |
| ScopedToken2 |  "t:[10charcode],cg:chat" |
| ScopedToken3 |  "t:[10charcode],cm:chat/LocalChatHistory" |


###  HTTP scoped tokens (info)
---

    command [cm] "example: `cm:chat/LocalChatHistory`
        "a single command, formated as cm:[workspace]/[commandname]"
    
    workspaces [ws] "example: ws: core, ws: group"
        "all commands under a workspace"

        "known workspaces"
        animation
        avatars
        chat
        core
        dialogs
        estate
        friends
        group
        home
        im
        info
        inventory
        movement
        notecard
        parcel
        self
        streamadmin
        +ALL
    
    commandgroups [cg] "example: `cg:chat`"
        chat
            chat/IM
            chat/LocalChatHistory
            chat/Say
            group/Groupchat
            group/GetGroupList
            group/GroupchatListAllUnreadGroups
            im/getimchat
            im/chatwindows
            im/haveunreadims
            im/listwithunread

        giver
            inventory/SendItem
            inventory/SendFolder
            inventory/InventoryFolders
            inventory/InventoryContents

        movement
            movement/AutoPilot
            movement/AutoPilotStop
            movement/Fly
            movement/RequestTeleport
            movement/RotateTo
            movement/RotateToFace
            movement/RotateToFaceVector
            movement/SendTeleportLure
            movement/Teleport


###  BetterRelay system
---
This is to replace the old broken relay thats currently built into the bot


> CustomRelay_1 = "encode-as-json::true,source-type::discord,source-filter::123451231235@12351312321,target-type::localchat,target-config::4"

> CustomRelay_2 = "encode-as-json::false,source-type::discord,source-filter::123451231235@12351312321,target-type::localchat,target-config::4"

or via the config file
customrelays

>{
>"CustomRelays": [
>	encode-as-json::false,source-type::discord,source-filter::123451231235@12351312321,target-type::chat,target-config::4
>]
}

To config the relay please use the settings below.


    source-type
	    discord
		    source-filter: serverid@serverchannel
	
	    localchat
		    source-filter: talker uuid or "all"

	    avatarim
		    source-filter: avatar uuid or "all"

	    objectim
		    source-filter: object uuid or "all"

	    groupchat
		    source-filter: group uuid or "all"


    target-type
	    discord
		    target-config: serverid@serverchannel
	
	    localchat
		    target-config: channel (default is 0)

	    avatarchat
		    target-config: avatar uuid

	    groupchat
		    target-config: group uuid

        http
            target-config: full url