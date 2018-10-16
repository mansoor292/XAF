using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace Solution1.Module.DatabaseUpdate
{
    [DefaultClassOptions]
    public class NewOrg : Organization
    {
        public NewOrg(Session session) : base(session)
        {

        }


    }
}
