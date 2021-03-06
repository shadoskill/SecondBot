﻿using BetterSecondBot.bottypes;
using BetterSecondBot.Static;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using System;
using System.Collections.Generic;
using System.Text;
using OpenMetaverse;
using Newtonsoft.Json;
using BetterSecondBotShared.Static;

namespace BetterSecondBot.HttpService
{
    public class Http_Funds : WebApiControllerWithTokens
    {
        public Http_Funds(SecondBot mainbot, TokenStorage setuptokens) : base(mainbot, setuptokens) { }

        [About("Requests the current balance and requests the balance to update.")]
        [ReturnHints("Current fund level")]
        [ReturnHints("Funds commands are disabled")]
        [Route(HttpVerbs.Get, "/Balance/{token}")]
        public object Balance(string token)
        {
            if (tokens.Allow(token, "funds", "Balance", handleGetClientIP()) == false)
            {
                return Failure("Token not accepted");
            }
            if (bot.GetAllowFunds == false)
            {
                return Failure("Funds commands are disabled");
            }
            bot.GetClient.Self.RequestBalance();
            return BasicReply(bot.GetClient.Self.Balance.ToString());
        }

        [About("Makes the bot pay a avatar")]
        [ReturnHints("Accepted")]
        [ReturnHints("avatar lookup")]
        [ReturnHints("Invaild amount")]
        [ReturnHints("Transfer funds to avatars disabled")]
        [Route(HttpVerbs.Get, "/PayAvatar/{avatar}/{amount}/{token}")]
        public object PayAvatar(string avatar, string amount, string token)
        {
            if (tokens.Allow(token, "funds", "PayAvatar", handleGetClientIP()) == false)
            {
                return BasicReply("Token not accepted");
            }
            if (bot.GetAllowFunds == false)
            {
                return BasicReply("Transfer funds to avatars disabled");
            }
            ProcessAvatar(avatar);
            if (avataruuid == UUID.Zero)
            {
                return BasicReply("avatar lookup");
            }
            if (int.TryParse(amount, out int amountvalue) == false)
            {
                return BasicReply("Invaild amount");
            }
            if (amountvalue < 1)
            {
                return BasicReply("Invaild amount");
            }
            bot.GetClient.Self.GiveAvatarMoney(avataruuid, amountvalue);
            return BasicReply("Accepted");
        }

        [About("Makes the bot pay a object")]
        [ReturnHints("ok")]
        [ReturnHints("Primname is empty")]
        [ReturnHints("Current fund level")]
        [ReturnHints("Invaild object UUID")]
        [ReturnHints("Invaild amount")]
        [ReturnHints("Amount out of range")]
        [ReturnHints("Funds commands are disabled")]
        [ArgHints("object", "URLARG", "UUID of the object to pay")]
        [ArgHints("primname", "URLARG", "The name of the prim on the object to pay")]
        [ArgHints("amount", "URLARG", "the amount to pay (from 1 to current balance)")]
        [Route(HttpVerbs.Get, "/PayObject/{object}/{primname}/{amount}/{token}")]
        public object PayObject(string objectuuid,string primname,string amount,string token)
        {
            if (tokens.Allow(token, "funds", "PayObject", handleGetClientIP()) == false)
            {
                return Failure("Token not accepted");
            }
            if (bot.GetAllowFunds == false)
            {
                return Failure("Funds commands are disabled");
            }
            if (UUID.TryParse(objectuuid, out UUID objectUUID) == false)
            {
                return Failure("Invaild object UUID");
            }
            if (int.TryParse(amount, out int amountvalue) == false)
            {
                return Failure("Invaild amount");
            }
            if((amountvalue < 0) || (amountvalue > bot.GetClient.Self.Balance))
            {
                return Failure("Amount out of range");
            }
            if(helpers.notempty(primname) == false)
            {
                return Failure("Primname is empty");
            }
            bot.GetClient.Self.GiveObjectMoney(objectUUID, amountvalue, primname);
            return BasicReply("ok");
        }
    }
}
