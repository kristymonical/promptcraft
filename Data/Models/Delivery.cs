using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SVT.Platform.Data.Models
{
    public class Delivery : BaseAuditModel
    {
        [Key]
        public int DeliveryId { get; set; }

        [StringLength(50)]
        [Required]
        public string CartId { get; set; }

        [StringLength(50)]
        public string OrderId { get; set; }

        public DateTime? Completed { get; set; }

        public DateTime? Canceled { get; set; }

        [StringLength(256)]
        [Required]
        public string UserId { get; set; }

        [StringLength(50)]
        [Required]
        public string DeliveryType { get; set; }

        [Required]
        public int DestinationAreaId { get; set; }

        public int? PreviousPrioritizedDeliveryId { get; set; }

        public bool Queued { get; set; }

        // NAVIGATION MEMBERS
        public virtual DeliveryType DeliveryTypeReference { get; set; }
        public virtual Area DestinationArea { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
        public virtual Delivery PreviousPrioritizedDelivery { get; set; }
        public virtual Delivery NextPrioritizedDelivery { get; set; }
        public virtual ICollection<Log> Logs { get; set; }

        public bool HasActiveJobs()
        {
            if (this.Jobs == null) return false;

            return this
                .Jobs
                .ToList()
                .TrueForAll(j => j.Completed == null
                    && j.Canceled == null
                    && j.Expired == null);
        }
    }
}