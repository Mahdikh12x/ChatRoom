namespace ChatRoomManagement.Application.Contracts.Group
{
    public class JoinPrivateGroup
    {
        public long OwnerId { get; set; }
        public long ReciverId { get; set; }
        public long GroupId { get; set; }
    }
}
