using BP_OnlineDOD.Server.Data;
using Microsoft.AspNetCore.Mvc;

namespace BP_OnlineDOD.Server.Controllers
{
    [Route("api/messages/upvotes")]
    public class UpvotesController : ControllerBase
    {
        private readonly IOnlineDOD _onlineDOD;

        public UpvotesController(IOnlineDOD onlineDOD)
        {
            _onlineDOD = onlineDOD;
        }

        [HttpPut("{id}")]
        public ActionResult UpvoteMessage(int id)
        {
            var messageModel = _onlineDOD.GetMessageById(id);
            if (messageModel == null)
            {
                return NotFound();
            }

            messageModel.ThumbsUpCount++;

            _onlineDOD.UpdateMessage(messageModel);
            _onlineDOD.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")] 
        public ActionResult CancelUpvote(int id)
        {
            var messageModel = _onlineDOD.GetMessageById(id);
            if (messageModel == null)
            {
                return NotFound();
            }

            messageModel.ThumbsUpCount--;

            _onlineDOD.UpdateMessage(messageModel);
            _onlineDOD.SaveChanges();

            return NoContent();
        }
    }
}
