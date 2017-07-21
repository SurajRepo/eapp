using System;
using System.Collections.Generic;

namespace VPortal.TokenManager
{
      public class Token
      {
        private DateTimeOffset _createdOn;
        private DateTimeOffset _expiresOn;
        private string _issuedBy;
        private string _subject;
        private int _userId;

        public Dictionary<string, object> Header { get; }
        public Dictionary<string, string> Claims { get; }
        public string ClientToken { get; set; }

        public Token()
        {
            this.Header = new Dictionary<string, object>();
            this.Claims = new Dictionary<string, string>();
        }
        public void AddHeader(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            this.Header.Add(key, value);
        }

        public void AddClaim(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            this.Claims.Add(key, value.ToString());
        }
        public string Subject
        {
            get { return this._subject; }
            set
            {
                this._subject = value;
                this.AddClaim("sub", value);
            }
        }

        public string IssuedBy
        {
            get { return this._issuedBy; }
            set
            {
                this._issuedBy = value;
                this.AddClaim("iss", value);
            }
        }
        public DateTimeOffset CreatedOn
        {
            get { return this._createdOn; }
            set
            {
                this._createdOn = value;
                this.AddClaim("iat", value.Ticks);
            }
        }

        public DateTimeOffset ExpiresOn
        {
            get { return this._expiresOn; }
            set
            {
                this._expiresOn = value;
                this.AddClaim("exp", value.Ticks);
            }
        }
        public int UserId
        {
            get
            {
                return this._userId;
            }
            set
            {
                this._userId = value;
                this.AddClaim("userid", value);
            }
        }
}
      
       
}
