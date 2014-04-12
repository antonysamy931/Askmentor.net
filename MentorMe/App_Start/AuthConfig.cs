using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using MentorMe.Models;

namespace MentorMe
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "",
            //    appSecret: "");

            //OAuthWebSecurity.RegisterGoogleClient();

            //Google client
            OAuthWebSecurity.RegisterClient(new MentorMe.GoogleCustomClient(), displayName: "Google", extraData: null);

            //Google plus client
            OAuthWebSecurity.RegisterClient(new MentorMe.GooglePlusClient("", ""), displayName: "Google+", extraData: null);

            //Yahoo client
            OAuthWebSecurity.RegisterClient(new MentorMe.YahooCustomClient(), displayName: "Yahoo", extraData: null);

            //LinkedIn client
            OAuthWebSecurity.RegisterClient(new MentorMe.LinkedInCustomClient("", ""), displayName: "LinkedIn", extraData: null);

            //Facebook client
            OAuthWebSecurity.RegisterClient(new MentorMe.MyFacebookClient("", ""), displayName: "Facebook", extraData: null);
        }
    }
}
