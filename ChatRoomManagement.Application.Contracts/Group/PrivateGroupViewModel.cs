namespace ChatRoomManagement.Application.Contracts.Group
{
    public class PrivateGroupViewModel
    {
        public long Id { get; set; }
        public long OwnerId { get; set; }
        public long ReciverId { get; set; }
        public bool OwnerIsOnline{ get; set; }
        public bool ReciverIsOnline{ get; set; }
        public string OwnerLastSeenDate { get; set; }
        public string ReciverLastSeenDate { get; set; }
        public string OwnerUserName { get; set; }
        public string ReciverUserName { get; set; }
        public string OwnerPicture { get; set; }
        public string ReciverPicture { get; set; }



    }
}
