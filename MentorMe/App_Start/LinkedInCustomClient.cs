using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using DotNetOpenAuth.OAuth.Messages;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;

namespace MentorMe
{
    public class LinkedInCustomClient : OAuthClient
    {
        
        public string userId { get; set; }
        public string userName { get; set; }

        private static XDocument LoadXDocumentFromStream(Stream stream)
        {
            var settings = new XmlReaderSettings
            {
                MaxCharactersInDocument = 65536L
            };
            return XDocument.Load(XmlReader.Create(stream, settings));
        }


        /// Describes the OAuth service provider endpoints for LinkedIn.
        private static readonly ServiceProviderDescription LinkedInServiceDescription =
                new ServiceProviderDescription
                {
                    AccessTokenEndpoint =
                            new MessageReceivingEndpoint("https://api.linkedin.com/uas/oauth/accessToken",
                            HttpDeliveryMethods.PostRequest),
                    RequestTokenEndpoint =
                            new MessageReceivingEndpoint("https://api.linkedin.com/uas/oauth/requestToken?scope=r_fullprofile+r_emailaddress",
                            HttpDeliveryMethods.PostRequest),
                    UserAuthorizationEndpoint =
                            new MessageReceivingEndpoint("https://www.linkedin.com/uas/oauth/authorize",
                            HttpDeliveryMethods.PostRequest),
                    TamperProtectionElements =
                            new ITamperProtectionChannelBindingElement[] { new HmacSha1SigningBindingElement() },
                    ProtocolVersion = ProtocolVersion.V10a
                };

        public LinkedInCustomClient(string consumerKey, string consumerSecret) :
            base("linkedIn", LinkedInServiceDescription, consumerKey, consumerSecret) { }

        /// Check if authentication succeeded after user is redirected back from the service provider.
        /// The response token returned from service provider authentication result. 
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "We don't care if the request fails.")]
        protected override AuthenticationResult VerifyAuthenticationCore(AuthorizedTokenResponse response)
        {
            // See here for Field Selectors API http://developer.linkedin.com/docs/DOC-1014
            const string profileRequestUrl =
                "https://api.linkedin.com/v1/people/~:(id,first-name,last-name,interests,headline,industry,summary,email-address,location:(name),picture-url,positions,associations,languages,honors,educations,date-of-birth,primary-twitter-account,three-current-positions,three-past-positions,group-memberships,specialties,skills)";


            string accessToken = response.AccessToken;
            string tokenSecret = (response as ITokenSecretContainingMessage).TokenSecret;
            //string Verifier = response.ExtraData.Values.First();


            var profileEndpoint =
                new MessageReceivingEndpoint(profileRequestUrl, HttpDeliveryMethods.GetRequest);
            HttpWebRequest request =
                WebWorker.PrepareAuthorizedRequest(profileEndpoint, accessToken);
            Dictionary<string, string> extraData = new Dictionary<string, string>();
            try
            {
                using (WebResponse profileResponse = request.GetResponse())
                {
                    using (Stream responseStream = profileResponse.GetResponseStream())
                    {
                        XDocument document = LoadXDocumentFromStream(responseStream);
                        userId = document.Root.Element("id").Value;
                        userName = document.Root.Element("email-address").Value;
                        extraData.Add("firstname", document.Root.Element("first-name").Value);
                        extraData.Add("lastname", document.Root.Element("last-name").Value);
                        extraData.Add("email", document.Root.Element("email-address").Value);
                        extraData.Add("accesstoken", accessToken);
                        return new AuthenticationResult(
                            isSuccessful: true,
                            provider: ProviderName,
                            providerUserId: userId,
                            userName: userName,
                            extraData: extraData);
                    }
                }
            }
            catch (Exception exception)
            {
                return new AuthenticationResult(exception);
            }
        }        
    }
}