using System;
using System.Collections.Generic;
using System.Text;

namespace CodeChallenge.DataObjects
{
    public class EntityWithTime
    {
        public Nullable<DateTime> Created { get; set; }
        public Nullable<DateTime> Updated { get; set; }
    }
}
