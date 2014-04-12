using System.Collections.Generic;
using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;

namespace MentorMe
{
    public class YahooCustomClient:OpenIdClient
    {
        public YahooCustomClient()
            : base("yahoo", WellKnownProviders.Yahoo)
        {
        }

        #region Methods
        /// <summary>
        /// Gets the extra data obtained from the response message when authentication is successful.
        /// </summary>
        /// <param name="response">
        /// The response message. 
        /// </param>
        /// <returns>A dictionary of profile data; or null if no data is available.</returns>
        protected override Dictionary<string, string> GetExtraData(IAuthenticationResponse response)
        {
            FetchResponse fetchResponse = response.GetExtension<FetchResponse>();
            if (fetchResponse != null)
            {
                var extraData = new Dictionary<string, string>();
                extraData.Add("email", fetchResponse.GetAttributeValue(WellKnownAttributes.Contact.Email));
                extraData.Add("country", fetchResponse.GetAttributeValue(WellKnownAttributes.Contact.HomeAddress.Country));
                extraData.Add("firstName", fetchResponse.GetAttributeValue(WellKnownAttributes.Name.First));
                extraData.Add("lastName", fetchResponse.GetAttributeValue(WellKnownAttributes.Name.Last));
                //extraData.Add("city", fetchResponse.GetAttributeValue(WellKnownAttributes.Contact.HomeAddress.City));
                //extraData.Add("postalcode", fetchResponse.GetAttributeValue(WellKnownAttributes.Contact.HomeAddress.PostalCode));
                //extraData.Add("state", fetchResponse.GetAttributeValue(WellKnownAttributes.Contact.HomeAddress.State));
                //extraData.Add("addressone", fetchResponse.GetAttributeValue(WellKnownAttributes.Contact.HomeAddress.StreetAddressLine1));
                //extraData.Add("addresstwo", fetchResponse.GetAttributeValue(WellKnownAttributes.Contact.HomeAddress.StreetAddressLine2));
                //extraData.Add("birthdate", fetchResponse.GetAttributeValue(WellKnownAttributes.BirthDate.WholeBirthDate));
                //extraData.Add("companyname", fetchResponse.GetAttributeValue(WellKnownAttributes.Company.CompanyName));
                //extraData.Add("jobtitle", fetchResponse.GetAttributeValue(WellKnownAttributes.Company.JobTitle));
                //extraData.Add("gender", fetchResponse.GetAttributeValue(WellKnownAttributes.Person.Gender));
                //extraData.Add("fullname", fetchResponse.GetAttributeValue(WellKnownAttributes.Name.FullName));
                //extraData.Add("mobile", fetchResponse.GetAttributeValue(WellKnownAttributes.Contact.Phone.Mobile));
                //extraData.Add("homepage", fetchResponse.GetAttributeValue(WellKnownAttributes.Contact.Web.Homepage));
                return extraData;
            }
            return null;
        }

        /// <summary>
        /// Called just before the authentication request is sent to service provider.
        /// </summary>
        /// <param name="request">
        /// The request. 
        /// </param>
        protected override void OnBeforeSendingAuthenticationRequest(IAuthenticationRequest request)
        {
            // Attribute Exchange extensions
            var fetchRequest = new FetchRequest();
            fetchRequest.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
            fetchRequest.Attributes.AddRequired(WellKnownAttributes.Contact.HomeAddress.Country);
            fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.First);
            fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.Last);
            //fetchRequest.Attributes.AddRequired(WellKnownAttributes.Contact.HomeAddress.City);
            //fetchRequest.Attributes.AddRequired(WellKnownAttributes.Contact.HomeAddress.PostalCode);
            //fetchRequest.Attributes.AddRequired(WellKnownAttributes.Contact.HomeAddress.State);
            //fetchRequest.Attributes.AddRequired(WellKnownAttributes.Contact.HomeAddress.StreetAddressLine1);
            //fetchRequest.Attributes.AddRequired(WellKnownAttributes.Contact.HomeAddress.StreetAddressLine2);
            //fetchRequest.Attributes.AddRequired(WellKnownAttributes.BirthDate.WholeBirthDate);
            //fetchRequest.Attributes.AddRequired(WellKnownAttributes.Company.CompanyName);
            //fetchRequest.Attributes.AddRequired(WellKnownAttributes.Company.JobTitle);
            //fetchRequest.Attributes.AddRequired(WellKnownAttributes.Person.Gender);
            //fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.FullName);
            //fetchRequest.Attributes.AddRequired(WellKnownAttributes.Contact.Phone.Mobile);
            //fetchRequest.Attributes.AddRequired(WellKnownAttributes.Contact.Web.Homepage);
            request.AddExtension(fetchRequest);

        }
        #endregion
    }
}