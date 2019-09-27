using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class DeliveryCommands
    {
        public static async Task<Delivery> PopDeliveryQueue(SVTContext context)
        {
            var top = await DeliveryCommands.GetDeliveryQueue(context).FirstAsync();

            top.NextPrioritizedDelivery.PreviousPrioritizedDeliveryId = null;

            return top;
        }
    }
}