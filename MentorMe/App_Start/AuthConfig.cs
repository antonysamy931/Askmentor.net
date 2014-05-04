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
            OAuthWebSecurity.RegisterClient(new MentorMe.GooglePlusClient("269446698098-7fd2hqiruibevrdeq7kdsv1sck44iivl.apps.googleusercontent.com", "OtyS_RCqnp2DiGMb3tBfC1kE"), displayName: "Google+", extraData: null);

            //Yahoo client
            OAuthWebSecurity.RegisterClient(new MentorMe.YahooCustomClient(), displayName: "Yahoo", extraData: null);

            //LinkedIn client
            OAuthWebSecurity.RegisterClient(new MentorMe.LinkedInCustomClient("75st8osu5xyi7s", "xCkejI67Hzx5rsVJ"), displayName: "LinkedIn", extraData: null);

            //Facebook client
            OAuthWebSecurity.RegisterClient(new MentorMe.MyFacebookClient("689806004399233", "e229eba44963e81944b14755ad46d5dt3"), displayName: "Facebook", extraData: null);

            OAuthWebSecurity.RegisterTwitterClient(consumerKey: "nQ8Com0ApTbJ4mltGe7jG98Rf", consumerSecret: "ZFM1W071ZfaT7QSkPhrJiEZMhF0Hg0thf8dv7OTF40IesHaZ25");
        }
    }
}
