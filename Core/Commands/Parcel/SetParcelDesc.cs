﻿using System;
using System.Collections.Generic;
using System.Text;
using BetterSecondBotShared.logs;
using BetterSecondBotShared.Static;
using OpenMetaverse;

namespace BSB.Commands.CMD_Parcel
{

    public class SetParcelDesc : CoreCommand
    {
        public override string[] ArgTypes { get { return new[] { "String" }; } }
        public override string[] ArgHints { get { return new[] { "The new desc" }; } }
        public override int MinArgs { get { return 1; } }
        public override string Helpfile { get { return "Updates the current parcels desc"; } }

        public override bool CallFunction(string[] args)
        {
            if (base.CallFunction(args) == true)
            {
                int localid = bot.GetClient.Parcels.GetParcelLocalID(bot.GetClient.Network.CurrentSim, bot.GetClient.Self.SimPosition);
                Parcel p = bot.GetClient.Network.CurrentSim.Parcels[localid];
                if (parcel_static.has_parcel_perm(p, bot) == true)
                {
                    p.Desc = args[0];
                    p.Update(bot.GetClient.Network.CurrentSim, false);
                    return true;
                }
                else
                {
                    return Failed("Incorrect perms to control parcel");
                }
            }
            return false;
        }
    }
}
