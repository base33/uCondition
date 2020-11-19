using System;
using System.Net;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using uCondition.Interfaces;
using uCondition.Models;

namespace uCondition.Predicates.Members
{
    public class IpRestrictionPredicate : Predicate
    {
        public IpRestrictionPredicate()
        {
            Name = "Is request from a set of allowed IP ranges?";
            Alias = "uCondition.IPRestriction";
            Icon = "icon-ethernet";
            Category = "Canada Life Security";
            Fields = new List<EditableProperty>
            {
                new EditableProperty("IP Ranges Allowed", "allowedIpRanges", "Textarea")
            };
        }

        public override bool Validate(IFieldValues fieldValues)
        {
            var ipRangesString = fieldValues.GetValue<string>("allowedIpRanges");
            var userIpAddress = HttpContext.Current.Request.UserHostAddress;

            string[] allowedIpRanges = ipRangesString.Split(
                new[] { 
                    Environment.NewLine,
                    "\r",
                    "\n"
                }, 
                StringSplitOptions.RemoveEmptyEntries);

            return allowedIpRanges.Any(range => IsInRange(userIpAddress, range));
        }

        /// <summary>
        /// Helper method to validate (without dependencies) if IP is in passed range.
        /// </summary>
        /// <param name="ipAddress">IP address (v4)</param>
        /// <param name="CIDRmask">CIDR range e.g. 10.0.0.0/8</param>
        /// <returns></returns>
        private bool IsInRange(string ipAddress, string CIDRmask)
        {
            string[] parts = CIDRmask.Split('/');

            int IP_addr = BitConverter.ToInt32(IPAddress.Parse(parts[0]).GetAddressBytes(), 0);
            int CIDR_addr = BitConverter.ToInt32(IPAddress.Parse(ipAddress).GetAddressBytes(), 0);
            int CIDR_mask = IPAddress.HostToNetworkOrder(-1 << (32 - int.Parse(parts[1])));

            return ((IP_addr & CIDR_mask) == (CIDR_addr & CIDR_mask));
        }
    }
}