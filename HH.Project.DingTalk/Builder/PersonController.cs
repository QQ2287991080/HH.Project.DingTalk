using DingTalk.Api.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.Project.DingTalk.Builder
{
  public  class PersonController
    {
        private OapiUserListResponse.UserlistDomain _user;
        private IDbConnection _conn;
        private string _code;
        public PersonController(OapiUserListResponse.UserlistDomain user, IDbConnection conn, string code)
        {
            _user = user;
            _conn = conn;
            _code = code;
        }
        public Person Construct(PersonBuilder builder)
        {
            builder.SelectPerson(_user, _conn, _code);
            Domain.OR_Person person = null;
            if (!builder.IsPerson())
            {
                person = builder.UpdatePerson(_conn);
            }
            else
            {
                person = builder.InsertPerson(_conn);
            }
            builder.SetRefDepartment(_conn, _code, person.Id);
            return builder.GetPerson();
        }
    }
}
