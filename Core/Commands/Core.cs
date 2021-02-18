﻿using BetterSecondBotShared.Json;
using BetterSecondBotShared.Static;
using BetterSecondBot.bottypes;
using OpenMetaverse;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading;

namespace BetterSecondBot.HttpService
{
    public class HTTP_Core : WebApiControllerWithTokens
    {
        public HTTP_Core(SecondBot mainbot, TokenStorage setuptokens) : base(mainbot, setuptokens) { }

        [About("Requests a new token (Vaild for 10 mins) <br/>to use with all other requests")]
        [NeedsToken(false)]
        [ReturnHints("A new token with full system scope")]
        [ArgHints("authcode", "text", "the first 10 chars of SHA1(unixtime+WebUIkey)<br/>unixtime can be +- 30 of the bots time.")]
        [ArgHints("unixtimegiven", "number", "the unixtime you made this request")]
        [Route(HttpVerbs.Post, "/GetToken")]
        public Object GetToken([FormField] string authcode, [FormField] string unixtimegiven)
        {
            long now = helpers.UnixTimeNow();
            long dif = 0;
            if(long.TryParse(unixtimegiven,out long given) == true)
            {
                dif = now - given;
                now = given;
            }
            if((dif < 30) && (dif > -30))
            {
                var raw = now.ToString() + bot.getMyConfig.Security_WebUIKey;
                string hash = helpers.GetSHA1(raw).Substring(0, 10);
                if (hash == authcode)
                {
                    string newtoken = tokens.CreateToken(handleGetClientIP());
                    if(newtoken != null) return BasicReply(newtoken);
                }
            }
            return Failure("Authcode not accepted");
        }

        [About("Used to check HTTP connections")]
        [ReturnHints("world")]
        [NeedsToken(false)]
        [Route(HttpVerbs.Get, "/Hello")]
        public object Hello()
        {
            return BasicReply("world");
        }

        [About("Delays a thead by X ms<br/>Mostly pointless but good if your doing custom commands")]
        [ReturnHints("Invaild amount")]
        [ReturnHints("ok")]
        [Route(HttpVerbs.Get, "/Delay/{amount}/{token}")]
        public object Delay(string amount,string token)
        {
            if (tokens.Allow(token, "core", "Delay", handleGetClientIP()) == false)
            {
                return Failure("Token not accepted");
            }
            if(int.TryParse(amount,out int amountvalue) == false)
            {
                return Failure("Invaild amount");
            }
            Thread.Sleep(amountvalue);
            return BasicReply("Ok");
        }

        [About("Removes the given token from the accepted token pool")]
        [ReturnHints("Failed to remove token")]
        [ReturnHints("ok")]
        [Route(HttpVerbs.Get, "/LogoutUI/{token}")]
        public object LogoutUI(string token)
        {
            if (tokens.Allow(token, "core", "LogoutUI", handleGetClientIP()) == false)
            {
                return Failure("Token not accepted");
            }
            return tokens.Expire(token);
        }
    }

    public class CommandLibCall
    {
        public string Command { get; set; }
        public string[] Args { get; set; }
        public string AuthCode { get; set; }
    }

    public class NearMeDetails
    {
        public string id { get; set; }
        public string name { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int range { get; set; }

    }


}
