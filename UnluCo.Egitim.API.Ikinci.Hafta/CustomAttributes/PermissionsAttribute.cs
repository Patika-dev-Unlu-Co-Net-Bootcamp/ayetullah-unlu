using System;

namespace UnluCo.Egitim.API.Ikinci.Hafta.CustomAttributes
{
    public class PermissionsAttribute: Attribute
    {
        public string Role { get; }

        public PermissionsAttribute(string role)
        {
            Role = role;
        }
    }
}
