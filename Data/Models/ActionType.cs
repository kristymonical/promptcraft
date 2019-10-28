using System.Collections.Generic;

namespace SVT.Platform.Data.Models
{
    public class ActionType : BaseReferenceModel
    {
        // NAVIGATION MEMBERS
        public virtual ICollection<Log> Logs { get; set; }
    }
}