﻿using System;
using System.Collections.Generic;
using OpenMetaverse;
using BetterSecondBotShared.logs;

namespace BSB.Commands.Group
{
    public class GroupInvite : CoreCommand_2arg
    {
        public override string[] ArgTypes { get { return new[] { "UUID", "Avatar", "UUID" }; } }
        public override string[] ArgHints { get { return new[] { "GROUP", "Avatar [UUID or Firstname Lastname]", "Role" }; } }
        public override string Helpfile { get { return "Invites the avatar [ARG 2] to the Group [ARG 1] {optional with the Role [ARG 3]} otherwise to role Everyone"; } }

        protected void ForRealGroupInvite(string[] args,UUID group,UUID avatar)
        {
            UUID target_role = UUID.Zero;
            if (args.Length == 3)
            {
                if(UUID.TryParse(args[2], out target_role) == false)
                {
                    LogFormater.Warn("GroupInvite: Role uuid not vaild using everyone");
                }
            }            
            bot.GetClient.Groups.Invite(group, new List<UUID>() { target_role }, avatar);
        }
        public override bool CallFunction(string[] args)
        {
            if (base.CallFunction(args) == true)
            {
                if (UUID.TryParse(args[0], out UUID target_group) == true)
                {
                    if (UUID.TryParse(args[1], out UUID target_avatar) == true)
                    {
                        if (bot.MyGroups.ContainsKey(target_group) == true)
                        {
                            if (bot.GetAllowGroupInvite(target_avatar, target_group) == true)
                            {
                                bot.GroupInviteLockoutArm(target_avatar,target_group); // enable 120 sec cooldown
                                ForRealGroupInvite(args, target_group, target_avatar);
                                return true;
                            }
                            else
                            {
                                return Failed("Group invite to this avatar is on 120sec cooldown");
                            }
                        }
                        else
                        {
                            return Failed("I am not a member of that group");
                        }
                    }
                    else
                    {
                        return Failed("Invaild avatar UUID");
                    }
                }
                else
                {
                    return Failed("Invaild group UUID");
                }
            }
            return false;
        }
    }
}