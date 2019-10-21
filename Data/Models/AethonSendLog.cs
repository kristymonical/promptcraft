using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Aethon;
using Newtonsoft.Json;

namespace SVT.Platform.Data.Models
{
    public class AethonSendLog : BaseLogModel
    {
        [Key]
        public int AethonSendLogId { get; set; }

        [NotMapped]
        public AethonSendMultiDestination Log
        {
            get { return _Serialized == null ? null : JsonConvert.DeserializeObject<AethonSendMultiDestination>(_Serialized); }
            set { _Serialized = JsonConvert.SerializeObject(value); }
        }
    }

    public class AethonSendMultiDestination
    {
        public MultiDestinationResponse Response { get; set; }

        public MultiDestinationRequest Request { get; set; }
    }
}